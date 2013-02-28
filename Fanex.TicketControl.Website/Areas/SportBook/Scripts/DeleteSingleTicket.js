/* File Created: February 5, 2013 */


var helper = {

    setStatusAllBettype: function (status){
        if (status) {
            $('.chkbettype').attr('checked', 'checked')
            $('#chkallbettype').attr('checked', 'checked')
            helper.setStatusAllAdvanceCS(true);        
        } else {
            $('.chkbettype').removeAttr('checked');
            $('#chkallbettype').removeAttr('checked');
            helper.setStatusAllAdvanceCS(false);        
        }
    },

    setStatusAllAdvanceCS: function (status){
        if (status) {
            $('.chk-score').attr('checked', 'checked');
            $('.chk-score').attr('disabled', false);
        } else {
            $('.chk-score').removeAttr('checked');
            $('.chk-score').attr('disabled', true);
        }
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
    }
};



var singleDetete = {
    search: function(){
        jQuery("#DeleteSingleTicketForm").validationEngine('validate');

        $('#msg').html(tooltip.show('hdp', 'what are you doing here!!!'));
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
        } else {
            $('.chkbettype').removeAttr('checked');
            helper.setStatusAllAdvanceCS(false);        
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
    });

    //Correct Scores check all
     $('body').delegate('#chk-score-all', 'click', function () {
        helper.setStatusAllAdvanceCS(this.checked);
    });

    //Correct Scores uncheck any item
     $('body').delegate('.chk-score', 'click', function () {
        helper.eventSubAdvanceCS();
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


