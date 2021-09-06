/* 
    作    者：胡政
    描    述：通用工具js包
    创建时间：2015-08-28
    联系方式：13436053642
*/
/* 通用js库
*/

/********************************************● 初始化加载************************************************************/
(function() {
    /*********************● 扩展String方法：去掉所有html标签和空格，并返回指定长度字符串**********/
    String.prototype.ignoreHtmlNbsp = function() {
        var result = this;
        result = result.replace(/<!--\[if\s+gte\s+mso\s+9\]>(?:(?!<!\[endif\]-->)[\s\S])*<!\[endif\]-->/gi, ""); //去掉<!--[if gte mso 9]><![endif]-->
        result = result.replace(/<[^>]+>/g, ""); //去掉html标签
        result = result.replace(/(^\s+)|(\s+$)/g, ""); //去掉首尾空格
        result = result.replace(/\s/g, ""); //去掉空格
        if (arguments.length == 1) {
            result = result.substr(0, arguments[0]);
        } else if (arguments.length == 2) {
            result = result.substr(arguments[0], arguments[1]);
        }
        return result;
    };

    /*********************● 扩展字符串方法，去掉所有html标签**************************************/
    String.prototype.ignoreHtml = function() {
        var result = this;
        result = result.replace(/<[^>]+>/g, ""); //去掉html标签
        if (arguments.length == 1) {
            result = result.substr(0, arguments[0]);
        } else if (arguments.length == 2) {
            result = result.substr(arguments[0], arguments[1]);
        }
        return result;
    };

    /*********************● 扩展String方法：源于C#中的string.Format()*****************************/
    String.prototype.format = function(args) {

        /**********来源：http://www.cnblogs.com/loogn/archive/2011/06/20/2085165.html *****************/
        /**********
         //两种调用方式
         var template1="我是{0}，今年{1}了";
         var template2="我是{name}，今年{age}了";
         var result1=template1.format("loogn",22);
         var result2=template2.format({name:"loogn",age:22});
         //两个结果都是"我是loogn，今年22了"
        ***********/

        args = args == null ? "" : args;
        var result = this;
        if (arguments.length > 0) {
            if (arguments.length == 1 && typeof (args) == "object") {
                for (var key in args) {
                    args[key] = args[key] == null ? "" : args[key];
                    if (args[key] != undefined) {
                        var reg = new RegExp("({" + key + "})", "g");
                        result = result.replace(reg, args[key]);
                    }
                }
            } else {
                for (var i = 0; i < arguments.length; i++) {
                    arguments[i] = arguments[i] == null ? "" : arguments[i];
                    if (arguments[i] != undefined) {
                        //var reg = new RegExp("({[" + i + "]})", "g");//这个在索引大于9时会有问题，谢谢何以笙箫的指出
                        var reg = new RegExp("({)" + i + "(})", "g");
                        result = result.replace(reg, arguments[i]);
                    }
                }
            }
        }
        return result;
    }

    ///*********************● 扩展String方法：对字符串编码******************************************/
    String.prototype.htmlEncode = function() {
        var str = this.toString();
        var s = "";
        if (str.length == 0) return "";
        s = str.replace(/&/g, "&amp;");
        s = s.replace(/</g, "&lt;");
        s = s.replace(/>/g, "&gt;");
        s = s.replace(/\'/g, "&#39;");
        s = s.replace(/\"/g, "&quot;");
        return s;
    };

    ///*********************● 扩展String方法：对字符串解码********************************************/
    String.prototype.htmlDecode = function() {
        var str = this.toString();
        var s = "";
        if (str.length == 0) return "";
        s = str.replace(/&amp;/g, "&");
        s = s.replace(/&lt;/g, "<");
        s = s.replace(/&gt;/g, ">");
        s = s.replace(/&#39;/g, "\'");
        s = s.replace(/&quot;/g, "\"");
        return s;
    };

    /*********************● 扩展jquery方法：用format方法替换原先的html****************************/
    $.fn.replaceFormat = function(args) {
        var $this = this;
        // jQuery中replaceWith不提供链式调用，此处不会出现不可调用的情况，使用selector保持链式调用
        var selector = $this.selector;
        $($this).each(function() {
            $(this).replaceWith($(this).prop("outerHTML").format(args));
        });
        // jQuery中replaceWith不提供链式调用，此处不会出现不可调用的情况，使用selector保持链式调用
        return $(selector);
    }

    /*********************● 扩展jQuery方法：序列化表单值为json对象********************************/
    $.fn.serializeObject = function() {
        var obj = new Object();
        $.each(this.serializeArray(), function(index, param) {
            if (!(param.name in obj)) {
                obj[param.name] = param.value;
            } else {
                if ((typeof obj[param.name]).toString().toLowerCase() === "object") {
                    obj[param.name].push(param.value)
                } else {
                    obj[param.name] = [obj[param.name], param.value]
                }
            }
        });
        return obj;
    };

    /*********************● 扩展jQuery方法：移除数组的元素*****************************************/
    $.fn.arrRemove = function(dx) {
        if (isNaN(dx) || dx > this.length) {
            return false;
        }
        for (var i = 0, n = 0; i < this.length; i++) {
            if (this[i] != this[dx]) {
                this[n++] = this[i];
            }
        }
        this.length -= 1;
    }

    /*********************● 全局变量：Url帮助类***************************************************/
    window.urlhelper = {
        //获取网络协议，示例："http:"
        getProtocol: function(url) {
            url = url || window.location.href;
            if (url.indexOf("//") == -1) return "";

            var result = url.split("//")[0];
            return result;
        },
        //获取主机头，示例：localhost:11748
        getHost: function(url) {
            url = url || window.location.href;

            var result = "";
            if (url.indexOf("//") == -1)
                result = url.split("/")[0];
            else
                result = url.split("//")[1].split("/")[0];
            return result;
        },
        //获取路径
        getPath: function(url) {
            url = url || window.location.href;

            var result = "";
            if (url.indexOf("//") == -1) {
                result = url.split("?")[0];
            } else {
                result = url.split("//")[1].split("?")[0];

                var arr = result.split("/");
                result = "";
                $(arr).each(function(i) {
                    if (i == 0) return true;
                    result += "/" + arr[i];
                });
            }
            return result;
        },
        //获取参数
        getSearch: function(url) {
            url = url || window.location.href;

            var result = "";
            if (url.indexOf("?") != -1)
                result = "?" + url.split("?")[1];

            result = result.split("#")[0];

            return result;

        },
        //判断url类型：mvc、aspx
        getType: function(url) {
            url = urlhelper.getPath(url || window.location.href);

            if ($.trim(url).indexOf("javascript") != -1) return "";

            if (url.split("/")[url.split("/").length - 1].indexOf(".") == -1 && url.indexOf("/") != -1) {
                return "mvc";
            } else if (!url || $.trim(url) == "/" || url.indexOf(".") != -1) {
                return "aspx";
            }

            return "";
        },
        //获取锚点
        getHash: function(url) {
            url = url || window.location.href;

            var result = "";
            if (url.indexOf("#") != -1)
                result = "#" + url.split("#")[1];

            return result;
        },
        //获取url所有参数
        getQs: function(url) {
            return $getQueryStrings(url);
        },
        //获取url绝对路径
        getAbpath: function(url) {
            return $getPath(url);
        },
        //设置url参数值
        setParams: function(obj, url) {
            url = url || window.location.href;

            var aprotocol = urlhelper.getProtocol(url);
            var ahost = urlhelper.getHost(url);
            var apath = urlhelper.getPath(url);
            var asearch = urlhelper.getSearch(url);
            var ahash = urlhelper.getHash(url);
            var result = url;
            var joinObj = function(joinObj_obj) {
                var result = '';
                for (var i in joinObj_obj) {
                    result += i + '=' + joinObj_obj[i] + "&";
                }
                return result && result.substr(0, result.length - 1);
            };
            var splitSearchToObj = function(str) {
                var resObj = {};
                var arr = str && str.replace("?", "").split('&');
                for (var i = 0; i < arr.length; i++) {
                    var name = arr[i].split("=")[0];
                    resObj[name] = arr[i].split("=").length > 1 ? arr[i].split("=")[1] : "";
                }
                return resObj;
            };
            var objExtend = function(oldobj, obj) {
                var result = {};
                for (var i in oldobj) {
                    result[i] = oldobj[i];
                }
                for (var i in obj) {
                    result[i] = obj[i];
                }
                return result;
            };

            if (!asearch) {
                result = (aprotocol && (aprotocol + '//')) + ahost + apath + (joinObj(obj) ? '?' : "") + joinObj(obj) + ahash;
            } else {
                var oldSearchObj = splitSearchToObj(asearch);
                result = (aprotocol && (aprotocol + '//')) + ahost + apath + "?" + joinObj(objExtend(oldSearchObj, obj)) + ahash;
            }

            return result;
        },
    }

    /*********************● 全局变量：网站url后面的参数对象***************************************/
    window.gloQs = $getQueryStrings();
})();

/********************************************● 获取URL后面的参数*****************************************************/
function $getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return "";
}

/********************************************● 获取url后面的参数*****************************************************/
function $getQueryStrings(url) {
    url = url || window.location.href;
    var paramStr = url.split("?").length > 1 ? url.split("?")[1] : "";
    var pattern = /([^&]+)=(((?!&).)+)/ig;
    var parames = {};
    paramStr.replace(pattern, function (a, b, c) {
        parames[b] = unescape(c);
    });
    return parames;
}

/********************************************● 获取Url绝对路径*******************************************************/
function $getPath(url) {
    url = url || window.location.href;
    url = url.replace("", "");

    var arrUrl = url.split("//");

    var start = arrUrl[1].indexOf("/");
    var relUrl = arrUrl[1].substring(start);//stop省略，截取从start开始到结尾的所有字符

    return relUrl;
}

/********************************************● 将字符串转化为Date类型(全局方法)**************************************/
function $formatDate(str, fmt) { //author: meizz  
    //调用实例：var time2 = $formatDate(dateStr, "yyyy-MM-dd hh:mm:ss");
    var date;
    if (!str || $.trim(str) == "" || str.indexOf("62135596800000") > -1) return "";
    if (str.indexOf('T') > -1) {
        str = str.replace(/T/g, ' ').replace(/\.[\d]{3}Z/, '')//去掉时间里面的“T”
        str = str.replace(/-/g, "/");//将yyyy-mm-dd转化为yyyy/mm/dd
        if (str.length > 19) {
            str = str.substr(0, 19);//去掉时间秒后面的位数
        }
        date = new Date(str);
    } else {
        var patt = new RegExp("/Date\\((\\d+)(?:\\+\\d{1,4})?\\)/");
        var groups = patt.exec(str);
        var milliseconds = parseInt(groups[1]);
        date = new Date(milliseconds);
    }

    var o = {
        "M+": date.getMonth() + 1, //月份 
        "d+": date.getDate(), //日 
        "h+": date.getHours(), //小时 
        "m+": date.getMinutes(), //分 
        "s+": date.getSeconds(), //秒 
        "q+": Math.floor((date.getMonth() + 3) / 3), //季度 
        "S": date.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (date.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    if (fmt.toString().indexOf("NaN") != -1)
        return "";
    return fmt;
};

/********************************************● 将yyyy-MM格式的时间字符串的月份+1,返回新的yyyy-MM格式的时间字符串*****/
function $dateStrAddMouth(dateStr) {
    if (!dateStr || $.trim(dateStr) == "")
        return dateStr;

    var yyyy = parseInt(dateStr.split("-")[0]);
    var MM = parseInt(dateStr.split("-")[1]);

    if (MM != 12) {
        return yyyy.toString() + "-" + (MM + 1).toString();
    }
    else {
        return (yyyy + 1).toString() + "-" + (1).toString();
    }

}

/********************************************● 小数转化为百分比数字*************************************************/
function numToPercent(point) {
    if (!point) {
        point = 0;
    }
    var str = Number(point * 100).toFixed(1);
    str += "%";
    return str;
}
