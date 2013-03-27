/* File Created: March 6, 2013 */

if (VoidNumberGame === undefined) {
    VoidNumberGame = {};
}

//var validateData =
VoidNumberGame.Validator = {

    checkParamBeforeGetData: function () {
        $('#msg').hide();

        if ($('#txtMatchId').val() == '') {
            $('#msg').html(tooltip.show('txtMatchId', 'MatchID is required.')).show();
            return false;
        }

        if (isNaN($('#txtMatchId').val()) || $('#txtMatchId').val().length > 10) {
            $('#msg').html(tooltip.show('txtMatchId', 'Invalid MatchID.')).show();
            return false;
        }

        if ($('#fromRefno').val() == '') {
            $('#msg').html(tooltip.show('fromRefno', 'From BetID is required.')).show();
            return false;
        }

        if (isNaN($('#fromRefno').val()) || $('#fromRefno').val().length > 10) {
            $('#msg').html(tooltip.show('fromRefno', 'Invalid From BetID.')).show();
            return false;
        }

        if ($('#toRefno').val() == '') {
            $('#msg').html(tooltip.show('toRefno', 'To BetID is required.')).show();
            return false;
        }

        if (isNaN($('#toRefno').val()) || $('#toRefno').val().length > 10) {
            $('#msg').html(tooltip.show('toRefno', 'Invalid To BetID.')).show();
            return false;
        }

        if (parseFloat($('#toRefno').val()) < parseFloat($('#fromRefno').val())) {
            $('#msg').html(tooltip.show('fromRefno', 'Invalid From BetID.')).show();
            return false;
        }

        if (VoidNumberGame.Helper.getBettypesChecked() == '') {
            $('#msg').html(tooltip.show('chkallbettype', 'At least one bet type must be selected.')).show();
            return false;
        }

        return true;
    },

    checkParamBeforeVoidTicket: function () {
        $('#msg').hide();

        if (VoidNumberGame.Helper.getSubTicketsChecked() == '') {
            $('#msg').html(tooltip.show('chk-ticket-all', 'At least one ticket must be selected.', true)).show();
            return false;
        }

        if (Message.Validator.validateMessageConstraints() === false) {
            return false;
        }

        if ($('#password').val() == '') {
            $('#msg').html(tooltip.show('password', 'Please input password.')).show();
            return false;
        }

        return true;
    }
};