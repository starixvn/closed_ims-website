/* File Created: March 12, 2013 */

$(function () {
    outright.init();
});

var outright = {
    init: function () {
        jQuery("#VoidOutrightTicketForm").validationEngine();

        $("#fromRefno").keyup(function () { }).ForceIntegerOnly();
        $("#toRefno").keyup(function () { }).ForceIntegerOnly();

        $('body').delegate('#btnSearch', 'click', function (e) {
            e.stopPropagation();
            outright.search(e);

        });

        outright.loadLeague();
        outright.loadTeamByLeague($('#ddlLeague').val());
    },

    loadLeague: function () {
        $.ajax({
            type: "POST",
            url: window.rootUrl + "SportBook/VoidOutrightTicket/GetOutrightLeague",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            beforeSend: function () {
                NapTicketControl.disableInput();
            },
            success: function (rs) {
                var league = [];

                for (var i = 0; i < rs.length; i++) {
                    league.push('<option value="' + rs[i].Id + '">' + rs[i].Name + '</option>');
                }

                $('#ddlLeague').empty().append(league.join(''));
            },
            error: function () {
                console.log('has error :(');
            },
            complete: function () {
                NapTicketControl.enableInput();
            }
        });
    },

    loadTeamByLeague: function (Id) {

        var param = {
            LeagueId: Id
        };

        $.ajax({
            type: "POST",
            url: window.rootUrl + "SportBook/VoidOutrightTicket/GetTeamByLeague",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            beforeSend: function () {
                NapTicketControl.disableInput();
            },
            success: function (rs) {
                var league = [];

                for (var i = 0; i < rs.length; i++) {
                    league.push('<option value="' + rs[i].Id + '">' + rs[i].Name + '</option>');
                }

                $('#ddlTeam').empty().append(league.join(''));
            },
            error: function () {
                console.log('has error :(');
            },
            complete: function () {
                NapTicketControl.enableInput();
            }
        });
    },

    search: function () {
        alert('search');
    }
}