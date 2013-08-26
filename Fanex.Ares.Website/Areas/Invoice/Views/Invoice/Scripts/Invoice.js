$(document).ready(function () {
    domain = location.protocol + '//' + location.host + $('#NapHubRoot').val();
    //------binding events
    invoiceApp.init();

});

var invoiceApp = {
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
    TotalAmount: 0,
    InvoiceNo: '',
    InvoiceName: '',
    InvoiceID: 0,
    IdNewItem: 0,
    messageTimeOut: 4000,
    isInsert: true,
    IdNewItemList: [],

    submit: function () {
        if (invoiceApp.filterControls.ddlOperator.prop("selectedIndex") == 0 ||
            invoiceApp.filterControls.ddlLicensee.prop("selectedIndex") == 0 ||
            $.isNumeric(invoiceApp.filterControls.txtInvoiceNo.val()) == false)
            return false;

        if (invoiceApp.filterControls.txtInvoiceNo.val().length == 0) {
            NapAres.Helper.showMsgBox({
                ErrorCode: 1,
                ErrorMsg: 'Please input Invoice No.!'
            });
            setTimeout('NapAres.Helper.hideMsgBox()', invoiceApp.messageTimeOut);
            return false;
        }

        invoiceApp.getFilteringInfo();

        if (invoiceApp.Products.length == 0) {
            NapAres.Helper.showMsgBox({
                ErrorCode: 1,
                ErrorMsg: 'Please select product!'
            });
            setTimeout('NapAres.Helper.hideMsgBox()', invoiceApp.messageTimeOut);

            return false;
        }

        var toDateSelected = new Date(invoiceApp.filterControls.cldToDate.val());
        var totalDays = invoiceApp.helper.getTotalDayOfMonth(toDateSelected.getMonth() + 1, toDateSelected.getFullYear());

        if (toDateSelected.getDate() != totalDays) {
            $("#dialog:ui-dialog").dialog("destroy");
            $("#ims-dialog-message").html('<p><span class="ui-icon ui-icon-alert" style="float:left; margin:0 7px 20px 0;"></span>ToDate is not set as month end.<br/> Are you sure you want to generate data?</p>');
            $("#ims-dialog-message").dialog({
                modal: true,
                resizable: false,
                width: 310,
                position: ['middle', 100],
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                        invoiceApp.getNewInvoiceData();
                    },
                    Cancel: function () {
                        $(this).dialog("close");
                    }
                }
            });
            $(".ui-dialog-titlebar").hide();
        } else {
            invoiceApp.getNewInvoiceData();
        }
    },

    getInvoiceType: function () {
        NapAres.disableInput();
        $.ajax({
            url: domain + "Invoice/Invoice/getInvoiceType",
            type: "POST",
            data: 'licenseeID=' + invoiceApp.filterControls.ddlLicensee.val(),
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
                        if (!$(this).is(':checked')) {
                            invoiceApp.filterControls.btnGenerate.removeClass('ui-state-disabled');
                            $('#saveID').addClass('ui-state-disabled').attr('disabled', 'disabled');
                            $('#reviewID').addClass('ui-state-disabled').attr('disabled', 'disabled');
                        }

                    }, // when an individual checkbox is clicked 
                    selectedList: 0,
                    isCheckAll: false,
                    isRetrieveDataAfterClose: false,
                    showParentSelectedText: true,
                    multiple: false,
                    shadow: true
                });

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
    },

    getInvoiceNo: function () {
        NapAres.disableInput();
        invoiceApp.getSelectedInvoiceType();
        $.ajax({
            url: domain + "Invoice/Invoice/GetInvoiceNo",
            type: "POST",
            data: 'PartyId=' + invoiceApp.filterControls.ddlOperator.val(),
            success: function (data, textStatus, jqXHR) {
                invoiceApp.filterControls.txtInvoiceNo.val(data);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                NapAres.Helper.showMsgBox({
                    ErrorCode: 1,
                    ErrorMsg: 'Loading InvoiceNo fail.'
                });
                setTimeout('NapAres.Helper.hideMsgBox()', invoiceApp.messageTimeOut);
            },
            complete: function (jqXHR, textStatus) {
                NapAres.enableInput();
            }
        });
    },

    getLicensees: function () {
        NapAres.disableInput();
        $.ajax({
            url: domain + "Invoice/Invoice/getLicenseesByParty",
            type: "POST",
            data: 'PartyId=' + invoiceApp.filterControls.ddlOperator.val(),
            success: function (data, textStatus, jqXHR) {
                if (data && data.length > 0) {
                    $.each(data, function (i, item) {
                        invoiceApp.filterControls.ddlLicensee.append('<option value="' + item.LicenseeID + '">' + item.BrandName + '</option>');
                    });
                }
                invoiceApp.Licensees = data;
            },
            error: function (xhr, ajaxOptions, thrownError) {
                NapAres.Helper.showMsgBox({
                    ErrorCode: 1,
                    ErrorMsg: 'Loading Licensee data fail.'
                });
                setTimeout('NapAres.Helper.hideMsgBox()', invoiceApp.messageTimeOut);
            },
            complete: function (jqXHR, textStatus) {
                NapAres.enableInput();
            }
        });
    },

    getFilteringInfo: function () {
        var Operator = operatorListModel[invoiceApp.filterControls.ddlOperator.prop("selectedIndex") - 1];
        var Licensee = invoiceApp.Licensees[invoiceApp.filterControls.ddlLicensee.prop("selectedIndex") - 1];

        invoiceApp.getSelectedInvoiceType();

        invoiceApp.Operator = Operator;
        invoiceApp.Licensee = Licensee;
        invoiceApp.FromDate = invoiceApp.filterControls.cldFromDate.val();
        invoiceApp.ToDate = invoiceApp.filterControls.cldToDate.val();
        invoiceApp.DueDate = invoiceApp.filterControls.cldDueDate.val();
        invoiceApp.Currency = {
            CurrencyID: invoiceApp.filterControls.ddlCurrency.val(),
            CurrencyName: $('#CurrencyID option:selected').text()
        };
        invoiceApp.InvoiceNo = invoiceApp.filterControls.txtInvoiceNo.val();
        invoiceApp.InvoiceName = this.mappingSpecialPartyName(Operator.PartyName) + ' INV-' + (invoiceApp.InvoiceNo > 9 ? invoiceApp.InvoiceNo : '0' + invoiceApp.InvoiceNo);

    },

    getNewInvoiceData: function () {
        NapAres.disableInput();
        $.ajax({
            url: domain + "Invoice/Invoice/IsExistInvoiceNo",
            type: "POST",
            data: 'PartyID=' + invoiceApp.Operator.PartyID + '&InvoiceNo=' + invoiceApp.InvoiceNo,
            success: function (data, textStatus, jqXHR) {
                if (data == false) {
                    $.ajax({
                        url: domain + "Invoice/Invoice/GetInvoice",
                        type: "POST",
                        data: 'PartyID=' + invoiceApp.Operator.PartyID + '&LicenseeID=' + invoiceApp.Licensee.LicenseeID + '&Products=' + invoiceApp.Products + '&FromDate=' + invoiceApp.FromDate + '&ToDate=' + invoiceApp.ToDate + '&BillingCurrency=' + invoiceApp.Currency.CurrencyID,
                        success: function (data, textStatus, jqXHR) {
                            $('#parentContainer').html(data);
                            invoiceApp.IdNewItemList = [];
                            invoiceApp.IdNewItem = invoiceInfoModel.StatementList.length + 1;
                            $("#parentContainer").css("margin-top", "90px !important;");
                            //disable Generate button
                            invoiceApp.filterControls.btnGenerate.addClass('ui-state-disabled');

                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            $('#parentContainer').html('');
                            NapAres.Helper.showMsgBox({
                                ErrorCode: 1,
                                ErrorMsg: 'Loading data fail.'
                            });
                            setTimeout('NapAres.Helper.hideMsgBox()', invoiceApp.messageTimeOut);
                        },
                        complete: function (jqXHR, textStatus) {
                            NapAres.enableInput();
                        }
                    });
                } else {
                    $('#parentContainer').html('');
                    NapAres.Helper.showMsgBox({
                        ErrorCode: 1,
                        ErrorMsg: 'The Invoice No. is existed, please enter a new number!'
                    });
                    setTimeout('NapAres.Helper.hideMsgBox()', invoiceApp.messageTimeOut);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                $('#parentContainer').html('');
                NapAres.Helper.showMsgBox({
                    ErrorCode: 1,
                    ErrorMsg: 'Loading data fail.'
                });
                setTimeout('NapAres.Helper.hideMsgBox()', invoiceApp.messageTimeOut);
            },
            complete: function (jqXHR, textStatus) {
                NapAres.enableInput();
            }
        });
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

    getSelectedInvoiceType: function () {
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

        invoiceApp.Products = produtID;
        invoiceApp.ProductsName = productName;
        invoiceApp.InvoiceType = {
            InvoiceTypeID: InvoiceTypeIdTmp,
            InvoiceTypeName: InvoiceTypeNameTmp
        };
    },

    filterControls: {
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

        var rangeDate = invoiceApp.helper.getDateRangeOfLastMonth(_today);
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
                invoiceApp.filterControls.btnGenerate.removeClass('ui-state-disabled');
                $('#saveID').addClass('ui-state-disabled').attr('disabled', 'disabled');
                $('#reviewID').addClass('ui-state-disabled').attr('disabled', 'disabled');
            },
            minDate: new Date(minDate)
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
                invoiceApp.DueDate = invoiceApp.filterControls.cldDueDate.val();
                $('#saveID').removeClass('ui-state-disabled').removeAttr('disabled');
                $('#reviewID').removeClass('ui-state-disabled').removeAttr('disabled');
                $('#pdfID').addClass('ui-state-disabled').attr('disabled', 'disabled');
            },
            minDate: new Date(minDate)
        });

        invoiceApp.filterControls.cldFromDate = $('#FromDatePicker');
        invoiceApp.filterControls.cldToDate = $('#ToDatePicker');
        invoiceApp.filterControls.cldDueDate = $('#DueDatePicker');

        invoiceApp.filterControls.btnGenerate.button();
        invoiceApp.filterControls.btnGenerate.click(function () {
            if (!$(this).hasClass('ui-state-disabled')) {
                invoiceApp.submit();
            }
        });

        invoiceApp.filterControls.btnNewInvoice.button();

        invoiceApp.filterControls.ddlLicensee.change(function () {
            if (invoiceApp.filterControls.ddlLicensee.prop("selectedIndex") != 0) {
                invoiceApp.getInvoiceType();
                invoiceApp.filterControls.btnGenerate.removeClass('ui-state-disabled');
                $('#saveID').addClass('ui-state-disabled').attr('disabled', 'disabled');
                $('#reviewID').addClass('ui-state-disabled').attr('disabled', 'disabled');
            } else {
                invoiceApp.filterControls.divInvoiceType.html('<a class="ui-multiselect ui-multiselect-widget ui-multiselect-state-default" id="ddlInvoiceType" style="width: 170px;" parent-checked-value="4"><input type="text" title="" value="Please Select One!" class="ui-multiselect-state-default" readonly="readonly" style="width: 157px;"><span class="ui-multiselect-icon ui-multiselect-icon-triangle-1-s"></span></a>');
                invoiceApp.filterControls.txtInvoiceNo.val('');
            }
        });

        invoiceApp.filterControls.ddlOperator.change(function () {
            invoiceApp.filterControls.ddlLicensee.html('');
            invoiceApp.filterControls.ddlLicensee.append('<option value="0">Please Select One!</option>');

            $('#divInvoiceType').html('<a class="ui-multiselect ui-multiselect-widget ui-multiselect-state-default" id="ddlInvoiceType" style="width: 170px;" parent-checked-value="4"><input type="text" title="" value="Please Select One!" class="ui-multiselect-state-default" readonly="readonly" style="width: 157px;"><span class="ui-multiselect-icon ui-multiselect-icon-triangle-1-s"></span></a>');
            invoiceApp.filterControls.txtInvoiceNo.val('');

            if (invoiceApp.filterControls.ddlOperator.prop("selectedIndex") != 0) {
                invoiceApp.getLicensees();
            }

            invoiceApp.getInvoiceNo();
        });

        invoiceApp.filterControls.txtInvoiceNo.change(function () {
            invoiceApp.filterControls.btnGenerate.removeClass('ui-state-disabled');
            $('#saveID').addClass('ui-state-disabled').attr('disabled', 'disabled');
            $('#reviewID').addClass('ui-state-disabled').attr('disabled', 'disabled');
        });

        invoiceApp.filterControls.ddlCurrency.change(function () {
            invoiceApp.filterControls.btnGenerate.removeClass('ui-state-disabled');
            $('#saveID').addClass('ui-state-disabled').attr('disabled', 'disabled');
            $('#reviewID').addClass('ui-state-disabled').attr('disabled', 'disabled');
        });

        //------binding data

        invoiceApp.filterControls.ddlOperator.append('<option value="0">Please Select One!</option>');
        $.each(operatorListModel, function (i, item) {
            invoiceApp.filterControls.ddlOperator.append('<option value="' + item.PartyID + '">' + item.PartyFullName + '</option>');
        });
        invoiceApp.filterControls.ddlLicensee.append('<option value="0">Please Select One!</option>');
        $('#divInvoiceType').html('<a class="ui-multiselect ui-multiselect-widget ui-multiselect-state-default" id="ddlInvoiceType" style="width: 170px;" parent-checked-value="4"><input type="text" title="" value="Please Select One!" class="ui-multiselect-state-default" readonly="readonly" style="width: 157px;"><span class="ui-multiselect-icon ui-multiselect-icon-triangle-1-s"></span></a>');
        NapAres.NapAresPopUp = $('#dialog-popup');
    },

    saveInvoice: function () {
        invoiceApp.updateItem();
        invoiceInfoModel.Operator = invoiceApp.Operator;
        invoiceInfoModel.Licensee = invoiceApp.Licensee;
        invoiceInfoModel.FromDate = invoiceApp.FromDate;
        invoiceInfoModel.ToDate = invoiceApp.ToDate;
        invoiceInfoModel.DueDate = invoiceApp.DueDate;
        invoiceInfoModel.Currency = invoiceApp.Currency.CurrencyID;
        invoiceInfoModel.CurrencyName = invoiceApp.Currency.CurrencyName;
        invoiceInfoModel.InvoiceType = invoiceApp.InvoiceType;
        invoiceInfoModel.Product = invoiceApp.Products;
        invoiceInfoModel.ProductsName = invoiceApp.ProductsName;
        invoiceInfoModel.InvoiceNo = invoiceApp.InvoiceNo;
        invoiceInfoModel.InvoiceName = invoiceApp.InvoiceName;
        invoiceInfoModel.TotalAmount = invoiceApp.TotalAmount;
        var message = '';
        $.ajax({
            url: domain + "Invoice/Invoice/saveInvoice",
            data: '{"invoiceModel":' + JSON.stringify(invoiceInfoModel) + '}',
            contentType: 'application/json',
            type: "POST",
            success: function (data, textStatus, jqXHR) {
                if (data) {
                    if (data.ErrorCode == 0) {
                        $('#saveID').addClass('ui-state-disabled').attr('disabled', 'disabled');
                        invoiceApp.filterControls.btnGenerate.addClass('ui-state-disabled');
                        $('#pdfID').removeClass('ui-state-disabled').removeAttr('disabled');

                        invoiceApp.filterControls.ddlOperator.attr('disabled', 'disabled');
                        invoiceApp.filterControls.ddlLicensee.attr('disabled', 'disabled');
                        invoiceApp.filterControls.cldFromDate.attr('disabled', 'disabled').addClass('ui-state-disabled');
                        invoiceApp.filterControls.cldToDate.attr('disabled', 'disabled').addClass('ui-state-disabled');
                        invoiceApp.filterControls.divInvoiceType.find('input').attr('disabled', 'disabled').addClass('ui-state-disabled');
                        invoiceApp.filterControls.ddlCurrency.attr('disabled', 'disabled');
                        invoiceApp.filterControls.txtInvoiceNo.attr('disabled', 'disabled');

                        invoiceApp.InvoiceID = data.ErrorMessage;
                        invoiceInfoModel.InvoiceID = invoiceApp.InvoiceID;
                        OrgInvoiceInfoModel.InvoiceID = invoiceApp.InvoiceID;
                        NapAres.Helper.showMsgBox({
                            ErrorCode: 0,
                            ErrorMsg: 'Saving invoice successfully.'
                        });
                        invoiceInfoModel = [];
                        invoiceInfoModel = $.extend(true, {}, OrgInvoiceInfoModel);
                    } else {
                        NapAres.Helper.showMsgBox({
                            ErrorCode: data.ErrCode,
                            ErrorMsg: data.ErrorMessage
                        });
                        setTimeout('NapAres.Helper.hideMsgBox()', invoiceApp.messageTimeOut);
                    }
                } else {
                    NapAres.Helper.showMsgBox({
                        ErrorCode: 1,
                        ErrorMsg: 'Saving invoice unsuccessfully.'
                    });
                    setTimeout('NapAres.Helper.hideMsgBox()', invoiceApp.messageTimeOut);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                NapAres.Helper.showMsgBox({
                    ErrorCode: 1,
                    ErrorMsg: 'Saving invoice unsuccessfully.'
                });
                setTimeout('NapAres.Helper.hideMsgBox()', invoiceApp.messageTimeOut);
            },
            complete: function (jqXHR, textStatus) {
                NapAres.enableInput();
            }
        });

        setTimeout('NapAres.Helper.hideMsgBox()', invoiceApp.messageTimeOut);
    },

    addItem: function (isDiscount) {
        var displayID = invoiceInfoModel.StatementList.length + 1;
        for (var i = 0; i < invoiceApp.IdNewItemList.length; i++) {
            var id = invoiceApp.IdNewItemList[i];
            var amount = $('#newItem_text_' + id).val();
            if ($('#newItem_display_' + id).attr('discountItem') == "false") {
                displayID++;
            }
        }
        if (invoiceApp.isInsert) {
            $('#invoiceTbl tr.total').before(' <tr id="newItem_' + invoiceApp.IdNewItem + '" style="width: 100%; text-align: center;">\
                                    <td   style="width: 10%; "><div discountItem="' + (isDiscount == true ? true : false) + '"  id="newItem_display_' + invoiceApp.IdNewItem + '" style="  font-size: 20px;display: inline-block;">' + (isDiscount == true ? "" : displayID) + ' </div> <div style="display: inline-block;" title="Delete"  class="delete" onclick="invoiceApp.deleteItem(' + invoiceApp.IdNewItem + ')"></div></td>\
                                <td style="width: 55%; text-align: center; height: 60px;">\
                                    <textarea  onfocus="invoiceApp.helper.onFocusInput(this)" onblur="invoiceApp.helper.onBlurInput(this)"  id="newItem_des_' + invoiceApp.IdNewItem + '" rows="2" cols="106" name="styled-textarea" id="styled_300"  onchange=" invoiceApp.allowUpdateNewInfo(this);"></textarea></td>\
                                <td style="width: 20%; text-align: center;"><textarea  onfocus="invoiceApp.helper.onFocusInput(this)" onblur="invoiceApp.helper.onBlurInput(this)" id="newItem_ref_' + invoiceApp.IdNewItem + '" rows="2" cols="33" name="styled-textarea"  onchange=" invoiceApp.allowUpdateNewInfo(this);"></textarea></td>\
                                <td style="width: 15%; text-align: center;">\
                                    <div style="width: 90%; height: 20px; margin: -20px 5px 0px 10px;text-align: right;">\
                                        <input class="numeric"  name=""  onblur="invoiceApp.recalculateTotal();" id="newItem_text_' + invoiceApp.IdNewItem + '" type="text" style="font-weight: bold;text-align:right;width:95%;"></input></div></td></tr>');
            invoiceApp.IdNewItemList.push(invoiceApp.IdNewItem);
            invoiceApp.IdNewItem = invoiceApp.IdNewItem + 1;
            $(".numeric").numeric();
            $('#saveID').addClass('ui-state-disabled').attr('disabled', 'disabled');
            $('#reviewID').addClass('ui-state-disabled').attr('disabled', 'disabled');
            $('#pdfID').addClass('ui-state-disabled').attr('disabled', 'disabled');
        }
    },

    deleteItem: function (trId) {
        $("#newItem_" + trId).remove();
        invoiceApp.IdNewItemList = jQuery.grep(invoiceApp.IdNewItemList, function (value) {
            return value != trId;
        });
        if (invoiceApp.IdNewItemList.length == 0) {
            $('#saveID').addClass('ui-state-disabled').attr('disabled', 'disabled');
            $('#pdfID').removeClass('ui-state-disabled').removeAttr('disabled');
        } else {
            $('#saveID').removeClass('ui-state-disabled').removeAttr('disabled');
            $('#pdfID').addClass('ui-state-disabled').attr('disabled', 'disabled');
        }

        invoiceApp.recalculateTotal();
    },

    recalculateTotal: function () {
        NapAres.Helper.hideMsgBox();
        var totalAmountTmp = _totalAmount;
        var index = invoiceInfoModel.StatementList.length + 1;

        for (var i = 0; i < invoiceApp.IdNewItemList.length; i++) {
            var id = invoiceApp.IdNewItemList[i];

            var amount = $('#newItem_text_' + id).val();
            if ($.isNumeric(amount) == true) {
                if ($('#newItem_display_' + id).attr('discountItem') == "true") {
                    $('#newItem_display_' + id).attr('amount', parseInt(amount) > 0 ? (parseInt(amount) * -1) : parseInt(amount));
                } else {
                    $('#newItem_display_' + id).attr('amount', amount);
                }
            }
        }

        for (var i = 0; i < invoiceApp.IdNewItemList.length; i++) {
            var id = invoiceApp.IdNewItemList[i];

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
                    setTimeout('NapAres.Helper.hideMsgBox()', invoiceApp.messageTimeOut);
                    invoiceApp.isInsert = false;
                    $('#saveID').addClass('ui-state-disabled').attr('disabled', 'disabled');
                    $('#reviewID').addClass('ui-state-disabled').attr('disabled', 'disabled');
                    $('#pdfID').addClass('ui-state-disabled').attr('disabled', 'disabled');

                    return false;
                }
                NapAres.Helper.showMsgBox({
                    ErrorCode: 1,
                    ErrorMsg: 'Please just input numberic !'
                });
                setTimeout('NapAres.Helper.hideMsgBox()', invoiceApp.messageTimeOut);
                invoiceApp.isInsert = false;
                $('#saveID').addClass('ui-state-disabled').attr('disabled', 'disabled');
                $('#reviewID').addClass('ui-state-disabled').attr('disabled', 'disabled');
                $('#pdfID').addClass('ui-state-disabled').attr('disabled', 'disabled');

                return false;
            }
            if (amount != '') {
                var formatNumber = '';
                if ($('#newItem_display_' + id).attr('discountItem') == "true") {
                    $('#newItem_text_' + id).addClass("red");
                    formatNumber = "(" + numeral(parseFloat(amount) > 0 ? parseFloat(amount) : (parseFloat(amount) * -1)).format(FormatConstant.TWO_DECIMAL_NUMBER) + ")";
                } else {
                    if (parseInt(amount) < 0) {
                        $('#newItem_text_' + id).addClass("red");
                        formatNumber = "(" + numeral(parseFloat(amount) * -1).format(FormatConstant.TWO_DECIMAL_NUMBER) + ")";
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

        $("#totalAmount").empty().html(invoiceApp.Currency.CurrencyName + ' ' + invoiceApp.helper.negativeFormat(totalAmountTmp));
        invoiceInfoModel.TotalAmount = invoiceApp.TotalAmount = totalAmountTmp;
        $('#reviewID').removeClass('ui-state-disabled').removeAttr('disabled');
        $('#saveID').removeClass('ui-state-disabled').removeAttr('disabled');

        invoiceApp.isInsert = true;
    },

    updateItem: function () {
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

        for (var i = 0; i < invoiceApp.IdNewItemList.length; i++) {
            var id = invoiceApp.IdNewItemList[i];
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
        }
        if (statement.StatementDetailList.length > 0) {
            invoiceInfoModel.StatementList.push(statement);
        }
        invoiceInfoModel.Note = $('#noteId').val().replace(/\n/g, '<br/>');
        return invoiceInfoModel.StatementList;
    },

    allowUpdateNewInfo: function (item) {
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

    mappingSpecialPartyName: function (partyName) {
        if (partyName.toLowerCase() == 'cozni') {
            partyName = 'CL';
        }
        return partyName
    }
}


var invoiceApp = invoiceApp || {};
invoiceApp.helper = {};
(function () {
    this.getDateRangeOfLastMonth = function (today) {
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
            endDate = new Date(currentYear, month, this.getTotalDayOfMonth(month + 1, currentYear));
        }
        return [startDate, endDate];
    }

    this.getTotalDayOfMonth = function (month, year) {
        //// Note Month fomr 1 > 12
        var m = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
        if (month != 2) return m[month - 1];
        if (year % 4 != 0) return m[1];
        if (year % 100 == 0 && year % 400 != 0) return m[1];
        return m[1] + 1;
    }

    this.negativeFormat = function (input) {
        var output = '';
        output = input >= 0 ? "<span style=\"font-weight: bold; \">" + numeral(input).format(FormatConstant.TWO_DECIMAL_NUMBER) + "</span>" : "<span style=\"color: red;font-weight: bold;\">(" + numeral(input * -1).format(FormatConstant.TWO_DECIMAL_NUMBER) + ")</span>";
        return output;
    }

    this.onFocusInput = function (obj) {
        obj.style.background = '#e5fff3';
    }

    this.onBlurInput = function (obj) {
        obj.style.background = 'white';
    }
}).apply(invoiceApp.helper);