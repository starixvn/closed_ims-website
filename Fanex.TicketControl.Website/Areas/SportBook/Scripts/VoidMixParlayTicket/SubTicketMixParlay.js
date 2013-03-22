/* File Created: March 7, 2013 */

function showMP (subTransId, refNo) {
    subTicketMP.getSubTicketMP(subTransId, refNo);
}

function showCombMP(subTransId, type, refNo) {
    //type un-used
    subTicketMP.getSubTicketMP(subTransId, refNo);
}

var subTicketMP = {
    getSubTicketMP: function (subTransId, refNo) {
        var param = {
            RefNo: refNo,
            TransId: subTransId
        };

        var subTicket = $('#divEvent_' + subTransId);

        

        if (subTicket.html() != '') {
            var display = subTicket.css('display');
            if (display == 'block') {
                subTicket.hide();
            }
            else {
                subTicket.show();
            }

            $("#ticketInfo").fanexGrid('resizeGrid');
            return;
        }

        $.ajax({
            type: "POST",
            url: window.rootUrl + "SportBook/VoidMixParlayTicket/GetParlaySubTicket",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            beforeSend: function () {
                NapTicketControl.disableInput();
            },
            success: function (rs) {
                $('#divEvent_' + subTransId).html(rs).show();
                $("#ticketInfo").fanexGrid('resizeGrid');
        
            },
            error: function () {
                console.log('has error :(');
            },
            complete: function () {
                NapTicketControl.enableInput();
            }
        });
    }
};