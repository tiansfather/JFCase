; (function () {
    abp.message.info = function (message, title, callback) {
        return $.Deferred(function ($dfd) {
            //app.$Modal.remove();
            //setTimeout(() => {                
            //    app.$Modal.info({
            //        title: title || '消息', content: message, onOk: function () {
            //            callback && callback();
            //            $dfd.resolve();
            //        }
            //    })
            //}, 300);
            app.$alert(message, title||'提示', {
                confirmButtonText: '确定',
                //cancelButtonText: '取消',
                type: 'info',
                confirmButtonClass: 'btn-footer',
                cancelButtonClass: 'btn-footer-qx',
                dangerouslyUseHTMLString: true,
                showClose:false
            }).then(() => {
                callback && callback();
                $dfd.resolve();
            });
        });
    };

    abp.message.success = function (message, title,callback) {
        return $.Deferred(function ($dfd) {
            //app.$Modal.remove();
            //setTimeout(() => {
            //    app.$Modal.success({
            //        title: title || '成功', content: message, onOk: function () {
            //            callback && callback();
            //            $dfd.resolve();
            //        }
            //    })
            //}, 300);
            app.$alert(message, title || '成功', {
                confirmButtonText: '确定',
                //cancelButtonText: '取消',
                type: 'success',
                confirmButtonClass: 'btn-footer',
                cancelButtonClass: 'btn-footer-qx',
                dangerouslyUseHTMLString: true,
                showClose: false
            }).then(() => {
                callback && callback();
                $dfd.resolve();
            });
        });
        //app.$Modal.success({ title: title||'成功',content: message });
    };

    abp.message.warn = function (message, title, callback) {
        return $.Deferred(function ($dfd) {
            //app.$Modal.remove();
            //setTimeout(() => {
            //    app.$Modal.warning({
            //        title: title || '警告', content: message, onOk: function () {
            //            callback && callback();
            //            $dfd.resolve();
            //        }
            //    })
            //}, 300);
            app.$alert(message, title || '警告', {
                confirmButtonText: '确定',
                //cancelButtonText: '取消',
                type: 'warning',
                confirmButtonClass: 'btn-footer',
                cancelButtonClass: 'btn-footer-qx',
                dangerouslyUseHTMLString: true,
                showClose: false
            }).then(() => {
                callback && callback();
                $dfd.resolve();
            });
        });
    };

    abp.message.error = function (message, title, callback) {        
        return $.Deferred(function ($dfd) {
            //app.$Modal.remove();
            //setTimeout(() => {
            //    app.$Modal.error({
            //        title: title || '错误', content: message, onOk: function () {
            //            callback && callback();
            //            $dfd.resolve();
            //        }
            //    })
            //}, 300);
            app.$alert(message, title || '错误', {
                confirmButtonText: '确定',
                //cancelButtonText: '取消',
                type: 'error',
                confirmButtonClass: 'btn-footer',
                cancelButtonClass: 'btn-footer-qx',
                dangerouslyUseHTMLString: true,
                showClose: false
            }).then(() => {
                callback && callback();
                $dfd.resolve();
            });
        });
    };

    abp.message.confirm = function (message, titleOrCallback, callback) {
        var title = "确认提交";
        if ($.isFunction(titleOrCallback)) {
            callback = titleOrCallback;
        } else if (titleOrCallback) {
            title = titleOrCallback;
        };


        return $.Deferred(function ($dfd) {
            //app.$Modal.remove();
            //setTimeout(() => {
            //    app.$Modal.confirm({
            //        title: title, content: message, onOk: function () {
            //            callback && callback();
            //            $dfd.resolve();
            //        }
            //    })
            //}, 300);
            app.$confirm(message, title, {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                confirmButtonClass: 'btn-footer',
                cancelButtonClass: 'btn-footer-qx',
                
                type: 'warning'
            }).then(() => {
                callback && callback();
                $dfd.resolve();
            })
        });
    };

})();


(function () {

    
    abp.ui.setBusy = function (elm, optionsOrPromise) {
        optionsOrPromise = optionsOrPromise || {};
        if (optionsOrPromise.always || optionsOrPromise['finally']) { //Check if it's promise
            optionsOrPromise = {
                promise: optionsOrPromise
            };
        }

        var options = $.extend({}, optionsOrPromise);

        if (!elm) {
            if (options.blockUI != false) {
                abp.ui.block();
            }
            $('body').append('<div id="abpUiBusy" class="ivu-spin ivu-spin-large ivu-spin-fix"><div class="ivu-spin-main"><span class="ivu-spin-dot"></span> <div class="ivu-spin-text"></div></div></div>')
            if (app && app.$loading) {
                abp.ui.loading = app.$loading();
            }
            //改这里
            //app.$Spin.show();
            //$('body').spin(abp.libs.spinjs.spinner_config);
        } else {
            //var $elm = $(elm);
            //var $busyIndicator = $elm.find('.abp-busy-indicator'); //TODO@Halil: What if  more than one element. What if there are nested elements?
            //if ($busyIndicator.length) {
            //    $busyIndicator.spin(abp.libs.spinjs.spinner_config_inner_busy_indicator);
            //} else {
            //    if (options.blockUI != false) {
            //        abp.ui.block(elm);
            //    }

            //    $elm.spin(abp.libs.spinjs.spinner_config);
            //}
        }

        if (options.promise) { //Supports Q and jQuery.Deferred
            if (options.promise.always) {
                options.promise.always(function () {
                    abp.ui.clearBusy(elm);
                });
            } else if (options.promise['finally']) {
                options.promise['finally'](function () {
                    abp.ui.clearBusy(elm);
                });
            }
        }
    };

    abp.ui.clearBusy = function (elm) {
        //TODO@Halil: Maybe better to do not call unblock if it's not blocked by setBusy
        if (!elm) {
            abp.ui.unblock();
            $('#abpUiBusy').remove();
            (app && abp.ui.loading) && abp.ui.loading.close();
            //改这里
            //app.$Spin.hide();
        } else {
            //var $elm = $(elm);
            //var $busyIndicator = $elm.find('.abp-busy-indicator');
            //if ($busyIndicator.length) {
            //    $busyIndicator.spin(false);
            //} else {
            //    abp.ui.unblock(elm);
            //    $elm.spin(false);
            //}
        }
    };

})();