var productManager;
$(document).ready(function () {
    domain = location.protocol + '//' + location.host + $('#NapHubRoot').val();
    productManager = new ProductManager();
    productManager.initPage();
});

ProductManager = function () {
    var self = this;
    var _table = {
        btnAddNew: null,
        productsContainer: null,
        dlgAddNew: null,
        dlgAddParty: null,
        frmProduct: null,
        frmPartyLicensee: null,
        errCode: null,
        errMsg: null
    };

    var _mappingColumns = {
        ProductID: 0,
        ProductName: 1,
        Description: 2,
        ProductPrefix: 3
    }

    var _dlgAddNew = {
        hidProductID: null,
        txtProductID: null,
        txtProductName: null,
        txtDescription: null,
        txtProductPrefix: null
    };
    var gridOptions = {};

    var cachedDOMElements = function () {
        _table.btnAddNew = $('#btnAddNew');
        _table.productsContainer = $('#ProductsContainer');
        _table.dlgAddNew = $('#AddNewDialog');
        _table.frmProduct = $('#formProduct');
        _table.errCode = $('#ErrCode');
        _table.errMsg = $('#ErrMsg');

        _dlgAddNew.hidProductID = $('#ProductID');
        _dlgAddNew.txtProductID = $('#txtProductID');
        _dlgAddNew.txtProductName = $('#ProductName');
        _dlgAddNew.txtDescription = $('#Description');
        _dlgAddNew.txtProductPrefix = $('#ProductPrefix');

        _dlgAddNew.txtValidateTips = $('#validateTips');
    }

    var bindingEvent = function () {
        _table.btnAddNew.click(function () {
            self.AddNewProduct();
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
            url: domain +"Admin/Product/GetAllProducts",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: null,
            beforeSend: function () {
                NapAres.disableInput();
            },
            success: function (rs) {
                renderProducts(rs);
            },
            error: function (event, jqXHR, ajaxSettings) {
                renderProducts([]);
            },
            complete: function () {
                NapAres.enableInput();
            }
        });

    };

    var renderProducts = function (data) {
        var bodyData = [];
        if (data && data.length > 0) {
            var dataLength = data.length;
            for (var i = 0; i < dataLength; i++) {
                var item = data[i];
                var currentRow = {
                    Columns: [
                                { Name: "ProductID", Value: item.ProductID },
                                { Name: "ProductName", Value: item.ProductName },
                                { Name: "Description", Value: item.Description },
                                { Name: "ProductPrefix", Value: item.ProductPrefix },
                                { Name: "Edit", Value: i, CellAlign: "center", CustomCellRender: function (cell, cellFormated) { return GenerateEditButton(cell, cellFormated); } }
                            ]
                };

                bodyData.push(currentRow);
            }
        }

        var GenerateEditButton = function (cell, cellFormated) {
            return "<input value='Edit' type='button' class='ui-button' onclick='productManager.EditProduct(" + cell.Value + ")' />";
        }

        var GenerateAddLicenseeButton = function (cell, cellFormated) {
            return "<input value='Add Licensee' type='button' class='ui-button' onclick='productManager.AddParty(" + cell.Value + ")' />";
        }

        gridOptions = {
            columns: [
                        { HeaderText: "ProductID", Width: 100, Name: "ProductID", HeaderAlign: "center", CellAlign: "left" },
                        { HeaderText: "Product Name", Width: 200, Name: "ProductName", HeaderAlign: "center", CellAlign: "left" },
                        { HeaderText: "Description", Width: 700, Name: "Description", HeaderAlign: "center", CellAlign: "left" },
                        { HeaderText: "Product Prefix", Width: 100, Name: "ProductPrefix", HeaderAlign: "center", CellAlign: "center" },
                        { HeaderText: "Edit", Width: 100, Name: "Edit", HeaderAlign: "center", CellAlign: "left" }
                    ],
            showTitle: true,
            //showFooter: true,
            titleInfo:
            {
                text: "Product Manager",
                showExcelButton: false,
                showCollapseExpandButton: true
            },
            showTree: true,
            bodyRows: bodyData,
            showDistinctRow: false,
            //footerRows: null,
            gridExpandHeight: 200
        };

        _table.productsContainer.fanexGrid(gridOptions);
    };

    self.AddNewProduct = function () {
        var originalDialog = _table.dlgAddNew.html();
        cachedDOMElements();
        _table.dlgAddNew.find('.update').each(function () { $(this).hide() });
        _table.dlgAddNew.dialog({
            autoOpen: false,
            height: 400,
            width: 500,
            modal: true,
            buttons: {
                "Create a Product": function () {
                    if (ValidateForm()) {
                        _dlgAddNew.txtValidateTips.hide();
                        _table.frmProduct.submit();
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

        _table.dlgAddNew.dialog('option', 'title', 'Add new Product');
        _table.dlgAddNew.dialog('open');
    };

    self.EditProduct = function (rowNum) {
        _table.dlgAddNew.attr('title', 'Edit Product');
        var originalDialog = _table.dlgAddNew.html();
        cachedDOMElements();
        FillDataToEditForm(rowNum);
        _table.dlgAddNew.dialog({
            autoOpen: false,
            height: 600,
            width: 500,
            modal: true,
            buttons: {
                "Edit Product": function () {
                    if (ValidateForm()) {
                        _dlgAddNew.txtValidateTips.hide();
                        _table.frmProduct.attr("Action", domain + "admin/Product/UpdateProduct");
                        _table.frmProduct.submit();
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

        _table.dlgAddNew.dialog('option', 'title', 'Edit Product');
        _table.dlgAddNew.dialog('open');
    };

    var FillDataToEditForm = function (rowNum) {
        var row = gridOptions.bodyRows[rowNum].Columns;
        _dlgAddNew.hidProductID.val(row[_mappingColumns.ProductID].Value);
        _dlgAddNew.txtProductID.val(row[_mappingColumns.ProductID].Value);
        _dlgAddNew.txtProductName.val(row[_mappingColumns.ProductName].Value);
        _dlgAddNew.txtDescription.val(row[_mappingColumns.Description].Value);
        _dlgAddNew.txtProductPrefix.val(row[_mappingColumns.ProductPrefix].Value);
    }

    var ValidateForm = function () {
        var result = true;
        _dlgAddNew.txtValidateTips.html("");
        if (_dlgAddNew.txtProductName.val().length <= 0) {
            _dlgAddNew.txtProductName.addClass("input-validation-error");
            _dlgAddNew.txtValidateTips.html(_dlgAddNew.txtValidateTips.html() + "Product Name must be enter.");
            result = false;
        }

        return result;
    }
}