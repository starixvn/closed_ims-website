//function ReCheckin() {
//    var KickedOut = 3;
//    var strOnUserId = $("#hdfOnUserId").val();

//    $.ajax({
//        url: location.protocol + '//' + location.host + "/UserLogin/Login/ReCheckLogin",
//        type: "POST",       
//        dataType: "json",
//        success: function (result) {
////            console.log(result);
//            if (result == "1") {
////                console.log(result + "OK");
//                window.location.href = location.protocol + '//' + location.host + "/UserLogin/Login/LogOut";
//            }
//        },
//        error: function (request, status, error) {
//            console.log(error);
//        }
//    });
//}

//function closeBrowser() {
//    var index = window;
//    while (index != index.parent) // Jump out
//    {
//        index = index.parent;
//    }

//    index.close();
//}

//$(document).ready(function () {
//    setInterval("ReCheckin()", 10000);
//});

var Recheckin = function ($) {

    return {
        time: null,
        root: null,
        headlineMsgBox: null,
        kickedOutMsg: null,

        init: function () {
            Recheckin.time = 5000;
            Recheckin.root = location.protocol + '//' + location.host + $('#NapHubRoot').val();
            Recheckin.kickedOutMsg = 'You have logged in from another session or your login session has expired!';
        },

        reCheckin: function () {
            $.ajax({
                url: Recheckin.root + "UserLogin/Login/ReCheckLogin",
                type: "POST",
                dataType: "json",
                success: function (result) {
                    if (typeof (result) != 'undefined' && result != null) {
                        if (result.IsKickedOut == "1" || result.IsKickedOut == "2") {
                            alert('Another session has been logged in or your session is expired. Forced to logout!');
                            window.location.href = Recheckin.root + "UserLogin/Login/LogOut";
                        }
                        else if (result.IsKickedOut == "3") {
                            window.location.href = Recheckin.root + "UserLogin/Common/Offline";
                        }
                    }
                },
                error: function (request, status, error) {
                }
            });
        },

        closeBrowser: function () {
            var index = window;
            while (index != index.parent) // Jump out
            {
                index = index.parent;
            }
            index.close();
        }
    }

} (jQuery);


$(document).ready(function () {
    Recheckin.init();

    Recheckin.headlineMsgBox = $.msgbox({
        message: '',
        //containerId: 'msg-bar',
        type: 'err',
        modal: false,
        position: ['top:130', 'center'],
        theme: '',
        zIndex: 9999,
        duration: 2000,
        useAnimation: false,
        beforeClose: function () {
            window.location.href = Recheckin.root + "UserLogin/Login/LogOut";
        }
    });

    setInterval("Recheckin.reCheckin()", Recheckin.time);
});