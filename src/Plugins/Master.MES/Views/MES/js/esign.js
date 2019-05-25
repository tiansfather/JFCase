/*
 * 利用Canvas实现简单的签字功能
 *
 * 在引入该JS后需要进行初始化动作
 * $().esign(canvasEdit, sign_show, sign_clear, sign_ok);
 * 其中四个参数均为需要的DOM的ID，允许传入null或""等，视为启用默认ID
 *
 * 需要的HTML包括：
 * 1.一个确定了大小的canvas区域，ID需要通过参数传入jQuery插件，如果不传则默认视canvasEdit为id
 * 2.一个用于显示签名结果的div去，id不传默认视为sign_show
 * 3.一个清除按钮，点击清空当前画板，id如若不传则视为sign_clear
 * 4.一个完成按钮，点击生成签名图片并显示在指定区域，id不传视为sign_ok
 *
 * tip:鼠标只要出了画图范围就视为松开鼠标
 * */

(function (root, doc, factory) {
    //根据加载方式不同，载入jQuery（根据你的requie命名自定义修改）
    if (typeof define === "function" && define.amd) {
        define( ["jquery"], function ($) {
            return factory($);
        });
    } else {
        factory(root.jQuery);
    }
}(this, document, function($) {
    $.fn.esign = function(canvasEdit, sign_show, sign_clear, sign_ok) {

        //初始化相关DOM，获得结果寄存命名空间
        var domMap = domInit(canvasEdit, sign_show, sign_clear, sign_ok);
        //获取画板引用
        var canvas = domMap.canvasEdit[0];
        //注册相关事件
        if (typeof document.ontouchstart != "undefined") {      //适配移动设备
            canvas.addEventListener('touchmove', onMouseMove, false);
            canvas.addEventListener('touchstart', onMouseDown, false);
            canvas.addEventListener('touchend', onMouseUp, false);
        } else {      //适配电脑
            canvas.addEventListener('mousemove', onMouseMove, false);
            canvas.addEventListener('mousedown', onMouseDown, false);
            canvas.addEventListener('mouseup', onMouseUp, false);
            canvas.addEventListener('mouseleave', onMouseUp, false);
        }
        //初始化上下文和参数
        var context = canvas.getContext('2d');
        var linex = new Array();
        var liney = new Array();
        var linen = new Array();
        var lastX = 1;
        var lastY = 30;
        var flag = 0;

        //注册按钮相关事件
        domMap.sign_ok.on("click",function(){
            preview();
            rewrite();
        });

        domMap.sign_clear.click(function(){
            clearImg();
            rewrite();
        });

        /*
        *方法声明
        */

        //相关DOM初始化动作，如果不传入则使用查找默认ID
        function domInit(canvasEdit, sign_show, sign_clear, sign_ok){
            var domMap = {};

            if(!canvasEdit){
                domMap.canvasEdit = $("#canvasEdit");
            } else{
                domMap.canvasEdit = $("#" + canvasEdit);
            }

            if(!sign_show){
                domMap.sign_show = $("#sign_show");
            } else{
                domMap.sign_show = $("#" + sign_show);
            }

            if(!sign_clear){
                domMap.sign_clear = $("#sign_clear");
            } else{
                domMap.sign_clear = $("#" + sign_clear);
            }

            if(!sign_ok){
                domMap.sign_ok = $("#sign_ok");
            } else{
                domMap.sign_ok = $("#" + sign_ok);
            }

            return domMap;
        }

        //根据鼠标坐标获取绘图坐标
        function getCanvasPos(canvas, evt) {
            var rect = canvas.getBoundingClientRect();
            var x, y;
            if (evt.targetTouches) {
                x = evt.targetTouches[0].clientX;
                y = evt.targetTouches[0].clientY;
            } else {
                x = evt.clientX;
                y = evt.clientY;
            }
            return {
                x: (x - rect.left) * (canvas.width / rect.width),
                y: (y - rect.top) * (canvas.height / rect.height)
            }
        }

        //鼠标移动的时候
        function onMouseMove(evt)
        {
            var x = getCanvasPos(canvas, evt).x,
                y = getCanvasPos(canvas, evt).y;

            //判断是否处于按下状态
            if (flag == 1) {
                //如果是则画图
                linex.push(x);
                liney.push(y);
                linen.push(1);
                context.save();
                context.translate(context.canvas.width / 2, context.canvas.height / 2);
                context.translate(-context.canvas.width / 2 , -context.canvas.height/2);
                context.beginPath();
                context.lineWidth = 2;
                for (var i = 1; i < linex.length; i++) {
                    lastX = linex[i];
                    lastY = liney[i];
                    if (linen[i] == 0)
                        context.moveTo(lastX, lastY);
                    else
                        context.lineTo(lastX, lastY);
                }
                context.shadowBlur = 10;
                context.stroke();
                context.restore();
            }
            evt.preventDefault();
        }

        //当鼠标按下的时候修改按下标志，并开始记录坐标
        function onMouseDown(evt) {
            var x = getCanvasPos(canvas, evt).x,
                y = getCanvasPos(canvas, evt).y;
            flag = 1;
            linex.push(x);
            liney.push(y);
            linen.push(0);
        }

        //鼠标松开清除标志
        function onMouseUp() {
            flag = 0;
        }

        //清空绘制区域
        function rewrite(){
            linex = new Array();
            liney = new Array();
            linen = new Array();
            context.clearRect(0, 0, canvas.width, canvas.height);
        }

        //清理已生成照片
        function clearImg(){
            domMap.sign_show[0].innerHTML = "";
        }

        //填充生成的图片
        function preview(){
            var show = domMap.sign_show[0];
            show.innerHTML = "";
            show.appendChild(convertCanvasToImage(canvas));
        }

        //生成签字图片，图片大小自行修改
        function convertCanvasToImage(canvas){
            var image = new Image();
            image.width = 80;
            image.height = 50;
            image.src = canvas.toDataURL("i/png");
            return image;
        }
    };
    return $.fn.esign;
}));
