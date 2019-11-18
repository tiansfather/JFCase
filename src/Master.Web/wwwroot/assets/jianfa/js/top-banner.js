var speed = 0;
var scrollTop = null;
//var hold = 0;
var jianfa_top;
var pos = null;
//var timer = null;
var moveHeight = null;
jianfa_top = document.getElementById("jianfa_top");
window.onscroll=scroll_ad;
function scroll_ad() {
    if (!jianfa_top)return
 scrollTop = document.documentElement.scrollTop+document.body.scrollTop;
 pos = scrollTop - jianfa_top.offsetTop;
 pos = pos
 moveHeight = pos>0?Math.ceil(pos):Math.floor(pos);
 if(moveHeight!=0){
  jianfa_top.style.top = jianfa_top.offsetTop+moveHeight+"px";
  setTimeout(scroll_ad,speed);
 }
}