// -----------------------------------------------------------------------
// <copyright file="EmptyStringModelBinder.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Ares.Core.Web
{
    using System.Web.Mvc;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EmptyStringModelBinder : DefaultModelBinder
    {
        /// <summary>
        /// Biding model.
        /// </summary>
        /// <param name="controllerContext">Controller context.</param>
        /// <param name="bindingContext">Biding context.</param>
        /// <returns>Return result.</returns>
        public override object BindModel(
                    ControllerContext controllerContext,
                    ModelBindingContext bindingContext)
        {
            bindingContext.ModelMetadata.ConvertEmptyStringToNull = false;
            return base.BindModel(controllerContext, bindingContext);
        }
    }
}
