var invoiceApp = invoiceApp || {};
invoiceApp.filter = {};
(function () {
    var filterControls = {
        ddlOperator: $('#ddlOperator'),
        ddlLicensee: $('#ddlLicensee'),
        cldFromDate: null,
        cldToDate: null,
        divInvoiceType: $('#divInvoiceType'),
        ddlCurrency: $('#CurrencyID'),
        txtInvoiceNo: $('#invoiceNo'),
        cldDueDate: null,
        btnGenerate: $('#btnGenerate'),
        btnNewInvoice: $('#btnNewInvoice')
    }
}).apply(invoiceApp.filter);