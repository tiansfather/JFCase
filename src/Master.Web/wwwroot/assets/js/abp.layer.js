var abp = abp || {};
(function ($) {
    if (!layui || !$) {
        return;
    }

    /* DEFAULTS *************************************************/

    abp.libs = abp.libs || {};
    abp.libs.layer = {
        config: {
            'default': {

            },
            info: {
            	icon: 0,
				title:"提示"
            },
            success: {
            	icon: 1,
				title:"成功"
            },
            warn: {
            	icon: 5,
				title:"警告"
            },
            error: {
            	icon: 2,
				title:"错误"
            },
            confirm: {
                icon: 3,
                title: '确认操作?',
                btn: ['确认', '取消']
            }
        }
    };

    /* MESSAGE **************************************************/

    var showMessage = function (type, message, title) {        

        var opts = $.extend(
            {},
            abp.libs.layer.config['default'],
            abp.libs.layer.config[type]
        );

        if (title) {
        	opts.title = title;
        }

        return $.Deferred(function ($dfd) {
        	layui.layer.alert(message, opts, function (index) {
        		layer.close(index);
        		$dfd.resolve();
        	});
        });
    };

    abp.message.info = function (message, title) {
        return showMessage('info', message, title);
    };

    abp.message.success = function (message, title) {
        return showMessage('success', message, title);
    };

    abp.message.warn = function (message, title) {
        return showMessage('warn', message, title);
    };

    abp.message.error = function (message, title) {
        return showMessage('error', message, title);
    };

    abp.message.confirm = function (message, titleOrCallback, callback) {
    	var opts = $.extend(
            {},
            abp.libs.layer.config['default'],
            abp.libs.layer.config.confirm
        );

        if ($.isFunction(titleOrCallback)) {
            callback = titleOrCallback;
        } else if (titleOrCallback) {
        	opts.title = titleOrCallback;
        };


        return $.Deferred(function ($dfd) {
        	layui.layer.confirm(message, opts, function (index) {
        		layer.close(index);
        		callback && callback();
        		$dfd.resolve();
        	})
        });
    };

    abp.event.on('abp.dynamicScriptsInitialized', function () {
        //abp.libs.sweetAlert.config.confirm.title = abp.localization.abpWeb('AreYouSure');
        //abp.libs.sweetAlert.config.confirm.buttons = [abp.localization.abpWeb('Cancel'), abp.localization.abpWeb('Yes')];
    });
    /* NOTIFICATION *********************************************/

    var showNotification = function (type, message, title) {
        layer.open({
            title: title,
            btn: null,
            shade: 0,
            content: message
            , offset: 'rb',
            icon: abp.libs.layer.config[type]["icon"],
            anim: 2
        });

        //toastr[type](message, title, options);
    };

    abp.notify.success = function (message, title, options) {
        showNotification('success', message, title, options);
    };

    abp.notify.info = function (message, title, options) {
        showNotification('info', message, title, options);
    };

    abp.notify.warn = function (message, title, options) {
        showNotification('warn', message, title, options);
    };

    abp.notify.error = function (message, title, options) {
        showNotification('error', message, title, options);
    };
})(jQuery);