//editLock调整顺序
//选择报工


var app;
//ondblclick = "app.loadItem({{item.id}});$(this).parent().addClass('my-tasksli-active');$(this).parent().siblings().removeClass('my-tasksli-active');"
//function typeNameDbClick(obj, id) {//双击切换加工单
//    app.loadItem(id);
//    $(obj).addClass('my-tasksli-active');
//    $(obj).parent().find('.my-li-date').addClass('my-tasksli-active');
//    $(obj).parent().siblings().find('.my-li-date ').removeClass('my-tasksli-active');
//    $(obj).parent().siblings().find('.my-li-typeName').removeClass('my-tasksli-active');
//    //var bColor = $(obj).css("border-color");
//    //if (bColor.search('0.5')) {
//    //    bColor = bColor.replace('0.5', 1);
//    //}
//    //$(obj).css("border-color",bColor);
//}
//function addRequired() {
//    var a = $("input[lay-verify='required']").parent().siblings().html()
//}
config.ready = function () {
    Vue.component('lay-switch', {//单选开关的组件
        mounted: function () {
            var form = layui.form;
            var that = this;
            form.render();
            form.on("switch(" + this.filter +")", function (data) {
                that.$emit('on-change', data.elem.checked)
            });
        },
        watch: {
            value: function (newV, oldV) {
                this.$nextTick(function () {
                    layui.form.render();
                });
            },
            disabled: function () {
                //console.log(arguments)
            }
        },
        //value:布尔值，默认选不选中，当外界的值改变，传进来的值也会改变，从而改变选中状态
        //filter:名字，在外层显示的input中的lay-filter的值，当用layui的方法监听此input改变时，好像组件本身的方法不起作用了
        //disabled:布尔值
        props: ['value', 'filter', 'disabled','laytext'],
        template: `
        <input v-if="disabled" disabled type="checkbox" lay-skin="switch" :lay-filter="filter" :checked="value==true" :lay-text="laytext">
        <input v-else type="checkbox" lay-skin="switch" :lay-filter="filter" :checked="value==true" :lay-text="laytext">`
    })
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
                var bStric3 = !(abp.setting.getBoolean("MES.JGKDStrictMode") && ['processTypeName', 'projectSN', 'unitName'].indexOf(selectId) != -1);//严格开单模式下的零件，模号，加工点都不允许直接填，得在bom中导入
                if (bStric3) {
                    resultArr.push(new Arr(searchVal));//将用户输入也作为选项返回
                }
                result = result.result;//每次进来清空数组
                //console.log(result)后台返回什么数据，我这就能拿到全部
                if (selectId == 'partName') {
                    app.oVariable.partArr = [];
                }
              
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
                        if (e[selectId] != searchVal) {//为了排重
                            resultArr.push(new Arr(e[selectId]))
                        } else if (!bStric3) {//当是严格开单模式时，查到的就有搜索项，将他添加回来
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
            function fPropertyDataSelect(selectId) {             
                var resultArr = [];
                abp.services.mouldtry.mouldTry.getHistory(selectId).done(function (data) {
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
                }, true);
            }
            function fDataSelect(selectId) {
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
                abp.services.mouldtry.mouldTry.getHistory(selectId).done(function (data) {
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
            var aPersonslt = ['tryPerson','tryColor','tryMaterial']//'poster',
            aPersonslt.forEach(function (n) {
                fDataSelect(n);
            })


            function fOperateSelect(selectId) {  
                var resultArr = [];
                abp.services.mouldtry.mouldTry.getOperateInfo(selectId).done(function (data) {
                    console.log("1649" + data);
                    data.forEach(function (e) {                        
                        resultArr.push(new Arr(e))
                    })
                    resultArr = resultArr.filter(function (kong) {//返回非空数组
                        return kong.name
                    });
                    formSelects.data(selectId, 'local', {
                        arr: resultArr
                    });
                    console.log(selectId+app.currentItem[selectId])
                    formSelects.value(selectId, [app.currentItem[selectId]]);
                })

                formSelects.on(selectId, function (id, vals, val, isAdd, isDisabled) {
                    //$('.xm-select-parent .xm-select .xm-select-input').hide();
                    var valsD = vals.length ? vals[0].name : '';//判断点击删除时，让输入的值为空字符串
                    app.currentItem[id] = valsD;
                    //console.log(app.currentItem[id])
                }, true);
            }
            var aOperateslt = ['toDemould', 'wkRequire', 'coolingWater']//'poster',
            aOperateslt.forEach(function (n) {
                fOperateSelect(n);
            })

            function fMaterialSelect(selectId) {
                var resultArr = [];
                abp.services.mouldtry.mouldTry.getMaterialInfo(selectId).done(function (data) {
                    //console.log("1649" + data);
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
                    //console.log(app.currentItem[id]) , 'materialOut_New', 'materialOut_Old', 'materialReturn_New', 'materialReturn_Old', 'materialUsed_New', 'materialUsed_Old'
                }, true);
            }
            var aMaterialslt = ['material', 'materialBack']//'poster',
            aMaterialslt.forEach(function (n) {
                fMaterialSelect(n);
            })
            //function fReasonSelect(selectId) {

            //    var resultArr = [];
            //    var arr1 = abp.setting.get('MES.JGKDReason').split(',');
            //    arr1.forEach(function (n){
            //        //    resultArr.push(rea);
            //        resultArr.push({"name":n,"value":n})
            //    })
            //    console.log("???"+resultArr)
            //    formSelects.data(selectId, 'local', {
            //        arr: resultArr
            //    });
            //    console.log("???？" + app.currentItem[selectId])
            //    formSelects.value(selectId, [app.currentItem[selectId]]);
            //    formSelects.on(selectId, function (id, vals, val, isAdd, isDisabled) {
            //        //$('.xm-select-parent .xm-select .xm-select-input').hide();
            //        var valsD = vals.length ? vals[0].name : '';//判断点击删除时，让输入的值为空字符串
            //        app.currentItem[id] = valsD;
            //        //console.log(app.currentItem[id])
            //    }, true);
            //}
            //var aReasonslt = ['reason']
            //aReasonslt.forEach(function (n) {
            //    console.log(n);
            //    fReasonSelect(n);
            //})
            formSelects.on('tryMachine', function (id, vals, val, isAdd, isDisabled)
            {
                var valsD = vals.length ? vals[0].value : '';
                console.log("t" + valsD)
                app.currentItem['tryMachine'] = valsD;
                //Vue.set(app.currentItem, 'feeType', valsD);
            },true)
            formSelects.on('tn', function (id, vals, val, isAdd, isDisabled) {
                var valsD = vals.length ? vals[0].name : '';
                if (valsD != '') {
                    var a = parseInt(valsD.substring(1));
                console.log(a);
                    app.currentItem['tn'] = a;
                }
            },true)
                // formSelects.on('tn', function (id, vals, val, isAdd, isDisabled) {
                //    app.currentItem['tn'] = 2;
                //})
            function fFormSelects(sArr) {//三个初始化
                var selectUrls = '';
                sArr.forEach(function (selectId) {
                    function fGetSUrl(sId) {
                        switch (sId) {
                            case 'projectSN':
                                sUrl = '/api/services/app/Project/GetAll?take=200';
                                break;
                            //case 'processTypeName':
                            //    sUrl = '/api/services/app/ProcessType/GetAll';
                            //    break;
                            case 'unitName':
                                sUrl = '/api/services/app/Unit/GetAllBySupplierType?SupplierType=加工';
                                break;
                            //case 'equipmentSN':
                            //    sUrl = '/api/services/app/Equipment/GetEquipmentInfosByProcessTypeName?processTypeName=' + (app.processTypeName||'');
                            //    //console.log(sUrl)
                            //    break;
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
                                //if (id == 'processTypeName' || id == 'equipmentSN') {
                                //    app.fSDataArr[id] = result.result;
                                //}
                                //if (id == 'equipmentSN') {
                                //    app.oEquipment.aequipmentSNMsg = result.result;
                                //}
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
                        if (vals.length == 0) {//当选择之后为空时，将重新渲染
                            fFormSelectsConfig();
                        }
                        var valsD = vals.length ? vals[0].name : '';//判断点击删除时，让输入的值为空字符串
                        if (selectId == 'unitName' || selectId == 'equipmentSN') {
                            date2Render();
                            $('#dom-unitnameall ul li').removeClass('opened');
                        }
                        //if (id == 'processTypeName'&&vals.length) {//当工艺选择之后会重新渲染加工点的选择
                        //    formSelects.config('unitName', {
                        //        searchUrl: '/api/services/app/MESUnit/GetAllUnitByProcessTypeName?processTypeName=' + vals[0].name,
                        //    })
                        //}
                        //if (id == 'processTypeName' || id == 'equipmentSN') {//给加工单价赋值
                        //    var arr = app.fSDataArr[id];
                        //    var empty = false;

                        //    var otherId = id == 'processTypeName' ? 'equipmentSN' : 'processTypeName';
                        //    var otherVal = formSelects.value(otherId).length ?formSelects.value(otherId)[0].name:'';
                        //    console.log(arr)
                        //    arr.forEach(function (item, index) {
                        //        if (item[id] == valsD) {
                        //            if (item.price) {
                        //                app.currentItem.price = item.price;
                        //            } else {
                        //                empty = true;
                        //            }
                        //        }
                        //    })
                        //    if (empty) {
                        //        app.fSDataArr[otherId].forEach(function (item, index) {
                        //            if (item[otherId] == otherVal) {
                        //                if (item.price) {
                        //                    app.currentItem.price = item.price;
                        //                } else {
                        //                    app.currentItem.price = 0;
                        //                }
                        //            }

                        //        })
                        //    }
                        //}
                        //if (id == 'processTypeName' && app.currentItem.partId) {
                        //    prevHadProTypeName();
                        //}
                        //function prevHadProTypeName() {//在手机端开单，开出去重复的工序则提醒
                        //    abp.services.app.part.getTaskPageResult({where:'id='+app.currentItem.partId }).done(function (res) {
                        //        if (res.data[0]) {
                        //            var bPrevHad = res.data[0].tasks.some(function (elem, index, arr) {
                        //                return elem.processTypeName == valsD
                        //            });
                        //            if (app.fromMobile && bPrevHad) {
                        //                layer.msg('您已经开出过名为' + valsD + '的工序');
                        //            }
                        //        }
                              
                        //    })
                           
                        //}
                        
                        app.currentItem[id] = valsD;
                    }, true);
                })
            };
            fFormSelects([ 'projectSN', 'unitName']);
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
                //formSelects.config('partName', {//零件名称特殊赋值
                //    searchName: 'key',
                //    searchUrl: partTaskSearchUrl,
                //    //(function () { return '/api/services/app/Part/GetAll' })(), 需要自执行，上午的时候返回的是函数，当然不行，猜测自执行是能够成功的，弃用的原因是data赋值给app.currentItem与下拉框请求数据都是异步的，无法确定能取到app.currentItem的值
                //    beforeSuccess: function (id, url, searchVal, result) {
                //        //console.log("a")
                //        var resultArr = [];
                //        if (partTaskSearchUrl) {
                //            resultArr = fBeforeSuccess('partName', searchVal, result);
                //        } else {
                //            resultArr.push(new Arr(searchVal));
                //            resultArr = resultArr.filter(function (kong) {//返回非空数组
                //                return kong.name
                //            });
                //        }
                //        return resultArr;
                //    },
                //    success: function (id, url, searchVal, result) {
                //        if (!searchVal) {
                //            //console.log(app.currentItem.partSN)
                //            formSelects.value('partName', [app.currentItem.partSN]);
                //        }

                //        //console.log(app.currentItem[id])
                //    }
                //})
                //console.log(partTaskSearchUrl)
            })();
            //formSelects.on('partName', function (id, vals, val) {//在零件名称点击时，给规格和数量赋值;删除时为空值也会触发
            //    //console.log(vals[0])
            //    if (vals.length == 0) {//零件没选中，重新渲染
            //        formSelects.config('partName', {
            //            searchUrl: fPTSeaUrl(),
            //        })
            //    }
            //    console.log(vals)
            //    var valsD = vals.length ? vals[0].name : '';//当前已选中的值
            //    var valsV = vals.length ? vals[0].value : '';
            //    var partIded = false;//后台是否有值；
            //    app.oVariable.partArr.forEach(function (n) {
            //        if (valsD.search(n.partSN)!=-1) {
            //            //app.oVariable.partId = n.partId;
            //            abp.services.app.processTask.getCraftsManByPartId(n.partId).done(function (data) {
            //                app.currentItem.craftsMan = data || '';
            //                formSelects.value('craftsMan', [app.currentItem.craftsMan]); 
            //                //fPersonSelect('craftsMan');
            //                console.log(n.partId, data, app.currentItem.craftsMan )
            //            });
            //        }
            //    })
            //    app.currentItem[id] = valsD;
            //    app.currentItem.partSN = '';
            //    app.oVariable.partArr.forEach(function (e) {
            //        //if (valsD == e.partName) {
            //        //app.currentItem.partSpecification = e.partSpecification;
            //        //app.currentItem.partNum = e.partNum;
            //        // }
            //        if (valsV == e.partSN) {//由于新增的value为**_，后台来的value为partSN；只有在选中内容是后台返回数据时，才运行下面
            //            app.currentItem.partSpecification = e.partSpecification;
            //            app.currentItem.partNum = e.partNum;
            //            app.currentItem.partSN = e.partSN;
            //            console.log(app.currentItem.partSN,'111')
            //            //app.currentItem.partId = e.partId;
            //            loadPartTasks(e.partId);
            //            partIded = true;
            //        }
            //        console.log(e,app.currentItem[id]+'0', valsD+'1',valsV+'2',e.partSN)
            //    });
            //    if (!partIded) {
            //        //app.currentItem.partName = valsD;
            //        loadPartTasks(-1);
            //        app.currentItem.partSpecification = '';
            //        app.currentItem.partNum = 1;
            //    }
            //    console.log( app.currentItem.partSN, valsD)
            //}, true)
            //formSelects.render('partName', {
            //    template: function (name, value, selected, disabled) {
            //        //console.log(value)//value是一个object，有name，innerHTML,value,selected,disable等属性
            //        //console.log(value, name, selected, disabled, app.oVariable.partArr)
            //        //return name.split('').reverse().join('');
            //        var temp = { name: value.name, partSpecification: '', partSN: '' };//初始赋值为输入的值
            //        app.oVariable.partArr.forEach(function (e) {
            //            if (e.partSN == value.value) {
            //                temp.name = e.partName;//这句话不加也行，只需得到当前对应的partSpecification值
            //                temp.partSpecification = e.partSpecification;
            //                temp.partSN = '(' + e.partSN + ')';
            //            }
            //        })
            //        return '<span class="partname-span">' + temp.name + '</span><span class="partname-span" style="font-size:12px;">' + temp.partSN + '</span><span class="partname-span" style="float:right">' + temp.partSpecification + '</span>'
            //    }
            //})
            formSelects.on('projectSN', function (id, vals) {
                if (vals.length == 0) {
                    formSelects.config('projectSN', {
                        searchUrl: '/api/services/app/Project/GetAll?take=200',
                    })
                }
                var valsD = vals.length ? vals[0].name : '';
                console.log("test" + valsD);

                app.oVariable.projectArr.forEach(function (n) {
                    //console.log(n,valsD)
                    if (n.projectSN == valsD) {
                        //app.oVariable.projectId = n.id;
                        //GET /api/services/app/ProcessTask/GetProjectChargerByProjectId
                        //abp.services.app.processTask.getProjectChargerByProjectId(n.id).done(function (data) {
                        //    console.log(n.id,data)
                        //    app.currentItem.projectCharger = data || '';//是否要设默认空值

                        //    formSelects.value('projectCharger', [app.currentItem.projectCharger]); 
                        //    //fPersonSelect('projectCharger');
                        //});
                    }
                })
                app.currentItem[id] = valsD;
                console.log("test2" + app.currentItem[id]);
                var projectSN = valsD ? valsD : '_';
                //formSelects.config('partName', {
                //    searchName: 'key',
                //    searchUrl: '/api/services/app/Part/GetAll?projectSN=' + projectSN,
                //    beforeSuccess: function (id, url, searchVal, result) {
                //        return (fBeforeSuccess('partName', searchVal, result));
                //    }
                //})

                //app.currentItem.partSN = '';
                //app.currentItem.partName = '';
            }, true)
            //formSelects.on('feeType', function (id, vals, val, isAdd, isDisabled) {
            //    //var a=Number(vals[0].value);
            //    var valsD = vals.length ? Number(vals[0].value) : 1;//判断点击删除时，让输入的值为空字符串
            //    //app.currentItem.feeType = valsD;
            //    //console.log('on监听', app.currentItem.feeType, vals[0].value);
            //    Vue.set(app.currentItem, 'feeType', valsD);
            //}, true);

            function date2Render() {//让预约日期默认置为明天
                //var myDate = new Date();
                //var date = myDate.getDate() + 1;
                //var mouth = myDate.getMonth() + 1;
                //var year = myDate.getFullYear();
                //var tDate = year + '-' + mouth + '-' + date;
                //console.log(tDate);

                //function GetDateStr(startDay, AddDayCount) {
                //    var dd =startDay? new Date(startDay):new Date();
                //    dd.setDate(dd.getDate() + AddDayCount);//获取AddDayCount天后的日期
                //    var y = dd.getFullYear();
                //    var m = (dd.getMonth() + 1) < 10 ? "0" + (dd.getMonth() + 1) : (dd.getMonth() + 1);//获取当前月份的日期，不足10补0
                //    var d = dd.getDate() < 10 ? "0" + dd.getDate() : dd.getDate();//获取当前几号，不足10补0
                //    return y + "-" + m + "-" + d;
                //}  
                //var tDate = GetDateStr('', 1);
                //layui.laydate.render({
                //    elem: '#date2',
                //    value: tDate,
                //    done: function (value) {
                //        app.currentItem.appointDate = value;
                //    }
                //});
                //app.currentItem.appointDate = tDate;
            }
            //云加工点
            layui.use('flow', function () {
                var flow = layui.flow;
                var typeNameValue = '';
                var typeNameValue_Last = 'all';
                var unitId = '';
                var unitId_last = '';
                var aMessegeArr = [];
                //监听关闭时，隐藏云设备
                function fCloseRemove() {
                    formSelects.closed('unitName', function (id) {
                        $('#' + unitId_last).hide();
                        $('#hoveredLi').hide();
                    });
                    //在其他selecct打开时，监听unitName关闭会失效
                    formSelects.opened(null, function (id) {
                        console.log(id != 'unitName')
                        if (id != 'unitName') {
                            $('#' + unitId_last).hide();
                            $('#hoveredLi').hide();
                        }

                    });
                    //function fBugFormOpened(item) {
                    //    item.forEach(function (s) {
                    //        formSelects.opened(s, function (id) {
                    //            $('#' + unitId_last).hide();
                    //            $('#hoveredLi').hide();
                    //        })
                    //    })
                    //};
                    //fBugFormOpened(['projectSN', 'partName', 'processTypeName','equipmentSN']);
                }
                fCloseRemove();
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
                            //date2Render();
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
                
            })
        }
        //--------------------------------------------------------------------------------------------
        var form = layui.form;
     
        form.on("submit(submit)", function () { app.fSubmitForm() });
        //form.on("submit(submitNext)", function () {
        //   /* app.setPriceJobFee();*///更改jobFee和price的数据
        //    function nextNew() {
        //        app.currentItem.id = 0;//提交时通过id为0告诉后台这是一个新的任务
        //        loadPartTasks(app.currentItem.partId);

        //        app.hadTaskidShow = true;//在下一道时也无法修改模具编号和零件
        //        //app.loadItem(data);
        //        //location.reload();
        //        function clearCurrentItem(cuItem) {
        //            cuItem.forEach(function (el) {
        //                Vue.set(app.currentItem, el, '');
        //                //app.currentItem[el] = ''
        //            });
        //        };
        //        //在下一道清空页面
        //        clearCurrentItem(['feeFactor', 'price', 'estimateHours', 'jobFee', 'appointDate', 'requireDate', 'unitName', 'processTypeName', 'equipmentSN', 'processSN','verified']);
        //        app.currentItem.feeType = 1;
        //        app.currentItem.processTaskStatus = null;
        //        app.currentItem.taskInfo = '参考附件';
        //        app.currentItem.files = [];
        //        app.currentItem.sheetFile = {};
        //        app.currentItem.inner = abp.setting.getBoolean('MES.DefaultSourceInner') ? true : false;
        //        app.currentItem.emergency = app.currentItem.cha = app.currentItem.xiu = false;

        //        app.currentItem.multi = false;
        //        app.currentItem.processTaskParts = [{}];

        //        refresh();

        //        //Vue.nextTick(function () {
        //        //    useFunction(app)
        //        //})
        //    }
        //    if (app.disabled.verified) {//已回单直接走下一步
        //        nextNew();
        //        return false;
        //    }
        //    if (app.ConfirmProcessFlag) {
        //        app.beforeSubmit();
        //        func.runAsync(abp.services.app.processTask.submitProcessTask(app.currentItem).done(function (data) {
        //            //返回的是加工单id
        //            //Vue.set(app.currentItem, 'partId', data.partId)
        //            //Vue.set(app.currentItem, 'partName', data.partName);
        //            //Vue.set(app.currentItem, 'partSN', data.partSN);
        //            //Vue.set(app.currentItem, 'processTaskStatus', data.processTaskStatus);
        //            app.currentItem.files = [];
        //            app.currentItem.processTaskParts = [];
        //            $.extend(true, app.currentItem,data)
        //            console.log(data, '下一道')
        //            layer.msg("保存成功");
        //            nextNew();

        //        }));
        //    } else {
        //        abp.message.error(L('此加工单已审核,不支持修改'));
        //        nextNew();
        //    }

        //    //nextNew();

        //    //layui.use('formSelects', useFunction);
        //});    projectCharger: '', unitName: '',
        app = new Vue({
            el: '#app',
            data: {
                currentItem: {
                    tryTimes: 0,mouldTrySN:'',id:0,memo: '', unitName: '', tryColor: '', tryNumber: 1, ifDry: false, poster: '', tryPerson: '', tn: 0, planDate: '', arrangeDate: '', actualDate: '', tryHours: '', tryMaterial: '', tryMachine: '', feeMachine: 0, feeMaterial: 0, feeOther:0, material: '', materialBack: '', materialOut_New: 0, materialOut_Old: 0, materialReturn_New: 0, materialReturn_New: 0, materialReturn_Old: 0, projectSN: '', materialOut_Old: 0, materialUsed_New: 0, materialUsed_Old: 0, coolingWater: '', wkRequire: '', toDemould:''
                },
                oVariable: { partArr: [], projectArr: [], projectId: '', partId: '' },
                fSDataArr: {
                    processTypeName: [],
                    equipmentSN: []
                },
                //a:[],
                liMesseage: { companyName: '天网', mobile: '', address: '请选择云加工点', speciality: '' },
                hadTaskidShow: false,
                //在下一道时改变
                //oEquipment: { bFactory: false, aEquipmentSNMsg:[]}//{厂内外:false为厂外,请求回来到的设备编号及其附带的信息}
                DetailsItems :[
                    { type: 'text', title: '加工类别', verify: '', key: 'type'},
                    { type: 'text', title: '数量', verify: '', key: 'number' },
                    { type: 'text', title: '单位', verify: '', key: 'measureMentUnit' },
                    { type: 'text', title: '单价', verify: '', key: 'price' },
                    { type: 'text', title: '金额', verify: '', key: 'cost' },
                    { type: 'text', title: '备注', verify: '', key: 'remarks' },
                ],
              
            },
            computed: {
                multiItems: function () {
                    var that = this;
                    var options = this.oVariable.partArr.map(function (n) {
                        return {text:n.partName+'('+n.partSN+')',value:n.partSN}
                    })
                    function onChange(val, index) {
                        var selectData = that.oVariable.partArr.filter(function (n) {
                            return n.partSN === val
                        })[0];
                        if (val) {
                            that.currentItem.processTaskParts[index].partName = selectData.partName;
                            that.currentItem.processTaskParts[index].partNum = selectData.partNum;
                            that.currentItem.processTaskParts[index].partSpecification = selectData.partSpecification;
                            //如果选中值了，将零件名称设为只读模式
                            $('.multi-part tbody tr ').eq(index).find('td.partName input').attr('readonly', '').css('color','#ccc')
                        } else {
                            //如果清空选中值，则解开只读
                            that.currentItem.processTaskParts[index].partName = '';
                            $('.multi-part tbody tr ').eq(index).find('td.partName input').removeAttr('readonly').css('color', '#000')
                        }
                    }
                    return [
                        { type: 'text', title: '零件名称', verify: '', key: 'partName' },
                        { type: 'select', title: '零件编号', verify: '', key: 'partSN', options: options, onChange: onChange },
                        { type: 'text', title: '规格', verify: '', key: 'partSpecification' },
                        { type: 'text', title: '数量', verify: '', key: 'partNum' },
                    ]
                },
                mustConfirmProcess: function() {//审核模式下，且此人有审核权限
                    return abp.setting.getBoolean("MES.MustConfirmProcess") && abp.auth.isGranted('Module.JGKD.Button.ConfirmProcess')
                },
                disabled: function () {
                    var hadId = this.currentItem.id?true:false;
                    var running = this.currentItem.processTaskStatus > 1;
                    var verified = this.currentItem.verified;
                    return { running: running, verified: verified, hadId: hadId}
                },
                showmode: function () {
                    return {
                        proSNpName: !(this.disabled.hadId || this.disabled.running || this.disabled.verified) && !this.hadTaskidShow,//false不显示projectSN和partName的select形式
                    }
                },
                FeeTypeContent: function () {
                    var FTcontent;
                    //console.log('vue改变', this.currentItem.feeType)
                    switch (this.currentItem.feeType) {
                        case 0:
                            //this.currentItem.price = 0;
                            FTcontent = { PriceName: '承包金额', feeFactor: false, feeFactorName: '' };
                            break;
                        case 1:
                            //this.currentItem.jobFee = Number(this.currentItem.price) * Number(this.currentItem.estimateHours);
                            //console.log(Number(this.currentItem.price), this.currentItem.estimateHours, Number(this.currentItem.estimateHours))
                            FTcontent = { PriceName: '加工单价', feeFactor: false, feeFactorName: '' };
                            break;
                        case 2:
                            //this.currentItem.jobFee = Number(this.currentItem.price) * Number(this.currentItem.feeFactor);
                            FTcontent = { PriceName: '加工单价', feeFactor: '平方', feeFactorName: 'm²' };
                            break;
                        case 3:
                            //this.currentItem.jobFee = Number(this.currentItem.price) * Number(this.currentItem.feeFactor);
                            FTcontent = { PriceName: '加工单价', feeFactor: '长度', feeFactorName: '米' };
                            break;
                        case 4:
                           // this.currentItem.jobFee = Number(this.currentItem.price) * Number(this.currentItem.feeFactor);
                            FTcontent = { PriceName: '加工单价', feeFactor: '重量', feeFactorName: 'Kg' };
                            break;
                        case 5:
                           // this.currentItem.jobFee = Number(this.currentItem.price) * Number(this.currentItem.feeFactor);
                            FTcontent = { PriceName: '加工单价', feeFactor: '数量', feeFactorName: '单位' };
                            break;
                        default:
                           // this.currentItem.jobFee = Number(this.currentItem.price) * Number(this.currentItem.feeFactor);
                            FTcontent = { PriceName: '加工单价', feeFactor: false };
                    };
                    //console.log(this.currentItem.jobFee, this.currentItem.price, this.currentItem.feeFactor)
                    return FTcontent;
                },
                isDefault: function () {
                    return this.currentItem.taskInfo === '参考附件'
                },
                ConfirmProcessFlag: function () {//只有当审核模式下，且此人无权限才会返回false
                    if (eval(abp.setting.values["MES.MustConfirmProcess"])) {
                        if (abp.auth.isGranted('Module.JGKD.Button.ConfirmProcess')) {
                            return true;
                        } else if (!this.currentItem.processSN) {
                            return true;
                        } else {
                            console.log('强制开单模式，账号无审核权限且此单已开出')
                            return false;
                        }
                    } else {
                        return true;
                    }
                },
                fromMobile: function () {
                    return location.pathname == "/Home/Show"?false:true
                }
            },
            watch: {
                currentItem: {
                    handler: function (val, oldVal) {
                        console.log('监测到currentItem变化');
                        //if (formSelects.value('feeType', 'val')[0] != app.currentItem.feeType) {
                        //    formSelects.value('feeType', [app.currentItem.feeType.toString()]);
                        //}
                       
                        if (formSelects.value('tryMachine', 'val')[0] != app.currentItem.tryMachine) {
                            console.log("tmc" + app.currentItem.tryMachine);
                        formSelects.value('tryMachine', [app.currentItem.tryMachine]);
                        }
                        formSelects.value('unitName', [app.currentItem.unitName]);
                        //formSelects.value('processTypeName', [app.currentItem.processTypeName]);
                        //formSelects.value('equipmentSN', [app.currentItem.equipmentSN]);
                        //formSelects.value('reason', [app.currentItem.reason])
                    },
                    deep: true
                },
                'currentItem.processTaskDetails': {
                    handler: function (val, oldVal) {
                        //console.log('监测到processTaskDetails变化');//当承包明细改变时，设置承包金额
                        //if (oldVal.length) {
                        //    var f = 0, num = 0;
                        //    val.forEach(function (n) {
                        //        num = Number(n.cost);
                        //        if (!isNaN(num)) {
                        //            f += num;
                        //        }
                        //    })
                        //    this.currentItem.jobFee = f;
                        //}
                    },
                    deep: true
                },
                'currentItem.tn': function (newVal, val) {
                    if (formSelects.value('tn', 'val')[0] != app.currentItem.tn) {
                        formSelects.value('tn', [app.currentItem.tn]);
                    }
                },
                'currentItem.projectSN': function (newVal,val) {
                    if (newVal!=val) {//如果取消选中模具编号，清空多零件清单
                        //this.currentItem.processTaskParts = [{}];
                    }
                }, 'currentItem.materialOut_New': function (newVal, val) {
                    if (newVal != val) {
                        //console.log(newVal);
                        formSelects.value('materialOut_New', [app.currentItem.materialOut_New]);
                        if (!isNaN(this.currentItem.materialOut_New) && !isNaN(this.currentItem.materialReturn_New)) {
                            console.log("true"+newVal);
                            this.currentItem.materialUsed_New = this.currentItem.materialOut_New - this.currentItem.materialReturn_New;
                            console.log("true?" + this.currentItem.materialUsed_New);
                        }
                    }
                }, 'currentItem.materialReturn_New': function (newVal, val) {
                    if (newVal != val) {   
                        formSelects.value('materialReturn_New', [app.currentItem.materialReturn_New]);
                        if (!isNaN(this.currentItem.materialOut_New) && !isNaN(this.currentItem.materialReturn_New)) {
                            //console.log("true" + newVal);
                            this.currentItem.materialUsed_New = this.currentItem.materialOut_New - this.currentItem.materialReturn_New;
                            //console.log("true?" + this.currentItem.materialUsed_New);
                        }
                    }
                }, 'currentItem.materialReturn_Old': function (newVal, val) {
                    if (newVal != val) {
                        formSelects.value('materialReturn_Old', [app.currentItem.materialReturn_Old]);
                        if (!isNaN(this.currentItem.materialOut_Old) && !isNaN(this.currentItem.materialReturn_Old)) {                           
                            this.currentItem.materialUsed_Old = this.currentItem.materialOut_Old - this.currentItem.materialReturn_Old;
                        }
                    }
                }, 'currentItem.materialOut_Old': function (newVal, val) {
                    if (newVal != val) {
                        formSelects.value('materialOut_Old', [app.currentItem.materialOut_Old]);
                        if (!isNaN(this.currentItem.materialOut_Old) && !isNaN(this.currentItem.materialReturn_Old)) {
                            this.currentItem.materialUsed_Old = this.currentItem.materialOut_Old - this.currentItem.materialReturn_Old;
                        }
                    }
                }
            },
            methods: {
                beforeSubmit: function () {//上传前处理数据
                    //inBeforeSubmit();//手机端处理图片
                    //Vue.set(app.currentItem, 'fromMobile', app.fromMobile);
                    if (app.currentItem.feeType != 0 && typeof app.currentItem.processTaskDetails == 'array') {//当上传时发现不是承包模式会将明细清空
                        app.currentItem.processTaskDetails.length = 0;
                    }
                    (function handleProcessTaskParts() {
                        //清除多零件中未填写零件名称的项，由于选择零件编号时自动填写名称，可以通过
                        typeof app.currentItem.processTaskParts == "object"&&(app.currentItem.processTaskParts=app.currentItem.processTaskParts.filter(function (elm) {
                            return elm.partName
                        }))
                    })();
                    function fValidationData() {
                        if (app.currentItem.processTaskParts !== "object") {
                            return false
                        }
                        //1.判断零件编号是否有重复
                        var repeatedSN = false;
                        var snArr = app.currentItem.processTaskParts.map(function (elm) {
                            if (elm.partSN && app.currentItem.partSN&&elm.partSN == app.currentItem.partSN) {
                                repeatedSN = true;
                            }
                            return elm.partSN
                        })
                        var distinct = function (snArr) {
                            return snArr.reduce(function (new_array, old_array_value) {
                                if (!old_array_value||new_array.indexOf(old_array_value) == -1) new_array.push(old_array_value);
                                return new_array; //最终返回的是 prev value 也就是recorder
                            }, []);
                        }
                        if (distinct(snArr).length !== snArr.length) {
                            repeatedSN = true;
                        }
                        repeatedSN?layer.msg("多零件中零件编号有重复，请仔细检查"):console.log('没重复')
                        return repeatedSN
                    }
                    //丢出true表示不通过验证
                    return fValidationData();
                },
                setPriceJobFee:function (){
                    switch(this.currentItem.feeType) {
                        case 0:
                            this.currentItem.price = 0;
                            this.currentItem.feeFactor = '';
                            break;
                        case 1:
                            this.currentItem.jobFee = Number(this.currentItem.price) * Number(this.currentItem.tryHours);
                            this.currentItem.feeFactor = '';
                            break;
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        default:
                            this.currentItem.jobFee = Number(this.currentItem.price) * Number(this.currentItem.feeFactor);
                    };
                },
                swtichIFDry: function (bol) {       //加急按钮点击后触发后函数，接到从组件传来的布尔值
                    this.currentItem.ifdry = bol;
                    //layui.layer.tips('为避免滥用，建议一天最高5次', $('[lay-filter="emergency"]').parent())
                },
                switchInner: function (bol){     //厂内按钮点击后触发后函数，接到从组件传来的布尔值
                    this.currentItem.inner = bol; this.currentItem.unitName=this.currentItem.equipmentSN = '';
                },
                flyOpenImg: function () {
                    var that = this;
                    layuiExt.fLayerImg('', this.currentItem.sheetFile ? this.currentItem.sheetFile.filePath : null,
                        {
                            callback: function () {
                                //Vue.set(app.currentItem, 'sheetFile.querystring', +new Date())
                                $('.my-file-img')[0].src = $('.my-file-img')[0].src + +new Date()
                            }
                        });
                },
                fSubmitForm: function (fn) {//提交函数
                    /*this.setPriceJobFee();*///更改jobFee和price的数据
                    //var verified = app.currentItem.verified;
      //              if (!fn&& verified) {
      //                  abp.message.error(L('此加工单已回单,无法编辑'));
						//return false;
      //              }
                    var that = this;
                    //console.log($("input[name='unitName']").val())
                    //if (this.ConfirmProcessFlag) {
                    //    if (this.beforeSubmit()) {
                    //        return false;
                    //    }
                        console.log(this.currentItem);
                        func.runAsync(abp.services.mouldtry.mouldTry.submitMouldTry(this.currentItem).done(function (data) {
                            //app.currentItem.files = [];
                            //app.currentItem.processTaskParts = [];
                            //$.extend(true, app.currentItem, data)
                            //返回的是加工单id
                            console.log("nmd"+data.id)
                            //that.$set(that.currentItem, 'processSN', data.processSN)
                            ////that.currentItem.processSN = data.processSN;
                            that.currentItem.id = parseInt(data.id);
                            that.currentItem.mouldTrySN = data.mouldTrySN;
                            //that.currentItem.partId = data.partId;
                            //that.currentItem.partSN = data.partSN;
                            //that.currentItem.processTaskStatus = data.processTaskStatus;
                            //loadPartTasks(that.currentItem.partId);
                            if (fn) {
                                fn()
                            } else {
                                if (data.processSN) {
                                    layer.msg("已成功开单或修改");
                                } else {
                                    layer.msg("信息保存成功");
                                }
                            }
                            //fn();
                            //app.loadItem(data);
                            //location.reload();
                        }));
                    //} 
                   // else {
                    //    if (fn) {
                    //        fn()
                    //    } else {
                    //        layer.msg("此加工单已审核,不支持修改")

                    //    }
                    //}
                  
                },
                //removeFile: function (index) {
                //    abp.message.confirm('确认删除此项?', function () {
                //        app.currentItem.files.splice(index, 1);
                //    });
                //},
                loadItem: function (tryid) {//有taskid代表是进入编辑页
                    var that = this;
               
                    //console.log(taskid)
                    if (!tryid) {
                        //abp.services.app.processTask.getLastTaskPersons().done(function (data) {
                        //    var fuzhiArr = ['poster'];
                        //    fuzhiArr.forEach(function (str) {
                        //        that.currentItem[str] = data[str];
                        //        //formSelects.value(str, [that.currentItem[str]]);
                        //        //console.log(that.currentItem[str], formSelects.value(str))
                        //    })
                        //    console.log(data);
                           useFunction(that);
                        //});
                        
                        //layui.use('formSelects', useFunction);
                        return;
                    }
                    func.runAsync(abp.services.mouldtry.mouldTry.getMouldTryInfoById(tryid).done(function (data) {//读数据
                        //if (that.currentItem.partId != data.partId) {//是否是点击下方table中自身id
                        //    //console.log(data.partId)
                        //    loadPartTasks(data.partId);
                        //}
                        //if (data.taskInfo == null) {
                        //    data.taskInfo = that.currentItem.taskInfo;
                        //}
                        //if (!data.processTaskDetails) {//如果承包明细清单为空，就附上默认值[]
                        //    data.processTaskDetails = [];
                        //}
                        that.currentItem = data;//需要在上两步之后赋值
                        useFunction(that);
                        //加载零件的加工线路
                        //layui.use('formSelects', useFunction);
                        //$('select[xm-select="projectSN"]').attr("disabled", "disabled");
                        //在有taskid时，禁用模具编号和零件编号，并重新给他两添加title
                        formSelects.disabled('projectSN');
                        //formSelects.disabled('partName');
                        //console.log("230  " + data.partId)
                        //console.log("1230  "+data.reason)
                        //formSelects.value("reason", data.reason);
                        var projectSNTitle = data.projectSN;
                        //var partNameTitle = data.partName + '(' + data.partSN + ')';
                        //$('#projectSNSelect').attr('title', projectSNTitle);
                        //$('#partNameSelect').attr('title', partNameTitle);

                        frenderDate(that);
                        //formSelects.value('feeType', [that.currentItem.feeType]); 
                        console.log('loadItem请求回来的数据：',data)
                        refresh();
                    }));
                }
            },
            created: function () {
                //todo:如果有参数taskid传入说明是编辑页

                var tryid = $.getUrlParam("tryid");
                this.loadItem(tryid);
                $("#app").removeClass("layui-hide");
                var that = this;
                //url: "http://localhost:62114/api/services/app/ProcessTask/GetLastTaskPersons",
                //五个人员赋值
                
                refresh();
            }
        });
        //零件图片
        //func.renderUpload('.picupload', {
        //    trigger: 'click', callback: function (res) {
        //        var item = $(upload.element);
        //        console.log(item);
        //        console.log(res);
        //        var partid = item.attr("partid");
        //        func.runAsync(abp.services.app.part.setPartImg(partid, res.filePath).done(function () {
        //            item.find('img').attr('src', res.filePath)
        //            //item.append("<img src='"+res.result.filePath+"'/>");
        //        }));
        //    }
        //})
        //加工示意图
        //func.renderUpload("#sheetFile", {
        //    trigger: 'click', callback: function (data) {
        //        console.log(data);
        //        app.currentItem.sheetFile = data;
        //    }
        //})
        //上传配置
        //func.renderUpload('#uploadBtn', {
        //    trigger: 'click',
        //    callback: function (res) {
        //        if (!app.currentItem.files || !app.currentItem.files.length) {
        //            app.currentItem.files = [];
        //        }
        //        app.currentItem.files=app.currentItem.files.concat( res);
        //    },
        //    multiple: true
        //})
        //日期配置
        function frenderDate(thatapp) {
            var rapp = thatapp || app;
            layui.laydate.render({
                elem: '#date1',
                value: rapp.currentItem.planDate || '',
                done: function (value) {
                    rapp.currentItem.planDate = value;
                }
            });
            if (!rapp.currentItem.planDate) {
                $('#date1').val('')
            }
            layui.laydate.render({
                elem: '#date2',
                value: rapp.currentItem.arrangeDate || '',
                done: function (value) {
                    rapp.currentItem.arrangeDate = value;
                }
            });
            if (!rapp.currentItem.arrangeDate) {
                $('#date2').val('')
            }
            layui.laydate.render({
                elem: '#date3',
                value: rapp.currentItem.actualDate || '',
                done: function (value) {
                    rapp.currentItem.actualDate = value;
                }
            });
            if (!rapp.currentItem.actualDate) {
                $('#date3').val('')
            }
        }
        frenderDate();

    })
    //加工路线
 //   layui.table.render({
 //       elem: '#PartTask' //指定原始表格元素选择器（推荐id选择器）
 //       , cols: [[
 //           { align: 'center', field: 'partName', width: '300', fixed: 'left', templet: '#partName', style: 'height:95px', title: '零件' },
 //           { align: 'center', field: 'tasks', minWidth: '3000', title: '工艺流程', templet: '#tasks' },
 //           { align: 'center', field: 'caoz', toolbar: '#toolbar', title: '操作' }
 //       ]] //设置表头
 //       , defaultToolbar: []
 //       , cellMinWidth: '80', where: { where: '1>2' }
 //       , page: false
 //       , skin: 'row'
 //       , even: true
 //       , done: config.onTableDone
 //       , url: '/api/services/app/Part/GetTaskPageResult'
	//});

}
//上传图片
//config.onTableDone = function () {
//    if ((!app.currentItem.sheetFile || !app.currentItem.sheetFile.filePath) && !app.currentItem.id) {
//        if (layui.table.cache.PartTask[0] && layui.table.cache.PartTask[0].partImg) {
//            console.log('原来没有，现在要有')
//            Vue.set(app.currentItem, 'sheetFile', { filePath: layui.table.cache.PartTask[0].partImg })
//        }
//    } else {
//        console.log('不合理')
//    }
//}
function refresh() {
    Vue.nextTick(function () {
        layui.form.render();
    })
}

function permission(fn,str) {
    if (abp.setting.getBoolean("MES.MustConfirmProcess")) {
        if (abp.auth.isGranted('Module.JGKD.Button.ConfirmProcess')) {
            app.fSubmitForm(function () {
                fn();
            });
        } else {
            //无权限
            if (app.currentItem.processSN) {//已审核开出的单子不能修改
                //打印
                fn();
            } else {
                //不打印
                layer.msg('保存并等待审核完成后即可' + str)
            }
        }
    } else {
        if (app.currentItem.inner || app.currentItem.unitName) {
            app.fSubmitForm(function () {
                fn();
            });
        } else {
            abp.message.error("请先填写试模地点或设备进行开单，再点击" + str);
            return false;

        }
    } 
}
//发送
//function doSend() {
//    function goahead() {
//        //func.runAsync(abp.services.app.processTask.sendToUnit(app.currentItem.id, { error: failF,abpHandleError:false}).done(function () {
//        //    abp.message.success(L("发送成功"));
//        //}))
//        function failF(err) {
//            if (err.code != -1) {
//                abp.ajax.showError(err)
//            } else {
//                var tenantId = abp.session.tenantId;
//                var src = '/api/qrcode?url=' + encodeURIComponent(location.origin + "/MES/Register?inviter=" + tenantId + "&companyName=" + encodeURIComponent(app.currentItem.unitName));
//                layer.open({
//                    title: '对方未加入'
//                    , content: `<div style="text-align:center;"><span>${err.message}</span><img src="${src}"/></div>`
//                    , area: '300px'
//                    , offset: '100px'
//                    ,btn:''
//                });
//            }
//        }
//        abp.services.app.mESUnit.findUnitOpenId(app.currentItem.supplierId, 'ReceiveOuterTask' ,{ error: failF, abpHandleError: false }).done(function (res) {
//            $('body').append('<iframe style="display:none;" src="/Home/Show?name=../MES/SheetView&taskid=' + app.currentItem.id + '&sendUnit=1">')
//            layer.msg('发送中，请稍后', {
//                icon: 16,
//                shade: 0.5,
//                time: 0
//            });
//        })
//        //$('body').append('<iframe style="display:none;" src="/Home/Show?name=../MES/SheetView&taskid=' + app.currentItem.id + '&sendUnit=1">')
//        //layer.msg('发送中，请稍后', {
//        //    icon: 16,
//        //    shade: 0.5,
//        //    time: 0
//        //});
//    }
//    if (app.currentItem.inner) {
//        abp.message.error("厂内单据无法发送");
//        return false;
//    } else if (!abp.auth.isGranted('Module.JGKD.Button.SendToUnit')) {
//        abp.message.error("您暂无发送权限，请联系管理员进行修改！");
//        return false;
//    }else {
//        abp.message.confirm(L('确认发送给加工点' + app.currentItem.unitName + '?'), function () {
//            permission(goahead, '发送')
//        })
//    }
//}
//预览
function doPreview() {
    //if (!app.currentItem.id  ) {
    //    abp.message.error("请先保存再进行预览");
    //    return false;
    //}
    function goahead() {
        layer.open({
            type: 2,
            title: "试模单预览",
            shadeClose: false,
            shade: 0.8,
            area: ['100%', '100%'],
            content: "/Home/Show?name=../MouldTry/SheetView&tryid=" + app.currentItem.id,
            btn: false,
            cancel: function () {
                app.loadItem(app.currentItem.id);
            }
        })
    }
    permission(goahead, '打印')
}
//加载零件的加工路线
function loadPartTasks(partId) {
    app.currentItem.partId = partId;
    var where = { where: 'id=' + partId };
    layui.table.reload('PartTask', {
        where: where
        , done: config.onTableDone

    })

}
////添加工序

//var inBeforeSubmit = function () {
//    //手机端处理图片等，要被重写的函数
//};

