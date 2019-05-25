/*
@license
dhtmlxScheduler v.5.1.1 Professional Evaluation

This software is covered by DHTMLX Evaluation License. Contact sales@dhtmlx.com to get Commercial or Enterprise license. Usage without proper license is prohibited.

(c) Dinamenta, UAB.
*/
Scheduler.plugin(function(e){e.form_blocks.multiselect={render:function(e){var t="dhx_multi_select_control dhx_multi_select_"+e.name;convertStringToBoolean(e.vertical)&&(t+=" dhx_multi_select_control_vertical");for(var a="<div class='"+t+"' style='overflow: auto; height: "+e.height+"px; position: relative;' >",n=0;n<e.options.length;n++)a+="<label><input type='checkbox' value='"+e.options[n].key+"'/>"+e.options[n].label+"</label>",convertStringToBoolean(e.vertical)&&(a+="<br/>");return a+="</div>";
},set_value:function(t,a,n,i){function r(e){for(var a=t.getElementsByTagName("input"),n=0;n<a.length;n++)a[n].checked=!!e[a[n].value]}for(var o=t.getElementsByTagName("input"),d=0;d<o.length;d++)o[d].checked=!1;var l={};if(n[i.map_to]){for(var s=(n[i.map_to]+"").split(i.delimiter||e.config.section_delimiter||","),d=0;d<s.length;d++)l[s[d]]=!0;r(l)}else{if(e._new_event||!i.script_url)return;var _=document.createElement("div");_.className="dhx_loading",_.style.cssText="position: absolute; top: 40%; left: 40%;",
t.appendChild(_);var c=[i.script_url,-1==i.script_url.indexOf("?")?"?":"&","dhx_crosslink_"+i.map_to+"="+n.id+"&uid="+e.uid()].join("");e.$ajax.get(c,function(a){for(var n=e.$ajax.xpath("//data/item",a.xmlDoc),o={},d=0;d<n.length;d++)o[n[d].getAttribute(i.map_to)]=!0;r(o),t.removeChild(_)})}},get_value:function(t,a,n){for(var i=[],r=t.getElementsByTagName("input"),o=0;o<r.length;o++)r[o].checked&&i.push(r[o].value);return i.join(n.delimiter||e.config.section_delimiter||",")},focus:function(e){}}});