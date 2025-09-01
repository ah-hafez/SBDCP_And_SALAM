function Signature(signatureControlId, hdnIdToSaveBase64Image, clearButtonId) {
 
    var $sigdiv = $('#' + signatureControlId).jSignature({})
     , $tools = $('#tools')
    , $extraarea = $('#displayarea')
    , pubsubprefix = 'jSignature.demo.';

    var topics = {}; $.publish = function (topic, args) {
        if (topics[topic]) {
            var currentTopic = topics[topic], args = args || {};

            for (var i = 0, j = currentTopic.length; i < j; i++) {
                currentTopic[i].call($, args);
            }
        }
    };

    $.subscribe = function (topic, callback) {
        if (!topics[topic]) {
            topics[topic] = [];
        }

        topics[topic].push(callback);
        return {
            'topic': topic,
            'callback': callback
        };
    };

    $.unsubscribe = function (handle) {

        var topic = handle.topic;

        if (topics[topic]) {
            var currentTopic = topics[topic];

            for (var i = 0, j = currentTopic.length; i < j; i++) {
                if (currentTopic[i] === handle.callback) {
                    currentTopic.splice(i, 1);
                }
            }
        }
    };

    $(document).ready(function () {

        $('#' + hdnIdToSaveBase64Image).val(null);
        var export_plugins = $sigdiv.jSignature('listPlugins', 'export')
        , chops = ['<span><b>Extract signature data as: </b></span><select>', '<option value="">(select export format)</option>']
        , name

        for (var i in export_plugins) {
            if (export_plugins.hasOwnProperty(i)) {
                name = export_plugins[i]
                chops.push('<option value=' + name + '>' + name + '</option>')
            }
        }

        chops.push('</select><span><b> or: </b></span>')

        $('#' + clearButtonId).on("click", function (e) {
            $('#' + signatureControlId).jSignature('clear');
            $('#' + hdnIdToSaveBase64Image).val(null);
        });


        $('#' + signatureControlId).bind('change', function (e) {

            var data = $('#' + signatureControlId).jSignature('getData', 'image');
            var signatureData = '';

            $.publish(pubsubprefix + 'formatchanged')

            if (typeof data == 'string') {
                signatureData = (data);
            }
            else if ($.isArray(data) && data.length == 2) {
                signatureData = (data.join(','));
                $.publish(pubsubprefix + data[0], data);
            }
            else {
                try {
                    signatureData = (JSON.stringify(data));
                }
                catch (ex) {
                    sImageData = ('');                    
                }
            }
            $('#' + hdnIdToSaveBase64Image).val(signatureData);
        });
    });
}

//function GetSignature(hiddenFieldIdToStoreSignatureData)
//{   
//    //$(document).ready(function ()
//    //{
//    var $sigdiv = $('#signatureTest').jSignature({ 'UndoButton': false })
//    , $tools = $('#tools')
//   , $extraarea = $('#displayarea')
//   , pubsubprefix = 'jSignature.demo.';

//        var data = $sigdiv.jSignature('getData', 'image');
//        var signatureData = '';

//        $.publish(pubsubprefix + 'formatchanged')

//        if (typeof data == 'string')
//        {
//            signatureData = (data);
//            $sigdiv.jSignature('clear');
//        }
//        else if ($.isArray(data) && data.length == 2) {
//            signatureData = (data.join(','));
//            $.publish(pubsubprefix + data[0], data);
//            $sigdiv.jSignature('clear');
//        }
//        else
//        {
//            try
//            {
//                signatureData = (JSON.stringify(data));
//                $sigdiv.jSignature('clear');
//            }
//            catch (ex) {
//                sImageData = ('');
//                $sigdiv.jSignature('clear');
//            }
//        }
//        hiddenFieldIdToStoreSignatureData.value = signatureData;
//    //});
//}