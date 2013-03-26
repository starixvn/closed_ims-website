/* File Created: February 5, 2013 */

var singleDetete = {
    param:{
        MatchId: '',
        FromRefno: '',
        ToRefno: ''
    },

    search: function(){
        //jQuery("#DeleteSingleTicketForm").validationEngine('validate', {promptPosition : "centerRight"});
        $('#msg-box').hide();
        $('#msg').hide();        
                
        //Load Match Info
        if(validateData.checkParamBeforeGetData()){
            singleDetete.checkMatchIdWithRefno();
        }
    },

    getSingleTicket: function (){

         var param = {
            MatchId: singleDetete.param.MatchId,
            FromRefNo: singleDetete.param.FromRefno,
            ToRefNo: singleDetete.param.ToRefno,
            Hdp: $('#hdp').val() == '' ? '9999' :  $('#hdp').val(),
            LiveIndicator: $('#EventType').val(),
            LiveHomeScore: helper.GetLiveScore().LiveHomeScore,
            LiveAwayScore: helper.GetLiveScore().LiveAwayScore,
            FromTransdate: helper.GetLiveScore().FromTransdate,
            ToTransdate: helper.GetLiveScore().ToTransdate,
            Bettypes: helper.GetBettypesChecked(),
            Betteam1378: $('#ddlBetTeam1378').val(),
            Betteam515: helper.GetBetteam515(),
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
                   // console.log('has error :(');
                },
                complete: function () {
                    NapTicketControl.enableInput();
                }
            });
    },

    loadMatchInfo: function(matchInfo){

        if(matchInfo != null){
            helper.displayContent(true);
        }

        var rows = [];
        rows.push({
        Columns: [{ Name: "code", Value: matchInfo.MatchCode, CssClass: 'bold' },
            { Name: "matchEvent", Value: (matchInfo.HomeName + ' -vs- ' + matchInfo.AwayName), CssClass: 'bold'},
            { Name: "eventDate", Value:  matchInfo.EventDate, CssClass: 'bold'}
        ]});

        
        var gridOptions = {
            columns: [
                { HeaderText: "Code", Name: "code", Width: 60, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Match Event", Name: "matchEvent", Width: 300, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Event Date", Name: "eventDate", Width: 80, HeaderAlign: "center", CellAlign: "center" }
            ],
            bodyRows: rows,
            hideYBackGroundScroller: true
        };

        $('#matchInfo').fanexGrid(gridOptions);
        
    },

    loadSingleTicket : function(groups)
    {
        var rows = [];
        var length = groups[0].Tickets.length;

        if(length>0){
            helper.displayContent(true);
        }

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
                    { Name: "SA", Value: currentRow.SA },
                    { Name: "Choice", Value:currentRow.Choice},
                    { Name: "CustomAdd", Value: currentRow.BetId, TdClass:"Demo", CssClass: "demo2" }
                ],
                CssClass: "DemoCss"
            });
        };

        
        var gridOptions = {
            columns: [
                { HeaderText: "No", Name: "No", Width: 40, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "BetID", Name: "BetId", Width: 100, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Member", Name: "Player", Width: 150, HeaderAlign: "center", CellAlign: "left" },                
                { HeaderText: "Trans.Date", Name: "TransDate", Width: 100, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Bettype", Name: "BetTypeName", Width: 100, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Odds", Name: "Odds", Width: 60, HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: "Stake", Name: "Stake", Width: 60, HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                { HeaderText: "Currency", Name: "CurrencyName", Width: 60, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Status", Name: "Status", Width: 80, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "S.A", Name: "SA", Width: 40, HeaderAlign: "center", CellAlign: "center" } ,
                { HeaderText: "Choice", Name: "Choice", Width: 250, HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: "<div><span>Option</span><span><input class='option' onclick='helper.setStausAllCheckTicket()' id='chk-ticket-all' type='checkbox' checked='checked'/></span></div>", Name: "CustomAdd", Width: 80, HeaderAlign: "center", CellAlign: "center", CustomCellRender: singleDetete.optionCustomRender } 
            ],
            bodyRows: rows,
            showPager: true,
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
        
        if(!validateData.checkParamBeforeVoidTicket()){
            return;
        }

        var param = {
            MatchId: $('#txtMatchId').val(),
            f_transid: $('#fromRefno').val(),
            t_transid: $('#toRefno').val(),
            Hdp: $('#hdp').val() == '' ? '9999' :  $('#hdp').val(),
            LiveIndicator: $('#EventType').val(),
            LiveHomeScore: $('#LiveHomeScore').val(),
            LiveAwayScore: $('#LiveAwayScore').val(),
            FromTransdate: $('#FromTransDate').val(),
            ToTransdate: $('#ToTransdate').val(),
            Bettypes: helper.GetBettypesChecked(),
            Betteam1378: $('#ddlBetTeam1378').val(),
            Betteam515: helper.GetBetteam515(),
            Betteam430: helper.GetBetteam430(),
            Betteam6: helper.GetBetteam6(),
            Betteam14: helper.GetBetteam14(),
            Betteam16: helper.GetBetteam16(),
            SAFilter: $('#TicketType').val(),
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
                url: window.rootUrl + "SportBook/DeleteSingleTicket/VoidSingleTicket",
                contentType: "application/json; charset=utf-8", 
                dataType: "json",
                data: JSON.stringify(param),
                beforeSend: function () {
                    NapTicketControl.disableInput();
                },
                statusCode: {
                    403: function (rs){
                        alert(rs);
                    }
                },
                success: function (rs) {
                    //render match info
                    NapTicketControl.Helper.showMsgBox(rs);
                    if(rs != null && rs.length>0 && rs[0].ErrorCode < 0){
                        helper.displayContent(false);
                        helper.resetContent();
                    }
                },
                error: function (e) {                    
                    NapTicketControl.Helper.showMsgBox(e.responseText);                  
                },
                complete: function () {
                    NapTicketControl.enableInput();
                }
            });
    },

    checkMatchIdWithRefno: function (){
        var param = {
            MatchId:  $('#txtMatchId').val(),
            FromRefno: $('#fromRefno').val(),
            ToRefno: $('#toRefno').val()
        };

        $.ajax({
                type: "POST",
                url: window.rootUrl + "SportBook/DeleteSingleTicket/CheckMatchIdWithRefno",
                contentType: "application/json; charset=utf-8", 
                dataType: "json",
                async: false,
                data: JSON.stringify(param),
                beforeSend: function () {
                  //  NapTicketControl.disableInput();
                },
                success: function (rs) {
                    if(rs.ErrorCode == 0){
                        singleDetete.param = param;       
                        singleDetete.getSingleTicket();      
                        helper.displayContent(true);                                   
                    }
                    else{                      
                        switch(rs.ErrorCode){
                            case 1:
                            case 2:
                                $('#msg').html(tooltip.show('txtMatchId', rs.ErrorMsg)).show();
                                break;
                            case 3:
                                $('#msg').html(tooltip.show('fromRefno', rs.ErrorMsg)).show();
                                break;
                            case 4:
                                $('#msg').html(tooltip.show('toRefno', rs.ErrorMsg)).show();
                                break;
                        }

                       helper.displayContent(false);             
                    }
                    
                },
                error: function () {
                    console.log('has error :(');
                },
                complete: function () {
                   // NapTicketControl.enableInput();
                }
            });
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
    NapTicketControl.Helper.GetBettypes('Sportsbook');
    self.SetupCalendar('', '.txtCalendar');
    $('.txtCalendar').attr('disabled', 'disabled');

    helper.setStatusAllBettype(false);

    // handle events from DOM element
    // change live type
    self.DOMElement.$ddlEventType.change(function () {
        var eventType = $(this).val(); // live type drop down list

        if (eventType === '1') {
            self.DOMElement.$optFilters.show();   

        } else {
            self.DOMElement.$optFilters.hide();        
        }
    });

    // enable/disable Trans Date
    self.DOMElement.$chkTransDateRange.click(function () {
        if ($(this).prop('checked')) {
            // fill in current date
            var today = new Date();
            $('.txtCalendar').removeAttr('disabled');
            self.SetupCalendar(today.format('Y/m/d H:i:s'), '.txtCalendar');

        } else {
            // clear text box
            $('.txtCalendar').val('').attr('disabled', 'disabled');
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
            helper.setStatusAllAdvanceCS(true, true);    
            helper.setStatusAllAdvanceTG(true, true);
            helper.setStatusAllAdvanceHDP(true);
            helper.setStatusAllAdvance1X2(true);
            helper.setStatusAllAdvanceHTFT(true, true);
            helper.setStatusAllAdvanceFGLG(true, true);
            helper.expandAdvancedMode();    
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
        
        var eventType = $('#EventType').val();

        var IsSelectedCS = (helper.IsSelectedBettype(4) || helper.IsSelectedBettype(30)) && eventType != '1';
        if(!(IsSelectedCS && helper.GetBetteam430() != '')){
            helper.setStatusAllAdvanceCS(IsSelectedCS, IsSelectedCS); 
        }
        
        var IsSelectedTG = helper.IsSelectedBettype(6) && eventType != '1';
        if(!(IsSelectedTG && helper.GetBetteam6() != '')){
            helper.setStatusAllAdvanceTG(IsSelectedTG, IsSelectedTG);
        }

        var IsSelectedHDP = helper.IsSelectedBettype(1) || helper.IsSelectedBettype(7) 
                            || helper.IsSelectedBettype(3) || helper.IsSelectedBettype(8)
                            || helper.IsSelectedBettype(2) || helper.IsSelectedBettype(12);
        helper.setStatusAllAdvanceHDP(IsSelectedHDP);

        var IsSelected1X2 =  helper.IsSelectedBettype(5) || helper.IsSelectedBettype(15);
        helper.setStatusAllAdvance1X2(IsSelected1X2);

        var IsSelectedHTFT =  helper.IsSelectedBettype(16) && eventType != '1';
        if(!(IsSelectedHTFT && helper.GetBetteam16() != '')){
            helper.setStatusAllAdvanceHTFT(IsSelectedHTFT, IsSelectedHTFT);
        }

        var IsSelectedGFLG =  helper.IsSelectedBettype(14)  && eventType != '1';
        if(!(IsSelectedGFLG && helper.GetBetteam14() != '')){
            helper.setStatusAllAdvanceFGLG(IsSelectedGFLG, IsSelectedGFLG);
        }

        if(IsSelectedCS || IsSelectedTG || IsSelectedHDP || IsSelected1X2 || IsSelectedHTFT || IsSelectedGFLG){
            helper.expandAdvancedMode();
        }
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
    
    $('#btnSearch').click(function (e) {        
        e.stopPropagation();
        singleDetete.search();
        return false;
    });

    $('#btnVoidBet').click(function(e){
        e.stopPropagation();
        singleDetete.voidSingleTicket();
        return false;
    });

    $("#txtMatchId").keyup(function () {}).ForceIntegerOnly();
    $("#fromRefno").keyup(function () {}).ForceIntegerOnly();
    $("#toRefno").keyup(function () {}).ForceIntegerOnly();
    $("#hdp").keyup(function () {}).ForceDecimalOnly();
    $("#LiveHomeScore").keyup(function () {}).ForceIntegerOnly();
    $("#LiveAwayScore").keyup(function () {}).ForceIntegerOnly();
};

DelSingleTicket.prototype.SetupCalendar = function (strdatetime, txtdate) {
    $(txtdate).datetimepicker({
        showSecond: true,
        dateFormat: 'yy/m/d',
        timeFormat: 'HH:mm:ss',
        hourGrid: 3,
        minuteGrid: 5,
        secondGrid: 5,
        separator: ' '
    });

    if (strdatetime != null && strdatetime != "") {
        $(txtdate).val(strdatetime).datetimepicker('setDate', (new Date(strdatetime)));
    }
}


