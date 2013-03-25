var helper = {

    setStatusAllBettype: function (isChecked) {
        if (isChecked) {
            $('.chkbettype').attr('checked', 'checked')
            $('#chkallbettype').attr('checked', 'checked')
            helper.setStatusAllAdvanceCS(true, true);
            helper.setStatusAllAdvanceTG(true, true);
            helper.setStatusAllAdvanceHDP(true);
            helper.setStatusAllAdvance1X2(true);
            helper.setStatusAllAdvanceHTFT(true, true);
            helper.setStatusAllAdvanceFGLG(true, true);
        } else {
            $('.chkbettype').removeAttr('checked');
            $('#chkallbettype').removeAttr('checked');
            helper.setStatusAllAdvanceCS(false, false);
            helper.setStatusAllAdvanceTG(false, false);
            helper.setStatusAllAdvanceHDP(false);
            helper.setStatusAllAdvance1X2(false);
            helper.setStatusAllAdvanceHTFT(false, false);
            helper.setStatusAllAdvanceFGLG(false, false);
        }
    },

    setStatusAllAdvanceCS: function (isChecked, isEnable) {
        if (isChecked) {
            $('.chk-score').attr('checked', 'checked');
            //            helper.expandAdvancedMode();
        } else {
            $('.chk-score').removeAttr('checked');
        }

        $('.chk-score').attr('disabled', !isEnable);
    },

    eventSubAdvanceCS: function () {
        var selectedItems = $('.all-chk-score .chk-score:input:checked').length;
        var totalItems = $('.all-chk-score .chk-score:input:checkbox').length;

        if (selectedItems === totalItems) {
            $('#chk-score-all').attr('checked', 'checked');
        } else {
            $('#chk-score-all').removeAttr('checked');
        }
    },

    setStatusAllAdvanceTG: function (isChecked, isEnable) {
        if (isChecked) {
            $('.chk-tg').attr('checked', 'checked');
            //            helper.expandAdvancedMode();
        } else {
            $('.chk-tg').removeAttr('checked');
        }

        $('.chk-tg').attr('disabled', !isEnable);
    },

    eventSubAdvanceTG: function () {
        var selectedItems = $('.advance-item-tg .chk-tg:input:checked').not('#chk-tg-all').length;
        var totalItems = $('.advance-item-tg .chk-tg:input:checkbox').not('#chk-tg-all').length;

        if (selectedItems === totalItems) {
            $('#chk-tg-all').attr('checked', 'checked');
        } else {
            $('#chk-tg-all').removeAttr('checked');
        }
    },

    setStatusAllAdvanceHDP: function (isEnable) {
        $('#ddlBetTeam1378').attr('disabled', !isEnable);
        $('#hdp').attr('disabled', !isEnable);

        if (isEnable) {
            //            helper.expandAdvancedMode();
        }
    },

    setStatusAllAdvance1X2: function (isEnable) {
        $('#ddlBetTeam515').attr('disabled', !isEnable);

        if (isEnable) {
            //            helper.expandAdvancedMode();
        }
    },

    setStatusAllAdvanceHTFT: function (isChecked, isEnable) {
        if (isChecked) {
            $('.chk-htft').attr('checked', 'checked');
            //            helper.expandAdvancedMode();
        } else {
            $('.chk-htft').removeAttr('checked');
        }

        $('.chk-htft').attr('disabled', !isEnable);
    },

    eventSubAdvanceHTFT: function () {
        var selectedItems = $('.advance-item-htft .chk-htft:input:checked').not('#chk-htft-all').length;
        var totalItems = $('.advance-item-htft .chk-htft:input:checkbox').not('#chk-htft-all').length;

        if (selectedItems === totalItems) {
            $('#chk-htft-all').attr('checked', 'checked');
        } else {
            $('#chk-htft-all').removeAttr('checked');
        }
    },

    setStatusAllAdvanceFGLG: function (isChecked, isEnable) {
        if (isChecked) {
            $('.chk-flg').attr('checked', 'checked');
            //            helper.expandAdvancedMode();
        } else {
            $('.chk-flg').removeAttr('checked');
        }

        $('.chk-flg').attr('disabled', !isEnable);
    },

    eventSubAdvanceFGLG: function () {
        var selectedItems = $('.advance-item-flg .chk-flg:input:checked').not('#chk-flg-all').length;
        var totalItems = $('.advance-item-flg .chk-flg:input:checkbox').not('#chk-flg-all').length;

        if (selectedItems === totalItems) {
            $('#chk-flg-all').attr('checked', 'checked');
        } else {
            $('#chk-flg-all').removeAttr('checked');
        }
    },

    IsSelectedBettype: function (bettypeId) {
        var checkedVals = $('.chkbettype:checkbox:checked').map(function () {
            return this.value;
        }).get();

        var isSelected = false;

        for (var i = 0; i < checkedVals.length; i++) {
            if (checkedVals[i] == bettypeId) {
                isSelected = true;
                break;
            }
        }

        return isSelected;
    },

    GetBettypesChecked: function () {
        var checkedVals = $('.chkbettype:checkbox:checked').map(function () {
            return this.value;
        }).get().join('^');

        return checkedVals;
    },

    GetBetteam430: function () {
        var checkedVals = '';
        checkedVals = $('.all-chk-score .chk-score:checkbox:checked').map(function () {
            return this.value;
        }).get().join('^');

        if ($('#EventType').val() == '1') {
            checkedVals = '';
        }
        return checkedVals;
    },

    GetBetteam6: function () {
        var checkedVals = '';
        checkedVals = $('.chk-tg:checkbox:checked').not('#chk-tg-all').map(function () {
            return this.value;
        }).get().join('^');

        if ($('#EventType').val() == '1') {
            checkedVals = '';
        }

        return checkedVals;
    },

    GetBetteam14: function () {
        var checkedVals = '';
        checkedVals = $('.chk-flg:checkbox:checked').not('#chk-flg-all').map(function () {
            return this.value;
        }).get().join('^');

        if ($('#EventType').val() == '1') {
            checkedVals = '';
        }

        return checkedVals;
    },

    GetBetteam16: function () {
        var checkedVals = '';
        checkedVals = $('.chk-htft:checkbox:checked').not('#chk-htft-all').map(function () {
            return this.value;
        }).get().join('^');

        if ($('#EventType').val() == '1') {
            checkedVals = '';
        }

        return checkedVals;
    },

    GetBetteam515: function () {
        var checkedVals = '';
        if ($('#EventType').val() == '1' || $('#EventType').val() == '2') {
            var IsSelected515 = helper.IsSelectedBettype(5) || helper.IsSelectedBettype(15);
            checkedVals = $('#ddlBetTeam515').val();
        }

        return checkedVals;
    },

    GetLiveScore: function () {
        var param = {
            LiveHomeScore: $('#EventType').val() == '1' ? $('#LiveHomeScore').val() : '',
            LiveAwayScore: $('#EventType').val() == '1' ? $('#LiveAwayScore').val() : '',
            FromTransdate: $('#EventType').val() == '1' ? $('#FromTransDate').val() : '',
            ToTransdate: $('#EventType').val() == '1' ? $('#ToTransDate').val() : ''
        };

        return param;
    },

    setStausAllCheckTicket: function () {
        var isChecked = $('#chk-ticket-all').prop('checked');
        if (isChecked) {
            $('.chk-ticket').attr('checked', 'checked');
        } else {
            $('.chk-ticket').removeAttr('checked');
        }
    },

    eventSubTicket: function () {
        var selectedItems = $('.chk-ticket:input:checked').length;
        var totalItems = $('.chk-ticket:input:checkbox').length;

        if (selectedItems === totalItems) {
            $('#chk-ticket-all').attr('checked', 'checked');
        } else {
            $('#chk-ticket-all').removeAttr('checked');
        }
    },

    getSubTicketsChecked: function () {
        var checkedVals = '';
        checkedVals = $('.chk-ticket:checkbox:checked').not('#chk-ticket-all').map(function () {
            return this.value;
        }).get().join(',');

        return checkedVals;
    },

    displayMoreMsg: function () {
        var symbol = $('#spMoreMsg').html();

        if (symbol == '[+]') {
            $('.other-languages').show();
        }
        else {
            $('.other-languages').hide();
        }

        symbol = symbol == '[-]' ? '[+]' : '[-]';
        $('#spMoreMsg').html(symbol);
    },

    displayFormMsg: function (isShow) {
        if (isShow) {
            $('.form').show();
        }
        else {
            $('.form').hide();
        }
    },

    resetContent: function () {
        $('#txtMatchId').val('');
        $('#fromRefno').val('');
        $('#toRefno').val('');
        $('#toRefno').val('');
        $('#LiveHomeScore').val('');
        $('#LiveAwayScore').val('');
        $('#LiveAwayScore').val('');
        $('#TransDate').removeAttr('checked');

        $('#password').val('');
        $('.msg-void-ticket').val('');
        helper.setStatusAllBettype(false);
        helper.collapseAdvancedMode();
    },

    displayContent: function (isShow) {
        if (isShow) {
            $('#report-content').show();
        }
        else {
            $('#report-content').hide();            
        }
    },

    expandAdvancedMode: function () {
        var iEle = $('.fieldset-advance-mode legend').find('i');
        iEle.removeClass('icon-plus-sign');
        iEle.addClass('icon-minus-sign');
        $('.tbl-advance-mode').show();
        $('.fieldset-advance-mode').removeClass('nb-lrb');
        $('.fieldset-advance-mode legend').attr('title', 'Collapse advanced mode');
    },

    collapseAdvancedMode: function () {
        var iEle = $('.fieldset-advance-mode legend').find('i');
        iEle.removeClass('icon-minus-sign');
        iEle.addClass('icon-plus-sign');
        $('.tbl-advance-mode').hide();
        $('.fieldset-advance-mode').addClass('nb-lrb');
        $(self).attr('title', 'Expand advanced mode');
    },

    toggleAdvancedMode: function (self) {
        // here, self is legend tag
        var iEle = $(self).find('i');

        if (iEle.hasClass('icon-minus-sign')) {
            iEle.removeClass('icon-minus-sign');
            iEle.addClass('icon-plus-sign');
            $('.tbl-advance-mode').hide();
            $('.fieldset-advance-mode').addClass('nb-lrb');
            $(self).attr('title', 'Expand advanced mode');

        } else {
            iEle.removeClass('icon-plus-sign');
            iEle.addClass('icon-minus-sign');
            $('.tbl-advance-mode').show();
            $('.fieldset-advance-mode').removeClass('nb-lrb');
            $(self).attr('title', 'Collapse advanced mode');
        }
    }
};