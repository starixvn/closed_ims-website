$(document).ready(function () {
    renderDatePicker();
    $('#dropDownListId').addClass('auto-width');
    $('#FromDate, #ToDate').each(function () {
        $(this).width($('#dropDownListId').width() + 2);
    });

    $('#btnGenerate').button();
    $('#btnGenerate').click(function () { Submit(); });

    $('#btnExcel').click(function () { exportReportToExcel(); });
    RenderFilterNote();

    $('#LicenseeId').change(function () {
        GetProductByLicenseeID($(this).val(), $('Product'));
    });
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
    $.ajax({
        url: "LicenseeSettlement/GetInvoice",
        type: "POST",
        data: $('#formLicenseeSettlement').serialize() + "&BaseCurrencyName=" + $('#BaseCurrency option:selected').text(),
        success: function (data, textStatus, jqXHR) {
            $('#parentContainer').html(data);
            $('#nonLiveBingoReportTitle').text("Settlement Report for " + $('#LicenseeId option:selected').text() + " from " + $('#FromDate').val() + " to " + $('#ToDate').val());
            $(".grid-title").show();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert("faild");
        },
        complete: function (jqXHR, textStatus) {
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

function GetProductByLicenseeID(licenseeID, destinationElement) {
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

function GetProductByLicenseeID(licenseeID, destinationElement) {
    $.ajax({
        url: "LicenseeSettlement/GetCurencyByLicenseeID",
        type: "POST",
        data: "&licenseeID=" + $('#LicenseeId').val(),
        success: function (data, textStatus, jqXHR) {
            var html = "<option value='0' selected='selected'>All</option>";
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