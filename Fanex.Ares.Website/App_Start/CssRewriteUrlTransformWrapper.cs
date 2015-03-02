namespace Fanex.Ares.Website
{
    using System.Web;
    using System.Web.Optimization;

    /// <summary>
    /// Route Config.
    /// </summary>
    public class CssRewriteUrlTransformWrapper : IItemTransform
    {
        public string Process(string includedVirtualPath, string input)
        {
            return new CssRewriteUrlTransform().Process("~" + VirtualPathUtility.ToAbsolute(includedVirtualPath), input);
        }
    }
}