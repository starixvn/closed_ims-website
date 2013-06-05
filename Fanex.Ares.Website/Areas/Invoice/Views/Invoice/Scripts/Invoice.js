$(document).ready(function () {
    domain = location.protocol + '//' + location.host + $('#NapHubRoot').val();
    //------binding events
    $('#fromTopickerplugin').nexDatePicker({
        nexDatePickerType: "quickchoose",
        datePickerFormat: {
            changeMonth: true,
            dateFormat: "M dd, yy"
        },
        firstDatePicker: {
            Id: "FromDatePicker",
            Label: labelFrom,
            InitDate: _today,
            LabelCss: "lbl-field col_3",
            TextboxCss: "edit-field col_6"
        },
        secondDatePicker: {
            Id: "ToDatePicker",
            Label: labelTo,
            InitDate: _today,
            LabelCss: "lbl-field col_1",
            TextboxCss: "edit-field col_6"
        },
        onSelectDatePicker: function () {
        },
        dropDownList:
        {
            Id: "dropDownListId1",
            Label: "Date ",
            LabelCss: "lbl-field col_3",
            OptionCss: "edit-field col_5"
        }
    });
    $('#duteDatepickerplugin').nexDatePicker({
        nexDatePickerType: "single",
        datePickerFormat: {
            changeMonth: true,
            dateFormat: "M dd, yy"
        },
        firstDatePicker: {
            Id: "DueDatePicker",
            Label: "Due Date",
            InitDate: "",
            LabelCss: "lbl-field col_3",
            TextboxCss: "edit-field col_8"
        },
        onSelectDatePicker: function () {
        }
    });
    $('#btnGenerate').button();
    $('#btnGenerate').click(function () { if (!$(this).hasClass('ui-state-disabled')) Submit(); });

    $("#ddlLicensee").change(function () {
        GetInvoiceType();
    });

    $("#ddlOperator").change(function () {
        CreatingInvoice.GetLicensees();
    });

    //------binding data
    $.each(operatorListModel, function (i, item) {
        $('#ddlOperator').append('<option value="'
                                 + item.PartyID
                                 + '">'
                                 + item.PartyFullName
                                 + '</option>');
    });

    CreatingInvoice.GetLicensees();



    NapAres.NapAresPopUp = $('#dialog-popup');
});

function Submit() {
    CreatingInvoice.GetNewInvoiceData();
}

function GetInvoiceType() {
    NapAres.disableInput();
    $.ajax({
        url: domain + "Invoice/Invoice/GetInvoiceType",
        type: "POST",
        data: 'licenseeID=' + $("#ddlLicensee").val(),
        success: function (result) {
            var invoiceTypeList = [];
            var productList = [];
            for (var i = 0; i < result.length; i++) {
                for (var j = 0; j < result[i].ProductList.length; j++) {
                    productList.push({
                        Id: "#ddlProduct" + result[i].ProductList[j].ProductID,
                        Value: result[i].ProductList[j].ProductID,
                        Text: result[i].ProductList[j].ProductName,
                        Checked: true,
                        Enable: true,
                        CssClass: ""
                    });
                }
                invoiceTypeList.push({
                    Id: "#ddInvoice" + result[i].InvoiceTypeID,
                    Value: result[i].InvoiceTypeID,
                    Text: result[i].InvoiceTypeName,
                    Checked: true,
                    Enable: true,
                    CssClass: "",
                    Items: productList
                });

                productList = [];
            }


            $("#ddlInvoiceType").multiSelect(
        {
            header: false,
            minWidth: 170,
            maxHeight: 200,
            shadow: true,
            showHeader: false,
            isCheckAll: false,
            showFooter: false,
            selectedList: 4,
            checkAllText: 'All',
            isUsedCustomCheckAllText: false,
            customCheckAllText: 'All',
            listData: invoiceTypeList
        });
        },
        error: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { }
        },
        complete: function (jqXHR, textStatus) {
            NapAres.enableInput();
        }
    });

    this.GetInvoiceNo();
}

function GetInvoiceNo() {
    NapAres.disableInput();
    $.ajax({
        url: domain + "Invoice/Invoice/GetInvoiceNo",
        type: "POST",
        data: 'PartyId=' + $('#ddlOperator').val() + '&LicenseeId=' + $('#ddlLicensee').val() + '&InvoiceTypeID=1',
        success: function (data, textStatus, jqXHR) {
            $('#invoiceNo').val(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert("fail");
        },
        complete: function (jqXHR, textStatus) {
            NapAres.enableInput();
        }
    });
}

var CreatingInvoice = {
    Licensees: [],
    FromDate: '',
    ToDate: '',
    DueDate: '',
    Operator: null,
    Licensee: null,
    InvoiceType: null,
    Currency: null,
    Products: '',
    InvoiceNo: '',
    InvoiceName: '',
    GetLicensees: function () {
        NapAres.disableInput();
        $('#ddlLicensee').html('');
        $.ajax({
            url: domain + "Invoice/Invoice/GetLicenseesByParty",
            type: "POST",
            data: 'PartyId=' + $('#ddlOperator').val(),
            success: function (data, textStatus, jqXHR) {
                if (data && data.length > 0) {
                    $.each(data, function (i, item) {
                        $('#ddlLicensee').append('<option value="'
                                 + item.LicenseeID
                                 + '">'
                                 + item.BrandName
                                 + '</option>');
                    });

                }
                CreatingInvoice.Licensees = data;
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert("fail");
            },
            complete: function (jqXHR, textStatus) {
                NapAres.enableInput();
            }
        });
    },
    GetFilteringInfo: function () {
        var Operator = operatorListModel[$("#ddlOperator").prop("selectedIndex")];
        var Licensee = CreatingInvoice.Licensees[$("#ddlLicensee").prop("selectedIndex")];

        CreatingInvoice.Operator = Operator;
        CreatingInvoice.Licensee = Licensee;
        CreatingInvoice.FromDate = $('#FromDatePicker').val();
        CreatingInvoice.ToDate = $('#ToDatePicker').val();
        CreatingInvoice.DueDate = $('#DueDatePicker').val();
        CreatingInvoice.InvoiceType = { InvoiceTypeID: 1, InvoiceTypeName: 'Software-Data Fees' };
        CreatingInvoice.Currency = { CurrencyID: $('#CurrencyID').val(), CurrencyName: $('#CurrencyID option:selected').text() };
        CreatingInvoice.InvoiceNo = $('#invoiceNo').val();
        CreatingInvoice.InvoiceName = Operator.PartyName + ' INV ' + (CreatingInvoice.InvoiceNo > 9 ? CreatingInvoice.InvoiceNo : '0' + CreatingInvoice.InvoiceNo);
        CreatingInvoice.Products = '1,2,3,5';
    },
    GetNewInvoiceData: function () {
        NapAres.disableInput();
        CreatingInvoice.GetFilteringInfo();

        $.ajax({
            url: domain + "Invoice/Invoice/GetInvoice",
            type: "POST",
            data: 'PartyID=' + CreatingInvoice.Operator.PartyID + '&LicenseeID=' + CreatingInvoice.Licensee.LicenseeID +
                '&Products=' + CreatingInvoice.Products + '&FromDate=' + CreatingInvoice.FromDate + '&ToDate=' + CreatingInvoice.ToDate +
                '&BillingCurrency=' + CreatingInvoice.Currency.CurrencyID,
            success: function (data, textStatus, jqXHR) {
                $('#parentContainer').html(data);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert("fail");
            },
            complete: function (jqXHR, textStatus) {
                NapAres.enableInput();
            }
        });
    },
    SaveInvoice: function () {
        
        invoiceInfoModel.Operator = CreatingInvoice.Operator;
        invoiceInfoModel.Licensee = CreatingInvoice.Licensee;
        invoiceInfoModel.FromDate = CreatingInvoice.FromDate;
        invoiceInfoModel.ToDate = CreatingInvoice.ToDate;
        invoiceInfoModel.DueDate = CreatingInvoice.DueDate;
        invoiceInfoModel.Currency = CreatingInvoice.Currency.CurrencyID;
        invoiceInfoModel.CurrencyName = CreatingInvoice.Currency.CurrencyName;
        invoiceInfoModel.InvoiceType = CreatingInvoice.InvoiceType;
        invoiceInfoModel.Product = CreatingInvoice.Products;
        invoiceInfoModel.InvoiceNo = CreatingInvoice.InvoiceNo;
        invoiceInfoModel.InvoiceName = CreatingInvoice.InvoiceName;
        var message = '';
        $.ajax({
            url: domain + "Invoice/Invoice/SaveInvoice",
            data: '{"invoiceModel":' + JSON.stringify(invoiceInfoModel) + '}',
            contentType: 'application/json',
            type: "POST",
            success: function (data, textStatus, jqXHR) {
                if (data) {
                    if (data.ErrorCode == 0) {
                        $('#saveID').addClass('ui-state-disabled');
                        $('#btnGenerate').addClass('ui-state-disabled');
                        $('#reviewID').removeClass('ui-state-disabled');
                        $('#pdfID').removeClass('ui-state-disabled');
                        
                        NapAres.Helper.showMsgBox({ ErrorCode: 0, ErrorMsg: 'Saving invoice succesffully.' });
                    }
                    else {
                        NapAres.Helper.showMsgBox(data);
                    }
                }
                else {
                    NapAres.Helper.showMsgBox({ ErrorCode: 1, ErrorMsg: 'Saving invoice unsuccesffully.' });                    
                }

                NapAres.popUp.removeHighLight();
                NapAres.popUp.show(message, 'btnGenerate');
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert("fail");
            },
            complete: function (jqXHR, textStatus) {
                NapAres.enableInput();
            }
        });

    }

}