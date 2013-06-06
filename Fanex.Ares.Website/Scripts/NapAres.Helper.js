
if (NapAres === undefined)
{
    NapAres = {};
}

NapAres.Helper = (function ($) {

    // define some private variables
    var _productTypes = {
        Sportsbook: 0,
        NumberGame: 1,
        LiveCasino: 6,
        TicketListing: 3
    };

    // used to store local bettypes, 
    // and avoid request to server when they are avaible
    var _betTypes = {
        Sportsbook: null,
        NumberGame: null,
        LiveCasino: null,
        TicketListing: null
    }

    return {
        authorizedMsg: 'You are not authorized on this function.',
        msgbox: $('#dialog-popup'),
        GetLicensees: function () {
            $.ajax({
                type: "POST",
                url: window.rootUrl + "Core/Common/GetLicensees",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: null,
                beforeSend: function () {
                    NapAres.disableInput();
                },
                success: function (licensees) {
                    if (licensees) {
                        NapAres.Helper.RenderLicenseeDropDownList(licensees);
                    }
                },
                error: function () {
                },
                complete: function () {
                    NapAres.enableInput();
                }
            });
        },
        showMsgBox: function (msgs) {
            if (this.msgbox.length == 0) {
                this.msgbox = $(
                    '<div class="alert alert-error" id="dialog-popup">' +
                        '<button type="button" class="close" data-dismiss="alert">&times;</button>' +
                        '<div class="alert-content">' +
                        '</div>' +
                    '</div>').prependTo('body');
            }


            var html = [];
            var isHasError = false;
            if ($.isPlainObject(msgs)) {
                var msg = msgs;
                if (msg.ErrorCode > 0) {//Fail
                    html.push('<span class="fail">' + msg.ErrorMsg + '<i class="icon-warning-sign"></i></span><br/>');
                    isHasError = true;
                }
                else {
                    html.push('<span>' + msg.ErrorMsg + '&nbsp;<i class="icon-ok"></i></span><br/>');
                }
            }
            else {
                if ($.isArray(msgs)) {
                    for (var i = 0; i < msgs.length; i++) {
                        var msg = msgs[i];
                        if (msg.ErrorCode > 0) {
                            //Fail
                            html.push('<span class="fail">' + msg.ErrorMsg + '&nbsp;<i class="icon-warning-sign"></i></span><br/>');
                            isHasError = true;
                        }
                        else {
                            html.push('<span>' + msg.ErrorMsg + '&nbsp;<i class="icon-ok"></i></span><br/>');
                        }
                    }
                } else {
                    html.push('<span class="fail">' + msgs + '<i class="icon-warning-sign"></i></span><br/>');
                    isHasError = true;
                }
            }

            this.msgbox.attr('class', 'alert alert-success' + (isHasError == true ? ' err' : '')).find('.alert-content').html(html.join(''));
            this.msgbox.fadeIn();
            this.msgbox.show();
        },
        displayContent: function (isShow) {
            if (isShow) {
                $('#report-content').show();
            }
            else {
                $('#report-content').hide();
            }
        },
        ToggleFilterBar: function () {
            if ($('.filter-bar-inner').css('display') == 'none') {
                $('.filter-bar-inner').show();
            }
            else {
                $('.filter-bar-inner').hide();
            }

        },
        hideMsgBox: function () {
            this.msgbox.fadeOut();
            this.msgbox.hide();
        }
    };
} (jQuery));