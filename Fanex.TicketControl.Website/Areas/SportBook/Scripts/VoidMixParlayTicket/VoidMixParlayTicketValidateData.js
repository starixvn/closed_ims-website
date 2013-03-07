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

        if ($('#EventType').val() == 1) {
            if ($('#LiveHomeScore').val() != '' && ($('#LiveHomeScore').val().length > 2 || isNaN($('#LiveHomeScore').val()))) {
                $('#msg').html(tooltip.show('LiveHomeScore', 'Invalid Live Home Score.')).show();
                return false;
            }

            if ($('#LiveAwayScore').val() != '' && ($('#LiveAwayScore').val().length > 2 || isNaN($('#LiveAwayScore').val()))) {
                $('#msg').html(tooltip.show('LiveAwayScore', 'Invalid Live Home Score.')).show();
                return false;
            }
        }

        if (helper.GetParlayTypeChecked() == '') {
            $('#msg').html(tooltip.show('chkMixParlay', 'Please select a type of Parlay.')).show();
            return false;
        }

        if (helper.GetBettypesChecked() == '') {
            $('#msg').html(tooltip.show('chkallbettype', 'At least on bettype must be selected.')).show();
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