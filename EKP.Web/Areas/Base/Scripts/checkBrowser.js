(function () {
    var browser = getBrowserInfo();
    var verinfo = (browser + "").replace(/[^0-9.]/ig, "");
    if ((browser + "").indexOf("ie") > 0 && parseInt(verinfo) < 9) {
        alert("你的浏览器IE版本过低，请升级到IE9及以上版本！");
    }
})();
//判断当前浏览类型  
function getBrowserInfo() {
    var agent = navigator.userAgent.toLowerCase();
    
    var regStr_ie = /msie [\d.]+;/gi;
    var regStr_ff = /firefox\/[\d.]+/gi
    var regStr_chrome = /chrome\/[\d.]+/gi;
    var regStr_saf = /safari\/[\d.]+/gi;
    //IE
    if (agent.indexOf("msie") > 0) {
        return agent.match(regStr_ie);
    }

    //firefox
    if (agent.indexOf("firefox") > 0) {
        return agent.match(regStr_ff);
    }

    //Chrome
    if (agent.indexOf("chrome") > 0) {
        return agent.match(regStr_chrome);
    }

    //Safari
    if (agent.indexOf("safari") > 0 && agent.indexOf("chrome") < 0) {
        return agent.match(regStr_saf);
    }

}