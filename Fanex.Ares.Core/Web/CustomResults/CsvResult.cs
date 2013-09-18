// --------------------------------------------------------------------------
// <copyright file="CsvResult.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Ares.Core.Web.CustomResults
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using ExcelUtil.Spreadsheet;

    /// <summary>
    /// C.s.v Result.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewd.")]
    public class CsvResult : SpreadSheetResult
    {
        public CsvResult(ExcelFile file, string fileName)
            : base(file, fileName)
        {
        }

        protected override string ContentType
        {
            get { return "application/csv"; }
        }

        protected override string Extension
        {
            get { return ".csv"; }
        }

        protected override Action<ExcelFile, Stream> SaveAction
        {
            get { return (f, s) => f.SaveCsv(s, CsvType.CommaDelimited); }
        }
    }
}
