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

                var arrChoice = [];
                for (var i = 0; i < rs.length; i++) {
                    row = rs[i];
                    if (subTransId == row.SubTransId) {
                        arrChoice.push('<div class="line"><div class="selected">' + row.Choice + '</div></div>');
                    }
                    else {
                        arrChoice.push('<div class="line">' + row.Choice + '</div>');
                    }
                }

                $('#divEvent_' + subTransId).html(arrChoice.join('')).show();
                $("#ticketInfo").fanexGrid('resizeGrid');

            },
            error: function (e) {},
            complete: function (e) {
                if (e.status == 403) {
                    NapTicketControl.Helper.showMsgBox(NapTicketControl.Helper.authorizedMsg);
                }
                NapTicketControl.enableInput();
            }
        });
    }
};