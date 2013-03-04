/* File Created: February 5, 2013 */


var helper = {

    setStatusAllBettype: function (isChecked){
        if (isChecked) {
            $('.chkbettype').attr('checked', 'checked')
            $('#chkallbettype').attr('checked', 'checked')
            helper.setStatusAllAdvanceCS(true, true);        
            helper.setStatusAllAdvanceTG(true, true);
            helper.setStatusAllAdvanceHDP(true);
            helper.setStatusAllAdvance1X2(true);
            helper.setStatusAllAdvanceHTFT(true, true);
            helper.setStatusAllAdvanceFGLG(true, true);
        } else {
            $('.chkbettype').removeAttr('checked');
            $('#chkallbettype').removeAttr('checked');
            helper.setStatusAllAdvanceCS(false, false);        
            helper.setStatusAllAdvanceTG(false, false);
            helper.setStatusAllAdvanceHDP(false);
            helper.setStatusAllAdvance1X2(false);
            helper.setStatusAllAdvanceHTFT(false, false);
            helper.setStatusAllAdvanceFGLG(false, false);
        }
    },

    setStatusAllAdvanceCS: function (isChecked, isEnable){
        if (isChecked) {
            $('.chk-score').attr('checked', 'checked');               
        } else {
            $('.chk-score').removeAttr('checked');                        
        }

        $('.chk-score').attr('disabled', !isEnable);            
    },

    eventSubAdvanceCS: function (){
        var selectedItems = $('.all-chk-score tbody input:checked').length;
        var totalItems = $('.all-chk-score tbody input:checkbox').length;

        if (selectedItems === totalItems) {
            $('#chk-score-all').attr('checked', 'checked');
        } else {
            $('#chk-score-all').removeAttr('checked');
        }
    },

    setStatusAllAdvanceTG: function (isChecked, isEnable){
        if (isChecked) {
            $('.chk-tg').attr('checked', 'checked');
        } else {
            $('.chk-tg').removeAttr('checked');            
        }

        $('.chk-tg').attr('disabled', !isEnable);            
    },

    eventSubAdvanceTG: function (){
        var selectedItems = $('.advance-item-tg .chk-tg:input:checked').length;
        var totalItems =    $('.advance-item-tg .chk-tg:input:checkbox').length;

        if (selectedItems === totalItems) {
            $('#chk-tg-all').attr('checked', 'checked');
        } else {
            $('#chk-tg-all').removeAttr('checked');
        }
    },

    setStatusAllAdvanceHDP: function (isEnable){
        $('#ddlBetTeam1378').attr('disabled', !isEnable);
        $('#hdp').attr('disabled', !isEnable);
    },

    setStatusAllAdvance1X2: function (isEnable){
        $('#ddlBetTeam515').attr('disabled', !isEnable);        
    },

    setStatusAllAdvanceHTFT: function (isChecked, isEnable){
        if (isChecked) {
            $('.chk-htft').attr('checked', 'checked');
        } else {
            $('.chk-htft').removeAttr('checked');            
        }

        $('.chk-htft').attr('disabled', !isEnable);            
    },

    eventSubAdvanceHTFT: function (){
        var selectedItems = $('.advance-item-htft .chk-htft:input:checked').not('#chk-htft-all').length;
        var totalItems =    $('.advance-item-htft .chk-htft:input:checkbox').not('#chk-htft-all').length;

        if (selectedItems === totalItems) {
            $('#chk-htft-all').attr('checked', 'checked');
        } else {
            $('#chk-htft-all').removeAttr('checked');
        }
    },

    setStatusAllAdvanceFGLG: function (isChecked, isEnable){
        if (isChecked) {
            $('.chk-flg').attr('checked', 'checked');
        } else {
            $('.chk-flg').removeAttr('checked');            
        }

        $('.chk-flg').attr('disabled', !isEnable);            
    },

     eventSubAdvanceFGLG: function (){
        var selectedItems = $('.advance-item-flg .chk-flg:input:checked').not('#chk-flg-all').length;
        var totalItems =    $('.advance-item-flg .chk-flg:input:checkbox').not('#chk-flg-all').length;

        if (selectedItems === totalItems) {
            $('#chk-flg-all').attr('checked', 'checked');
        } else {
            $('#chk-flg-all').removeAttr('checked');
        }
    },

    IsSelectedBettype: function (bettypeId){
        var checkedVals = $('.chkbettype:checkbox:checked').map(function() {
            return this.value;
        }).get();

        var isSelected = false;

        for(var i = 0; i<checkedVals.length;i++){
            if(checkedVals[i] == bettypeId){
                isSelected = true;
                break;
            }
        }

        return isSelected;
    },

    GetBettypesChecked: function (){
        var checkedVals = $('.chkbettype:checkbox:checked').map(function() {
            return this.value;
        }).get().join('^');

        return checkedVals;
    },

    GetBetteam430: function(){
        var checkedVals = '';
        checkedVals = $('.all-chk-score .chk-score:checkbox:checked').map(function() {
                return this.value;
            }).get().join('^'); 

        return checkedVals;
    },

    GetBetteam6: function(){
        var checkedVals = '';
        checkedVals = $('.chk-tg:checkbox:checked').not('#chk-tg-all').map(function() {
                return this.value;
            }).get().join('^'); 

        return checkedVals;
    },

    GetBetteam14: function (){
        var checkedVals = '';
        checkedVals = $('.chk-flg:checkbox:checked').not('#chk-flg-all').map(function() {
                return this.value;
            }).get().join('^'); 

        return checkedVals;
    },

    GetBetteam16: function (){
        var checkedVals = '';
        checkedVals = $('.chk-htft:checkbox:checked').not('#chk-htft-all').map(function() {
                return this.value;
            }).get().join('^'); 

        return checkedVals;
    },

    setStausAllCheckTicket: function ()
    {
        var isChecked = $('#chk-ticket-all').prop('checked');
         if (isChecked) {
            $('.chk-ticket').attr('checked', 'checked');
        } else {
            $('.chk-ticket').removeAttr('checked');            
        }
    },

    eventSubTicket: function (){
        var selectedItems = $('.chk-ticket:input:checked').length;
        var totalItems =    $('.chk-ticket:input:checkbox').length;

        if (selectedItems === totalItems) {
            $('#chk-ticket-all').attr('checked', 'checked');
        } else {
            $('#chk-ticket-all').removeAttr('checked');
        }
    },

    displayMoreMsg: function (){
        var symbol = $('#spMoreMsg').html();

        if(symbol == '[+]'){
            $('.other-languages').show();
        }
        else{
            $('.other-languages').hide();
        }

        symbol = symbol == '[-]' ? '[+]' : '[-]';
        $('#spMoreMsg').html(symbol);
    },

    displayFormMsg: function(){
        $('.form').show();
    }
   

    
};



var singleDetete = {
    search: function(){
        jQuery("#DeleteSingleTicketForm").validationEngine('validate');

        $('#msg').html(tooltip.show('hdp', 'what are you doing here!!!'));
                
        //Load Match Info
        singleDetete.getSingleTicket();
    },

    getSingleTicket: function (){

         var param = {
            MatchId: $('#txtMatchId').val(),
            FromRefNo: $('#fromRefno').val(),
            ToRefNo: $('#toRefno').val(),
            Hdp: $('#hdp').val() == '' ? '999' :  $('#hdp').val(),
            LiveIndicator: $('#EventType').val(),
            LiveHomeScore: $('#LiveHomeScore').val(),
            LiveAwayScore: $('#LiveAwayScore').val(),
            FromTransdate: $('#FromTransDate').val(),
            ToTransdate: $('#ToTransdate').val(),
            Bettypes: helper.GetBettypesChecked(),
            Betteam1378: $('#ddlBetTeam1378').val(),
            Betteam515: $('#ddlBetTeam515').val(),
            Betteam430: helper.GetBetteam430(),
            Betteam6: helper.GetBetteam6(),
            Betteam14: helper.GetBetteam14(),
            Betteam16: helper.GetBetteam16(),
            SAFilter: $('#TicketType').val()
         };

         $.ajax({
                type: "POST",
                url: window.rootUrl + "SportBook/DeleteSingleTicket/GetSingleTicket",
                contentType: "application/json; charset=utf-8", 
                dataType: "json",
                data: JSON.stringify(param),
                beforeSend: function () {
                    NapTicketControl.disableInput();
                },
                success: function (rs) {
                    //render match info
                    singleDetete.loadMatchInfo(rs.MatchInfo) ;
                    singleDetete.loadSingleTicket(rs.Groups);
                },
                error: function () {
                    console.log('has error :(');
                },
                complete: function () {
                    NapTicketControl.enableInput();
                }
            });
    },

    loadMatchInfo: function(matchInfo){
        var rows = [];
        rows.push({
        Columns: [{ Name: "code", Value: matchInfo.MatchCode },
            { Name: "matchEvent", Value: (matchInfo.HomeName + ' -vs- ' + matchInfo.AwayName)},
            { Name: "eventDate", Value:  matchInfo.EventDate}
        ]});

        var id = 'matchInfo';
        var gridOptions = {
            columns: [
                { HeaderText: "Code", Name: "code", Width: 60, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Match Event", Name: "matchEvent", Width: 300, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Event Date", Name: "eventDate", Width: 80, HeaderAlign: "center", CellAlign: "center" }
            ],
            bodyRows: rows
        };

        $('#' + id).fanexGrid(gridOptions);
    },

    loadSingleTicket : function(groups)
    {
        var rows = [];
        var length = groups[0].Tickets.length;
        for (var index = 0; index < length; index++) {
            var currentRow = groups[0].Tickets[index];
            rows.push({
                Columns:[
                    { Name: "No", Value: currentRow.No  },
                    { Name: "Player", Value: currentRow.Player },
                    { Name: "BetId", Value: currentRow.BetId },                    
                    { Name: "TransDate", Value:currentRow.TransDate  },
                    { Name: "BetTypeName", Value:currentRow.BetTypeName },
                    { Name: "Odds", Value:currentRow.Odds},
                    { Name: "Choice", Value:currentRow.Choice},
                    { Name: "Stake", Value: currentRow.Stake},
                    { Name: "CurrencyName", Value:currentRow.CurrencyName  },
                    { Name: "Score", Value: currentRow.Score },
                    { Name: "Status", Value: currentRow.Status },
                    { Name: "SA", Value: currentRow.SA },
                    { Name: "CustomAdd", Value: currentRow.BetId, TdClass:"Demo", CssClass: "demo2" }
                ],
                CssClass: "DemoCss"
            });
        };

        
        var gridOptions = {
            columns: [
                { HeaderText: "No", Name: "No", Width: 20, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Player", Name: "Player", Width: 100, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "BetID", Name: "BetId", Width: 100, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Trans.Date", Name: "TransDate", Width: 80, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Bettype", Name: "BetTypeName", Width: 100, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Odds", Name: "Odds", Width: 60, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Choice", Name: "Choice", Width: 200, HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: "Stake", Name: "Stake", Width: 60, HeaderAlign: "center", CellAlign: "center", FormatNumber: "0,0.00" },
                { HeaderText: "Currency", Name: "CurrencyName", Width: 60, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Score", Name: "Score", Width: 60, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Status", Name: "Status", Width: 60, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "S.A", Name: "SA", Width: 40, HeaderAlign: "center", CellAlign: "center" } ,
                { HeaderText: "<div><span>Option</span><span><input onclick='helper.setStausAllCheckTicket()' id='chk-ticket-all' type='checkbox' checked='checked'/></span></div>", Name: "CustomAdd", Width: 80, HeaderAlign: "center", CellAlign: "center", CustomCellRender: singleDetete.optionCustomRender } 
            ],
            bodyRows: rows
        };

        $('#ticketInfo').fanexGrid(gridOptions);
        helper.displayFormMsg();
    },

    optionCustomRender: function (cell)
    {
        return "<input onclick='helper.eventSubTicket()'  class='chk-ticket' value='"+ cell.Value +"' type='checkbox' checked='checked'/>";
    }

    
};

 

function DelSingleTicket() {
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

DelSingleTicket.prototype.Mode = {
    Sportsbook: 0
};

DelSingleTicket.prototype.CacheDOMElement = function () {
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

DelSingleTicket.prototype.Init = function () {
    
    jQuery("#DeleteSingleTicketForm").validationEngine();

    var self = this;

    self.CacheDOMElement();    
    Helper.GetBettypes('Sportsbook');
    self.SetupCalendar('', '.txtCalendar');
    $('.txtCalendar').attr('disabled', 'disabled');

    helper.setStatusAllBettype(false);

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
        helper.setStatusAllBettype(false);
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

    // check all bettypes
    $('body').delegate('#chkallbettype', 'click', function () {
        if ($('#chkallbettype').prop('checked')) {
            $('.chkbettype').attr('checked', 'checked')
            helper.setStatusAllAdvanceCS(true, true);    
            helper.setStatusAllAdvanceTG(true, true);
            helper.setStatusAllAdvanceHDP(true);
            helper.setStatusAllAdvance1X2(true);
            helper.setStatusAllAdvanceHTFT(true, true);
            helper.setStatusAllAdvanceFGLG(true, true);
        } else {
            $('.chkbettype').removeAttr('checked');
            helper.setStatusAllAdvanceCS(false, false);        
            helper.setStatusAllAdvanceTG(false, false);
            helper.setStatusAllAdvanceHDP(false);
            helper.setStatusAllAdvance1X2(false);
            helper.setStatusAllAdvanceHTFT(false, false);
            helper.setStatusAllAdvanceFGLG(false, false);
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
        
        var IsSelectedCS = helper.IsSelectedBettype(4) || helper.IsSelectedBettype(30);
        helper.setStatusAllAdvanceCS(IsSelectedCS, IsSelectedCS); 
        
        var IsSelectedTG = helper.IsSelectedBettype(6);
        helper.setStatusAllAdvanceTG(IsSelectedTG, IsSelectedTG);

        var IsSelectedHDP = helper.IsSelectedBettype(1) || helper.IsSelectedBettype(7) 
                            || helper.IsSelectedBettype(3) || helper.IsSelectedBettype(8)
                            || helper.IsSelectedBettype(2) || helper.IsSelectedBettype(12);
        helper.setStatusAllAdvanceHDP(IsSelectedHDP);

        var IsSelected1X2 =  helper.IsSelectedBettype(5) || helper.IsSelectedBettype(15);
        helper.setStatusAllAdvance1X2(IsSelected1X2);

        var IsSelected1X2 =  helper.IsSelectedBettype(16);
        helper.setStatusAllAdvanceHTFT(IsSelected1X2, IsSelected1X2);

        var IsSelectedGFLG =  helper.IsSelectedBettype(14);
        helper.setStatusAllAdvanceFGLG(IsSelectedGFLG, IsSelectedGFLG);
    });

    //Correct Scores check all
    $('body').delegate('#chk-score-all', 'click', function () {
        helper.setStatusAllAdvanceCS(this.checked, true);
    });

    //Correct Scores uncheck any item
    $('body').delegate('.chk-score', 'click', function () {
        helper.eventSubAdvanceCS();
    });

     //TG check all
    $('body').delegate('#chk-tg-all', 'click', function () {
        helper.setStatusAllAdvanceTG(this.checked, true);
    });

     //TG uncheck any item
    $('body').delegate('.chk-tg', 'click', function () {
        helper.eventSubAdvanceTG();
    });

     //HTFT check all 
    $('body').delegate('#chk-htft-all', 'click', function () {        
        helper.setStatusAllAdvanceHTFT(this.checked, true);
    });
  
    //HTFT uncheck any item
    $('body').delegate('.chk-htft', 'click', function () {
        helper.eventSubAdvanceHTFT();
    });

      //FGLG check all 
    $('body').delegate('#chk-flg-all', 'click', function () {        
        helper.setStatusAllAdvanceFGLG(this.checked, true);
    });
  
    //FGLG uncheck any item
    $('body').delegate('.chk-flg', 'click', function () {
        helper.eventSubAdvanceHTFT();
    });
    
    $('body').delegate('#btnSearch', 'click', function () {
        singleDetete.search();
    });
};


DelSingleTicket.prototype.SetupCalendar = function (strdatetime, txtdate) {
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


