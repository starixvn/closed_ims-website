/* File Created: March 8, 2013 */

 $(document).ready(function () {
    recoverTicket.init();
 });

 var recoverTicket = {
     init: function () {
         jQuery("#RecoverTicketForm").validationEngine();
         $("#txtBetId").keyup(function () { }).ForceIntegerOnly();

//         $('body').delegate('#btnSearch', 'click', function () {
//             recoverTicket.search();
//         });
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

     search: function () {
         if (!recoverTicket.validData()) {
             return;
         }

         alert('gogo');
     }
 };