$(document).ready(function () {
    domain = location.protocol + '//' + location.host + $('#NapHubRoot').val();

    $('#fromTopickerplugin').nexDatePicker({
        nexDatePickerType: "quickchoose",
        datePickerFormat: {
            changeMonth: true,
            dateFormat: "M dd, yy"
        },
        firstDatePicker: {
            Id: "FromDatePicker",
            Label: labelFrom,
            InitDate: _today,
            LabelCss: "lbl-field col_3",
            TextboxCss: "edit-field col_6"
        },
        secondDatePicker: {
            Id: "ToDatePicker",
            Label: labelTo,
            InitDate: _today,
            LabelCss: "lbl-field col_1",
            TextboxCss: "edit-field col_6"
        },
        onSelectDatePicker: function () {
        },
        dropDownList:
        {
            Id: "dropDownListId1",
            Label: "Date ",
            LabelCss: "lbl-field col_3",
            OptionCss: "edit-field col_5"
        }
    });


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
            TextboxCss: "edit-field col_8"
        },
        onSelectDatePicker: function () {
        }
    });


    $('#btnGenerate').button();
    $('#btnGenerate').click(function () { if (!$(this).hasClass('ui-state-disabled')) Submit(); });
});

function Submit() {
    NapTicketControl.disableInput();
    $.ajax({
        url: domain + "Invoice/Invoice/GetInvoice",
        type: "POST",
        success: function (data, textStatus, jqXHR) {
            $('#parentContainer').html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert("fail");
        },
        complete: function (jqXHR, textStatus) {
            NapTicketControl.enableInput();
        }
    });
}