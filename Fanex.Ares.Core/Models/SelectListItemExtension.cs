namespace Fanex.Ares.Core.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    /// <summary>
    /// Select List Item Extension.
    /// </summary>
    public class SelectListItemExtension
    {
        public SelectListItemExtension()
        {
            Item = new SelectListItem();
            Extension = new Dictionary<string, object>();
        }

        public SelectListItem Item { get; set; }

        public Dictionary<string, object> Extension { get; set; }
    }
}
