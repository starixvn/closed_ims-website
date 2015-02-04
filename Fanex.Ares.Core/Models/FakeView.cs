namespace Fanex.Ares.Core.Models
{
    using System;
    using System.IO;
    using System.Web.Mvc;

    /// <summary>
    /// Fake View.
    /// </summary>
    public class FakeView : IView
    {
        public void Render(ViewContext viewContext, TextWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
