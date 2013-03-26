/* File Created: February 5, 2013 */
var VoidNumberGame = {
    param:{
        MatchId: '',
        FromRefno: '',
        ToRefno: '',
        Dead: '',
        Live: ''
    },

    search: function(){
        $('#msg-box').hide();
        $('#msg').hide();        
                
        //Load Match Info
        if(VoidNumberGame.Validator.checkParamBeforeGetData()){
            VoidNumberGame.checkMatchIdWithRefno();
        }
    },

    getNumberGameTicket: function (){

         var param = $.extend(true, {}, VoidNumberGame.param, {
            Dead: VoidNumberGame.Helper.getDeadBallBetTypes(),
            Live: VoidNumberGame.Helper.getLiveBallBetTypes()
         });

         VoidNumberGame.param = param;

         $.ajax({
                type: "POST",
                url: window.rootUrl + "Gaming/VoidNumberGameTicket/GetNumberGameTicket",
                contentType: "application/json; charset=utf-8", 
                dataType: "json",
                async: false,
                data: JSON.stringify(param),
                beforeSend: function () {
                    NapTicketControl.disableInput();
                },
                success: function (rs) {
                    //render match info
                    VoidNumberGame.loadMatchInfo(rs.MatchInfo) ;
                    VoidNumberGame.loadNumberGameTicket(rs.Groups);
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
        if(matchInfo !== null && matchInfo !== undefined){
            var rows = [];
            rows.push({
            Columns: [{ Name: "code", Value: matchInfo.MatchCode, CssClass: 'bold' },
                { Name: "matchEvent", Value: (matchInfo.HomeName + ' -vs- ' + matchInfo.AwayName), CssClass: 'bold'},
                { Name: "eventDate", Value:  matchInfo.EventDate, CssClass: 'bold'}
            ]});

            var id = 'matchInfo';
            var gridOptions = {
                columns: [
                    { HeaderText: "Code", Name: "code", Width: 60, HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Match Event", Name: "matchEvent", Width: 300, HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Event Date", Name: "eventDate", Width: 120, HeaderAlign: "center", CellAlign: "center" }
                ],
                bodyRows: rows
            };

            $('#' + id).fanexGrid(gridOptions);
        }
    },

    loadNumberGameTicket : function(groups)
    {
        var rows = [];
        var length = groups[0].Tickets.length;

        for (var index = 0; index < length; index++) {
            var currentRow = groups[0].Tickets[index];
            rows.push({
                Columns:[
                    { Name: "No", Value: currentRow.No  },
                    { Name: "BetId", Value: currentRow.BetId },                    
                    { Name: "Member", Value: currentRow.Member },                    
                    { Name: "TransDate", Value:currentRow.TransDate  },
                    { Name: "BetTypeName", Value:currentRow.BetTypeName },
                    { Name: "Odds", Value:currentRow.OddHtml},
                    { Name: "Stake", Value: currentRow.StakeHtml},
                    { Name: "CurrencyName", Value:currentRow.Currency },
                    { Name: "Status", Value: currentRow.Status },
                    { Name: "Choice", Value:currentRow.ChoiceHtml},
                    { Name: "CustomAdd", Value: currentRow.BetId, TdClass:"Demo", CssClass: "demo2" }
                ],
                CssClass: "DemoCss"
            });
        };

        
        var gridOptions = {
            columns: [
                { HeaderText: "No", Name: "No", Width: 20, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "BetID", Name: "BetId", Width: 100, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Member", Name: "Member", Width: 100, HeaderAlign: "center", CellAlign: "left" },                
                { HeaderText: "Trans.Date", Name: "TransDate", Width: 140, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Bettype", Name: "BetTypeName", Width: 100, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Odds", Name: "Odds", Width: 60, HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: "Stake", Name: "Stake", Width: 60, HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                { HeaderText: "Currency", Name: "CurrencyName", Width: 60, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Status", Name: "Status", Width: 60, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Choice", Name: "Choice", Width: 270, HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: "<div><span>Option</span><span><input class='option' onclick='VoidNumberGame.Helper.setStausAllCheckTicket()' id='chk-ticket-all' type='checkbox' checked='checked'/></span></div>", Name: "CustomAdd", Width: 80, HeaderAlign: "center", CellAlign: "center", CustomCellRender: VoidNumberGame.optionCustomRender } 
            ],
            bodyRows: rows,
            showPager: true,
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
            VoidNumberGame.Helper.displayFormMsg(false);
        }
        else{
            VoidNumberGame.Helper.displayFormMsg(true);
        }
        
    },

    optionCustomRender: function (cell)
    {
        return "<input onclick='VoidNumberGame.Helper.eventSubTicket()'  class='chk-ticket' value='"+ cell.Value +"' type='checkbox' checked='checked'/>";
    },

    voidNumberGameTicket: function (){
        
        if(!VoidNumberGame.Validator.checkParamBeforeVoidTicket()){
            return;
        }

        var param = $.extend(true, {}, VoidNumberGame.param, {
            TransIds: VoidNumberGame.Helper.getSubTicketsChecked(),
            TransIds_N: VoidNumberGame.Helper.getSubTicketsChecked(), // NOTE: will be deleted when integrated with new sp
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
        });

        param.F_transid = param.FromRefno;
        param.T_transid = param.ToRefno;
        delete param.FromRefno;
        delete param.ToRefno;

        $.ajax({
                type: "POST",
                url: window.rootUrl + "Gaming/VoidNumberGameTicket/VoidNumberGameTicket",
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
                        VoidNumberGame.Helper.displayContent(false);
                    }
                },
                error: function () {
                    console.log('has error :(');
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
                url: window.rootUrl + "Gaming/VoidNumberGameTicket/CheckMatchIdWithRefno",
                contentType: "application/json; charset=utf-8", 
                dataType: "json",
                data: JSON.stringify(param),
                beforeSend: function () {
                    NapTicketControl.disableInput();
                },
                success: function (rs) {
                    if(rs.ErrorCode == 0){
                        VoidNumberGame.param = param;
                        VoidNumberGame.getNumberGameTicket();
                        VoidNumberGame.Helper.displayContent(true);
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
                        //VoidNumberGame.Helper.displayContent(false);
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
};

function VoidNumberGameTicket() {
    this.DOMElement =
    {
        $bettypeWrp: null,
        $body: null,
    };

    this.OldDataRequest = null;
};

VoidNumberGameTicket.prototype.CacheDOMElement = function () {
    var self = this;

    self.DOMElement.$bettypeWrp = $('.bettype-wrp');
    self.DOMElement.$body = $('#body');
}

VoidNumberGameTicket.prototype.Init = function () {
    
    jQuery("#VoidNumberGameTicketForm").validationEngine();

    var self = this;

    self.CacheDOMElement();    
    NapTicketControl.Helper.GetBettypes('NumberGame');
    VoidNumberGame.Helper.setStatusAllBettype(true);

    // handle events from DOM element
    $('.toggle-filter-bar').click(function () {
        NapTicketControl.Helper.ToggleFilterBar(this);
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
    
    $('#btnSearch').click(function (e) {        
        e.stopPropagation();
        VoidNumberGame.search();
        return false;
    });

    $('#btnVoidBet').click(function(e){
         e.stopPropagation();
        VoidNumberGame.voidNumberGameTicket();
        return false;
    });

    $("#txtMatchId").keyup(function () {}).ForceIntegerOnly();
    $("#fromRefno").keyup(function () {}).ForceIntegerOnly();
    $("#toRefno").keyup(function () {}).ForceIntegerOnly();
};
