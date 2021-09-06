/*****************
参考拖拽插件（拖放）：http://www.css88.com/jquery-ui-api/draggable/
参考拖拽插件（放置）：http://www.css88.com/jquery-ui-api/droppable/index.html#option-accept
参考排序插件（排序）：http://www.css88.com/jquery-ui-api/sortable/#option-placeholder
******************/

/**************************获取引擎对象****************************/
formSetting = {
    plugins: ["form", "column", "text", "textarea"],
    fsPlugins: [],
    getFsPlugin: function (lname) {
        for (var i = 0 ; i < formSetting.fsPlugins.length ; i++) {
            if (formSetting.fsPlugins[i].lname == lname) {
                return formSetting.fsPlugins[i];
            }
        }
        return null;
    },
    getObj: function (options) {
        //初始化参数
        var defaults = {
            cst: null
        };
        var opts = $.extend(defaults, options);
        var obj = {
            cst: opts.cst,
            getParams: function () {
                return opts;
            }
        };

        //初始化左侧工具栏悬浮
        $(window).scroll(function () {
            var scrollTop = $(window).scrollTop();

            var navtop = 107 - scrollTop;     
            if (navtop <= 46)
                navtop = 46;
            $(obj.cst).find(".cst-nav").css("top", navtop + "px");

            var bodytop = scrollTop;
            if (bodytop <= (107 - 46))
                bodytop = 0;
            else
                bodytop = scrollTop - 66;
            $(obj.cst).find(".cst-body").css("top", bodytop + "px");
        });

        //初始化左侧工具栏效果
        $('#collapseLayout').collapse('show');
        $('#collapseForm').collapse('show');
        $('#collapseTxt').collapse('show');

        //初始化工具箱、属性切换状态
        $(obj.cst).find(".cst-nav-tab .tbtn").click(function () {
            var tbtn = this;
            var name = $(tbtn).attr("name");

            $(obj.cst).find(".cst-nav-tab .tbtn").removeClass("tbtn-active");
            $(tbtn).addClass("tbtn-active");

            $(obj.cst).find(".cst-nav-body").hide();
            $(obj.cst).find(".cst-nav-body[name='{0}']".format(name)).show();
        });

        //初始化左侧工具栏插件
        $(formSetting.plugins).each(function (i, lname) {
            var plugin = fsPlugin[lname].init();
        });

        return obj;
    },
    getTemplate: function (name, callback) {
        var template = "";
        $.ajax({
            type: "get",
            cache: true,
            async: callback == null ? false : true,//如果没有回调函数，则必须采用同步的ajax
            url: "/Areas/Manager/Scripts/formSetting/JsTmplate.xml",
            dataType: "xml",
            error: function (xml) {
                alert('未能加载XML文档' + xml);
            },
            success: function (xml) {
                var all = $(xml).find("#template").text(); 
                template = $.trim($(all).find(name).prop("outerHTML")); 
                if (callback) {
                    callback(template);
                }
            }
        });
        return template;
    }
}

/**************************插件基类****************************/
//插件基类
fsPlugin = function (son) {
    this.lname = son.lname;
    this.rname = son.rname;
    this.rin = son.rin || son.rname;
    this.dragHelper = son.dragHelper || this.dragHelper;
    this.loadPropBoxAfter = son.loadPropBoxAfter || this.loadPropBoxAfter;

    this.rcontrol = $(formSetting.getTemplate("*[pluginname='{0}']".format(this.lname)));
}
fsPlugin.prototype = {
    lname: null,
    rname: null,
    rcontrol: null,
    rin: null,
    loadPropBoxAfter: function (control) { },
    load: function () {
        var self = this;
        
        //拖拽
        var tooldrag = $(".lt_tooldrag[pluginname='{0}']".format(self.lname));
        $(tooldrag).draggable({
            appendTo: 'body',
            zIndex: 99999,
            drag: function (e, ui) {
                if (!$(self.rname).hasClass("drap-waiting")) {
                    $(self.rname).addClass("drap-waiting"); 
                    $(self.rname).append("<div class=\"control-will col-md-{1}\" style=\"height:{0}px\"></div>".format(
                        $(self.rcontrol).attr("willheight"), $(self.rcontrol).bootGetClassWidth("col-md")));
                }
            },
            stop: function () {
                $(self.rname).removeClass("drap-waiting");
                $(self.rname).find(".control-will").remove();
            },
            helper: function () {
                return self.dragHelper();
            }
        });
        
        //如果是第一个插件，则初始化其放置控件
        if (self.lname == "form") {
            $(self.rname).droppable({
                activeClass: 'dropzone-active',
                hoverClass: 'dropzone-hover',
                accept: tooldrag,
                greedyType: false,
                toleranceType: "pointer",
                drop: function (e, ui) {
                    self.drop(e, ui.draggable, this, self.rin);
                }
            });
        }
    },
    dragHelper: function () {
        var self = this;

        return "<div>" 
                 "<img src=\"/Areas/Manager/Scripts/formSetting/images/lt_tooldrag_{0}.png\" style=\"height:150px;\" />"
               "</div>".format(self.lname);
    },
    drop: function (e, l, r, rin) {
        if ($(".ui-sortable-placeholder").length) return; //当前处于拖动状态

        var self = this;
        var pluginname = $(l).attr("pluginname");
        var fsPlugin = formSetting.getFsPlugin(pluginname);

        //放置控件
        var control = $(formSetting.getTemplate("*[pluginname='{0}']".format(pluginname)));
        $(control).append("<div class=\"control-toolbar\">" +
                             "<a class=\"control-btn\" onclick=\"fsPlugin.loadPropBox(this)\" title=\"属性\"><i class=\"fa fa fa-eye\"></i> 属性</a>" +
                             "<a class=\"control-btn\" onclick=\"fsPlugin.removePlugin(this)\" title=\"删除\"><i class=\"fa fa-trash\"></i> 删除</a>" +
                          "<div>");
        if (fsPlugin.rname == fsPlugin.rin) {
            $(r).append(control);
        }
        else {
            $(r).find(rin).append(control);
        }

        //绑定控件事件
        $(control).click(function (e) {
            $(".cst-body .edit-active")
                .removeClass("edit-active")
                .children(".control-toolbar").hide();
            $(control)
                .removeClass("edit-hover")
                .addClass("edit-active")
                .children(".control-toolbar").show();

            return false;
        });

        $(control).hover(function (e) {
            $(".cst .edit-hover").removeClass("edit-hover");
            $(".cst").find(".control-toolbar").each(function (i, toolbar) {
                var control = $(toolbar).closest(".control");
                if (!control.hasClass("edit-active"))
                    $(toolbar).hide();
            });
            
            if (!$(control).hasClass("edit-active")) {
                $(control).addClass("edit-hover");
                $(control).children(".control-toolbar").show();
            }

            return false;
        }, function (e) {
            $(".cst .edit-hover").removeClass("edit-hover");
            $(".cst").find(".control-toolbar").each(function (i, toolbar) {
                var control = $(toolbar).closest(".control");
                if (!control.hasClass("edit-active"))
                    $(toolbar).hide();
            });

            var fcontrol = $(e.currentTarget).parents(".control");
            if (!$(fcontrol).hasClass("edit-active")) {
                $(fcontrol).addClass("edit-hover");
                $(fcontrol).children(".control-toolbar").show();
            };

            return false;
        });

        //递归初始化被放入的控件的放置
        $(control).droppable({
            activeClass: 'dropzone-active',
            hoverClass: 'dropzone-hover',
            greedyType: false,
            toleranceType: "pointer",
            drop: function (e, ui) {
                var fsPlugin = formSetting.getFsPlugin($(ui.draggable).attr("pluginname"));

                self.drop(e, ui.draggable, $(control), fsPlugin.rin);
            }
        }).sortable({
            items: '.control',
            opacity: 0.5,
            revert: 300
        });
    },
};

//移除插件
fsPlugin.removePlugin = function (btn) {
    var control = $(btn).closest(".control");
    control.remove();
}

//加载属性框
fsPlugin.loadPropBox = function (btn) {
    var control = $(btn).closest(".control");
    var pluginname = $(control).attr("pluginname");
    var propertyBox = $(formSetting.getTemplate(".property-box[pluginname='{0}']".format(pluginname)));
    var fsPlugin = formSetting.getFsPlugin(pluginname);
    
    $("#cst_property").html(propertyBox);

    fsPlugin.loadPropBoxAfter(control);

    $(".cst-nav-tab .tbtn[name='property']").trigger("click");
}

/**************************插件子类****************************/
//表单 - form
fsPlugin.form = {
    lname: "form",
    rname: ".control-container",
    rin: ".control-container",
    init: function () {
        var plugin = new fsPlugin(this);
        plugin.load();
        formSetting.fsPlugins.push(plugin);
    }
}

//栏目 - column
fsPlugin.column = {
    lname: "column",
    rname: ".control-form",
    rin: ".form-body",
    loadPropBoxAfter: function (control) {
        var model = {
            name: $(control).find(".p-name").html()
        };

        //栏目名称
        $("#cst_property .s-name")
        .val(model.name)
        .change(function () {
            $(control).find(".p-name").html($(this).val());
        });
    },
    dragHelper: function () {
        return "<div><img src=\"/Areas/Manager/Scripts/formSetting/images/lt_tooldrag_column.png\" style=\"width:500px;height:47px;\" /></div>";
    },
    init: function () {
        var plugin = new fsPlugin(this);
        plugin.load();
        formSetting.fsPlugins.push(plugin);
    }
}

//单行文本框 - text
fsPlugin.text = {
    lname: "text",
    rname: ".control-form",
    rin: ".form-body",
    loadPropBoxAfter: function (control) {
        var model = {
            name: $(control).find(".p-name").html(),
            placeholder: $(control).find("input").attr("placeholder"),
            isRequired: $(control).find("required").length ? "1" : "0",
            width: $(control).bootGetClassWidth("col-md"),
            inputWidth: $(control).find("input").closest("div").bootGetClassWidth("col-md")
        };
        
        //表单名称
        $("#cst_property .s-name")
            .val(model.name)
            .change(function () {
                $(control).find(".p-name").html($(this).val());
            });

        //placeholder
        $("#cst_property .s-placeholder")
            .val(model.placeholder)
            .change(function () {
                $(control).find("input").attr("placeholder", $(this).val());
            });

        //是否必填
        $("#cst_property .s-isRequired")
            .val(model.isRequired)
            .change(function () {
                var s = this;
                if ($(s).val() == "1" && !$(control).find(".required").length) {
                    $(control).find(".control-label").prepend("<span class=\"required\"> * </span>");
                }
                else {
                    $(control).find(".required").remove();
                }
            }).trigger("change");

        //宽度
        $("#cst_property .s-width")
            .val(model.width.toString())
            .change(function () {
                $(control).bootAddClass("col-md-{0}".format($(this).val()));
            });

        //输入框宽度
        $("#cst_property .s-inputWidth")
            .val(model.inputWidth.toString())
            .change(function () {
                $(control).find("input").closest("div").bootAddClass("col-md-{0}".format($(this).val()));
            });
    },
    dragHelper: function () {
        return "<div><img src=\"/Areas/Manager/Scripts/formSetting/images/lt_tooldrag_text.png\" style=\"width:200px;height:30px;\" /></div>";
    },
    init: function () {
        var plugin = new fsPlugin(this);
        plugin.load();
        formSetting.fsPlugins.push(plugin);
    }
}

//多行文本框 - textarea
fsPlugin.textarea = {
    lname: "textarea",
    rname: ".control-form",
    rin: ".form-body",
    loadPropBoxAfter: function (control) {
        var model = {
            name: $(control).find(".p-name").html(),
            placeholder: $(control).find("textarea").attr("placeholder"),
            isRequired: $(control).find("required").length ? "1" : "0",
            width: $(control).bootGetClassWidth("col-md"),
            inputWidth: $(control).find("textarea").closest("div").bootGetClassWidth("col-md"),
            rows: $(control).find("textarea").attr("rows")
        };

        //表单名称
        $("#cst_property .s-name")
            .val(model.name)
            .change(function () {
                $(control).find(".p-name").html($(this).val());
            });

        //placeholder
        $("#cst_property .s-placeholder")
            .val(model.placeholder)
            .change(function () {
                $(control).find("textarea").attr("placeholder", $(this).val());
            });

        //是否必填
        $("#cst_property .s-isRequired")
            .val(model.isRequired)
            .change(function () {
                var s = this;
                if ($(s).val() == "1" && !$(control).find(".required").length) {
                    $(control).find(".control-label").prepend("<span class=\"required\"> * </span>");
                }
                else {
                    $(control).find(".required").remove();
                }
            }).trigger("change");

        //宽度
        $("#cst_property .s-width")
            .val(model.width.toString())
            .change(function () {
                $(control).bootAddClass("col-md-{0}".format($(this).val()));
            });

        //输入框宽度
        $("#cst_property .s-inputWidth")
            .val(model.inputWidth.toString())
            .change(function () {
                $(control).find("textarea").closest("div").bootAddClass("col-md-{0}".format($(this).val()));
            });

        //行
        $("#cst_property .s-rows")
            .val(model.rows.toString())
            .change(function () {
                $(control).find("textarea").attr("rows", $(this).val());
            });
    },
    dragHelper: function () {
        return "<div><img src=\"/Areas/Manager/Scripts/formSetting/images/lt_tooldrag_textarea.png\" style=\"width:200px;height:30px;\" /></div>";
    },
    init: function () {
        var plugin = new fsPlugin(this);
        plugin.load();
        formSetting.fsPlugins.push(plugin);
    }
}

