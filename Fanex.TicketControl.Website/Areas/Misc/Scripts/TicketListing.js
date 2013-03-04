/* File Created: February 5, 2013 */
function TicketListing() {
    this.DOMElement =
    {
        $ddlEventType: null,
        $product: null,
        $bettypeWrp: null,
        $chkTransDateRange: null,
        $txtCalendar: null,
        $optFilters: null,
        $eventTypeWrp: null,
        $body: null,
    };

    this.OldDataRequest = null;
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

    self.DOMElement.$ddlEventType = $('.ddlEventType');
    self.DOMElement.$product = $('.product');
    self.DOMElement.$bettypeWrp = $('.bettype-wrp');
    self.DOMElement.$chkTransDateRange = $('.chkTransDateRange');
    self.DOMElement.$txtCalendar = $('.txtCalendar');
    self.DOMElement.$optFilters = $('.opt-filters');
    self.DOMElement.$eventTypeWrp = $('.event-type-wrp');
    self.DOMElement.$body = $('#body');
}

TicketListing.prototype.Init = function () {

    jQuery("#TicketListingForm").validationEngine();

    var self = this;

    self.CacheDOMElement();
    Helper.GetLicensees();
    Helper.GetBettypes(self.CollectSelectedProduct());
    self.SetupCalendar('', '.txtCalendar');
    $('.txtCalendar').attr('disabled', 'disabled');

    // handle events from DOM element
    // change live type
    self.DOMElement.$ddlEventType.change(function () {
        var eventType = $(this).val(); // live type drop down list

        if (eventType === '0') {
            self.DOMElement.$optFilters.show();
        } else {
            self.DOMElement.$optFilters.hide();
        }

        Helper.GetBettypes('Sportsbook', eventType);
    });

    // enable/disable Trans Date
    self.DOMElement.$chkTransDateRange.click(function () {
        if ($(this).prop('checked')) {
            // fill in current date
            var today = new Date();
            $('.txtCalendar').removeAttr('disabled');
            self.SetupCalendar(today.format('m/d/Y H:i:s'), '.txtCalendar');

        } else {
            // clear text box
            $('.txtCalendar').val('').attr('disabled', 'disabled');
        }
    });

    // choose another product
    self.DOMElement.$product.click(function () {
        var product = $(this).val();
        var eventType = self.DOMElement.$ddlEventType.val();

        if (product === 'Sportsbook') {
            // show Event Type drop down
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

        Helper.GetBettypes(product, eventType);
    });

    // check all bettypes
    $('body').delegate('#chkallbettype', 'click', function () {
        if ($('#chkallbettype').prop('checked')) {
            $('.chkbettype').attr('checked', 'checked')
        } else {
            $('.chkbettype').removeAttr('checked');
        }
    });

    $('body').delegate('.chkbettype', 'click', function () {
        var selectedItems = $('.tbl-bettype tbody input:checked').length;
        var totalItems = $('.tbl-bettype tbody input:checkbox').length;

        if (selectedItems === totalItems) {
            $('#chkallbettype').attr('checked', 'checked');
        } else {
            $('#chkallbettype').removeAttr('checked');
        }
    });

    // submit data to server
    $('.submit').click(function () {
        if ($("#TicketListingForm").validationEngine('validate') === true){

            var url = $("#TicketListingForm").attr('action');
            
            // should save old params
            var type = self.CollectSelectedProduct();
            var bettypes = self.CollectSelectedBettypes();
            var livebettypes = self.CollectSelectedLiveBettypes();

            var dataReq = {
                MatchID: $('#MatchID').val(),
                LicenseeID: $('#LicenseeID').val(),
                LiveType: $('#LiveType').val(),
                LiveHomeScore: $('#LiveHomeScore').val(),
                LiveAwayScore: $('#LiveAwayScore').val(),
                FromTransDate: $('#FromTransDate').val(),
                ToTransDate: $('#ToTransDate').val(),
                Type: type,
                BetTypes: bettypes,
                BetTypesLive: livebettypes
            };

            if(dataReq.BetTypesLive.length === 0)
            {
                delete dataReq.BetTypesLive;
            }
            
            // save old data request, to serve for follow-up requests
            self.OldDataRequest = dataReq;

            $.ajax({
                type: "POST",
                url: url,           
                contentType: "application/json; charset=utf-8", 
                dataType: "json",
                data: JSON.stringify(dataReq),
                beforeSend: function () {
                    NapTicketControl.disableInput();
                },
                success: function (ticketlisting) {
                    // check result to see any error from server
                    if (ticketlisting.Result.ErrorCode !== 0) {
                        NapTicketControl.showMessageBox('alert alert-error', ticketlisting.Result.ErrorMsg);
                    }else {
                        self.RenderMatchInfo(ticketlisting.MatchInfo);
                        self.RenderTickets(ticketlisting.Items);
                    }
                },
                error: function () {
                    console.log('has error :(');
                },
                complete: function () {
                    NapTicketControl.enableInput();
                }
            });
        }
        return false;
    });

    // handle request  to get ticket detail
    $('body').delegate('.lnk-totaltrans', 'click', function(){
        var $this = $(this);
        var bettypeId = $(this).attr('bettypeid'); 
        var liveIndicator = $(this).attr('liveindicator');
        var newAttributes = {};

        // Selected Product is 'Number Game'
        if(self.OldDataRequest.Type === 'NumberGame')
        {
            if(liveIndicator ===  'true') // is Live Ball
            {
                newAttributes.BetTypes = null;
                newAttributes.BetTypesLive = bettypeId;
            }else { // is Dead Ball
                newAttributes.BetTypes = bettypeId;
                newAttributes.BetTypesLive = null;
            }
        }else 
        {
            newAttributes.BetTypes = bettypeId;

            if(liveIndicator === 'true'){ // is Live Ball
                newAttributes.LiveType = 1;
            }else {  // is Dead Ball
                newAttributes.LiveType = 0;
            }
        }

        // get new Data Request Object to get ticket details
        var newDataReq = {};
        $.extend(true, newDataReq, self.OldDataRequest, newAttributes);
        console.log(newDataReq.toString());

        $.ajax({
                type: "POST",
                url: window.rootUrl + "Misc/TicketListing/GetTicketListingBetList",           
                contentType: "application/json; charset=utf-8", 
                dataType: "json",
                data: JSON.stringify(newDataReq),
                beforeSend: function () {
                    NapTicketControl.disableInput();
                },
                success: function (betlist) {
                    // check result to see any error from server
                    console.log('render bet list here.');
                    self.RenderTicketListingBetlist(betlist);
                },
                error: function () {
                    console.log('has error :(');
                },
                complete: function () {
                    NapTicketControl.enableInput();
                }
            });
    });

};

TicketListing.prototype.CollectSelectedProduct = function(){
    return $('input[name=Product]:checked').val();
}

TicketListing.prototype.CollectSelectedBettypes = function(){
    var bettypes = '';

    $('input[name=BetTypes]:checked').each(function(){
        bettypes += $(this).val() + ',';
    });
    console.log(bettypes);
    return bettypes.substring(0, bettypes.length - 1);
}

TicketListing.prototype.CollectSelectedLiveBettypes = function(){
    var livebettypes = '';
    
    $('input[name=LiveBetTypes]:checked').each(function(){
        livebettypes += $(this).val() + ',';
    })
    console.log(livebettypes);
    return livebettypes.substring(0, livebettypes.length - 1);
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

TicketListing.prototype.RenderMatchInfo = function (matchInfo) {
    var self = this;

    var gridOptions = {
        columns: [
            { HeaderText: "CODE", Name: "code", Width: 80, HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "MATCH EVENT", Name: "matchEvent", Width: 300, HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "EVENT DATE", Name: "eventDate", Width: 80, HeaderAlign: "center", CellAlign: "center" },
        ],
        bodyRows: self.GenerateMatchInfoData(matchInfo)
    };

    $('#match-info-container').fanexGrid(gridOptions);
};

TicketListing.prototype.GenerateMatchInfoData = function(matchInfo){
    var rows = [];
    rows.push({
        Columns: [{ Name: "code", Value: matchInfo.MatchCode },
            { Name: "matchEvent", Value: (matchInfo.HomeName + ' -vs- ' + matchInfo.AwayName)},
            { Name: "eventDate", Value:  matchInfo.EventDate}
    ]});

    return rows;
};

TicketListing.prototype.RenderTickets = function (tickets) {
    var self = this;

    var gridOptions = {
        columns: [
            { HeaderText: "BET TYPES", Name: "bettypes", Width: 150, HeaderAlign: "center", CellAlign: "left", ShowCollapseExpand: true },
            { HeaderText: "TOTAL BETS", Name: "totalBets", Width: 80, HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: "EXPORT", Name: "export", Width: 80, HeaderAlign: "center", CellAlign: "center" },
        ],
        bodyRows: self.GenerateTicketsData(tickets),
        showTree: true
    };

    $('#tickets-container').fanexGrid(gridOptions);
};

TicketListing.prototype.GenerateTicketsData = function(tickets){
    var self = this;
    var rows = [];

    $.each(tickets, function(idx, ticket) {
        var currentTicket = {
            Columns: [
                    { Name: "bettypes", Value: ticket.BetTypeName },
                    { Name: "totalBets", Value: { bettypeID: ticket.BetTypeID, liveIndicator: ticket.LiveIndicator, TotalTrans: ticket.TotalTrans} },
                    { Name: "export", Value: { bettypeID: ticket.BetTypeID, liveIndicator: ticket.LiveIndicator} }
                ]
        };

        currentTicket.Columns[1].CustomCellRender = function(cell){
            return TicketListing.prototype.RenderTotalTransCell(cell);
        };

        currentTicket.Columns[2].CustomCellRender = function(cell){
            return TicketListing.prototype.RenderExportCell(cell);
        }

        // add additional attributes
        currentTicket.Columns[1].CustomAttr = 'bettypeID="' + ticket.BetTypeID + '"';
        rows.push(currentTicket);
    });

    return rows;
};

TicketListing.prototype.RenderTotalTransCell = function(cell){
    return '<a href="javascript:void(0);" class="lnk-totaltrans" ' 
    + 'bettypeID="' + cell.Value.bettypeID 
    + '" liveIndicator="' + cell.Value.liveIndicator + '" >'
    + cell.Value.TotalTrans + '</a>';
}

TicketListing.prototype.RenderExportCell = function(cell){
    return '<a href="javascript:;" class="lnk-excel" '
    + 'bettypeID="' + cell.Value.bettypeID 
    + '" liveIndicator="' + cell.Value.liveIndicator + '" >'
    + '<i class="icon-excel"></i></a>';
}

// Detail Betlist for TicketListing

TicketListing.prototype.RenderTicketListingBetlist = function(betlist){
    var self = this;

    var gridOptions = {
        
        titleInfo: {
                text: "Tickets Listing for - Match ID: xxx - Bet type: yyy",
                showExcelButton: true,
                exportExcelFunction: function () {
                    alert("Export Excel Button Clicked");
                },
                showBackButton: true,
                backFunction: function () {
                    alert("Back Button Clicked");
                }
            },
        showTitle: true,
        columns: [
            { HeaderText: "BetID", Name: "betId", Width: 80, HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Trans.Date", Name: "transDate", Width: 80, HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Player", Name: "player", Width: 80, HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Choice", Name: "choice", Width: 80, HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: "Odds", Name: "odds", Width: 80, HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: "Stake", Name: "stake", Width: 80, HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: "Winloss", Name: "winloss", Width: 80, HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: "Currency", Name: "currency", Width: 80, HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Status", Name: "status", Width: 80, HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "S.A", Name: "sa", Width: 80, HeaderAlign: "center", CellAlign: "center" },
        ],
        bodyRows: self.GenerateBetListData(betlist.Items)
    };

    $('#betlist-container').fanexGrid(gridOptions);
}


TicketListing.prototype.GenerateBetListData = function(betlist){
    var self = this;
    var rows = [];

    $.each(betlist, function(idx, bet) {
        var currentBet = {
            Columns: [
                    { Name: "betId", Value: bet.Ticket.TransId },
                    { Name: "transDate", Value: bet.TransDate },
                    { Name: "player", Value: bet.Ticket.UserName },
                    { Name: "choice", Value: bet.Choice },
                    { Name: "odds", Value: bet.OddsHtml },
                    { Name: "stake", Value: bet.Ticket.Stake },
                    { Name: "winloss", Value: bet.Ticket.Winlost },
                    { Name: "currency", Value: bet.Currency },
                    { Name: "status", Value: bet.Ticket.Status },
                    { Name: "sa", Value: bet.SAFlag }
                ]
        };
        rows.push(currentBet);
    });

    return rows;
};