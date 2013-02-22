var core = {

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

