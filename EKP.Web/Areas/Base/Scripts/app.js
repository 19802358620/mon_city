layui.use(['layer', 'layedit', 'laydate'], function () {
    var layer = layui.layer
        , layedit = layui.layedit
        , laydate = layui.laydate;

    $(function () {
        app.init();
        $(document).pjax('a[data-pjax]', '#page-content');

        $("#side-menu li a").click(function () {
            $(this).parent().siblings("li").removeClass("active");
            $(this).parent().addClass("active");
        });
    })

})

var app = {
    vue(options) {
        var el=!!options.el?options.el:"#page-content";
        var index;
        var defaults = {
            el: el,
            loading: false,
            beforeCreate() {
                //if (options.loading) {
                //    layui.use('layer', function () {
                //        var layer = layui.layer;
                //        index = layer.load();
                //    });
                //}
            },
            updated: function () {
                if (options.loading) {
                    layui.use('layer', function () {
                        var layer = layui.layer;
                        layer.close(index);
                    });
                }
                app.init();
            }
        }
        options = $.extend(true, defaults, options);
        return new Vue(options);
    },
    init() {
        $("#page-content .openWindow").unbind("click");
        $("#page-content .openWindow").click(function (event) {
            event.preventDefault();
            var href = $(this).attr("href");
            var title = $(this).attr("title");
            var width = $(this).attr("width");
            var height = $(this).attr("height");
            if (width === undefined) {
                width = "70%;"
            }
            if (height === undefined) {
                height = "80%;"
            }
            layer.open({
                type: 2 //此处以iframe举例
                , title: title
                //, area: ['80%', '80%']
                , area: [width, height]
                , shade: 0.2
                , maxmin: true
                , content: href
                , btn: ['确定', '关闭']
                , yes: function (index, layero) {
                    var body = layer.getChildFrame('body', index);
                    var iframeWin = layero.find('iframe')[0]; //得到iframe页的窗口对象，执行iframe页的方法：iframeWin.method();
                    var form = body.find('form')[0];

                    var loading = iframeWin.contentWindow.layer.load();//显示loading提示
                    var options = {
                        success: function (result) {
                            //关闭loading提示
                            iframeWin.contentWindow.layer.close(loading);
                            if (result.success) {
                                swal({
                                    title: result.message,
                                    type: "success"
                                }, function () {
                                    setTimeout(function () {
                                        //关闭Iframe
                                        layer.close(index);
                                        if (v && typeof (v.refresh) === "function") {
                                            v.refresh();
                                        } else if (v && typeof (v.search) === "function") {
                                            v.search();
                                        }
                                    }, 500);//延时0.1秒，对应360 7.1版本bug
                                });
                                //iframeWin.contentWindow.jmsg(result.message);
                                //刷新当前列表页数据
                                //$("#jqGrid").jqGrid("setGridParam").trigger("reloadGrid");  //重载JQGrid
                                //setTimeout(function () {
                                //    //关闭Iframe
                                //    layer.close(index);
                                //    if (typeof successCallback === 'function') {
                                //        successCallback.call(this);
                                //    }
                                //}, 1000);//延时0.1秒，对应360 7.1版本bug

                            } else {
                                swal({
                                    title: result.message,
                                    text: result.error,
                                    type: "error"
                                });
                            }
                        }
                    }
                    $(form).ajaxSubmit(options);
                }
                , btn2: function () {
                    layer.closeAll();
                }
            });
        });

        $("#page-content .ajax").unbind("click");
        $("#page-content .ajax").click(function (event) {
            event.preventDefault();
            _a = this;
            var href = $(_a).attr("href");
            //var title = $(_a).attr("title");
            var title = $(_a).text();
            swal({
                title: "你确定要执行" + title + "操作?",
                //text: "Your will not be able to recover this imaginary file!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "确定",
                cancelButtonText: "取消",
                closeOnConfirm: false
                //closeOnCancel: false
            }, function (isConfirm) {
                if (isConfirm) {
                    axios.post(href).then(function (response) {
                        var result = response.data;
                        if (result.success) {
                            swal({
                                title: result.message,
                                type: "success"
                            }, function () {
                                if (v && typeof (v.refresh) === "function") {
                                    v.refresh();
                                } else if (v && typeof (v.search) === "function") {
                                    v.search();
                                }
                            });

                        } else {
                            swal({
                                title: result.message,
                                text: result.error,
                                type: "error"
                            });
                        }
                    }).catch(function (error) {

                    });

                } else {

                }
            });
        });
        //初始化复选框
        $('.i-checks').iCheck({
            checkboxClass: 'icheckbox_square-green',
            radioClass: 'iradio_square-green',
        });
    },
    filterEnumAttribute(enums) {
        Vue.filter('EnumAttributeCN', function (value) {
            for (var i = 0; i < enums.length; i++) {
                if (enums[i].Value.toLowerCase() == value.toLowerCase()) {
                    isEnable = false;
                    return enums[i].Text;
                }
            }
            return "未知";
        })
    },
    filterEnumStatus(enums) {
        Vue.filter('EnumStatusCN', function (value) {
            for (var i = 0; i < enums.length; i++) {
                if (enums[i].Value.toLowerCase() == value.toLowerCase()) {
                    isEnable = false;
                    return enums[i].Text;
                }
            }
            return "未知";
        })
    },
    filterOmit() {//字符串截取
        Vue.filter('Omit', function (str,len) {
            if (str.length * 2 <= len) {
                return str;
            }
            var strlen = 0;
            var s = "";
            for (var i = 0; i < str.length; i++) {
                s = s + str.charAt(i);
                if (str.charCodeAt(i) > 128) {
                    strlen = strlen + 2;
                    if (strlen >= len) {
                        return s.substring(0, s.length - 1) + "...";
                    }
                } else {
                    strlen = strlen + 1;
                    if (strlen >= len) {
                        return s.substring(0, s.length - 2) + "...";
                    }
                }
            }
            return s; 
        })
    },
    filterDate() {
        Vue.filter('FormatDate', function (str, fmt = "yyyy年MM月dd日") {
            var date = new Date();
            if (!str || $.trim(str) == "" || str.indexOf("62135596800000") > -1) return "";
            if (str.indexOf('T') > -1) {
                str = str.replace("T", " ");//去掉时间里面的“T”
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
        })
    },
    filterDateDiff() {
        Vue.filter('DateDiff', function (dateTime) {
            var dateTimeStamp = Date.parse(dateTime.replace(/-/gi, "/"));
            var minute = 1000 * 60;
            var hour = minute * 60;
            var day = hour * 24;
            var halfamonth = day * 15;
            var month = day * 30;
            var now = new Date().getTime();
            var diffValue = now - dateTimeStamp;
            if (diffValue < 0) { return; }
            var monthC = diffValue / month;
            var weekC = diffValue / (7 * day);
            var dayC = diffValue / day;
            var hourC = diffValue / hour;
            var minC = diffValue / minute;
            if (monthC >= 1) {
                result = "" + parseInt(monthC) + "月前";
            }
            else if (weekC >= 1) {
                result = "" + parseInt(weekC) + "周前";
            }
            else if (dayC >= 1) {
                result = "" + parseInt(dayC) + "天前";
            }
            else if (hourC >= 1) {
                result = "" + parseInt(hourC) + "小时前";
            }
            else if (minC >= 1) {
                result = "" + parseInt(minC) + "分钟前";
            } else
                result = "刚刚";
            return result;
        })
    },
    chartHelper(id,type){
        return "";
    }
}