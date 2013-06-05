var partyManager;
$(document).ready(function () {
    partyManager = new PartyManager();
    partyManager.initPage();
});

PartyManager = function () {
    var self = this;
    var _table = {
        btnAddNew: null,
        partiesContainer: null,
        dlgAddNew: null,
        dlgAddLicensee: null,
        frmParty: null,
        frmPartyLicensee: null,
        errCode: null,
        errMsg: null
    };

    var _mappingColumns = {
        PartyID: 0,
        PartyName: 1,
        PartyType: 2,
        Description: 3,
        PartyFullName: 4,
        AccountNumber: 5,
        AccountName: 6,
        BankName: 7,
        Benificiary: 8,
        IBAN: 9,
        SwiftCode: 10,
        CorrespondentBank: 11,
        AttnName: 12,
        To1: 13,
        To2: 14
    }

    var _dlgAddNew = {
        hidPartyID: null,
        txtPartyID: null,
        txtPartyName: null,
        selPartyType: null,
        txtDescription: null,
        txtPartyFullName: null,
        txtAccountNumber: null,
        txtAccountName: null,
        txtBankName: null,
        txtBenificiary: null,
        txtIBAN: null,
        txtSwiftCode: null,
        txtCorrespondentBank: null,
        txtAttnName: null,
        txtTo1: null,
        txtTo2: null,
        txtLogo: null,
        txtValidateTips: null
    };
    var gridOptions = {};

    var cachedDOMElements = function () {
        _table.btnAddNew = $('#btnAddNew');
        _table.partiesContainer = $('#PartiesContainer');
        _table.dlgAddNew = $('#AddNewDialog');
        _table.dlgAddLicensee = $('#PartyLicenseeDialog');
        _table.frmParty = $('#formParty');
        _table.frmPartyLicensee = $('#formPartyLicensee');
        _table.errCode = $('#ErrCode');
        _table.errMsg = $('#ErrMsg');

        _dlgAddNew.hidPartyID = $('#PartyID');
        _dlgAddNew.txtPartyID = $('#txtPartyID');
        _dlgAddNew.txtPartyName = $('#PartyName');
        _dlgAddNew.selPartyType = $('#PartyType');
        _dlgAddNew.txtDescription = $('#Description');
        _dlgAddNew.txtPartyFullName = $('#PartyFullName');
        _dlgAddNew.txtAccountNumber = $('#AccountNumber');
        _dlgAddNew.txtAccountName = $('#AccountName');
        _dlgAddNew.txtBankName = $('#BankName');
        _dlgAddNew.txtBenificiary = $('#Benificiary');
        _dlgAddNew.txtIBAN = $('#IBAN');
        _dlgAddNew.txtSwiftCode = $('#SwiftCode');
        _dlgAddNew.txtCorrespondentBank = $('#CorrespondentBank');
        _dlgAddNew.txtAttnName = $('#AttnName');
        _dlgAddNew.txtTo1 = $('#To1');
        _dlgAddNew.txtTo2 = $('#To2');
        _dlgAddNew.txtLogo = $('#Logo');
        _dlgAddNew.txtValidateTips = $('#validateTips');
    }

    var bindingEvent = function () {
        _table.btnAddNew.click(function () {
            self.AddNewParty();

        });
    }

    self.initPage = function () {
        cachedDOMElements();
        bindingEvent();
        GenerateData();
        checkError();
    }

    var checkError = function () {
        if (!(_table.errCode.val() == undefined || _table.errCode.val() == "" || _table.errCode.val() == "0")) {
            alert(_table.errMsg.val());
        }
    }

    var GenerateData = function () {

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
                                { Name: "PartyID", Value: item.PartyID },
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
                                { Name: "Edit", Value: i, CellAlign: "center", CustomCellRender: function (cell, cellFormated) { return GenerateEditButton(cell, cellFormated); } },
                                { Name: "AddLicensee", Value: i, CellAlign: "center", CustomCellRender: function (cell, cellFormated) { return GenerateAddLicenseeButton(cell, cellFormated); } }
                            ]
                };

                bodyData.push(currentRow);
            }
        }

        var GenerateEditButton = function (cell, cellFormated) {
            return "<input value='Edit' type='button' onclick='partyManager.EditParty(" + cell.Value + ")' />";
        }

        var GenerateAddLicenseeButton = function (cell, cellFormated) {
            return "<input value='Add Licensee' type='button' onclick='partyManager.AddLicensee(" + cell.Value + ")' />";
        }

        gridOptions = {
            columns: [
                    { HeaderText: "Party ID", Width: 100, Name: "PartyID", HeaderAlign: "center", CellAlign: "left", Hide: true },
                    { HeaderText: "Party Name", Width: 150, Name: "PartyName", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "PartyType", Width: 100, Name: "PartyType", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Description", Width: 100, Name: "Description", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Party Full Name", Width: 200, Name: "PartyFullName", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Account Number", Width: 100, Name: "AccountNumber", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Account Name", Width: 100, Name: "AccountName", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Bank Name", Width: 100, Name: "BankName", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Benificiary", Width: 100, Name: "Benificiary", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "IBAN", Width: 100, Name: "IBAN", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "SwiftCode", Width: 100, Name: "SwiftCode", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "CorrespondentBank", Width: 100, Name: "CorrespondentBank", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Attn Name", Width: 100, Name: "AttnName", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "To1", Width: 270, Name: "To1", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "To2", Width: 270, Name: "To2", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Edit", Width: 100, Name: "Edit", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Add Licensee", Width: 100, Name: "AddLicensee", HeaderAlign: "center", CellAlign: "left"}],
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

        _table.partiesContainer.fanexGrid(gridOptions);
        _table.partiesContainer.fanexGrid("hideColumns", gridOptions, [_mappingColumns.PartyID,
                                                                        _mappingColumns.PartyType,
                                                                        _mappingColumns.Description,
                                                                        _mappingColumns.AccountNumber,
                                                                        _mappingColumns.AccountName,
                                                                        _mappingColumns.BankName,
                                                                        _mappingColumns.Benificiary,
                                                                        _mappingColumns.IBAN,
                                                                        _mappingColumns.SwiftCode,
                                                                        _mappingColumns.CorrespondentBank]);
    };

    self.AddNewParty = function () {
        var originalDialog = _table.dlgAddNew.html();
        cachedDOMElements();
        _table.dlgAddNew.find('.update').each(function () { $(this).hide() });
        _table.dlgAddNew.dialog({
            autoOpen: false,
            height: 600,
            width: 500,
            modal: true,
            buttons: {
                "Create a Party": function () {
                    if (ValidateForm()) {
                        _dlgAddNew.txtValidateTips.hide();
                        _table.frmParty.submit();
                        $(this).dialog("close");
                    }
                    else {
                        _dlgAddNew.txtValidateTips.show();
                    }
                },
                Cancel: function () {
                    $(this).dialog("close");
                }
            },
            open: function () {

            },
            close: function () {
                _table.dlgAddNew.html(originalDialog);
            }
        });

        _table.dlgAddNew.dialog('option', 'title', 'Add new Party');
        _table.dlgAddNew.dialog('open');
    };

    self.EditParty = function (rowNum) {
        _table.dlgAddNew.attr('title', 'Edit Party');
        var originalDialog = _table.dlgAddNew.html();
        cachedDOMElements();
        FillDataToEditForm(rowNum);
        _table.dlgAddNew.dialog({
            autoOpen: false,
            height: 600,
            width: 500,
            modal: true,
            buttons: {
                "Edit Party": function () {
                    if (ValidateForm()) {
                        _dlgAddNew.txtValidateTips.hide();
                        _table.frmParty.attr("Action", "/admin/Party/UpdateParty");
                        _table.frmParty.submit();
                        $(this).dialog("close");
                    }
                    else {
                        _dlgAddNew.txtValidateTips.show();
                    }
                },
                Cancel: function () {
                    $(this).dialog("close");
                }
            },
            open: function () {

            },
            close: function () {
                _table.dlgAddNew.html(originalDialog);
            }
        });

        _table.dlgAddNew.dialog('option', 'title', 'Edit Party');
        _table.dlgAddNew.dialog('open');
    };

    self.AddLicensee = function (rowNum) {
        var originalDialog = _table.dlgAddLicensee.html();
        var row = gridOptions.bodyRows[rowNum].Columns;
        var partyID = row[_mappingColumns.PartyID].Value;
        $('#formPartyLicensee > #PartyID').val(partyID);
        _table.dlgAddLicensee.dialog({
            autoOpen: false,
            height: 350,
            width: 380,
            modal: true,
            buttons: {
                "Save": function () {
                    var xmlData = "<Settings>";
                    $('#selected_columns option').each(function () {
                        xmlData += "<Setting>";
                        xmlData += "<Party>";
                        xmlData += partyID.toString();
                        xmlData += "</Party>";
                        xmlData += "<Licensee>";
                        xmlData += $(this).attr('value');
                        xmlData += "</Licensee>";
                        xmlData += "</Setting>";
                    })

                    xmlData += "</Settings>";

                    $('#formPartyLicensee > #XmlSettings').val(xmlData);
                    $('#formPartyLicensee').submit();
                    $(this).dialog("close");
                },
                Cancel: function () {
                    $(this).dialog("close");
                }
            },
            open: function () {
                $.ajax({
                    type: "POST",
                    url: window.rootUrl + "Admin/Party/_PartyLicensee",
                    dataType: "html",
                    data: "PartyID=" + partyID,
                    beforeSend: function () {
                        NapTicketControl.disableInput();
                    },
                    success: function (rs) {
                        $('#PartyLicenseeDialog').html(rs);
                    },
                    error: function (event, jqXHR, ajaxSettings) {
                        alert('fail');
                    },
                    complete: function () {
                        NapTicketControl.enableInput();
                    }
                });

                $('#selected_columns option').each(function () {
                    $(this).attr('disabled', 'disabled');
                })
            },
            close: function () {
                _table.dlgAddLicensee.html(originalDialog);
            }
        });

        _table.dlgAddLicensee.dialog('option', 'title', 'Add Licensee to Party');
        _table.dlgAddLicensee.dialog('open');
    }

    var FillDataToEditForm = function (rowNum) {
        var row = gridOptions.bodyRows[rowNum].Columns;
        _dlgAddNew.hidPartyID.val(row[_mappingColumns.PartyID].Value);
        _dlgAddNew.txtPartyID.val(row[_mappingColumns.PartyID].Value);
        _dlgAddNew.txtPartyName.val(row[_mappingColumns.PartyName].Value);
        _dlgAddNew.selPartyType.val(row[_mappingColumns.PartyType].Value);
        _dlgAddNew.txtDescription.val(row[_mappingColumns.Description].Value);
        _dlgAddNew.txtPartyFullName.val(row[_mappingColumns.PartyFullName].Value);
        _dlgAddNew.txtAccountNumber.val(row[_mappingColumns.AccountNumber].Value);
        _dlgAddNew.txtAccountName.val(row[_mappingColumns.AccountName].Value);
        _dlgAddNew.txtBankName.val(row[_mappingColumns.BankName].Value);
        _dlgAddNew.txtBenificiary.val(row[_mappingColumns.Benificiary].Value);
        _dlgAddNew.txtIBAN.val(row[_mappingColumns.IBAN].Value);
        _dlgAddNew.txtSwiftCode.val(row[_mappingColumns.SwiftCode].Value);
        _dlgAddNew.txtCorrespondentBank.val(row[_mappingColumns.CorrespondentBank].Value);
        _dlgAddNew.txtAttnName.val(row[_mappingColumns.AttnName].Value);
        _dlgAddNew.txtTo1.val(row[_mappingColumns.To1].Value);
        _dlgAddNew.txtTo2.val(row[_mappingColumns.To2].Value);
    }

    var ValidateForm = function () {
        var result = true;
        _dlgAddNew.txtValidateTips.html("");
        if (_dlgAddNew.txtPartyName.val().length <= 0) {
            _dlgAddNew.txtPartyName.addClass("input-validation-error");
            _dlgAddNew.txtValidateTips.html(_dlgAddNew.txtValidateTips.html() + "Party Name must be enter.");
            result = false;
        }

        return result;
    }
}

function moveOptions(theSelFrom, theSelTo) {
    var selLength = theSelFrom.length;
    var selectedText = new Array();
    var selectedValues = new Array();
    var selectedCount = 0;
    var i;
    for (i = selLength - 1; i >= 0; i--) {
        if (theSelFrom.options[i].selected) {
            selectedText[selectedCount] = theSelFrom.options[i].text;
            selectedValues[selectedCount] = theSelFrom.options[i].value;
            deleteOption(theSelFrom, i);
            selectedCount++;
        }
    }
    for (i = selectedCount - 1; i >= 0; i--) {
        addOption(theSelTo, selectedText[i], selectedValues[i]);
    }
}

function addOption(theSel, theText, theValue) {
    var newOpt = new Option(theText, theValue);
    var selLength = theSel.length;
    theSel.options[selLength] = newOpt;
}

function deleteOption(theSel, theIndex) {
    var selLength = theSel.length;
    if (selLength > 0) {
        theSel.options[theIndex] = null;
    }
} 