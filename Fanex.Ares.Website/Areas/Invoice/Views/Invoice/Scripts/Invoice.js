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
                        Checked: (i == 0 ? true : false),
                        Enable: true,
                        CssClass: ""
                    });
                }
                invoiceTypeList.push({
                    Id: "#ddlInvoiceType" + result[i].InvoiceTypeID,
                    Value: result[i].InvoiceTypeID,
                    Text: result[i].InvoiceTypeName,
                    Checked: (i==0 ? true: false) ,
                    Enable: true,
                    CssClass: "",
                    Items: productList
                });

                productList = [];
            }

            $("#ddlInvoiceType").multiRadioSelect(
            { //multiple: false,
                showCheckAll: false,
                maxHeight: 705,
                minWidth: 170,
                showFooter:false,
                listData: invoiceTypeList,
                hasOptGroup: true,
                onLeave: function () {
                
                }, // when all selected is made and datalist collapsed
                onCheck: function () {
                  
                }, // when an individual checkbox is clicked 
                onOpen: function () { }, // when the select menu is opened 
                onCheckAll: function () {
                  
                }, // when the check all link is clicked 
                onUncheckAll: function () {
                  
                }, // when the uncheck all link is clicked 
                selectedList: 0,
                isCheckAll: false,
                isRetrieveDataAfterClose: false,
                showParentSelectedText: true,
                multiple: false
                //selectedList: 4
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
            NapAres.Helper.showMsgBox({ ErrorCode: 1, ErrorMsg: 'Loading InvoiceNo fail.' });
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
    invoiceInfoModel: [],
    isInsert: true,
    IdNewItemList: [],
    init: function () {
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
                TextboxCss: "edit-field col_6"
            },
            onSelectDatePicker: function () {
                CreatingInvoice.DueDate = $('#DueDatePicker').val();
                $('#saveID').removeClass('ui-state-disabled');
                //            $('#btnGenerate').addClass('ui-state-disabled');
                $('#pdfID').addClass('ui-state-disabled');
            }
        });

        $('#btnGenerate').button();
        $('#btnGenerate').click(function () { if (!$(this).hasClass('ui-state-disabled')) Submit(); });

        $('#btnNewInvoice').button();
        //        $('#btnNewInvoice').click(function () {
        //           

        //        });

        $("#ddlLicensee").change(function () {
            GetInvoiceType();
        });

        $("#ddlOperator").change(function () {
            CreatingInvoice.GetLicensees();
        });

        //------binding data

        $('#ddlOperator').append('<option value="0">Please Select One !</option>');
        $.each(operatorListModel, function (i, item) {
            $('#ddlOperator').append('<option value="'
                                 + item.PartyID
                                 + '">'
                                 + item.PartyFullName
                                 + '</option>');
        });


        CreatingInvoice.GetLicensees();

        NapAres.NapAresPopUp = $('#dialog-popup');
    },
    GetLicensees: function () {
        NapAres.disableInput();
        $('#ddlLicensee').html('');
        $.ajax({
            url: domain + "Invoice/Invoice/GetLicenseesByParty",
            type: "POST",
            data: 'PartyId=' + $('#ddlOperator').val(),
            success: function (data, textStatus, jqXHR) {
                if (data && data.length > 0) {
                    //                    $('#ddlLicensee').append('<option value="0">Please Select a Licensee</option>');
                    $.each(data, function (i, item) {
                        $('#ddlLicensee').append('<option value="'
                                 + item.LicenseeID
                                 + '">'
                                 + item.BrandName
                                 + '</option>');
                    });
                }
                GetInvoiceType();
                CreatingInvoice.Licensees = data;
            },
            error: function (xhr, ajaxOptions, thrownError) {
                NapAres.Helper.showMsgBox({ ErrorCode: 1, ErrorMsg: 'Loading Licensee data fail.' });
                setTimeout('NapAres.Helper.hideMsgBox()', CreatingInvoice.messageTimeOut);
            },
            complete: function (jqXHR, textStatus) {
                NapAres.enableInput();
            }
        });


    },
    GetFilteringInfo: function () {
        var listChecked = $('#ddlInvoiceType').next('div.ui-multiselect-options').find('label:not(.ui-multiselect-state-disabled,.ui-multiselect-group-mark)').find('input:not(#' + $('#ddlProduct').attr('id') + "HeaderCheckBox)").filter('[checked]');
        var productName = '';
        var produtID = '';
        var index = 0;
        for (var i = 0; i < listChecked.length; i++) {
            if (index < listChecked.length - 1) {
                productName += listChecked[i].title + ',';
                produtID += listChecked[i].value + ',';
            }
            else {
                productName += listChecked[i].title;
                produtID += listChecked[i].value;
            }
            index++;
        }

        var radioCheck = $("#divInvoiceType input[type='radio']:checked");

        var InvoiceTypeIdTmp = radioCheck[0].value;
        var InvoiceTypeNameTmp = radioCheck[0].title;

        var Operator = operatorListModel[$("#ddlOperator").prop("selectedIndex")];
        var Licensee = CreatingInvoice.Licensees[$("#ddlLicensee").prop("selectedIndex")];

        CreatingInvoice.Operator = Operator;
        CreatingInvoice.Licensee = Licensee;
        CreatingInvoice.FromDate = $('#FromDatePicker').val();
        CreatingInvoice.ToDate = $('#ToDatePicker').val();
        CreatingInvoice.DueDate = $('#DueDatePicker').val();
        CreatingInvoice.InvoiceType = { InvoiceTypeID: InvoiceTypeIdTmp, InvoiceTypeName: InvoiceTypeNameTmp };
        CreatingInvoice.Currency = { CurrencyID: $('#CurrencyID').val(), CurrencyName: $('#CurrencyID option:selected').text() };
        CreatingInvoice.InvoiceNo = $('#invoiceNo').val();
        CreatingInvoice.InvoiceName = Operator.PartyName + ' INV ' + (CreatingInvoice.InvoiceNo > 9 ? CreatingInvoice.InvoiceNo : '0' + CreatingInvoice.InvoiceNo);
        CreatingInvoice.Products = produtID;
        CreatingInvoice.ProductsName = productName;
    },
    CheckIsExistInvoiceNo: function () {
        CreatingInvoice.GetFilteringInfo();
        $.ajax({
            url: domain + "Invoice/Invoice/IsExistInvoiceNo",
            type: "POST",
            data: 'PartyID=' + CreatingInvoice.Operator.PartyID + '&LicenseeID=' + CreatingInvoice.Licensee.LicenseeID +
                        '&InvoiceNo=' + CreatingInvoice.InvoiceNo + '&InvoiceType=' + CreatingInvoice.InvoiceType.InvoiceTypeID,
            success: function (data, textStatus, jqXHR) {
                return data;
            },
            error: function (xhr, ajaxOptions, thrownError) {
                NapAres.Helper.showMsgBox({ ErrorCode: 1, ErrorMsg: 'Loading data fail.' });
                setTimeout('NapAres.Helper.hideMsgBox()', CreatingInvoice.messageTimeOut);
                return false;
            },
            complete: function (jqXHR, textStatus) {
                NapAres.enableInput();
            }
        });
    },
    GetNewInvoiceData: function () {
        NapAres.disableInput();
        var isExistInvoiceNo = CreatingInvoice.CheckIsExistInvoiceNo();
        if (isExistInvoiceNo) {
            $.ajax({
                url: domain + "Invoice/Invoice/GetInvoice",
                type: "POST",
                data: 'PartyID=' + CreatingInvoice.Operator.PartyID + '&LicenseeID=' + CreatingInvoice.Licensee.LicenseeID +
                        '&Products=' + CreatingInvoice.Products + '&FromDate=' + CreatingInvoice.FromDate + '&ToDate=' + CreatingInvoice.ToDate +
                        '&BillingCurrency=' + CreatingInvoice.Currency.CurrencyID,
                success: function (data, textStatus, jqXHR) {
                    $('#parentContainer').html(data);
                    CreatingInvoice.IdNewItemList = [];
                    CreatingInvoice.IdNewItem = invoiceInfoModel.StatementList.length + 1;
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    NapAres.Helper.showMsgBox({ ErrorCode: 1, ErrorMsg: 'Loading data fail.' });
                    setTimeout('NapAres.Helper.hideMsgBox()', CreatingInvoice.messageTimeOut);
                },
                complete: function (jqXHR, textStatus) {
                    NapAres.enableInput();
                }
            });
        }
        else {
            NapAres.Helper.showMsgBox({ ErrorCode: 1, ErrorMsg: 'Invoice No is existed!' });
            setTimeout('NapAres.Helper.hideMsgBox()', CreatingInvoice.messageTimeOut);
        }
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
                        $('#saveID').addClass('ui-state-disabled');
                        $('#btnGenerate').addClass('ui-state-disabled');
                        $('#pdfID').removeClass('ui-state-disabled');

                        $('#ddlOperator').addClass('ui-state-disabled');
                        $('#ddlLicensee').addClass('ui-state-disabled');
                        $('#dropDownListId1').addClass('ui-state-disabled');
                        $('#FromDatePicker').addClass('ui-state-disabled');
                        $('#ToDatePicker').addClass('ui-state-disabled');
                        $('#divInvoiceType').addClass('ui-state-disabled');
                        $('#CurrencyID').addClass('ui-state-disabled');
                        $('#invoiceNo').addClass('ui-state-disabled');

                        CreatingInvoice.InvoiceID = data.ErrorMessage;
                        invoiceInfoModel.InvoiceID = CreatingInvoice.InvoiceID;
                        NapAres.Helper.showMsgBox({ ErrorCode: 0, ErrorMsg: 'Saving invoice succesffully.' });
                    }
                    else {
                        NapAres.Helper.showMsgBox(data);
                    }
                }
                else {
                    NapAres.Helper.showMsgBox({ ErrorCode: 1, ErrorMsg: 'Saving invoice unsuccesffully.' });
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                NapAres.Helper.showMsgBox({ ErrorCode: 1, ErrorMsg: 'Saving invoice unsuccesffully.' });
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
            }
            else {
                operator.push(listChecked[i].value);
            }
        }
        var result = { 'normal': normalLicensees, 'operator': operator }
        return result;
    },
    AddItem: function (isDiscount) {
        for (var i = 0; i < CreatingInvoice.IdNewItemList.length; i++) {
            var id = CreatingInvoice.IdNewItemList[i];
            var amount = $('#newItem_text_' + id).val();
            if (amount == '') {
                NapAres.Helper.showMsgBox({ ErrorCode: 1, ErrorMsg: 'Please input value !' });
                return false;
            }
        }
        if (CreatingInvoice.isInsert) {
            var displayID = CreatingInvoice.IdNewItemList.length + invoiceInfoModel.StatementList.length + 1;
            //        var trId = "newItem_" + CreatingInvoice.IdNewItem;
            $('#invoiceTbl tr.total').before(' <tr id="newItem_' + CreatingInvoice.IdNewItem + '" style="width: 100%; text-align: center;">\
                                    <td  style="width: 10%; "><span  id="newItem_display_' + CreatingInvoice.IdNewItem + '">' + (isDiscount == true ? "" : displayID) + ' </span></br></br> <span  class="delete" onclick="CreatingInvoice.DeleteItem(' + CreatingInvoice.IdNewItem + ')"></span></td>\
                                <td style="width: 55%; text-align: center; height: 60px;">\
                                    <textarea id="newItem_des_' + CreatingInvoice.IdNewItem + '" rows="2" cols="106" name="styled-textarea" id="styled_300" ></textarea></td>\
                                <td style="width: 20%; text-align: center;"><textarea id="newItem_ref_' + CreatingInvoice.IdNewItem + '" rows="2" cols="33" name="styled-textarea" ></textarea></td>\
                                <td style="width: 15%; text-align: center;">\
                                    <div style="width: 90%; height: 20px; margin: -20px 5px 0px 10px;text-align: right;">\
                                        <input name="" onblur="CreatingInvoice.RecalculatorTotal();" id="newItem_text_' + CreatingInvoice.IdNewItem + '" type="text" style="font-weight: bold;text-align:right;"></input></div></td></tr>');
            CreatingInvoice.IdNewItemList.push(CreatingInvoice.IdNewItem);
            CreatingInvoice.IdNewItem = CreatingInvoice.IdNewItem + 1;
        }
        $('#saveID').removeClass('ui-state-disabled');
    },
    DeleteItem: function (trId) {
        //        $(document.getElementById(trId)).remove();
        $("#newItem_" + trId).remove();
        CreatingInvoice.IdNewItemList = jQuery.grep(CreatingInvoice.IdNewItemList, function (value) {
            return value != trId;
        });
        if (CreatingInvoice.IdNewItemList.length == 0) {
            $('#saveID').addClass('ui-state-disabled')
        }
        else {
            $('#saveID').removeClass('ui-state-disabled')
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

            $('#newItem_display_' + id).empty().html(index);
            index++;
            if ($.isNumeric(amount) == false) {
                $('#saveID').addClass('ui-state-disabled');
                if (amount == '') {
                    NapAres.Helper.showMsgBox({ ErrorCode: 1, ErrorMsg: 'Please input value !' });
                    CreatingInvoice.isInsert = false;
                    return false;
                }
                NapAres.Helper.showMsgBox({ ErrorCode: 1, ErrorMsg: 'Please just input numberic !' });
                CreatingInvoice.isInsert = false;
                return false;
            }
            if (amount != '') {
                //                $('#newItem_text_' + id).empty().val(numeral(amount).format(FormatConstant.TWO_DECIMAL_NUMBER));
                totalAmountTmp += parseFloat(amount);
            }
            //            CreatingInvoice.StatementDetailList.push(statementDetail);
        }
        $("#totalAmount").empty().html(NegativeFormat(totalAmountTmp));
        CreatingInvoice.isInsert = true;
    },
    UpdateItemDisplay: function () {

    },
    UpdateItem: function () {
        var statement = {
            StatementID: 1000,
            StatementDetailList: []
        };
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
            statementDetail.Amount = $('#newItem_text_' + id).val();
            statement.StatementDetailList.push(statementDetail);
            //            CreatingInvoice.StatementDetailList.push(statementDetail);
        }
        invoiceInfoModel.StatementList.push(statement);
        return invoiceInfoModel.StatementList;
    }
}


function NegativeFormat(input) {
     var output = '';
     output = input >= 0 ? "<span style=\"font-weight: bold; \">" + numeral(input).format(FormatConstant.TWO_DECIMAL_NUMBER) + "</span>" : "<span style=\"color: red;font-weight: bold;\">(" + numeral(input * -1).format(FormatConstant.TWO_DECIMAL_NUMBER) + ")</span>";
     return output;
}


