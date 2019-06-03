; (function () {
    abp.message.info = function (message, title) {
        app.$Message.info({ content: message });
    };

    abp.message.success = function (message, title) {
        app.$Message.success({ content: message });
    };

    abp.message.warn = function (message, title) {
        app.$Message.warning({ content: message });
    };

    abp.message.error = function (message, title) {
        app.$Message.error({ content: message });
    };

    //abp.message.confirm = function (message, titleOrCallback, callback) {
    //    var opts = $.extend(
    //        {},
    //        abp.libs.layer.config['default'],
    //        abp.libs.layer.config.confirm
    //    );

    //    if ($.isFunction(titleOrCallback)) {
    //        callback = titleOrCallback;
    //    } else if (titleOrCallback) {
    //        opts.title = titleOrCallback;
    //    };


    //    return $.Deferred(function ($dfd) {
    //        layui.layer.confirm(message, opts, function (index) {
    //            layer.close(index);
    //            callback && callback();
    //            $dfd.resolve();
    //        })
    //    });
    //};

})();