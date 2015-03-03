"use strict";

var LicenseeSettlement = (function () {
    var me = {};

    var btnGenerate = $('#btnGenerate');
    var licenseeFilter = $('#LicenseeId');
    var domain = '';
    var formOriginalAction = '';
    var today = '';
    var minDate = '';
    var labelFrom = '';
    var labelTo = '';
    var noteMessage = '';

    var renderGenerateButton = function () {
        btnGenerate.button();
        btnGenerate.click(function () {
            if (!$(this).hasClass('ui-state-disabled')) {
                submit();

                var productId = $('#Product').val();
                var exportExcelRoutes = "?ProductId=" + parseInt(productId, 10) + "&documentType=Xlsx";
                var exportPdfRoutes = "?ProductId=" + parseInt(productId, 10) + "&documentType=Pdf";

                $('#pdfForm').attr('action', formOriginalAction + exportPdfRoutes);
                $('#xlsForm').attr('action', formOriginalAction + exportExcelRoutes);
            }
        });
    };

    var renderFilterNote = function () {
        var $this = $('#notemessage');
        $this.attr("title", noteMessage);
        $this.tipsy({
            html: true,
            opacity: 0.9,
            gravity: 'ne'
        });
    };

    var renderLicenseeFilter = function () {
        licenseeFilter.change(function () {
            getProductByLicenseeID($(this).val());
            getCurrencyByLicenseeID($(this).val());
            if ($(this).val() === "0") {
                btnGenerate.addClass("ui-state-disabled");
            } else {
                btnGenerate.removeClass("ui-state-disabled");
            }
        });
    };

    var getCurrencyByLicenseeID = function (licenseeId) {
        $.ajax({
            url: domain + "Report/LicenseeSettlement/GetCurrencyByLicenseeID",
            type: "POST",
            data: "&licenseeID=" + licenseeId,
            success: function (data) {
                var html = "";
                if (data.length !== 0) {
                    html = "<option value='0' selected='selected'>All</option>";
                } else {
                    btnGenerate.addClass("ui-state-disabled");
                }

                for (var i = 0; i < data.length; i++) {
                    html += "<option value='" + data[i].Value + "'>" + data[i].Text + "</option>";
                }
                $('#Currency').html(html);
            },
            error: function () {
                alert("fail");
            }
        });
    };

    var getProductByLicenseeID = function (licenseeId) {
        $.ajax({
            url: domain + "Report/LicenseeSettlement/GetProductByLicenseeID",
            type: "POST",
            data: "&licenseeID=" + licenseeId,
            success: function (data) {
                var html = "";
                for (var i = 0; i < data.length; i++) {
                    html += "<option value='" + data[i].Value + "'>" + data[i].Text + "</option>";
                }
                $('#Product').html(html);
            },
            error: function () {
                alert("fail");
            }
        });
    };

    var renderMonthPicker = function () {
        var now = new Date();
        var lastMonth = new Date();

        lastMonth = lastMonth.setMonth(lastMonth.getMonth() - 1);

        $('#ddlMonth').monPickr({
            today: now,
            maxDate: now,
            minDate: new Date(minDate),
            showDateRange: true,
            fromDayId: 'ddlFrom',
            month: lastMonth,
            sortDesc: true,
            log: function (msg) {
                console.warn(msg);
            }
        });
    };

    var initEvent = function () {
        renderGenerateButton();
        renderFilterNote();
        renderLicenseeFilter();
        renderMonthPicker();
    };

    var submit = function () {
        $.blockUI();

        var baseCurrencyName = $('#BaseCurrency option:selected').text();
        var productName = $('#Product option:selected').text();

        $.ajax({
            url: domain + "Report/LicenseeSettlement/GetReport",
            type: "POST",
            data: $('#formLicenseeSettlement').serialize()
                + "&BaseCurrencyName=" + baseCurrencyName
                + "&ProductName=" + productName
                + "&FromDate=" + $('#ddlMonth').monPickr("getFromDate", "MMM dd, yyyy")
                + "&ToDate=" + $('#ddlMonth').monPickr("getToDate", "MMM dd, yyyy"),
            success: function (data) {
                $('#parentContainer').html(data);
                $('#nonLiveBingoReportTitle').text("Settlement Report for " + $('#LicenseeId option:selected').text()
                + " from  " + $('#ddlMonth').monPickr("getFromDate", "dd-MMM-yyyy")
                + " to  " + $('#ddlMonth').monPickr("getToDate", "dd-MMM-yyyy"));
                $(".curr").empty().text("Licensee Settlement Report for " + productName);
                $(".grid-title").show();
                if (data.indexOf("There is no data") !== -1) {
                    $(".grid-toolbox").hide();
                }
                else {
                    $(".grid-toolbox").show();
                }
            },
            error: function (/*xhr, ajaxOptions, thrownError*/) {
                alert("fail");
            },
            complete: function () {
                $.unblockUI();
            }
        });
    };

    var initFields = function () {
        formOriginalAction = $('#xlsForm').attr('action');
        domain = location.protocol + '//' + location.host + $('#NapHubRoot').val();
        today = $('#today').val();
        minDate = $('#minDate').val();
        labelFrom = $('#labelFrom').val();
        labelTo = $('#labelTo').val();
        noteMessage = $('#noteMessage').val();
    };

    var initFilterValue = function () {
        initFields();
        renderDatePicker();
    };

    var renderDatePicker = function () {
        $('#datepickerplugin').nexDatePicker({
            nexDatePickerType: "quickchoose",
            datePickerFormat: {
                changeMonth: true,
                changeYear: true,
                dateFormat: "d M yy"
            },
            dropDownList:
            {
                Id: "dropDownListId",
                Label: "Date ",
                LabelCss: "lbl-field col_3",
                OptionCss: "edit-field col_6"
            },
            firstDatePicker: {
                Id: "FromDate",
                Label: labelFrom,
                InitDate: today,
                LabelCss: "lbl-field col_3",
                TextboxCss: "edit-field col_6"
            },
            secondDatePicker: {
                Id: "ToDate",
                Label: labelTo,
                InitDate: today,
                LabelCss: "lbl-field col_3",
                TextboxCss: "edit-field col_6"
            },
            selectedItem: 5,
            onSelectDatePicker: function () {
            },
            minDate: new Date(minDate),
            maxDate: new Date(today)
        });
    };

    var arrangeView = function () {
        $('#dropDownListId').addClass('auto-width');

        $('#FromDate, #ToDate').each(function () {
            $(this).width($('#dropDownListId').width() + 2);
        });

        btnGenerate.addClass("ui-state-disabled");

        $('.monpickr-super-wrapper .monpickr-label').eq(0).addClass('col_3 align-right lbl-field');
        $('.monpickr-super-wrapper .daypickr-wrap-parent').eq(0).addClass('col_3');
    };

    me.init = function () {
        initFilterValue();
        initEvent();
        arrangeView();
    };

    return me;
} (jQuery));

$(document).ready(function () {
    LicenseeSettlement.init();
});
