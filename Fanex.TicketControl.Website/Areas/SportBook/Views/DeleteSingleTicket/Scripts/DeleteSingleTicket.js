//Namespace.Register("Fanex.TicketControl.SportBook.DeleteSingleTicket");
//Fanex.TicketControl.SportBook.DeleteSingleTicket = function () {
//    var self = this;
//
//}

$(document).ready(function () {
    test.selBettype();
});

var test = {

    TicketControlMode : {
        SingleTicket : 0,
        NumberGame : 1,
        EarlyKO : 2,
        TicketListing : 3,
        MixParlay : 4
    },

    selBettype: function () {
        $.ajax({
            type: "POST",
            url: window.rootUrl + "SportBook/DeleteSingleTicket/SelBettype",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(test.TicketControlMode.SingleTicket),
            beforeSend: function () {
              
            },
            success: function (rs) {
                alert(rs);
            },
            error: function (event, jqXHR, ajaxSettings) {
            
            },
            complete: function () {
                
            }
        });
    }


};