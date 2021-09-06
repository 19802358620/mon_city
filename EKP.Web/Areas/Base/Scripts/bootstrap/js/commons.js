(function (window, undefined) {
    //调用父窗口上面的CqutDialog的对象。

    var CQUTDialog = {
        /*弹出错误框,依赖于artDialog组件*/
        showError: function (options) {
            art.dialog.through({
                icon: "error",
                content: '<p style="font-size:18px;color:red;width:400px">' + options.message + '</p>',
                cancelVal: '关闭',
                fixed: true,
                resize: true,
                drag: true,
                title: '提示信息',
                cancel: function () {
                    if (typeof options.cancel == "function") {
                        options.cancel();
                    }
                },
                ok: function () {
                    if (typeof options.ok == "function") {
                        options.ok();
                    }
                }
            });
        },
        showSuccess: function (options) {
            //样式文件在frame.css中
            var html = $("<div class=\"operatMessage\">" + options.message + "</div>");
            var body = $(window.top.document.body);
            $(body).append(html);
            //动画显示过程：600毫秒的时间慢慢显示->显示信息800毫秒->600毫秒的时间慢慢消失->移除信息
            $(html).fadeIn("slow", function () {
                if (typeof options.onCreate == "function") {
                    options.onCreate();
                }

            }).delay(800).fadeOut("slow", function () {
                $(this).remove();
                if (typeof options.onDestroy == "function") {
                    options.onDestroy();
                }
                if (options.href != null && options.href!="") {
                    window.location.href = options.href;
                }
            });

        }
    };

    window.CQUTDialog = CQUTDialog;

})(window);