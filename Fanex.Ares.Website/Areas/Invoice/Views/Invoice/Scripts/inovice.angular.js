
var Invoice = angular.module("Invoice", []);

Invoice.controller(
            "InvoiceController",
            function ($scope) {
                $scope.invoiceItems = { "invoicemodel":
            [{ "StatementList": { "StatementDetailList": [{ Amount: 500, Description: "dfaf", Reference: "dfasfdaad" },
                                                        { Amount: 500, Description: "dfaf", Reference: "dfasfdaad"}]
            }
            },
              { "StatementList": { "StatementDetailList": [{ Amount: 500, Description: "dfaf", Reference: "dfasfdaad" },
                                                        { Amount: 500, Description: "dfaf", Reference: "dfasfdaad"}]
              }
              },
              { "StatementList": { "StatementDetailList": [{ Amount: 500, Description: "dfaf", Reference: "dfasfdaad" },
                                                        { Amount: 500, Description: "dfaf", Reference: "dfasfdaad"}]
              }
              },
            ]
                };
            })
            
