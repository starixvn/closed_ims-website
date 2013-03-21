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
                $('#msg').html(tooltip.show('chk-score-all', 'Please select a score for bettype: Correct score!.')).show();
                return false;
            }
        }

        //htft
        var IsSelectedHTFT = helper.IsSelectedBettype(16);
        if (IsSelectedHTFT && eventType != '1') {
            if (helper.GetBetteam16() == '') {
                $('#msg').html(tooltip.show('chk-htft-all', 'Please select a betteam for bettype: HT/FT!.')).show();
                return false;
            }
        }

        //tg
        var IsSelectedTG = helper.IsSelectedBettype(6);
        if (IsSelectedTG && eventType != '1') {
            if (helper.GetBetteam6() == '') {
                $('#msg').html(tooltip.show('chk-tg-all', 'Please select a goal for bettype: Total Goal!.')).show();
                return false;
            }
        }

        //fg/lg
        var IsSelectedGFLG = helper.IsSelectedBettype(14);
        if (IsSelectedGFLG && eventType != '1') {
            if (helper.GetBetteam14() == '') {
                $('#msg').html(tooltip.show('chk-flg-all', 'Please select a goal for bettype: First Goal/Last Goal!.')).show();
                return false;
            }
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

        if ($("#msg-cs").val() != '' && $("#msg-cs").val().indexOf("@@") < 0) {
            $('#msg').html(tooltip.show('msg-cs', 'You must include @@ in Chinese (Cantonese) message for replacing ticket number')).show();
            return false;
        }

        if ($("#msg-th").val() != '' && $("#msg-th").val().indexOf("@@") < 0) {
            $('#msg').html(tooltip.show('msg-th', 'You must include @@ in Thai message for replacing ticket number')).show();
            return false;
        }

        if ($("#msg-it").val() != '' && $("#msg-it").val().indexOf("@@") < 0) {
            $('#msg').html(tooltip.show('msg-it', 'You must include @@ in Italian message for replacing ticket number')).show();
            return false;
        }

        if ($("#msg-jp").val() != '' && $("#msg-jp").val().indexOf("@@") < 0) {
            $('#msg').html(tooltip.show('msg-jp', 'You must include @@ in Japanese message for replacing ticket number')).show();
            return false;
        }

        if ($("#msg-es").val() != '' && $("#msg-es").val().indexOf("@@") < 0) {
            $('#msg').html(tooltip.show('msg-es', 'You must include @@ in Spanish message for replacing ticket number')).show();
            return false;
        }

        if ($("#msg-ko").val() != '' && $("#msg-ko").val().indexOf("@@") < 0) {
            $('#msg').html(tooltip.show('msg-ko', 'You must include @@ in Korean message for replacing ticket number')).show();
            return false;
        }

        if ($('#password').val() == '') {
            $('#msg').html(tooltip.show('password', 'Please input password.')).show();
            return false;
        }

        return true;
    }
};