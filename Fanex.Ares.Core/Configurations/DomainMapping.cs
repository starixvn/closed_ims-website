// --------------------------------------------------------------------------
// <copyright file="DomainMapping.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Ares.Core.Configurations
{
    using System;
    using System.Configuration;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DomainMapping : ConfigurationSection
    {
        public static DomainMapping Instance
        {
            get
            {
                var section =
                 ConfigurationManager.GetSection("DomainMapping") as DomainMapping;
                return section;
            }
        }

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
    }

    /// <summary>
    /// Domain collection.
    /// </summary>
    public class DomainCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        /// <summary>
        /// Domain element.
        /// </summary>
        /// <param name="index">Element index.</param>
        /// <returns>Return value.</returns>
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

        /// <summary>
        /// Domain element.
        /// </summary>
        /// <param name="name">Element name.</param>
        /// <returns>Return value.</returns>
        public new DomainElement this[string name]
        {
            get
            {
                return (DomainElement)BaseGet(name);
            }
        }

        /// <summary>
        /// Adding element.
        /// </summary>
        /// <param name="domainElement">Domain element.</param>
        public void Add(DomainElement domainElement)
        {
            BaseAdd(domainElement);
        }

        /// <summary>
        /// Removing element.
        /// </summary>
        /// <param name="domainElement">Domain element.</param>
        public void Remove(DomainElement domainElement)
        {
            if (BaseIndexOf(domainElement) >= 0)
            {
                BaseRemove(domainElement.Name);
            }   
        }

        /// <summary>
        /// Remove element.
        /// </summary>
        /// <param name="index">Element index.</param>
        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        /// <summary>
        /// Remove element.
        /// </summary>
        /// <param name="alias">Element alias.</param>
        public void Remove(string alias)
        {
            BaseRemove(alias);
        }

        /// <summary>
        /// Clear element.
        /// </summary>
        public void Clear()
        {
            BaseClear();
        }
        
        /// <summary>
        /// Index of element.
        /// </summary>
        /// <param name="domainElement">Domain element.</param>
        /// <returns>Return value.</returns>
        public int IndexOf(DomainElement domainElement)
        {
            return BaseIndexOf(domainElement);
        }

        /// <summary>
        /// Create new element.
        /// </summary>
        /// <returns>Return value.</returns>
        protected override sealed ConfigurationElement CreateNewElement()
        {
            return new DomainElement();
        }

        /// <summary>
        /// Get element key.
        /// </summary>
        /// <param name="element">Configuration element.</param>
        /// <returns>Return value.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DomainElement)element).Name;
        }

        /// <summary>
        /// Base adding.
        /// </summary>
        /// <param name="element">Configuration element.</param>
        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }
    }

    /// <summary>
    /// Domain element.
    /// </summary>
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
