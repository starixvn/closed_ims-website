/* File Created: January 28, 2013 */

var AccReport = function ($) {
    var MessageType = {
        Success: 1,
        Warning: 2,
        Error: 3
    };
    return {
        disableInput: function () {
            try { $.blockUI(); } catch (e) { }
        },
        enableInput: function () {
            try { $.unblockUI(); } catch (e) { }
        },
        popUp:
        {
            arrayHighLightControl: [],
            show: function (message, targets, messageType) {
                AccReport.accReportPopUp
                .removeClass("err")
                .removeClass("info")
                .empty()
                .text(message);

                var bodyWith = $(body).width();
                var popUpWidth = AccReport.accReportPopUp.width();
                var leftPosition = Math.round((bodyWith - popUpWidth) / 2);
                AccReport.accReportPopUp.css("left", leftPosition);
                //// 1: Infor, 2: Warning, 3: undefined, Error
                if (typeof (messageType) == 'undefined') {
                    AccReport.accReportPopUp
                    .addClass("err")
                    .append('<a class="close" title="Close Message" href="javascript:AccReport.popUp.hide();"></a>');

                    if (typeof (targets) != 'undefined') {
                        AccReport.popUp.highLightControl(targets);
                    }
                }
                else {
                    switch (messageType) {
                        case MessageType.Success: ////Success
                            AccReport.accReportPopUp.addClass("info");
                            setTimeout(function () {
                                AccReport.accReportPopUp.hide();
                            }, 4000);
                            break;
                        case MessageType.Warning:
                            break;
                        case MessageType.Error:
                            break;
                    }
                }

                AccReport.accReportPopUp.show();
            },
            hide: function () {
                AccReport.accReportPopUp.hide();
            },
            highLightControl: function (targets) {
                var controlIds = targets.split(',');
                for (var i = 0; i < controlIds.length; i++) {
                    var controlId = controlIds[i];
                    if (controlId.indexOf("#") < 0) {
                        controlId = "#" + controlId;
                    }
                    AccReport.popUp.arrayHighLightControl.push(controlId);
                    $(controlId).addClass("input-validation-error");
                }

            },
            removeHighLight: function () {
                var controlIds = AccReport.popUp.arrayHighLightControl;
                for (var i = 0; i < controlIds.length; i++) {
                    $(controlIds[i]).removeClass("input-validation-error");
                }
                AccReport.popUp.arrayHighLightControl = [];
                AccReport.popUp.hide();
            },
            reCheckIn: function () {
                $.ajax({
                    url: AccReport.accReportRoot + "UserLogin/Login/ReCheckLogin",
                    type: "POST",
                    dataType: "json",
                    success: function (result) {
                        if (typeof (result) != 'undefined' && result != null) {
                            if (result.IsKickedOut == "1" || result.IsKickedOut == "2") {
                                alert('Another session has been logged in or your session is expired. Forced to logout!');
                                window.location.href = AccReport.accReportRoot + "UserLogin/Login/LogOut";
                            }
                            else if (result.IsKickedOut == "3") {
                                window.location.href = AccReport.accReportRoot + "UserLogin/Common/Offline";
                            }
                        }
                    },
                    error: function (request, status, error) {
                    }
                });
            }
        }
    };
} (jQuery);