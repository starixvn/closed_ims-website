namespace Fanex.Ares.Core.Configurations
{
    using System;
    using System.Configuration;

    public class ControllerMapping : ConfigurationSection
    {
        [ConfigurationProperty("ControllerSetting", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(ControllerSettingCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public ControllerSettingCollection ControllerSetting
        {
            get
            {
                var sportsCollection =
                    (ControllerSettingCollection)base["ControllerSetting"];
                return sportsCollection;
            }
        }

        public static ControllerMapping Instance
        {
            get
            {
                var spSection =
                 ConfigurationManager.GetSection("ControllerMapping") as ControllerMapping;
                return spSection;
            }
        }
    }

    public class ControllerSettingCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public void Add(ControllerSettingElement ReportUrlElement)
        {
            BaseAdd(ReportUrlElement);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(ControllerSettingElement ReportUrlElement)
        {
            if (BaseIndexOf(ReportUrlElement) >= 0)
                BaseRemove(ReportUrlElement.Name);
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

        public ControllerSettingElement this[int index]
        {
            get
            {
                return (ControllerSettingElement)BaseGet(index);
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

        public new ControllerSettingElement this[string name]
        {
            get
            {
                return (ControllerSettingElement)BaseGet(name);
            }
        }

        public int IndexOf(ControllerSettingElement ReportUrlElement)
        {
            return BaseIndexOf(ReportUrlElement);
        }

        protected override sealed ConfigurationElement CreateNewElement()
        {
            return new ControllerSettingElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ControllerSettingElement)element).Name;
        }
    }

    public class ControllerSettingElement : ConfigurationElement
    {
        [ConfigurationProperty("name", DefaultValue = "NA", IsRequired = true, IsKey = false)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("active", DefaultValue = true, IsRequired = true, IsKey = true)]
        public bool Active
        {
            get
            {
                return (bool)this["active"];
            }
            set
            {
                this["active"] = value;
            }
        }

        [ConfigurationProperty("controllername", DefaultValue = "", IsRequired = true, IsKey = true)]
        public String ControllerName
        {
            get
            {
                var controllerName = (String)this["controllername"];
                return string.IsNullOrWhiteSpace(controllerName) ? string.Empty : controllerName.Trim();
            }
            set
            {
                this["controllername"] = value;
            }
        }

        [ConfigurationProperty("url", DefaultValue = "", IsRequired = true, IsKey = true)]
        public String Url
        {
            get
            {
                var url = (String)this["url"];
                return string.IsNullOrWhiteSpace(url) ? string.Empty : url.Trim();
            }
            set
            {
                this["url"] = value;
            }
        }
    }
}
