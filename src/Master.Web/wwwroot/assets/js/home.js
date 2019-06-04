; (function () {
    abp.message.info = function (message, title, callback) {
        return $.Deferred(function ($dfd) {
            app.$Modal.info({
                title: title || '消息', content: message, loading: false, onOk: function () {
                    callback && callback();
                    $dfd.resolve();
                }
            })
        });
    };

    abp.message.success = function (message, title,callback) {
        return $.Deferred(function ($dfd) {
            app.$Modal.success({
                title: title || '成功', content: message, loading: false, onOk: function () {
                    callback && callback();
                    $dfd.resolve();
                }
            })
        });
        //app.$Modal.success({ title: title||'成功',content: message });
    };

    abp.message.warn = function (message, title, callback) {
        return $.Deferred(function ($dfd) {
            app.$Modal.warning({
                title: title || '警告', content: message, loading: false, onOk: function () {
                    callback && callback();
                    $dfd.resolve();
                }
            })
        });
    };

    abp.message.error = function (message, title, callback) {        
        return $.Deferred(function ($dfd) {
            app.$Modal.error({
                title: title || '错误', content: message, loading: false, onOk: function () {
                    callback && callback();
                    $dfd.resolve();
                }
            })
        });
    };

    abp.message.confirm = function (message, titleOrCallback,loading, callback) {
        var title = "";
        if ($.isFunction(titleOrCallback)) {
            callback = titleOrCallback;
        } else if (titleOrCallback) {
            title = titleOrCallback;
        };


        return $.Deferred(function ($dfd) {
            app.$Modal.confirm({
                title: title, content: message, loading: loading, onOk: function () {
                    callback && callback();
                    $dfd.resolve();
                }
            })
        });
    };

})();