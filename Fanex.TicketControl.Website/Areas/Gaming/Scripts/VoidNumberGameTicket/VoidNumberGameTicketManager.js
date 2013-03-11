/* File Created: February 6, 2013 */

$(function () {
    var voidNumberGameTicketManager = new VoidNumberGameTicketManager();
    voidNumberGameTicketManager.Init();
});

function VoidNumberGameTicketManager() {
    this.voidNumberGameTicket = new VoidNumberGameTicket();
}

VoidNumberGameTicketManager.prototype.Init = function () {
    var self = this;
    self.voidNumberGameTicket.Init();
}