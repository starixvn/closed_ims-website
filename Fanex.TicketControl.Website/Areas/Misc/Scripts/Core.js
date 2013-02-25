var Core = (function ($) {
    // define some private variables

    var _productTypes = {
        Sportsbook: 0,
        NumberGame: 1,
        VirtualSports: 5,
        LiveCasino: 6
    };

    // used to store local bettypes, 
    // and avoid request to server when they are avaible
    var _betTypes = {
        Sportsbook: null,
        NumberGame: null,
        VirtualSports: null,
        LiveCasino: null
    }

    return {

        GetLicensees: function () {
            $.ajax({
                type: "POST",
                url: window.rootUrl + "Core/Common/GetLicensees",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: null,
                beforeSend: function () {
                    NapTicketControl.disableInput();
                },
                success: function (licensees) {
                    if (licensees) {
                        Core.RenderLicenseeDropDownList(licensees);
                    }
                },
                error: function () {

                },
                complete: function () {
                    NapTicketControl.enableInput();
                }
            });
        },

        RenderLicenseeDropDownList: function (licensees) {
            var self = this;
            var licenseeLength = licensees.length;
            var $licensee = $('.ddlLicensee');
            var licenseeHtml = '';

            for (var index = 0; index < licenseeLength; index++) {
                var currentLicensee = licensees[index];
                licenseeHtml += '<option value="' + currentLicensee.Id + '">' + currentLicensee.Name + '</option>';
            }

            $licensee.append("<optgroup label='Licensee'></optgroup>");
            $licensee.find('optgroup').append(licenseeHtml);
        },

        GetBettypes: function (product, eventType) {
            var self = this;

            // get the list of bet types belong selected product        
            if (_betTypes[product] !== null) {                
                var html = self.RenderBettypes(product, eventType);
                $('.bettype-wrp').html(html);
            }
            else {

                var param = {
                    mode: product
                };

                $.ajax({
                    type: "POST",
                    url: window.rootUrl + "Core/Common/GetBettypes",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(param),
                    beforeSend: function () {
                        NapTicketControl.disableInput();
                    },
                    success: function (bettypes) {
                        if (bettypes) {
                            _betTypes[product] = bettypes;
                            var html = self.RenderBettypes(product, eventType);
                            $('.bettype-wrp').html(html);
                        }
                    },
                    error: function () {

                    },
                    complete: function () {
                        NapTicketControl.enableInput();
                    }
                });
            }
        },

        // Render Bettypes:
        // product: selected product
        // eventType: only affect when product is Sportsbook (0: dead ball, 1: live ball, 2: all)
        RenderBettypes: function (product, eventType) {
            var self = this;

            // get the bettype list from the storage object
            var bettypes = _betTypes[product];
            // filter by ball status
            var deadBallBettypes = $.grep(bettypes, function (value) {
                return value.LiveIndicator === false;
            });
            var liveBallBettypes = $.grep(bettypes, function (value) {
                return value.LiveIndicator === true;
            });

            var html = '<table class="tbl-bettype">';
            html += '<thead><tr><th colspan="5">'
                + '<input type="checkbox" id="chkallbettype" checked="checked" />'
                + '<label for="chkallbettype"><b>All Bet types</b>'
                + '</th></tr></thead>';

            switch (product) {
                case 'Sportsbook':
                    switch (eventType) {
                        case '1':
                            // render only dead ball bettypes
                            html += this.RenderBettypesDetails(deadBallBettypes, 'BetTypes');
                            break;
                        case '0':
                            // render only live ball bettypes
                            html += this.RenderBettypesDetails(liveBallBettypes, 'BetTypes');
                            break;
                        default:
                            // render all bettypes
                            html += this.RenderBettypesDetails(bettypes, 'BetTypes');
                            break;
                    }
                    break;
                case 'NumberGame':
                    // render dead ball bettypes
                    html += '<tr class="bettype-group-title"> <td colspan="5">' + 'Dead ball' + '</td></tr>';
                    html += this.RenderBettypesDetails(deadBallBettypes, 'BetTypes', 0);

                    // render live ball bettype
                    html += '<tr class="bettype-group-title"> <td colspan="5">' + 'Live ball' + '</td></tr>';
                    html += this.RenderBettypesDetails(liveBallBettypes, 'LiveBetTypes', deadBallBettypes.length);
                    break;
                default:
                    html += this.RenderBettypesDetails(bettypes, 'BetTypes');
                    break;
            }

            html += '</table>';

            return html;
        },

        RenderBettypesDetails: function (bettypes, nameValue, startPosition) {
            var length = bettypes.length;
            var step = 5;
            var repeatTime = length / step;
            var html = '';

            if (startPosition === undefined) {
                startPosition = 0;
            }

            for (var i = 0; i < repeatTime; i++) {
                var offset = i * step;
                html += '<tr>';
                for (var j = 0; j < step; j++) {

                    if (offset + j < length) {
                        var bt = bettypes[offset + j];
                        var id = 'chk-bettype-' + startPosition;
                        var betname = bt.Name;

                        switch (bt.Id) {
                            case 85:
                                betname = "Next Over/Under";
                                break;
                            case 86:
                                if (bt.LiveIndicator === true) {
                                    betname = "Next Odd/Even";
                                } else {
                                    betname = "FT. Odd/Even";
                                }
                                break;
                            case 90:
                                if (bt.LiveIndicator === true) {
                                    betname += " (Live Game)";
                                } else {
                                    betname += " (Pre Game)";
                                }
                                break;
                        }
                        html += '<td>';
                        html += '<input type="checkbox" checked="checked" class="chkbettype"'
                            + ' id=' + id + ' value=' + bt.Id + ' name=' + nameValue + ' />';
                        html += '<label for="' + id + '">' + betname + '</label>';
                        html += '</td>';
                    } else {
                        html += '<td>&nbsp;</td>';
                    }

                    startPosition++;
                }
                html += '</tr>';
            }

            return html;
        }
    };
} (jQuery));



// Validators library
var Validators = {
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
            var pos = Validators.utils.findPos(elementInput);
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
