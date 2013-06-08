var licenseeManager;
$(document).ready(function () {
    licenseeManager = new LicenseeManager();
    licenseeManager.initPage();
});

LicenseeManager = function () {
    var self = this;
    var _table = {
        btnAddNew: null,
        licenseesContainer: null,
        dlgAddNew: null,
        dlgAddParty: null,
        frmLicensee: null,
        frmPartyLicensee: null,
        errCode: null,
        errMsg: null
    };

    var _mappingColumns = {
        LicenseeID: 0,
        LicenseeName: 1,
        BrandName: 2,
        CompanyName: 3,
        IsDeposit: 4,
        IsActive: 5,
        IsOneWorkActive: 6,
        AttnName: 7,
        To1: 8,
        To2: 9
    }

    var _dlgAddNew = {
        hidLicenseeID: null,
        txtLicenseeID: null,
        txtLicenseeName: null,
        txtBrandName: null,
        txtCompanyName: null,
        chkIsDeposit: null,
        chkIsActive: null,
        chkIsOneWorkActive: null,
        txtAttnName: null,
        txtTo1: null,
        txtTo2: null
    };
    var gridOptions = {};

    var cachedDOMElements = function () {
        _table.btnAddNew = $('#btnAddNew');
        _table.licenseesContainer = $('#LicenseesContainer');
        _table.dlgAddNew = $('#AddNewDialog');
        _table.dlgAddParty = $('#PartyLicenseeDialog');
        _table.frmLicensee = $('#formLicensee');
        _table.frmPartyLicensee = $('#formPartyLicensee');
        _table.errCode = $('#ErrCode');
        _table.errMsg = $('#ErrMsg');

        _dlgAddNew.hidLicenseeID = $('#LicenseeID');
        _dlgAddNew.txtLicenseeID = $('#txtLicenseeID');
        _dlgAddNew.txtLicenseeName = $('#LicenseeName');
        _dlgAddNew.txtBrandName = $('#BrandName');
        _dlgAddNew.txtCompanyName = $('#CompanyName');
        _dlgAddNew.chkIsDeposit = $('#IsDeposit');
        _dlgAddNew.chkIsActive = $('#IsActive');
        _dlgAddNew.chkIsOneWorkActive = $('#IsOneWorkActive');
        _dlgAddNew.txtAttnName = $('#AttnName');
        _dlgAddNew.txtTo1 = $('#To1');
        _dlgAddNew.txtTo2 = $('#To2');
        _dlgAddNew.txtValidateTips = $('#validateTips');
    }

    var bindingEvent = function () {
        _table.btnAddNew.click(function () {
            self.AddNewLicensee();
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
            url: window.rootUrl + "Admin/Licensee/GetAllLicensees",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: null,
            beforeSend: function () {
                NapAres.disableInput();
            },
            success: function (rs) {
                renderLicensees(rs);
            },
            error: function (event, jqXHR, ajaxSettings) {
                renderLicensees([]);
            },
            complete: function () {
                NapAres.enableInput();
            }
        });

    };

    var renderLicensees = function (data) {
        var bodyData = [];
        if (data && data.length > 0) {
            var dataLength = data.length;
            for (var i = 0; i < dataLength; i++) {
                var item = data[i];
                var currentRow = {
                    Columns: [
                                { Name: "LicenseeID", Value: item.LicenseeID },
                                { Name: "LicenseeName", Value: item.LicenseeName },
                                { Name: "BrandName", Value: item.BrandName },
                                { Name: "CompanyName", Value: item.CompanyName },
                                { Name: "IsDeposit", Value: item.IsDeposit },
                                { Name: "IsActive", Value: item.IsActive },
                                { Name: "IsOneWorkActive", Value: item.IsOneWorkActive },
                                { Name: "AttnName", Value: item.AttnName },
                                { Name: "To1", Value: item.To1 },
                                { Name: "To2", Value: item.To2 },
                                { Name: "Edit", Value: i, CellAlign: "center", CustomCellRender: function (cell, cellFormated) { return GenerateEditButton(cell, cellFormated); } },
                                { Name: "AddParty", Value: i, CellAlign: "center", CustomCellRender: function (cell, cellFormated) { return GenerateAddLicenseeButton(cell, cellFormated); } }
                            ]
                };

                bodyData.push(currentRow);
            }
        }

        var GenerateEditButton = function (cell, cellFormated) {
            return "<input value='Edit' type='button' onclick='licenseeManager.EditLicensee(" + cell.Value + ")' />";
        }

        var GenerateAddLicenseeButton = function (cell, cellFormated) {
            return "<input value='Add Party' type='button' onclick='licenseeManager.AddParty(" + cell.Value + ")' />";
        }

        gridOptions = {
            columns: [
                    { HeaderText: "LicenseeID", Width: 100, Name: "LicenseeID", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Licensee Name", Width: 100, Name: "LicenseeName", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Brand Name", Width: 100, Name: "BrandName", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Company Name", Width: 200, Name: "CompanyName", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Is Label", Width: 100, Name: "IsDeposit", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Is Active", Width: 100, Name: "IsActive", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Is One Work Active", Width: 100, Name: "IsOneWorkActive", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Attn Name", Width: 100, Name: "AttnName", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "To1", Width: 100, Name: "To1", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "To2", Width: 100, Name: "To2", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Edit", Width: 100, Name: "Edit", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Add Party", Width: 100, Name: "AddParty", HeaderAlign: "center", CellAlign: "left"}],
            showTitle: true,
            //showFooter: true,
            titleInfo:
            {
                text: "Licensee Manager",
                showExcelButton: false,
                showCollapseExpandButton: true
            },
            showTree: true,
            bodyRows: bodyData,
            showDistinctRow: false,
            //footerRows: null,
            gridExpandHeight: 200
        };

        _table.licenseesContainer.fanexGrid(gridOptions);
//        _table.licenseesContainer.fanexGrid("hideColumns", gridOptions, [_mappingColumns.PartyID,
//                                                                        _mappingColumns.PartyType,
//                                                                        _mappingColumns.Description,
//                                                                        _mappingColumns.AccountNumber,
//                                                                        _mappingColumns.AccountName,
//                                                                        _mappingColumns.BankName,
//                                                                        _mappingColumns.Benificiary,
//                                                                        _mappingColumns.IBAN,
//                                                                        _mappingColumns.SwiftCode,
//                                                                        _mappingColumns.CorrespondentBank]);
    };

    self.AddNewLicensee = function () {
        var originalDialog = _table.dlgAddNew.html();
        cachedDOMElements();
        _table.dlgAddNew.find('.update').each(function () { $(this).hide() });
        _table.dlgAddNew.dialog({
            autoOpen: false,
            height: 600,
            width: 500,
            modal: true,
            buttons: {
                "Create a Licensee": function () {
                    if (ValidateForm()) {
                        _dlgAddNew.txtValidateTips.hide();
                        _table.frmLicensee.submit();
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

        _table.dlgAddNew.dialog('option', 'title', 'Add new Licensee');
        _table.dlgAddNew.dialog('open');
    };

    self.EditLicensee = function (rowNum) {
        _table.dlgAddNew.attr('title', 'Edit Licensee');
        var originalDialog = _table.dlgAddNew.html();
        cachedDOMElements();
        FillDataToEditForm(rowNum);
        _table.dlgAddNew.dialog({
            autoOpen: false,
            height: 600,
            width: 500,
            modal: true,
            buttons: {
                "Edit Licensee": function () {
                    if (ValidateForm()) {
                        _dlgAddNew.txtValidateTips.hide();
                        _table.frmLicensee.attr("Action", "/admin/Licensee/UpdateLicensee");
                        _table.frmLicensee.submit();
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

        _table.dlgAddNew.dialog('option', 'title', 'Edit Licensee');
        _table.dlgAddNew.dialog('open');
    };

    self.AddParty = function (rowNum) {
        var originalDialog = _table.dlgAddParty.html();
        var row = gridOptions.bodyRows[rowNum].Columns;
        var LicenseeID = row[_mappingColumns.LicenseeID].Value;
        $('#formPartyLicensee > #LicenseeID').val(LicenseeID);
        _table.dlgAddParty.dialog({
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
                        xmlData += $(this).attr('value');
                        xmlData += "</Party>";
                        xmlData += "<Licensee>";
                        xmlData += LicenseeID.toString();
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
                    url: window.rootUrl + "Admin/Licensee/_PartyLicensee",
                    dataType: "html",
                    data: "LicenseeID=" + LicenseeID,
                    beforeSend: function () {
                        NapAres.disableInput();
                    },
                    success: function (rs) {
                        $('#PartyLicenseeDialog').html(rs);
                    },
                    error: function (event, jqXHR, ajaxSettings) {
                        alert('fail');
                    },
                    complete: function () {
                        NapAres.enableInput();
                    }
                });

                $('#selected_columns option').each(function () {
                    $(this).attr('disabled', 'disabled');
                })
            },
            close: function () {
                _table.dlgAddParty.html(originalDialog);
            }
        });

        _table.dlgAddParty.dialog('option', 'title', 'Add Party to Licensee');
        _table.dlgAddParty.dialog('open');
    }

    var FillDataToEditForm = function (rowNum) {
        var row = gridOptions.bodyRows[rowNum].Columns;
        _dlgAddNew.hidLicenseeID.val(row[_mappingColumns.LicenseeID].Value);
        _dlgAddNew.txtLicenseeID.val(row[_mappingColumns.LicenseeID].Value);
        _dlgAddNew.txtLicenseeName.val(row[_mappingColumns.LicenseeName].Value);
        _dlgAddNew.txtBrandName.val(row[_mappingColumns.BrandName].Value);
        _dlgAddNew.txtCompanyName.val(row[_mappingColumns.CompanyName].Value);
        _dlgAddNew.chkIsDeposit.attr('checked', row[_mappingColumns.IsDeposit].Value);
        _dlgAddNew.chkIsActive.attr('checked', row[_mappingColumns.IsActive].Value);
        _dlgAddNew.chkIsOneWorkActive.attr('checked', row[_mappingColumns.IsOneWorkActive].Value);
        _dlgAddNew.txtAttnName.val(row[_mappingColumns.AttnName].Value);
        _dlgAddNew.txtTo1.val(row[_mappingColumns.To1].Value);
        _dlgAddNew.txtTo2.val(row[_mappingColumns.To2].Value);
    }

    var ValidateForm = function () {
        var result = true;
        _dlgAddNew.txtValidateTips.html("");
        if (_dlgAddNew.txtLicenseeName.val().length <= 0) {
            _dlgAddNew.txtLicenseeName.addClass("input-validation-error");
            _dlgAddNew.txtValidateTips.html(_dlgAddNew.txtValidateTips.html() + "Licensee Name must be enter.");
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