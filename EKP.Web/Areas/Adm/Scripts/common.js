/* 
    作    者：胡政
    描    述：项目通用js方法
    创建时间：2015-08-28
    联系方式：13436053642
*/
/* 项目通用js方法
*/

/*****************************● 初始化加载**********************************************/
(function() {
    /*************************● 全局变量：获取当前请求信息*******************************************/
    window.appInfo = getAppInfo();
})();

/*****************************● 初始化非母版页信息**************************************/
function admPage(options) {
    var defaults = {
        title: "",
    };
    var opts = $.extend(defaults, options);

    //设置favicon
    $("#style_favicon").attr("href", appInfo.site.Favicon);

    //设置title
    var title = opts.title;
    if (title)
        document.title = title + "——" + appInfo.site.Name;
    else
        document.title = appInfo.site.Name;
}

/*****************************● 初始化站点母版页信息************************************/
function admLayout(options) {
    var defaults = {
        title: "",
        menuName: "",
        layout: "_Layout",
        userPhoto: "/Areas/Base/Content/Metronicv/assets/admin/layout4/img/avatar9.jpg"
    };
    var opts = $.extend(defaults, options);
    opts.userPhoto = appInfo.loginUser.UserInfo.Photo || opts.userPhont;
    opts.layout = (appInfo.admSetting && appInfo.admSetting.Layout) || opts.layout;

    //初始化模板
    if (opts.layout == "_Layout") {
        admTemplate(opts);
    }
    else if (opts.layout == "_Layout2") {
        admTemplate2(opts);
    }
    

    //设置favicon
    $("#style_favicon").attr("href", appInfo.site.Favicon);

    //网站title
    var title = opts.title || $(".page-sidebar-menu").find("li[authority='" + opts.menuName + "'] a").text();
    if (title)
        document.title = title + "——" + appInfo.site.Name;
    else
        document.title = appInfo.site.Name;

    //设置logo
    $("#img_logo").attr("src", appInfo.site.Logo);

    //网站底部
    $(".page-footer-inner").html(appInfo.site.Name);

    //菜单顶部个人信息
    $("#tl_userPhoto").attr("src", opts.userPhoto);

    //顶部左侧
    $("#logoLink").attr("href", getUrl("Company", "Index", { area: "" })).attr("target", "_blank");
    $("#logoLink img").attr("src", appInfo.site.Logo);

    //顶部右侧
    $(".companyInfo").closest(".pull-right").show();
    $(".companyInfo span").prepend(appInfo.site.Name);
    $("a.companyInfo").attr("href", getUrl("Company", "Index", { area: "" })).show();
    $("#userPhoto").attr("src", opts.userPhoto);
    $("#userAccount").html(appInfo.loginUser.UserName);
    $("#userRoleName").html(appInfo.loginUser.LoginRoleName);

    //更多模板
    $("#header_shared_bar").find(".mix-grid").mixitup({
        targetSelector: '.mix',
        effects: ['fade'],
        easing: 'snap',
        onMixEnd: (function () {
            //鼠标经过显示工具栏
            $("#header_shared_bar").find('.mix').hover(
                function () {
                    $(this).find('.label').stop().animate({ bottom: 0 }, 200, 'easeOutQuad');
                    $(this).find('img').stop().animate({ top: -30 }, 500, 'easeOutQuad');
                },

                function () {
                    $(this).find('.label').stop().animate({ bottom: -43 }, 200, 'easeInQuad');
                    $(this).find('img').stop().animate({ top: 0 }, 300, 'easeOutQuad');
                }
            );

            $("#header_shared_bar").on("click", function (e) {
                e.stopPropagation();
            })

            //预览模板
            $('#header_shared_bar a.priview').click(function (e) {
                e.stopPropagation();

                var mix = $(this).closest(".mix");
                var img = $(mix).find(".img-responsive");
                $.fancybox.open('<div class="text-align-center"><img src=\"{0}\" style="max-width:100%"></div>'.format($(img).attr("src")));
            });

            //使用模板
            $('#header_shared_bar .mix[d-name="{0}"]'.format(opts.layout)).addClass("active");
            $('#header_shared_bar a.use').click(function (e) {
                var mix = $(this).closest(".mix");
                $.ajax({
                    type: "POST",
                    dataType: "json",
                    data: {
                        UserId: appInfo.loginUser.Id,
                        Layout: $(mix).attr("d-name")
                    },
                    url: getUrl("AdmSetting", "ChangeTemplate"),
                    success: function (data) {
                        if (data.Type == "Success") {
                            window.location.href = window.location.href;
                        } else {
                            $dialogShow(data);
                        }
                    }
                });
            });
        })()
    });

    //切换角色
    $(appInfo.switchRoles).each(function (i, role) {
        var dom = $("<li class=\"external\"><a  href =\"javascript:switchRole('{Id}');\" >{Name}</a></li>".format(role));
        if (role.Id == appInfo.loginUser.LoginRoleId) {
            dom.removeClass("external");           
        }
        $("#switchRolesBox").append(dom);
    });
}

/*****************************● 模板1***************************************************/
function admTemplate(opts) {
    //初始化工具栏
    initToolbar(appInfo.admSetting && JSON.parse(appInfo.admSetting.Setting), false);

    //初始化菜单
    initMenu(appInfo.menus[0].ChildEntitys);

    //网站地图
    var firstActiceMenu = $(".page-sidebar-menu li.active").eq(0);
    var lastActiceMenu = $(".page-sidebar-menu li.active").eq(1);
    var pageTitle = "<h1>{0} <small><i class=\"fa fa-cog\"></i> {1}</small></h1>".format(opts.title || $(lastActiceMenu).text(), $(firstActiceMenu).find("span:first").text());
    $("#page_bar").append("<li><i class=\"fa fa-home\"></i><a href=\"index.html\">首页</a><i class=\"fa fa-angle-right\"></i></li>");
    if (firstActiceMenu.length) {
        $("#page_bar").append("<li><a href=\"{0}\">{1}</a><i class=\"fa fa-angle-right\"></i></li>"
            .format($(firstActiceMenu).find("a:first").attr("href"), $(firstActiceMenu).find("span:first").text()));
    }
    if (lastActiceMenu.length) {
        $("#page_bar").append("<li><a href=\"{0}\">{1}</a><i class=\"fa fa-angle-right\"></i></li>"
            .format($(lastActiceMenu).find("a:first").attr("href"), $(lastActiceMenu).find("a:first").text()));
    }
    $("#page_bar .fa-angle-right:last").remove();

    //初始化工具栏
    function initToolbar(opts, isUpdate) {
        var defaults = {
            theme: "light",
            border: "rounded",
            layoutBox: "fluid",
            headerMode: "default",
            headerMenu: "light",
            menuMode: "default",
            menuStyle: "accordion",
            footerMode: "default"
        };
        opts = $.extend(defaults, opts);

        //工具栏
        $(".page-toolbar *[data-theme='{theme}']".format(opts)).addClass("active");
        $(".page-toolbar *[data-border='{border}']".format(opts)).attr("selected", true);
        $(".page-toolbar *[data-layoutBox='{layoutBox}']".format(opts)).attr("selected", true);
        $(".page-toolbar *[data-headerMode='{headerMode}']".format(opts)).attr("selected", true);
        $(".page-toolbar *[data-headerMenu='{headerMenu}']".format(opts)).attr("selected", true);
        $(".page-toolbar *[data-menuMode='{menuMode}']".format(opts)).attr("selected", true);
        $(".page-toolbar *[data-menuStyle='{menuStyle}']".format(opts)).attr("selected", true);
        $(".page-toolbar *[data-footerMode='{footerMode}']".format(opts)).attr("selected", true);

        //工具栏按钮点击事件
        $(".page-toolbar .theme-color").click(function () { //皮肤
            var btn = $(this);
            $(".page-toolbar .theme-color").removeClass("active");
            $(btn).addClass("active");

            opts.theme = $(btn).attr("data-theme");
            initLayoutSetting(opts, true);
        });
        $(".page-toolbar .layout-style-option").change(function () {
            var btn = $(this);
            opts.border = $(btn).val();
            initLayoutSetting(opts, true);
        });
        $(".page-toolbar .layout-option").change(function () {
            var btn = $(this);
            opts.layoutBox = $(btn).val();
            initLayoutSetting(opts, true);
        });
        $(".page-toolbar .page-header-option").change(function () {
            var btn = $(this);
            opts.headerMode = $(btn).val();
            initLayoutSetting(opts, true);
        });
        $(".page-toolbar .page-header-top-dropdown-style-option").change(function () {
            var btn = $(this);
            opts.headerMenu = $(btn).val();
            initLayoutSetting(opts, true);
        });
        $(".page-toolbar .sidebar-option").change(function () {
            var btn = $(this);
            opts.menuMode = $(btn).val();
            if (opts.menuMode == "fixed") {
                $(".page-toolbar .sidebar-menu-option").find("option[value='accordion']").attr("selected", true);
                opts.menuStyle = "accordion";
            }

            initLayoutSetting(opts, true);
        });
        $(".page-toolbar .sidebar-menu-option").change(function () {
            var btn = $(this);
            if (opts.menuMode == "fixed" && $(btn).val() == "hover") {
                $(".page-toolbar .sidebar-menu-option").find("option[value='accordion']").attr("selected", true);
                layer.alert("菜单定位为“固定”时不允许将菜单样式置为“悬浮”状态", { icon: 5 });
                return;
            }
            opts.menuStyle = $(btn).val();
            initLayoutSetting(opts, true);
        });
        $(".page-toolbar .page-footer-option").change(function () {
            var btn = $(this);
            opts.footerMode = $(btn).val();
            initLayoutSetting(opts, true);
        });

        //初始化页面风格
        initLayoutSetting(opts, isUpdate);

        //初始化页面风格
        function initLayoutSetting(opts, isUpdate) {
            //同步界面显示
            $("#style_color").attr("href", $("#style_color").attr("data-href").format(opts.theme));
            $("#style_components").attr("href", $("#style_components").attr("data-href").format(opts.border == "rounded" ? "components-rounded" : "components"));
            if (opts.layoutBox == "boxed") {
                if (!$("body").hasClass("page-boxed")) {
                    $("body").addClass("page-boxed");
                    $('.page-header > .page-header-inner').addClass("container");
                    $('body > .clearfix').after('<div class="container"></div>');
                    $('.page-container').appendTo('body > .container');
                }
            }
            else {
                if ($("body").hasClass("page-boxed")) {
                    $("body").removeClass("page-boxed");
                    $('.page-header > .page-header-inner').removeClass("container");
                    $('.page-container').insertAfter('body > .clearfix');
                    $('.page-footer').insertAfter('.page-container');
                    $('body > .container').remove();
                }
            }

            if (opts.headerMode == "fixed") {
                $("body").addClass("page-header-fixed");
                $("body > .page-header").addClass("navbar-fixed-top");
                $("body > .page-header").removeClass("navbar-static-top");
            }
            else {
                $("body").removeClass("page-header-fixed");
                $("body > .page-header").removeClass("navbar-fixed-top");
                $("body > .page-header").addClass("navbar-static-top");
            }

            if (opts.headerMenu == "dark") {
                $(".top-menu > .navbar-nav > li.dropdown").addClass("dropdown-dark");
            }
            else {
                $(".top-menu > .navbar-nav > li.dropdown").removeClass("dropdown-dark");
            }

            if (opts.menuMode == "fixed") {
                $("body").addClass("page-sidebar-fixed");
                $("page-sidebar-menu").addClass("page-sidebar-menu-fixed");
                $("page-sidebar-menu").removeClass("page-sidebar-menu-default");
                Layout.initFixedSidebarHoverEffect();
            }
            else {
                $("body").removeClass("page-sidebar-fixed");
                $("page-sidebar-menu").addClass("page-sidebar-menu-default");
                $("page-sidebar-menu").removeClass("page-sidebar-menu-fixed");
                $('.page-sidebar-menu').unbind('mouseenter').unbind('mouseleave');
            }
            Layout.fixContentHeight();
            Layout.initFixedSidebar();

            if (opts.menuStyle == "hover") {
                $(".page-sidebar-menu").addClass("page-sidebar-menu-hover-submenu");
            }
            else {
                $(".page-sidebar-menu").removeClass("page-sidebar-menu-hover-submenu");
            }

            if (opts.footerMode == "fixed") {
                $("body").addClass("page-footer-fixed");
            }
            else {
                $("body").removeClass("page-footer-fixed");
            }

            //同步数据到数据库
            if (isUpdate) {
                var model = {
                    Layout: "_Layout",
                    Setting: JSON.stringify(opts),
                    UserId: appInfo.loginUser.Id
                };
                $.ajax({
                    type: "POST",
                    dataType: "json",
                    data: { model: model },
                    url: getUrl("AdmSetting", "ChangeTemplateSetting")
                });
            }
        }
    }

    //初始化菜单
    function initMenu(menus) {
        var leftMenus = null //左侧所有菜单
        var topMenu = null;//顶部菜单

        //顶部菜单
        $(menus).each(function (i, m) {
            if (m.isTop == "1") {
                m.href = getUrl(m.href);
                var dom = $('<li>' +
                    '<a href="{href}"> '.format(m) +
                    '<i class="fa {class}"></i> {title}'.format(m) +
                    '</a> ' +
                    '</li>');
                if (isInMenu(m, opts.menuName)) {
                    topMenu = m;
                    $(dom).addClass("classic-menu-dropdown active");
                    $(dom).find("a").append("<span class='selected'></span>");
                }
                $("#navTop").append(dom);
            }
        });

        //左侧菜单
        if (topMenu) {
            leftMenus = topMenu.ChildEntitys;
        }
        else {
            leftMenus = menus;
        }
        if (leftMenus.length) {
            $(leftMenus).each(function (i, m1) {
                var html = "";
                html +=
                    "<li authority=\"" + m1.name + "\" class=\"m1\">" +
                    "<a href=\"" + getUrl(m1.href) + "\">" +
                    "<i class=\"" + m1["class"] + "\"></i>" +
                    "<span class=\"title\">" + m1.title + "</span>" +
                    "<span class=\"arrow\"></span>" +
                    "</a>";
                if (m1.ChildEntitys && m1.ChildEntitys.length > 0) {
                    html += "<ul class=\"sub-menu\">";
                    $(m1.ChildEntitys).each(function (j, m2) {
                        html += "<li authority=\"" + m2.name + "\">" +
                            "<a href=\"" + getUrl(m2.href) + "\">" +
                            "<i></i>{0}".format(m2.title) +
                            "</a>" +
                            "</li>";
                    });
                    html += "</ul>";
                }
                html += "</li>";

                var dom = $(html);
                if (!m1.ChildEntitys || m1.ChildEntitys.length == 0) {
                    dom.find(".arrow").remove();
                }
                $(".page-sidebar-menu").append(dom);
            });
            $(".page-sidebar-menu .sidebar-toggler-wrapper").nextAll("li").each(function (i, li) {
                if (i == 0) {
                    $(li).addClass("start");
                } else if (i == $(".page-sidebar-menu .sidebar-toggler-wrapper").nextAll("li").length - 1 && i != 0) {
                    $(li).addClass("last");
                }
            });

            //聚焦当前被选中的左侧菜单
            $(".page-sidebar-menu").find("li[authority='" + opts.menuName + "']").addClass("active");//二级菜单
            $(".page-sidebar-menu").find("li[authority='" + opts.menuName + "']").closest("li.m1").addClass("active open")
            $(".page-sidebar-menu").find("li[authority='" + opts.menuName + "']").closest("li.m1").find("span.arrow:first").addClass("open");
            $(".page-sidebar-menu").find("li[authority='" + opts.menuName + "']").closest("li.m1").find("span.arrow:first").before("<span class=\"selected\"></span>");
        }

        //判断菜单是否是某个菜单节点的子菜单
        function isInMenu(menu, childMenuName) {
            if (menu.name == childMenuName) return true;

            if (menu.ChildEntitys) {
                for (var i = 0; i < menu.ChildEntitys.length; i++) {
                    if (isInMenu(menu.ChildEntitys[i], childMenuName)) return true;
                }
            }

            return false;
        }
    }
}

/*****************************● 模板2***************************************************/
function admTemplate2(opts) {
    //初始化工具栏
    initToolbar(appInfo.admSetting && JSON.parse(appInfo.admSetting.Setting), false);

    //初始化菜单
    initMenu(appInfo.menus[0].ChildEntitys);

    //网站地图
    var firstActiceMenu = $("#navMenuBox li.active").eq(0);
    var lastActiceMenu = $("#navMenuBox li.active").eq(1);
    var pageTitle = "<h1>{0} <small><i class=\"fa fa-cog\"></i> {1}</small></h1>".format(opts.title || $(lastActiceMenu).text(), $(firstActiceMenu).find("span:first").text());
    $("#page_bar").append("<li><i class=\"fa fa-home\"></i><a href=\"index.html\">首页</a><i class=\"fa fa-angle-right\"></i></li>");
    if (firstActiceMenu.length) {
        $("#page_bar").append("<li><a href=\"{0}\">{1}</a><i class=\"fa fa-angle-right\"></i></li>"
            .format($(firstActiceMenu).find("a:first").attr("href"), $(firstActiceMenu).find("span:first").text()));
    }
    if (lastActiceMenu.length) {
        $("#page_bar").append("<li><a href=\"{0}\">{1}</a><i class=\"fa fa-angle-right\"></i></li>"
            .format($(lastActiceMenu).find("a:first").attr("href"), $(lastActiceMenu).find("a:first").text()));
    }
    $("#page_bar .fa-angle-right:last").remove();

    //初始化工具栏
    function initToolbar(opts, isUpdate) {
        var defaults = {
            theme: "default",
            border: "rounded",
            layoutBox: "fluid",
            headerMode: "default",
            headerMenu: "light",
            menuMode: "default",
            menuStyle: "dark"
        };
        opts = $.extend(defaults, opts);

        //工具栏
        $(".page-toolbar *[data-theme='{theme}']".format(opts)).addClass("active");
        $(".page-toolbar *[data-border='{border}']".format(opts)).attr("selected", true);
        $(".page-toolbar *[data-layoutBox='{layoutBox}']".format(opts)).attr("selected", true);
        $(".page-toolbar *[data-headerMode='{headerMode}']".format(opts)).attr("selected", true);
        $(".page-toolbar *[data-headerMenu='{headerMenu}']".format(opts)).attr("selected", true);
        $(".page-toolbar *[data-menuMode='{menuMode}']".format(opts)).attr("selected", true);
        $(".page-toolbar *[data-menuStyle='{menuStyle}']".format(opts)).attr("selected", true);

        //工具栏按钮点击事件
        $(".page-toolbar .theme-color").click(function () { //皮肤
            var btn = $(this);
            $(".page-toolbar .theme-color").removeClass("active");
            $(btn).addClass("active");

            opts.theme = $(btn).attr("data-theme");
            initLayoutSetting(opts, true);
        });
        $(".page-toolbar .layout-style-option").change(function () {
            var btn = $(this);
            opts.border = $(btn).val();
            initLayoutSetting(opts, true);
        });
        $(".page-toolbar .layout-option").change(function () {
            var btn = $(this);
            opts.layoutBox = $(btn).val();
            initLayoutSetting(opts, true);
        });
        $(".page-toolbar .page-header-option").change(function () {
            var btn = $(this);
            opts.headerMode = $(btn).val();
            if (opts.menuMode == "fixed" && $(btn).val() == "fixed") {
                $(".page-toolbar .page-header-option").find("option[value='default']").attr("selected", true);
                layer.alert("菜单定位为“固定”时不允许将头部定位设置为“固定”状态", { icon: 5 });
                return;
            }
            initLayoutSetting(opts, true);
        });
        $(".page-toolbar .page-header-top-dropdown-style-option").change(function () {
            var btn = $(this);
            opts.headerMenu = $(btn).val();
            initLayoutSetting(opts, true);
        });
        $(".page-toolbar .sidebar-option").change(function () {
            var btn = $(this);
            opts.menuMode = $(btn).val();
            if (opts.headerMode == "fixed" && $(btn).val() == "fixed") {
                $(".page-toolbar .sidebar-option").find("option[value='default']").attr("selected", true);
                layer.alert("头部定位为“固定”时不允许将菜单定位设置为“固定”状态", { icon: 5 });
                return;
            }
            initLayoutSetting(opts, true);
        });
        $(".page-toolbar .theme-setting-mega-menu-style-option").change(function () {
            var btn = $(this);
            opts.menuStyle = $(btn).val();
            initLayoutSetting(opts, true);
        });

        //初始化页面风格
        initLayoutSetting(opts, isUpdate);

        //初始化页面风格
        function initLayoutSetting(opts, isUpdate) {
            //同步界面显示
            $("#style_color").attr("href", $("#style_color").attr("data-href").format(opts.theme));
            $("#style_components").attr("href", $("#style_components").attr("data-href").format(opts.border == "rounded" ? "components-rounded" : "components"));
            if (opts.layoutBox == "boxed") {
                if ($(".page-header-top > .container-fluid").length) {
                    $('.page-header-top > .container-fluid').addClass("container").removeClass('container-fluid');
                    $('.page-header-menu > .container-fluid').addClass("container").removeClass('container-fluid');
                    $('.page-head > .container-fluid').addClass("container").removeClass('container-fluid');
                    $('.page-content > .container-fluid').addClass("container").removeClass('container-fluid');
                    $('.page-prefooter > .container-fluid').addClass("container").removeClass('container-fluid');
                    $('.page-footer > .container-fluid').addClass("container").removeClass('container-fluid');
                }
            }
            else {
                if (!$(".page-header-top > .container-fluid").length) {
                    $('.page-header-top > .container').removeClass("container").addClass('container-fluid');
                    $('.page-header-menu > .container').removeClass("container").addClass('container-fluid');
                    $('.page-head > .container').removeClass("container").addClass('container-fluid');
                    $('.page-content > .container').removeClass("container").addClass('container-fluid');
                    $('.page-prefooter > .container').removeClass("container").addClass('container-fluid');
                    $('.page-footer > .container').removeClass("container").addClass('container-fluid');
                }
            }

            if (opts.headerMode == "fixed") {
                $("body").addClass("page-header-top-fixed");
            }
            else {
                $("body").removeClass("page-header-top-fixed");
            }

            if (opts.headerMenu == "dark") {
                $(".top-menu > .navbar-nav > li.dropdown").addClass("dropdown-dark");
            }
            else {
                $(".top-menu > .navbar-nav > li.dropdown").removeClass("dropdown-dark");
            }

            if (opts.menuMode == "fixed") {
                $("body").addClass("page-header-menu-fixed");
            }
            else {
                $("body").removeClass("page-header-menu-fixed");
            }

            if (opts.menuStyle == "dark") {
                $(".hor-menu").removeClass("hor-menu-light");
            }
            else {
                $(".hor-menu").addClass("hor-menu-light");
            }

            //同步数据到数据库
            if (isUpdate) {
                var model = {
                    Layout: "_Layout2",
                    Setting: JSON.stringify(opts),
                    UserId: appInfo.loginUser.Id
                };
                $.ajax({
                    type: "POST",
                    dataType: "json",
                    data: { model: model },
                    url: getUrl("AdmSetting", "ChangeTemplateSetting")
                });
            }
        }
    }

    //初始化菜单
    function initMenu(menus) {
        //一级菜单
        $(menus).each(function (i, m1) {
            m1.href = getUrl(m1.href);
            var menu1 = $('<li class="menu-dropdown">' +
                '<a href="{href}"> '.format(m1) +
                '<i class="fa {class}"></i> {title}'.format(m1) +
                '</a></li>');
            if (m1.ChildEntitys && m1.ChildEntitys.length) {
                $(menu1).children("a")
                    .attr("data-hover", "megamenu-dropdown")
                    .attr("data-close-others", "true")
                    .attr("data-toggle", "dropdown")
                    .attr("class", "dropdown-toggle")
                    .append(" <i class=\"fa fa-angle-down\"></i>");
            }

            if (isInMenu(m1, opts.menuName)) {
                $(menu1).addClass("classic-menu-dropdown active");
            }
            else {
                $(menu1).addClass("classic-menu-dropdown");
            }

            $("#navMenuBox").append(menu1);

            //二级菜单
            if (m1.ChildEntitys) {
                var menuBox2 = null;
                $(m1.ChildEntitys).each(function (j, m2) {
                    if (j == 0) {
                        menuBox2 = $("<ul class=\"dropdown-menu pull-left\"></ul>");
                        $(menu1).append(menuBox2);
                    }

                    m2.href = getUrl(m2.href);
                    var menu2 =
                        $("<li authority=\"{name}\">".format(m2) +
                        "<a href=\"{href}\">".format(m2) +
                        "<i class=\"{class}\"></i> ".format(m2) +
                        "<span class=\"title\">{title}</span>".format(m2) +
                        "</a></li>");
                    if (m2.ChildEntitys && m2.ChildEntitys.length) {
                        $(menu2).addClass("dropdown-submenu");
                    }
                    if (isInMenu(m2, opts.menuName)) {
                        $(menu2).addClass("active");
                    }
                    $(menuBox2).append(menu2);

                    //三级菜单
                    if (m2.ChildEntitys && m2.ChildEntitys.length > 0) {
                        var menuBox3 = null;
                        $(m2.ChildEntitys).each(function (k, m3) {
                            if (k == 0) {
                                menuBox3 = $("<ul class=\"dropdown-menu\"></ul>");
                                $(menu2).append(menuBox3);
                            }

                            m3.href = getUrl(m3.href);
                            var menu3 =
                                $("<li authority=\"{name}\">".format(m3) +
                                    "<a href=\"{href}\">".format(m3) +
                                    "<i class=\"{class}\"></i>".format(m3) +
                                    "<span class=\"title\">{title}</span>".format(m3) +
                                    "</a></li>");
                                if (m3.ChildEntitys && m3.ChildEntitys.length) {
                                    $(menu3).addClass("dropdown-submenu");
                                }
                                if (isInMenu(m3, opts.menuName)) {
                                    $(menu3).addClass("active");
                                }
                            $(menuBox3).append(menu3);
                        });
                    }
                });
            }
        });

        //下拉触发菜单显示
        $('#navMenuBox .dropdown-toggle').dropdownHover();

        //判断菜单是否是某个菜单节点的子菜单
        function isInMenu(menu, childMenuName) {
            if (menu.name == childMenuName) return true;

            if (menu.ChildEntitys) {
                for (var i = 0; i < menu.ChildEntitys.length; i++) {
                    if (isInMenu(menu.ChildEntitys[i], childMenuName)) return true;
                }
            }

            return false;
        }
    }
}

/*****************************● 获取站点信息********************************************/
function getAppInfo() {
    var appInfo = null;

    $.ajax({
        async: false,
        type: "POST",
        dataType: "json",
        url: getUrl("Shared", "GetAppInfo"),
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

/*****************************● 切换角色************************************************/
function switchRole(roleId) {
    $.ajax({
        type: "POST",
        dataType: "json",
        data: { roleId: roleId },
        url: getUrl("User", "SwitchRole"),
        success: function (data) {
            if (data.Type == "Success") {
                window.location.href = getUrl("Home", "Index");
            }
            else {
                $dialogShow(data);
            }
        }
    });
}

/*****************************● 点击注销************************************************/
function btnLoginOut() {
    $dialogShow({
        Type: "confirm",
        Title: "注销",
        Content: "确定要注销当前登陆用户<strong style='color:red'>" + $("#userAccount").text() + "</strong>吗？",
        SureFn: function (modal) {
            $(modal).on('hidden.bs.modal', function () {
                loginOut(true);
            }).modal("hide");
        }
    });
}

/*****************************● 退出登录************************************************/
function loginOut(goLogin) {
    $.ajax({
        type: "POST",
        dataType: "json",
        url: getUrl("User", "LoginOut"),
        success: function (data) {
            if (goLogin) {
                window.location.href = getUrl("User", "Login");
            }
        }
    });
}

/*****************************● 窗体登录************************************************/
function dialodLogin() {
    var username = $.trim($(".modal-login .user-name").html());
    var password = $(".modal-login .user-password").val();

    var loginModel = {
        Name: username,
        Password: password
    }

    if (password == "") {
        $(".help-block").html("* 请输入密码");
        return;
    }
    $.ajax({
        async: true,
        type: "POST",
        data: loginModel,
        url: getUrl("User", "Login"),
        success: function (data) {
            if (!data.IsSucceed) {
                $(".help-block").html("* 密码验证失败");
                return false;
            }
            $(".modal-login").modal("hide");
            return true;
        }
    });
}

/*****************************● 解除锁屏************************************************/
function dialodLock() {
    var username = $.trim($(".modal-lock .user-name").html());
    var password = $(".modal-lock .user-password").val();

    var loginModel = {
        Name: username,
        Password: password
    }

    if (password == "") {
        $(".help-block").html("* 请输入密码");
        return;
    }
    $.ajax({
        async: true,
        type: "POST",
        data: loginModel,
        dataType: "json",
        url: getUrl("User", "Login"),
        success: function (data) {
            if (!data.IsSucceed) {
                $(".help-block").html("* 密码验证失败");
                return false;
            }
            $(".modal-lock").modal("hide");
            return true;
        }
    });
}

