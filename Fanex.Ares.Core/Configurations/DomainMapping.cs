namespace Fanex.Ares.Core.Configurations
{
    using System;
    using System.Configuration;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    /// 
    public class DomainMapping : ConfigurationSection
    {
        [ConfigurationProperty("Domain", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(DomainCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public DomainCollection Domain
        {
            get
            {
                var sportsCollection =
                    (DomainCollection)base["Domain"];
                return sportsCollection;
            }
        }

        public static DomainMapping Instance
        {
            get
            {
                var spSection =
                 ConfigurationManager.GetSection("DomainMapping") as DomainMapping;
                return spSection;
            }
        }
    }

    public class DomainCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public void Add(DomainElement DomainElement)
        {
            BaseAdd(DomainElement);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(DomainElement DomainElement)
        {
            if (BaseIndexOf(DomainElement) >= 0)
                BaseRemove(DomainElement.Name);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string alias)
        {
            BaseRemove(alias);
        }

        public void Clear()
        {
            BaseClear();
        }

        public DomainElement this[int index]
        {
            get
            {
                return (DomainElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public new DomainElement this[string name]
        {
            get
            {
                return (DomainElement)BaseGet(name);
            }
        }

        public int IndexOf(DomainElement DomainElement)
        {
            return BaseIndexOf(DomainElement);
        }

        protected override sealed ConfigurationElement CreateNewElement()
        {
            return new DomainElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DomainElement)element).Name;
        }
    }

    public class DomainElement : ConfigurationElement
    {        
        [ConfigurationProperty("name", DefaultValue = "NA", IsRequired = true, IsKey = false)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("currentdomain", DefaultValue = "", IsRequired = true, IsKey = true)]
        public string CurrentDomain
        {
            get
            {
                return (string)this["currentdomain"];
            }
            set
            {
                this["currentdomain"] = value;
            }
        }

        [ConfigurationProperty("converttodomain", DefaultValue = "", IsRequired = true, IsKey = true)]
        public string ConvertToDomain
        {
            get
            {
                return (string)this["converttodomain"];
            }
            set
            {
                this["converttodomain"] = value;
            }
        }
    }
}
