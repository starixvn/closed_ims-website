// --------------------------------------------------------------------------
// <copyright file="WorkBookExtension.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Ares.Core.Extensions
{
    /// <summary>
    /// Excel, Word, PDF... extension.
    /// </summary>
    public class WorkBookExtension
    {
        public WorkBookExtension()
        {
        }

        public enum DocumentType
        {
            /// <summary>
            /// The Excel type.
            /// </summary>
            Xlsx, 
            
            /// <summary>
            /// The P.d.f type.
            /// </summary>
            Pdf
        }

        public enum PageOrientation
        {
            /// <summary>
            /// Landscape view.
            /// </summary>
            Landscape, 
            
            /// <summary>
            /// Portrait view.
            /// </summary>
            Portrait
        }
    }
}
