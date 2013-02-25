/* File Created: January 28, 2013 */

var NapTicketControl = function ($) {
    var MessageType = {
        Success: 1,
        Warning: 2,
        Error: 3
    };


    return {
        miniMenu: null,

        init: function () {
            NapTicketControl.miniMenu = $('header .mini-menu').change(function () {
                window.location.replace($(this).val());
            });
        },

        disableInput: function () {
            try { $.blockUI(); } catch (e) { }
        },
        enableInput: function () {
            try { $.unblockUI(); } catch (e) { }
        },
        popUp: {
            arrayHighLightControl: [],
            show: function (message, targets, messageType) {
                NapTicketControl.NapTicketControlPopUp
                .removeClass("err")
                .removeClass("info")
                .empty()
                .text(message);

                var bodyWith = $(body).width();
                var popUpWidth = NapTicketControl.NapTicketControlPopUp.width();
                var leftPosition = Math.round((bodyWith - popUpWidth) / 2);
                NapTicketControl.NapTicketControlPopUp.css("left", leftPosition);
                //// 1: Infor, 2: Warning, 3: undefined, Error
                if (typeof (messageType) == 'undefined') {
                    NapTicketControl.NapTicketControlPopUp
                    .addClass("err")
                    .append('<a class="close" title="Close Message" href="javascript:NapTicketControl.popUp.hide();"></a>');

                    if (typeof (targets) != 'undefined') {
                        NapTicketControl.popUp.highLightControl(targets);
                    }
                }
                else {
                    switch (messageType) {
                        case MessageType.Success: ////Success
                            NapTicketControl.NapTicketControlPopUp.addClass("info");
                            setTimeout(function () {
                                NapTicketControl.NapTicketControlPopUp.hide();
                            }, 4000);
                            break;
                        case MessageType.Warning:
                            break;
                        case MessageType.Error:
                            break;
                    }
                }

                NapTicketControl.NapTicketControlPopUp.show();
            },
            hide: function () {
                NapTicketControl.NapTicketControlPopUp.hide();
            },
            highLightControl: function (targets) {
                var controlIds = targets.split(',');
                for (var i = 0; i < controlIds.length; i++) {
                    var controlId = controlIds[i];
                    if (controlId.indexOf("#") < 0) {
                        controlId = "#" + controlId;
                    }
                    NapTicketControl.popUp.arrayHighLightControl.push(controlId);
                    $(controlId).addClass("input-validation-error");
                }

            },
            removeHighLight: function () {
                var controlIds = NapTicketControl.popUp.arrayHighLightControl;
                for (var i = 0; i < controlIds.length; i++) {
                    $(controlIds[i]).removeClass("input-validation-error");
                }
                NapTicketControl.popUp.arrayHighLightControl = [];
                NapTicketControl.popUp.hide();
            },
            reCheckIn: function () {
                $.ajax({
                    url: NapTicketControl.NapTicketControlRoot + "UserLogin/Login/ReCheckLogin",
                    type: "POST",
                    dataType: "json",
                    success: function (result) {
                        if (typeof (result) != 'undefined' && result != null) {
                            if (result.IsKickedOut == "1" || result.IsKickedOut == "2") {
                                alert('Another session has been logged in or your session is expired. Forced to logout!');
                                window.location.href = NapTicketControl.NapTicketControlRoot + "UserLogin/Login/LogOut";
                            }
                            else if (result.IsKickedOut == "3") {
                                window.location.href = NapTicketControl.NapTicketControlRoot + "UserLogin/Common/Offline";
                            }
                        }
                    },
                    error: function (request, status, error) {
                    }
                });
            }
        }, 

    };
} (jQuery);

$(document).ready(function () {
    NapTicketControl.init();
});