/* File Created: February 5, 2013 */
var voidNumberGame = {
    param:{
        MatchId: '',
        FromRefno: '',
        ToRefno: ''
    },

    search: function(){
        $('#msg-box').hide();
        $('#msg').hide();        
                
        //Load Match Info
        if(validateData.checkParamBeforeGetData()){
            voidNumberGame.checkMatchIdWithRefno();
        }
    },

    getNumberGameTicket: function (){

         var param = {
            MatchId: voidNumberGame.param.MatchId,
            FromRefNo: voidNumberGame.param.FromRefno,
            ToRefNo: voidNumberGame.param.ToRefno,
            Dead: helper.getDeadBallBetTypes(),
            Live: helper.getLiveBallBetTypes()
         };

         $.ajax({
                type: "POST",
                url: window.rootUrl + "Gaming/VoidNumberGameTicket/GetNumberGameTicket",
                contentType: "application/json; charset=utf-8", 
                dataType: "json",
                data: JSON.stringify(param),
                beforeSend: function () {
                    NapTicketControl.disableInput();
                },
                success: function (rs) {
                    //render match info
                    voidNumberGame.loadNumberGameTicket(rs.Groups);
                },
                error: function () {
                    console.log('has error :(');
                },
                complete: function () {
                    NapTicketControl.enableInput();
                }
            });
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
                    { Name: "BetId", Value: currentRow.Ticket.TransId },                    
                    { Name: "Member", Value: currentRow.Ticket.UserName },                    
                    { Name: "TransDate", Value:currentRow.TransDate  },
                    { Name: "BetTypeName", Value:currentRow.BetTypeName },
                    { Name: "Odds", Value:currentRow.OddHtml},
                    { Name: "Stake", Value: currentRow.StakeHtml},
                    { Name: "CurrencyName", Value:currentRow.Currency },
                    { Name: "Status", Value: currentRow.Ticket.Status },
                    { Name: "Choice", Value:currentRow.ChoiceHtml},
                    { Name: "CustomAdd", Value: currentRow.Ticket.TransId, TdClass:"Demo", CssClass: "demo2" }
                ],
                CssClass: "DemoCss"
            });
        };

        
        var gridOptions = {
            columns: [
                { HeaderText: "No", Name: "No", Width: 20, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "BetID", Name: "BetId", Width: 100, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Member", Name: "Member", Width: 100, HeaderAlign: "center", CellAlign: "center" },                
                { HeaderText: "Trans.Date", Name: "TransDate", Width: 80, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Bettype", Name: "BetTypeName", Width: 100, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Odds", Name: "Odds", Width: 60, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Stake", Name: "Stake", Width: 60, HeaderAlign: "center", CellAlign: "center", FormatNumber: "0,0.00" },
                { HeaderText: "Currency", Name: "CurrencyName", Width: 60, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Status", Name: "Status", Width: 60, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Choice", Name: "Choice", Width: 200, HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: "<div><span>Option</span><span><input class='option' onclick='helper.setStausAllCheckTicket()' id='chk-ticket-all' type='checkbox' checked='checked'/></span></div>", Name: "CustomAdd", Width: 80, HeaderAlign: "center", CellAlign: "center", CustomCellRender: voidNumberGame.optionCustomRender } 
            ],
            bodyRows: rows,
            showPager: true,
            pagerOption: {
                itemsPerPage: 50,
                maxItemsPerPage: 100,
                currentPage: 0,
                totalItem: 100,
                startPerPage: 50,
                pageStep: 50
            }
        };

        $('#ticketInfo').fanexGrid(gridOptions);

        if(groups[0].Tickets.length == 0){
            helper.displayFormMsg(false);
        }
        else{
            helper.displayFormMsg(true);
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
                success: function (rs) {
                    //render match info
                    NapTicketControl.Helper.showMsgBox(rs);
                    if(rs != null && rs.length>0 && rs[0].ErrorCode < 0){
                        helper.displayContent(false);
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
//                    if(rs.ErrorCode == 0){
                        voidNumberGame.param = param;
                        voidNumberGame.getNumberGameTicket();
                        helper.displayContent(true);
//                    }
//                    else{
//                        switch(rs.ErrorCode){
//                            case 1:
//                            case 2:
//                                $('#msg').html(tooltip.show('txtMatchId', rs.ErrorMsg)).show();
//                                break;
//                            case 3:
//                                $('#msg').html(tooltip.show('fromRefno', rs.ErrorMsg)).show();
//                                break;
//                            case 4:
//                                $('#msg').html(tooltip.show('toRefno', rs.ErrorMsg)).show();
//                                break;
//                        }

//                        helper.displayContent(false);
//                    }
                    
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

VoidNumberGameTicket.prototype.Mode = {
    Sportsbook: 0
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
    helper.setStatusAllBettype(false);

    // handle events from DOM element
    $('.tbl-advance-mode thead th').click(function(){
        var iEle = $(this).find('i');
        if(iEle.hasClass('icon-plus-sign'))
        {
            iEle.removeClass('icon-plus-sign');
            iEle.addClass('icon-minus-sign');
            $('.tbl-advance-mode tbody').show();
        }
        else
        {
            iEle.addClass('icon-plus-sign');
            iEle.removeClass('icon-minus-sign');
            $('.tbl-advance-mode tbody').hide();
        }
    })

    // collapse / expand filter bar
    $('.colapse-expand-filter-bar').click(function () {
        var iEle = $(this).find('i');
        if (iEle.hasClass('icon-minus-sign')) {
            iEle.removeClass('icon-minus-sign');
            iEle.addClass('icon-plus-sign');
            $('.note-filter-bar').hide();
            $('.filter-bar-inner').hide();
            $('#collapse-expand-label').text('Expand Filter');
        } else {
            iEle.removeClass('icon-plus-sign');
            iEle.addClass('icon-minus-sign');
            $('.note-filter-bar').show();
            $('.filter-bar-inner').show();
            $('#collapse-expand-label').text('Collapse Filter');
        }
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
        voidNumberGame.search();
        return false;
    });

    $("#txtMatchId").keyup(function () {}).ForceIntegerOnly();
    $("#fromRefno").keyup(function () {}).ForceIntegerOnly();
    $("#toRefno").keyup(function () {}).ForceIntegerOnly();
};
