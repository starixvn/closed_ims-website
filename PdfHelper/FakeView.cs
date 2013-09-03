// -----------------------------------------------------------------------
// <copyright file="FakeView.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Core.PdfHelper
{
    using System;
    using System.IO;
    using System.Web.Mvc;

    /// <summary>
    /// Fake View.
    /// </summary>
    public class FakeView : IView
    {
        #region IView Members
        /// <summary>
        /// Rendering view.
        /// </summary>
        /// <param name="viewContext">View context.</param>
        /// <param name="writer">Text writer.</param>
        public void Render(ViewContext viewContext, TextWriter writer)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
