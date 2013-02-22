var core = {

    GetLicensees: function () {
        $.ajax({
            type: "POST",
            url: window.rootUrl + "Core/Common/GetLicensees",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: null,
            beforeSend: function () {
                NapTicketControl.disableInput();
            },
            success: function (licensees) {
                if (licensees) {
                    core.RenderLicenseeDropDownList(licensees);
                }
            },
            error: function () {

            },
            complete: function () {
                NapTicketControl.enableInput();
            }
        });
    },

    RenderLicenseeDropDownList: function (licensees) {
        var self = this;
        var licenseeLength = licensees.length;
        var $licensee = $('.ddlLicensee');
        var licenseeHtml = '';

        for (var index = 0; index < licenseeLength; index++) {
            var currentLicensee = licensees[index];
            licenseeHtml += '<option value="' + currentLicensee.Id + '">' + currentLicensee.Name + '</option>';
        }

        $licensee.append("<optgroup label='Licensee'></optgroup>");
        $licensee.find('optgroup').append(licenseeHtml);
    }
};

