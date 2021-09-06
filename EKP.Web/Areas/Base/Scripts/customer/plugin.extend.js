/* 
    作    者：胡政
    描    述：插件初始参数配置和扩展
    创建时间：2015-08-28
    联系方式：13436053642
*/
/* 通用js插件配置
*/

/********************************************● 初始化加载************************************************************/
(function () {
    /*********************● jqgrid默认参数配置********************************************/
    if (jQuery.jgrid) {
        jQuery.extend(jQuery.jgrid.defaults, {
            //重写jqgrid默认参数
            rowList: [15, 30, 50],
            viewsortcols: [true, 'vertical', true],
            viewrecords: true,
            pginput: true,
            rowNum: 15,
            altRows: false,
            sortname: "Id",
            sortorder: "asc",
            datatype: 'json',
            height: 400,
            mtype: 'POST',
            autowidth: true,
            shrinkToFit: true,
            autoScroll: false,
            multiselect: true,
            multiboxonly: true,
            cellEdit: false,
            cellsubmit: 'clientArray',
            editurl: 'clientArray',
            updateToolbar: function (grid) {
                var selarrrow = $(grid).jqGrid('getGridParam', 'selarrrow');
                var id = $(grid).jqGrid('getGridParam', 'selrow');
                var toolbar = $(grid).jqGrid('getGridParam', 'toolbar');
                if (toolbar) {
                    $(toolbar).find(".toolbar-data").attr("disabled", selarrrow.length > 1 || id == null);
                    $(toolbar).find(".toolbar-datas").attr("disabled", selarrrow.length < 1 && id == null);
                }
            },
            loadCompleteExt: function (grid) {
                $(window).resize(function () {
                    //重设表格宽度
                    var width = $(grid).closest(".table-scrollable").width();
                    $(grid).setGridWidth(width);

                    //防止出现横向滚动条
                    var diff = $(grid).width() - width;
                    if (diff > 0) {
                        var tdLastWidth = parseInt($(grid).find(".jqgfirstrow td:last").css("width"));
                        $(grid).find(".jqgfirstrow td:last").css("width", (tdLastWidth - diff) + "px");
                    } 
                }).trigger("resize");
            },
            prmNames: {
                page: 'Page',
                rows: 'PageSize',
                sort: 'SortBy',
                order: 'SortOrder'
            },
            jsonReader: {
                page: 'Page',
                total: 'TotalPages',
                records: 'TotalRecords',
                root: 'Rows',
                repeatitems: false
            }
        });
    }

    /*********************● bootstrap-datepicker默认参数配置******************************/
    if (jQuery.fn.datepicker) {
        jQuery.extend(jQuery.fn.datepicker.defaults, {
            autoclose: true,
            beforeShowDay: $.noop,
            calendarWeeks: false,
            clearBtn: false,
            daysOfWeekDisabled: [],
            endDate: Infinity,
            forceParse: true,
            format: 'yyyy-mm-dd',
            keyboardNavigation: true,
            language: 'zh-CN',
            minViewMode: 0,
            orientation: "auto",
            rtl: false,
            startDate: -Infinity,
            startView: 0,
            todayBtn: false,
            todayHighlight: false,
            weekStart: 0
        });
        if ($.fn.datepicker.dates) {
            $.fn.datepicker.dates['zh-CN'].format = 'yyyy-mm-dd';
        }
    }

    /*********************● bootstrap-modal默认参数配置***********************************/
    $.fn.extend({
        destroyModal: function () {
            $(this)
                .off('.modal')
                .removeData('modal')
                .removeData('bs.modal')
                .removeClass('in')
                .attr('aria-hidden', true);
        }
    });

    /*********************● layer默认参数配置**********************************************/
    if (typeof (layer) != undefined && typeof (layer) != "undefined") {
        layer.config({
            extend: 'extend/layer.ext.js',
            zIndex: 8990
        });
    }

    /*********************● KindEditor默认参数配置*****************************************/
    if (typeof (KindEditor) != undefined && typeof (KindEditor) != "undefined") {
        window.KeHelper = {
            create: function (kindeditors) {
                $(kindeditors).each(function (i, ke) {
                    KindEditor.basePath = '/Areas/Base/Scripts/kindeditor-4.1.10/';
                    KindEditor.create(ke, {
                        minWidth: 200,
                        width: 700,
                        height: parseInt($(ke).css("height")) || 600,
                        themeType: 'simple',
                        cssPath: '/Areas/Base/Scripts/kindeditor-4.1.10/plugins/code/prettify.css',
                        uploadJson: '/Areas/Base/Scripts/kindeditor-4.1.10/asp.net/upload_json.ashx',
                        fileManagerJson: '/Areas/Base/Scripts/kindeditor-4.1.10/asp.net/file_manager_json.ashx',
                        allowFileManager: true
                    });
                });
            },
            createImage: function (btns) {
                var editor = KindEditor.editor({
                    uploadJson: '/Areas/Base/Scripts/kindeditor-4.1.10/asp.net/upload_json.ashx',
                    fileManagerJson: '/Areas/Base/Scripts/kindeditor-4.1.10/asp.net/file_manager_json.ashx',
                    allowFileManager: true
                });
                $(btns).each(function (i, btn) {
                    var keImage = $(btn).closest(".keImage");
                    var input = $(keImage).find("input");
                    var img = $(keImage).find("img");
                    KindEditor(btn).click(function () {
                        editor.loadPlugin('image', function () {
                            editor.plugin.imageDialog({
                                imageUrl: KindEditor(input).val(),
                                clickFn: function (url, title, width, height, border, align) {
                                    KindEditor(input).val(url);
                                    editor.hideDialog();
                                    $(img).attr("src", url);
                                    $(input).val(url);
                                }
                            });
                        });
                    });
                });
            },
            createFile: function (btns, options) {
                var defaults = {
                    dirName: "file"
                };
                var opts = $.extend(defaults, options);

                var editor = KindEditor.editor({
                    uploadJson: '/Areas/Base/Scripts/kindeditor-4.1.10/asp.net/upload_json.ashx',
                    fileManagerJson: '/Areas/Base/Scripts/kindeditor-4.1.10/asp.net/file_manager_json.ashx',
                    allowFileManager: true
                });
                $(btns).each(function (i, btn) {
                    var keFile = $(btn).closest(".keFile");
                    var input = $(keFile).find("input");
                    KindEditor(btns).click(function () {
                        editor.loadPlugin('insertfile', function () {
                            editor.plugin.fileDialog({
                                dirName: "ckplayerMedia",
                                clickFn: function (url, title, data) {
                                    $(input).val(url);
                                    editor.hideDialog();
                                }
                            });
                        });
                    });
                });
            }
        }
    }

    /*********************● Ckplayer默认参数配置*****************************************/
    if (typeof (CKobject) != undefined && typeof (CKobject) != "undefined") {
        window.CkplayerHelper = {
            init_det_ckplayer: function (options) {
                var defaults = {
                    boxId: null,
                    videoUrl: null,
                    previewImg: null,
                    width: 600,
                    height: 600
                };
                var opts = $.extend(defaults, options);

                var box = $("#" + opts.boxId);
                var flashvars = {
                    f: opts.videoUrl,
                    c: 0,
                    b: 1,
                    i: opts.previewImg,
                    e: 2,
                    p: box.attr("autostart") == "true" ? 1 : 0
                };
                var params = { bgcolor: '#FFF', allowFullScreen: true, allowScriptAccess: 'always', wmode: 'transparent' };
                CKobject.embedSWF('/Areas/Base/Scripts/ckplayer6.8/ckplayer/ckplayer.swf', opts.boxId, 'ckplayer_' + opts.boxId, opts.width.toString(), opts.height.toString(), flashvars, params);
                /*
                CKobject.embedSWF(播放器路径,容器id,播放器id/name,播放器宽,播放器高,flashvars的值,其它定义也可省略);
                下面三行是调用html5播放器用到的
                */
                var video = [opts.videoUrl];
                var support = ['iPad', 'iPhone', 'ios', 'android+false', 'msie10+false'];
                flashvars.e = 4;
                CKobject.embedHTML5(opts.boxId, 'ckplayer_' + opts.boxId, opts.width, opts.height, video, flashvars, support);
            }
        }
    }

})()

function playerstop() {
    CKobject.getObjectById('ckplayer_ckVideo').videoPlay();
    //CKobject.getObjectById('ckplayer_ckVideo').videoSeek("1");
    //var index = layer.open({
       //type: 1,
       //content: '视频已结束1.',
       //area: ['320px', '195px'],
       //maxmin: true
    //});
    setTimeout(function(){
       CKobject.getObjectById('ckplayer_ckVideo').playOrPause();
       //alert("视频已结束.");
    },500);
}