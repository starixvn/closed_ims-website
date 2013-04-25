$(document).ready(function () {
    renderDatePicker();
    $('#dropDownListId').addClass('auto-width');
    $('#FromDate, #ToDate').each(function () {
        $(this).width($('#dropDownListId').width() + 2);
    });

    $('#btnGenerate').button();
    $('#btnGenerate').click(function () { Submit(); });
    $('#btnPDF').click(function () { PrintPDF(); });
    $('#btnExcel').click(function () { exportReportToExcel(); });
    RenderFilterNote();


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
    $this.attr("title", "aaaaaaaaaaaaaaaaaaa");
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
        data: $('#formLicenseeSettlement').serialize(),
        success: function (data, textStatus, jqXHR) {
            $('#parentContainer').html(data);
            $('#nonLiveBingoReportTitle').val("Settlement Report for " + $('#LicenseeId').val() + " from " + $('#FromDate').val() + " to " + $('#ToDate').val());
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert("faild");
        },
        complete: function (jqXHR, textStatus) {
        }
    });
}

function PrintPDF() {

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