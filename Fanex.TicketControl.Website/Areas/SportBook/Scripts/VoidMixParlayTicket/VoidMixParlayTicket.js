/* File Created: February 5, 2013 */
var parlayDelete = {
    param:{
        MatchId: ''
    },

    search: function(){        
        $('#msg-box').hide();
        $('#msg').hide();        
                
        //Load Match Info
        if(validateData.checkParamBeforeGetData()){
            parlayDelete.getSingleTicket();
        }
        
    },

    getSingleTicket: function (){

         var param = {
            MatchId: $('#txtMatchId').val(),            
            Hdp: $('#hdp').val() == '' ? '9999' :  $('#hdp').val(),
            Type: $('#EventType').val(),
            LiveHomeScore: helper.GetLiveScore().LiveHomeScore,
            LiveAwayScore: helper.GetLiveScore().LiveAwayScore,
            FromTransdate: helper.GetLiveScore().FromTransdate,
            ToTransdate: helper.GetLiveScore().ToTransdate,
            Bettype: helper.GetParlayTypeChecked(),
            Bettypes: helper.GetBettypesChecked(),
            Betteam1378: $('#ddlBetTeam1378').val(),
            Betteam515: helper.GetBetteam515(),
            Betteam430: helper.GetBetteam430(),
            Betteam6: helper.GetBetteam6(),
            Betteam14: helper.GetBetteam14(),
            Betteam16: helper.GetBetteam16()
         };

         $.ajax({
                type: "POST",
                url: window.rootUrl + "SportBook/VoidMixParlayTicket/GetParlayTicket",
                contentType: "application/json; charset=utf-8", 
                dataType: "json",
                data: JSON.stringify(param),
                beforeSend: function () {
                    NapTicketControl.disableInput();
                },
                success: function (rs) {
                    //render match info
                    parlayDelete.loadMatchInfo(rs.MatchInfo) ;
                    parlayDelete.loadSingleTicket(rs.Groups);
                },
                error: function (e) {},
                complete: function (e) {
                    if(e.status == 403){
                        NapTicketControl.Helper.showMsgBox(NapTicketControl.Helper.authorizedMsg);
                    }
                    NapTicketControl.enableInput();
                }
            });
    },

    loadMatchInfo: function(matchInfo){

        if(matchInfo == null)
        {
            $('#matchInfo').hide();
            return;
        }
        NapTicketControl.Helper.displayContent(true);

        var rows = [];
        rows.push({
        Columns: [{ Name: "code", Value: matchInfo.MatchCode, CssClass: 'bold' },
            { Name: "matchEvent", Value: (matchInfo.HomeName + ' -vs- ' + matchInfo.AwayName), CssClass: 'bold'},
            { Name: "eventDate", Value:  matchInfo.EventDate, CssClass: 'bold'}
        ]});

        var gridOptions = {
            columns: [
                { HeaderText: "Code", Name: "code", Width: 100, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Match Event", Name: "matchEvent", Width: 300, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Event Date", Name: "eventDate", Width: 100, HeaderAlign: "center", CellAlign: "center" }
            ],
            hideYBackGroundScroller: true,
            bodyRows: rows
        };

        $('#matchInfo').fanexGrid(gridOptions);
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
                    { Name: "BetId", Value: currentRow.BetId },                    
                    { Name: "Player", Value: currentRow.Player },                    
                    { Name: "TransDate", Value:currentRow.TransDate  },
                    { Name: "BetTypeName", Value:currentRow.BetTypeName },
                    { Name: "Odds", Value:currentRow.Odds},                    
                    { Name: "Stake", Value: currentRow.Stake},
                    { Name: "CurrencyName", Value:currentRow.CurrencyName  },                    
                    { Name: "Status", Value: currentRow.Status },                    
                    { Name: "Choice", Value:currentRow.Choice + '<div style="display:none" id=divEvent_'+ currentRow.SubTransId +'>'},
                    { Name: "CustomAdd", Value: currentRow.SubTransId, TdClass:"Demo", CssClass: "demo2" }
                ],
                CssClass: "DemoCss"
            });
        };

        
        var gridOptions = {
            columns: [
                { HeaderText: "No", Name: "No", Width: 40, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "BetID", Name: "BetId", Width: 100, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Member", Name: "Player", Width: 150, HeaderAlign: "center", CellAlign: "center" },                
                { HeaderText: "Trans.Date", Name: "TransDate", Width: 100, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Bettype", Name: "BetTypeName", Width: 100, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Odds", Name: "Odds", Width: 60, HeaderAlign: "center", CellAlign: "center" },                
                { HeaderText: "Stake", Name: "Stake", Width: 60, HeaderAlign: "center", CellAlign: "center", FormatNumber: "0,0.00" },
                { HeaderText: "Currency", Name: "CurrencyName", Width: 60, HeaderAlign: "center", CellAlign: "center" },                
                { HeaderText: "Status", Name: "Status", Width: 60, HeaderAlign: "center", CellAlign: "center" },                
                { HeaderText: "Choice", Name: "Choice", Width: 250, HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: "<div><span>Option</span><span><input class='option' onclick='helper.setStausAllCheckTicket()' id='chk-ticket-all' type='checkbox' checked='checked'/></span></div>", Name: "CustomAdd", Width: 80, HeaderAlign: "center", CellAlign: "center", CustomCellRender: parlayDelete.optionCustomRender } 
            ],
            bodyRows: rows,
            showPager: true,
            isResetScroller: false,
            hideYBackGroundScroller: true,
            pagerOption: {
                itemsPerPage: 50,
                maxItemsPerPage: 100,
                currentPage: 0,
                totalItem: 100,
                startPerPage: 50,
                pageStep: 50,
                afterPagedFunction: function()
                {
                    $('#chk-ticket-all').attr('checked', 'checked');
                }
            }
        };

        $('#ticketInfo').fanexGrid(gridOptions);

        if(groups[0].Tickets.length == 0){
            Message.Helper.displayFormMsg(false);
        }
        else{
            Message.Helper.displayFormMsg(true);            
        }
        
    },

    optionCustomRender: function (cell)
    {
        return "<input onclick='helper.eventSubTicket()'  class='chk-ticket' value='"+ cell.Value +"' type='checkbox' checked='checked'/>";
    },

    voidSingleTicket: function (){
        
        if(!validateData.checkParamBeforVoidTicket()){
            return;
        }

        var param = {
            MatchId: $('#txtMatchId').val(),            
            Hdp: $('#hdp').val() == '' ? '9999' :  $('#hdp').val(),
            LiveIndicator: $('#EventType').val(),
            LiveHomeScore: $('#LiveHomeScore').val(),
            LiveAwayScore: $('#LiveAwayScore').val(),
            FromTransdate: $('#FromTransDate').val(),       
            ToTransdate: $('#ToTransDate').val(),     
            Bettypes: helper.GetBettypesChecked(),
            Betteam1378: $('#ddlBetTeam1378').val(),
            Betteam515: helper.GetBetteam515(),
            Betteam430: helper.GetBetteam430(),
            Betteam6: helper.GetBetteam6(),
            Betteam14: helper.GetBetteam14(),
            Betteam16: helper.GetBetteam16(),            
            TransIds: helper.getSubTicketsChecked(),
            UserPwd: $('#password').val(),
            UserName: $('#username').val(),
            Msg: {
                Content_en: $('#msg-en').val(),
                Content_ch: $('#msg-ch').val(),
                Content_cs: $('#msg-cs').val(),
                Content_th: $('#msg-th').val(),
                Content_it: $('#msg-it').val(),
                Content_jp: $('#msg-jp').val(),
                Content_es: $('#msg-es').val(),
                Content_ko: $('#msg-ko').val()
            }
         };

          $.ajax({
                type: "POST",
                url: window.rootUrl + "SportBook/VoidMixParlayTicket/VoidParlayTicket",
                contentType: "application/json; charset=utf-8", 
                dataType: "json",
                data: JSON.stringify(param),
                beforeSend: function () {
                    NapTicketControl.disableInput();
                },
                success: function (rs) {
                    //render match info
                    NapTicketControl.Helper.showMsgBox(rs);
                    if(rs != null && rs.length>0 && rs[0].ErrorCode < 0){
                        NapTicketControl.Helper.displayContent(false);
                        helper.resetInput();
                    }
                },
                error: function (e) {},
                complete: function (e) {
                    if(e.status == 403){
                        NapTicketControl.Helper.showMsgBox(NapTicketControl.Helper.authorizedMsg);
                    }
                    NapTicketControl.enableInput();
                }
            });
    }

};

function VoidMixParlayTicket() {
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

VoidMixParlayTicket.prototype.Mode = {
    Sportsbook: 0
};

VoidMixParlayTicket.prototype.CacheDOMElement = function () {
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

VoidMixParlayTicket.prototype.Init = function () {
    
    jQuery("#VoidMixParlayTicketForm").validationEngine();

    var self = this;

    self.CacheDOMElement();    
    NapTicketControl.Helper.GetBettypes('Sportsbook');
    self.SetupCalendar('', '.txtCalendar');
    $('.txtCalendar').attr('disabled', 'disabled');

    helper.setStatusAllBettype(false);

    // handle events from DOM element
    // change live type
    self.DOMElement.$ddlEventType.change(function () {
        var eventType = $(this).val(); // live type drop down list
        helper.checkConstraintBettyeByEventTypeForAdvanceMode();            
        if (eventType === '1') {
            self.DOMElement.$optFilters.show();               
        } else {
            self.DOMElement.$optFilters.hide();                 
        }     

       // NapTicketControl.Helper.GetBettypes('Sportsbook', eventType);
        //helper.setStatusAllBettype(false);
    });

    // enable/disable Trans Date
    self.DOMElement.$chkTransDateRange.click(function () {
        if ($(this).prop('checked')) {
            // fill in current date
            var today = new Date();
            $('.txtCalendar').removeAttr('disabled');
            $('.txtCalendar').css('cursor', 'default');
            self.SetupCalendar(today.format('Y/m/d H:i:s'), '.txtCalendar');

        } else {
            // clear text box
            $('.txtCalendar').val('').attr('disabled', 'disabled');
            $('.txtCalendar').css('cursor', 'not-allowed');
        }
    });

    $('.icon-from-transdate-calendar').click(function () {
        $('#FromTransDate').datetimepicker("show");
    });

    $('.icon-to-transdate-calendar').click(function () {
        $('#ToTransDate').datetimepicker("show");
    });

    $('.fieldset-advance-mode legend').click(function(){
        helper.toggleAdvancedMode(this);
    });

    $('.toggle-filter-bar').click(function(){
        NapTicketControl.Helper.ToggleFilterBar(this);
    });

    // check all bettypes
    $('body').delegate('#chkallbettype', 'click', function (e) {
        e.stopPropagation();

        if ($('#chkallbettype').prop('checked')) {
            $('.chkbettype').attr('checked', 'checked')
            helper.checkConstraintBettyeByEventTypeForAdvanceMode();            
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
        
        helper.checkConstraintBettyeByEventTypeForAdvanceMode();
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
        helper.eventSubAdvanceFGLG();
    });
    
    $('#btnSearch').click(function(e){
        e.stopPropagation();
        parlayDelete.search();
        return false;
    });

    $('#btnVoidBet').click(function(e){
        e.stopPropagation();
        parlayDelete.voidSingleTicket();
        return false;
    });

    $("#txtMatchId").keyup(function () {}).ForceIntegerOnly();    
    $("#hdp").keyup(function () {}).ForceDecimalOnly();
    $("#LiveHomeScore").keyup(function () {}).ForceIntegerOnly();
    $("#LiveAwayScore").keyup(function () {}).ForceIntegerOnly();
};

VoidMixParlayTicket.prototype.SetupCalendar = function (strdatetime, txtdate) {
    $(txtdate).datetimepicker({
        showSecond: true,
        dateFormat: 'yy/m/d',
        timeFormat: 'HH:mm:ss',
        hourGrid: 3,
        minuteGrid: 5,
        secondGrid: 5,
        separator: ' ',
        showButtonPanel: true
    });

    if (strdatetime != null && strdatetime != "") {
        $(txtdate).val(strdatetime).datetimepicker('setDate', (new Date(strdatetime)));
    }
}


