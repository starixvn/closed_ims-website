/* File Created: March 28, 2013 */
var validateData = {
    checkParamBeforeUdpMsg: function () {
        var TransId = $('#TransId').val().replace(/\n/g, '').replace(/ /g, '');

        
        if (TransId == '') {
            $('#msg').html(tooltip.show('TransId', "TransId is required")).show();
            return false;
        }

        

        var arrTransId = TransId.split(',');
        var arrTransIdFail = [];
        for (var i = 0; i < arrTransId.length; i++) {
            if (isNaN(arrTransId[i]) || arrTransId[i].length > 10 || arrTransId[i].indexOf('.') != -1) {
                arrTransIdFail.push(arrTransId[i]);
            }
        }

        if (arrTransIdFail.length > 0) {
            $('#msg').html(tooltip.show('TransId', "Invalid TransId: " + arrTransIdFail.join(', '))).show();
            return false;
        }

        if (!Message.Validator.validateMessageConstraints()) {
            return false;
        }
        return true;
    }
};