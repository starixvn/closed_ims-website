/* File Created: February 5, 2013 */


var helper = {

    setStatusAllBettype: function (status){
        if (status) {
            $('.chkbettype').attr('checked', 'checked')
            $('#chkallbettype').attr('checked', 'checked')
            helper.setStatusAllAdvanceCS(true);        
            helper.setEnableAllAdvanceCS(true);
            helper.setStatusAllAdvanceTG(true);
            helper.setEnableAllAdvanceTG(true);
        } else {
            $('.chkbettype').removeAttr('checked');
            $('#chkallbettype').removeAttr('checked');
            helper.setStatusAllAdvanceCS(false);        
            helper.setEnableAllAdvanceCS(false);
            helper.setStatusAllAdvanceTG(false);
            helper.setEnableAllAdvanceTG(false);
        }
    },

    setStatusAllAdvanceCS: function (status){
        if (status) {
            $('.chk-score').attr('checked', 'checked');               
        } else {
            $('.chk-score').removeAttr('checked');                        
        }
    },

    setEnableAllAdvanceCS: function(isEnable){
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

    setStatusAllAdvanceTG: function (status){
        if (status) {
            $('.chk-tg').attr('checked', 'checked');
        } else {
            $('.chk-tg').removeAttr('checked');            
        }
    },

    setEnableAllAdvanceTG: function(isEnable){
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
    }

    
};



var singleDetete = {
    search: function(){
        jQuery("#DeleteSingleTicketForm").validationEngine('validate');

        $('#msg').html(tooltip.show('hdp', 'what are you doing here!!!'));

        alert(helper.GetBetteam430());
        return;
                
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
            Betteam6: helper.GetBetteam6()
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
                { HeaderText: "Event Date", Name: "eventDate", Width: 80, HeaderAlign: "center", CellAlign: "center" },
            ],
            bodyRows: rows
        };

        $('#' + id).fanexGrid(gridOptions);
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
            helper.setStatusAllAdvanceCS(true);    
            helper.setEnableAllAdvanceCS(true);           
            helper.setStatusAllAdvanceTG(true);
            helper.setEnableAllAdvanceTG(true);
        } else {
            $('.chkbettype').removeAttr('checked');
            helper.setStatusAllAdvanceCS(false);        
            helper.setEnableAllAdvanceCS(false);           
            helper.setStatusAllAdvanceTG(false);
            helper.setEnableAllAdvanceTG(false);
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
        helper.setStatusAllAdvanceCS(IsSelectedCS); 
        helper.setEnableAllAdvanceCS(IsSelectedCS);       
        
        var IsSelectedTG = helper.IsSelectedBettype(6);
        helper.setStatusAllAdvanceTG(IsSelectedTG);
        helper.setEnableAllAdvanceTG(IsSelectedTG);
    });

    //Correct Scores check all
     $('body').delegate('#chk-score-all', 'click', function () {
        helper.setStatusAllAdvanceCS(this.checked);
    });

    //Correct Scores uncheck any item
     $('body').delegate('.chk-score', 'click', function () {
        helper.eventSubAdvanceCS();
    });


     //TG check all
     $('body').delegate('#chk-tg-all', 'click', function () {
        helper.setStatusAllAdvanceTG(this.checked);
    });


     //TG uncheck any item
     $('body').delegate('.chk-tg', 'click', function () {
        helper.eventSubAdvanceTG();
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


