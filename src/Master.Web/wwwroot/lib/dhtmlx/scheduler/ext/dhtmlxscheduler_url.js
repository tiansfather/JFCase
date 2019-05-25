/*
@license
dhtmlxScheduler v.5.1.1 Professional Evaluation

This software is covered by DHTMLX Evaluation License. Contact sales@dhtmlx.com to get Commercial or Enterprise license. Usage without proper license is prohibited.

(c) Dinamenta, UAB.
*/
Scheduler.plugin(function(e){e._get_url_nav=function(){for(var e={},t=(document.location.hash||"").replace("#","").split(","),a=0;a<t.length;a++){var n=t[a].split("=");2==n.length&&(e[n[0]]=n[1])}return e},e.attachEvent("onTemplatesReady",function(){function t(t){r=t,e.getEvent(t)&&e.showEvent(t)}var a=!0,n=e.date.str_to_date("%Y-%m-%d"),i=e.date.date_to_str("%Y-%m-%d"),r=e._get_url_nav().event||null;e.attachEvent("onAfterEventDisplay",function(e){return r=null,!0}),e.attachEvent("onBeforeViewChange",function(o,l,d,s){
if(a){a=!1;var _=e._get_url_nav();if(_.event)try{if(e.getEvent(_.event))return t(_.event),!1;var c=e.attachEvent("onXLE",function(){t(_.event),e.detachEvent(c)})}catch(u){}if(_.date||_.mode){try{this.setCurrentView(_.date?n(_.date):null,_.mode||null)}catch(u){this.setCurrentView(_.date?n(_.date):null,d)}return!1}}var h=["date="+i(s||l),"mode="+(d||o)];r&&h.push("event="+r);var p="#"+h.join(",");return document.location.hash=p,!0})})});