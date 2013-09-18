// -----------------------------------------------------------------------
// <copyright file="PdfHelperSection.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Core.PdfHelper.Configuration
{
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Helper section.
    /// </summary>
    public class PdfHelperSection : ConfigurationSection
    {
        public enum ElementType
        {
            /// <summary>
            /// Style sheet.
            /// </summary>
            StyleSheet
        }

        public static PdfHelperSection Instance
        {
            get
            {
                var section = ConfigurationManager.GetSection("PdfHelper") as PdfHelperSection;
                return section;
            }
        }
               
        [ConfigurationProperty("StyleSheet", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(PdfHelperCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public PdfHelperCollection StyleSheet
        {
            get
            {
                var collection = (PdfHelperCollection)base["StyleSheet"];
                return collection;
            }
        }    

        public PdfHelperCollection this[ElementType name]
        {
            get
            {
                var collection = (PdfHelperCollection)this.GetType().GetProperty(name.ToString()).GetValue(this, System.Reflection.BindingFlags.GetField, null, null, null);
                return collection;
            }
        }
    }

    /// <summary>
    /// Helper collection.
    /// </summary>
    [SuppressMessage(
        "StyleCop.CSharp.MaintainabilityRules", 
        "SA1402:FileMayOnlyContainASingleClass",
        Justification = "Reviewed. Suppression is OK here.")]
    public class PdfHelperCollection : ConfigurationElementCollection
    {
        public PdfHelperCollection()
        {
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }
        
        public PdfHelperElement this[int index]
        {
            get
            {
                return (PdfHelperElement)BaseGet(index);
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

        public new PdfHelperElement this[string name]
        {
            get
            {
                return (PdfHelperElement)BaseGet(name);
            }
        }

        /// <summary>
        /// Add element.
        /// </summary>
        /// <param name="helperElement">Helper element.</param>
        public void Add(PdfHelperElement helperElement)
        {
            BaseAdd(helperElement);
        }

        /// <summary>
        /// Remove element.
        /// </summary>
        /// <param name="helperElement">Helper element.</param>
        public void Remove(PdfHelperElement helperElement)
        {
            if (BaseIndexOf(helperElement) >= 0)
            {
                BaseRemove(helperElement.Name);
            }
        }
                
        /// <summary>
        /// Remove element.
        /// </summary>
        /// <param name="index">Remove index.</param>
        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        /// <summary>
        /// Remove alias.
        /// </summary>
        /// <param name="alias">Alias name.</param>
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
        /// <param name="helperElement">Helper element.</param>
        /// <returns>Return value.</returns>
        public int IndexOf(PdfHelperElement helperElement)
        {
            return BaseIndexOf(helperElement);
        }

        /// <summary>
        /// Create new element.
        /// </summary>
        /// <returns>Return value.</returns>
        protected override sealed ConfigurationElement CreateNewElement()
        {
            return new PdfHelperElement();
        }

        /// <summary>
        /// Get element key.
        /// </summary>
        /// <param name="element">Element key.</param>
        /// <returns>Return value.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PdfHelperElement)element).Name;
        }
        
        /// <summary>
        /// Base add function.
        /// </summary>
        /// <param name="element">Configuration element.</param>
        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }
    }

    /// <summary>
    /// Helper element.
    /// </summary>
    public class PdfHelperElement : ConfigurationElement
    {
        public PdfHelperElement()
        {
        }

        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }

            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("value", IsRequired = false)]
        public string Value
        {
            get
            {
                return (string)this["value"];
            }

            set
            {
                this["value"] = value;
            }
        }

        [ConfigurationProperty("items", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(PdfHelperCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public PdfHelperCollection Items
        {
            get
            {
                var items =
                    (PdfHelperCollection)base["items"];
                return items;
            }
        }
    }
}
