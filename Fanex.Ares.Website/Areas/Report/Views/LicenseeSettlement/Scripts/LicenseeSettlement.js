/// <reference path="../../../../../Scripts/NapTicketControl.js" />
$(document).ready(function () {
    renderDatePicker();
    $('#dropDownListId').addClass('auto-width');
    $('#FromDate, #ToDate').each(function () {
        $(this).width($('#dropDownListId').width() + 2);
    });

    $('#btnGenerate').button();
    $('#btnGenerate').click(function () { if (!$(this).hasClass('ui-state-disabled')) Submit(); });

    $('#btnExcel').click(function () { exportReportToExcel(); });

    RenderFilterNote();

    if ($('#LicenseeId').val() == "-1") {
        $('#btnGenerate').addClass("ui-state-disabled");
    } else {
        $('#btnGenerate').removeClass("ui-state-disabled");
    }

    $('#LicenseeId').change(function () {
        GetProductByLicenseeID($(this).val());
        GetCurrencyByLicenseeID($(this).val());
        if ($(this).val() == "-1") {
            $('#btnGenerate').addClass("ui-state-disabled");
        } else {
            $('#btnGenerate').removeClass("ui-state-disabled");
        }
    });
    var now = new Date();
    $('#ddlMonth').monPickr({
        today: now,
        maxDate: now,
        minDate: new Date(2005, 1, 1),
        showDateRange: true,
        fromDayId: 'ddlFrom',
        month: now,
        sortDesc: true,
        log: function (msg) {
            console.warn(msg);
        }
    });

    $('.monpickr-super-wrapper .monpickr-label').eq(0).addClass('col_3 align-right lbl-field');
    $('.monpickr-super-wrapper .daypickr-wrap-parent').eq(0).addClass('col_3');

});

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

function RenderFilterNote() {
    var $this = $('#notemessage');
    $this.attr("title", notemessage);
    $this.tipsy({
        html: true,
        opacity: 0.9,
        gravity: 'ne'
    });
};

function Submit() {
    NapTicketControl.disableInput();
    $.ajax({
        url: "LicenseeSettlement/GetReport",
        type: "POST",
        data: $('#formLicenseeSettlement').serialize()
                + "&BaseCurrencyName=" + $('#BaseCurrency option:selected').text()
                + "&ProductName=" + $('#Product option:selected').text()
                + "&FromDate=" + $('#ddlMonth').monPickr("getFromDate", "MMM dd, yyyy")
                + "&ToDate=" + $('#ddlMonth').monPickr("getToDate", "MMM dd, yyyy")
                ,
        success: function (data, textStatus, jqXHR) {
            $('#parentContainer').html(data);
            var month = $('#Month').val();
            var year = $('#Year').val();
            var fromDate = new Date(year, month - 1, 1);
            var toDate = lastDateInMonth(month, year);
            $('#nonLiveBingoReportTitle').text("Settlement Report for " + $('#LicenseeId option:selected').text() + " of  " + $('#Month  option:selected').text() + "-" + $('#Year').val());
            $(".curr").empty().text("Licensee Settlement Report for " + $('#Product option:selected').text());
            $(".grid-title").show();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert("fail");
        },
        complete: function (jqXHR, textStatus) {
            NapTicketControl.enableInput();
        }
    });
}


function exportReportToExcel() {
    var url = window.rootUrl + 'LicenseeSettlement/ExportSettlementReportToExcel?exchange=' + $('#BaseCurrency :selected').text() + '&product=' + $('#Product :selected').text();

    var iframe = $('body iframe');
    if (iframe.length == 0) {
        iframe = $('<iframe width="0" height="0" class="display-none"></iframe>');
        iframe.appendTo('body');
    }

    iframe.attr('src', url);
}

function GetProductByLicenseeID(licenseeID) {
    $.ajax({
        url: "LicenseeSettlement/GetProductByLicenseeID",
        type: "POST",
        data: "&licenseeID=" + $('#LicenseeId').val(),
        success: function (data, textStatus, jqXHR) {
            var html = "";
            for (var i = 0; i < data.length; i++) {
                html += "<option value='" + data[i].Value + "'>" + data[i].Text + "</option>";
            }
            $('#Product').html(html);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert("fail");
        },
        complete: function (jqXHR, textStatus) {
        }
    });
}

function GetCurrencyByLicenseeID(licenseeID) {
    $.ajax({
        url: "LicenseeSettlement/GetCurencyByLicenseeID",
        type: "POST",
        data: "&licenseeID=" + $('#LicenseeId').val(),
        success: function (data, textStatus, jqXHR) {
            var html = "";
            if (data.length != 0)
                html = "<option value='0' selected='selected'>All</option>";
            for (var i = 0; i < data.length; i++) {
                html += "<option value='" + data[i].Value + "'>" + data[i].Text + "</option>";
            }
            $('#Currency').html(html);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert("fail");
        },
        complete: function (jqXHR, textStatus) {
        }
    });
}

function ClientPrinting() {
    if ($.browser.msie) {
        window.open('LicenseeSettlement/PrintScreen?productName=' + $('#Product :selected').text());
    } else {
        window.open('LicenseeSettlement/ExportPDF?isSavePDF=false');
    }
}

function daysInMonth(month, year) {
    return new Date(year, month, 0).getDate();
}

function lastDateInMonth(month, year) {
    return new Date(year, month, daysInMonth(month, year));
}