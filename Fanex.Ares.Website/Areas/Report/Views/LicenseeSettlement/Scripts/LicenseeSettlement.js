$(document).ready(function () {
    renderDatePicker();
    $('#dropDownListId').addClass('auto-width');
    $('#FromDate, #ToDate').each(function () {
        $(this).width($('#dropDownListId').width() + 2);
    });

    $('#btnGenerate').button();
    $('#btnGenerate').click(function () { Submit(); });
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

function Submit() {
    $.ajax({
        url: "LicenseeSettlement/GetInvoice",
        type: "POST",
        data: $('#formLicenseeSettlement').serialize(),
        success: function (data, textStatus, jqXHR) {
            $('#parentContainer').html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
        },
        complete: function (jqXHR, textStatus) {
        }
    });
}