/* File Created: March 6, 2013 */
var validateData = {
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

        if (helper.GetBettypesChecked() == '') {
            $('#msg').html(tooltip.show('chkallbettype', 'At least one bet type must be selected.')).show();
            return false;
        }

        return true;
    },

    checkParamBeforVoidTicket: function () {
        $('#msg').hide();

        if (helper.getSubTicketsChecked() == '') {
            $('#msg').html(tooltip.show('chk-ticket-all', 'At least on ticket must be selected.')).show();
            return false;
        }

        if ($("#msg-en").val() == "") {
            $('#msg').html(tooltip.show('msg-en', 'You must enter English message')).show();
            return false;
        }
        if ($("#msg-en").val().indexOf("@@") < 0) {
            $('#msg').html(tooltip.show('msg-en', 'You must include @@ in English message for replacing ticket number')).show();
            return false;
        }
        if ($("#msg-ch").val() == "") {
            $('#msg').html(tooltip.show('msg-ch', 'You must enter Chinese (Mandarin) message')).show();
            return false;
        }
        if ($("#msg-ch").val().indexOf("@@") < 0) {
            $('#msg').html(tooltip.show('msg-ch', 'You must include @@ in Chinese (Mandarin) message for replacing ticket number')).show();
            return false;
        }

        if ($('#password').val() == '') {
            $('#msg').html(tooltip.show('password', 'Please input password.')).show();
            return false;
        }

        return true;
    }
};