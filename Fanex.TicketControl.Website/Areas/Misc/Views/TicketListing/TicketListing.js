/* File Created: February 5, 2013 */
function TicketListing() {
    this.DOMElement =
    {
        $liccenseeDropDownList: null
    };
};

TicketListing.prototype.Init = function () {
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
    var $licensee = self.DOMElement.$liccenseeDropDownList;
    var licenseeHtml = '';
    for (var index = 0; index < licenseeLength; index++) {
        var currentLicensee = licensees[index];
        licenseeHtml += '<option value="' + currentLicensee.Id + '">' + currentLicensee.Name + '</option>';
    }
    $licensee.find('optgroup').append(licenseeHtml);
};
TicketListing.prototype.CacheDOMElement = function () {
    var self = this;
    self.DOMElement.$liccenseeDropDownList = $('#licenseeDropDown');
}