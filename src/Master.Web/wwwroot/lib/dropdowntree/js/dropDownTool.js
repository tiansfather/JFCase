/**
 * Created by 陈熠 on 2017/6/21
 * email   :  228112142@qq.com
 */
(function ($) {
    var cyProps = {};
    var dropDownTrees = [];

    /* 入口函数 */
    $.fn.dropDownTool = function () {
        //参数数据
        cyProps = $(this).attr("cyProps");
        if (!cyProps) {
            return
        }
        cyProps = cyProps ? cyProps : "";
        //将表格参数转为json
        cyProps = eval("({" + cyProps + "})");
        if (!cyProps.key_value) cyProps.key_value = "name";
        if (!cyProps.key_code) cyProps.key_code = "id";
        //创建节点
        createDropDownDom($(this), cyProps);

    };

    var radioSetting = {
        view: {
            fontCss: setFontCss_ztree
        },
        data: {
            simpleData: {
                idKey: 'id',
                pIdKey: 'parentId',
                rootPId: 'null',
                enable: true
            },
            key: {
                name: "name",
                title: "name",
                url: "nourl"
            }
        },
        callback: {
            onClick: checkNode
        }
    };

    var checkboxSetting = {
        view: {
            fontCss: setFontCss_ztree
        },
        data: {
            simpleData: {
                enable: true,
                idKey: "id",
                pIdKey: "pId",
                rootPId: -1
            },
            key: {
                url: "nourl"
                
            }
        },
        check: {
            enable: true,
            nocheckInherit: true
        },
        callback: {
            onCheck: checkNode
        }
    };

    function checkNode(event, treeId, treeNode) {
        //单选
        if (treeId.indexOf("radio") !== -1) {
            $("#" + treeId).parents('[cytype="dropDownTool"]').find(".combo-text").val(treeNode.name);
            $("#" + treeId).parents('[cytype="dropDownTool"]').find(".form-input").val(treeNode.id);
            //隐藏
            $(".drop-panel").hide();
        }
        //多选
        else {
            var treeObj = $.fn.zTree.getZTreeObj(treeId);
            var nodes = treeObj.getCheckedNodes(true);
            var checkIds="";
            var checkNames="";
            for (var i = 0; i < nodes.length; i++) {
                checkIds += nodes[i].id + (i === nodes.length - 1 ? "" : ",");
                checkNames += nodes[i].name + (i === nodes.length - 1 ? "" : ",");
            }
            $("#" + treeId).parents('[cytype="dropDownTool"]').find(".combo-text").val(checkNames);
            $("#" + treeId).parents('[cytype="dropDownTool"]').find(".form-input").val(checkIds);
        }

    }


    /**创建相关dom元素 **/
    function createDropDownDom($inputValue, cyProps) {
        var _name = $($inputValue).attr("name") || "";
        var _value = $($inputValue).attr("value") || "";
        var _tips = cyProps.tips || "请选择";
        var _search = cyProps.search === "false" ? "none" : "block";

        var nowTime = (new Date()).getTime();
        var txtId = "text-" + nowTime + $(".combo-text").length;
        var ztreeType = cyProps.checkbox === "false" ? "radio" : "checkbox";
        var ztreeId = ztreeType + "-" + nowTime + $(".ztree").length;
        var searchId = "search_condition-" + nowTime + $(".search_condition").length;
        /** 输入框**/
        var inputHtml =
            $inputValue.html([
                '<input type="hidden" class="form-input" name="' + _name + '" value="' + _value + '">',
                '<input type="text" readonly="true" placeholder="' + _tips + '" id="' + txtId + '" class="combo-text layui-input" >',
                '<i class="layui-icon  clear-btn drop-down-clear" style="margin-top:-10px">&#x1006;</i>',
            ].join(""));
        $inputValue.append(inputHtml);

        /** 下拉层 **/
        var panelId = "panel-" + nowTime + $(".combo-panel").length;
        $inputValue.data("panelId", panelId);
        var panelHtml = ['<div class="drop-panel" id="' + panelId + '" style="position:absolute;left:0px;right:0px;">',
            '<dd lay-value="" class="drop-search-div"  style="display:' + _search + '">',
            '<i class="layui-icon  drop-search-btn" >&#xe615;</i>',
            '<input class="layui-input search_condition"  id="' + searchId + '" placeholder="关键字搜索" >',
            ' <i class="layui-icon  clear-btn search-clear-btn" >&#x1006;</i>',
            '</dd>',
            '<div class="tree-div">',
            '<div id="' + ztreeId + '" class="ztree"></div>',
            '</div>',
            '</div>'].join("");
        $inputValue.append(panelHtml);

        dropDownTrees.push($($inputValue));

        var treeData = getTreeData(cyProps.url);
        var _setting = cyProps.checkbox === "true" ? checkboxSetting : radioSetting;
        var ztree = $.fn.zTree.init($("#" + ztreeId), _setting, treeData);
        ztree.key_code = cyProps.key_code;
        ztree.key_value = cyProps.key_value;
        ztree.expandAll(true);
        //默认值回选
        //获取下拉树默认值的id
        var _value = $($inputValue).attr("value")||"";
        var valueName = "";
        //单选
        if (cyProps.checkbox === "false") {
            var node = ztree.getNodeByParam("id", _value);
            if (node != null) {
                //获取下拉树要显示的值
                valueName = node.name;
                // 选中下拉树默认节点
                ztree.selectNode(node);
                $($inputValue).find(".combo-text").val(valueName);
            }
        }
        //多选
        else {
            var _values = _value.split(",");
            for (var i = 0; i < _values.length; i++) {
                var node = ztree.getNodeByParam("id", _values[i]);
                if (node != null) {
                    //获取下拉树要显示的值
                    valueName += node.name + (i === _values.length - 1 ? "" : ",");
                    // 勾选
                    ztree.checkNode(node, true, false);
                }
            }

            $($inputValue).find(".combo-text").val(valueName);
        }
        
    }

    /**隐藏其他下拉树  **/
    function hideOtherDropDownTree() {
        $.each(dropDownTrees, function () {
            var panelId = this.data("panelId");
            if ($("#" + panelId).is(":hidden"))return;
            $("#" + panelId).hide();
        });
    }

    /**下拉层的显示隐藏**/
    function BindPanelShowHide(obj) {
        var $treeDiv = $(obj).parent().find(".drop-panel");
        if ($treeDiv.is(":hidden")) {
            $treeDiv.show();
            $treeDiv.css("z-index", 99900); //修改弹出层优先级，低于对话框
            hideOtherDropDownTree();
            $treeDiv.show();
        } else {
            $treeDiv.hide();
            $treeDiv.css("z-index", "0");
        }
    }

    /**获取下拉树数据  **/
    function getTreeData(url) {
        var data="";
        $.ajax({
            type: "get",
            url: url,
            contentType: "application/json",
            async: false,
            dataType: "json",
            success: function (R) {
                //if (R.code == 0) {
                //    data = R.data
                //} else {
                //    //abp.message.error(R.msg);
                //}
                data = R.result;
            },
            error: function () {
                //abp.message.error("系统错误");
            }
        });
        return data;
    }

    /**
     * 搜索树，高亮显示并展示【模糊匹配搜索条件的节点s】
     * @param treeId
     * @param searchConditionId 文本框的id
     */
    function search_ztree(treeId, searchConditionId) {
        var treeObj = $.fn.zTree.getZTreeObj(treeId);

        //<1>.搜索条件
        var searchCondition = $.trim($('#' + searchConditionId).val());
        //<2>.得到模糊匹配搜索条件的节点数组集合
        var highlightNodes = [];

        if (searchCondition != "") {
            highlightNodes = treeObj.getNodesByParamFuzzy("name", searchCondition, null);
        } else {
            showAllNodes(treeObj);
        }
        removeHighlight(treeObj);
        close_ztree(treeId);
        searchCondition && showTreePaths(treeObj, highlightNodes);
    }

    /** 删除高亮显示  **/
    function removeHighlight(treeObj){
        var treeNodes = treeObj.transformToArray(treeObj.getNodes());
        for (var i = 0; i < treeNodes.length; i++) {
            if(!treeNodes[i].highlight) continue;
            treeNodes[i].highlight = false;
            treeObj.updateNode(treeNodes[i]);
        }
    }
    /**
     * 收起树：只展开根节点下的一级节点
     * @param treeId
     */
    function close_ztree(treeId) {
        var treeObj = $.fn.zTree.getZTreeObj(treeId);
        var nodes = treeObj.transformToArray(treeObj.getNodes());
        var nodeLength = nodes.length;
        for (var i = 0; i < nodeLength; i++) {
            if (nodes[i][treeObj.key_code] == '0') {
                //根节点：展开
                (!nodes[i].open) && treeObj.expandNode(nodes[i], true, true, false);
            } else {
                //非根节点：收起
                nodes[i].open && treeObj.expandNode(nodes[i], false, true, false);
            }
        }
    }

    /** 显示对应节点的根路径并展开高亮  **/
    function showTreePaths(treeObj, nodeList) {
        /**  获取父节点路径直到根节点 **/
        var treeStructure = {};
        var key_code = treeObj.key_code || "id";
        var createTreeStructure = function (node) {
            if (node == null || node.getParentNode() == null)return;
            var parentNode = node.getParentNode();
            treeStructure[node[key_code]].pid = parentNode[key_code];
            if (!treeStructure[parentNode[key_code]]) {
                treeStructure[parentNode[key_code]] = {node: parentNode, pid: "", cid: {}};
            }
            treeStructure[parentNode[key_code]].cid[node[key_code]] = true;
            arguments.callee(parentNode);
        }
        $.each(nodeList, function () {
            if (!treeStructure[this[key_code]]) treeStructure[this[key_code]] = {node: this, pid: "", cid: {}};
            this.highlight = true;
            treeObj.updateNode(this);
            createTreeStructure(this);
        });

        var showAndExpandNodes = function (nodeStructure) {
            if ($.isEmptyObject(nodeStructure.cid)) return;
            var levelNodes = nodeStructure.node.children;
            treeObj.hideNodes(levelNodes);
            for (var cid in nodeStructure.cid) {

                treeObj.showNode(treeStructure[cid].node);
                treeObj.expandNode(treeStructure[cid].node, true, false, false);

                arguments.callee(treeStructure[cid]);
            }
        };
        treeObj.hideNodes(treeObj.getNodes());
        (function () {
            for (var code in treeStructure) {
                var nodeStructure = treeStructure[code];
                if (nodeStructure.pid)continue;
                var currentNode = nodeStructure.node;
                treeObj.showNode(currentNode);
                treeObj.expandNode(currentNode, true, false, false);
                showAndExpandNodes(nodeStructure);
            }
        })();
    }

    /** 显示所有节点  **/
    function showAllNodes(treeObj) {
        var nodes = treeObj.getNodesByParam("isHidden", true);
        treeObj.showNodes(nodes);
    }

    /**
     * 设置树节点字体样式
     */
    function setFontCss_ztree(treeId, treeNode) {
        var treeObj = $.fn.zTree.getZTreeObj(treeId);
        if (treeNode[treeObj.key_code] == 0) {
            //根节点
            return {color:"#333", "font-weight":"bold"};
        } else if (treeNode.isParent == false){
            //叶子节点
            return (!!treeNode.highlight) ? {color:"#ff0000", "font-weight":"bold"} : {color:"#333", "font-weight":"normal"};
        } else {
            //父节点
            return (!!treeNode.highlight) ? {color:"#ff0000", "font-weight":"bold"} : {color:"#333", "font-weight":"normal"};
        }
    }

    /**下拉点击监听  **/
    $(document).on("click", ".combo-text", function (event) {
        BindPanelShowHide(this);
        event.stopPropagation();
    });
    $(document).on("click", ".drop-panel", function (event) {
        event.stopPropagation();
    });
    $(document).on("click", function () {
        $(".drop-panel").hide();
    });

    /**搜索监听  **/
    $(document).on("keypress", ".drop-search-div .search_condition", function (event) {
        if (/^13$/.test(event.keyCode)) { //是否为Enter键
            search_ztree($(this).parents(".drop-panel").find(".ztree").attr("id"), $(this).attr("id"));
        }
    });
    /**清空搜索条件**/
    $(document).on("click", ".search-clear-btn", function (event) {
        $(this).prev().val("");
        search_ztree($(this).parents(".drop-panel").find(".ztree").attr("id"), $(this).attr("id"));
        event.stopPropagation();
    });
    /**清空下拉树选中的值**/
    $(document).on("click", ".drop-down-clear", function (event) {
        var _value=$(this).prev().prev().val()||"";
        var _values=_value.split(",");
        $(this).prev().val("");
        $(this).prev().prev().val("");
        var treeId=$(this).parents("[cyType='dropDownTool']").find(".ztree").attr("id");
        var treeObj = $.fn.zTree.getZTreeObj(treeId);
        //单选
        if (treeId.indexOf("radio") !== -1) {
            treeObj.cancelSelectedNode();
        }else{
            for (var i = 0; i < _values.length; i++) {
                var node = treeObj.getNodeByParam("id", _values[i]);
                if (node != null) {
                    treeObj.checkNode(node, false, false);
                }

            }
        }
        
        event.stopPropagation();
    });


})(jQuery);

$(document).ready(function () {
    //var dropDownTools = $("[cyType='dropDownTool']");
    //for (var i = 0; i < dropDownTools.length; i++) {
    //    $(dropDownTools[i]).dropDownTool();
    //}

});