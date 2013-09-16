var invoiceApp = invoiceApp || {};
invoiceApp.filter = {};
(function () {
    this.getInvoiceType = function () {
        NapAres.disableInput();
        $.ajax({
            url: domain + "Invoice/Invoice/getInvoiceType",
            type: "POST",
            data: 'licenseeID=' + invoiceApp.filterControls.ddlLicensee.val() + '&partyId=' + invoiceApp.filterControls.ddlOperator.val() ,
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
                    minWidth: 200,
                    showFooter: false,
                    listData: invoiceTypeList,
                    hasOptGroup: true,
                    onCheck: function () {
                         invoiceApp.filter.getBaseCurrency();  
                            invoiceApp.filterControls.btnGenerate.removeClass('ui-state-disabled');
                            $('#saveID').addClass('ui-state-disabled').attr('disabled', 'disabled');
                            $('#reviewID').addClass('ui-state-disabled').attr('disabled', 'disabled');
                    }, // when an individual checkbox is clicked 
                    selectedList: 0,
                    isCheckAll: false,
                    isRetrieveDataAfterClose: false,
                    showParentSelectedText: true,
                    multiple: false,
                    shadow: true
                });

                 invoiceApp.filter.getBaseCurrency();
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

    this.getInvoiceNo = function () {
        NapAres.disableInput();
        this.getSelectedInvoiceType();
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
    }

    this.getBaseCurrency = function(){
       NapAres.disableInput();
       invoiceApp.filter.getSelectedInvoiceType();
                            $.ajax({
                                url: domain + "Invoice/Invoice/GetBaseCurrency",
                                type: "POST",
                                data: 'licenseeID=' + invoiceApp.filterControls.ddlLicensee.val() + '&partyId=' + invoiceApp.filterControls.ddlOperator.val() + '&Products=' + invoiceApp.Products,
                                success: function (data) {
                                     if (data && data.length > 0) {
                                         $(invoiceApp.filterControls.ddlCurrency).find('option').remove();
                                        $.each(data, function (i, item) {
                                            invoiceApp.filterControls.ddlCurrency.append('<option value="' + item.Value + '">' + item.Text + '</option>');
                                        });
                                        if(data.length == 1)
                                        {
                                            $(invoiceApp.filterControls.ddlCurrency).attr('disabled', 'disabled');
                                        }
                                    }
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

    this.getLicensees = function () {
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
    }

    this.getFilteringInfo = function () {
        var Operator = operatorListModel[invoiceApp.filterControls.ddlOperator.prop("selectedIndex") - 1];
        var Licensee = invoiceApp.Licensees[invoiceApp.filterControls.ddlLicensee.prop("selectedIndex") - 1];

        this.getSelectedInvoiceType();

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
        invoiceApp.InvoiceName = invoiceApp.mappingSpecialPartyName(Operator.PartyName) + ' INV-' + (invoiceApp.InvoiceNo > 9 ? invoiceApp.InvoiceNo : '0' + invoiceApp.InvoiceNo);
        
    }

    this.getNewInvoiceData =  function () {
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
    }

    this.getProductId = function () {
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
    }

    this.getSelectedInvoiceType = function () {
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
    }

}).apply(invoiceApp.filter);