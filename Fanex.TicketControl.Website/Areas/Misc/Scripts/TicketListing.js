/* File Created: February 5, 2013 */
function TicketListing() {
    this.DOMElement =
    {
        $ddlLiccensee: null,
        $ddlLiveType: null
    };
};

TicketListing.prototype.Init = function () {
    var self = this;
    self.GetLicensees();
    self.GetBettypes(self.Mode.VirtualSports);
    self.CacheDOMElement.$ddlLiveType.change(function () {
        var livetype = $(this).val(); // live type drop down list

        if (livetype === '0') {
            $('.type-title').show();
        } else {
            $('.type-title').hide();
        }
    });
};

TicketListing.prototype.GetLicensees = function () {
    var self = this;
    self.CacheDOMElement();
    $.ajax({
        type: "POST",
        url: window.rootUrl + "Misc/TicketListing/GetLicensees",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: null,
        beforeSend: function () {
            NapTicketControl.disableInput();
        },
        success: function (licensees) {
            if (licensees) {
                self.RenderLicenseeDropDownList(licensees);
            }
        },
        error: function () {

        },
        complete: function () {
            NapTicketControl.enableInput();
        }
    });
};

TicketListing.prototype.RenderLicenseeDropDownList = function (licensees) {
    var self = this;
    var licenseeLength = licensees.length;
    var $licensee = self.DOMElement.$ddlLiccensee;
    var licenseeHtml = '';
    
    for (var index = 0; index < licenseeLength; index++) {
        var currentLicensee = licensees[index];
        licenseeHtml += '<option value="' + currentLicensee.Id + '">' + currentLicensee.Name + '</option>';
    }

    $licensee.append("<optgroup label='Licensee'></optgroup>");
    $licensee.find('optgroup').append(licenseeHtml);
};


TicketListing.prototype.Mode = {
    SingleTicket: 0,
    NumberGame: 1,
    EarlyKO: 2,
    TicketListing: 3,
    MixParlay: 4,
    VirtualSports:5
};

TicketListing.prototype.GetBettypes = function (mode) {
    var self = this;
    self.CacheDOMElement();

    var param = {
        mode: mode
    };

    $.ajax({
        type: "POST",
        url: window.rootUrl + "Misc/TicketListing/GetBettypes",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(param),
        beforeSend: function () {
            NapTicketControl.disableInput();
        },
        success: function (bettypes) {
            if (bettypes) {
                //self.RenderLicenseeDropDownList(licensees);
            }
        },
        error: function () {

        },
        complete: function () {
            NapTicketControl.enableInput();
        }
    });
};

TicketListing.prototype.ChangeLiveType = function () {
    var livetype = $(this).val(); // live type drop down list

    if (livetype === '0') {
        $('type-title').show();
    } else {
        $('type-title').hide();
    }
}


TicketListing.prototype.CacheDOMElement = function () {
    var self = this;
    self.DOMElement.$ddlLiccensee = $('#ddlLicensee');
    self.CacheDOMElement.$ddlLiveType = $('#ddlLiveType');
}