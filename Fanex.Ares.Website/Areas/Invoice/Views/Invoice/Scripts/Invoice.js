$(document).ready(function () {
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
});