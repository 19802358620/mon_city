/* 
    作    者：胡政
    描    述：通用工具js包
    创建时间：2015-08-28
    联系方式：13436053642
*/
/* 通用项目js方法
*/

/********************************************● 初始化加载************************************************************/
(function() {
    /*********************● 初始化表单************************************************************/
    $.fn.initForm = function() {
        var forms = null;
        if ($(this).prop("tagName").toLowerCase() == "form" || $(this).prop("tagName").toLowerCase() == "div") {
            forms = $(this).find("*");
        } else {
            forms = $(this);
        }

        $(forms).each(function(i, input) {
            //select2控件
            if ($(input).prop("tagName").toLowerCase() == "select") {
                $(input).select2();
            }
            //icheck控件
            else if ($(input).prop("tagName").toLowerCase() == "input" && ($(input).attr("type") == "radio" || $(input).attr("type") == "checkbox")) {
                $(input).iCheck({
                    checkboxClass: "icheckbox_minimal-grey",
                    radioClass: "iradio_minimal-grey"
                });
            }
            //图片上传
            else if ($(input).hasClass("imageDialog")) {
                var editor = KindEditor.editor({
                    uploadJson: '/Areas/Base/Scripts/kindeditor-4.1.10/asp.net/upload_json.ashx',
                    fileManagerJson: '/Areas/Base/Scripts/kindeditor-4.1.10/asp.net/file_manager_json.ashx',
                    allowFileManager: true
                });
                var img = $(input).find("img");
                var imgInput = $(input).find("input");
                var btn = $(input).find("button");
                KindEditor(btn).click(function() {
                    editor.loadPlugin('image', function() {
                        editor.plugin.imageDialog({
                            imageUrl: KindEditor(imgInput).val(),
                            clickFn: function(url, title, width, height, border, align) {
                                KindEditor(imgInput).val(url);
                                editor.hideDialog();
                                $(img).attr("src", url);
                                $(imgInput).val(url);
                            }
                        });
                    });
                });
            }
        });
    }

    /*********************● 清除表单数据**********************************************************/
    $.fn.clearData = function() {
        $(this).each(function(i, input) {
            $(input).val("");
        });
    }

    /*********************● 表单数据绑定**********************************************************/
    $.fn.bindData = function(data) {
        if (!data) return;
        $(this).each(function(i, input) {
            var name = $(input).attr("name");
            if (!name) return true;
            var fieldArray = name.split(".");
            if (!fieldArray) return true;
            var fieldName = fieldArray[fieldArray.length - 1];

            //普通select控件
            if ($(input).prop("tagName").toLowerCase() == "select" && !$(input).hasClass("select2-offscreen")) {
                $(input).val(data[fieldName] == null ? "" : data[fieldName].toString());
            }
            //select2控件
            else if ($(input).hasClass("select2-offscreen")) {
                //用select标签初始化的select2控件
                if ($(input).prop("tagName").toLowerCase() == "select") {
                    $(input).select2("val", data[fieldName] == null ? "" : data[fieldName].toString());
                }
            }
            //date-picker控件
            else if ($(input).hasClass("date-picker")) {
                $(input).closest("div.date-picker").datepicker("update", data[fieldName]);
            }
            //icheck-radio控件
            else if ($(input).hasClass("icheck") && $(input).attr("type") == "radio") {
                if (data[fieldName] == $(input).val())
                    $(input).iCheck('check');
            }
            //icheck-checkbox控件
            else if ($(input).hasClass("icheck") && $(input).attr("type") == "checkbox") {
                if ($(input).val() && data[fieldName].indexOf($(input).val()) > -1)
                    $(input).iCheck('check');
            }
            //其他input控件
            else {
                if (data[fieldName]) {
                    $(input).val(data[fieldName]);
                } else {
                    $(input).val(data[fieldName]);
                }
            }
        });
    }

    /*********************● 表单验证方法**********************************************************/
    $.fn.metronicValidate = function() {
        var metronicValidateHelper = {
            //验证是否为空
            validate_required: function(str) {
                if (!str || $.trim(str) == "")
                    return false;
                return true;
            }
        };
        var isValidated = true;
        $(".validate-error").each(function() {
            var inputName = $(this).attr("errorfor");
            var input = null;
            var inputValue = null;

            if (!inputName) return false;

            input = $("*[name='" + inputName + "']");
            if (!input) return false;

            inputValue = $(input).val();
            if ($(input).hasClass("required") && !metronicValidateHelper.validate_required(inputValue)) {
                $(this).html("带 * 为必填项");
                isValidated = false;
            }
            return true;
        });
        return isValidated;
    };

    /*********************● 获取bootstrap类名*****************************************************/
    $.fn.bootGetClass = function(btc) {
        var self = this;

        var btr = null;
        $($(self).attr("class").split(" ")).each(function(i, className) {
            if (!className) return;

            if (className.indexOf(btc) == 0)
                btr = className;
        });

        return btr;
    }

    /*********************● 获取bootstrap类宽度***************************************************/
    $.fn.bootGetClassWidth = function(btc) {
        var self = this;
        var className = $(self).bootGetClass(btc);

        if (className)
            return parseInt(className.split("-")[2]);

        return null;
    }

    /*********************● 添加bootstrap类名*****************************************************/
    $.fn.bootAddClass = function(addName) {
        var self = this;
        var className = $(self).bootGetClass(addName.substr(0, 7));

        $(self).removeClass(className).addClass(addName);
    }

    /*********************● 分页插件**************************************************************/
    $.fn.jqPager = function(options, params) {
        var $this = this;
        var plugin = getPlugin($this);

        var defaults = {
            scrollPager: false, //是否滚动分页
            scrollTrigger: "document", //滚动分页触发元素

            //ajax请求分页参数
            pageNo: 1,
            pageSize: 10,
            sortBy: null,
            sortOrder: null,
            pmap: {
                pageNo: "Page",
                pageSize: "PageSize",
                sortBy: "SortBy",
                sortOrder: "SortOrder"
            },

            jsonReader: {
                total: "TotalRecords",
                totalPages: "TotalPages"
            },

            //ajax请求参数
            url: null,
            type: "post",
            dataType: "json",
            postData: {}, //额外的请求参数

            //ajax请求结果
            postResult: null,

            //分页html模板
            pagerTempalte: "t1",

            //事件
            loadBefore: null, //请求数据前执行方法
            loadComplete: null //请求数据成功后执行方法
        };

        if (options == null || typeof (options) == "object") {
            var plugins = initPlugin();
            return plugins;
        } else if (options == "gotoPage") {
            gotoPage(params);
        } else if (options == "setPostData") {
            return setPostData(params);
        } else if (options == "getPostData") {
            return getPostData();
        } else if (options == "getPostResult") {
            return getPostResult();
        }

        //初始化插件
        function initPlugin() {
            var initBox = function($this) {
                var opts = $.extend(defaults, options);
                var plugin = getPlugin($this);
                setOpts(opts);
                gotoPage();
            };

            return $this.each(function(i, jqPager) {
                var jqPager = $(jqPager);
                $.fn.jqPager.plugins = $.fn.jqPager.plugins || [];
                $.fn.jqPager.plugins.push(jqPager);

                initBox(jqPager);
            });
        }

        //获取插件
        function getPlugin(jqPager) {
            var plugin = null;
            $($.fn.jqPager.plugins).each(function(i, p) {
                if (p[0] == jqPager[0]) {
                    plugin = p;
                    return false;
                }
                return true;
            });

            return plugin;
        }

        //跳转到某一页
        function gotoPage(postData) {
            var opts = getOpts();

            var model = getPostData() || {};

            if (!postData) {
                model[opts.pmap["pageSize"]] = opts.pageSize;
                model[opts.pmap["pageNo"]] = opts.pageNo;

                if (opts.sortBy) {
                    model[opts.pmap["sortBy"]] = opts.sortBy;
                }
                if (opts.sortOrder) {
                    model[opts.pmap["sortOrder"]] = opts.sortOrder;
                }

                if (opts.postData) {
                    for (var name in opts.postData) {
                        model[name] = opts.postData[name];
                    }
                }
            }
            if (postData) {
                for (var name in postData) {
                    model[name] = postData[name];
                }
            }

            setPostData(model);

            $.ajax({
                url: opts.url,
                type: opts.type,
                dataType: opts.dataType,
                data: model,
                beforeSend: function() {
                    if (opts.loadBefore)
                        opts.loadBefore();
                    $this.addClass("pager_loading");
                },
                success: function(result) {
                    //读取数据成功事件
                    if (opts.loadComplete) {
                        opts.loadComplete(result);
                        setPostResult(result);
                    }

                    //生成分页Html并注册相应事件
                    opts.pageNo = result.Page;
                    var html = createPagerHtml(getTempalte(opts.pagerTempalte), model[opts.pmap["pageNo"]], model[opts.pmap["pageSize"]], result[opts.jsonReader["total"]]);
                    $this.html(html);
                    if (opts.pagerTempalte == "t1") {
                        if (model[opts.pmap["pageNo"]] <= (result[opts.jsonReader["totalPages"]] - 5))
                            $("#pageDiv .lastPage").before("<a class='next_omit'>...</a>");
                        else
                            $("#pageDiv .lastPage").hide();

                        if ((model[opts.pmap["pageNo"]] - 6) >= 1) {
                            $("#pageDiv .firstPage").show();
                            $("#pageDiv .firstPage").after("<a class='prev_omit'>...</a>");
                        } else {
                            $("#pageDiv .firstPage").hide();
                            $("#pageDiv .prev_omit").remove();
                        }
                    }
                    $this.find(".pages a").click(function() {
                        var pageno = $(this).attr("pageno");
                        if (pageno) {
                            model[opts.pmap["pageNo"]] = parseInt(pageno);
                            gotoPage(model);
                        }
                    });

                    //滚动分页
                    if (opts.scrollPager) {
                        $this.hide();

                        if (opts.scrollTrigger == "document") {
                            $(window).unbind("scroll").scroll(function() {
                                if ($(window).scrollTop() == $(document).height() - $(window).height()) {
                                    if (result[opts.jsonReader["totalPages"]] > result.Page) {
                                        $this.find(".pages a.active").next("a").trigger("click");
                                    }
                                }
                            });
                        } else {
                            $(opts.scrollTrigger).unbind("scroll").scroll(function() {
                                var $trigger = $(this),
                                    viewH = $trigger.height(), //可见高度  
                                    contentH = $trigger.get(0).scrollHeight, //内容高度  
                                    scrollTop = $trigger.scrollTop(); //滚动高度  
                                if (contentH - viewH - scrollTop <= 50 && !$this.hasClass("pager_loading")) { //到达底部50px时,加载新内容 
                                    if (result[opts.jsonReader["totalPages"]] > result.Page) {
                                        $this.find(".pages a.active").next("a").trigger("click");
                                    }
                                }
                            });
                        }
                    }

                    $this.removeClass("pager_loading");
                }
            });

            //生成分页html
            function createPagerHtml(tempalte, pageNo, pageSize, total) {
                var totalPage = (total % pageSize) === 0 ? total / pageSize : parseInt(total / pageSize) + 1;
                pageNo = totalPage == 0 ? 0 : parseInt(pageNo);
                var html = "<div>" + tempalte + "</div>";
                html = html.format({
                    total: total,
                    pageNo: pageNo,
                    totalPage: totalPage,
                    firstPage: 1,
                    prePage: (total > 0 && pageNo > 1) ? pageNo - 1 : 1,
                    nextPage: (total > 0 && pageNo != totalPage) ? pageNo + 1 : totalPage,
                    lastPage: totalPage
                });

                var $html = $(html);
                //添加中间的页码数
                var mit = opts.pagerTempalte == "t1" ? 8 : 9;
                var numbersHtml = "", start = 1, end = 1;
                if (pageNo > 5)
                    start = pageNo - 5;
                end = start + mit > totalPage ? totalPage : start + mit;
                if (opts.pagerTempalte == "t1") {
                    for (var i = start; i <= end; i++) {
                        if (i == pageNo) {
                            numbersHtml += "<a class=\"numbers current\" pageNo='" + i + "'>" + i + "</a>";
                        } else
                            numbersHtml += "<a class=\"numbers\" pageNo='" + i + "' >" + i + "</a>";
                    }
                }
                if (opts.pagerTempalte == "t2") {
                    for (var i = start; i <= end; i++) {
                        if (i == pageNo) {
                            numbersHtml += "<a class=\"numbers current\" pageNo='" + i + "'>" + i + "</a>";
                        } else
                            numbersHtml += "<a class=\"numbers\" pageNo='" + i + "' >" + i + "</a>";
                    }
                }
                $html.find(".numbers").after(numbersHtml);
                return $html.html();
            };
        }

        //设置插件参数
        function setOpts(opts) {
            var plugin = getPlugin($this);
            plugin.opts = opts;
        }

        //获取插件参数
        function getOpts() {
            var plugin = getPlugin($this);
            return plugin.opts;
        }

        //设置postData
        function setPostData(data) {
            var plugin = getPlugin($this);
            plugin.postData = data;
        }

        //获取postData
        function getPostData() {
            var plugin = getPlugin($this);
            return plugin.postData;
        }

        //设置ajax请求结果
        function setPostResult(postResult) {
            var plugin = getPlugin($this);
            plugin.postResult = postResult;
        }

        //获取ajax请求结果
        function getPostResult() {
            var plugin = getPlugin($this);
            return plugin.postResult;
        }

        //获取分页模板html
        function getTempalte(pagerTempalte) {
            if (pagerTempalte == "t1") {
                return "<div class='pages'>" +
                    "<a class=\"Pre_page\" pageno=\"{prePage}\" jqPager-options=\"showfor:false\">上一页</a>" +
                    "<div class=\"page_num_radius\">" +
                    "<a class=\"firstPage\" pageno=\"{firstPage}\" style=\"display:none\" jqPager-options=\"showfor:false\">{firstPage}</a>" +
                    "<a class=\"numbers\" pageno=\"{page}\" style=\"display:none\"  jqPager-options=\"count:10\">{page}</a>" +
                    "<a class=\"lastPage\" pageno=\"{lastPage}\" style=\"display:none\" jqPager-options=\"showfor:false\">{lastPage}</a>" +
                    "</div>" +
                    "<a class=\"Next_page\" pageno=\"{nextPage}\" jqPager-options=\"showfor:false\">下一页</a>" +
                    "</div>";

            } else if (pagerTempalte == "t2") {
                return "<div class=\"pagination pages\">" +
                    "<div class=\"shul fl\"><span>共{totalPage}页</span>&nbsp;<span>第{pageNo}/{totalPage}页</span></div>" +
                    "<div class=\"page_num fr\">" +
                    "<span class=\"dian3\" style=\"display:none\">首页</span>" +
                    "<a pageno=\"{prePage}\" class=\"mr10\">上一页 </a>" +
                    "<span class=\"numbers\" pageno=\"{page}\" style=\"display:none\" jqPager-options=\"count:10\">1</span>" +
                    "<a pageno=\"{nextPage}\" class=\"ml10\">下一页</a>" +
                    "<span class=\"dian3\" style=\"display:none\">尾页</span>" +
                    "</div>" +
                "</div>";
            }

            return "";
        }
    }

    /*********************● 智图帮助类******************************************/
    window.zthelper = {
        //填充详情模型
        getShowDetail: function(detail) {
            var showDetail = {};
            showDetail = $.extend(true, showDetail, detail);

            showDetail.ShowCover = "";
            if (true) {
                showDetail.ShowCover = detail.ShowCover || "/Areas/Front/templates/Shared/t1/images/book.jpg";
            }

            showDetail.ShowAuthor = "";
            if (detail.Author) {
                var links = "";
                $(showDetail.Author.split(';')).each(function(i, det) {
                    if (!det) return;
                    links += "<a href=\"{1}\" title=\"{0};\">{0}</a>".format(det, getUrl("Article", "Pager", { Author: escape(det) }));
                });
                showDetail.ShowAuthor = links;
            }

            showDetail.ShowInstitution = "";
            if (detail.Institution) {
                var links = "";
                $(showDetail.Institution.split(';')).each(function(i, det) {
                    if (!det) return;
                    links += "<a href=\"{1}\" title=\"{0};\">{0}</a>".format(det, getUrl("Article", "Pager", { Organ: escape(det) }));
                });
                showDetail.ShowInstitution = links;
            }

            showDetail.ShowSource = "";
            if (detail.Source) {
                var links = "";
                $(showDetail.Source.split(';')).each(function(i, det) {
                    if (!det) return;
                    links += "<a href=\"{1}\" title=\"{0};\">《{0}》</a>".format(det, getUrl("Article", "Pager", { Source: escape(det) }));
                });
                showDetail.ShowSource = links;
            }

            showDetail.ShowPublisher = "";
            if (detail.Publisher) {
                showDetail.ShowPublisher = "《{Publisher}》".format(detail);
            }

            showDetail.ShowSubject = "";
            if (detail.Subject) {
                var links = "";
                $(showDetail.Subject.split(';')).each(function(i, det) {
                    if (!det) return;
                    links += "<a href=\"{1}\" title=\"{0};\">{0}</a>".format(det, getUrl("Article", "Pager", { Subject: escape(det) }));
                });
                showDetail.ShowSubject = links;
            }

            detail.ShowEnglishSource = "";
            if (detail.EnglishSource) {
                showDetail.ShowEnglishSource = "《{EnglishSource}》".format(detail);
            }

            return showDetail;
        },

        //根据不同文献类型获取该文献类型的字段
        getFields: function(type) {
            var model = {
                isShow: null, //是否显示
                fields: [] //字段集合
            };

            if (type == 0) { //全部文献
                model = {
                    isShow: true,
                    fields: [
                        { value: "KeyWord", text: "全部字段" },
                        { value: "Title", text: "标题" },
                        { value: "Author", text: "作者" },
                        { value: "Subject", text: "主题词" },
                        { value: "Organ", text: "机构" },
                        { value: "SubjectESC", text: "学科分类号" },
                        { value: "Description", text: "摘要" },
                        { value: "IdentifierIssn", text: "ISSN" },
                        { value: "DescriptionFund", text: "基金资助" },
                    ]
                };
            } else if (type == 1) { //图书
                model = {
                    isShow: true,
                    fields: [
                        { value: "KeyWord", text: "全部字段" },
                        { value: "Title", text: "标题" },
                        { value: "Author", text: "作者" },
                        { value: "Subject", text: "主题词" },
                        { value: "Organ", text: "机构" },
                        { value: "SubjectESC", text: "学科分类号" },
                        { value: "Description", text: "摘要" },
                        { value: "IdentifierIssn", text: "ISSN" },
                        { value: "DescriptionFund", text: "基金资助" },
                    ]
                };
            } else if (type == 3) { //期刊文章
                model = {
                    isShow: true,
                    fields: [
                        { value: "KeyWord", text: "全部字段" },
                        { value: "Title", text: "标题" },
                        { value: "Author", text: "作者" },
                        { value: "Subject", text: "主题词" },
                        { value: "Organ", text: "机构" },
                        { value: "SubjectESC", text: "学科分类号" },
                        { value: "Description", text: "摘要" },
                        { value: "IdentifierIssn", text: "ISSN" },
                        { value: "DescriptionFund", text: "基金资助" },
                    ]
                };
            } else if (type == 4) { //学位论文
                model = {
                    isShow: true,
                    fields: [
                        { value: "KeyWord", text: "全部字段" },
                        { value: "Title", text: "标题" },
                        { value: "Author", text: "作者" },
                        { value: "Subject", text: "主题词" },
                        { value: "Organ", text: "机构" },
                        { value: "SubjectESC", text: "学科分类号" },
                        { value: "Description", text: "摘要" },
                        { value: "IdentifierIssn", text: "ISSN" },
                        { value: "DescriptionFund", text: "基金资助" },
                    ]
                };
            } else if (type == 5) { //标准
                model = {
                    isShow: true,
                    fields: [
                        { value: "KeyWord", text: "全部字段" },
                        { value: "Title", text: "标题" },
                        { value: "Author", text: "作者" },
                        { value: "Subject", text: "主题词" }
                    ]
                };
            } else if (type == 6) { //会议论文
                model = {
                    isShow: true,
                    fields: [
                        { value: "KeyWord", text: "全部字段" },
                        { value: "Title", text: "标题" },
                        { value: "Author", text: "作者" },
                        { value: "Subject", text: "主题词" },
                        { value: "Organ", text: "机构" },
                        { value: "SubjectESC", text: "学科分类号" },
                        { value: "Description", text: "摘要" },
                        { value: "IdentifierIssn", text: "ISSN" },
                        { value: "DescriptionFund", text: "基金资助" },
                    ]
                };
            } else if (type == 7) { //专利
                model = {
                    isShow: true,
                    fields: [
                        { value: "KeyWord", text: "全部字段" },
                        { value: "Title", text: "标题" },
                        { value: "Author", text: "作者" },
                        { value: "Subject", text: "主题词" }
                    ]
                };
            } else if (type == 8) { //政策法规
                model = {
                    isShow: true,
                    fields: [
                        { value: "KeyWord", text: "全部字段" },
                        { value: "Title", text: "标题" },
                        { value: "Author", text: "作者" },
                        { value: "Subject", text: "主题词" }
                    ]
                };
            } else if (type == 9) { //科技成果
                model = {
                    isShow: true,
                    fields: [
                        { value: "KeyWord", text: "全部字段" },
                        { value: "Title", text: "标题" },
                        { value: "Author", text: "作者" },
                        { value: "Subject", text: "主题词" },
                        { value: "Organ", text: "机构" }
                    ]
                };
            } else if (type == 12) { //科技报告
                model = {
                    isShow: true,
                    fields: [
                        { value: "KeyWord", text: "全部字段" },
                        { value: "Title", text: "标题" },
                        { value: "Author", text: "作者" },
                        { value: "Subject", text: "主题词" },
                        { value: "Organ", text: "机构" }
                    ]
                };
            } else if (type == 13) { //产品样本
                model = {
                    isShow: true,
                    fields: [
                        { value: "KeyWord", text: "全部字段" },
                        { value: "Title", text: "标题" },
                        { value: "Author", text: "作者" },
                        { value: "Subject", text: "主题词" },
                        { value: "Organ", text: "机构" }
                    ]
                };
            } else if (type == 99) { //Find+
                model = {
                    isShow: true,
                    fields: [
                        { value: "KeyWord", text: "全部字段" },
                        { value: "Title", text: "标题" },
                        { value: "Author", text: "作者" },
                        { value: "Subject", text: "主题词" }
                    ]
                };
            }

            return model;
        }
    };

    /*********************● 全局变量：获取当前请求信息********************************************/
    window.appInfo = null;
})();

/********************************************● 消息对话框框**********************************************************/
function $dialogShow(param) {
    //基于layer的错误消息窗口
    if (param.Type.toLowerCase() == "error") {
        layer.open({
            content: "<div style='margin-top: 25px; margin-left: 18px; width:223px;'> " + param.Content + "</div>",
            title: "",
            icon: 5,
            type: param.type || 1,
            btn: ['<i class="fa fa-check"></i>&nbsp;确定']
        });
    }
    //基于layer的成功消息通知栏
    else if (param.Type.toLowerCase() == "success") {
        layer.msg(param.Content, {
            title: param.Title,
            time: 800
        });
    }
    //基于bootstrap-modal的登陆窗口
    else if (param.Type.toLowerCase() == "login") {
        var loginTemplate = getDialogHtml("#dialog_login");
        $("body .modal-dialog-box-dialogLogin").remove();
        $("body").append("<div class='modal-dialog-box-dialogLogin'></div>");
        $(".modal-dialog-box-dialogLogin").html(loginTemplate.format({ Title: param.Title, Name: $("#userAccount").text() }));
        $(".modal-login").modal('show');
    }
    //基于bootstrap-modal的锁屏窗口
    else if (param.Type.toLowerCase() == "lock") {
        var loginTemplate = getDialogHtml("#dialog_lock");
        $("body .modal-dialog-box-dialogLock").remove();
        $("body").append("<div class='modal-dialog-box-dialogLock'></div>");
        $(".modal-dialog-box-dialogLock").html(loginTemplate.format({ Title: param.Title, Name: $("#userAccount").text() }));
        $(".modal-lock").modal('show');
    }
    //基于bootstrap-modal的confirm弹出窗口
    else if (param.Type.toLowerCase() == "confirm") {
        var loginTemplate = getDialogHtml("#dialog_confirm");
        $("body .modal-dialog-box-dialogConfirm").remove();
        $("body").append("<div class='modal-dialog-box-dialogConfirm'></div>");
        $(".modal-dialog-box-dialogConfirm").html(loginTemplate.format({ Title: param.Title, Content: param.Content }));
        $(".modal-confirm").modal('show');
        //为“确定”按钮绑定事件
        $(".modal-dialog-box-dialogConfirm .btn-confirm-sure").click(function () {
            debugger;
            if (param.SureFn) {
                param.SureFn($(".modal-confirm"));
            } else {
                $(".btn-confirm-close").trigger("click");
            }
        });
    }
    //基于layer插件的loading加载
    else if (param.Type.toLowerCase() == "loading") {
        if (param.IsShow)
            layer.msg(param.Content, { time: 0, icon: 16 });
        else
            layer.closeAll();
    }
    //基于layer插件的弹出框
    else if (param.Type.toLowerCase() == "alert") {
        layer.alert(param.Content, { title: param.Title || "确认" });
    }

    //弹出框模板
    function getDialogHtml(name) {
        var template = "";
        $.ajax({
            type: "get",
            cache: true,
            async: false,
            url: "/Areas/Base/Scripts/customer/html/dialog.html",
            dataType: "html",
            error: function (html) {
                alert('未能加载html文档' + html);
            },
            success: function (html) {
                debugger;
                html = "<div>{0}</div>".format(html);
                template = $(html).find(name).prop("outerHTML");
            }
        });
        return template;
    }
}

/********************************************● 链接通用方法******************************************************/
function getUrl(p1, p2, p3) {
    var locationPath = urlhelper.getPath();

    if ($.trim(p1).indexOf("javascript") != -1) return p1;

    if (urlhelper.getType() == "mvc") { //mvc链接  
        if (p1 && p1.indexOf("/") == -1) {
            return getMvcRouteUrl(p1, p2, p3);
        }
        else {
            return getMvcUrl(p1, p2);
        }
    }
    else if (urlhelper.getType() == "aspx") { //普通链接
        return getAspxUrl(p1, p2, p3);
    }

    return p1;

    //Mvc链接（路径）
    function getMvcUrl(p1, p2) {
        var siteDomain = locationPath.split("/")[1];
        var area = (appInfo && appInfo.area == "Front") ? "" : locationPath.split("/")[2];
        var url = "";

        //设置url路径
        if (p2) {
            if (p2.siteDomain != null) {
                siteDomain = p2.siteDomain;
            }
            if (p2.area != null) {
                area = p2.area;
            }
        }

        url = "{0}{1}{2}".format(siteDomain && "/" + siteDomain, area && "/" + area, p1);

        //设置url参数
        if (p2) {
            var obj = p2;
            delete obj.siteDomain;
            delete obj.area;

            url = urlhelper.setParams(obj, url);
        }

        return url;
    }

    //Mvc链接（路由）
    function getMvcRouteUrl(p1, p2, p3) {
        var siteDomain = locationPath.split("/")[1];
        var area = (appInfo && appInfo.area == "Front") ? "" : locationPath.split("/")[2];
        var controller = p1;
        var action = p2;
        var url = "";

        //设置url路径
        if (p3) {
            if (p3.siteDomain != null) {
                siteDomain = p3.siteDomain;
            }
            if (p3.area != null) {
                area = p3.area;
            }
        }
        url = "{0}{1}{2}{3}".format(siteDomain && "/" + siteDomain, area && "/" + area, controller && "/" + controller, action && "/" + action);

        //设置url参数
        if (p3) {
            var obj = p3;
            delete obj.siteDomain;
            delete obj.area;

            url = urlhelper.setParams(obj, url);
        }

        return url;
    }

    //普通链接
    function getAspxUrl(p1, p2) {
        var url = p1 || $getPath();//待跳转的url
        var cid = $getQueryString(gloConfig.cid);//当前cid参数
        var params = p2 || {};//url相关参数

        if (url.indexOf("?") < 0) {
            url += "?";
        }

        for (var p in params) {
            url += "&{0}={1}".format(p, params[p]);
        }

        var urlParams = $getQueryStrings(url);
        if (cid && urlParams[gloConfig.cid.toLowerCase()] == null) {
            url += "&{0}={1}".format(gloConfig.cid, cid);
        }

        return url;
    }
}

/********************************************● 跳转通用方法******************************************************/
function goUrl(link, p1, p2, p3) {
    if (!!link.nodeName)
        $(link).attr("href", getUrl(p1, p2, p3));
    else {
        debugger;
        window.location.href = getUrl(p1, p2, p3);
    }
}


