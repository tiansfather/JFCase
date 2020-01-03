function initBee() {
    var manyState = layui.data('manyState');
    if (!manyState.hideBee) {
        $("#help-bee").show();
    }
    var flag;
    function hideBee() {
        flag = true;
        document.onmousemove = null;
        document.onmouseup = null;
        $("#help-bee").css({ 'right': '' });
        $("#help-bee").css({ 'left': '', 'right': $("#help-bee").css('right') });
        $("#help-bee").animate({
            right: '400px',
            top: '300px',
            opacity: '0.7'
        }, "2000", 'linear');
        $("#help-bee").animate({
            right: '300px',
            top: '0px',
            opacity: '0.3',
        }, '2000', 'linear', function () { $('#help-bee').hide() });
        $('#r-shade').hide();
        layui.data('manyState', {
            key: 'hideBee'
            , value: true
        });
        return false;
    }
    var disX = 0, disY = 0, oDiv = $('#help-bee')[0];
    $('#help-bee').on('mousedown', function (ev) {
        $('#r-shade').show();
        var oEvent = ev || event;

        disX = oEvent.clientX - oDiv.offsetLeft;
        disY = oEvent.clientY - oDiv.offsetTop;

        document.onmousemove = function (ev) {
            var oEvent = ev || event;
            var l = oEvent.clientX - disX;
            var t = oEvent.clientY - disY;

            if (l < 0) {
                hideBee();
                l = 0;
            }
            else if (l > $(document).width() - $('#help-bee img').width()) {
                hideBee();
                l = $(document).width() - $('#help-bee img').width();
            }

            if (t < 0) {
                hideBee();
                t = 0;
            }
            else if (t > $(document).height() - $('#help-bee img').height()){
                hideBee();
                t = $(document).height() - $('#help-bee img').height();
            }
            if (!flag) {
                oDiv.style.left = l + 'px';
                oDiv.style.top = t + 'px';
            }
        };

        document.onmouseup = function () {
            document.onmousemove = null;
            document.onmouseup = null;
            $('#r-shade').hide();
        };

        return false;
    })
}   
initBee();
var index = {
    //加载对应菜单组
    showMenu: function (menuName) {
        $("[menu-name]").hide();
		$("[menu-name='" + menuName + "']").fadeIn();
		//window["default"].$("body").trigger("menuChange", menuName);
        layui.element.tabChange('layadmin-layout-tabs', '/Home'); 
        layui.admin.tabsBodyChange(0);
    }
};
var app = new Vue({
    el: '#LAY_app',
    data: { text: '', lastText: '', sign: '', tipsText:''},
    methods: {
        toReadNotices: function () {
            //function weihuJingGao(startTime) {
            //    var startTime = startTime || ''
            //    app.setBulletinText(startTime + '&nbsp;&nbsp;&nbsp;&nbsp;如有问题，请咨询tel:15906863922')
            //    function jinggao() {
            //        layui.layer.closeAll(layerJinggao);
            //        var layerJinggao = layer.open({
            //            //area: ['500px', '300px'],
            //            type: 0,
            //            offset: '10px',
            //            anim: 0,
            //            shade: 0,
            //            btn:[],
            //            title: false,
            //            content: `<div style="">
            //                            <span style="text-align:center">${startTime}</span>
            //                            <p style="display: flex;justify-content: center;">如有问题，请咨询tel:15906863922</p>
            //                        </div>`,
            //            cancel: function (index, layero) {
            //                layer.close(index);
            //            }   
            //        });
            //    }
            //    jinggao();
            //}
            var alreadyKnow = false;
            abp.services.app.notice.getActiveNotices({ error: function (err) { console.log(err); }, abpHandleError:false}).done(function (data) {
                if (data) {
                    $("#newMinerTip").show();
                } else {
                    $("#newMinerTip").hide();
                }
            })
        },
        scrollBulletin: function () {//滚动公告
            var that = this;
            var demo = document.getElementById("bulletin");
            var demo1 = document.getElementById("bulletin1");
            var demo2 = document.getElementById("bulletin2");
            if (!demo || !demo1 || !demo2) return;
            //demo2.innerHTML = document.getElementById("bulletin1").innerHTML;
            var hadStart = false;
            var setFTime;
            function Marquee() {
                if (!hadStart) {
                    if (demo.scrollLeft - demo2.offsetWidth >= 0) {
                        hadStart = true;
                        demo.scrollLeft -= demo1.offsetWidth;
                    } else {
                        demo.scrollLeft++;
                    }
                } else if (!setFTime){//在运动到重新开始时，暂停10s
                    setFTime = setTimeout(function() {
                        hadStart = false;
                        setFTime = null;
                    }, 10000)
                }
            }
            clearInterval(this.myvar);
            this.myvar = setInterval(Marquee, 50);
            demo.onmouseout = function () {
                that.myvar = setInterval(Marquee, 50);
            }
            demo.onmouseover = function () {
                clearInterval(that.myvar);
            }
        },
        setBulletinText: function (theHtml, sign = 'd', time = 36000) {
            var that = this;
            this.text = theHtml;
            if (theHtml) {
                this.sign = sign;
            }
            if (time) {
                setTimeout(function () { that.text = ''; that.lastText = theHtml; that.sign=''}, time * 1000);
            }
            this.$nextTick(that.shortClear)
        },
        shortClear: function () {
            var nowWidth = $('#bulletin').width()
            var tLeft = $('.layui-layout-right').outerWidth() - nowWidth
            var odd = $('.layui-header').width() - ($('.layui-layout-left').outerWidth() + $('.layui-layout-left').offset().left);
            $('#bulletin').width(odd - tLeft)
            var demo = document.getElementById("bulletin");
            var demo1 = document.getElementById("bulletin1");
            //console.log($('.bulletin-board').html(), this.text)
            if ($('.bulletin-board').width() > $('#bulletin1').width()) {
                $('#bulletin2').hide();
                clearInterval(this.myvar);
                demo.scrollLeft -= demo1.offsetWidth;
                demo.onmouseout = null;
                demo.onmouseover = null;
            } else {
                //console.log('a')
                $('#bulletin2').show();
                this.scrollBulletin();
            }


        }
    }
})
function initIndex() {
    //token检验
    checkToken();
    //当前用户信息
    abp.services.app.session.getCurrentLoginInformations().done(function (data) {
        console.log(data);
        if (data.user.needChangePwd) {
            layer.open({
                type: 2,
                title: "首次登录必须修改密码",
                scrollbar: false,
                shadeClose: false,
                closeBtn:false,
                shade: 0.8,
                area: ['500px', '400px'],
                content: "/Account/ChangePassword",
                btn: ['提交'],
                btnAlign: 'l',
                yes: function (index, layero) {
                    var iframeWin = window[layero.find('iframe')[0]['name']]; //得到iframe页的窗口对象，执行iframe页的方法：iframeWin.method();
                    if (iframeWin.submit) { iframeWin.submit(); return false; }
                }
            });
        }
        layui.data('session', { key: 'loginInfo', value: JSON.stringify(data) });
    });
    //帮助文档
    if (abp.services.help) {
        $("a[custom-event='help']").closest("li").removeClass("layui-hide");
    }
    //锁屏
    var lockPage = function () {
        layer.prompt({ title: '请输入密码进行解锁', formType: 1, closeBtn: 0, btn: ['确定'] }, function (pass, index) {
			func.runAsync(abp.services.app.session.checkPassword(pass).done(function (data) {
				if (data) {
					layer.close(index);
					window.sessionStorage.setItem("lockcms", false);
				} else {
					layer.msg('密码错误', { icon: 5, anim: 6 })
				}
			}))
            
        });
    }
    // 判断是否显示锁屏
    if (window.sessionStorage.getItem("lockcms") == "true") {
        lockPage();
    }

    $("[lay-href='']").removeAttr("lay-href");
    //加载第一个菜单组
    $("li.menuitem:first a").click();

    // 点击空白处关闭右键弹窗
    $(document).click(function () {
        $('.rightmenu').hide();
    })

    //自定义事件
    $("body").on("click", "*[custom-event]", function () {
        var othis = $(this)
            , attrEvent = othis.attr('custom-event');
        customevents[attrEvent] && customevents[attrEvent].call(this, othis);
    })

    customevents = {
        lock: function (othis) {
            window.sessionStorage.setItem("lockcms", true);
            lockPage();
        },
        help: function (othis) {
            var text= $('.layui-tab-title .layui-this span').text().trim();
            abp.services.help.helps.getHelpTitlesByMenuDisplayName(text + '').done(function (res) {
                console.log(text, res)
                var string = layui.laytpl(`<ul>
{{# if($.isEmptyObject(d.res)){}}
<li class="help-li">欢迎到"帮助文档"添加文档</li>
{{#}else{}}
  {{#  layui.each(d.res, function(index, item){ }}
    <li>
      <span class="help-li" data-id="{{index}}">{{ item }}</span>
    </li>
  {{#  }); }}
{{#}}}
</ul>`).render({
                    res: res
                });
                var lay=layer.open({
                    title: "帮助文档",
                    shadeClose :true,
                    shade: 0.1,
                    offset: ["41px",  othis.offset().left - 250 + "px"],
                    anim: -1,
                    id: "LAY_adminNote",
                    skin: "layadmin-note layui-anim layui-anim-upbit",
                    content: string,
                    btn:[]
                })

                $('.help-li').click(function () {
                    var id = $(this).data('id');
                    if (id) {
                        abp.services.help.helps.getHelpInfo(id).done(function (h) {
                            console.log(h)
                            layer.open({
                                type: 1,
                                title: h.helpTitle,
                                shadeClose: true,
                                shade: false,
                                maxmin: true, //开启最大化最小化按钮
                                area: ['893px', '600px'],
                                content: '<div class="help-wrap_div">'+ h.helpContent+'</div>'
                            });
                        })
                    }
                    layer.close(lay);
                })
            })
        }
    }

    /**
    * 注册tab右键菜单点击事件
    */
    $("body").on('contextmenu', '.layui-tab-title li', function (e) {
        var popupmenu = $(".rightmenu");
        l = ($(document).width() - e.clientX) < popupmenu.width() ? (e.clientX - popupmenu.width()) : e.clientX;
        t = ($(document).height() - e.clientY) < popupmenu.height() ? (e.clientY - popupmenu.height()) : e.clientY;

        //判断是否是首页
        var currentActiveTabID = $("li[class='layui-this']").attr('lay-id');// 获取当前激活的选项卡ID
        if (currentActiveTabID != "/Home") {
            popupmenu.css({ left: l, top: t }).show();
        }
        return false;
    });
    $(".rightmenu li").click(function () {
        var currentActiveTabID = $("li[class='layui-this']").attr('lay-id');// 获取当前激活的选项卡ID
        var tabtitle = $(".layui-tab-title li");
        var allTabIDArr = [];
        $.each(tabtitle, function (i) {
            //去除主页
            if ($(this).attr("lay-id") != "/Home") {
                allTabIDArr[i] = $(this).attr("lay-id");
            }
        })

        switch ($(this).attr("data-type")) {
            case "closethis"://关闭当前，如果开始了tab可关闭，实际意义不大
                tabDelete(currentActiveTabID);
                break;
            case "closeall"://关闭所有
                tabDeleteAll(allTabIDArr);
                break;
            case "closeothers"://关闭非当前
                $.each(allTabIDArr, function (i) {
                    var tmpTabID = allTabIDArr[i];
                    if (currentActiveTabID != tmpTabID)
                        tabDelete(tmpTabID);
                })
                break;
            case "closeleft"://关闭左侧全部
                var index = allTabIDArr.indexOf(currentActiveTabID);
                tabDeleteAll(allTabIDArr.slice(0, index));
                break;
            case "closeright"://关闭右侧全部
                var index = allTabIDArr.indexOf(currentActiveTabID);
                tabDeleteAll(allTabIDArr.slice(index + 1));
                break;
            default:
                $('.rightmenu').hide();

        }
        $('.rightmenu').hide();
    });

    tabDelete = function (id) {
        console.log("删除的TabID：" + id)
        layui.element.tabDelete("layadmin-layout-tabs", id);//删除
    }
    tabDeleteAll = function (ids) {
        $.each(ids, function (i, item) {
            layui.element.tabDelete("layadmin-layout-tabs", item);
        })
    }


    setInterval("app.toReadNotices()", 60000);
    app.toReadNotices();
    //app.scrollBulletin();
}

function checkToken() {
    //管理账套及Default账套不进行检测
    if (!abp.session.tenantId || abp.session.tenantId==1) { return; }
    //模拟登录不进行检测
    if ($.cookie("simulateLogin")) { return; }

    var token = $.cookie("token");
    var func = function () {
        if (token != $.cookie("token")) {
            top.layer.alert("当前账号已退出", function () {
                top.location.href = "/Account/Login";
            });
            return;
        }
        abp.services.app.user.getCurrentToken({
            error: function () {
                abp.message.error("网络连接不通畅，无法提供服务。请关闭窗口稍后重试。");
                //window.clearInterval(checkTokenInterval);
            }
        }).done(function (data) {
            if (data && data != token) {
                $.removeCookie("token");
                top.layer.alert("当前账号已在其它电脑登录", function () {
                    top.location.href = "/Account/Login";
                });
                window.setTimeout(function () { top.location.href = "/Account/Login"; }, 5000);
            } else {
                window.setTimeout(func, 5000);
            }
            
        });
    };
    func();
    //var checkTokenInterval = window.setInterval(, 1000);
}