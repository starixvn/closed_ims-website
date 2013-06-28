$(document).ready(function () {
    domain = location.protocol + '//' + location.host + $('#NapHubRoot').val();
    //------binding events
    CreatingInvoice.init();

});

function Submit() {
    CreatingInvoice.GetNewInvoiceData();
}

function GetInvoiceType() {
    NapAres.disableInput();
    $.ajax({
        url: domain + "Invoice/Invoice/GetInvoiceType",
        type: "POST",
        data: 'licenseeID=' + CreatingInvoice.FilterControls.ddlLicensee.val(),
        success: function (result) {
            var invoiceTypeList = [];
            var productList = [];
            for (var i = 0; i < result.length; i++) {
                for (var j = 0; j < result[i].ProductList.length; j++) {
                    productList.push({
                        Id: "#ddlProduct" + result[i].ProductList[j].ProductID,
                        Value: result[i].ProductList[j].ProductID,
                        Text: result[i].ProductList[j].ProductName,
                        Checked: (i == 0 ? true : false),
                        Enable: true,
                        CssClass: ""
                    });
                }
                invoiceTypeList.push({
                    Id: "#ddlInvoiceType" + result[i].InvoiceTypeID,
                    Value: result[i].InvoiceTypeID,
                    Text: result[i].InvoiceTypeName,
                    Checked: (i == 0 ? true : false),
                    Enable: true,
                    CssClass: "",
                    Items: productList
                });

                productList = [];
            }

            $("#ddlInvoiceType").multiRadioSelect({ //multiple: false,
                showCheckAll: false,
                maxHeight: 705,
                minWidth: 170,
                showFooter: false,
                listData: invoiceTypeList,
                hasOptGroup: true,
                onCheck: function () {
                    CreatingInvoice.FilterControls.btnGenerate.removeClass('ui-state-disabled');
                    $('#saveID').addClass('ui-state-disabled').attr('disabled', 'disabled');
                }, // when an individual checkbox is clicked 
                selectedList: 0,
                isCheckAll: false,
                isRetrieveDataAfterClose: false,
                showParentSelectedText: true,
                multiple: false
            });

            $("#divInvoiceType input[type='radio']").change(function () {
                GetInvoiceNo();

            });


            GetInvoiceNo();
        },
        error: function () {
            try {
                parent.jQuery.unblockUI();
            } catch (e) { }
        },
        complete: function (jqXHR, textStatus) {
            NapAres.enableInput();
        }
    });


}

function GetInvoiceNo() {

    NapAres.disableInput();
    CreatingInvoice.GetSelectedInvoiceType();
    $.ajax({
        url: domain + "Invoice/Invoice/GetInvoiceNo",
        type: "POST",
        data: 'PartyId=' + CreatingInvoice.FilterControls.ddlOperator.val() + '&LicenseeId=' + CreatingInvoice.FilterControls.ddlLicensee.val() + '&InvoiceTypeID=' + CreatingInvoice.InvoiceType.InvoiceTypeID,
        success: function (data, textStatus, jqXHR) {
            CreatingInvoice.FilterControls.txtInvoiceNo.val(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            NapAres.Helper.showMsgBox({
                ErrorCode: 1,
                ErrorMsg: 'Loading InvoiceNo fail.'
            });
            setTimeout('NapAres.Helper.hideMsgBox()', CreatingInvoice.messageTimeOut);
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
    ProductsName: '',
    InvoiceNo: '',
    InvoiceName: '',
    InvoiceID: 0,
    IdNewItem: 0,
    messageTimeOut: 4000,
    isInsert: true,
    IdNewItemList: [],
    FilterControls: {
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
    },
    init: function () {
        $("#notemessage").tipsy({
            html: true,
            opacity: 0.9,
            gravity: 'ne'
        });

        var rangeDate = CreatingInvoice.getDateRangeOfLastMonth(_today);
        $('#fromTopickerplugin').nexDatePicker({
            nexDatePickerType: "daterange",
            datePickerFormat: {
                changeMonth: true,
                dateFormat: "M dd, yy",
                maxDate: new Date(maxDate)
            },
            firstDatePicker: {
                Id: "FromDatePicker",
                Label: labelFrom,
                InitDate: rangeDate[0],
                LabelCss: "lbl-field col_3",
                TextboxCss: "edit-field col_5"
            },
            secondDatePicker: {
                Id: "ToDatePicker",
                Label: labelTo,
                LabelCss: "lbl-field col_1",
                TextboxCss: "edit-field col_5"
            },
            onSelectDatePicker: function () {
                CreatingInvoice.FilterControls.btnGenerate.removeClass('ui-state-disabled');
                $('#saveID').addClass('ui-state-disabled').attr('disabled', 'disabled');
            },
            minDate: new Date(minDate)
            //,maxDate: new Date(maxDate)
        });

        $('#ToDatePicker').datepicker("setDate", rangeDate[1]);

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
                TextboxCss: "edit-field col_6",
                InitDate: new Date(selectDueDate)
            },
            onSelectDatePicker: function () {
                CreatingInvoice.DueDate = CreatingInvoice.FilterControls.cldDueDate.val();
                $('#saveID').removeClass('ui-state-disabled').removeAttr('disabled');
                $('#pdfID').addClass('ui-state-disabled').attr('disabled', 'disabled');
            },
            minDate: new Date(minDate)
        });

        CreatingInvoice.FilterControls.cldFromDate = $('#FromDatePicker');
        CreatingInvoice.FilterControls.cldToDate = $('#ToDatePicker');
        CreatingInvoice.FilterControls.cldDueDate = $('#DueDatePicker');

        CreatingInvoice.FilterControls.btnGenerate.button();
        CreatingInvoice.FilterControls.btnGenerate.click(function () {
            if (!$(this).hasClass('ui-state-disabled')) Submit();
        });

        CreatingInvoice.FilterControls.btnNewInvoice.button();

        CreatingInvoice.FilterControls.ddlLicensee.change(function () {
            if (CreatingInvoice.FilterControls.ddlLicensee.prop("selectedIndex") != 0) {
                GetInvoiceType();
                CreatingInvoice.FilterControls.btnGenerate.removeClass('ui-state-disabled');
                $('#saveID').addClass('ui-state-disabled').attr('disabled', 'disabled');
            } else {
                CreatingInvoice.FilterControls.divInvoiceType.html('<a class="ui-multiselect ui-multiselect-widget ui-multiselect-state-default" id="ddlInvoiceType" style="width: 170px;" parent-checked-value="4"><input type="text" title="" value="Please Select One!" class="ui-multiselect-state-default" readonly="readonly" style="width: 157px;"><span class="ui-multiselect-icon ui-multiselect-icon-triangle-1-s"></span></a>');
                CreatingInvoice.FilterControls.txtInvoiceNo.val('');
            }
        });

        CreatingInvoice.FilterControls.ddlOperator.change(function () {
            CreatingInvoice.FilterControls.ddlLicensee.html('');
            CreatingInvoice.FilterControls.ddlLicensee.append('<option value="0">Please Select One!</option>');

            $('#divInvoiceType').html('<a class="ui-multiselect ui-multiselect-widget ui-multiselect-state-default" id="ddlInvoiceType" style="width: 170px;" parent-checked-value="4"><input type="text" title="" value="Please Select One!" class="ui-multiselect-state-default" readonly="readonly" style="width: 157px;"><span class="ui-multiselect-icon ui-multiselect-icon-triangle-1-s"></span></a>');
            CreatingInvoice.FilterControls.txtInvoiceNo.val('');

            if (CreatingInvoice.FilterControls.ddlOperator.prop("selectedIndex") != 0) {
                CreatingInvoice.GetLicensees();
            }

        });

        CreatingInvoice.FilterControls.txtInvoiceNo.change(function () {
            CreatingInvoice.FilterControls.btnGenerate.removeClass('ui-state-disabled');
            $('#saveID').addClass('ui-state-disabled').attr('disabled', 'disabled');
        });

        CreatingInvoice.FilterControls.ddlCurrency.change(function () {
            CreatingInvoice.FilterControls.btnGenerate.removeClass('ui-state-disabled');
            $('#saveID').addClass('ui-state-disabled').attr('disabled', 'disabled');
        });

        //------binding data

        CreatingInvoice.FilterControls.ddlOperator.append('<option value="0">Please Select One!</option>');
        $.each(operatorListModel, function (i, item) {
            CreatingInvoice.FilterControls.ddlOperator.append('<option value="' + item.PartyID + '">' + item.PartyFullName + '</option>');
        });
        CreatingInvoice.FilterControls.ddlLicensee.append('<option value="0">Please Select One!</option>');
        $('#divInvoiceType').html('<a class="ui-multiselect ui-multiselect-widget ui-multiselect-state-default" id="ddlInvoiceType" style="width: 170px;" parent-checked-value="4"><input type="text" title="" value="Please Select One!" class="ui-multiselect-state-default" readonly="readonly" style="width: 157px;"><span class="ui-multiselect-icon ui-multiselect-icon-triangle-1-s"></span></a>');
        NapAres.NapAresPopUp = $('#dialog-popup');


    },
    GetLicensees: function () {
        NapAres.disableInput();
        $.ajax({
            url: domain + "Invoice/Invoice/GetLicenseesByParty",
            type: "POST",
            data: 'PartyId=' + CreatingInvoice.FilterControls.ddlOperator.val(),
            success: function (data, textStatus, jqXHR) {
                if (data && data.length > 0) {
                    //                    CreatingInvoice.FilterControls.ddlLicensee.append('<option value="0">Please Select a Licensee</option>');
                    $.each(data, function (i, item) {
                        CreatingInvoice.FilterControls.ddlLicensee.append('<option value="' + item.LicenseeID + '">' + item.BrandName + '</option>');
                    });
                }
                CreatingInvoice.Licensees = data;
            },
            error: function (xhr, ajaxOptions, thrownError) {
                NapAres.Helper.showMsgBox({
                    ErrorCode: 1,
                    ErrorMsg: 'Loading Licensee data fail.'
                });
                setTimeout('NapAres.Helper.hideMsgBox()', CreatingInvoice.messageTimeOut);
            },
            complete: function (jqXHR, textStatus) {
                NapAres.enableInput();
            }
        });
    },
    GetFilteringInfo: function () {
        var Operator = operatorListModel[CreatingInvoice.FilterControls.ddlOperator.prop("selectedIndex") - 1];
        var Licensee = CreatingInvoice.Licensees[CreatingInvoice.FilterControls.ddlLicensee.prop("selectedIndex") - 1];

        CreatingInvoice.GetSelectedInvoiceType();

        CreatingInvoice.Operator = Operator;
        CreatingInvoice.Licensee = Licensee;
        CreatingInvoice.FromDate = CreatingInvoice.FilterControls.cldFromDate.val();
        CreatingInvoice.ToDate = CreatingInvoice.FilterControls.cldToDate.val();
        CreatingInvoice.DueDate = CreatingInvoice.FilterControls.cldDueDate.val();
        CreatingInvoice.Currency = {
            CurrencyID: CreatingInvoice.FilterControls.ddlCurrency.val(),
            CurrencyName: $('#CurrencyID option:selected').text()
        };
        CreatingInvoice.InvoiceNo = CreatingInvoice.FilterControls.txtInvoiceNo.val();
        CreatingInvoice.InvoiceName = this.mappingSpecialPartyName(Operator.PartyName) + ' INV-' + (CreatingInvoice.InvoiceNo > 9 ? CreatingInvoice.InvoiceNo : '0' + CreatingInvoice.InvoiceNo);

    },
    GetNewInvoiceData: function () {
        if (CreatingInvoice.FilterControls.ddlOperator.prop("selectedIndex") == 0 ||
            CreatingInvoice.FilterControls.ddlLicensee.prop("selectedIndex") == 0 ||
            $.isNumeric(CreatingInvoice.FilterControls.txtInvoiceNo.val()) == false)
            return false;

        if (CreatingInvoice.FilterControls.txtInvoiceNo.val().length == 0) {
            NapAres.Helper.showMsgBox({
                ErrorCode: 1,
                ErrorMsg: 'Please input Invoice No.!'
            });
            setTimeout('NapAres.Helper.hideMsgBox()', CreatingInvoice.messageTimeOut);
            return false;
        }

        CreatingInvoice.GetFilteringInfo();

        if (CreatingInvoice.Products.length == 0) {
            NapAres.Helper.showMsgBox({
                ErrorCode: 1,
                ErrorMsg: 'Please select product!'
            });
            setTimeout('NapAres.Helper.hideMsgBox()', CreatingInvoice.messageTimeOut);

            return false;
        }

        NapAres.disableInput();
        $.ajax({
            url: domain + "Invoice/Invoice/IsExistInvoiceNo",
            type: "POST",
            data: 'PartyID=' + CreatingInvoice.Operator.PartyID + '&LicenseeID=' + CreatingInvoice.Licensee.LicenseeID + '&InvoiceNo=' + CreatingInvoice.InvoiceNo + '&InvoiceType=' + CreatingInvoice.InvoiceType.InvoiceTypeID,
            success: function (data, textStatus, jqXHR) {
                if (data == false) {
                    $.ajax({
                        url: domain + "Invoice/Invoice/GetInvoice",
                        type: "POST",
                        data: 'PartyID=' + CreatingInvoice.Operator.PartyID + '&LicenseeID=' + CreatingInvoice.Licensee.LicenseeID + '&Products=' + CreatingInvoice.Products + '&FromDate=' + CreatingInvoice.FromDate + '&ToDate=' + CreatingInvoice.ToDate + '&BillingCurrency=' + CreatingInvoice.Currency.CurrencyID,
                        success: function (data, textStatus, jqXHR) {
                            $('#parentContainer').html(data);
                            CreatingInvoice.IdNewItemList = [];
                            CreatingInvoice.IdNewItem = invoiceInfoModel.StatementList.length + 1;
                            $("#parentContainer").css("margin-top", "90px !important;");
                            //disable Generate button
                            CreatingInvoice.FilterControls.btnGenerate.addClass('ui-state-disabled');

                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            $('#parentContainer').html('');
                            NapAres.Helper.showMsgBox({
                                ErrorCode: 1,
                                ErrorMsg: 'Loading data fail.'
                            });
                            setTimeout('NapAres.Helper.hideMsgBox()', CreatingInvoice.messageTimeOut);
                        },
                        complete: function (jqXHR, textStatus) {
                            NapAres.enableInput();
                        }
                    });
                } else {
                    $('#parentContainer').html('');
                    NapAres.Helper.showMsgBox({
                        ErrorCode: 1,
                        ErrorMsg: 'Invoice No is existed!'
                    });
                    setTimeout('NapAres.Helper.hideMsgBox()', CreatingInvoice.messageTimeOut);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                $('#parentContainer').html('');
                NapAres.Helper.showMsgBox({
                    ErrorCode: 1,
                    ErrorMsg: 'Loading data fail.'
                });
                setTimeout('NapAres.Helper.hideMsgBox()', CreatingInvoice.messageTimeOut);
            },
            complete: function (jqXHR, textStatus) {
                NapAres.enableInput();
            }
        });
    },
    SaveInvoice: function () {

        CreatingInvoice.UpdateItem();
        invoiceInfoModel.Operator = CreatingInvoice.Operator;
        invoiceInfoModel.Licensee = CreatingInvoice.Licensee;
        invoiceInfoModel.FromDate = CreatingInvoice.FromDate;
        invoiceInfoModel.ToDate = CreatingInvoice.ToDate;
        invoiceInfoModel.DueDate = CreatingInvoice.DueDate;
        invoiceInfoModel.Currency = CreatingInvoice.Currency.CurrencyID;
        invoiceInfoModel.CurrencyName = CreatingInvoice.Currency.CurrencyName;
        invoiceInfoModel.InvoiceType = CreatingInvoice.InvoiceType;
        invoiceInfoModel.Product = CreatingInvoice.Products;
        invoiceInfoModel.ProductsName = CreatingInvoice.ProductsName;
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
                        $('#saveID').addClass('ui-state-disabled').attr('disabled', 'disabled');
                        CreatingInvoice.FilterControls.btnGenerate.addClass('ui-state-disabled');
                        $('#pdfID').removeClass('ui-state-disabled').removeAttr('disabled');

                        CreatingInvoice.FilterControls.ddlOperator.attr('disabled', 'disabled');
                        CreatingInvoice.FilterControls.ddlLicensee.attr('disabled', 'disabled');
                        CreatingInvoice.FilterControls.cldFromDate.attr('disabled', 'disabled').addClass('ui-state-disabled');
                        CreatingInvoice.FilterControls.cldToDate.attr('disabled', 'disabled').addClass('ui-state-disabled');
                        CreatingInvoice.FilterControls.divInvoiceType.find('input').attr('disabled', 'disabled').addClass('ui-state-disabled');
                        CreatingInvoice.FilterControls.ddlCurrency.attr('disabled', 'disabled');
                        CreatingInvoice.FilterControls.txtInvoiceNo.attr('disabled', 'disabled');

                        CreatingInvoice.InvoiceID = data.ErrorMessage;
                        invoiceInfoModel.InvoiceID = CreatingInvoice.InvoiceID;
                        OrgInvoiceInfoModel.InvoiceID = CreatingInvoice.InvoiceID;
                        NapAres.Helper.showMsgBox({
                            ErrorCode: 0,
                            ErrorMsg: 'Saving invoice succesffully.'
                        });
                        invoiceInfoModel = [];
                        invoiceInfoModel = $.extend(true, {}, OrgInvoiceInfoModel);
                    } else {
                        NapAres.Helper.showMsgBox({
                            ErrorCode: data.ErrCode,
                            ErrorMsg: data.ErrorMessage
                        });
                        setTimeout('NapAres.Helper.hideMsgBox()', CreatingInvoice.messageTimeOut);
                    }
                } else {
                    NapAres.Helper.showMsgBox({
                        ErrorCode: 1,
                        ErrorMsg: 'Saving invoice unsuccesffully.'
                    });
                    setTimeout('NapAres.Helper.hideMsgBox()', CreatingInvoice.messageTimeOut);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                NapAres.Helper.showMsgBox({
                    ErrorCode: 1,
                    ErrorMsg: 'Saving invoice unsuccesffully.'
                });
                setTimeout('NapAres.Helper.hideMsgBox()', CreatingInvoice.messageTimeOut);
            },
            complete: function (jqXHR, textStatus) {
                NapAres.enableInput();
            }
        });

        setTimeout('NapAres.Helper.hideMsgBox()', CreatingInvoice.messageTimeOut);
    },
    getProductId: function () {
        var listChecked = $('#ddlProduct').next('div.ui-multiselect-options').find('label:not(.ui-multiselect-state-disabled,.ui-multiselect-group-mark)').find('input:not(#' + $('#ddlLicenseese').attr('id') + "HeaderCheckBox)").filter('[checked]');
        var normalLicensees = [];
        var operator = [];
        for (var i = 0; i < listChecked.length; i++) {
            if (listChecked[i].id.indexOf('Detail') != -1) {
                normalLicensees.push(listChecked[i].value);
            } else {
                operator.push(listChecked[i].value);
            }
        }
        var result = {
            'normal': normalLicensees,
            'operator': operator
        }
        return result;
    },
    AddItem: function (isDiscount) {

        var displayID = invoiceInfoModel.StatementList.length + 1;
        for (var i = 0; i < CreatingInvoice.IdNewItemList.length; i++) {
            var id = CreatingInvoice.IdNewItemList[i];
            var amount = $('#newItem_text_' + id).val();
            if ($('#newItem_display_' + id).attr('discountItem') == "false") {
                displayID++;
            }
        }
        if (CreatingInvoice.isInsert) {
            $('#invoiceTbl tr.total').before(' <tr id="newItem_' + CreatingInvoice.IdNewItem + '" style="width: 100%; text-align: center;">\
                                    <td   style="width: 10%; "><span discountItem="' + (isDiscount == true ? true : false) + '"  id="newItem_display_' + CreatingInvoice.IdNewItem + '">' + (isDiscount == true ? "" : displayID) + ' </span></br></br> <span  class="delete" onclick="CreatingInvoice.DeleteItem(' + CreatingInvoice.IdNewItem + ')"></span></td>\
                                <td style="width: 55%; text-align: center; height: 60px;">\
                                    <textarea  onfocus="onFocusInput(this)" onblur="onBlurInput(this)"  id="newItem_des_' + CreatingInvoice.IdNewItem + '" rows="2" cols="106" name="styled-textarea" id="styled_300"  onchange=" CreatingInvoice.AllowUpdateNewInfo(this);"></textarea></td>\
                                <td style="width: 20%; text-align: center;"><textarea  onfocus="onFocusInput(this)" onblur="onBlurInput(this)" id="newItem_ref_' + CreatingInvoice.IdNewItem + '" rows="2" cols="33" name="styled-textarea"  onchange=" CreatingInvoice.AllowUpdateNewInfo(this);"></textarea></td>\
                                <td style="width: 15%; text-align: center;">\
                                    <div style="width: 90%; height: 20px; margin: -20px 5px 0px 10px;text-align: right;">\
                                        <input class="numeric"  name=""  onblur="CreatingInvoice.RecalculatorTotal();" id="newItem_text_' + CreatingInvoice.IdNewItem + '" type="text" style="font-weight: bold;text-align:right;"></input></div></td></tr>');
            CreatingInvoice.IdNewItemList.push(CreatingInvoice.IdNewItem);
            CreatingInvoice.IdNewItem = CreatingInvoice.IdNewItem + 1;
            $(".numeric").numeric();
            $('#saveID').addClass('ui-state-disabled').attr('disabled', 'disabled');
            $('#reviewID').addClass('ui-state-disabled').attr('disabled', 'disabled');
            $('#pdfID').addClass('ui-state-disabled').attr('disabled', 'disabled');
        }
    },
    DeleteItem: function (trId) {
        //        $(document.getElementById(trId)).remove();
        $("#newItem_" + trId).remove();
        CreatingInvoice.IdNewItemList = jQuery.grep(CreatingInvoice.IdNewItemList, function (value) {
            return value != trId;
        });
        if (CreatingInvoice.IdNewItemList.length == 0) {
            $('#saveID').addClass('ui-state-disabled').attr('disabled', 'disabled');
            $('#pdfID').removeClass('ui-state-disabled').removeAttr('disabled');
        } else {
            $('#saveID').removeClass('ui-state-disabled').removeAttr('disabled');
            $('#pdfID').addClass('ui-state-disabled').attr('disabled', 'disabled');
        }

        CreatingInvoice.RecalculatorTotal();
    },
    RecalculatorTotal: function () {
        NapAres.Helper.hideMsgBox();
        var totalAmountTmp = _totalAmount;
        var index = invoiceInfoModel.StatementList.length + 1;

        for (var i = 0; i < CreatingInvoice.IdNewItemList.length; i++) {
            var id = CreatingInvoice.IdNewItemList[i];

            var amount = $('#newItem_text_' + id).val();
            if ($.isNumeric(amount) == true) {
                if ($('#newItem_display_' + id).attr('discountItem') == "true") {
                    $('#newItem_display_' + id).attr('amount', parseInt(amount) > 0 ? (parseInt(amount) * -1) : parseInt(amount));
                } else {
                    $('#newItem_display_' + id).attr('amount', amount);
                }
            }
        }

        for (var i = 0; i < CreatingInvoice.IdNewItemList.length; i++) {
            var id = CreatingInvoice.IdNewItemList[i];

            var amount = $('#newItem_display_' + id).attr('amount');
            if ($('#newItem_display_' + id).attr('discountItem') == "false") {
                $('#newItem_display_' + id).empty().html(index);
                index++;
            }
            if ($.isNumeric(amount) == false) {
                $('#saveID').addClass('ui-state-disabled').attr('disabled', 'disabled');
                if (amount == '') {
                    NapAres.Helper.showMsgBox({
                        ErrorCode: 1,
                        ErrorMsg: 'Please input value !'
                    });
                    setTimeout('NapAres.Helper.hideMsgBox()', CreatingInvoice.messageTimeOut);
                    CreatingInvoice.isInsert = false;
                    $('#saveID').addClass('ui-state-disabled').attr('disabled', 'disabled');
                    $('#reviewID').addClass('ui-state-disabled').attr('disabled', 'disabled');
                    $('#pdfID').addClass('ui-state-disabled').attr('disabled', 'disabled');

                    return false;
                }
                NapAres.Helper.showMsgBox({
                    ErrorCode: 1,
                    ErrorMsg: 'Please just input numberic !'
                });
                setTimeout('NapAres.Helper.hideMsgBox()', CreatingInvoice.messageTimeOut);
                CreatingInvoice.isInsert = false;
                $('#saveID').addClass('ui-state-disabled').attr('disabled', 'disabled');
                $('#reviewID').addClass('ui-state-disabled').attr('disabled', 'disabled');
                $('#pdfID').addClass('ui-state-disabled').attr('disabled', 'disabled');

                return false;
            }
            if (amount != '') {
                var formatNumber = '';
                if ($('#newItem_display_' + id).attr('discountItem') == "true") {
                    $('#newItem_text_' + id).addClass("red");
                    formatNumber = "(" + numeral(parseInt(amount) > 0 ? parseInt(amount) : (parseInt(amount) * -1)).format(FormatConstant.TWO_DECIMAL_NUMBER) + ")";
                } else {
                    if (parseInt(amount) < 0) {
                        $('#newItem_text_' + id).addClass("red");
                        formatNumber = "(" + numeral(parseInt(amount) * -1).format(FormatConstant.TWO_DECIMAL_NUMBER) + ")";
                    } else {
                        $('#newItem_text_' + id).removeClass("red");
                        formatNumber = numeral(amount).format(FormatConstant.TWO_DECIMAL_NUMBER);
                    }

                }
                $('#newItem_text_' + id).empty().val(formatNumber);
                totalAmountTmp += parseFloat(amount);
            }
            if (numeral(amount).format(FormatConstant.TWO_DECIMAL_NUMBER) == '0.00') {
                NapAres.Helper.showMsgBox({
                    ErrorCode: 1,
                    ErrorMsg: 'Amount is invalid!'
                });
                $('#saveID').addClass('ui-state-disabled').attr('disabled', 'disabled');
                $('#reviewID').addClass('ui-state-disabled').attr('disabled', 'disabled');
                $('#pdfID').addClass('ui-state-disabled').attr('disabled', 'disabled');

                return false;
            }
        }


        $("#totalAmount").empty().html(CreatingInvoice.Currency.CurrencyName + ' ' + NegativeFormat(totalAmountTmp));
        $('#reviewID').removeClass('ui-state-disabled').removeAttr('disabled');
        $('#saveID').removeClass('ui-state-disabled').removeAttr('disabled');

        CreatingInvoice.isInsert = true;
    },
    UpdateItem: function () {
        var statement = {
            StatementID: 1000,
            StatementDetailList: []
        };

        var index = 0;
        for (var i = 0; i < invoiceInfoModel.StatementList.length; i++) {

            for (var j = 0; j < invoiceInfoModel.StatementList[i].StatementDetailList.length; j++) {
                invoiceInfoModel.StatementList[i].StatementDetailList[j].Description = '';
                invoiceInfoModel.StatementList[i].StatementDetailList[j].Reference = '';

                invoiceInfoModel.StatementList[i].StatementDetailList[j].Description = $('textarea#orgItem_des_' + index).val();
                invoiceInfoModel.StatementList[i].StatementDetailList[j].Reference = $('textarea#orgItem_ref_' + index).val();
                index++;
            }
        }

        for (var i = 0; i < CreatingInvoice.IdNewItemList.length; i++) {
            var id = CreatingInvoice.IdNewItemList[i];
            var statementDetail = {
                Description: '',
                Reference: '',
                Amount: 0,
                AdjustItem: true
            };
            statementDetail.Description = $('textarea#newItem_des_' + id).val();
            statementDetail.Reference = $('textarea#newItem_ref_' + id).val();
            statementDetail.Amount = $('#newItem_display_' + id).attr('amount');
            statementDetail.DiscountItem = $('#newItem_display_' + id).attr('discountItem');
            statement.StatementDetailList.push(statementDetail);
            //            CreatingInvoice.StatementDetailList.push(statementDetail);
        }
        if (statement.StatementDetailList.length > 0) {
            invoiceInfoModel.StatementList.push(statement);
        }
        invoiceInfoModel.Note = $('#noteId').val().replace(/\n/g, '<br/>');
        return invoiceInfoModel.StatementList;
    },
    GetSelectedInvoiceType: function () {
        var listChecked = $('#ddlInvoiceType').next('div.ui-multiselect-options').find('label:not(.ui-multiselect-state-disabled,.ui-multiselect-group-mark)').find('input:not(#' + $('#ddlProduct').attr('id') + "HeaderCheckBox)").filter('[checked]');
        var productName = '';
        var produtID = '';
        var index = 0;
        for (var i = 0; i < listChecked.length; i++) {
            if (listChecked[i].value == 'on')
                continue;
            if (index < listChecked.length - 1) {
                productName += listChecked[i].title + ',';
                produtID += listChecked[i].value + ',';
            } else {
                productName += listChecked[i].title;
                produtID += listChecked[i].value;
            }
            index++;
        }

        var radioCheck = $("#divInvoiceType input[type='radio']:checked");
        var InvoiceTypeIdTmp = '';
        var InvoiceTypeNameTmp = '';
        if (radioCheck && radioCheck.length > 0) {
            InvoiceTypeIdTmp = radioCheck[0].value;
            InvoiceTypeNameTmp = radioCheck[0].title;
        }

        CreatingInvoice.Products = produtID;
        CreatingInvoice.ProductsName = productName;
        CreatingInvoice.InvoiceType = {
            InvoiceTypeID: InvoiceTypeIdTmp,
            InvoiceTypeName: InvoiceTypeNameTmp
        };

    },
    AllowUpdateNewInfo: function (item) {
        if (item != undefined) {
            var amountID = 'newItem_text_' + $(item).prop('id').split('_')[2];
            if ($('#' + amountID).val() != '') {
                $('#reviewID').removeClass('ui-state-disabled').removeAttr('disabled');
                $('#saveID').removeClass('ui-state-disabled').removeAttr('disabled');
                $('#pdfID').addClass('ui-state-disabled').attr('disabled');
            }
        } else {
            $('#reviewID').removeClass('ui-state-disabled').removeAttr('disabled');
            $('#saveID').removeClass('ui-state-disabled').removeAttr('disabled');
            $('#pdfID').addClass('ui-state-disabled').attr('disabled');
        }


    },
    getDateRangeOfLastMonth: function (today) {
        var currentDate = new Date(today);
        var currentMonth = currentDate.getMonth();
        var currentYear = currentDate.getFullYear();
        var lastYear = currentDate.getFullYear() - 1;
        var startDate = new Date();
        var endDate = new Date();

        //// If Current month is Jan, so switch to Last year
        if (currentMonth == 0) {
            startDate = new Date(lastYear, 11, 1);
            endDate = new Date(lastYear, 11, 31);
        } else {
            var month = currentMonth - 1;
            startDate = new Date(currentYear, month, 1);
            endDate = new Date(currentYear, month, CreatingInvoice.getTotalDayOfMonth(month + 1, currentYear));
        }
        return [startDate, endDate];
    },
    getTotalDayOfMonth: function (month, year) {
        //// Note Month fomr 1 > 12
        var m = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
        if (month != 2) return m[month - 1];
        if (year % 4 != 0) return m[1];
        if (year % 100 == 0 && year % 400 != 0) return m[1];
        return m[1] + 1;
    },
    mappingSpecialPartyName: function (partyName) {
        if (partyName.toLowerCase() == 'cozni') {
            partyName = 'CL';
        }
        return partyName
    }
}


function NegativeFormat(input) {
    var output = '';
    output = input >= 0 ? "<span style=\"font-weight: bold; \">" + numeral(input).format(FormatConstant.TWO_DECIMAL_NUMBER) + "</span>" : "<span style=\"color: red;font-weight: bold;\">(" + numeral(input * -1).format(FormatConstant.TWO_DECIMAL_NUMBER) + ")</span>";
    return output;
}

$.extend({
    URLEncode: function (s) {
        s = encodeURIComponent(s);
        s = s.replace(/\~/g, '%7E').replace(/\!/g, '%21').replace(/\(/g, '%28').replace(/\)/g, '%29').replace(/\'/g, '%27');
        s = s.replace(/%20/g, '+');
        return s;
    },
    URLDecode: function (s) {
        s = s.replace(/\+/g, '%20');
        s = decodeURIComponent(s);
        return s;
    }
});

function onFocusInput(obj) {
    obj.style.background = '#e5fff3';
}

function onBlurInput(obj) {
    obj.style.background = 'white';
}