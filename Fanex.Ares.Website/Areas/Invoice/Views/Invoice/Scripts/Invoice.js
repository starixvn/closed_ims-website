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
    newItemID: 0,
    messageTimeOut: 4000,
    isInsert: true,
    newItemIDsList: [],

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
        var filterControl = invoiceApp.filterControls;
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
                filterControl.btnGenerate.removeClass('ui-state-disabled');
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
                invoiceApp.DueDate = filterControl.cldDueDate.val();
                $('#saveID').removeClass('ui-state-disabled').removeAttr('disabled');
                $('#reviewID').removeClass('ui-state-disabled').removeAttr('disabled');
                $('#pdfID').addClass('ui-state-disabled').attr('disabled', 'disabled');
            },
            minDate: new Date(minDate)
        });

        filterControl.cldFromDate = $('#FromDatePicker');
        filterControl.cldToDate = $('#ToDatePicker');
        filterControl.cldDueDate = $('#DueDatePicker');

        filterControl.btnGenerate.button();
        filterControl.btnGenerate.click(function () {
            if (!$(this).hasClass('ui-state-disabled')) {
                invoiceApp.submit();
            }
        });

        filterControl.btnNewInvoice.button();

        filterControl.ddlLicensee.change(function () {
            if (filterControl.ddlLicensee.prop("selectedIndex") != 0) {
                invoiceApp.filter.getInvoiceType();
                filterControl.btnGenerate.removeClass('ui-state-disabled');
                $('#saveID').addClass('ui-state-disabled').attr('disabled', 'disabled');
                $('#reviewID').addClass('ui-state-disabled').attr('disabled', 'disabled');
            } else {
                filterControl.divInvoiceType.html('<a class="ui-multiselect ui-multiselect-widget ui-multiselect-state-default" id="ddlInvoiceType" style="width: 170px;" parent-checked-value="4"><input type="text" title="" value="Please Select One!" class="ui-multiselect-state-default" readonly="readonly" style="width: 157px;"><span class="ui-multiselect-icon ui-multiselect-icon-triangle-1-s"></span></a>');
                filterControl.txtInvoiceNo.val('');
            }
        });

        filterControl.ddlOperator.change(function () {
            filterControl.ddlLicensee.html('');
            filterControl.ddlLicensee.append('<option value="0">Please Select One!</option>');

            $('#divInvoiceType').html('<a class="ui-multiselect ui-multiselect-widget ui-multiselect-state-default" id="ddlInvoiceType" style="width: 170px;" parent-checked-value="4"><input type="text" title="" value="Please Select One!" class="ui-multiselect-state-default" readonly="readonly" style="width: 157px;"><span class="ui-multiselect-icon ui-multiselect-icon-triangle-1-s"></span></a>');
            filterControl.txtInvoiceNo.val('');

            if (filterControl.ddlOperator.prop("selectedIndex") != 0) {
                invoiceApp.filter.getLicensees();
            }

            invoiceApp.filter.getInvoiceNo();
            $(invoiceApp.filterControls.ddlCurrency).find('option').remove();
        });

        filterControl.txtInvoiceNo.change(function () {
            filterControl.btnGenerate.removeClass('ui-state-disabled');
            $('#saveID').addClass('ui-state-disabled').attr('disabled', 'disabled');
            $('#reviewID').addClass('ui-state-disabled').attr('disabled', 'disabled');
        });

        filterControl.ddlCurrency.change(function () {
            filterControl.btnGenerate.removeClass('ui-state-disabled');
            $('#saveID').addClass('ui-state-disabled').attr('disabled', 'disabled');
            $('#reviewID').addClass('ui-state-disabled').attr('disabled', 'disabled');
        });

        //------binding data

        filterControl.ddlOperator.append('<option value="0">Please Select One!</option>');
        $.each(operatorListModel, function (i, item) {
            filterControl.ddlOperator.append('<option value="' + item.PartyID + '">' + item.PartyFullName + '</option>');
        });
        filterControl.ddlLicensee.append('<option value="0">Please Select One!</option>');
        $('#divInvoiceType').html('<a class="ui-multiselect ui-multiselect-widget ui-multiselect-state-default" id="ddlInvoiceType" style="width: 170px;" parent-checked-value="4"><input type="text" title="" value="Please Select One!" class="ui-multiselect-state-default" readonly="readonly" style="width: 157px;"><span class="ui-multiselect-icon ui-multiselect-icon-triangle-1-s"></span></a>');
        NapAres.NapAresPopUp = $('#dialog-popup');
    },

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

        invoiceApp.filter.getFilteringInfo();

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
                        invoiceApp.filter.getNewInvoiceData();
                    },
                    Cancel: function () {
                        $(this).dialog("close");
                    }
                }
            });
            $(".ui-dialog-titlebar").hide();
        } else {
            invoiceApp.filter.getNewInvoiceData();
        }
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

        for (var i = 0; i < invoiceApp.newItemIDsList.length; i++) {
            var id = invoiceApp.newItemIDsList[i];
            var amount = $('#newItem_text_' + id).val();
            if (amount == '') {
                NapAres.Helper.showMsgBox({
                    ErrorCode: 1,
                    ErrorMsg: 'Please input value !'
                });
                return false;
            }
            if ($('#newItem_display_' + id).attr('discountItem') == "false") {
                displayID++;
            }
        }

        if (invoiceApp.isInsert) {
            $('#invoiceTbl tr.total').before(' <tr id="newItem_' + invoiceApp.newItemID + '" style="width: 100%; text-align: center;">\
                                    <td   style="width: 10%; "><div discountItem="' + (isDiscount == true ? true : false) + '"  id="newItem_display_' + invoiceApp.newItemID + '" style="  font-size: 20px;display: inline-block;">' + (isDiscount == true ? "" : displayID) + ' </div> <div style="display: inline-block;" title="Delete"  class="delete" onclick="invoiceApp.deleteItem(' + invoiceApp.newItemID + ')"></div></td>\
                                <td style="width: 55%; text-align: center; height: 60px;">\
                                    <textarea  onfocus="invoiceApp.helper.onFocusInput(this)" onblur="invoiceApp.helper.onBlurInput(this)"  id="newItem_des_' + invoiceApp.newItemID + '" rows="2" cols="106" name="styled-textarea" id="styled_300"  onchange=" invoiceApp.allowUpdateNewInfo(this);"></textarea></td>\
                                <td style="width: 20%; text-align: center;"><textarea  onfocus="invoiceApp.helper.onFocusInput(this)" onblur="invoiceApp.helper.onBlurInput(this)" id="newItem_ref_' + invoiceApp.newItemID + '" rows="2" cols="33" name="styled-textarea"  onchange=" invoiceApp.allowUpdateNewInfo(this);"></textarea></td>\
                                <td style="width: 15%; text-align: center;">\
                                    <div style="width: 90%; height: 20px; margin: 0px 5px 0px 10px;text-align: right;">\
                                        <input class="numeric"  name=""  onblur="invoiceApp.recalculateTotal();" id="newItem_text_' + invoiceApp.newItemID + '" type="text" style="font-weight: bold;text-align:right;width:95%;"></input></div></td></tr>');
            invoiceApp.newItemIDsList.push(invoiceApp.newItemID);
            invoiceApp.newItemID = invoiceApp.newItemID + 1;
            $(".numeric").numeric();
            $('#saveID').addClass('ui-state-disabled').attr('disabled', 'disabled');
            $('#reviewID').addClass('ui-state-disabled').attr('disabled', 'disabled');
            $('#pdfID').addClass('ui-state-disabled').attr('disabled', 'disabled');
        }
    },

    deleteItem: function (trId) {
        $("#newItem_" + trId).remove();
        invoiceApp.newItemIDsList = jQuery.grep(invoiceApp.newItemIDsList, function (value) {
            return value != trId;
        });
        if (invoiceApp.newItemIDsList.length == 0) {
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

        for (var i = 0; i < invoiceApp.newItemIDsList.length; i++) {
            var id = invoiceApp.newItemIDsList[i];

            var amount = $('#newItem_text_' + id).val();
            if ($.isNumeric(amount) == true) {
                if ($('#newItem_display_' + id).attr('discountItem') == "true") {
                    $('#newItem_display_' + id).attr('amount', parseInt(amount) > 0 ? (parseInt(amount) * -1) : parseInt(amount));
                } else {
                    $('#newItem_display_' + id).attr('amount', amount);
                }
            }
        }

        for (var i = 0; i < invoiceApp.newItemIDsList.length; i++) {
            var id = invoiceApp.newItemIDsList[i];

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

        for (var i = 0; i < invoiceApp.newItemIDsList.length; i++) {
            var id = invoiceApp.newItemIDsList[i];
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

            statementDetail.statementDetail[0].Description = $('textarea#newItem_des_' + id).val();
            statementDetail.statementDetail[0].Reference = $('textarea#newItem_ref_' + id).val();
            statementDetail.statementDetail[0].Amount = $('#newItem_display_' + id).attr('amount');
            statementDetail.statementDetail[0].DiscountItem = $('#newItem_display_' + id).attr('discountItem');
            statementDetail.statementDetail[0].ItemType = 0;
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
    },

    viewDetail: function (fromDate, toDate) {
        NapAres.disableInput();
        invoiceApp.updateItem();
        invoiceInfoModel.Operator = invoiceApp.Operator;
        invoiceInfoModel.Licensee = invoiceApp.Licensee;
        invoiceInfoModel.FromDate = fromDate;
        invoiceInfoModel.ToDate = toDate;
        invoiceInfoModel.DueDate = invoiceApp.DueDate;
        invoiceInfoModel.Currency = invoiceApp.Currency.CurrencyID;
        invoiceInfoModel.CurrencyName = invoiceApp.Currency.CurrencyName;
        invoiceInfoModel.InvoiceType = invoiceApp.InvoiceType;
        invoiceInfoModel.Product = invoiceApp.Products;
        invoiceInfoModel.ProductsName = invoiceApp.ProductsName;
        invoiceInfoModel.InvoiceNo = invoiceApp.InvoiceNo;
        invoiceInfoModel.InvoiceName = invoiceApp.InvoiceName;
        $.ajax({
            url: domain + "Invoice/Invoice/Detail",
            data: '{"invoice":' + JSON.stringify(invoiceInfoModel) + '}',
            contentType: 'application/json',
            type: "POST",
            success: function (data, textStatus, jqXHR) {
                if (data) {
                    var win = window.open((domain + 'Invoice/Invoice/ReviewInvoice'), ('invoice' + new Date().getMilliseconds()));

                    if ($.browser.msie) {
                        win.load = new function () {
                            win.document.write(data);
                            win.document.title = invoiceApp.InvoiceName;
                        }
                    }
                    else {
                        $(win).load(function () {
                            win.document.write(data);
                            win.document.title = invoiceApp.InvoiceName;
                        });
                    }
                }
            },
            error: function () {
                NapAres.Helper.showMsgBox({ ErrorCode: 1, ErrorMsg: 'Fail' });
                setTimeout('NapAres.Helper.hideMsgBox()', invoiceApp.messageTimeOut);
                NapAres.enableInput();
            }
        });
        NapAres.enableInput();
    },

    previewInvoice: function () {
        NapAres.disableInput();
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
        invoiceInfoModel.IsSignature = true;
        $.ajax({
            url: domain + "Invoice/Invoice/PrintScreen",
            data: '{"invoice":' + JSON.stringify(invoiceInfoModel) + '}',
            contentType: 'application/json',
            type: "POST",
            success: function (data, textStatus, jqXHR) {
                if (data) {
                    var win = window.open((domain + 'Invoice/Invoice/ReviewInvoice'), ('invoice' + new Date().getMilliseconds()));

                    if ($.browser.msie) {
                        win.load = new function () {
                            win.document.write(data);
                            win.document.title = invoiceApp.InvoiceName;
                        }
                    }
                    else {
                        $(win).load(function () {
                            win.document.write(data);
                            win.document.title = invoiceApp.InvoiceName;
                        });
                    }
                }
            },
            error: function () {
                NapAres.Helper.showMsgBox({ ErrorCode: 1, ErrorMsg: 'Fail' });
                setTimeout('NapAres.Helper.hideMsgBox()', invoiceApp.messageTimeOut);
                NapAres.enableInput();
            }
        });
        NapAres.enableInput();
        invoiceInfoModel = [];
        invoiceInfoModel = $.extend(true, {}, OrgInvoiceInfoModel);
    }
}
