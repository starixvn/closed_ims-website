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
    $scope.isInsert = false;
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
                    ProductType: -1
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

    $scope.deleteItem = function ($index) {
        $scope.invoiceItems.StatementList.splice($index, 1);
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
//    '[Description, Reference, AmountFormated]',
    $scope.$watch(function () {
        var total = 0;
        var count = 1;
        for (var i = 0; i < $scope.invoiceItems.StatementList.length; i++) {
            var statement = $scope.invoiceItems.StatementList[i];
            if (statement.StatementDetailList[0].DiscountItem == true) {
                statement.index = '';
            }
            else {
                statement.index = count;
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