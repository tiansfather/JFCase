
var config = {
    layuiBase: '/assets/layuiadmin/',
    layuiIndex: 'lib/index',
    layuiExtends: {
        authtree: 'lib/extend/authtree',
        ztree: 'lib/extend/ztree/ztree',
        droptree: 'lib/extend/droptree',
		formSelects: '../lib/extend/formselects/formSelects-v4',
		tableSelect: '../lib/extend/tableselect/tableSelect',
        treeGrid: '../lib/extend/treegrid/treegrid',
        verifyInit: '../lib/extend/verifyInit/verifyInit',
        multiSelect: '../lib/extend/multiSelect/multiSelect',
        suggest: '../lib/extend/suggest/suggest',
        soulTable:'../lib/extend/soulTable/soulTable'
    },
    layuiModules: ['index', 'table', 'layer', 'form', 'element', 'laydate', 'tree', 'upload', 'colorpicker', 'formSelects', 'tableSelect', 'verifyInit', 'multiSelect', 'suggest','soulTable'],
    //页面layui加载完后调用
    ready: function () {
        console.log("onready not implemented");
    },
    //页面layui加载完遍历调用
    readyFuncs:[],
    //table加载完后调用
    onTableDone: function (res, curr, count) {
        $(window).resize()
        config.onTableDoneExport(res, curr, count);
        console.log("onTableDone not implemented");
    },
    onTableDoneExport: function (res, curr, count) {

    },
    tableRowdone: function (obj) {

    },
    tableCheckBoxdone: function (obj) {
    },
    //检索后调用
    reloadTable: function () {
        console.log("reloadTable not implemented");
    },
    refresh: function () {
        console.log("refresh not implemented");
    },
    showSearchForm: function (moduleKey) {
        //展示检索窗体
        var url = "/ModuleData/Search?moduleKey=" + moduleKey;
        layer.open({
            type: 2,
            title: L('检索'),
            closeBtn: 1,
            shade: 0.1,
            shadeClose: true,
            area: ['600px', '100%'],
            offset: 'l', //左侧弹出
            anim: 3,
            content: [url], //iframe的url，no代表不显示滚动条
            end: function () { //此处用于演示

            }
        });
    },
    showRelativeModuleForm: function (option) {
        var moduleKey = option.moduleKey,
            columnKey = option.columnKey,
            maxReferenceNumber = option.maxReferenceNumber || 1;
        //展示关联引用窗体
        var url = "/ModuleData/RelativeSelect?moduleKey=" + moduleKey + "&columnKey=" + columnKey + "&maxReferenceNumber" + maxReferenceNumber;
        window.referenceLayerIndex = layer.open({
            type: 2,
            title: L('关联查询'),
            closeBtn: 1,
            shade: 0.1,
            shadeClose: true,
            area: ['80%', '100%'],
            offset: 'r', //右侧弹出
            anim: 0,
            content: [url], //iframe的url，no代表不显示滚动条
            end: function () { //此处用于演示

            }
        });
    }
};
//给页面layui加载完遍历调用的数组进行监听
(function () {
    const arrayProto = Array.prototype
    const arrayMethods = Object.create(arrayProto)
    Object.defineProperty(arrayMethods, 'push', {
        value: function mutator() {
            const original = arrayProto['push']
            let args = Array.from(arguments)
            //缓存原生方法，之后调用
            if (func.typeof(args[0]) == 'function') {
                //塞进去
                original.apply(this, args)
            } else {
                console.warn('往这个数组里传非函数类型的，将无效！');
                return
            }
            //当已经加载过layui了，你再push进来我就直接帮你运行
            if (config.readyFuncs.hadRun) {
                args[0]();
                console.warn('layui的use已完成，这个数组已经遍历运行过了，直接执行吧！');
            }
        }

    })
    config.readyFuncs.__proto__ = arrayMethods;
}) ()
$(function () {

    //全局事件
    //tip事件
    $("body").on("mouseenter", "*[tips]", function () {
        if (layer) {
            var e = $(this);
            if (!e.attr("tips")) { return; }
            var i = e.attr("tips"),
                t = e.attr("lay-offset"),
                n = e.attr("lay-direction"),
                b = e.attr("lay-background"),
                a = e.attr("lay-area"),
                s = layer.tips(i, this, {
                    tips: [n || 1, b || "#000"],
                    time: -1,
                    anim: -1,
                    maxWidth: a || '',
                    success: function (e, a) {
                        t && e.css("margin-left", t + "px");
                    }
                });
            e.data("index", s)
        }
    }).on("mouseleave", "*[tips]", function () {
        layer&&layer.close($(this).data("index"))
    });
    $("body").on("mouseenter", "*[formtips]", function () {
        var e = $(this);
        if (!e.attr("formtips")) { return; }
        var i = e.attr("formtips"),
            b = e.attr("lay-background"),
            t = e.attr("lay-offset"),
            n = e.attr("lay-direction"),
            s = layer.tips(i, this, {
                tips: [n || 1, b||"#FFB800"],
                time: -1,
                success: function (e, a) {
                    t && e.css("margin-left", t + "px");
                    e.css("width", "auto");
                    e.css("max-width", "600px");
                }
            });
        e.data("index", s);
    }).on("mouseleave", "*[formtips]", function () {
        layer.close($(this).data("index"));
		});

	//通用layer弹出
	$("body").on("click", ".laydialog", function () {
        var obj = $(this);
        var url = obj.attr('url');
        var type;
        var content;
        if (url.indexOf('http') == 0 || url.indexOf('/') == 0) {
            type = 2;
            content = obj.attr("url");
        } else {
            type = 1;
            content = $(url);
            console.log(content)
        }
		layer.open({
			type: type,
            shade: 0.8,
            shadeClose: true,
			title: obj.attr("title")||obj.text()
			, area: [obj.attr("width")||'80%',obj.attr("height")||'80%']
			, content:content 
		});
	});
    //布局初始化,如div自适应
    func.initUI();

    //图片缩略放大事件 2018/5/24 13:55 lijianbo
    $("body").on('click', "img.thumb", function () {
        var img = $(this);
        var fileid = img.attr("FileID");
        var arr = /(.*?)(?:[\?&]w=\d+)|(.*)/.exec(img.attr('src'));
        var src = arr[1] || arr[2];
        layuiExt.fLayerImg('', src);
        //top.layer.open({
        //    title: '图片显示'
        //    , skin: 'picturesshow'
        //    , area: ['80%', '80%']
        //    , content: '<img style=\'width:100%,height:100%\' src=\'/File/Thumb?fileid=' + fileid + '\' >'
        //});
    });

    //清空table的检索缓存
    $("table[module]").each(function () {
        var moduleKey = $(this).attr("module");
        layui.sessionData(moduleKey, null);
    })
})


var func = {
    typeof:function(data){
        if(arguments.length === 0) return new Error('type方法未传参');
        var typeStr = Object.prototype.toString.call(data);
        return typeStr.match(/\[object (.*?)\]/)[1].toLowerCase();      
    },
    array: {
        //扩展array的去重方法
        distinct:function () {
            return this.reduce(function (new_array, old_array_value) {
                if (new_array.indexOf(old_array_value) == -1) new_array.push(old_array_value);
                return new_array; //最终返回的是 prev value 也就是recorder
            }, []);
        }
    },
    splitDrag: function (_ul, _table, options = {}) {
        //左侧tree，右侧table的布局，
        $.extend(options, {
            left: 10,
            right: 500,
        })
        var tParent = _table.parent();
        tParent = !options.wrap ? tParent : typeof options.wrap == 'string' ? $(options.wrap) : options.wrap;
        tParent.append('<div id="r-treedrag"><div id="r-treedrag-ul">\n </div>\n <div id="r-treedrag-width"></div>\n <div id="r-treedrag-table">\n <div class="r-treedrag-table_wrap">\n </div>\n  </div>\n </div>')
        $('#r-treedrag-ul').append(_ul);
        $(".r-treedrag-table_wrap").append(_table);
        //tParent.remove();
        //宽度读取改变应在加载前完成
        var costTWidth = layui.data('manyChangeWidth')[options.key];
        if (costTWidth) {//如果之前写入过了，就读取这个值
            $('#r-treedrag-ul').width(costTWidth);
        } else {//如果没写入过，则设置初始值
            layui.data('manyChangeWidth', {
                key: options.key
                , value: '250'
            });
        }

        (function () {
            var oDiv = document.getElementById('r-treedrag-width');
            var disX = 0;

            oDiv.onmousedown = function (ev) {
                var oEvent = ev || event;
                disX = oEvent.clientX - oDiv.offsetLeft;
                var rowWrapPadding = $('.r-treedrag').outerWidth(true) - $('.r-treedrag').width() / 2;//border+padding+margin合宽
                console.log(rowWrapPadding)
                document.onmousemove = function (ev) {
                    var oEvent = ev || event;
                    var l = oEvent.clientX - disX - rowWrapPadding;//当前鼠标位置-padding宽度
                    //console.log(l)
                    if (l < options.left) {
                        l = options.left;
                    }
                    else if (l > options.right) {
                        l = options.right;
                    }
                    $('#r-treedrag-ul').width(l);

                    layui.data('manyChangeWidth', {
                        key: options.key
                        , value: l
                    });
                };

                document.onmouseup = function () {
                    document.onmousemove = null;
                    document.onmouseup = null;
                };

                return false;
            };

        })()
    },
    
    formatDate : function (now, op) {
        //type取值范围
        //var types = { 'S':8, 'M':5, 'H':2, 'Day':true,'Mounth'}
        var defaultOp = { type: 'S', split: '-' },
            op = $.extend(defaultOp, op),
            split = op.split,
            y = now.getFullYear(),
            m = now.getMonth() + 1,
            d = now.getDate();
        var ymd = y + split + (m < 10 ? "0" + m : m) + split + (d < 10 ? "0" + d : d)

        var rData;
        switch (op.type) {
            case 'S':
                rData = ymd + " " + now.toTimeString().substr(0, 8);
                break;
            case 'M':
                rData = ymd + " " + now.toTimeString().substr(0, 5);
                break;
            case 'H':
                rData = ymd + " " + now.toTimeString().substr(0, 2);
                break;
            case 'Day':
                rData = ymd;
                break;
            case 'Mounth':
                rData = (m < 10 ? "0" + m : m) + split + (d < 10 ? "0" + d : d);
                break;
            default:
                rData = ymd + " " + now.toTimeString().substr(0, 8);
        }
        return rData
    },
    judgeFileType: function (files) {
        var defaultObj = { type: 'default', iconfont:'iconfont icon-m-fileFormat' , color: '#000' }; //默认对象
        if (files instanceof Array) {
            var returnArr = [];
            files.forEach(function (f) {
                returnArr.push(fSwitch(f));
            })
            return returnArr
        } else if (typeof files == 'string') {
            return fSwitch(files);
        }
        function fSwitch(file) {//传进来一个字符串
            //预处理字符串
            if (/./g.test(file)) {//传进来的是带.的，代表是完整链接，要处理一下
                file= file.split('.');
                file = file[file.length - 1];
            }
            var obj;
            var allFiles = {
                excal: [['excal', 'docx', 'doc', 'xls', 'xlsx'], 'icon-excel', '#289be5'],
                zip: [['zip','gzip'], 'icon-yasuobao', '#289be5'],
                pdf: [['pdf'], 'icon-pdf1','#289be5'],
                txt: [['txt'], 'icon-txt','#289be5'],
                video: [['mp4', 'avi', 'WAV', 'rm', 'rmvb', 'mpeg1', 'mpeg2', 'mpeg3', 'mpeg4', 'mov', 'mtv', 'dat', 'wmv', 'avi', '3gp', 'amv', 'dmv', 'flv'], 'icon-filevideo', '#289be5'],
                cad: [['dwg', 'dwt', 'dxf', 'dws'], 'icon-CAD','#289be5'],
                jpg: ["bmp,jpg,png,tif,gif,pcx,tga,exif,fpx,svg,psd,cdr,pcd,dxf,ufo,eps,ai,raw,WMF,webp,jpeg,svg,smp".split(','), '', '#289be5'],//所有类型，图标，颜色
                //['PSD', 'PDD', 'GIF', 'JPEG', 'JPG', 'SVG', 'png', 'BMP', 'CDR']
            };
            for (n in allFiles) {
                allFiles[n][0].forEach(function (m) {
                    if (m.toLowerCase() == file.toLowerCase()) {
                        obj={ type: n, iconfont: 'iconfont '+allFiles[n][1], color: allFiles[n][2]}
                    }
                })
                //if (allFiles[n].indexOf('b') != -1) {
                //    obj = n;
                //};
            }
            if (obj) {
                return obj;
            } else {
                return defaultObj;
            }
        }
    },
    renderSimpleUpload: function (selector,options) {
        layui.upload.render({
            elem: selector,
            field: 'file',
            accept: 'file',
            exts: options.exts || null,
            acceptMime: options.acceptMime||null,
            multiple: false,
            number: 10,
            size: 204800,
            url:!options.temp? '/file/upload/':'/file/upload?temp=true'
            , choose: function (obj) {
                if (window.isReUpload) { window.isReUpload = false; return; }
                var files = this.files = obj.pushFile(); //将每次选择的文件追加到文件队列
                console.log(this.files);
                $("#uploadList").html('');
                for (var index in this.files) {
                    var file = files[index];
                    var tr = $(['<tr id="upload-' + index + '" index="' + index + '" class="uploaditem" uploaded=0>'
                        , '<td width="30%">' + file.name + '</td>'
                        , '<td width="10%">' + (file.size / 1014).toFixed(1) + 'kb</td>'
                        , '<td width="30%"><div class="layui-progress layui-progress-big" lay-showPercent="true" lay-filter="progress_' + index + '"><div class= "layui-progress-bar" lay-percent="0%" ><span class="layui-progress-text">0%</span></div></div></td>'
                        , '<td class="status" width="20%"><i class="layui-icon layui-icon-loading layui-icon layui-anim layui-anim-rotate layui-anim-loop"></i></td>'
                        , '<td>'
                        , '<button class="layui-btn layui-btn-xs demo-reload layui-hide" type="button">重传</button>'
                        , '</td>'
                        , '</tr>'].join(''));
                    tr.data("file", file);
                    //单个重传
                    tr.find('.demo-reload').on('click', function () {
                        window.isReUpload = true;//重传标记
                        var index = $(this).closest("tr").attr("index");
                        var file = $(this).closest("tr").data("file");
                        console.log("重新上传" + index);
                        $(this).addClass("layui-hide");
                        $(this).closest("tr").find(".status").html('<i class="layui-icon layui-icon-loading layui-icon layui-anim layui-anim-rotate layui-anim-loop"></i>');
                        obj.upload(index, file);
                    });

                    $("#uploadList").append(tr);
                }
                window.uploadlistlayer = layer.open({
                    type: 1,
                    shade: false,
                    btn: null,
                    title: "上传进度",
                    area: ['80%', '80%'],
                    content: $('#uploadListContainer'), //捕获的元素，注意：最好该指定的元素要存放在body最外层，否则可能被其它的相对元素所影响
                    success: function () {

                    }
                });
            }, xhr: function (index, e) {
                var percent = e.loaded / e.total;//计算百分比
                console.log(percent);
                percent = Math.round(parseFloat(percent.toFixed(2)) * 100);//解决小数点问题
                layui.element.progress('progress_' + index + '', percent + '%');
                //console.log(index+"-----" + percent);
            }
            , before: function (obj) {
                loadLayerIndex = top.layer.msg('正在拼命上传中...', {
                    icon: 16
                    , shade: false, time: 0, offset: 't'
                });
            }
            , done: function (res, index, upload) {

                //如果上传失败
                if (!res.result.success) {
                    $("#upload-" + index).attr("uploaded", 1).find(".status").html("上传失败:" + res.result.msg);
                    $("#upload-" + index).find(".demo-reload").removeClass("layui-hide");
                    layui.element.progress('progress_' + index + '', '0%');
                    layer.msg(res.result.msg, { icon: 5, anim: 6 });
                }
                //上传成功
                else {
                    console.log(index + "上传成功");
                    $("#upload-" + index).attr("uploaded", 1).find(".status").html("上传完成");
                    $("#upload-" + index).hide();
                    if (JSON.parse($.getUrlParam('multiple'))) {
                        if (!(this.dataArr instanceof Array)) {
                            this.dataArr = [];
                        }
                        this.dataArr.push(res.result);
                    } else {
                        top.layer.close(loadLayerIndex);
                        options.callback && options.callback(res.result);
                        layer.closeAll('iframe');
                    }

                    //app.currentItem.files.push({ fileName: res.result.fileName, filePath: res.result.filePath, fileType: '' });
                    delete this.files[index];
                }
                //已全部上传
                if ($(".uploaditem[uploaded=0]").size() == 0) {
                    top.layer.close(loadLayerIndex);
                    //全部成功
                    if (Object.keys(this.files).length == 0) {
                        layer.close(window.uploadlistlayer);
                    }

                }
            }, error: function (index, upload) {
                console.log("error:" + index);
                $("#upload-" + index).attr("uploaded", 1).find(".status").html("上传失败:请稍候重试");
                $("#upload-" + index).find(".demo-reload").removeClass("layui-hide");
                layui.element.progress('progress_' + index + '', '0%');
            }
            , allDone: function (res) {
                console.log('allDone');
                console.log(this.dataArr)
                top.layer.close(loadLayerIndex);
                options.callback && options.callback(this.dataArr);
                layer.closeAll('iframe');
            }
        });
    },
	//上传文件绑定
	renderUpload: function (selector, options) {
		var opt = $.extend({
            trigger: 'dblclick',
            uploadType: 'single',
            multiple:false
        }, options);
        //console.log(opt)
		$("body").on(opt.trigger, selector, function () {
			try {
				window.upload = {
					element: this,
					callback: opt.callback || ($(this).attr("callback") ? eval($(this).attr("callback")) : $.noop)
				};
				layer.open({
					type: 2,
					title: "文件上传",
					shadeClose: false,
					shade: 0.8,
                    area: ['500px','400px'],
                    content: '/File/CommonUpload?multiple='+opt.multiple
				});
			} catch(e){
				//console.log(e);
			}
			
		});
		
	},
    //获取模块的js名
    getModuleServiceName: function (name) {
        return name ? (name[0].toLowerCase() + name.substring(1)) : "moduleData";
    },
    //模块按钮事件
    callModuleButtonEvent: function (element) {
		var ev = getEvent();
		var ele = element || $(ev.srcElement || ev.target),
            moduleKey = ele.attr("modulekey"),
            layevent = ele.attr("lay-event"),
            dataid = ele.attr("dataid"),
            confirmmsg = ele.attr("confirmmsg"),
            title=ele.attr("title"),
            buttonname = ele.attr("buttonname"),
            buttonactiontype = ele.attr("buttonactiontype"),
            buttonactionparam = ele.attr("params"),
            buttonactionurl = ele.attr("buttonactionurl"),
            callback = ele.attr("callback"),
            opentop = ele.attr("opentop"),
            fornonerow = ele.attr("fornonerow");

        //提交的数据
        var data = [];
        data = layui.table.checkStatus(moduleKey).data.map(function (o) { return o.id || o.Id; });
        if (dataid) { data = [dataid]; }
		else if (!fornonerow) {
			
            if (data.length === 0) {
                abp.message.info("请先选择记录");
                return false;
            }
		}
		var hash = buttonactionurl.indexOf('#') > 0 ? buttonactionurl.split('#')[1] : '';
		var url = buttonactionurl.split('#')[0] + (buttonactionurl.indexOf("?") > 0 ? "&" : "?") + "modulekey=" + moduleKey + "&data=" + data.join(',') + '#' + hash; //url

        var funcProxyWrapper;//方法包装
        //异步提交方式
        if (buttonactiontype === "Ajax") {
            var funcProxy = eval(buttonactionurl);
            if (!funcProxy) {
                layer.alert("未找到代理方法" + buttonactionurl);
                return false;
            }

			funcProxyWrapper = function () {
				func.runAsync(funcProxy(data).done(function (data) {
                    var result=true;
                    callback && (result = eval(callback)(data));
                    //如果自定义回调返回false,则不执行默认处理
                    if (!result) { return;}
                    parent.layer.msg("提交成功");
					moduleKey && func.reload(moduleKey);
				}))
                //abp.ui.setBusy(
                //    $('body'),
                //    funcProxy(data, {
                //        success: function (data) {
                //            //abp.message.success("提交成功");
                //            parent.layer.msg("提交成功");
                //            callback && eval(callback)(data);
                //            moduleKey && func.reload(moduleKey);
                //        }
                //    })

                //);
            }

        }
        //展示窗体
        else if (buttonactiontype === "Form") {
            var defaultOption = {
                type: 2,
				title: title||buttonname,
				scrollbar: false,
                shadeClose: false,
                shade: 0.8,
                area: ['80%', '80%'],
                content: url,
				btn: ['提交', '关闭'],
				btnAlign: 'l',
                yes: function (index, layero) {
                    var iframeWin = window[layero.find('iframe')[0]['name']]; //得到iframe页的窗口对象，执行iframe页的方法：iframeWin.method();
                    if (iframeWin.submit) { iframeWin.submit(); return false; }
                },
                btn2: function (index, layero) {
                    var iframeWin = window[layero.find('iframe')[0]['name']];
                    if (iframeWin.submit2) { iframeWin.submit2(); return false; }
                },
                btn3: function (index, layero) {
                    var iframeWin = window[layero.find('iframe')[0]['name']];
                    if (iframeWin.submit3) { iframeWin.submit3(); return false; }
                }
            };
            var param = buttonactionparam ? $.parseJSON(buttonactionparam) : {};

            funcProxyWrapper = function () {
                (opentop ? top.layer : layer).open($.extend(defaultOption, param));
            }
        }
        //打开Tab
        else if (buttonactiontype === "Tab") {
            funcProxyWrapper = function () {
                top.layui.index.openTabsPage(url, buttonname);
            }
        }

        //提交
        if (confirmmsg) {
            abp.message.confirm(confirmmsg, funcProxyWrapper);
        } else {
            funcProxyWrapper();
        }
    },
    //表格重载
    reload: function (tableid, option) {
        layui.table.reload(tableid, option);
        config.refresh();
    },
    //异步执行
    runAsync: function (fun) {
        top.abp.ui.setBusy(
            null,
            fun
        );
    },
    //表单初始化
    initForm: function () {
        if (layui.data('session').loginInfo) {
            $.extend(abp.session, JSON.parse(layui.data('session').loginInfo));
        }
        $("div.layui-inline").each(function () {
            var parentNode = $(this).parent();
            var prevNode = $(this).prev();
            if (!parentNode.is(".layui-form-item")) {
                if (prevNode.is(".layui-form-item")) {
                    $(this).appendTo(prevNode);
                } else {
                    $(this).wrap('<div class="layui-form-item"></div>');
                }
            }
        })
        $("table[lay-filter]").each(function () {
            var fillterId = $(this).attr('lay-filter');
            layui.table.on('row(' + fillterId + ')', function (obj) {
                if ($(obj.tr).hasClass('my-table-active')) {
                    $(obj.tr).removeClass('my-table-active');
                } else {
                    $(obj.tr).addClass('my-table-active');
                    $(obj.tr).siblings().removeClass('my-table-active');
                }
                config.tableRowdone(obj);
            });
            layui.table.on('checkbox(' + fillterId + ')', function (obj) {
                if (obj.type == 'all') {
                    if (obj.checked) {
                        $('[lay-filter="' + fillterId + '"]').siblings('.layui-table-view').find('tbody tr').addClass('my-table-active_checked')
                    } else {
                        $('[lay-filter="' + fillterId + '"]').siblings('.layui-table-view').find('tbody tr').removeClass('my-table-active_checked')
                    }
                } else {
                    if (obj.checked) {
                        $(obj.tr).addClass('my-table-active_checked');
                    } else {
                        $(obj.tr).removeClass('my-table-active_checked');
                        $(obj.tr).removeClass('my-table-active');
                    }
                }
                config.tableCheckBoxdone(obj);
            });
        })
    },
    initUI: function ($body) {
        //元素自适应高度
		func.initLayout($body);
		//通用上传绑定;
		func.renderUpload(".uploadinsert");
    },
    //初始化元素自适应
    initLayout: function ($body) {
        var $target = $body || $("body");
        //<div layouth='130'></div>
        $target.find("[layouth]").each(function () {
            var layouth = $(this).attr("layouth");
            var h = top.$(".layui-body").height();
            //var h = $(document).height();
            h = h - parseInt(layouth);
            $(this).css("overflow-y", "auto");
            $(this).css("height", h + "px");
        })
    },
    //构建查询条件
    buildSearchCondition: function (moduleKey) {
        var conditions = layui.sessionData(moduleKey).conditions;
        if (!conditions || conditions == "") {
            return "";
        } else {
            //var conditionStr = "";
            //for (var i = 0; i < conditions.length; i++) {
            //    var condition = conditions[i];
            //    conditionStr += condition.leftBracket +' '+ condition.column.columnKey + ' ' + condition.operator + ' ' + condition.value + ' ' + condition.rightBracket + ' ' + condition.joiner+' ';
            //}
            return JSON.stringify(conditions);
        }
    },
    //查找返回,子页面调用
    bringBack: function (moduleKey, isReturn) {
        var checkStatus = layui.table.checkStatus(moduleKey);
        if (checkStatus.data.length == 0) {
            layer.msg(L('请至少选择一项'), { icon: 5, anim: 6 });
            return false;
        }
        var key = $.getUrlParam("key");
        parent.func.getBringBack(checkStatus.data, key, isReturn);
    },
    //获取返回数据,父页面调用
    getBringBack: function (data, key, isReturn) {
        //调用页面中定义的回调函数
        if (func.bringBackFuncs[key](data)) {
            //成功回调
            if (isReturn) {
                layer.closeAll('iframe');
            }
        }

        console.log(data);
    },
    bringBackFuncs: [],
    referenceDatas: []
};
/*多语种*/
abp.localization.defaultSourceName = "Master";
//修正前台权限判定，当权限定义不存在时返回true
abp.auth.isGranted = function (n) { return abp.auth.grantedPermissions[n] != undefined || !abp.auth.allPermissions[n] }
function L(name) {
    return abp.localization.localize(name);
}

function getEvent() { //同时兼容ie和ff的写法 
    if (document.all) return window.event;
    var func = getEvent.caller;
    while (func !== null) {
        var arg0 = func.arguments[0];
        if (arg0) {
            if ((arg0.constructor === Event || arg0.constructor === MouseEvent)
                || (typeof (arg0) === "object" && arg0.preventDefault && arg0.stopPropagation)) {
                return arg0;
            }
        }
        func = func.caller;
    }
    return null;
}

/*jquery扩展*/
//获取url的参数值
$.getUrlParam = function (name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
	if (r !== null) return decodeURIComponent(r[2]); return null;
}

//把name/value的数组转为obj对象
$.arrayToObj = function (array) {
    var result = {};
    for (var i = 0; i < array.length; i++) {
        var field = array[i];
        if (field.name in result) {
            result[field.name] += ',' + field.value;
        } else {
            result[field.name] = field.value;
        }
    }
    return result;
}
$.newid = function () {
    return new Date().getTime() + '' + Math.round(Math.random() * 1000);
}
function getCheckboxValue(name) {
    var data = [];
    $(":checked[name='" + name + "']").each(function () {
        data.push($(this).val());
    })
    return data;
}



function Full() {//全屏
    var fullState = $('#LAY_app_tabs').offset().left;
    console.log(fullState)
    if (fullState > 0) {
        $('#LAY_app_tabs').addClass('quanping-tabs');
        $('#LAY_app_body').addClass('quanping-body');
    } else if (fullState == 0) {
        $('#LAY_app_tabs').removeClass('quanping-tabs');
        $('#LAY_app_body').removeClass('quanping-body');
    }
}
//给全局jquery对象加一个risize方法，$('.a').resize(function(){callback});
/*
(function ($, h, c) {
    var a = $([]), e = $.resize = $.extend($.resize, {}), i, k = "setTimeout", j = "resize", d = j
        + "-special-event", b = "delay", f = "throttleWindow";
    e[b] = 350;
    e[f] = true;
    $.event.special[j] = {
        setup: function () {
            if (!e[f] && this[k]) {
                return false
            }
            var l = $(this);
            a = a.add(l);
            $.data(this, d, {
                w: l.width(),
                h: l.height()
            });
            if (a.length === 1) {
                g()
            }
        },
        teardown: function () {
            if (!e[f] && this[k]) {
                return false
            }
            var l = $(this);
            a = a.not(l);
            l.removeData(d);
            if (!a.length) {
                clearTimeout(i)
            }
        },
        add: function (l) {
            if (!e[f] && this[k]) {
                return false
            }
            var n;
            function m(s, o, p) {
                var q = $(this), r = $.data(this, d);
                r.w = o !== c ? o : q.width();
                r.h = p !== c ? p : q.height();
                n.apply(this, arguments)
            }
            if ($.isFunction(l)) {
                n = l;
                return m
            } else {
                n = l.handler;
                l.handler = m
            }
        }
    };
    function g() {
        i = h[k](function () {
            a.each(function () {
                var n = $(this), m = n.width(), l = n.height(), o = $
                    .data(this, d);
                if (m !== o.w || l !== o.h) {
                    n.trigger(j, [o.w = m, o.h = l])
                }
            });
            g()
        }, e[b])
    }
})(jQuery, this); 
*/
//格式化时间
Date.prototype.pattern = function (fmt) {
    var o = {
        "M+": this.getMonth() + 1, //月份         
        "d+": this.getDate(), //日         
        "h+": this.getHours() % 12 == 0 ? 12 : this.getHours() % 12, //小时         
        "H+": this.getHours(), //小时         
        "m+": this.getMinutes(), //分         
        "s+": this.getSeconds(), //秒         
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度         
        "S": this.getMilliseconds() //毫秒         
    };
    var week = {
        "0": "/u65e5",
        "1": "/u4e00",
        "2": "/u4e8c",
        "3": "/u4e09",
        "4": "/u56db",
        "5": "/u4e94",
        "6": "/u516d"
    };
    if (/(y+)/.test(fmt)) {
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    }
    if (/(E+)/.test(fmt)) {
        fmt = fmt.replace(RegExp.$1, ((RegExp.$1.length > 1) ? (RegExp.$1.length > 2 ? "/u661f/u671f" : "/u5468") : "") + week[this.getDay() + ""]);
    }
    for (var k in o) {
        if (new RegExp("(" + k + ")").test(fmt)) {
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        }
    }
    if (+this===0||isNaN(+this)) {
        //当new Date(null)或者new Date(undefined)时会进到这里
        return ''
    }
    return fmt;
}
Date.prototype.addDay = function (number = 1,interval='d',pattern) {
    switch (interval.toLowerCase()) {
        case "y": this.setFullYear(this.getFullYear() + number);break;
        case "m": this.setMonth(this.getMonth() + number); break;
        case "d": this.setDate(this.getDate() + number); break;
        case "w": this.setDate(this.getDate() + 7 * number); break;
        case "h": this.setHours(this.getHours() + number); break;
        case "n": this.setMinutes(this.getMinutes() + number); break;
        case "s": this.setSeconds(this.getSeconds() + number); break;
        case "l": this.setMilliseconds(this.getMilliseconds() + number); break;
    }
    return pattern ? this.pattern(pattern) : this;
}
//动态改变disabled状态
Vue.directive('disabled', function (el, binding) {
    if (binding.value) {
        $(el).addClass('layui-disabled')
        $(el).attr("disabled", true);
    } else {
        $(el).removeClass('layui-disabled')
        $(el).attr("disabled", false);
    }
})
//动态改变formSelects的disabled状态
Vue.directive('fs-disabled', function (el, binding) {
    var formSelets = layui.formSelects;
    var name = $(el).attr('xm-select');
    if (formSelets) {
        if (binding.value) {
            formSelets.disabled(name);
        } else {
            formSelets.undisabled(name);
        }
    }

})
Vue.filter('objEmptyStr', function (value,str) {
    if (!value) return '';
    return value[str]
})

function showPdf(filePath,fileName) {
    layer.open({
        type: 2,
        title: fileName,
        shadeClose: false,
        shade: 0.8,
        area: ['100%', '100%'],
        content: '/pdfviewer/web/viewer.html?file=' + filePath
    });
}