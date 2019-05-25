//当前锁定行标记
var lockIndex = '';
//var offsetN = {};
function editLock(ev) {
    var aReportDataId;
    var aOriDataId;
    //恢复原样
    function fReOri() {
        var changeFlag = false;
        if (aReportDataId && aReportDataId.length > 0) {
            aOriDataId.forEach(function (n, index) {
                if (n != aReportDataId[index]) {
                    console.log('不相等');
                    changeFlag = true;
                }
            })
        }
        if (changeFlag) {
            func.runAsync(abp.services.app.processTask.reSort(aReportDataId).done(function (data) {
                layer.msg('顺序调整成功')
            }));
        }
        $('.layui-table-main tbody tr:eq(' + tdIndex + ') [data-field="tasks"]').removeClass('lock-li')
        var obj = $('.layui-table-main tbody tr:eq(' + tdIndex + ') [data-field="tasks"] .my-tasks-li');
        lockIndex = '';
        $('.layui-table-main tbody tr:eq(' + tdIndex + ') [data-field="tasks"] .my-tasks_li-lock>i').removeClass('icon-lock1').addClass('icon-lock2');
        obj.off('mousedown', fLiClkMove);
        obj.find('.my-li-typeName').attr({ tips: '双击编辑加工单' });
        obj.find('.my-li-date').attr({ title: '点击报工' });
        obj.each(function () {
            var itemid = $(this).find('.my-li-date').attr('data-itemid');
            var ptstatus = $(this).find('.my-li-date').attr('data-ptstatus');
            $(this).find('.my-li-typeName').attr({ ondblclick: 'typeNameDbClick(this,' + itemid + ')' });
            $(this).find('.my-li-date').attr({ onclick: 'reportTimes(' + itemid + ',' + ptstatus + ')' });
        })
        obj.css({ "cursor": "Default" });
    }
    function removeLiId() {
        var that = this;
        setTimeout(fDeleteConfim, 300);
        function fDeleteConfim() {
            layer.confirm('确定删除吗？', {
                btn: ['确定', '取消'] //按钮
            }, function (index) {
                console.log('删除')
                var ids = $(that).siblings('.my-li-date').attr('data-itemid');
                console.log(ids);
                func.runAsync(abp.services.app.processTask.delTask([ids]).done(function (data) {
                    layer.msg('删除成功');
                    $(that).parent().remove();
                }));
                layer.close(index);
            }, function () {
                console.log('取消')
            });
        }

        window.event ? window.event.cancelBubble = true : event.stopPropagation();
    }

    var tdIndex = $(ev).closest('tr').index();
    console.log(tdIndex, lockIndex);
    $('.layui-table-main tbody tr:eq(' + tdIndex + ') [data-field="tasks"]').addClass('lock-li')
    var _tdObj = $('.layui-table-main tbody tr:eq(' + tdIndex + ') [data-field="tasks"] .my-tasks-li');
    if (lockIndex !== tdIndex) {
        $('.lock-li .my-li-delete').mousedown(removeLiId);
        //$('.lock-li .my-li-delete').get(0).addEventListener('mousedown', removeLiId, false);

        lockIndex = tdIndex;
        $('.layui-table-main tbody tr:eq(' + tdIndex + ') [data-field="tasks"] .my-tasks_li-lock>i').removeClass('icon-lock2').addClass('icon-lock1');

        aOriDataId = [];
        $('.icon-lock1').closest('.my-tasks-ul').find('.my-tasks-li .my-li-date').each(function () {
            aOriDataId.push($(this).attr('data-itemid'));
        });
        function fLiClkMove(ev) {
            var oEvent = ev || event;
            var bLeft = $(this).offset().left;
            //造个新的li
            $(this).parent().append('<li class="my-tasks-li dom-remove_li" >' + $(this).html() + '</li>')
            $('.dom-remove_li').offset({ left: bLeft });
            //隐藏原来的li,造出虚线li
            $(this).after('<li class="my-tasks-li dom-dashed_li"></li>');
            var distance = 80 + 15 + 4;
            disX = oEvent.clientX - bLeft;
            $(this).remove();
            document.onmousemove = function (ev) {
                var oEvent = ev || event;
                var l = oEvent.clientX - disX;
                var z = $('.dom-remove_li').closest('td').offset().left + 35;
                var z = z > 0 ? z : 0;
                var y = $('.my-tasks_li-lock').offset().left || 0 - $('.dom-remove_li').width() - 10;

                if (l < z) {
                    l = z;
                }
                else if (l > y) {
                    l = y;
                }
                //_thisObj.offset({ left: l });
                $('.dom-remove_li').offset({ left: l });
                if (l > bLeft + distance) {
                    console.log('右边超一个');
                    bLeft += distance;
                    var dashedLiIndex = $('.dom-dashed_li').index();
                    $('.dom-dashed_li').parent().find('li').eq(dashedLiIndex + 1).after($('.dom-dashed_li'));
                    $('.dom-dashed_li:first-child').remove();
                } else if (l < bLeft - distance) {
                    console.log('左边超一个')
                    bLeft -= distance;
                    var dashedLiIndex = $('.dom-dashed_li').index();
                    $('.dom-dashed_li').parent().find('li').eq(dashedLiIndex - 1).before($('.dom-dashed_li'));
                    $('.dom-dashed_li:last-child').remove();
                }
                return false;
            };
            document.onmouseup = function () {
                $('.dom-dashed_li').after($('.dom-remove_li'));
                $('.dom-dashed_li').remove();
                $('.dom-remove_li:last-child').remove();
                $('.dom-remove_li').removeClass('dom-remove_li').css('left', 0).css({ "cursor": "move" }).mousedown(fLiClkMove).find('.my-li-delete').mousedown(removeLiId);

                aReportDataId = [];
                $('.icon-lock1').closest('.my-tasks-ul').find('.my-tasks-li .my-li-date').each(function () {
                    aReportDataId.push($(this).attr('data-itemid'));
                });
                console.log(aReportDataId);

                document.onmousemove = null;
                document.onmouseup = null;
            };
            return false;
        }
        _tdObj.find(' .my-li-typeName').removeAttr('tips').removeAttr('ondblclick');
        _tdObj.find(' .my-li-date').removeAttr('title').removeAttr('onclick');
        _tdObj.css({ "cursor": "move" });
        _tdObj.mousedown(fLiClkMove);
        //_tdObj.get(0).addEventListener('mousedown', fLiClkMove,false )
        function fRemoveLock(e) {
            var e = e || window.event; //浏览器兼容性
            var elem = e.target || e.srcElement;
            if ($(elem).parents('[data-index="' + tdIndex + '"]').length != 1 && $(elem).parents('.layui-layer ').length != 1) {
                console.log('移除');
                $(document).off('click', fRemoveLock);
                fReOri();
            }
        }
        $(document).on('click', fRemoveLock);
    } else {
        fReOri()
    }

}
//选择报工
var canOpened = true;//没点击过为true
function reportTimes(Id, processTypeName) {
    if (canOpened) {
        canOpened = !canOpened;
        if (processTypeName == 0) {
            layer.msg('未开单，无法报工');
            canOpened = true;
        } else {
            abp.services.app.processTask.getTaskInfoById(Id).done(function (data) {
                app.TaskInfoData.Id = Id;
                app.TaskInfoData.startDate = data.startDate || '未报工';
                app.TaskInfoData.endDate = data.endDate || '未报工';
                var partName = data.partName || '';//零件
                var partSpecification = data.partSpecification || '';//规格
                var partNum = data.partNum || '';   //数量
                var processTypeName = data.processTypeName || '';//工艺

                //console.log(app.TaskInfoData.startDate, app.TaskInfoData.endDate)
                layer.open({
                    type: 1,
                    title: "选择报工",
                    shadeClose: false,
                    shade: 0.8,
                    area: ['500px', '500px'],
                    content: $("#dom-repTimes"),
                    btn: false,
                    end: function (index, layero) {
                        app.TaskInfoData.Id = '上次的数据';
                        app.TaskInfoData.startDate = '上次的数据';
                        app.TaskInfoData.endDate = '上次的数据';
                        app.startDateHandle = '上次的数据';
                        app, endDateHandle = '上次的数据';
                    }
                });
                canOpened = true;
                layui.laydate.render({
                    elem: '#date1'
                    , type: 'datetime'
                    , done: function (value) {
                        if (value != '') {
                            app.TaskInfoData.startDate = value;
                        }
                    }
                    //, value: app.startDateHandle=='未报工' ? '' : app.startDateHandle//当上次有数据时，这次没数据，设为value设为''，将不能顶掉上次定的数据
                });
                layui.laydate.render({
                    elem: '#date2'
                    , type: 'datetime'
                    , done: function (value) {
                        if (value != '') {
                            app.TaskInfoData.endDate = value;
                        }
                    }
                    //, value: app.endDateHandle=='未报工' ? '' : app.endDateHandle
                });
                $('#partName1').html(partName);
                $('#partSpecification1').html(partSpecification);
                $('#partNum1').html(partNum);
                $('#processTypeName1').html(processTypeName);
                $('#date1').attr('placeholder', app.startDateHandle);
                $('#date2').attr('placeholder', app.endDateHandle);
            });
            //console.log(Id);
        }
    }
}

var app;
//ondblclick = "app.loadItem({{item.id}});$(this).parent().addClass('my-tasksli-active');$(this).parent().siblings().removeClass('my-tasksli-active');"
function typeNameDbClick(obj, id) {
    app.loadItem(id);
    $(obj).addClass('my-tasksli-active');
    $(obj).parent().find('.my-li-date').addClass('my-tasksli-active');
    $(obj).parent().siblings().find('.my-li-date ').removeClass('my-tasksli-active');
    $(obj).parent().siblings().find('.my-li-typeName').removeClass('my-tasksli-active');
    //var bColor = $(obj).css("border-color");
    //if (bColor.search('0.5')) {
    //    bColor = bColor.replace('0.5', 1);
    //}
    //$(obj).css("border-color",bColor);
}
//function addRequired() {
//    var a = $("input[lay-verify='required']").parent().siblings().html()
//}
config.ready = function () {
    layui.use('formSelects', function () {
        var formSelects = layui.formSelects;
        //addRequired();
        function Arr(pName) {//构造函数，给下拉框返回的数据做处理
            this.name = pName;
            this.value = pName;
            this.selected = "";
            this.disabled = "";
        }
        function useFunction(app) {
            function fBeforeSuccess(selectId, searchVal, result) {//返回下拉框数据
                var resultArr = [];
                resultArr.push(new Arr(searchVal));//将用户输入也作为选项返回
                result = result.result;//每次进来清空数组
                //console.log(result)后台返回什么数据，我这就能拿到全部
                app.oVariable.partArr = [];
                result.forEach(function (e) {
                    if (selectId == 'partName') {
                        function ArrPartName(name, value) {
                            this.name = name + '(' + value + ')';
                            this.value = value;
                            this.selected = "";
                            this.disabled = "";
                        };
                        resultArr.push(new ArrPartName(e.partName, e.partSN))
                    } else {
                        if (e[selectId] != searchVal) {
                            resultArr.push(new Arr(e[selectId]))
                        }
                    }
                    if (e.partSpecification || e.partNum || e.partSN) {
                        function Part(partName, partSpecification, partNum, partSN, id) {
                            this.partName = partName;
                            this.partSpecification = partSpecification;
                            this.partNum = partNum;
                            this.partSN = partSN;
                            this.partId = id;
                        }
                        //console.log(e);后台返回id，partName，partNum，partSN，partSpecification
                        app.oVariable.partArr.push(new Part(e.partName, e.partSpecification, e.partNum, e.partSN, e.id))
                        //console.log(app.oVariable, e);
                    }
                });
                resultArr = resultArr.filter(function (kong) {//返回非空数组
                    return kong.name
                });
                return resultArr;
            };

            function fPersonSelect(selectId) {
                //formSelects.config(selectId, {
                //    searchUrl: sUrl,
                //    delay: 2000,
                //    beforeSuccess: function (id, url, searchVal, result) {
                //        var resultArr = [];
                //        resultArr.push(new Arr(searchVal));
                //        result = result.result;
                //        result.forEach(function (e) {
                //            resultArr.push(new Arr(e))
                //        })
                //        resultArr = resultArr.filter(function (kong) {//返回非空数组
                //            return kong.name
                //        });
                //        //console.log(resultArr)
                //        return resultArr;
                //    },
                //    success: function (id, url, searchVal, result) {
                //        //console.log(app.currentItem[id])
                //        if (!searchVal) {
                //            formSelects.value(id, [app.currentItem[id]]);
                //        }
                //        //console.log(app.currentItem[id])
                //    }
                //});
                var resultArr = [];
                abp.services.app.processTask.getHistoryPerson(selectId).done(function (data) {
                    data.forEach(function (e) {
                        resultArr.push(new Arr(e))
                    })
                    resultArr = resultArr.filter(function (kong) {//返回非空数组
                        return kong.name
                    });
                    formSelects.data(selectId, 'local', {
                        arr: resultArr
                    });
                    formSelects.value(selectId, [app.currentItem[selectId]]);  
                })
            
                formSelects.on(selectId, function (id, vals, val, isAdd, isDisabled) {
                    //$('.xm-select-parent .xm-select .xm-select-input').hide();
                    var valsD = vals.length ? vals[0].name : '';//判断点击删除时，让输入的值为空字符串
                    app.currentItem[id] = valsD;
                    //console.log(app.currentItem[id])
                },true);
            }
            var aPersonslt = ['poster', 'projectCharger', 'craftsMan', 'verifier', 'checker']
            aPersonslt.forEach(function (n) {
                fPersonSelect(n);
            })
            
            function fFormSelects(sArr) {//三个初始化
                var selectUrls = '';
                sArr.forEach(function (selectId) {
                    function fGetSUrl(sId) {
                        switch (sId) {
                            case 'projectSN':
                                sUrl = '/api/services/app/Project/GetAll';
                                break;
                            case 'processTypeName':
                                sUrl = '/api/services/app/ProcessType/GetAll';
                                break;
                            case 'unitName':
                                sUrl = '/api/services/app/Unit/GetAll';
                                break;
                            case 'equipmentSN':
                                sUrl = '/api/services/app/Equipment/GetEquipmentInfosByProcessTypeName?processTypeName=' + (app.processTypeName||'');
                                console.log(sUrl)
                                break;
                            default:
                                sUrl = '';
                        };
                        return sUrl
                    }
                    
                    var fFormSelectsConfig;
                    (fFormSelectsConfig = function () {
                        selectUrls = fGetSUrl(selectId);
                        formSelects.config(selectId, {
                            searchName: 'key',
                            searchUrl: selectUrls,
                            beforeSuccess: function (id, url, searchVal, result) {//接到后台返回的数据result后处理输出到下拉框中
                                if (id == 'projectSN') {
                                    app.oVariable.projectArr = result.result;
                                }
                                if (id == 'equipmentSN') {
                                    app.oEquipment.aequipmentSNMsg = result.result;
                                }
                                return (fBeforeSuccess(selectId, searchVal, result));
                            },
                            success: function (id, url, searchVal, result) {
                                if (!searchVal) {
                                    formSelects.value(selectId, [app.currentItem[id]]);
                                }

                            }
                        })
                    })();
                    formSelects.on(selectId, function (id, vals, val, isAdd, isDisabled) {
                        if (vals.length == 0) {
                            fFormSelectsConfig();
                        }
                        var valsD = vals.length ? vals[0].name : '';//判断点击删除时，让输入的值为空字符串
                        if (selectId == 'unitName') {
                            date2Render();
                            $('#dom-unitnameall ul li').removeClass('opened');
                        }
                        app.currentItem[id] = valsD;
                    }, true);
                })
            };
            fFormSelects(['processTypeName', 'projectSN', 'unitName','equipmentSN']);
            function fPTSeaUrl() {
                var pTSeaUrl;
                if (app.currentItem['projectSN']) {
                    pTSeaUrl = '/api/services/app/Part/GetAll?projectSN=' + app.currentItem['projectSN'];
                } else {
                    pTSeaUrl = '';
                }
                return pTSeaUrl;
            }
            var partTaskSearchUrl = fPTSeaUrl();
            
            var fpartNmFSelConfig;
            (fpartNmFSelConfig = function () {
                formSelects.config('partName', {//零件名称特殊赋值
                    searchName: 'key',
                    searchUrl: partTaskSearchUrl,
                    //(function () { return '/api/services/app/Part/GetAll' })(), 需要自执行，上午的时候返回的是函数，当然不行，猜测自执行是能够成功的，弃用的原因是data赋值给app.currentItem与下拉框请求数据都是异步的，无法确定能取到app.currentItem的值
                    beforeSuccess: function (id, url, searchVal, result) {
                        //console.log("a")
                        var resultArr = [];
                        if (partTaskSearchUrl) {
                            resultArr = fBeforeSuccess('partName', searchVal, result);
                        } else {
                            resultArr.push(new Arr(searchVal));
                            resultArr = resultArr.filter(function (kong) {//返回非空数组
                                return kong.name
                            });
                        }
                        return resultArr;
                    },
                    success: function (id, url, searchVal, result) {
                        if (!searchVal) {
                            console.log(app.currentItem.partSN)
                            formSelects.value('partName', [app.currentItem.partSN]);
                        }

                        //console.log(app.currentItem[id])
                    }
                })
                console.log(partTaskSearchUrl)
            })();
            formSelects.on('partName', function (id, vals, val) {//在零件名称点击时，给规格和数量赋值;删除时为空值也会触发
                //console.log(vals[0])
                if (vals.length == 0) {
                    formSelects.config('partName', {
                        searchUrl: fPTSeaUrl(),
                    })
                }
                var valsD = vals.length ? vals[0].name : '';//当前已选中的值
                var valsV = vals.length ? vals[0].value : '';
                var partIded = false;//后台是否有值；
                app.oVariable.partArr.forEach(function (n) {
                    if (valsD.search(n.partSN)!=-1) {
                        //app.oVariable.partId = n.partId;
                        abp.services.app.processTask.getCraftsManByPartId(n.partId).done(function (data) {
                            app.currentItem.craftsMan = data || '';
                            formSelects.value('craftsMan', [app.currentItem.craftsMan]); 
                            //fPersonSelect('craftsMan');
                            console.log(n.partId, data, app.currentItem.craftsMan )
                        });
                    }
                })
                app.currentItem[id] = valsD;
                app.currentItem.partSN = '';
                app.oVariable.partArr.forEach(function (e) {
                    //if (valsD == e.partName) {
                    //app.currentItem.partSpecification = e.partSpecification;
                    //app.currentItem.partNum = e.partNum;
                    // }
                    if (valsV == e.partSN) {//由于新增的value为**_，后台来的value为partSN；只有在选中内容是后台返回数据时，才运行下面
                        app.currentItem.partSpecification = e.partSpecification;
                        app.currentItem.partNum = e.partNum;
                        app.currentItem.partSN = e.partSN;
                        console.log(app.currentItem.partSN,'111')
                        //app.currentItem.partId = e.partId;
                        loadPartTasks(e.partId);
                        partIded = true;
                    }
                    console.log(e,app.currentItem[id]+'0', valsD+'1',valsV+'2',e.partSN)
                });
                if (!partIded) {
                    //app.currentItem.partName = valsD;
                    loadPartTasks(-1);
                    app.currentItem.partSpecification = '';
                    app.currentItem.partNum = 1;
                }
                console.log( app.currentItem.partSN, valsD)
            }, true)
            formSelects.on('projectSN', function (id, vals) {
                if (vals.length == 0) {
                    formSelects.config('projectSN', {
                        searchUrl: '/api/services/app/Project/GetAll',
                    })
                }
                var valsD = vals.length ? vals[0].name : '';
                app.oVariable.projectArr.forEach(function (n) {
                    //console.log(n,valsD)
                    if (n.projectSN == valsD) {
                        //app.oVariable.projectId = n.id;
                        //GET /api/services/app/ProcessTask/GetProjectChargerByProjectId
                        abp.services.app.processTask.getProjectChargerByProjectId(n.id).done(function (data) {
                            console.log(n.id,data)
                            app.currentItem.projectCharger = data || '';//是否要设默认空值

                            formSelects.value('projectCharger', [app.currentItem.projectCharger]); 
                            //fPersonSelect('projectCharger');
                        });
                    }
                })
                app.currentItem[id] = valsD;
                var projectSN = valsD ? valsD : '_';
                formSelects.config('partName', {
                    searchName: 'key',
                    searchUrl: '/api/services/app/Part/GetAll?projectSN=' + projectSN,
                    beforeSuccess: function (id, url, searchVal, result) {
                        return (fBeforeSuccess('partName', searchVal, result));
                    }
                })

                app.currentItem.partSN = '';
                app.currentItem.partName = '';
            }, true)
            formSelects.on('feeType', function (id, vals, val, isAdd, isDisabled) {
                //var a=Number(vals[0].value);
                var valsD = vals.length ? Number(vals[0].value) : 1;//判断点击删除时，让输入的值为空字符串
                //app.currentItem.feeType = valsD;
                app.$set(app.currentItem, 'feeType', valsD);
                //console.log(app.currentItem, vals[0].value);
            }, true);
            formSelects.render('partName', {
                template: function (name, value, selected, disabled) {
                    //console.log(value)//value是一个object，有name，innerHTML,value,selected,disable等属性
                    //console.log(value, name, selected, disabled, app.oVariable.partArr)
                    //return name.split('').reverse().join('');
                    var temp = { name: value.name, partSpecification: '', partSN: '' };//初始赋值为输入的值
                    app.oVariable.partArr.forEach(function (e) {
                        if (e.partSN == value.value) {
                            temp.name = e.partName;//这句话不加也行，只需得到当前对应的partSpecification值
                            temp.partSpecification = e.partSpecification;
                            temp.partSN = '(' + e.partSN + ')';
                        }
                    })
                    return '<span class="partname-span">' + temp.name + '</span><span class="partname-span" style="font-size:12px;">' + temp.partSN + '</span><span class="partname-span" style="float:right">' + temp.partSpecification + '</span>'
                }
            })
            function date2Render() {//让预约日期默认置为明天
                //var myDate = new Date();
                //var date = myDate.getDate() + 1;
                //var mouth = myDate.getMonth() + 1;
                //var year = myDate.getFullYear();
                //var tDate = year + '-' + mouth + '-' + date;
                //console.log(tDate);

                function GetDateStr(startDay, AddDayCount) {
                    var dd =startDay? new Date(startDay):new Date();
                    dd.setDate(dd.getDate() + AddDayCount);//获取AddDayCount天后的日期
                    var y = dd.getFullYear();
                    var m = (dd.getMonth() + 1) < 10 ? "0" + (dd.getMonth() + 1) : (dd.getMonth() + 1);//获取当前月份的日期，不足10补0
                    var d = dd.getDate() < 10 ? "0" + dd.getDate() : dd.getDate();//获取当前几号，不足10补0
                    return y + "-" + m + "-" + d;
                }  
                var tDate = GetDateStr('', 1);
                layui.laydate.render({
                    elem: '#date2',
                    value: tDate,
                    done: function (value) {
                        app.currentItem.appointDate = value;
                    }
                });
                app.currentItem.appointDate = tDate;
            }
            layui.use('flow', function () {
                var flow = layui.flow;
                var typeNameValue = '';
                var typeNameValue_Last = 'all';
                var unitId = '';
                var unitId_last = '';
                var aMessegeArr = [];
                formSelects.opened('unitName', function (id) {
                    var selectValue = formSelects.value('processTypeName');
                    typeNameValue = selectValue.length > 0 ? selectValue[0].name : '';//现在工艺的值
                    if (typeNameValue != typeNameValue_Last) {
                        unitId = 'dom-unitname' + (typeNameValue == '' ? 'all' : typeNameValue.charCodeAt());
                        console.log(unitId)
                        $('#' + unitId_last).remove();
                        $(`<div class="my-unitname-cloud" id="${unitId}"><b id="my-unitnamecld-b">云设备</b><ul id="dom-unitnamecld_ul"></ul></div>`).appendTo($(`dl[xid="unitName"]`).parent())
                        flowLoad();
                        fLiHover();
                        fLiClick();
                    } else {
                        console.log('相等');
                    }
                    typeNameValue_Last = typeNameValue;
                    unitId_last = unitId;
                    $('#' + unitId).show();
                    //$('#'+unitId).addClass('my-unitnamecld_opened');
                    function flowLoad() {
                        var itemName_last;
                        function MessegeCreate(companyName, mobile, address, speciality) {
                            this.companyName = companyName;
                            this.mobile = mobile;
                            this.address = address;
                            this.speciality = speciality;
                        }
                        flow.load({
                            elem: '#dom-unitnamecld_ul' //指定列表容器
                            , scrollElem: '#' + unitId
                            , done: function (page, next) { //到达临界点（默认滚动触发），触发下一页
                                var lis = [];
                                abp.services.app.cloudEquipment.getFreeEquipmentByKey(typeNameValue, page).done(function (res) {
                                    var largePage = Math.floor(res.count / 50);
                                    console.log(largePage, page, '111', res.data)
                                    layui.each(res.data, function (index, item) {
                                        if (item.companyName != itemName_last) {//如果不等于上一个则存储在lis，把信息存在aMessegeArr
                                            lis.push(`<li class="dom-unitnamecld_li" title="${item.companyName}"><i class="xm-iconfont"></i><span class="unitcld-li_span">${item.companyName} </span></li>`);
                                            aMessegeArr.push(new MessegeCreate(item.companyName, item.mobile, item.address, item.speciality));
                                        }
                                        itemName_last = item.companyName;
                                    });
                                    next(lis.join(''), page < largePage);
                                });
                            }
                        });
                    };
                    //添加li的hover事件
                    function fLiHover() {
                        $('#dom-unitnamecld_ul').on('mouseenter', 'li', function () {
                            var liKey = $(this).find('span').html();
                            var hoveredLi = aMessegeArr.filter(function (o) {
                                return (o.companyName.trim() == liKey.trim())
                            });
                            //console.log(hoveredLi);
                            app.liMesseage.companyName = hoveredLi[0].companyName;
                            app.liMesseage.mobile = hoveredLi[0].mobile;
                            app.liMesseage.address = hoveredLi[0].address;
                            app.liMesseage.speciality = hoveredLi[0].speciality;
                            $('#hoveredLi').show()
                        });
                    }
                    //添加点击事件
                    function fLiClick() {
                        function cldHovLiClick() {
                            date2Render();
                            //formSelects.search('unitName', app.liMesseage.companyName);
                            //formSelects.value('unitName', [app.liMesseage.companyName]);
                            var nowComName = app.liMesseage.companyName;
                            formSelects.value('unitName', []);
                            app.currentItem.unitName = nowComName;//让数据有
                            //$("input[name='unitName']").val(nowComName)//不再需要验证加工点
                            $('.dom-span-magic').remove();
                            $('div[fs_id="unitName"] .xm-select-label input').before($(`<span class="dom-span-magic" fsw="xm-select" value="${nowComName}">
                                        <font fsw="xm-select">${nowComName}</font>
                                        <i fsw="xm-select" class="xm-iconfont icon-close"></i>
                                        </span>`))
                            $(this).siblings().removeClass('opened');
                            $(this).addClass('opened');
                            $('#' + unitId_last).hide();
                            $('#hoveredLi').hide();
                        }
                        $('#dom-unitnamecld_ul').on('click', 'li', cldHovLiClick);
                        $('#hoveredLi').on('click', cldHovLiClick);

                    }
                });
                //监听关闭时，隐藏云设备
                function fCloseRemove() {
                    formSelects.closed('unitName', function (id) {
                        $('#' + unitId_last).hide();
                        $('#hoveredLi').hide();
                    });
                    //在其他selecct打开时，监听unitName关闭会失效
                    function fBugFormOpened(item) {
                        item.forEach(function (s) {
                            formSelects.opened(s, function (id) {
                                $('#' + unitId_last).hide();
                                $('#hoveredLi').hide();
                            })
                        })
                    };
                    fBugFormOpened(['projectSN', 'partName', 'processTypeName','equipmentSN']);
                }
                fCloseRemove();
            })
        }
        //--------------------------------------------------------------------------------------------
        var form = layui.form;
        form.verify({
            passnumber: [
                /^(\s*|\d+)$/
                , '只能填写数字'
            ]
        });
        form.on("switch(emergency)", function (data) {
            if (data.elem.checked) {
                layui.layer.tips('为避免滥用，建议一天最高5次', data.othis)
            }
            app.currentItem.emergency = data.elem.checked;
        });
        form.on("switch(bFactory)", function (data) {
            if (data.elem.checked) {
                app.currentItem.unitName = '厂内';
                layui.layer.tips('自动选择加工点为厂内', data.othis)
            } else {
                app.currentItem.unitName = '';
            };
            app.oEquipment.bFactory = data.elem.checked;
        });
        form.on("submit(submit)", function () { app.fSubmitForm() });
        form.on("submit(submitNext)", function () {
            console.log(app.currentItem);
            func.runAsync(abp.services.app.processTask.submitProcessTask(app.currentItem).done(function (data) {
                //返回的是加工单id
                app.currentItem.id = 0;
                loadPartTasks(app.currentItem.partId);
                layer.msg("提交成功");
                //app.loadItem(data);
                //location.reload();
                function clearCurrentItem(cuItem) {
                    cuItem.forEach(function (el) { app.currentItem[el] = '' });
                };
                //在下一道清空页面
                clearCurrentItem(['feeFactor', 'price', 'estimateHours', 'jobFee', 'appointDate', 'requireDate', 'unitName', 'processTypeName','equipmentSN']);
                app.currentItem.feeType = 1;
                app.currentItem.taskInfo = '参考附件';
                app.currentItem.files = [];
                app.currentItem.sheetFile = {};
                app.currentItem.emergency = false;
                refresh();

            }));

            //layui.use('formSelects', useFunction);
        });
        app = new Vue({
            el: '#app',
            data: {
                currentItem: {
                    partNum: 1, files: [], emergency: false, partSpecification: '', partSN: '', taskInfo: '参考附件', feeType: 1, feeFactor: '', price: 0, jobFee: 0, estimateHours: '', sheetFile: {}, requireDate: '', appointDate: '', poster: '', projectCharger: '', craftsMan: '', verifier: '', checker: '',equipmentSN:'',unitName:''
                },
                oVariable: { partArr: [], projectArr: [], projectId: '', partId: '' },
                //a:[],
                liMesseage: { companyName: '天网', mobile: '', address: '请选择云加工点', speciality: '' },
                oEquipment: { bFactory: false, aEquipmentSNMsg:[]}//{厂内外:false为厂外,请求回来到的设备编号及其附带的信息}
                
            },
            computed: {
                FeeTypeContent: function () {
                    var FTcontent;
                    console.log('改变', this.currentItem.feeType)
                    switch (this.currentItem.feeType) {
                        case 0:
                            this.currentItem.price = 0;
                            FTcontent = { PriceName: '承包金额', feeFactor: false, feeFactorName: '' };
                            break;
                        case 1:
                            this.currentItem.jobFee = Number(this.currentItem.price) * Number(this.currentItem.estimateHours);
                            console.log(Number(this.currentItem.price), this.currentItem.estimateHours, Number(this.currentItem.estimateHours))
                            FTcontent = { PriceName: '加工单价', feeFactor: false, feeFactorName: '' };
                            break;
                        case 2:
                            this.currentItem.jobFee = Number(this.currentItem.price) * Number(this.currentItem.feeFactor);
                            FTcontent = { PriceName: '加工单价', feeFactor: '平方', feeFactorName: 'm²' };
                            break;
                        case 3:
                            this.currentItem.jobFee = Number(this.currentItem.price) * Number(this.currentItem.feeFactor);
                            FTcontent = { PriceName: '加工单价', feeFactor: '长度', feeFactorName: '米' };
                            break;
                        case 4:
                            this.currentItem.jobFee = Number(this.currentItem.price) * Number(this.currentItem.feeFactor);
                            FTcontent = { PriceName: '加工单价', feeFactor: '重量', feeFactorName: 'Kg' };
                            break;
                        case 5:
                            this.currentItem.jobFee = Number(this.currentItem.price) * Number(this.currentItem.feeFactor);
                            FTcontent = { PriceName: '加工单价', feeFactor: '数量', feeFactorName: '单位' };
                            break;
                        default:
                            this.currentItem.jobFee = Number(this.currentItem.price) * Number(this.currentItem.feeFactor);
                            FTcontent = { PriceName: '加工单价', feeFactor: false };
                    };
                    console.log(this.currentItem.jobFee, this.currentItem.price, this.currentItem.feeFactor)
                    return FTcontent;
                },
                isDefault: function () {
                    return this.currentItem.taskInfo === '参考附件'
                }
            },
            watch: {
                currentItem: {
                    handler: function (val, oldVal) {
                        console.log('监测到currentItem变化');
                        if (formSelects.value('feeType', 'val')[0] != app.currentItem.feeType) {
                            formSelects.value('feeType', [app.currentItem.feeType.toString()]);
                        }
                        formSelects.value('unitName', [app.currentItem.unitName]);
                        formSelects.value('processTypeName', [app.currentItem.processTypeName]);
                    },
                    deep: true
                },
            },
            methods: {
                flyOpenImg: function () {
                    layuiExt.fLayerImg('', this.currentItem.sheetFile ? this.currentItem.sheetFile.filePath : null);
                },
                fSubmitForm: function (fn) {//提交函数
                    var fn = fn || function () { }
                    var that = this;
                    console.log($("input[name='unitName']").val())
                    console.log(this.currentItem)
                    func.runAsync(abp.services.app.processTask.submitProcessTask(this.currentItem).done(function (data) {
                        //返回的是加工单id
                        that.currentItem.id = data.id;
                        that.currentItem.partId = data.partId;
                        that.currentItem.partSN = data.partSN;
                        loadPartTasks(that.currentItem.partId);
                        layer.msg("提交成功");
                        fn();
                        //app.loadItem(data);
                        //location.reload();
                    }));
                },
                removeFile: function (index) {
                    abp.message.confirm('确认删除此项?', function () {
                        app.currentItem.files.splice(index, 1);
                    });

                },
                loadItem: function (taskid) {
                    var that = this;
                    //console.log(taskid)
                    if (!taskid) {
                        abp.services.app.processTask.getLastTaskPersons().done(function (data) {
                            var fuzhiArr = ['poster', 'verifier', 'checker'];
                            fuzhiArr.forEach(function (str) {
                                that.currentItem[str] = data[str];
                                //formSelects.value(str, [that.currentItem[str]]);
                                //console.log(that.currentItem[str], formSelects.value(str))
                            })
                            console.log(data);
                            useFunction(that);
                        });
                        
                        //layui.use('formSelects', useFunction);
                        return;
                    }
                    func.runAsync(abp.services.app.processTask.getTaskInfoById(taskid).done(function (data) {//读数据
                        if (that.currentItem.partId != data.partId) {
                            console.log(data.partId)
                            loadPartTasks(data.partId);
                        }
                        if (data.taskInfo == null) {
                            data.taskInfo = that.currentItem.taskInfo;
                        }
                        that.currentItem = data;
                        if (data.unitName == '厂内') {
                            that.oEquipment.bFactory = true;
                        }
                        useFunction(that);
                        //加载零件的加工线路
                        //layui.use('formSelects', useFunction);
                        //$('select[xm-select="projectSN"]').attr("disabled", "disabled");
                        //在有taskid时，禁用模具编号和零件编号，并重新给他两添加title
                        formSelects.disabled('projectSN');
                        formSelects.disabled('partName');
                        var projectSNTitle = data.projectSN;
                        var partNameTitle = data.partName + '(' + data.partSN + ')';
                        $('#projectSNSelect').attr('title', projectSNTitle);
                        $('#partNameSelect').attr('title', partNameTitle);

                        frenderDate(that);
                        //formSelects.value('feeType', [that.currentItem.feeType]); 
                        console.log(that.currentItem)
                        refresh();
                    }));
                }
            },
            created: function () {
                //todo:如果有参数taskid传入说明是编辑页

                var taskid = $.getUrlParam("taskid");
                this.loadItem(taskid);
                $("#app").removeClass("layui-hide");
                var that = this;
                //url: "http://localhost:62114/api/services/app/ProcessTask/GetLastTaskPersons",
                //五个人员赋值
                
                refresh();
            }
        });
        //加工图片上传
        func.renderUpload("#sheetFile", {
            trigger: 'click', callback: function (data) {
                app.currentItem.sheetFile = data;
            }
        })
        //上传配置
        layui.upload.render({
            elem: '#uploadBtn',
            field: 'file',
            accept: 'file',
            multiple: true,
            number: 10,
            size: 20480,
            url: '/file/upload/'
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
                    app.currentItem.files.push({ fileName: res.result.fileName, filePath: res.result.filePath, fileType: '' });
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
                top.layer.close(loadLayerIndex);

            }
        });
        //日期配置
        function frenderDate(thatapp) {
            var rapp = thatapp || app;
            layui.laydate.render({
                elem: '#date1',
                value: rapp.currentItem.requireDate || '',
                done: function (value) {
                    rapp.currentItem.requireDate = value;
                }
            });
            if (!rapp.currentItem.requireDate) {
                $('#date1').val('')
            }
            layui.laydate.render({
                elem: '#date2',
                value: rapp.currentItem.appointDate || '',
                done: function (value) {
                    rapp.currentItem.appointDate = value;
                }
            });
            if (!rapp.currentItem.appointDate) {
                $('#date2').val('')
            }
        }
        frenderDate();

    })
    //加工路线
    layui.table.render({
        elem: '#PartTask' //指定原始表格元素选择器（推荐id选择器）
        , cols: [[
            { align: 'center', field: 'partName', width: '300', fixed: 'left', templet: '#partName', style: 'height:95px', title: '零件' },
            { align: 'center', field: 'tasks', minWidth: '1500', title: '工艺流程', templet: '#tasks' },
            { align: 'center', field: 'caoz', toolbar: '#toolbar', title: '操作' }
        ]] //设置表头
        , defaultToolbar: []
        , cellMinWidth: '80', where: { where: '1>2' }
        , page: false
        , skin: 'row'
        , even: true
        , done: config.onTableDone
        , url: '/api/services/app/Part/GetTaskPageResult'
        , parseData: function (res) { //res 即为原始返回的数据
            //console.log(res)
            //return {
            //    "code": res.status, //解析接口状态
            //    "msg": res.message, //解析提示文本
            //    "count": res.total, //解析数据长度
            //    "data": res.data.item //解析数据列表
            //};
        }
    });
}
//上传图片
config.onTableDone = function () {
    func.renderUpload('.picupload', {
        trigger: 'click', callback: function (res) {
            var item = $(upload.element);
            console.log(item);
            console.log(res);
            var partid = item.attr("partid");
            func.runAsync(abp.services.app.part.setPartImg(partid, res.filePath).done(function () {
                item.find('img').attr('src', res.filePath)
                //item.append("<img src='"+res.result.filePath+"'/>");
            }));
        }
    })
    //layui.upload.render({
    //    elem: '.picupload',
    //    size: 1024,
    //    url: '/file/upload/'
    //    , done: function (res, index, upload) {
    //        //获取当前触发上传的元素，一般用于 elem 绑定 class 的情况，注意：此乃 layui 2.1.0 新增
    //        var item = this.item;
    //        //如果上传失败
    //        if (!res.result.success) {
    //            layer.msg(res.result.msg, { icon: 5, anim: 6 });
    //        }
    //        //上传成功
    //        else {

    //            console.log(index + "上传成功");
    //            var partid = item.attr("partid");
    //            func.runAsync(abp.services.app.part.setPartImg(partid, res.result.filePath).done(function () {
    //                item.find('img').attr('src', res.result.filePath)
    //                //item.append("<img src='"+res.result.filePath+"'/>");
    //            }));

    //        }
    //    }
    //})

}
function refresh() {
    Vue.nextTick(function () {
        layui.form.render();
    })
}
//预览
function doPreview() {
    //if (!app.currentItem.id  ) {
    //    abp.message.error("请先保存再进行预览");
    //    return false;
    //}
    if (!app.currentItem.unitName) {
        abp.message.error("请先填写加工点再进行预览");
        return false;

    }
    console.log(app.currentItem)
    app.fSubmitForm(function () {
        layer.open({
            type: 2,
            title: "加工单预览",
            shadeClose: false,
            shade: 0.8,
            area: ['100%', '100%'],
            content: "/Home/Show?name=../MES/SheetView&taskid=" + app.currentItem.id,
            btn: false,
            cancel: function () {
                app.loadItem(app.currentItem.id);
            }
        })
    });
}
//加载零件的加工路线
function loadPartTasks(partId) {
    var where = { where: 'id=' + partId };
    layui.table.reload('PartTask', {
        where: where

    })

}
//添加工序
function addProcess(partId) {
    layer.open({
        type: 2,
        title: "选择工序",
        shadeClose: false,
        shade: 0.8,
        area: ['500px', '550px'],
        content: "/Home/Show?name=../MES/ProcessTypeChoose&partId=" + partId,
        btn: false
    });
    //选择工序回调
    //window.selProcessTypeCallback = function (processTypeId) {
    //    func.runAsync(abp.services.app.processTask.submitSimpleProcessType(partId, processTypeId).done(function () {
    //        layui.table.reload('PartTask');

    //    }));

    //}

}