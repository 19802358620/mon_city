(function () {
    $(function () {
        // 新版检索 横向列表
        $(".con-search-ul li").click(function () {
            $(".con-search-ul li").removeClass("con-s-c-t");
            $(this).addClass("con-s-c-t");
            var valtype = $(this).attr("data-val");
            $('#searchTypeOptions i[opt_type="' + valtype + '"]').click(); // 原版
            $("#con-search-sel").val(valtype); // 下拉框
            //$.cookie("yzsearchtype", valtype);//设置cookie
        });
        // 新版检索 下拉框
        $("#con-search-sel").change(function () {
            var valtype = $("#con-search-sel").val();
            $('#searchTypeOptions i[opt_type="' + valtype + '"]').click(); // 原版
            $('.con-search-ul li[data-val="' + valtype + '"]').click();
        });
        $("#yzbtn-searchbtn").click(function () {
            if ($(".con-search-ul .con-s-c-t").attr("data-val") != "11") {
                $("#txtMainSearchType").val($("#yzbtn-searchtb").val());
                $("#searchBtn").click();
            }
            else {
                var searchword = $("#yzbtn-searchtb").val();
                if (searchword)
                    location.href = "/zk/searchnstl.aspx?key=" + searchword;
                else{
                    alert("请输入检索条件");
                }
            }
        });
        // 检索用 获取当前检索类型
        $("#yzbtn-searchtb").val($("#txtMainSearchType").val());
        //var yzsearchtype = $.cookie("yzsearchtype");
        //if (yzsearchtype)
        //    $('.con-search-ul li[data-val="' + yzsearchtype + '"]').click();


        $('#yzbtn-searchtb').keydown(function (e) {
            if (e.keyCode === 13) {
                $('#yzbtn-searchbtn').click(); //处理事件
            }
        });
    });
})();