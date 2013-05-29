$(document).ready(function () {
    var partyManager = new PartyManager();
    partyManager.GenerateData();
});

PartyManager = function () {
    var self = this;

    self.cacheDOMElements = function () { }

    self.BindingEvent = function () {

    }

    self.GenerateData = function () {
        $.ajax({
            type: "POST",
            url: window.rootUrl + "Admin/Party/GetAllParties",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: null,
            beforeSend: function () {
                NapTicketControl.disableInput();
            },
            success: function (rs) {
                renderParties(rs);
            },
            error: function (event, jqXHR, ajaxSettings) {
                renderParties([]);
            },
            complete: function () {
                NapTicketControl.enableInput();
            }
        });

    };

    var renderParties = function (data) {
        var bodyData = [];
        if (data && data.length > 0) {
            var dataLength = data.length;
            for (var i = 0; i < dataLength; i++) {
                var item = data[i];
                var currentRow = {
                    Columns: [
                                { Name: "PartyName", Value: item.PartyName },
                                { Name: "PartyType", Value: item.PartyType },
                                { Name: "Description", Value: item.Description },
                                { Name: "PartyFullName", Value: item.PartyFullName },
                                { Name: "AccountNumber", Value: item.AccountNumber },
                                { Name: "AccountName", Value: item.AccountName },
                                { Name: "BankName", Value: item.BankName },
                                { Name: "Benificiary", Value: item.Benificiary },
                                { Name: "IBAN", Value: item.IBAN },
                                { Name: "SwiftCode", Value: item.SwiftCode },
                                { Name: "CorrespondentBank", Value: item.CorrespondentBank },
                                { Name: "AttnName", Value: item.AttnName },
                                { Name: "To1", Value: item.To1 },
                                { Name: "To2", Value: item.To2 },
                                { Name: "Edit", Value: item.PartyID, CellAlign: "center", CustomCellRender: function (cell, cellFormated) { return GenerateEditButton(cell, cellFormated); } },
                                { Name: "AddLicensee", Value: item.PartyID, CellAlign: "center", CustomCellRender: function (cell, cellFormated) { return GenerateAddLicenseeButton(cell, cellFormated); } }
                            ]
                };

                bodyData.push(currentRow);
            }
        }

        var GenerateEditButton = function (cell, cellFormated) {
            return "<input value='Edit' type='button' onclick='EditParty(" + cell.Value + ")' />";
        }

        var GenerateAddLicenseeButton = function (cell, cellFormated) {
            return "<input value='Add Licensee' type='button' onclick='AddLicensee(" + cell.Value + ")' />";
        }

        var gridOptions = {
            columns: [{ HeaderText: "Party Name", Width: 150, Name: "PartyName", HeaderAlign: "center", CellAlign: "left", ShowCollapseExpand: true },
                    { HeaderText: "PartyType", Width: 100, Name: "PartyType", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Description", Width: 100, Name: "Description", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "PartyFullName", Width: 100, Name: "PartyFullName", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "AccountNumber", Width: 100, Name: "AccountNumber", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "AccountName", Width: 100, Name: "AccountName", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "BankName", Width: 100, Name: "BankName", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Benificiary", Width: 100, Name: "Benificiary", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "IBAN", Width: 100, Name: "IBAN", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "SwiftCode", Width: 100, Name: "SwiftCode", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "CorrespondentBank", Width: 100, Name: "CorrespondentBank", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "AttnName", Width: 100, Name: "AttnName", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "To1", Width: 100, Name: "To1", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "To2", Width: 100, Name: "To2", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Edit", Width: 100, Name: "Edit", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "AddLicensee", Width: 100, Name: "AddLicensee", HeaderAlign: "center", CellAlign: "left"}],
            showTitle: true,
            //            showFooter: true,
            titleInfo:
            {
                text: "Party Manager",
                showExcelButton: false,
                showCollapseExpandButton: true
            },
            showTree: true,
            bodyRows: bodyData,
            showDistinctRow: false,
            //footerRows: null,
            gridExpandHeight: 200
        };

        $('#GridParties').fanexGrid(gridOptions);
    };
}

