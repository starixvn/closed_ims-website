function validateNumber(event) {
    // Prevent shift key since its not needed
    if (event.shiftKey == true) {
        event.preventDefault();
    }
    // Allow Only: keyboard 0-9, numpad 0-9, backspace, tab, left arrow, right arrow, delete
    if ((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105) || event.keyCode == 8 || event.keyCode == 188 || event.keyCode == 190 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 39 || event.keyCode == 46) {
        // Allow normal operation
    } else {
        // Prevent the rest
        event.preventDefault();
    }
}

var app = angular.module('Invoice',  []).service('invoiceService', function ($rootScope) {
    return {
        getOperatorList: function()
        {
            operatorListService= operatorListModel;
            return operatorListService;
        },
        getInvoiceModel: function()
        {
            invoiceModelService = invoiceInfoModel
            return invoiceModelService;
        },
        setOperatorSelect: function(operator)
        {
           operatorSelected = operator;
        },
        getOperatorSelected: function(){
            return operatorSelected;
        }
    };
    
})

app.controller('filterController', function($scope, invoiceService) {
    $scope.operators = invoiceService.getOperatorList();
    
    $scope.licensees = new Array();

    $scope.invoiceNo = '';
    $scope.baseCurrencies = new Array();

    $scope.products = '';
    $scope.productsName = '';

    $scope.invoiceType = {
            InvoiceTypeID: '',
            InvoiceTypeName: ''
        };

     $scope.getLicensees = function(){
       NapAres.disableInput();
       
       var partyIdTmp =  $scope.operator == null ? -1 :  $scope.operator.PartyID ;
        $.ajax({
            url: domain + "Invoice/Invoice/getLicenseesByParty",
            type: "POST",
            data: 'PartyId=' + partyIdTmp,
            success: function (data, textStatus, jqXHR) {
                    $scope.licensees = data;
                    if(data.length == 0){
                         $('#divInvoiceType').html('').append('<option value="0">Please Select One!</option>');
                         $('#divInvoiceType').html('<a class="ui-multiselect ui-multiselect-widget ui-multiselect-state-default" id="ddlInvoiceType" style="width: 200px;" parent-checked-value="4"><input type="text" title="" value="Please Select One!" class="ui-multiselect-state-default" readonly="readonly" style="width: 185px;"><span class="ui-multiselect-icon ui-multiselect-icon-triangle-1-s"></span></a>');
                    }
                    getInvoiceNo();
                    $scope.$apply();
            },
            error: function () {
                errorAjax('Loading Licensee data fail.');
            },
            complete: function (jqXHR, textStatus) {
                NapAres.enableInput();
            }
        });
     };

     $scope.getInvoiceType = function(){
      NapAres.disableInput();
       var partyIdTmp =  $scope.operator == null ? -1 :  $scope.operator.PartyID ;
        var licenseeIdTmp =  $scope.licensee == null ? -1 :  $scope.licensee.LicenseeID ;

        if($scope.operator == null || $scope.licensee == null){
                    $('#divInvoiceType').html('').append('<option value="0">Please Select One!</option>');
                         $('#divInvoiceType').html('<a class="ui-multiselect ui-multiselect-widget ui-multiselect-state-default" id="ddlInvoiceType" style="width: 200px;" parent-checked-value="4"><input type="text" title="" value="Please Select One!" class="ui-multiselect-state-default" readonly="readonly" style="width: 185px;"><span class="ui-multiselect-icon ui-multiselect-icon-triangle-1-s"></span></a>');
        }
        else{
            $.ajax({
                url: domain + "Invoice/Invoice/getInvoiceType",
                type: "POST",
                data: 'licenseeID=' + licenseeIdTmp + '&partyId=' + partyIdTmp,
                success: function (result) {
                    createInvoiceTypeDropdowList(result);
                    getBaseCurrency();
                },
                error: function () {
                     errorAjax('Loading Invoice Type fail.');
                },
                complete: function (jqXHR, textStatus) {
                    NapAres.enableInput();
                }
            });
        }
        NapAres.enableInput();
     };
     
     $scope.submit = function () {
           
          if($scope.operator == null || $scope.licensee == null || !$.isNumeric($scope.invoiceNo)){
             return false;
          }
          invoiceService.setOperatorSelect($scope.operator);

        if ($scope.invoiceNo == '') {
            errorAjax('Please input Invoice No.!');
            return false;
        }

        if ($scope.products == '') {
            errorAjax('Please select product!');
            return false;
        }

        var toDateSelected = new Date($('#ToDatePicker').val());
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
                        getNewInvoiceData();
                    },
                    Cancel: function () {
                        $(this).dialog("close");
                    }
                }
            });
            $(".ui-dialog-titlebar").hide();
        } else {
            getNewInvoiceData();
        }
    },

     createInvoiceTypeDropdowList = function(result){
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
                             getBaseCurrency();
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
     };

     errorAjax = function(messege){
       NapAres.Helper.showMsgBox({
                    ErrorCode: 1,
                    ErrorMsg: messege
                });
                setTimeout('NapAres.Helper.hideMsgBox()', 4000);
     };

     getInvoiceNo = function () {
        NapAres.disableInput();
        var partyIdTmp =  $scope.operator == null ? -1 :  $scope.operator.PartyID ;
//        getSelectedInvoiceType();
        $.ajax({
            url: domain + "Invoice/Invoice/GetInvoiceNo",
            type: "POST",
            data: 'PartyId=' + partyIdTmp,
            success: function (data, textStatus, jqXHR) {
               $scope.invoiceNo = data;
               $scope.$apply();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                 errorAjax('Loading Invoiceno fail.');
            },
            complete: function (jqXHR, textStatus) {
                NapAres.enableInput();
            }
        });
    };

     getSelectedInvoiceType = function () {
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
        var invoiceTypeIdTmp = '';
        var invoiceTypeNameTmp = '';
        if (radioCheck && radioCheck.length > 0) {
            invoiceTypeIdTmp = radioCheck[0].value;
            invoiceTypeNameTmp = radioCheck[0].title;
        }

         $scope.products = produtID;
         $scope.productsName = productName;
         $scope.invoiceType = {
            InvoiceTypeID: invoiceTypeIdTmp,
            InvoiceTypeName: invoiceTypeNameTmp
        };
    }

     getBaseCurrency = function(){
       NapAres.disableInput();
        var partyIdTmp =  $scope.operator == null ? -1 :  $scope.operator.PartyID ;
        var licenseeIdTmp =  $scope.licensee == null ? -1 :  $scope.licensee.LicenseeID ;
       getSelectedInvoiceType();
                            $.ajax({
                                url: domain + "Invoice/Invoice/GetBaseCurrency",
                                type: "POST",
                                data: 'licenseeID=' + licenseeIdTmp + '&partyId=' + partyIdTmp + '&Products=' + $scope.products,
                                success: function (data) {
                                      $scope.baseCurrencies = data;
                                      $scope.baseCurrency = $scope.baseCurrencies[0];
                                       $scope.$apply();
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
    
     getNewInvoiceData =  function () {
        NapAres.disableInput();
//        $rootScope.invoiceInfoModel.Operator = $scope.operator;
        $.ajax({
            url: domain + "Invoice/Invoice/IsExistInvoiceNo",
            type: "POST",
            data: 'PartyID=' + $scope.operator.PartyID + '&InvoiceNo=' + $scope.invoiceNo,
            success: function (data, textStatus, jqXHR) {
                if (data == false) {
                    $.ajax({
                        url: domain + "Invoice/Invoice/GetInvoice",
                        type: "POST",
                        data: 'PartyID=' + $scope.operator.PartyID + '&LicenseeID=' + $scope.licensee.LicenseeID + '&Products=' + $scope.products + '&FromDate=' + $('#FromDatePicker').val() + '&ToDate=' + $('#ToDatePicker').val() + '&BillingCurrency=' + $scope.baseCurrency.Value,
                        success: function (data) {
                            $('#parentContainer').html(data);
                            $("#parentContainer").css("margin-top", "90px !important;");
                            $('#btnGenerate').addClass('ui-state-disabled');
                            
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            $('#parentContainer').html('');
                             errorAjax('Loading data fail.');
                        },
                        complete: function (jqXHR, textStatus) {
                            NapAres.enableInput();
                        }
                    });
                } else {
                    $('#parentContainer').html('');
                    errorAjax('The Invoice No. is existed, please enter a new number!');
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                $('#parentContainer').html('');
               errorAjax('Loading data fail.');
            },
            complete: function (jqXHR, textStatus) {
                NapAres.enableInput();
            }
        });
    }
});

app.controller('InvoiceController', function($scope, invoiceService) {
    $scope.invoiceItems = invoiceService.getInvoiceModel();
    $scope.invoiceItems.Operator =  invoiceService.getOperatorSelected();
    $scope.isInsert = false;
    $scope.descript = "Software & Data Licensing & Data transmission fees for ";

    $scope.insertItem = function (isDiscount_temp) {
        if ($scope.isInsert) {
            $scope.invoiceItems.StatementList.push({
                StatementDetailList: [{
                    ID: 0,
                    Reference: "",
                    Description: "",
                    Amount: 0,
                    AmountFormated: '',
                    InvoiceMonth: '',
                    AdjustItem: true,
                    DiscountItem: isDiscount_temp,
                    ProductType: -1,
                    StatementProductDetail: [{
                            ID: 0,
                            Reference: "",
                            Description: "",
                            Amount: 0,
                            AmountFormated: '',
                            InvoiceMonth: '',
                            AdjustItem: true,
                            DiscountItem: isDiscount_temp,
                            ProductType: -1
                        }],
                }]
            });
        }
        else {
            NapAres.Helper.showMsgBox({
                ErrorCode: 1,
                ErrorMsg: 'Please input value !'
            });
            setTimeout('NapAres.Helper.hideMsgBox()', 4000);
        }
    };

    $scope.deleteItem = function (index) {
        $scope.invoiceItems.StatementList.splice(index, 1);
    };

    $scope.change = function (parentIndex, index) {
        var amountTmp = $scope.invoiceItems.StatementList[parentIndex].StatementDetailList[index].Amount;
        var isDiscountTmp = $scope.invoiceItems.StatementList[parentIndex].StatementDetailList[index].DiscountItem;

        $scope.invoiceItems.StatementList[parentIndex].StatementDetailList[index].Amount = (isDiscountTmp == true ? Math.abs(amountTmp) * -1 : Math.abs(amountTmp));

        return; 
    };

    $scope.negativeClass = function (input) {
        //        return numeral().unformat(input) < 0 ? "red" : "";
        return input < 0 ? "red" : "";
    }

    $scope.$watch('invoiceItems.StatementList',function () {
        var total = 0;
        var count = 1;
        for (var i = 0; i < $scope.invoiceItems.StatementList.length; i++) {
            var statement = $scope.invoiceItems.StatementList[i];
            if (statement.StatementDetailList[0].DiscountItem == true) {
                    statement.No = '';
                }
                else {
                    statement.No = count;
                    count += 1;
                }
            for (var j = 0; j < statement.StatementDetailList.length; j++) {
                var statementDetail = statement.StatementDetailList[j];
                if (statementDetail.AmountFormated != '') {
                    var amountTmp = numeral().unformat(statementDetail.AmountFormated);
                    statementDetail.Amount = (statementDetail.DiscountItem == true ? (Math.abs(amountTmp) * -1) : Math.abs(amountTmp));
                    total = total + statementDetail.Amount;
                    $scope.isInsert = true;
                }
                else {
                    $scope.isInsert = false;
                }
            }
        }
        $scope.invoiceItems.TotalAmount = total;
        $scope.invoiceItems.TotalAmountFomarted = numeral($scope.invoiceItems.TotalAmount).format(FormatConstant.NEGATIVE_TWO_DECIMAL_NUMBER);
    }, true);

});

app.directive('renderOnblur', function () {
    return {
        require: 'ngModel',
        restrict: 'A',
        link: function (scope, elm, attrs, ctrl) {
            elm.bind('blur', function () {
                var json = jQuery.parseJSON(attrs.discountItem);
                var viewValue = ctrl.$modelValue;
//                if (attrs.discountItem !== undefined && json.DiscountItem == true) {
//                    viewValue = Math.abs(viewValue) * -1;
//                    json.Amount = json.Amount * -1;
//                }

                viewValue = json.DiscountItem == true ? (Math.abs(viewValue) * -1) : Math.abs(viewValue);

                ctrl.$viewValue = numeral(viewValue).format(FormatConstant.NEGATIVE_TWO_DECIMAL_NUMBER);
                ctrl.$render();
            });
        }
    };
});


$(document).ready(function () {
    domain = location.protocol + '//' + location.host + $('#NapHubRoot').val();
    //------binding events
    invoiceApp.init();

});

var invoiceApp = {
    init: function () {
        $("#notemessage").tipsy({
            html: true,
            opacity: 0.9,
            gravity: 'ne'
        });

          $('#divInvoiceType').html('').append('<option value="0">Please Select One!</option>');
          $('#divInvoiceType').html('<a class="ui-multiselect ui-multiselect-widget ui-multiselect-state-default" id="ddlInvoiceType" style="width: 200px;" parent-checked-value="4"><input type="text" title="" value="Please Select One!" class="ui-multiselect-state-default" readonly="readonly" style="width: 185px;"><span class="ui-multiselect-icon ui-multiselect-icon-triangle-1-s"></span></a>');

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
                $('#btnGenerate').removeClass('ui-state-disabled');
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

        $('#btnGenerate').button();
        
        $('#btnNewInvoice').button();
    },


    saveInvoice: function () {
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
        invoiceInfoModel.IsSignature = ($('#chkSign').hasClass("checkedCls") == true ? true : false);
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
    },

    exportPDF: function ()
    {       
        var link = domain + "Invoice/Invoice/ExportDocument?isSignature= " + ($('#chkSign').hasClass("checkedCls") == true ? true : false);
        location.href = link;
    },

    checkSignature: function()
    {
        $('#chkSign').hasClass("checkedCls") == true ? $('#chkSign').removeClass('checkedCls').addClass('uncheckedCls') : $('#chkSign').removeClass('uncheckedCls').addClass('checkedCls');
    }
}
