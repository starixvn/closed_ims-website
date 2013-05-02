using System.Configuration;

namespace Fanex.Core.PdfHelper.Configuration
{
    public class PdfHelperSection : ConfigurationSection
    {
        public enum ElementType
        {
            StyleSheet
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
                var spCollection =
                    (PdfHelperCollection)base["StyleSheet"];
                return spCollection;
            }
        }


      

        public static PdfHelperSection Instance
        {
            get
            {
                var spSection =
                 ConfigurationManager.GetSection("PdfHelper") as PdfHelperSection;
                return spSection;
            }
        }


        public PdfHelperCollection this[ElementType name]
        {
            get
            {

                var spCollection = (PdfHelperCollection)this.GetType().GetProperty(name.ToString()).GetValue(this, System.Reflection.BindingFlags.GetField, null, null, null);
                return spCollection;
            }
        }


    }

    public class PdfHelperCollection : ConfigurationElementCollection
    {

        public PdfHelperCollection()
        {
            //var storeProcedure = (HelperElement) CreateNewElement();
            //Add(storeProcedure);
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public void Add(PdfHelperElement HelperElement)
        {
            BaseAdd(HelperElement);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(PdfHelperElement HelperElement)
        {
            if (BaseIndexOf(HelperElement) >= 0)
                BaseRemove(HelperElement.Name);
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

        new public PdfHelperElement this[string Name]
        {
            get
            {
                return (PdfHelperElement)BaseGet(Name);
            }
        }

        public int IndexOf(PdfHelperElement HelperElement)
        {
            return BaseIndexOf(HelperElement);
        }


        protected override sealed ConfigurationElement CreateNewElement()
        {
            return new PdfHelperElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PdfHelperElement)element).Name;
        }
    }

    public class PdfHelperElement : ConfigurationElement
    {



        public PdfHelperElement()
        {

        }

       

        [ConfigurationProperty("name", IsRequired = true)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
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
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
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
