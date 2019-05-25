/*
@license
dhtmlxScheduler v.5.1.1 Professional Evaluation

This software is covered by DHTMLX Evaluation License. Contact sales@dhtmlx.com to get Commercial or Enterprise license. Usage without proper license is prohibited.

(c) Dinamenta, UAB.
*/
Scheduler.plugin(function(e){!function(){function t(e,t,a){var i=e+"="+a+(t?"; "+t:"");document.cookie=i}function a(e){var t=e+"=";if(document.cookie.length>0){var a=document.cookie.indexOf(t);if(-1!=a){a+=t.length;var i=document.cookie.indexOf(";",a);return-1==i&&(i=document.cookie.length),document.cookie.substring(a,i)}}return""}var i=!0;e.attachEvent("onBeforeViewChange",function(n,r,l,o){if(i&&e._get_url_nav){var d=e._get_url_nav();(d.date||d.mode||d.event)&&(i=!1)}var s=(e._obj.id||"scheduler")+"_settings";
if(i){i=!1;var _=a(s);if(_){e._min_date||(e._min_date=o),_=unescape(_).split("@"),_[0]=this.templates.xml_date(_[0]);var c=this.isViewExists(_[1])?_[1]:l,u=isNaN(+_[0])?o:_[0];return window.setTimeout(function(){e.setCurrentView(u,c)},1),!1}}var h=escape(this.templates.xml_format(o||r)+"@"+(l||n));return t(s,"expires=Sun, 31 Jan 9999 22:00:00 GMT",h),!0});var n=e._load;e._load=function(){var t=arguments;if(e._date)n.apply(this,t);else{var a=this;window.setTimeout(function(){n.apply(a,t)},1)}}}()});