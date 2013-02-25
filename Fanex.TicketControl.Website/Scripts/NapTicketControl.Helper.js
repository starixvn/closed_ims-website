var Helper = (function ($) {

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