/* File Created: February 6, 2013 */

$(function () {
    var voidMixParlayTicketManager = new VoidMixParlayTicketManager();
    voidMixParlayTicketManager.Init();
});

function VoidMixParlayTicketManager() {
    this.voidMixParlayTicket = new VoidMixParlayTicket();
}

VoidMixParlayTicketManager.prototype.Init = function () {
    var self = this;
    self.voidMixParlayTicket.Init();
}