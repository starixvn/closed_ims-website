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
        GetLicensees();
    });
    //------binding data
    $.each(operatorListModel, function (i, item) {
        $('#ddlOperator').append('<option value="'
                                 + item.PartyID
                                 + '">'
                                 + item.PartyFullName
                                 + '</option>');
    });

    GetLicensees();

});

function Submit() {
    NapTicketControl.disableInput();
    $.ajax({
        url: domain + "Invoice/Invoice/GetInvoice",
        type: "POST",
        data: 'PartyID=' + $('#ddlOperator').val() + '&LicenseeID=' + $('#ddlLicensee').val() +
                '&Products=1,2,3,5' + '&FromDate=' + $('#FromDatePicker').val() + '&ToDate=' + $('#ToDatePicker').val() +
                '&BillingCurrency=' + $('#CurrencyID').val(),
        success: function (data, textStatus, jqXHR) {
            $('#parentContainer').html(data);
            $('#pdfID').button();
            $('#pdfID').click(function () { if (!$(this).hasClass('ui-state-disabled')) Submit(); });
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert("fail");
        },
        complete: function (jqXHR, textStatus) {
            NapTicketControl.enableInput();
        }
    });
}

function GetInvoiceType() {
    NapTicketControl.disableInput();
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
            NapTicketControl.enableInput();
        }
    });

    this.GetInvoiceNo();
}

function GetInvoiceNo() {
    NapTicketControl.disableInput();
    $.ajax({
        url: domain + "Invoice/Invoice/GetInvoiceNo",
        type: "POST",
        data: 'PartyId=' + $('#ddlOperator').val() + '&LicenseeID=' + $('#ddlLicensee').val() + '&InvoiceTypeID=1',
        success: function (data, textStatus, jqXHR) {
            $('#invoiceNo').val(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert("fail");
        },
        complete: function (jqXHR, textStatus) {
            NapTicketControl.enableInput();
        }
    });
}

function GetLicensees() {
    NapTicketControl.disableInput();
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

        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert("fail");
        },
        complete: function (jqXHR, textStatus) {
            NapTicketControl.enableInput();
        }
    });
}