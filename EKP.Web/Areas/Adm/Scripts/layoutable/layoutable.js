window.layoutable = {
    //初始化
    init: function (opt) {
        var param = {
            box: null,
            data: null
        };
        opt = $.extend(param, opt);

        //添加布局
        var layoutTool = createLayoutTool($(opt.box)); 
        $(opt.box).prepend(layoutTool);

        //拖拽
        $(opt.box).sortable({
            handle: ".drag",
            items: '.lyarea',
            tolerance: "pointer",
            helper: "clone",
            start: function (event, ui) {
                $(ui.placeholder).append("<div class='view'></div>");
            }
        });

        //初始化已有数据
        if (opt.data) {
            $(opt.data.HomeLayoutAreas).each(function (i, homeLayoutArea) {
                //初始化区域
                var area = layoutable.createArea($(opt.box), {
                    width: homeLayoutArea.Width,
                    homeLayoutAreaId: homeLayoutArea.Id
                });

                //初始化模块
                if (homeLayoutArea.HomeLayoutModules) {
                    $(homeLayoutArea.HomeLayoutModules).each(function (j, homeLayoutModule) {
                        layoutable.createModule(area, {
                            id: homeLayoutModule.Id,
                            moduleId: homeLayoutModule.ModuleId,
                            width: homeLayoutModule.Width,
                            height: homeLayoutModule.Height,
                            moduleName: homeLayoutModule.ModuleName
                        });
                    });
                }
            });
        }

        //刷新全部
        setTimeout(function () {
            layoutable.refreshAll(opt.box)
        }, 200); 

        //添加布局工具栏
        function createLayoutTool(layout) {
            var tool = $(
                '<a href="#close" class="remove label label-important"><i class="fa fa-times"></i> 删除</a>' +
                '<span class="drag label"><i class="fa fa-arrows"></i>拖动</span>' +
                '<span class="btn-group add">' +
                '<a class="btn btn-default btn-xs dropdown-toggle" data-toggle="dropdown" href="#"><i class="fa fa-plus"></i> 添加 <i class="fa fa-angle-down"></i></a>' +
                '<ul class="dropdown-menu">' +
                '<li class="active"><a href="javascript:;" class="btnCreateArea">区域</a></li>' +
                '</ul>' +
                '</span>');
            $(tool).find(".btnCreateArea").click(function () {
                layoutable.createArea(layout);
            });

            return tool;
        }
    },

    //刷新全部
    refreshAll: function (dropzone) {
        $(dropzone).find(".lyarea").each(function (i, area) {
            layoutable.refreshAreaSize(area);
        });
        $(dropzone).find(".lymodule").each(function (i, module) {
            layoutable.refreshModuleSize(module);
        });
    },

    //添加区域
    createArea: function (layout, opt) {
        var param = {
            homeLayoutAreaId: null,
            width: null,
            height: 100
        };
        var opt = $.extend(param, opt);

        var area = $(
            '<div class="area lyarea" d-homeLayoutAreaId="{homeLayoutAreaId}">'.format(opt) +
                '<div class="view">' +
                    '<div class="area-fluid clearfix">' +
                         '<div class="sizefix"></div>' +
                    '</div > ' +
                '</div > '+
            '</div >');
        $(layout).append(area);

        //设置宽度
        if (opt.width) {
            $(area).css("width", opt.width + "px");
        }
        else {
            var layoutWidth = parseInt($(layout).css("width")) - 20;
            $(area).css("width", (layoutWidth) / 2) + "px";
        }

        //设置高度
        $(area).css("height", opt.height + "px");

        //添加区域工具栏
        var areaTool = createAreaTool(area);
        $(area).prepend(areaTool);

        //刷新区域size
        setTimeout(function () {
            layoutable.refreshAreaSize(area);
        }, 200); 

        //拉升
        $(area).find(".view").resizable({
            grid: [5, 5],
            handles: "e",
            resize: function (event, ui) {
                var w = parseInt($(ui.element).width());
                $(area).css("width", (w + 20).toString() + "px");
                layoutable.refreshAll(layout);
            }
        });

        //删除
        $(area).find(".remove").click(function () {
            $(area).find(".view").resizable("destroy");;
            $(area).remove();
        });

        return area;

        //添加区域工具栏
        function createAreaTool(area) {
            var tool = $(
                '<a href="javascript:;" class="remove label label-important"><i class="fa fa-times"></i> 删除</a>' +
                '<span class="drag label"><i class="fa fa-arrows"></i>拖动</span>' +
                '<span class="btn-group add">' +
                '<a class="btn btn-default btn-xs dropdown-toggle" data-toggle="dropdown" href="#"><i class="fa fa-plus"></i> 添加 <i class="fa fa-angle-down"></i></a>' +
                '<ul class="dropdown-menu">' +
                '<li class="active"><a href="javascript:;" class="btnCreateModule">模块</a></li>' +
                '</ul>' +
                '</span>');
            $(tool).find(".btnCreateModule").click(function () {
                layoutable.openCreateModule(area);
            });

            return tool;
        }
    },

    //刷新区域size
    refreshAreaSize: function (area) {
        var width = $(area).find(".view:first").outerWidth(); 
        var height = $(area).find(".view:first").outerHeight();
        var sizeWidth = $(area).find(".sizefix").outerWidth();
        var sizeHeight = $(area).find(".sizefix").height(); 
        $(area).find(".sizefix").html("{0}*{1}".format(width, height));
        $(area).find(".sizefix").css("left", (width - sizeWidth) / 2 + "px");
        $(area).find(".sizefix").css("top", (height - sizeHeight) / 2 + "px");
    },

    //添加模块
    createModule: function (area, opt) {
        var module = $(
            '<div class="module lymodule" title="{moduleName}" d-moduleId="{moduleId}" d-Id="{id}" style="height:{height}px">'.format(opt) +
            '<div class="view">' +
            '<div class="module-fluid clearfix">' +
                '<div class="fix">{moduleName}</div>'.format(opt) +
                '<div class="sizefix"></div>' +
            '</div>' +
            '</div>' +
            '</div>');

        $(area).find(".area-fluid").append(module);
        $(area).css("height", "auto");

        //设置宽度
        if (opt.width) {
            $(module).css("width", opt.width + "px");
        }
        else {
            var areaWidth = parseInt($(area).css("width")); 
            $(module).css("width", (areaWidth / 2 - 20) + "px");
        }

        //添加模块工具栏
        var moduleTool = createModuleTool();
        $(module).prepend(moduleTool);

        //刷新模块size
        setTimeout(function () {
            layoutable.refreshModuleSize(module);
        }, 1000); 

        //拉升
        $(module).find(".view").resizable({
            grid: [5, 5],
            handles: "e, s",
            resize: function (event, ui) {
                var w = parseInt($(ui.element).width());
                var h = parseInt($(ui.element).height());
                $(module).css("width", (w + 20).toString() + "px");
                $(module).css("height", (h + 20).toString() + "px");
                layoutable.refreshModuleSize(module);
            }
        });

        //拖拽
        $(area).sortable({
            connectWith: $('.lyarea'),
            handle: ".drag",
            items: '.lymodule',
            tolerance: "pointer",
            helper: "clone",
            start: function (event, ui) {
                $(ui.placeholder).append("<div class='view'></div>");
            }
        });

        //删除
        $(module).find(".remove").click(function () {
            $(module).find(".view").resizable("destroy");;
            $(module).remove();
        });

        //添加模块工具栏
        function createModuleTool() {
            var tool = $(
                '<a href="javascript:;" class="remove label label-important"><i class="fa fa-times"></i> 删除</a>' +
                '<span class="drag label"><i class="fa fa-arrows"></i>拖动</span>');

            return tool;
        }
    },

    //刷新模块size
    refreshModuleSize: function (module) {
        var width = $(module).find(".view:first").outerWidth();
        var height = $(module).find(".view:first").outerHeight();
        var sizeWidth = $(module).find(".sizefix").outerWidth();
        var sizeHeight = $(module).find(".sizefix").height();
        $(module).find(".sizefix").html("{0}*{1}".format(width, height));
        $(module).find(".sizefix").css("left", (width - sizeWidth) / 2 + "px");
        $(module).find(".sizefix").css("top", (height - sizeHeight) / 2 + "px");
    },

    //打开添加模块
    openCreateModule: function (area) {
        $.ajax({
            url: getUrl("SiteModule", "ChooseModule"),
            dataType: 'html',
            type: 'get',
            beforeSend: function () {
                layer.load(0, { shade: false });
            },
            success: function (html) {
                layer.closeAll("loading");
                layer.confirm(html, {
                    title: '<i class="fa fa-plus"></i> 添加模块',
                    area: ['900px', '600px'],
                    btn: ['<i class="fa fa-save"></i>&nbsp;保存', '<i class="fa fa-times"></i>&nbsp;取消']
                }, function (v, box) {
                    $("#chooseModule").find(".mix.active").each(function (i, cModule) {
                        layoutable.createModule(area, {
                            id: null,
                            moduleId: $(cModule).attr("d-id"),
                            moduleName: $(cModule).find(".mix-inner").attr("title"),
                            height: 200
                        });
                    });

                    layer.close(v);
                });

                initChooseModule();
            }
        });
    },
}