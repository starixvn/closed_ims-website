/* File Created: February 5, 2013 */
function TicketListing() {
    this.DOMElement =
    {
        $ddlLiccensee: null,
        $ddlEventType: null,
        $product: null,
        $bettypeWrp: null,
        $chkTransDateRange: null,
        $txtCalendar: null,
        $optFilters: null,
        $eventTypeWrp: null
    };
};

TicketListing.prototype.Mode = {
    Sportsbook: 0,
    NumberGame: 1,
    VirtualSports: 5,
    LiveCasino: 6
};

TicketListing.prototype.BetTypes = {
    Sportsbook: null,
    NumberGame: null,
    VirtualSports: null,
    LiveCasino: null
};

TicketListing.prototype.CacheDOMElement = function () {
    var self = this;

    self.DOMElement.$ddlLiccensee = $('.ddlLicensee');
    self.DOMElement.$ddlEventType = $('.ddlEventType');
    self.DOMElement.$product = $('.product');
    self.DOMElement.$bettypeWrp = $('.bettype-wrp');
    self.DOMElement.$chkTransDateRange = $('.chkTransDateRange');
    self.DOMElement.$txtCalendar = $('.txtCalendar');
    self.DOMElement.$optFilters = $('.opt-filters');
    self.DOMElement.$eventTypeWrp = $('.event-type-wrp');
}

TicketListing.prototype.Init = function () {
    var self = this;

    self.CacheDOMElement();
    self.GetLicensees();
    self.GetBettypes('Sportsbook');

    self.SetupCalendar('', '.txtCalendar');
    // handle events from DOM element

    // change live type
    self.DOMElement.$ddlEventType.change(function () {
        var eventType = $(this).val(); // live type drop down list

        if (eventType === '0') {
            self.DOMElement.$optFilters.show();
        } else {
            self.DOMElement.$optFilters.hide();
        }

        // render bet type which correspond to with selected eventType
        //        if (TicketListing.prototype.BetTypes['Sportsbook'] === null) {
        //            TicketListing.prototype.GetBettypes('Sportsbook');
        //        } else {
        //            var html = self.RenderBettypes('Sportsbook', eventType);
        //            self.DOMElement.$controlBettypes.html(html);
        //        }
    });

    // enable/disable Trans Date
    self.DOMElement.$chkTransDateRange.click(function () {
        if ($(this).prop('checked')) {
            // fill in current date
            var today = new Date();
            self.SetupCalendar(today.format('m/d/Y H:i:s'), '.txtCalendar');
        } else {
            // clear text box
            $('.txtCalendar').val('');
        }
    });

    // choose another product
    self.DOMElement.$product.click(function () {
        var product = $(this).val();

        if (product === 'Sportsbook') {
            // show Event Type drop down
            var eventType = self.DOMElement.$ddlEventType.val();
            self.DOMElement.$eventTypeWrp.show();

            if (eventType == 0) {
                self.DOMElement.$optFilters.show();
            } else {
                self.DOMElement.$optFilters.hide();
            }
        } else {
            // hide Live Type drop down                
            self.DOMElement.$eventTypeWrp.hide();
            self.DOMElement.$optFilters.hide();
        }

        // get the list of bet types belong selected product        
//        if (TicketListing.prototype.BetTypes[product] === null) {
//            self.GetBettypes(product);
//        }
//        else {
//            var livetype = $('#ddlLiveType').val();
//            var html = self.RenderBettypes(product, livetype);
//            self.DOMElement.$bettypes.html(html);
//        }
    });

    // check all bettypes
    $('body').delegate('#chkallbettype', 'click', function () {
        if ($('#chkallbettype').prop('checked')) {
            $('.bettype').attr('checked', 'checked')
        } else {
            $('.bettype').removeAttr('checked');
        }
    });

    $('body').delegate('.bettype', 'click', function () {
        var selectedItems = $('#tbl-bettype tbody input:checked').length;
        var totalItems = $('#tbl-bettype tbody input:checkbox').length;

        if (selectedItems === totalItems) {
            $('#chkallbettype').attr('checked', 'checked');
        } else {
            $('#chkallbettype').removeAttr('checked');
        }
    });

    // submit data to server
    $('input[type="submit"]').click(function () {
        var params = $("#TicketListingForm").serialize();
    });
};

TicketListing.prototype.GetLicensees = function () {
    var self = this;

    $.ajax({
        type: "POST",
        url: window.rootUrl + "Misc/TicketListing/GetLicensees",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: null,
        beforeSend: function () {
            NapTicketControl.disableInput();
        },
        success: function (licensees) {
            if (licensees) {
                self.RenderLicenseeDropDownList(licensees);
            }
        },
        error: function () {

        },
        complete: function () {
            NapTicketControl.enableInput();
        }
    });
};

TicketListing.prototype.RenderLicenseeDropDownList = function (licensees) {
    var self = this;
    var licenseeLength = licensees.length;
    var $licensee = self.DOMElement.$ddlLiccensee;
    var licenseeHtml = '';
    
    for (var index = 0; index < licenseeLength; index++) {
        var currentLicensee = licensees[index];
        licenseeHtml += '<option value="' + currentLicensee.Id + '">' + currentLicensee.Name + '</option>';
    }

    $licensee.append("<optgroup label='Licensee'></optgroup>");
    $licensee.find('optgroup').append(licenseeHtml);
};

TicketListing.prototype.GetBettypes = function (product) {
    var self = this;

    var param = {
        //mode: TicketListing.prototype.Mode[product]
        mode: product
    };

    $.ajax({
        type: "POST",
        url: window.rootUrl + "Misc/TicketListing/GetBettypes",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(param),
        beforeSend: function () {
            NapTicketControl.disableInput();
        },
        success: function (bettypes) {
            if (bettypes) {
                TicketListing.prototype.BetTypes[product] = bettypes;
                var livetype = $('#ddlLiveType').val();
                var html = TicketListing.prototype.RenderBettypes(product, livetype);
                self.DOMElement.$bettypeWrp.html(html);
            }
        },
        error: function () {

        },
        complete: function () {
            NapTicketControl.enableInput();
        }
    });
};

// Render Bettypes:
// product: selected product
// eventType: only affect when product is SB (0: dead ball, 1: live ball, 2: all)
TicketListing.prototype.RenderBettypes = function (product, eventType) {
    var self = this;

    // get the bettype list from the storage object
    var bettypes = TicketListing.prototype.BetTypes[product];
    // filter by ball status
    var deadBallBettypes = $.grep(bettypes, function (value) {
        return value.LiveIndicator === false;
    });
    var liveBallBettypes = $.grep(bettypes, function (value) {
        return value.LiveIndicator === true;
    });

    var html = '<table id="tbl-bettype">';
    html += '<thead><tr><td colspan="5">'
        + '<input type="checkbox" id="chkallbettype" checked="checked" />'
        + '<label for="chkallbettype"> All Bet types'
        + '</td></tr></thead>';

    switch (product) {
        case 'Sportsbook':
            switch (eventType) {
                case '0':
                    // render only dead ball bettypes
                    html += self.RenderBettypesDetail(deadBallBettypes, 'BetTypes');
                    break;
                case '1':
                    // render only live ball bettypes
                    html += self.RenderBettypesDetail(liveBallBettypes, 'BetTypes');
                    break;
                default:
                    // render all bettypes
                    html += self.RenderBettypesDetail(bettypes, 'BetTypes');
                    break;
            }
            break;
        case 'NumberGame':
            // render dead ball bettypes
            html += '<tr class="betgroup-title"> <td colspan="5">' + 'Dead ball' + '</td></tr>';
            html += self.RenderBettypesDetail(deadBallBettypes, 'BetTypes', 0);

            // render live ball bettype
            html += '<tr class="betgroup-title"> <td colspan="5">' + 'Live ball' + '</td></tr>';
            html += self.RenderBettypesDetail(liveBallBettypes, 'LiveBetTypes', deadBallBettypes.length);
            break;
        default:
            html += self.RenderBettypesDetail(bettypes, 'BetTypes');
            break;
    }

    html += '</table>';

    return html;
};

TicketListing.prototype.RenderBettypesDetail = function (bettypes, name, startPosition) {
    var length = bettypes.length;
    var step = 5;
    var repeatTime = length / step;
    var html = '';

    if(startPosition === undefined){
        startPosition = 0;
    }

    for (var i = 0; i < repeatTime; i++) {
        var offset = i * step;
        html += '<tr>';
        for (var j = 0; j < step; j++) {
            if (offset + j < length) {
                var bt = bettypes[offset + j];
                var id = 'chkbettype_' + startPosition;

                html += '<td>';
                html += '<input type="checkbox" checked="checked" class="chkbettype"'
                    + ' id=' + id + ' value=' +  bt.Id  + ' name=' + name + ' />';
                html += '<label for="' + id + '">' + bt.Name + '</label>';
                html += '</td>';
            } else {
                html += '<td>&nbsp;</td>';
            }

            startPosition ++;
        }
        html += '</tr>';
    }

    return html;
}

TicketListing.prototype.SetupCalendar = function (strdatetime, txtdate) {
    $(txtdate).datetimepicker({
        showSecond: true,
        timeFormat: 'hh:mm:ss',   
        hourGrid: 3,
        minuteGrid: 5,
        secondGrid: 5
    });

    if (strdatetime != null && strdatetime != "") {
        $(txtdate).val(strdatetime).datetimepicker('setDate', (new Date(strdatetime)));
    }
}

TicketListing.prototype.CollectData = function () {
    var params = $("#TicketListingForm").serialize();
}



