/* File Created: February 6, 2013 */

$(function () {
    var delSingleTicketManager = new DelSingleTicketManager();
    delSingleTicketManager.Init();
});

function DelSingleTicketManager() {
    this.delSingleTicket = new DelSingleTicket();
}

DelSingleTicketManager.prototype.Init = function () {
    var self = this;
    self.delSingleTicket.Init();
}