var helper = {

    setStatusAllBettype: function (isChecked) {
        if (isChecked) {
            $('.chkbettype').attr('checked', 'checked')
            $('#chkallbettype').attr('checked', 'checked')
        } else {
            $('.chkbettype').removeAttr('checked');
            $('#chkallbettype').removeAttr('checked');
        }
    },

    isSelectedBettype: function (bettypeId) {
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

    getBettypesChecked: function () {
        var checkedVals = $('.chkbettype:checkbox:checked').map(function () {
            return this.value;
        }).get().join('^');

        return checkedVals;
    },

    getDeadBallBetTypes: function () {
        var checkedVals = $('input[name=BetTypes]:checked').map(function () {
            return this.value;
        }).get().join('^');

        return checkedVals;
    },

    getLiveBallBetTypes: function () {
        var checkedVals = $('input[name=LiveBetTypes]:checked').map(function () {
            return this.value;
        }).get().join('^');

        return checkedVals;
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

    displayContent: function (isShow) {
        if (isShow) {
            $('#report-content').show();
        }
        else {
            $('#report-content').hide();
            $('#txtMatchId').val('');
            $('#fromRefno').val('');
            $('#toRefno').val('');
            $('#toRefno').val('');
            helper.setStatusAllBettype(false);
        }
    }
};