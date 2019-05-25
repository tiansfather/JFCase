
var im = null;
//abp.signalr.startConnection('/signalr-chathub', function (connection) {
//    chatHub = connection; // Save a reference to the hub

    
//}).then(function (connection) {
//    abp.log.debug('Connected to myChatHub server!');
//    abp.event.trigger('abp.signalr.connected');
//});

(function () {
    if ($.cookie("simulateLogin")) {
        return;
    }
    abp.event.on('abp.signalr.connected', function () { // Register for connect event
        console.log('invoke sendOffLineMessage');
        //连接成功后获取未读消息
        abp.signalr.hubs.common.invoke('sendOffLineMessage');
        //获取未读通知
        abp.signalr.hubs.common.invoke('sendOffLineNotification');
    });

    abp.event.on('abp.notifications.received', function (userNotification) {
        //获取通知消息
        console.log(userNotification);
        abp.notifications.showUiNotifyForUserNotification(userNotification);
        //获取未读数量
        im.refreshMsgbox();
        //向服务器发送已读
        //abp.signalr.hubs.common.invoke('setNotificationReaded', userNotification.id);
    });

    layui.use('layim', function () {
        var layim = layui.layim;
        im = layim;

        im.refreshMsgbox = function () {
            //获取未读数量
            abp.services.app.layIM.getUnReadedNotificationCount().done(function (data) {
                im.msgbox(data);
            })
        }

        /*Hub事件*/
        if (abp.signalr.hubs.common) {
            abp.signalr.hubs.common.on('getIMMessage', function (message) { // Register for incoming messages
                console.log(message);
                if (message.cid > 0) {
                    //向服务器发送已读
                    abp.signalr.hubs.common.invoke('setIMMessageReaded', message.cid);
                }
                layim.getMessage(message);
            });
            abp.signalr.hubs.common.on('getAllMessage', function (message) { // Register for incoming messages
                console.log('received all message: ' + message);
            });
            abp.signalr.hubs.common.on('friendStatusChange', function (userid, name, status) { // 用户上下线
                console.log(userid + status);
                layim.setFriendStatus(userid, status);
                if (status == "online") {
                    //todo:优化上线提示
                    layer.msg(name + "上线了");
                }

            });
        }

        /**/


        //基础配置
        layim.config({
            //初始化接口
            init: {
                url: '/api/services/im/data/GetInitData'
                , data: {}
            }
            //查看群员接口
            , members: {
                url: '/api/services/im/data/GetMember'
                , data: {}
            }

            , uploadImage: {
                url: '/IM/Upload' //（返回的数据格式见下文）
            }
            , uploadFile: {
                url: '/IM/Upload' //（返回的数据格式见下文）
            }

            , isAudio: true //开启聊天工具栏音频
            , isVideo: true //开启聊天工具栏视频

            //扩展工具栏
            , tool: [{
                alias: 'code'
                , title: '代码'
                , icon: '&#xe64e;'
            }]

            //,brief: true //是否简约模式（若开启则不显示主面板）

            , title: 'WebIM' //自定义主面板最小化时的标题
            //,right: '100px' //主面板相对浏览器右侧距离
            //,minRight: '90px' //聊天面板最小化时相对浏览器右侧距离
            , initSkin: '3.jpg' //1-5 设置初始背景
            //,skin: ['aaa.jpg'] //新增皮肤
            //,isfriend: false //是否开启好友
            //,isgroup: false //是否开启群组
            //,min: true //是否始终最小化主面板，默认false
            , notice: true //是否开启桌面消息提醒，默认false
            //,voice: false //声音提醒，默认开启，声音文件为：default.mp3
            , copyright: true
            , msgbox: '/im/msgbox' //消息盒子页面地址，若不开启，剔除该项即可
            //, find: '/layim/demo/find.html' //发现页面地址，若不开启，剔除该项即可
            , chatLog: '/im/chatlog' //聊天记录页面地址，若不开启，剔除该项即可

        });
        //监听在线状态的切换事件
        layim.on('online', function (status) {
            if (status == "hide") {
                layer.msg("你想干什么？不允许隐身！");
            }

        });

        //监听签名修改
        layim.on('sign', function (value) {
            abp.signalr.hubs.common.invoke('changeSign', value)
                .then(function () {
                    layer.msg("提交成功");
                });;
            //layer.msg(value);
        });
        //监听自定义工具栏点击，以添加代码为例
        layim.on('tool(code)', function (insert) {
            layer.prompt({
                title: '插入代码 '
                , formType: 2
                , shade: 0
            }, function (text, index) {
                layer.close(index);
                insert('[pre class=layui-code]' + text + '[/pre]'); //将内容插入到编辑器
            });
        });

        //监听layim建立就绪
        layim.on('ready', function (res) {
            //console.log(res.mine);
            //layim.msgbox(5); //模拟消息盒子有新消息，实际使用时，一般是动态获得
        });
        //监听发送消息
        layim.on('sendMessage', function (data) {
            if (data.to.type == "friend" && data.to.id == data.mine.id) {
                return false;
            }
            var To = data.to;
            console.log(data);

            abp.signalr.hubs.common.invoke('sendIMMessage', data)
                .then(function () {
                    layer.msg("发送成功");
                });;

            //if (data.to.type === 'friend') {
            //     // Send a message to the server
            //    //layim.setChatStatus('<span style="color:#FF5722;">对方正在输入。。。</span>');
            //} else if (data.to.type === "group") {
            //    abp.signalr.hubs.common.invoke('sendGroupMessage', data.to.id, data.mine.content);
            //}

            //演示自动回复
            //setTimeout(function () {
            //    var obj = {};
            //    if (To.type === 'group') {
            //        obj = {
            //            username: '模拟群员' + (Math.random() * 100 | 0)
            //            , avatar: layui.cache.dir + 'images/face/' + (Math.random() * 72 | 0) + '.gif'
            //            , id: To.id
            //            , type: To.type
            //            , content: autoReplay[Math.random() * 9 | 0]
            //        }
            //    } else {
            //        obj = {
            //            username: To.name
            //            , avatar: To.avatar
            //            , id: To.id
            //            , type: To.type
            //            , content: autoReplay[Math.random() * 9 | 0]
            //        }
            //        layim.setChatStatus('<span style="color:#FF5722;">在线</span>');
            //    }
            //    layim.getMessage(obj);
            //}, 1000);
        });
        //监听查看群员
        layim.on('members', function (data) {
            //console.log(data);
        });

        //监听聊天窗口的切换
        layim.on('chatChange', function (res) {
            var type = res.data.type;
            console.log(res)
            if (type === 'friend') {
                //模拟标注好友状态
                //if (res.data.status == "online") {
                //    layim.setChatStatus('<span style="color:#FF5722;">在线</span>');
                //} else {
                //    layim.setChatStatus('<span style="color:#d4c7c3;">离线</span>');
                //}

            } else if (type === 'group') {
                //模拟系统消息
                //layim.getMessage({
                //    system: true
                //    , id: res.data.id
                //    , type: "group"
                //    , content: '模拟群员' + (Math.random() * 100 | 0) + '加入群聊'
                //});
            }
        });



    });

})();
