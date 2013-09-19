// --------------------------------------------------------------------------
// <copyright file="ControllerMapping.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Ares.Core.Configurations
{
    using System.Configuration;

    /// <summary>
    /// Controller mapping.
    /// </summary>
    public class ControllerMapping : ConfigurationSection
    {
        public static ControllerMapping Instance
        {
            get
            {
                var section =
                 ConfigurationManager.GetSection("ControllerMapping") as ControllerMapping;
                return section;
            }
        }

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
    }

    /// <summary>
    /// Controller setting collection.
    /// </summary>
    public class ControllerSettingCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        /// <summary>
        /// Controller setting element.
        /// </summary>
        /// <param name="index">Element index.</param>
        /// <returns>Return value.</returns>
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

        /// <summary>
        /// Setting element.
        /// </summary>
        /// <param name="name">Setting name.</param>
        /// <returns>Return value.</returns>
        public new ControllerSettingElement this[string name]
        {
            get
            {
                return (ControllerSettingElement)BaseGet(name);
            }
        }

        /// <summary>
        /// Adding setting.
        /// </summary>
        /// <param name="reportUrlElement">Report url element.</param>
        public void Add(ControllerSettingElement reportUrlElement)
        {
            BaseAdd(reportUrlElement);
        }

        /// <summary>
        /// Removing element.
        /// </summary>
        /// <param name="reportUrlElement">Report url element.</param>
        public void Remove(ControllerSettingElement reportUrlElement)
        {
            if (BaseIndexOf(reportUrlElement) >= 0)
            {
                BaseRemove(reportUrlElement.Name);
            }   
        }

        /// <summary>
        /// Removing element.
        /// </summary>
        /// <param name="index">Element index.</param>
        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        /// <summary>
        /// Removing element.
        /// </summary>
        /// <param name="alias">Element alias.</param>
        public void Remove(string alias)
        {
            BaseRemove(alias);
        }

        /// <summary>
        /// Clear setting.
        /// </summary>
        public void Clear()
        {
            BaseClear();
        }

        /// <summary>
        /// Index of setting.
        /// </summary>
        /// <param name="reportUrlElement">Report url element.</param>
        /// <returns>Return value.</returns>
        public int IndexOf(ControllerSettingElement reportUrlElement)
        {
            return BaseIndexOf(reportUrlElement);
        }

        /// <summary>
        /// The base adding configuration element.
        /// </summary>
        /// <param name="element">Configuration element.</param>
        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        /// <summary>
        /// Creating new element.
        /// </summary>
        /// <returns>Return result.</returns>
        protected override sealed ConfigurationElement CreateNewElement()
        {
            return new ControllerSettingElement();
        }

        /// <summary>
        /// Get element key.
        /// </summary>
        /// <param name="element">Configuration element.</param>
        /// <returns>Return value.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ControllerSettingElement)element).Name;
        }
    }

    /// <summary>
    /// Controller setting element.
    /// </summary>
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
        public string ControllerName
        {
            get
            {
                var controllerName = (string)this["controllername"];
                return string.IsNullOrWhiteSpace(controllerName) ? string.Empty : controllerName.Trim();
            }

            set
            {
                this["controllername"] = value;
            }
        }

        [ConfigurationProperty("url", DefaultValue = "", IsRequired = true, IsKey = true)]
        public string Url
        {
            get
            {
                var url = (string)this["url"];
                return string.IsNullOrWhiteSpace(url) ? string.Empty : url.Trim();
            }

            set
            {
                this["url"] = value;
            }
        }
    }
}