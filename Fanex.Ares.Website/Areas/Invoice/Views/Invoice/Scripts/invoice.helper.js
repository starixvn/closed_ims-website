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
         $('#reviewID').removeClass('ui-state-disabled').removeAttr('disabled');
        $('#saveID').removeClass('ui-state-disabled').removeAttr('disabled');
        $('#pdfID').addClass('ui-state-disabled').attr('disabled', 'disabled');
    }
}).apply(invoiceApp.helper);
