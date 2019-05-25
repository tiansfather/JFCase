
(function (layui, window, factory) {
    if (typeof exports === 'object') { // 支持 CommonJS
        module.exports = factory();
    } else if (typeof define === 'function' && define.amd) { // 支持 AMD
        define(factory);
    } else if (window.layui && layui.define) { //layui加载
        layui.define(['form'], function (exports) {
            exports('verifyInit', factory());
        });
    } else {
        window.verifyInit = factory();
    }
})(typeof layui == 'undefined' ? null : layui, window, function () {
    form = layui.form;
    form.verify({
            numberOrempty: [
                /^\s*(\d*\.\d*|\d*\.?|\.?\d*)\s*$/
                ///^(\s*|\d+)$///前可空格任意 （数.数|数.|.数） 后可任意空格
            , '只能填写数字'
        ]
    });

    form.verify({
        integer: [
            /^(\s*|\d+)$/
            , '只能填写整数'
        ]
    });
})