/*****************************● 初始化加载**********************************************/
(function() {
    /*************************● 全局变量：获取当前请求信息*******************************************/
    window.appInfo = getAppInfo();
})();

/*****************************● 初始化站点模板页信息************************************/
function frontLayout(options) {
    var defaults = {
        title: "", //网站title
        isHome: false, //是否是首页
        secendTitle: appInfo.site.Name, //网站二级title
        menuName: "", //当前菜单
        metaKeywords: appInfo.site.MetaKeywords, //网站Keywords
        metaDescription: appInfo.site.MetaDescription, //网站Description
        showBackTop: false //是否显示“回到顶部”按钮
    };
    var opts = $.extend(defaults, options);
    
    //框架
    Layout.init(); 
    Layout.initOWL();
    RevosliderInit.initRevoSlider(); 

    //菜单显示
    if (appInfo.loginUser && appInfo.loginUser.LoginRoleId) {
        $("#navMenu *").each(function(i, dom) {
            if ($(dom).attr("roleId") && $(dom).attr("roleId").indexOf(appInfo.loginUser.LoginRoleId) > -1) {
                $(dom).show();
            }
        });
    }
    
    $("#navMenu > ul").show();

    //设置favicon
    $("#style_favicon").attr("href", appInfo.site.Favicon);

    //设置title
    if (opts.title)
        document.title = "{0}——{1}".format(opts.title, opts.secendTitle);
    else
        document.title = appInfo.site.Name;

    //设置meta 
    $("meta[name='keywords']").attr("content", opts.metaKeywords);
    $("meta[name='description']").attr("content", opts.metaDescription);

    //设置logo
    $("#img_logo").attr("src", appInfo.site.Logo);

    //顶部左侧
    $("#topright").replaceFormat(appInfo.site).show();

    //登录/未登录
    if (appInfo.loginUser) {
        $("#logined").show(); 
        $("#logined .login_center").replaceFormat({
            Photo: appInfo.loginUser.UserInfo.Photo || "/Areas/Front/templates/Shared/t1/images/icon_gr.png",
            Account: appInfo.loginUser.UserInfo.Account
        });
    }
    else {
        $("#loginnot").show();
    }

    //顶部右侧菜单
    $(".login_ywcd").hover(function () {
        $(".login_ywcd").hide();
        $(".login_ywcd_current").show();
    });
    $(".ywcd_box").hover(function () {

    }, function () {
        $(".login_ywcd").show();
        $(".login_ywcd_current").hide();
    });

    //当前聚焦菜单
    $("#navMenu li[sign='{0}']".format(opts.menuName)).addClass("active");
    $("#navMenu li[sign='{0}']".format(opts.menuName)).parents("li").addClass("active");

    //友情链接
    $(appInfo.blogrolls).each(function (i, row) {
        $("#blogrollBox").append('<a href="{Link}" target="_blank" title="{Name}">{Name}</a>'.format(row));
    });

    //底部信息
    $(".pre-footer, .footer").replaceFormat(appInfo.site).show();
    $(".pre-footer .EnWeChat").attr("src", appInfo.site.EnWeChat);

    //回到顶部
    if (opts.showBackTop) {
        $("#back_top_box").show();
    }

    //权限控制
    $(appInfo.authoritys).each(function (i, authority) {
        if (authority.Name) {
            var name = "";
            $(authority.Name.split('.')).each(function (i, partName) {
                if (!name) {
                    name += "{0}".format(partName);
                } else {
                    name += ".{0}".format(partName);
                }
                var authorityItem = $("*[AuthorityItem='{0}.{1}']".format(authority.Type, name));
                $(authorityItem).show();
            });
        }
    });
}

/*****************************● 初始化站点视图页信息************************************/
function frontView(options) {
    var defaults = {
        title: "", //网站title
        secendTitle: appInfo.site.Name, //网站二级title
        metaKeywords: appInfo.site.MetaKeywords, //网站Keywords
        metaDescription: appInfo.site.MetaDescription //网站Description
    };
    var opts = $.extend(defaults, options);

    //设置favicon
    $("#style_favicon").attr("href", appInfo.site.Favicon);

    //设置title
    if (opts.title)
        document.title = "{0}——{1}".format(opts.title, opts.secendTitle);
    else
        document.title = appInfo.site.Name;

    //设置meta 
    $("meta[name='keywords']").attr("content", opts.metaKeywords);
    $("meta[name='description']").attr("content", opts.metaDescription);

    //设置logo
    $("#img_logo").attr("src", appInfo.site.Logo);
}

/*****************************● 获取站点信息********************************************/
function getAppInfo() {
    var appInfo = null;

    $.ajax({
        async: false,
        type: "POST",
        dataType: "json",
        url: getUrl("Shared", "GetAppInfo", { area: "Front" }),
        success: function (data) {
            appInfo = data;
        }
    });

    return appInfo;
}

/*****************************● 加载新模块**********************************************/
function loadView(id, url, param) {
    param = !param ? {} : param;
    $.ajax({
        type: param.type || "get",
        dataType: "html",
        url: url,
        beforeSend: function () {
            $(id).show();
            $(id).html('<div style="text-align:center;line-height:50px;"><img src="/Areas/Adm/Images/loading_5.gif"></div>');
        },
        success: function (data) {
            $(id).html(data);
            //执行成功执行函数
            if (param.success) {
                param.success(data);
            }
        }
    });
}

/*****************************● 退出登录************************************************/
function loginOut(callBack) {
    $.ajax({
        type: "POST",
        dataType: "json",
        url: getUrl("User", "LoginOut", { area: "Adm" }),
        success: function (data) {
            if (callBack) {
                callBack(data);
            }
            else {
                $dialogShow(data);
                if (data.Type == "Success") {
                    setTimeout(function () {
                        window.location.href = getUrl("User", "Login");
                    }, 500);
                }
            }
        }
    });
}

/*****************************● 加载项目树**********************************************/
function loadProjectTree(options) {
    var defaults = {
        box: null, 
        mode: 1,
        selectNode: null
    };
    var opts = $.extend(defaults, options);

    $.ajax({
        type: "POST",
        dataType: "json",
        url: getUrl("Project", "StudyTree", { area: "Adm", siteId: appInfo.site.Id }),
        beforeSend: function() {
            $(opts.box).find(".loading").show();
            $(opts.box).find(".projectTree").hide();
        },
        success: function (data) {
            $(opts.box).find(".loading").hide();
            $(opts.box).find(".projectTree").show();
            $(opts.box).find(".projectTree").html("");

            //显示节点
            $(data).each(function (i, treeNode) {
                showTree(treeNode, $(opts.box).find(".projectTree"), 1);
            });

            //默认展开第一个节点
            var selectFirst = function (li) {
                $(li).find(">.node-item .rbtn ").trigger("click");
                if ($(li).find("ul").length) {
                    selectFirst($(li).find("ul li:first"));
                }
                else {
                    $(li).find("a:first").trigger("click");
                }
            }
            selectFirst($(opts.box).find(".projectTree li:first"));
            
            //显示节点
            function showTree(treeNode, ul, leaf) {
                var node = $('<li><span class="node-item"><a href="javascript:;">{text}</a></li>'.format(treeNode));
                $(ul).append(node);

                //显示节点（全部展开）
                if (treeNode.children && treeNode.children.length && ($(ul).parents(".list-unstyled").length + 1) < opts.mode) {
                    if (leaf <= 2) {
                        $(node).find("span").prepend('<i class="fa fa-myacar-open car{0}-{1}" oreder="{1}"></i> '.format(leaf, $(node).siblings("li").length + 1));
                    }
                    $(node).find("span").append('<i class="rbtn fa-myacar-open"></i></span>');
                    var childUl = $("<ul class='list-unstyled'></ul>");
                    $(node).append(childUl);
                    $(treeNode.children).each(function(i, childNode) {
                        showTree(childNode, childUl, leaf + 1);
                    });
                } else {
                    $(node).find("a").css("margin-left", "10px");
                }
                
                //点击节点
                $(node).find(">.node-item a").click(function (e) {
                    $(opts.box).find(".projectTree li.active").removeClass("active");
                    $($(e.target).closest("li")).addClass("active");
                    opts.selectNode && opts.selectNode(treeNode);
                });

                //点击伸缩按钮
                $(node).find(">.node-item i").click(function () {
                    var i = $(this);
                    var nodeItem = $(this).closest(".node-item");
                    if (i.hasClass("fa-myacar")) {
                        $(nodeItem).find("i").removeClass("fa-myacar").addClass("fa-myacar-open");
                        $(nodeItem).find("i").closest(".node-item").siblings(".list-unstyled").show();
                        $(nodeItem).addClass("current");
                    }
                    else if (i.hasClass("fa-myacar-open")) {
                        $(nodeItem).find("i").removeClass("fa-myacar-open").addClass("fa-myacar");
                        $(nodeItem).find("i").closest(".node-item").siblings(".list-unstyled").hide();
                        $(nodeItem).removeClass("current");
                    }
                });

                //收缩所有节点
                $(node).find(">.node-item i.rbtn").trigger("click");
            }
        }
    });
}