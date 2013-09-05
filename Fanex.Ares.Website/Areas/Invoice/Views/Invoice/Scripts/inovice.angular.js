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
    if ((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105) || event.keyCode == 8 || event.keyCode == 109 || event.keyCode == 188 || event.keyCode == 190 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 39 || event.keyCode == 46) {
        // Allow normal operation
    } else {
        // Prevent the rest
        event.preventDefault();
    }
}


function InvoiceController($scope) {
    //        $scope.invoiceItems = Items.query();
    $scope.total = 0;
    $scope.totalFormater = '0';
    $scope.no = 0;
    $scope.invoiceItems = [[{ Amount: 2500.56, AmountFormated: '2,500.56', Description: "dfaf", Reference: "dfasfdaad", isNew: false, isDiscount: false},
                            { Amount: -3600.45, AmountFormated: '(3,500.56)', Description: "dfaf", Reference: "dfasfdaad", isNew: false, isDiscount: true}]];

    $scope.insertItem = function (isDiscount_temp) {
        //                        $scope.invoiceItems.splice(1, 0, [[{ Amount: 500, Description: "new", Reference: "new"}]]);
        if (isDiscount_temp)
        {
            $scope.invoiceItems.push([{ Amount: -1300.56, AmountFormated: '(1,300.56)', Description: "new", Reference: "new", isNew: true, isDiscount: true}]);
        }
        else{
            $scope.invoiceItems.push([{ Amount: 1300.56, AmountFormated: '1,300.56', Description: "new", Reference: "new", isNew: true, isDiscount: false}]);
        }
       
    };

    $scope.deleteItem = function ($index) {
        $scope.invoiceItems.splice($index, 1);
        //            $scope.invoiceItems.pop($index);
    };

    $scope.change = function ($index) {
//        $scope.invoiceItems[$index][0].Amount = numeral().unformat($scope.invoiceItems[$index][0].AmountFormated);
        //                $scope.invoiceItems[$index][0].AmountFormated = numeral($scope.invoiceItems[$index][0].AmountFormated).format(FormatConstant.NEGATIVE_TWO_DECIMAL_NUMBER); 
    };


    $scope.negativeClass = function (input) {
        numeral().unformat(input) < 0 ? "red" : "";
    }

    $scope.increseIndex = function ($index) {
        if (!$scope.invoiceItems[$index][0].isDiscount) {
            $scope.no = $scope.no + 1;
            return $scope.no;
        }
        else {
            return '';
        }

    }


    $scope.$watch(function () {
        var total = 0;
        for (var i = 0; i < $scope.invoiceItems.length; i++) {
            for (var j = 0; j < $scope.invoiceItems[i].length; j++) {
                if ($scope.invoiceItems[i][j].AmountFormated == '') {
                    $scope.invoiceItems[i][j].AmountFormated = '0';
                }
                $scope.invoiceItems[i][j].Amount = numeral().unformat($scope.invoiceItems[i][j].AmountFormated);
                total = total + $scope.invoiceItems[i][j].Amount;
            }
        }
        $scope.total = total;
        $scope.totalFormater = numeral($scope.total).format(FormatConstant.NEGATIVE_TWO_DECIMAL_NUMBER);
    });
}

var shoppingModule = angular.module('Invoice', []);

shoppingModule.directive('renderOnblur', function () {
    return {
        require: 'ngModel',
        restrict: 'A',
        link: function (scope, elm, attrs, ctrl) {
            elm.bind('blur', function () {
                var viewValue = ctrl.$modelValue;
                ctrl.$viewValue = numeral(viewValue).format(FormatConstant.NEGATIVE_TWO_DECIMAL_NUMBER);
                ctrl.$render();
            });
        }
    };
});

