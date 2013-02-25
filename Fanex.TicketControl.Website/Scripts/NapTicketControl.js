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

        add: function (elementInput, expr, message) {
            // Bind onchange event of input
            // Validate with expr
            // Show message and animate background color
        },
        show: function (elementInput, message, xPos, pointerXPos, yPos, pointerYPos, height, timeout) {
            // <span class="hint"><span>This is the message.<span><span class="hint-pointer">&nbsp;</span></span>
            var hint = document.getElementById('fHint');
            var hintPointer = null;
            if (hint == null) {
                var hint = document.createElement("SPAN");
                hint.className = 'hint';
                var hintMsg = document.createElement("SPAN");
                hint.appendChild(hintMsg);
                hintPointer = document.createElement("SPAN");
                hintPointer.className = 'hint-pointer';
                hint.appendChild(hintPointer);
                hint.id = 'fHint';
                hint.style.top = '10px';
                hint.style.left = '10px';
                hint.onclick = function () { // Hide onclick
                    hint.style.display = 'none';
                };
                document.body.appendChild(hint);
            };
            hintPointer = hint.lastChild;
            hint.firstChild.innerHTML = message;
            if (this.timeOutHideTooltip) {
                clearTimeout(this.timeOutHideTooltip);
                this.timeOutHideTooltip = null;
            };
            if (typeof (timeout) == 'undefined') {
                timeout = 10000;
            }
            this.timeOutHideTooltip = setTimeout(function () {
                if (hint == null) {
                    return;
                }
                hint.style.display = 'none';
            }, timeout); // Auto hide in 10s
            if (elementInput != null) {
                if (typeof xPos == 'undefined') {
                    xPos = 0;
                }
                if (typeof pointerXPos == 'undefined') {
                    pointerXPos = 10;
                }
                if (typeof yPos == 'undefined') {
                    yPos = -45;
                }
                if (typeof (pointerYPos) == 'undefined') {
                    pointerYPos = 38;
                }
                if (typeof (height) == 'undefined') {
                    height = 18;
                }
                hint.style.height = height + 'px';
                hintPointer.style.left = (pointerXPos + 'px');
                hintPointer.style.top = (pointerYPos + 'px');
                var pos = NapTicketControl.utils.findPos(elementInput);
                hint.style.left = (pos[0] + xPos) + 'px';
                hint.style.top = (pos[1] + yPos) + 'px';
                elementInput.onchange = function () {
                    hint.style.display = 'none';
                };
                elementInput.focus();
            };
            hint.style.display = 'block';
        },
        hide: function () {
            var hint = document.getElementById('fHint');
            if (hint == null) {
                return;
            }
            hint.style.display = 'none';
        },
        animate: function (input) {
            var previousColor = input.style.color;
            var previousBackground = input.style.backgroundColor;
            var c = 4;
            if (this.intervalAnimate) {
                clearInterval(this.intervalAnimate);
                this.intervalAnimate = null;
            }
            var int = this.intervalAnimate = setInterval(function () {
                c--;
                if (input.style.backgroundColor == previousBackground) {
                    input.style.backgroundColor = '#ff7700';
                    input.style.color = '#ffffff';
                } else {
                    input.style.backgroundColor = previousBackground;
                    input.style.color = previousColor;
                }
                if (c == 0) {
                    clearInterval(int);
                }
            }, 300);
        },
        utils: {
            // findPos() by quirksmode.org
            // Finds the absolute position of an element on a page
            findPos: function (obj) {
                var curleft = curtop = 0;
                if (obj.offsetParent) {
                    do {
                        curleft += obj.offsetLeft;
                        curtop += obj.offsetTop;
                    } while (obj = obj.offsetParent);
                }
                return [curleft, curtop];
            },
            // getPageScroll() by quirksmode.org
            // Finds the scroll position of a page
            getPageScroll: function () {
                var xScroll, yScroll;
                if (self.pageYOffset) {
                    yScroll = self.pageYOffset;
                    xScroll = self.pageXOffset;
                } else if (document.documentElement && document.documentElement.scrollTop) {
                    yScroll = document.documentElement.scrollTop;
                    xScroll = document.documentElement.scrollLeft;
                } else if (document.body) { // all other Explorers
                    yScroll = document.body.scrollTop;
                    xScroll = document.body.scrollLeft;
                }
                return [xScroll, yScroll]
            },
            // Finds the position of an element relative to the viewport.
            findPosRelativeToViewport: function (obj) {
                var objPos = this.findPos(obj)
                var scroll = this.getPageScroll()
                return [objPos[0] - scroll[0], objPos[1] - scroll[1]]
            }
        }
    };

} (jQuery);

$(document).ready(function () {
    NapTicketControl.init();
});