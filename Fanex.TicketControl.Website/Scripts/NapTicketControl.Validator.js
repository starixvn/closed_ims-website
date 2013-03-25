if (NapTicketControl === undefined) {
    NapTicketControl = {};
}

NapTicketControl.Validator = (function ($) {

    return {
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
                var pos = NapTicketControl.Validator.utils.findPos(elementInput);
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
        },

        validateMessageBeforeVoidTicket: function() {

            if ($("#msg-en").val() == "") {
                $('#msg').html(tooltip.show('msg-en', 'You must enter English message')).show();
                return false;
            }
            if ($("#msg-en").val().indexOf("@@") < 0) {
                $('#msg').html(tooltip.show('msg-en', 'You must include @@ in English message for replacing ticket number')).show();
                return false;
            }
            if ($("#msg-ch").val() == "") {
                $('#msg').html(tooltip.show('msg-ch', 'You must enter Chinese (Mandarin) message')).show();
                return false;
            }
            if ($("#msg-ch").val().indexOf("@@") < 0) {
                $('#msg').html(tooltip.show('msg-ch', 'You must include @@ in Chinese (Mandarin) message for replacing ticket number')).show();
                return false;
            }

            if ($("#msg-cs").val() != '' && $("#msg-cs").val().indexOf("@@") < 0) {
                $('#msg').html(tooltip.show('msg-cs', 'You must include @@ in Chinese (Cantonese) message for replacing ticket number')).show();
                return false;
            }

            if ($("#msg-th").val() != '' && $("#msg-th").val().indexOf("@@") < 0) {
                $('#msg').html(tooltip.show('msg-th', 'You must include @@ in Thai message for replacing ticket number')).show();
                return false;
            }

            if ($("#msg-it").val() != '' && $("#msg-it").val().indexOf("@@") < 0) {
                $('#msg').html(tooltip.show('msg-it', 'You must include @@ in Italian message for replacing ticket number')).show();
                return false;
            }

            if ($("#msg-jp").val() != '' && $("#msg-jp").val().indexOf("@@") < 0) {
                $('#msg').html(tooltip.show('msg-jp', 'You must include @@ in Japanese message for replacing ticket number')).show();
                return false;
            }

            if ($("#msg-es").val() != '' && $("#msg-es").val().indexOf("@@") < 0) {
                $('#msg').html(tooltip.show('msg-es', 'You must include @@ in Spanish message for replacing ticket number')).show();
                return false;
            }

            if ($("#msg-ko").val() != '' && $("#msg-ko").val().indexOf("@@") < 0) {
                $('#msg').html(tooltip.show('msg-ko', 'You must include @@ in Korean message for replacing ticket number')).show();
                return false;
            }

            if ($('#password').val() == '') {
                $('#msg').html(tooltip.show('password', 'Please input password.')).show();
                return false;
            }
            
            return true;
        }
    };
} (jQuery));