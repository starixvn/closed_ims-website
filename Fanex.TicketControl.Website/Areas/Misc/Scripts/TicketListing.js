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

    self.DOMElement.$ddlLiccensee = $('.ddlLicensee');
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

//    // toggle each licensee group and toggle all 
//    $('body').delegate('.lnk-toggle', 'click', function () {
//        var $this = $(this),
//            icon = $this.find('i');

//        $this.parents('table').find('tr[pid=' + $this.parents('tr').attr('id') + ']').fadeToggle('fast');

//        if (icon.hasClass('icon-plus-sign')) {
//            icon.addClass('icon-minus-sign').removeClass('icon-plus-sign');
//        } else {
//            icon.addClass('icon-plus-sign').removeClass('icon-minus-sign');
//        }
//    });

//    $('body').delegate('.lnk-toggle-all', 'click', function () {
//        var $this = $(this),
//            icon = $this.find('i'),
//            tBody = $this.parents('table').find('tbody');

//        if (icon.hasClass('icon-plus-sign')) {
//            tBody.find('tr:not(.lic)').fadeIn('fast');
//            icon.addClass('icon-minus-sign').removeClass('icon-plus-sign');
//            tBody.find('.lnk-toggle i').addClass('icon-minus-sign').removeClass('icon-plus-sign');
//        } else {
//            tBody.find('tr:not(.lic)').fadeOut('fast');
//            icon.addClass('icon-plus-sign').removeClass('icon-minus-sign');
//            tBody.find('.lnk-toggle i').addClass('icon-plus-sign').removeClass('icon-minus-sign');
//        }
//    });

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

    var id = 'match-info-container';
    if($('#' + id).length > 0){
        $('#' + id).remove(); // remove it
    }

    var matchContainer = '<div class="freegrid  content-grid" id="' + id + '"></div>';
    var gridOptions = {
        columns: [
            { HeaderText: "Code", Name: "code", Width: 60, HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Match Event", Name: "matchEvent", Width: 300, HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Event Date", Name: "eventDate", Width: 80, HeaderAlign: "center", CellAlign: "center" },
        ],
        bodyRows: self.GenerateMatchInfoData(matchInfo)
    };

    self.DOMElement.$body.append(matchContainer);

    $('#' + id).fanexGrid(gridOptions);
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

    var id = 'tickets-container';
    if($('#' + id).length > 0){
        $('#' + id).remove(); // remove it
    }

    var matchContainer = '<div class="freegrid  content-grid" id="' + id + '"></div>';
    var gridOptions = {
        columns: [
            { HeaderText: "Licensee", Name: "licensee", Width: 150, HeaderAlign: "center", CellAlign: "left", ShowCollapseExpand: true },
            { HeaderText: "Total Trans", Name: "totalTrans", Width: 80, HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: "Export Excel", Name: "export", Width: 80, HeaderAlign: "center", CellAlign: "center" },
        ],
        bodyRows: self.GenerateTicketsData(tickets),
        showTree: true
    };

    self.DOMElement.$body.append(matchContainer);

    $('#' + id).fanexGrid(gridOptions);
};

TicketListing.prototype.GenerateTicketsData = function(tickets){
    var self = this;
    var rows = [];

    $.each(tickets, function(idx, group) {
        var currentGroup = {
            Columns: [
                    { Name: "licensee", Value: group.Name },
                    { Name: "totalTrans", Value: { groupId: group.Id, childId: '', liveIndicator: group.LiveIndicator, TotalTrans: group.TotalTrans} },
                    { Name: "export", Value: { groupId: group.Id, childId: '', liveIndicator: group.LiveIndicator} }
                ],
            SubRows: self.GenerateTicketDetailsData(group.Id, group.Items)
        };

        currentGroup.Columns[1].CustomCellRender = function(cell){
            return TicketListing.prototype.RenderTotalTransCell(cell);
        };
        currentGroup.Columns[2].CustomCellRender = function(cell){
            return TicketListing.prototype.RenderExportCell(cell);
        }

        // add additional attributes
        currentGroup.Columns[1].CustomAttr = 'groupId="' + group.Name + '"';
        rows.push(currentGroup);
    });

    return rows;
};

TicketListing.prototype.GenerateTicketDetailsData = function(parent, items){
    var self = this;
    var rows = [];

     $.each(items, function(idx, item){
        var currentRow = {
            Columns: [
                    { Name: "licensee", Value: item.Name },
                    { Name: "totalTrans", Value:  { groupId: parent, childId: item.Name, liveIndicator: item.LiveIndicator, TotalTrans: item.TotalTrans}},
                    { Name: "export", Value: { groupId: parent, childId: item.Name, liveIndicator: item.LiveIndicator } }
                ]
        }

        currentRow.Columns[1].CustomCellRender = function(cell){
            return TicketListing.prototype.RenderTotalTransCell(cell);
        };

        currentRow.Columns[2].CustomCellRender = function(cell){
            return TicketListing.prototype.RenderExportCell(cell);
        }

        rows.push(currentRow);
     });

    return rows;
}

TicketListing.prototype.RenderTotalTransCell = function(cell){
    return '<a href="javascript:void(0);" class="lnk-totaltrans" ' 
    + 'groupId="' + cell.Value.groupId + '" childId="' + cell.Value.childId 
    + '" liveIndicator="' + cell.Value.liveIndicator + '" >'
    + cell.Value.TotalTrans + '</a>';
}

TicketListing.prototype.RenderExportCell = function(cell){
    return '<a href="javascript:;" class="lnk-excel" '
    + 'groupId="' + cell.Value.groupId + '" childId="' + cell.Value.childId 
    + '" liveIndicator="' + cell.Value.liveIndicator + '" >'
    + '<i class="icon-excel"></i></a>';
}