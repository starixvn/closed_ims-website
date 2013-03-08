/* File Created: March 8, 2013 */

$(function () {
    recoverTicket.init();
});
var recoverTicket = {
    init: function () {
        jQuery("#RecoverTicketForm").validationEngine();
        jQuery("#VoidMixParlayTicketForm").validationEngine();
        $("#txtBetId").keyup(function () { }).ForceIntegerOnly();

        $('body').delegate('#btnSearch', 'click', function (e) {
            e.stopPropagation();
            recoverTicket.search(e);
            
        });
    },

    validData: function () {
        if ($('#txtBetId').val() == '') {
            $('#msg').html(tooltip.show('txtBetId', 'Bet ID is required.')).show();
            return false;
        }

        if ($('#txtBetId').val().length > 10 || isNaN($('#txtBetId').val())) {
            $('#msg').html(tooltip.show('txtBetId', 'Invalid Bet ID.')).show();
            return false;
        }
        return true;
    },

    search: function (e) {
        if (!recoverTicket.validData(e)) {
            return;
        }

        alert('gogo');
    }
};