/* File Created: March 8, 2013 */

$(function () {
    recoverTicket.init();
});

var helper = {
    displayContent: function (isShow) {
        if (isShow) {
            $('#report-content').show();
        }
        else {
            $('#report-content').hide();
            $('#txtBetId').val('');
        }
    },

    displayMoreMsg: function () {
        var symbol = $('#spMoreMsg').html();

        if (symbol == '[+]') {
            $('.other-languages').show();
        }
        else {
            $('.other-languages').hide();
        }

        symbol = symbol == '[-]' ? '[+]' : '[-]';
        $('#spMoreMsg').html(symbol);
    },

    checkParamBeforRecoverTicket: function () {
        $('#msg').hide();

        if ($("#msg-en").val() == "") {
            $('#msg').html(tooltip.show('msg-en', 'You must enter English message')).show();
            return false;
        }
        if ($("#msg-en").val().indexOf("@@") < 0) {
            $('#msg').html(tooltip.show('msg-en', 'You must include @@ in English message for replacing ticket number')).show();
            return false;
        }
        if ($("#msg-ch").val() == "") {
            $('#msg').html(tooltip.show('msg-ch', 'You must enter Chinese (Mandarin) message')).show();
            return false;
        }
        if ($("#msg-ch").val().indexOf("@@") < 0) {
            $('#msg').html(tooltip.show('msg-ch', 'You must include @@ in Chinese (Mandarin) message for replacing ticket number')).show();
            return false;
        }

      
        return true;
    }
}

var recoverTicket = {
    init: function () {
        jQuery("#RecoverTicketForm").validationEngine();
        
        $("#txtBetId").keyup(function () { }).ForceIntegerOnly();

        $('body').delegate('#btnSearch', 'click', function (e) {
            e.stopPropagation();
            recoverTicket.search(e);

        });
    },

    validData: function () {

        $('#msg-box').hide();
        if ($('#txtBetId').val() == '') {
            $('#msg').html(tooltip.show('txtBetId', 'Bet ID is required.')).show();
            return false;
        }

        if ($('#txtBetId').val().length > 10 || isNaN($('#txtBetId').val())) {
            $('#msg').html(tooltip.show('txtBetId', 'Invalid Bet ID.')).show();
            return false;
        }
        return true;
    },

    search: function (e) {
        if (!recoverTicket.validData(e)) {
            return;
        }

        var param = {
            TransId: $('#txtBetId').val()
        };
        $.ajax({
            type: "POST",
            url: window.rootUrl + "SportBook/RecoverTicket/GetTicketRecover",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            beforeSend: function () {
                NapTicketControl.disableInput();
            },
            success: function (rs) {
                if (rs != null && rs.ErrorCode > 0) {
                    $('#msg').html(tooltip.show('txtBetId', rs.ErrorMsg)).show();
                    helper.displayContent(false);
                    $('#fsMessage').hide();
                }
                else {
                    helper.displayContent(true);
                    recoverTicket.loadMatchInfo(rs.MatchInfo);
                    recoverTicket.loadSingleTicket(rs.Groups);
                    $('#fsMessage').show();
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

    loadMatchInfo: function (matchInfo) {
        var rows = [];
        rows.push({
            Columns: [{ Name: "code", Value: matchInfo.MatchCode },
            { Name: "matchEvent", Value: (matchInfo.HomeName + ' -vs- ' + matchInfo.AwayName) },
            { Name: "eventDate", Value: matchInfo.EventDate }
        ]
        });

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

    loadSingleTicket: function (groups) {
        var rows = [];
        var length = groups[0].Tickets.length;
        for (var index = 0; index < length; index++) {
            var currentRow = groups[0].Tickets[index];
            rows.push({
                Columns: [
                    { Name: "No", Value: currentRow.No },
                    { Name: "BetId", Value: currentRow.BetId },
                    { Name: "Player", Value: currentRow.Player },
                    { Name: "TransDate", Value: currentRow.TransDate },
                    { Name: "BetTypeName", Value: currentRow.BetTypeName },
                    { Name: "Odds", Value: currentRow.Odds },
                    { Name: "Choice", Value: currentRow.Choice },
                    { Name: "Stake", Value: currentRow.Stake },
                    { Name: "CurrencyName", Value: currentRow.CurrencyName },
                    { Name: "Score", Value: currentRow.Score },
                    { Name: "Status", Value: currentRow.Status }
                    //{ Name: "CustomAdd", Value: currentRow.BetId, TdClass: "Demo", CssClass: "demo2" }
                ],
                CssClass: "DemoCss"
            });
        };


        var gridOptions = {
            columns: [
                { HeaderText: "No", Name: "No", Width: 20, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "BetID", Name: "BetId", Width: 100, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Member", Name: "Player", Width: 100, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Trans.Date", Name: "TransDate", Width: 80, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Bettype", Name: "BetTypeName", Width: 100, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Odds", Name: "Odds", Width: 60, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Choice", Name: "Choice", Width: 200, HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: "Stake", Name: "Stake", Width: 60, HeaderAlign: "center", CellAlign: "center", FormatNumber: "0,0.00" },
                { HeaderText: "Currency", Name: "CurrencyName", Width: 60, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Score", Name: "Score", Width: 60, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Status", Name: "Status", Width: 60, HeaderAlign: "center", CellAlign: "center" }
               // { HeaderText: "<div><span>Recover</span></div>", Name: "CustomAdd", Width: 80, HeaderAlign: "center", CellAlign: "center", CustomCellRender: recoverTicket.optionCustomRender }
            ],
            bodyRows: rows
        };

        $('#ticketInfo').fanexGrid(gridOptions);



    },

    optionCustomRender: function (cell) {

        return "<div onclick='recoverTicket.actionRecover(" + cell.Value + ")' class='undo'></div>";
    },

    actionRecover: function () {

        var BetId = $('#txtBetId').val();

        if (!helper.checkParamBeforRecoverTicket()) {
            return;
        }

        if (!confirm('Are you sure you want to recover transaction ' + BetId + ' ?')) {
            return;
        }

        var param = {
            TransId: BetId,
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
            url: window.rootUrl + "SportBook/RecoverTicket/RecoverTicket",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            beforeSend: function () {
                NapTicketControl.disableInput();
            },
            success: function (rs) {
                if (rs != null && rs.ErrorCode > 0) {
                    helper.displayContent(true);
                }
                else {
                    helper.displayContent(false);
                }

                NapTicketControl.Helper.showMsgBox(rs);
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