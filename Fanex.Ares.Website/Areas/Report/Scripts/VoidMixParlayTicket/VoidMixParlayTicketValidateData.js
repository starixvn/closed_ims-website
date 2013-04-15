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

        var eventType = $('#EventType').val();

        if (eventType == '1') {
            if ($('#LiveHomeScore').val() != '' && ($('#LiveHomeScore').val().length > 2 || isNaN($('#LiveHomeScore').val()))) {
                $('#msg').html(tooltip.show('LiveHomeScore', 'Invalid Live Home Score.')).show();
                return false;
            }

            if ($('#LiveAwayScore').val() != '' && ($('#LiveAwayScore').val().length > 2 || isNaN($('#LiveAwayScore').val()))) {
                $('#msg').html(tooltip.show('LiveAwayScore', 'Invalid Live Home Score.')).show();
                return false;
            }
        }

        //cs
        var IsSelectedCS = helper.IsSelectedBettype(4) || helper.IsSelectedBettype(30);
        if (IsSelectedCS && eventType != '1') {
            if (helper.GetBetteam430() == '') {
                helper.expandAdvancedMode();
                $('#msg').html(tooltip.show('chk-score-all', 'Please select a score for bettype: Correct score!.')).show();
                return false;
            }
        }

        //htft
        var IsSelectedHTFT = helper.IsSelectedBettype(16);
        if (IsSelectedHTFT && eventType != '1') {
            if (helper.GetBetteam16() == '') {
                helper.expandAdvancedMode();
                $('#msg').html(tooltip.show('chk-htft-all', 'Please select a betteam for bettype: HT/FT!.')).show();
                return false;
            }
        }

        //tg
        var IsSelectedTG = helper.IsSelectedBettype(6);
        if (IsSelectedTG && eventType != '1') {
            if (helper.GetBetteam6() == '') {
                helper.expandAdvancedMode();
                $('#msg').html(tooltip.show('chk-tg-all', 'Please select a goal for bettype: Total Goal!.')).show();
                return false;
            }
        }

        //fg/lg
        var IsSelectedGFLG = helper.IsSelectedBettype(14);
        if (IsSelectedGFLG && eventType != '1') {
            if (helper.GetBetteam14() == '') {
                helper.expandAdvancedMode();
                $('#msg').html(tooltip.show('chk-flg-all', 'Please select a goal for bettype: First Goal/Last Goal!.')).show();
                return false;
            }
        }

        if (helper.GetBettypesChecked() == '') {
            $('#msg').html(tooltip.show('chkallbettype', 'At least one bettype must be selected.')).show();
            return false;
        }

        return true;
    },

    checkParamBeforVoidTicket: function () {
        $('#msg').hide();

        if (helper.getSubTicketsChecked() == '') {
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