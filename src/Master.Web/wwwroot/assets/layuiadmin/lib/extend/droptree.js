// ***********************************************************************
// Assembly         : OpenAuth.Mvc
// Author           : yubaolee
// Created          : 10-16-2016
//
// Last Modified By : yubaolee
// Last Modified On : 10-16-2016
// ***********************************************************************
// <copyright file="droptree.js" company="www.cnblogs.com/yubaolee">
//     版权所有 玉宝(C) 2017
// </copyright>
//单击文本框弹出的选择列表,可以多选。调用： 
//var droptree = layui.droptree("/UserSession/GetOrgs", "#Organizations", "#OrganizationIds");
// droptree.render();
// ***********************************************************************

layui.define(['jquery', 'layer'], function (exports) {
    var $ = layui.jquery;
    var layer = layui.layer;
    var zTreeObj;
    var inst;   //droptree实体

    //显示下拉菜单
    var showMenu = function () {
        $(inst.config.treeElem).css({
            left: "10px",
            top: $(inst.config.displayElem).outerHeight() + "px"
        }).slideDown("fast");
        $("body").bind("mousedown", onBodyDown);
    };

    //隐藏下拉菜单
    var hideMenu = function () {
        $(inst.config.treeElem).fadeOut("fast");
        $("body").unbind("mousedown", onBodyDown);
    }

    //滚动条下拉
    function onBodyDown(event) {
        if (!(event.target.id == "menuContent" || $(event.target).parents("#menuContent").length > 0)) {
            hideMenu();
        }
    }

    //点击tree
    var onClick = function (e, treeId, treeNode) {
        var nodes = zTreeObj.getSelectedNodes();

        for (var i = 0, l = nodes.length; i < l; i++) {
            $(inst.config.displayElem).val(nodes[i][inst.config.text]);
            $(inst.config.valueElem).val(nodes[i][inst.config.key]);
            break;
        }
        $(inst.config.valueElem).change();  //如果不调change，VUE不会监听idDom
        hideMenu();
    }

    //tree复选框
    var onCheck = function (e, treeId, treeNode) {
        var nodes = zTreeObj.getCheckedNodes(true);

        var ids = nodes.map(function (e) { return e[inst.config.key]; }).join(",");
        var names = nodes.map(function (e) { return e[inst.config.text]; }).join(",");
        $(inst.config.displayElem).val(names);
        $(inst.config.valueElem).val(ids);
        $(inst.config.valueElem).change();  //如果不调change，VUE不会监听idDom
    }

    //构造器
    var  Class = function (options) {
        var that = this;
        that.config = $.extend({}, that.config, options);

        //上级机构选择框
        $(that.config.displayElem).on("click", function () {
            if (!that.loaded) {
                that.render();
            } else {
                showMenu();
            }
            
        });
     };
    //默认配置
    Class.prototype.config = {
        text: 'displayName',
        key: 'id',
        parentKey: 'parentId',
        selectedMulti: false    //默认单选
    };

    //加载数据
    Class.prototype.render = function () {
        this.loaded = true;

        var that = this;
        var setting = {
            view: { selectedMulti: that.config.selectedMulti },
            check: {
                enable: that.config.selectedMulti,
                chkStyle: "checkbox",
                chkboxType: { "Y": "", "N": "" } //去掉勾选时级联
            },
            data: {
                key: {
                    name: that.config.text,
                    title: that.config.text
                },
                simpleData: {
                    enable: true,
                    idKey: that.config.key,
                    pIdKey: that.config.parentKey,
                    rootPId: ''
                }
            },
            callback: {
                onClick: onClick,
                onCheck: onCheck
            }
        };
        var index = layer.load();
        that.config.func().done(function (json) {
            layer.close(index);
            if (json.length == 0) {
                $(that.config.displayElem).val('');
                $(that.config.valueElem).val('');
                return;
            }
            zTreeObj = $.fn.zTree.init($(that.config.treeElem.find("ul")), setting,json);
            //var newNode = { displayName: "根节点", id: null, parentId: "" };
            //json.push(newNode);
            //zTreeObj.addNodes(null, json);
            that.setCheck();
            zTreeObj.expandAll(true);
            showMenu();
        })
        //$.getJSON(this.config.url,
        //    {
                
        //    },
        //    function (json) {
        //        layer.close(index);
        //        if (json.length == 0) {
        //            $(that.config.nameDOM).val('');
        //            $(that.config.idDOM).val('');
        //            return;
        //        }
        //        zTreeObj = $.fn.zTree.init($(that.config.treeDOM), setting);
        //        var newNode = { displayName: "根节点", id: null, parentId: "" };
        //        json.push(newNode);
        //        zTreeObj.addNodes(null, json);
        //        that.setCheck();
        //        zTreeObj.expandAll(true);
        //        showMenu();
        //    });
    }

    //设置初始选中的值
    Class.prototype.setCheck = function () {   
        zTreeObj.checkAllNodes(false);

        var value = $(this.config.valueElem).val();
        if (value == undefined) return;
        var nodeids = value.split(",");
        var that = this;
        $.each(nodeids,
            function () {
                var node = zTreeObj.getNodeByParam(that.config.key, this, null);
                if (node != null) {
                    zTreeObj.checkNode(node, true, false);
                }
            });
    }

    exports('droptree', function (options) {
        var op = $.extend({ selectedMulti: false }, options)
            //func: abp.services.app.depart.getDepartTreeJson,//接口函数
            //displayElem:$("#@(column.ColumnKey)_data"),//名称显示文本框
            //valueElem:$("#@(column.ColumnKey)"),//值对应文本框,
            //treeElem: $("#menuContent"),//对应树显示区域
            //selectedMulti:false,//是否多选
        inst = new Class(op);
        return inst;
    });
});