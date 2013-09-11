var FormatConstant = {
    INT_NUMBER: '0,0',
    TWO_DECIMAL_NUMBER: '0,0.00',
    FOUR_DECIMAL_NUMBER: '0,0.0000',
    NEGATIVE_TWO_DECIMAL_NUMBER: '(0,0.00)',
    TWO_DECIMAL_PERCENT_NUMBER: '0,0.00%',
    MMM_DD_YYYY_STRING: 'MMM DD, YYYY',
    M_DD_YY_STRING: 'M dd, yy',
    MM_DD_YYYY_WITH_SLASH_STRING: 'MM/dd/yyyy',
    MMM_YY_STRING: 'MMM YY',
    MMM_YY_STRING_MONTH_PICKER: 'MMM yyyy',
    MMM_DD_YY_STRING_MONTH_PICKER: 'MMM dd, yyyy',
    MM_DD_YYYY_WITH_SLASH_STRING_MOMENT: 'M/D/YYYY',
    MMM_DD_YYYY_STRING_MOMENT: 'MMM D, YYYY'
}

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


function InvoiceController($scope) {
    //        $scope.invoiceItems = Items.query();
    $scope.invoiceItems = OrgInvoiceInfoModel;

    $scope.insertItem = function (isDiscount_temp) {
            $scope.invoiceItems.StatementList.push({
                StatementDetailList: [{
                    ID: 0,
                    Reference: "",
                    Description: "",
                    Amount: 0,
                    AmountFormated: '0',
                    InvoiceMonth: '',
                    AdjustItem: true,
                    DiscountItem: isDiscount_temp,
                    ProductType: -1
                }]
            });
    };

    $scope.deleteItem = function ($index) {
        $scope.invoiceItems.StatementList.splice($index, 1);
        //            $scope.invoiceItems.pop($index);
    };

    $scope.change = function (parentIndex, index) {
        var amountTmp = $scope.invoiceItems.StatementList[parentIndex].StatementDetailList[index].Amount;
        var isDiscountTmp = $scope.invoiceItems.StatementList[parentIndex].StatementDetailList[index].DiscountItem;

        $scope.invoiceItems.StatementList[parentIndex].StatementDetailList[index].Amount = (isDiscountTmp == true ? Math.abs(amountTmp) * -1 : Math.abs(amountTmp));

        return; 
        //        var amountTmp = numeral().unformat(statementDetail.AmountFormated);
        //        return statementDetail.Amount = statementDetail.DiscountItem == true ? Math.abs(amountTmp) * -1 : Math.abs(amountTmp);
    };


    $scope.negativeClass = function (input) {
        //        return numeral().unformat(input) < 0 ? "red" : "";
        return input < 0 ? "red" : "";
    }
//    '[Description, Reference, AmountFormated]',
    $scope.$watch( function () {
        var total = 0;
        var count = 1;
        for (var i = 0; i < $scope.invoiceItems.StatementList.length; i++) {
            var statement = $scope.invoiceItems.StatementList[i];
            if (statement.StatementDetailList[0].DiscountItem == true) {
                statement.index = '';
            }
            else {
                statement.index = count;
                count +=  1;
            }

            for (var j = 0; j < statement.StatementDetailList.length; j++) {
                var statementDetail = statement.StatementDetailList[j];
                if (statementDetail.AmountFormated == '') {
                    statementDetail.AmountFormated = '0';
                }

                var amountTmp = numeral().unformat(statementDetail.AmountFormated);
                statementDetail.Amount = (statementDetail.DiscountItem == true ?( Math.abs(amountTmp) * -1 ): Math.abs(amountTmp));
//                statementDetail.Amount = numeral().unformat(statementDetail.AmountFormated);

                total = total + statementDetail.Amount;
            }
        }
        $scope.invoiceItems.TotalAmount = total;
        $scope.invoiceItems.TotalAmountFomarted = numeral($scope.invoiceItems.TotalAmount).format(FormatConstant.NEGATIVE_TWO_DECIMAL_NUMBER);
    });
}

var shoppingModule = angular.module('Invoice', []);

shoppingModule.directive('renderOnblur', function () {
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