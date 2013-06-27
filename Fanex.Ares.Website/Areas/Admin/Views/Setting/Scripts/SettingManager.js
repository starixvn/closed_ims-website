/// <reference path="../../../../../Scripts/NapAres.js" />

var settingManager;
$(document).ready(function () {
    domain = location.protocol + '//' + location.host + $('#NapHubRoot').val();
    settingManager = new SettingManager();
    settingManager.initPage();

});

SettingManager = function () {
    var self = this;
    var _table = {
        selLicenseeID: null,
        selPartyID: null,
        btnAddNew: null,
        btnSearch: null,
        settingsContainer: null,
        dlgAddNew: null,
        dlgLicenseeProductCharge: null,
        dlgLicenseeProductDiscount: null,
        formSearch: null,
        frmPartyLicensee: null,
        errCode: null,
        errMsg: null
    };

    var _mappingColumns = {
        PartyID: 0,
        LicenseeID: 1,
        ProductID: 2,
        PartyName: 3,
        LicenseeName: 4,
        ProductName: 5,
        FirstBillingDate: 6,
        HeadOfTerms: 7,
        MinimumFee: 8,
        WaivedDate: 9,
        WaivedMonth: 10,
        Currency: 11,
        MinimumFeeTerm: 12
    }

    var _dlgAddNew = {
        selLicenseeID: null,
        selProductID: null,
        selPartyID: null,
        dateFirstBillingDate: null,
        dateHeadOfTerms: null,
        intMinimumFee: null,
        dateWaivedDate: null,
        dateWaivedMonth: null,
        intCurrency: null,
        txtMinimumFeeTerm: null,
    };

    var gridOptions = {};

    var cachedDOMElements = function () {
        _table.btnAddNew = $('#btnAddNew');
        _table.btnSearch = $('#btnSearch');
        _table.settingsContainer = $('#SettingsContainer');
        _table.dlgAddNew = $('#AddNewDialog');
        _table.dlgLicenseeProductCharge = $('#LicenseeProductChargeDialog');
        _table.dlgLicenseeProductDiscount = $('#LicenseeProductDiscountDialog');

        _table.formSearch = $('#formSearch');
        _table.formAddNew = $('#formAddNew');
        _table.selLicenseeID = $('#LicenseeID');
        _table.selPartyID = $('#PartyID');
        _table.frmPartyLicensee = $('#formPartyLicensee');
        _table.errCode = $('#ErrCode');
        _table.errMsg = $('#ErrMsg');

        _dlgAddNew.selLicenseeID = $('#formAddNew #LicenseeID');
        _dlgAddNew.selProductID = $('#formAddNew #ProductID');
        _dlgAddNew.selPartyID = $('#formAddNew #PartyID');
        _dlgAddNew.dateFirstBillingDate = $('#FirstBillingDate');
        _dlgAddNew.dateHeadOfTerms = $('#HeadOfTerms');
        _dlgAddNew.intMinimumFee = $('#MinimumFee');
        _dlgAddNew.txtMinimumFeeTerm = $('#MinimumFeeTerm');
        _dlgAddNew.dateWaivedDate = $('#WaivedDate');
        _dlgAddNew.dateWaivedMonth = $('#WaivedMonth');
        _dlgAddNew.intCurrency = $('#Currency');
        _dlgAddNew.txtValidateTips = $('#validateTips');
    }

    var bindingEvent = function () {
        _table.btnAddNew.click(function () {
            self.AddNewLicensee();
        });

        _table.btnSearch.click(function () {
            GenerateData();
        });

        _dlgAddNew.selPartyID.change(function () {
            self.GetLicenseesByPartyID($(this).val(), _dlgAddNew.selLicenseeID, false, function () { _dlgAddNew.selLicenseeID.change(); });

        });

        _dlgAddNew.selLicenseeID.change(function () {
            self.GetProductsByLicenseePartyID($(this).val(), _dlgAddNew.selPartyID.val(), _dlgAddNew.selProductID, false);
        });
    }

    self.GetLicenseesByPartyID = function (partyID, targetElement, hasAllRow, func) {
        $.ajax({
            url: domain + "Admin/Setting/GetLicenseesByPartyID",
            type: "POST",
            data: "&partyID=" + partyID,
            success: function (data, textStatus, jqXHR) {
                var html = "";
                if (hasAllRow) {
                    html = "<option value='0' selected='selected'>All</option>";
                }

                for (var i = 0; i < data.length; i++) {
                    html += "<option value='" + data[i].Value + "'>" + data[i].Text + "</option>";
                }

                targetElement.html(html);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert("fail");
            },
            complete: function (jqXHR, textStatus) {
                if ($.isFunction(func)) {
                    func();
                }
            }
        });
    }

    self.GetProductsByLicenseePartyID = function (licenseeID, partyID, targetElement, hasAllRow) {
        $.ajax({
            url: domain + "Admin/Setting/GetProductsByLicenseeIDPartyID",
            type: "POST",
            data: "&partyID=" + partyID + "&licenseeID=" + licenseeID,
            success: function (data, textStatus, jqXHR) {
                var html = "";
                if (hasAllRow) {
                    html = "<option value='0' selected='selected'>All</option>";
                }

                for (var i = 0; i < data.length; i++) {
                    html += "<option value='" + data[i].Value + "'>" + data[i].Text + "</option>";
                }

                targetElement.html(html);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert("fail");
            },
            complete: function (jqXHR, textStatus) {
            }
        });
    }

    self.initPage = function () {
        $('.datepicker').each(function () {
            $(this).datepicker();

        });
        cachedDOMElements();
        _table.selLicenseeID.val(0);
        _table.selPartyID.val(0);
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
            url: domain + "Admin/Setting/GetAllSettings",
            dataType: "json",
            data: _table.formSearch.serialize(),
            beforeSend: function () {
                NapAres.disableInput();
            },
            success: function (rs) {
                renderSettings(rs);
            },
            error: function (event, jqXHR, ajaxSettings) {
                renderSettings([]);
            },
            complete: function () {
                NapAres.enableInput();
            }
        });

    };

    var renderSettings = function (data) {
        var bodyData = [];
        if (data && data.length > 0) {
            var dataLength = data.length;
            for (var i = 0; i < dataLength; i++) {
                var item = data[i];
                var currentRow = {
                    Columns: [
                                { Name: "PartyID", Value: item.PartyID },
                                { Name: "LicenseeID", Value: item.LicenseeID },
                                { Name: "ProductID", Value: item.ProductID },
                                { Name: "PartyName", Value: item.PartyName },
                                { Name: "LicenseeName", Value: item.LicenseeName },
                                { Name: "ProductName", Value: item.ProductName },
                                { Name: "FirstBillingDate", Value: item.FirstBillingDate },
                                { Name: "HeadOfTerms", Value: item.HeadOfTerms },
                                { Name: "MinimumFee", Value: item.MinimumFee },
                                { Name: "WaivedDate", Value: item.WaivedDate },
                                { Name: "WaivedMonth", Value: item.WaivedMonth },
                                { Name: "Currency", Value: item.Currency },
                                { Name: "MinimumFeeTerm", Value: item.MinimumFeeTerm },
                                { Name: "Edit", Value: i, CellAlign: "center", CustomCellRender: function (cell, cellFormated) { return GenerateEditButton(cell, cellFormated); } },
                                { Name: "LicenseeProductCharge", Value: i, CellAlign: "center", CustomCellRender: function (cell, cellFormated) { return GenerateLicenseeProductChargeButton(cell, cellFormated); } },
                                { Name: "LicenseeProductDiscount", Value: i, CellAlign: "center", CustomCellRender: function (cell, cellFormated) { return GenerateLicenseeProductDiscountButton(cell, cellFormated); } }
                            ]
                };

                bodyData.push(currentRow);
            }
        }

        var GenerateEditButton = function (cell, cellFormated) {
            return "<input value='Edit' type='button' onclick='settingManager.EditSetting(" + cell.Value + ")' />";
        }

        var GenerateLicenseeProductChargeButton = function (cell, cellFormated) {
            return "<input value='Charge' type='button' onclick='settingManager.LicenseeProductCharge(" + cell.Value + ")' />";
        }

        var GenerateLicenseeProductDiscountButton = function (cell, cellFormated) {
            return "<input value='Discount' type='button' onclick='settingManager.LicenseeProductDiscount(" + cell.Value + ")' />";
        }

        gridOptions = {
            columns: [
                    { HeaderText: "PartyID", Width: 100, Name: "PartyID", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "LicenseeID", Width: 100, Name: "LicenseeID", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "ProductID", Width: 100, Name: "ProductID", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Party", Width: 100, Name: "PartyName", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Licensee", Width: 100, Name: "LicenseeName", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Product", Width: 100, Name: "ProductName", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "First Billing Date", Width: 100, Name: "FirstBillingDate", HeaderAlign: "center", CellAlign: "center", FormatDateTime: "DD MMM YYYY" },
                    { HeaderText: "Head Of Terms", Width: 100, Name: "HeadOfTerms", HeaderAlign: "center", CellAlign: "center", FormatDateTime: "DD MMM YYYY" },
                    { HeaderText: "Minimum Fee", Width: 100, Name: "MinimumFee", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Waived Date", Width: 100, Name: "WaivedDate", HeaderAlign: "center", CellAlign: "center", FormatDateTime: "DD MMM YYYY" },
                    { HeaderText: "Waived Month", Width: 100, Name: "WaivedMonth", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Currency", Width: 60, Name: "Currency", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "MinimumFeeTerm", Width: 60, Name: "MinimumFeeTerm", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Edit", Width: 100, Name: "Edit", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Charge", Width: 100, Name: "LicenseeProductCharge", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Discount", Width: 100, Name: "LicenseeProductDiscount", HeaderAlign: "center", CellAlign: "left"}],

            showTitle: true,
            //showFooter: true,
            titleInfo:
            {
                text: "Setting Manager",
                showExcelButton: false,
                showCollapseExpandButton: true
            },
            showTree: true,
            bodyRows: bodyData,
            showDistinctRow: false,
            //footerRows: null,
            gridExpandHeight: 200
        };

        _table.settingsContainer.fanexGrid(gridOptions);
        _table.settingsContainer.fanexGrid("hideColumns", gridOptions, [_mappingColumns.PartyID,
                                                                        _mappingColumns.LicenseeID,
                                                                        _mappingColumns.ProductID,
                                                                        _mappingColumns.MinimumFeeTerm]);
    };

    self.AddNewLicensee = function () {
        var originalDialog = _table.dlgAddNew.html();
        cachedDOMElements();
        _table.dlgAddNew.find('.update').each(function () { $(this).show() });

        _table.dlgAddNew.dialog({
            autoOpen: false,
            height: 600,
            width: 500,
            modal: true,
            buttons: {
                "Create a Setting": function () {
                    if (ValidateForm()) {
                        _dlgAddNew.txtValidateTips.hide();
                        _table.formAddNew.attr("Action", domain + "admin/Setting/CreateNewSetting");
                        _table.dlgAddNew.find('.update').each(function () { $(this).show() });
                        _table.formAddNew.submit();

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

                $('#formAddNew #PartyID').change(function () {
                    self.GetLicenseesByPartyID($(this).val(),
                                        $('#formAddNew #LicenseeID'),
                                        false,
                                        function () {
                                        }
                    )
                });

                self.GetLicenseesByPartyID($('#formAddNew #PartyID').val(),
                                        $('#formAddNew #LicenseeID'),
                                        false,
                                        function () {
                                        }
                    )
                
                $('#FirstBillingDate').datepicker({ dateFormat: 'd MM yy' });
                $('#HeadOfTerms').datepicker({ dateFormat: 'd MM yy' });
                $('#WaivedDate').datepicker({ dateFormat: 'd MM yy' });
            },
            close: function () {
                _table.dlgAddNew.html(originalDialog);
            }
        });

        _table.dlgAddNew.dialog('option', 'title', 'Add new Setting');
        _table.dlgAddNew.dialog('open');
    };

    self.EditSetting = function (rowNum) {
        _table.dlgAddNew.attr('title', 'Edit Setting');
        var originalDialog = _table.dlgAddNew.html();
        cachedDOMElements();
        _table.dlgAddNew.find('.update').hide();
        FillDataToEditForm(rowNum);
        _table.dlgAddNew.dialog({
            autoOpen: false,
            height: 500,
            width: 500,
            modal: true,
            buttons: {
                "Edit Setting": function () {
                    if (ValidateForm()) {
                        _dlgAddNew.txtValidateTips.hide();
                        _table.formAddNew.attr("Action", domain + "admin/Setting/UpdateSetting");
                        _table.dlgAddNew.find('.update').each(function () { $(this).show() });
                        _table.formAddNew.submit();

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
                $('#FirstBillingDate').datepicker({ dateFormat: 'd MM yy' });
                $('#HeadOfTerms').datepicker({ dateFormat: 'd MM yy' });
                $('#WaivedDate').datepicker({ dateFormat: 'd MM yy' });

            },
            close: function () {
                _table.dlgAddNew.html(originalDialog);
            }
        });

        _table.dlgAddNew.dialog('option', 'title', 'Edit Setting');
        _table.dlgAddNew.dialog('open');
    };

    self.LicenseeProductCharge = function (rowNum) {
        var originalDialog = _table.dlgLicenseeProductCharge.html();
        var row = gridOptions.bodyRows[rowNum].Columns;
        var LicenseeID = row[_mappingColumns.LicenseeID].Value;
        var PartyID = row[_mappingColumns.PartyID].Value;
        var ProductID = row[_mappingColumns.ProductID].Value;
        $('#formLicenseeProductCharge > #LicenseeID').val(LicenseeID);
        $('#formLicenseeProductCharge > #ProductID').val(ProductID);
        $('#formLicenseeProductCharge > #PartyID').val(PartyID);
        _table.dlgLicenseeProductCharge.dialog({
            autoOpen: false,
            height: 600,
            width: 1000,
            modal: true,
            buttons: {
                "Save": function () {
                    $('#formLicenseeProductCharge > #XmlSettings').val(tableToXML('formLicenseeProductCharge'));
                    $('#formLicenseeProductCharge').submit();
                    $(this).dialog("close");
                },
                Cancel: function () {
                    $(this).dialog("close");
                }
            },
            open: function () {
                $.ajax({
                    type: "POST",
                    url: domain + "Admin/Setting/GetAllLicenseeProductCharge",
                    dataType: "html",
                    data: "LicenseeID=" + LicenseeID + "&PartyID=" + PartyID + "&ProductID=" + ProductID,
                    beforeSend: function () {
                        NapAres.disableInput();
                    },
                    success: function (rs) {
                        var data = eval(rs);
                        if (data.length > 0) {
                            var $row = $('#formLicenseeProductCharge>table>tbody>tr').eq(0);
                            // FillData
                            for (var i = 0; i < data.length; i++) {
                                if (i == 0) {
                                    $('#DistributorRate', $row).val(data[i].DistributorRate * 100);
                                    $('#PartnerRate', $row).val(data[i].PartnerRate * 100);
                                    $('#BillingCurrency', $row).val(data[i].BillingCurrency);
                                    $('#ChargeType', $row).val(data[i].ChargeType);
                                    $('#EndDate', $row).val(moment(data[i].EndDate).format('DD MMM YYYY'));
                                    $('#ONEWorksRate', $row).val(data[i].ONEWorksRate * 100);
                                    $('#StartDate', $row).val(moment(data[i].StartDate).format('DD MMM YYYY'));
                                    $('#Description', $row).val(data[i].Description);
                                } else {
                                    var $newRow = addNewRow('formLicenseeProductCharge');
                                    $('#DistributorRate', $newRow).val(data[i].DistributorRate * 100);
                                    $('#PartnerRate', $newRow).val(data[i].PartnerRate * 100);
                                    $('#BillingCurrency', $newRow).val(data[i].BillingCurrency);
                                    $('#ChargeType', $newRow).val(data[i].ChargeType);
                                    $('#EndDate' + (i + 1), $newRow).val(moment(data[i].EndDate).format('DD MMM YYYY'));
                                    $('#ONEWorksRate', $newRow).val(data[i].ONEWorksRate * 100);
                                    $('#StartDate' + (i + 1), $newRow).val(moment(data[i].StartDate).format('DD MMM YYYY'));
                                    $('#Description', $newRow).val(data[i].Description);
                                }
                            }


                        }
                    },
                    error: function (event, jqXHR, ajaxSettings) {
                        alert('fail');
                    },
                    complete: function () {
                        $('.date').each(function () { $(this).datepicker({ dateFormat: 'd M yy' }) });
                        NapAres.enableInput();

                    }
                });

                $('#selected_columns option').each(function () {
                    $(this).attr('disabled', 'disabled');
                })
            },
            close: function () {
                _table.dlgLicenseeProductCharge.html(originalDialog);
            }
        });

        _table.dlgLicenseeProductCharge.dialog('option', 'title', 'Licensee Product Charge');
        _table.dlgLicenseeProductCharge.dialog('open');
    }

    self.LicenseeProductDiscount = function (rowNum) {
        var originalDialog = _table.dlgLicenseeProductDiscount.html();
        var row = gridOptions.bodyRows[rowNum].Columns;
        var LicenseeID = row[_mappingColumns.LicenseeID].Value;
        var PartyID = row[_mappingColumns.PartyID].Value;
        var ProductID = row[_mappingColumns.ProductID].Value;
        $('#formLicenseeProductDiscount > #LicenseeID').val(LicenseeID);
        $('#formLicenseeProductDiscount > #ProductID').val(ProductID);
        $('#formLicenseeProductDiscount > #PartyID').val(PartyID);
        _table.dlgLicenseeProductDiscount.dialog({
            autoOpen: false,
            height: 600,
            width: 1000,
            modal: true,
            buttons: {
                "Save": function () {
                    $('#formLicenseeProductDiscount > #XmlSettings').val(tableToXML('formLicenseeProductDiscount'));
                    $('#formLicenseeProductDiscount').submit();
                    $(this).dialog("close");
                },
                Cancel: function () {
                    $(this).dialog("close");
                }
            },
            open: function () {
                $.ajax({
                    type: "POST",
                    url: domain + "Admin/Setting/GetAllLicenseeProductDiscount",
                    dataType: "html",
                    data: "LicenseeID=" + LicenseeID + "&PartyID=" + PartyID + "&ProductID=" + ProductID,
                    beforeSend: function () {
                        NapAres.disableInput();
                    },
                    success: function (rs) {
                        var data = eval(rs);
                        if (data.length > 0) {
                            var $row = $('#formLicenseeProductDiscount>table>tbody>tr').eq(0);
                            // FillData
                            for (var i = 0; i < data.length; i++) {
                                if (i == 0) {
                                    $('#Currency', $row).val(data[i].Currency);
                                    $('#FromTotalAmount', $row).val(data[i].FromTotalAmount);
                                    $('#ToTotalAmount', $row).val(data[i].ToTotalAmount);
                                    $('#DiscountRate', $row).val(data[i].DiscountRate * 100);
                                } else {
                                    var $newRow = addNewRow('formLicenseeProductDiscount');
                                    $('#Currency', $newRow).val(data[i].Currency);
                                    $('#FromTotalAmount', $newRow).val(data[i].FromTotalAmount);
                                    $('#ToTotalAmount', $newRow).val(data[i].ToTotalAmount);
                                    $('#DiscountRate', $newRow).val(data[i].DiscountRate * 100);
                                }
                            }
                        }
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
                _table.dlgLicenseeProductDiscount.html(originalDialog);
            }
        });

        _table.dlgLicenseeProductDiscount.dialog('option', 'title', 'Licensee Product Discount');
        _table.dlgLicenseeProductDiscount.dialog('open');
    }

    var FillDataToEditForm = function (rowNum) {
        var row = gridOptions.bodyRows[rowNum].Columns;        
        _dlgAddNew.selLicenseeID.val(row[_mappingColumns.LicenseeID].Value);
        _dlgAddNew.selProductID.val(row[_mappingColumns.ProductID].Value);
        _dlgAddNew.selPartyID.val(row[_mappingColumns.PartyID].Value);
        _dlgAddNew.dateFirstBillingDate.val(moment(row[_mappingColumns.FirstBillingDate].Value).format('DD MMM YYYY'));
        _dlgAddNew.dateHeadOfTerms.val(moment(row[_mappingColumns.HeadOfTerms].Value).format('DD MMM YYYY'));
        _dlgAddNew.intMinimumFee.val(row[_mappingColumns.MinimumFee].Value);
        _dlgAddNew.dateWaivedDate.val(moment(row[_mappingColumns.WaivedDate].Value).format('DD MMM YYYY'));
        _dlgAddNew.dateWaivedMonth.val(row[_mappingColumns.WaivedMonth].Value);
        _dlgAddNew.intCurrency.val(row[_mappingColumns.Currency].Value);
        _dlgAddNew.txtMinimumFeeTerm.val(row[_mappingColumns.MinimumFeeTerm].Value);

    }

    var ValidateForm = function () {
        var result = true;
        _dlgAddNew.txtValidateTips.html("");
        //        if (_dlgAddNew.txtLicenseeName.val().length <= 0) {
        //            _dlgAddNew.txtLicenseeName.addClass("input-validation-error");
        //            _dlgAddNew.txtValidateTips.html(_dlgAddNew.txtValidateTips.html() + "Setting Name must be enter.");
        //            result = false;
        //        }

        return result;
    }
}

var addNewRow = function (formID) {
    var $lastRow = $('#' + formID + '>table>tbody>tr:last');

    if (!isBlankRow($lastRow)) {
        var $cloneRow = $lastRow.clone();
        var rowNum = parseInt($cloneRow.find('td.No').html()) + 1
        $cloneRow.find(":text").val('');
        $cloneRow.find('td.No').html(rowNum);
        $cloneRow.find('input[name="StartDate"]:eq(0)').attr('id', 'StartDate' + rowNum.toString());
        $cloneRow.find('input[name="EndDate"]:eq(0)').attr('id', 'EndDate' + rowNum.toString());
        $lastRow.after($cloneRow);
        $('.date').removeClass('hasDatepicker').datepicker({ dateFormat: 'd M yy' });
    }

    return $cloneRow;
}

var deleteRow = function (e, element, formID) {
    if (e.keyCode == 46) {
        if (confirm('Do you want to delete this row?')) {
            if ($('#' + formID + '>table>tbody>tr').length > 1) {
                $(element).remove();
            }
            else {
                $(element).find(':text').val('');
            }
        }
    }
}

function isBlankRow(row) {
    var result = true;
    row.find(':text').each(function () {
        value = $(this).val();
        if (result && value != undefined && value != "") {
            result = false;
            return false;
        }
    });

    return result;
}

var tableToXML = function (formID) {
    var $rowCollection = $('#' + formID + '>table>tbody>tr');
    var xmlData = "<Settings>";
    $('#' + formID + '>table>tbody>tr').each(function () {
        if (!isBlankRow($(this))) {
            xmlData += "<Setting>";
            var licenseeID = $('#' + formID + '>#LicenseeID').val();
            var productID = $('#' + formID + '>#ProductID').val();
            var partyID = $('#' + formID + '>#PartyID').val();
            xmlData += "<LicenseeID>";
            xmlData += licenseeID.toString();
            xmlData += "</LicenseeID>";
            xmlData += "<ProductID>";
            xmlData += productID.toString();
            xmlData += "</ProductID>";
            xmlData += "<PartyID>";
            xmlData += partyID.toString();
            xmlData += "</PartyID>";
            $(this).find('td').each(function () {
                $(this).find('input, select, textarea').each(function () {
                    var name = $(this).attr('name');
                    var value = $(this).val();
                    xmlData += "<" + name + ">";
                    if (name.toLowerCase().indexOf('date') != -1) {
                        xmlData += moment(value).format('MM/DD/YYYY');
                    } else if (name.toLowerCase().indexOf('rate') != -1) {
                        xmlData += (value / 100).toString();
                    } else {
                        xmlData += value.toString();
                    }
                    xmlData += "</" + name + ">";

                });
            });

            xmlData += "</Setting>";
        }
    });

    xmlData += "</Settings>";

    return xmlData;
}