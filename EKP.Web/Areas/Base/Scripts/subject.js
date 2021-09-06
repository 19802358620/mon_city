/**********************************考试**************************************/
window.zttest = {
    //获取考试
    getTest: function (testId) {
        var test = null;
        $.ajax({
            async: false,
            type: "post",
            dataType: "json",
            data: { id: testId },
            url: getUrl("Test", "Detail", { area: "Adm" }),
            success: function (data) {
                test = data;
            }
        });

        return test;
    },

    //获取出题设置
    getTestSettings: function (testId) {
        var testSettings = [];
        $.ajax({
            async: false,
            type: "post",
            dataType: "json",
            data: { TestId: testId, Page: 1, PageSize: 999, SortBy: "SortIndex", SortOrder: "asc" },
            url: getUrl("TestSetting", "Pager", { area: "Adm" }),
            success: function (data) {
                testSettings = data.Rows;
            }
        });

        return testSettings;
    },

    //获取交卷信息
    getTestPaperHand: function (opts) {
        var param = {
            id: null, //交卷Id
            testId: null,//考试Id
            userId: null //交卷人Id
        };
        var opts = $.extend(param, opts);

        var testPaperHand = null;
        if (opts.id) {
            $.ajax({
                async: false,
                type: "post",
                dataType: "json",
                data: { id: opts.id },
                url: getUrl("TestPaperHand", "Detail", { area: "Adm" }),
                success: function (data) {
                    testPaperHand = data;
                }
            });
        } else {
            $.ajax({
                async: false,
                type: "post",
                dataType: "json",
                data: { testId: opts.testId, userId: opts.userId },
                url: getUrl("TestPaperHand", "GetUserDetail", { area: "Adm" }),
                success: function (data) {
                    testPaperHand = data;
                }
            });
        }

        return testPaperHand;
    },

    //初始化试卷结构
    initTest: function (opts) {
        var param = {
            testId: null,//试卷Id
            callBack: null //初始化完成后事件
        };
        var opts = $.extend(param, opts);

        $.ajax({
            type: "post",
            dataType: "json",
            data: { id: opts.testId },
            url: getUrl("Test", "InitTest", { area: "Adm" }),
            success: function (data) {
                if (opts.callBack) {
                    opts.callBack(data);
                }
            }
        });
    }
}

/**********************************题目**************************************/
window.ztsubject = {
    //初始化显示题目
    init: function (opts) {
        //初始化参数
        var param = {
            box: null,//显示题目的div层
            subject: null, //题目数据源
            sNum: 1, //题目序号
            sqno: 1, //问题序号
            afterInit: null, 
            afterQuestionInit: null
        };
        var opts = $.extend(param, opts);
        //加载初始html
        $(opts.box).html(getSubjectInitHtml());

        //初始化题目
        for (var i in opts.subject) {
            if (i == "Name") {
                var sHtml = opts.subject.Name;
                $(opts.box).find(".subject-Num").append(param.sNum || 1);
                $(opts.box).find(".subject-" + i).append(sHtml);
                $(opts.box).find(".subject-" + i).show();
            }
            else if (opts.subject[i]) {
                $(opts.box).find(".subject-" + i).append(opts.subject[i]);
                $(opts.box).find(".subject-" + i).show();
            }
        }

        //循环显示小题
        $(opts.subject.Questions).each(function (j, qdata) {
            qdata.OptionsColumns = qdata.OptionsColumns ? qdata.OptionsColumns : 1;//如果选项没有设置列数则显示4列

            //根据不同的问题类型解析问题
            if (qdata.Type == "single") { //单选
                var options = JSON.parse(qdata.Options);
                var questionBox = $("<div class='question-Option-box question-box' type='{Type}' qid='{Id}'></div>".format(qdata));
                $(options).each(function (k, op) {
                    var choice = ztsubject.getChoice(k);
                    questionBox.append(("<p class='lb_option' style='width:{3}%'><i><input type=\"radio\" value=\"{0}\" name=\"_{2}\" class=\"answer_q\"/></i>{0}.{1}</p>")
                        .format(choice, op, qdata.Id, 100 / qdata.OptionsColumns));
                });
                $(opts.box).find(".question-Option")
                    .append(questionBox);
            }
            else if (qdata.Type == "multi") { //多选
                var options = JSON.parse(qdata.Options);
                var questionBox = $("<div class='question-Option-box question-box' type='{Type}' qid='{Id}'></div>".format(qdata));
                $(options).each(function (k, op) {
                    var choice = ztsubject.getChoice(k);
                    questionBox.append(("<p class='lb_option' style='width:{3}%'><i><input type=\"checkbox\" value=\"{0}\" name=\"_{2}\" class=\"answer_q\"/></i>{0}.{1}</p>")
                        .format(choice, op, qdata.Id, 100 / qdata.OptionsColumns));
                });
                $(opts.box).find(".question-Option")
                    .append(questionBox);
            }
            else if (qdata.Type == "bit") { //判断
                var options = [{ value: "1", name: "正确" }, { value: "0", name: "错误" }];
                var questionBox = $("<div class='question-Option-box question-box' type='{Type}' qid='{Id}'></div>".format(qdata));
                $(options).each(function (k, op) {
                    var choice = op.value;
                    questionBox.append(("<p class='lb_option' style='width:{3}%'><i><input type=\"radio\" value=\"{0}\" name=\"_{2}\" class=\"answer_q\"/></i>{1}</p>")
                        .format(choice, op.name, qdata.Id, 100 / qdata.OptionsColumns));
                });
                $(opts.box).find(".question-Option")
                    .append(questionBox);
            }
            else if (qdata.Type == "fill") { //填空
                var questionBox = $("<div class='question-Option-box question-box hide' type='{Type}' qid='{Id}'></div>".format(qdata));
                $(opts.box).find(".question-Option").append(questionBox);
            }
            else if (qdata.Type == "shortAnswer") {
                var questionBox = $("<div class='question-Option-box question-box' type='{Type}' qid='{Id}'></div>".format(qdata));
                questionBox.append('<textarea  class="kindeditor answer_q" name=\"_{0}\" id=\"_{0}\"></textarea>'.format(qdata.Id));
                $(opts.box).find(".question-Option").append(questionBox);
                
                KindEditor.basePath = '/Areas/Base/Scripts/kindeditor-4.1.10/';
                KindEditor.create($(questionBox).find('.kindeditor'), {
                    minWidth: 200,
                    width: ($(opts.box).width() - 3).toString(),
                    height: '150',
                    themeType: 'simple',
                    cssPath: '/Areas/Base/Scripts/kindeditor-4.1.10/plugins/code/prettify.css',
                    uploadJson: '/Areas/Base/Scripts/kindeditor-4.1.10/asp.net/upload_json.ashx',
                    fileManagerJson: '/Areas/Base/Scripts/kindeditor-4.1.10/asp.net/file_manager_json.ashx',
                    allowFileManager: true,
                    items: [
                        'source', 'plainpaste', 'wordpaste', 'justifyleft', 'justifycenter', 'justifyright',
                        'justifyfull', 'insertorderedlist', 'insertunorderedlist', 'indent', 'outdent', 'formatblock',
                        'fontname', 'fontsize', 'bold', 'italic', 'underline', 'image', 'media', 'insertfile', 'jme'
                    ]
                });
            }

            //呈现一道问题
            if ($(opts.box).find(".question-Option").html().ignoreHtmlNbsp())
                $(opts.box).find(".question-Option").show();

            //初始化问题完成事件
            if (opts.afterQuestionInit) {
                opts.afterQuestionInit(opts, qdata, questionBox);
            }
        });
        
        //初始化完成事件
        if (opts.afterInit) {
            opts.afterInit(opts);
        }

        //获取加载题目的初始html
        function getSubjectInitHtml() {
            var html =
                '<h3><i>第&nbsp;<span class="subject-Num"></span>&nbsp;题</i></h3>' +
                '<div class="subject-con">' +
                    '<p class="subject-Name font16"></p>' +
                    '<p class="question-Option"></p>' +
                '</div>' +
                '<p class="subject-tool button" style="display:none">&nbsp;<span></span></p>' + 
                '<div class="sunject-answer key">' + 
                    '<h4 class="isRight"></span>' +
                    '<h4 class="answer"><span class="pre">参考答案：</span><span class="con"></span></h4>' +
                    '<h4 class="analysis"><span class="pre">本题解析：</span><span class="con"></span></h4>' +
                '</div > ';
            return html;
        }
    },

    //核对答案
    checkAnswer: function (opts) {
        //初始化参数
        var param = {
            subject_item: null,//题目dom
            subject: null, //题目
            isShowCheck: true,  //是否显示判卷结果
            isShowAnswer: true, //是否显示答案
            isShowAnalysis: true, //是否显示解析
            checkCallback: null
        };
        var opts = $.extend(param, opts);

        //显示参考答案
        var answer = ztsubject.getAnswer(opts.subject_item, opts.subject.Type);
        $(opts.subject_item).find(".sunject-answer").show();
        $(opts.subject_item).find(".answer .con").html(opts.subject.Questions[0].ShowAnswer);
        $(opts.subject_item).find(".analysis .con").html(opts.subject.Analysis);

        //显示判卷结果
        if (opts.isShowCheck) {
            var checkAnswers = ztsubject.getCheckAnswers([{ Answer: answer, SubjectId: opts.subject.Id }]);
            var isRight = checkAnswers[0].IsRight;
            if (opts.subject.Type != "shortAnswer") {//简答题没有正确错误之分
                $(opts.subject_item).find(".isRight").html(getIsRightTxt(isRight));
            }
            if (opts.checkCallback) {
                opts.checkCallback(isRight);
            }
        }

        //是否显示答案
        if (!opts.isShowAnswer) {
            $(opts.subject_item).find(".answer").hide();
        }

        //是否显示解析
        if (!opts.isShowAnalysis) {
            $(opts.subject_item).find(".analysis").hide();
        }


        //判卷文本结果
        function getIsRightTxt(b) {
            if (b == true) return '<span class="label label-success font-size-16">回答正确</span>&nbsp;&nbsp;&nbsp;';
            else if (b == false) return '<span class="label  label-danger font-size-16">回答错误</span>&nbsp;&nbsp;&nbsp;';

            return "";
        }
    },

    //获取题目
    getSubject: function (id) {
        var subject = null;
        $.ajax({
            async: false,
            type: "post",
            dataType: "json",
            data: { id: id },
            url: getUrl("Subject", "Detail", { area: "Adm" }),
            success: function(data) {
                subject = data;
            }
        });

        return subject;
    },

    //获取问题列表
    getQuestions: function (chapterId, subjectId) {
        var questions = [];
        $.ajax({
            async: false,
            type: "post",
            dataType: "json",
            data: { ChapterId: chapterId, SubjectId: subjectId, Page: 1, PageSize: 999 },
            url: getUrl("Question", "Pager", { area: "Adm" }),
            success: function(data) {
                questions = data.Rows;
            }
        });

        return questions;
    },

    //获取题目类型列表
    getSubjectTypes: function () {
        var subjectTypes = new Array();
        $.ajax({
            async: false,
            type: "post",
            dataType: "json",
            data: { },
            url: getUrl("Subject", "SubjectTypes", { area: "Adm" }),
            success: function (data) {
                subjectTypes = data;
            }
        });

        return subjectTypes;
    },

    //获取回答内容
    getAnswer: function (subject_item, subjectType) {
        if (subjectType == "single") {
            return $(subject_item).find(".answer_q:checked").val();
        }
        else if (subjectType == "multi") {
            var answer = "";
            $(subject_item).find(".answer_q:checked").each(function (i, item) {
                answer += $(item).val();
            });
            return answer;
        }
        else if (subjectType == "bit") {
            return $(subject_item).find(".answer_q:checked").val();
        }

        return "";
    },

    //获取答题检测结果
    getCheckAnswers: function (checkAnswerParams) {
        var checkAnswers = null;
        $.ajax({
            async: false,
            type: "post",
            dataType: "json",
            data: { param: checkAnswerParams },
            url: getUrl("Subject", "CheckAnswers", { area: "Adm" }),
            success: function(data) {
                checkAnswers = data;
            }
        });

        return checkAnswers;
    },

    //获取所有选项
    getChoices: function () {
        var arr = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "X", "Y", "Z"];

        return arr;
    },

    //获取选项，根据索引获取
    getChoice: function(i) {
        var choices = ztsubject.getChoices();
        return choices[i];
    }
};

/**********************************试卷**************************************/
window.zttp = {
    //初始化显示试卷
    init: function (opts) {
        //初始化参数
        var param = {
            box: null, // 显示试卷的div层
            testPaper: null, //试卷源数据
            testPaperReplys: null, //试卷答题信息
            afterInit: null, //加载完成后事件
            afterStructInit: null, //试卷结构加载完事件
            afterSubjectInit: null, //题目加载完成后事件
            afterQuestionInit: null //问题加载完成后事件
        };
        var opts = $.extend(param, opts);

        //加载试卷结构
        var testPaperStructs = opts.testPaper.TestPaperStructs;
        var sNum = 1;
        $(testPaperStructs).each(function (i, tps) {
            tps.Order = numHelper.numberToChinese(i + 1);
            var tps_box = $("<h2 class=\"tps-box\" tpsTructId=\"{Id}\"><i class=\"title\">{Order}、{ShowSubjectType}</i></h2> ".format(tps));
            var sb_box = $("<ul class=\"sb-box subjectBox\" tpsTructId=\"{Id}\" class=\"clr\"></ul>".format(tps));
            $(opts.box).append(tps_box);
            $(opts.box).append(sb_box);

            var subjects = tps.Subjects;
            $(subjects).each(function (j, subject) {
                var subject_item = $("<li class=\"subject_item\" tpSubjectId=\"{TestPaperSubjectId}\" Type=\"{Type}\"></li>".format(subject));
                $(sb_box).append(subject_item); 
                ztsubject.init({
                    box: subject_item,
                    subject: subject, 
                    sNum: sNum,
                    afterInit: function (sOpts) {
                        if (opts.afterSubjectInit) {
                            opts.afterSubjectInit(sOpts);
                        }
                    },
                    afterQuestionInit: function (sOpts, qdata, questionBox) {
                        if (opts.afterQuestionInit) {
                            if (opts.testPaperReplys) {
                                var testPaperReply = null;
                                $(opts.testPaperReplys).each(function (i, tpr) {
                                    if (tpr.QuestionId == qdata.Id) {
                                        testPaperReply = tpr;
                                    }
                                });
                            }
                            opts.afterQuestionInit(sOpts, qdata, questionBox, testPaperReply);
                        }
                    }
                });
                sNum = sNum + 1;
            });

            if (opts.afterStructInit) {
                opts.afterStructInit(tps, tps_box);
            }
        });
        //初始化完成事件
        if (opts.afterInit) {
            opts.afterInit(opts);
        }
    },

    //初始化显示答题卡
    initShowAnswerCard: function (opts) {
        //初始化参数
        var param = {
            box: null, // 显示答题卡的div层
            testPaperBox: null, //显示试卷的div层
            testPaper: null, //试卷源数据
            top: 100, //固定悬浮在指定top位置
            right: 100 //固定悬浮在指定right位置
        };
        var opts = $.extend(param, opts);
        
        var testPaperStructs = opts.testPaper.TestPaperStructs;
        var sNum = 1;
        $(testPaperStructs).each(function (i, tps) {
            tps.Order = numHelper.numberToChinese(i + 1);
            var tps_box = $("<h4>{Order}、{ShowSubjectType}</h4>".format(tps));
            var sb_box = $("<p tpsTructId=\"{Id}\"></p>".format(tps));
            $(opts.box).find(".answer_items").append(tps_box);
            $(opts.box).find(".answer_items").append(sb_box);

            var subjects = tps.Subjects;
            $(subjects).each(function (j, subject) {
                var subject_item = $("<a href=\"javascript:;\" class=\"tpSubject\" tpsubjectid={0}>{1}</a>".format(subject.TestPaperSubjectId, sNum));
                $(sb_box).append(subject_item);
                sNum = sNum + 1;
            });
        });

        //刷新答题卡状态
        if (opts.testPaperBox) {
            window.setInterval(function () { refresh() }, 500);
            function refresh() {
                $(opts.box).find(".tpSubject").each(function (i, tpSubject) {
                    var tpSubjectId = $(tpSubject).attr("tpsubjectid");
                    var subject_item = $(".subject_item[tpsubjectid='{0}']".format(tpSubjectId));
                    var type = $(subject_item).attr("type");

                    if (type == "single" || type == "multi" || type == "bit") {
                        var sel = $(subject_item).find(".answer_q:checked");
                        if (sel.length) {
                            $(tpSubject).addClass("current");
                        } else {
                            $(tpSubject).removeClass("current");
                        }
                    }
                    else if (type == "fill") {
                        var aq = $(subject_item).find(".subject-input.answer_q");
                        var answer = $(aq).val();
                        if (answer) {
                            $(tpSubject).addClass("current");
                        } else {
                            $(tpSubject).removeClass("current");
                        }
                    }
                    else if (type == "shortAnswer"){
                        var aq = $(subject_item).find(".kindeditor.answer_q");
                        KindEditor.sync(aq);
                        var answer = $(aq).val();
                        if (answer) {
                            $(tpSubject).addClass("current");
                        } else {
                            $(tpSubject).removeClass("current");
                        }
                    }
                });
            }
        }

        //固定悬浮在指定位置
        if (opts.top) {
            $(document).scroll(function () {
                var top = $(document).scrollTop();
                if (top > opts.top) {
                    $(opts.box).css("position", "fixed");
                    $(opts.box).css("top", opts.top);
                    $(opts.box).css("right", opts.right);
                }
                else {
                    $(opts.box).css("position", "absolute");
                    $(opts.box).css("top", "0");
                    $(opts.box).css("right", "0");
                }
            })
        }

        //点击节点滚动到相应的题号
        $(opts.box).find(".tpSubject").click(function () {
            tpSubjectId = $(this).attr("tpSubjectId");
            var subjectItem = $(".subject_item[tpSubjectId='{0}']".format(tpSubjectId));
            var top = $(subjectItem).offset().top; 
            var headerbox = $("#headerbox").height();
            $('html,body').animate({
                scrollTop: top - headerbox - 10
            },300);    
        });
    },

    //获取试卷答题结果
    subjectReply: function (subject_item) {
        var type = $(subject_item).attr("type");
        var replys = [];
        if (type == "single") {
            var reply = {
                value: "",
                questionId: $(subject_item).find(".question-box").attr("qid")
            };
            var answer_q = $(subject_item).find(".answer_q:checked");
            if (answer_q.length) {
                reply.value = answer_q.val();
            }
            replys.push(reply);
        }
        else if (type == "multi") {
            var reply = {
                value: "",
                questionId: $(subject_item).find(".question-box").attr("qid")
            };
            var answer_qs = $(subject_item).find(".answer_q:checked");
            $(answer_qs).each(function (i, aq) {
                reply.value += $(aq).val();
            });
            replys.push(reply);
        }
        else if (type == "bit") {
            var reply = {
                value: "",
                questionId: $(subject_item).find(".question-box").attr("qid")
            };
            var answer_q = $(subject_item).find(".answer_q:checked");
            if (answer_q.length) {
                reply.value = answer_q.val();
            }
            replys.push(reply);
        }
        else if (type == "fill") {
            var reply = {
                value: "",
                questionId: $(subject_item).find(".question-box").attr("qid")
            };
            var replyValues = [];
            $(subject_item).find(".subject-input.answer_q").each(function (i, aq) {
                replyValues.push($(aq).val());
            });
            reply.value = JSON.stringify(replyValues);
            replys.push(reply);
        }
        else if (type == "shortAnswer") {
            var reply = {
                value: "",
                questionId: $(subject_item).find(".question-box").attr("qid")
            };
            var answer_q = $(subject_item).find(".kindeditor.answer_q");
            reply.value = $(answer_q).val();
            replys.push(reply);
        }

        return replys;
    },

    //获取试卷
    getTestPaper: function (testPaperId, testId) {
        var testPaper = null;
        $.ajax({
            async: false,
            type: "post",
            dataType: "json",
            data: { id: testPaperId, testId: testId },
            url: getUrl("TestPaper", "Detail", { area: "Adm" }),
            success: function (data) {
                testPaper = data;
            }
        });

        return testPaper;
    },

    //倒计时
    timeOut: function (opts) {
        var param = {
            time: null,
            box:null,
            finishedCallBack: null,//到时间回调函数
            timeCallBackArray:[]//时间段内的调用函数
        };
        var opts = $.extend(param, opts);
        var interval = setInterval(getRTime, 1000);
        function getRTime() {
            timeSeconds--;
            var minutes = parseInt(timeSeconds / 60);
            var seconds = timeSeconds % 60;
            if (timeSeconds <= 0) {
                opts.finishedCallBack();
                clearInterval(interval);
            }
            $(opts.timeCallBackArray).each(function (i, row) {
                if (minutes == row.time && seconds == 0) {
                    row.callBack();
                }
            });
            $(opts.box).text(minutes + ":" + seconds);
        }
        var time = opts.time;
        var timeSeconds = time;
    }
}

/**********************************题目增、删、改**************************************/
//根据不同题型显示html
function showSubjectHtml(currentType) {
    if (currentType == "single") {
        $("#selectFormBox").show();
        $("#answerSingle").show();
        $("#answerMulti").hide();
        $("#answerBit").hide();
        $("#answerFill").hide();
        refreshSubjectColumns();
    } else if (currentType == "multi") {
        $("#selectFormBox").show();
        $("#answerSingle").hide();
        $("#answerMulti").show();
        $("#answerBit").hide();
        $("#answerFill").hide();
        refreshSubjectColumns();
    } else if (currentType == "bit") {
        $("#selectFormBox").hide();
        $("#answerSingle").hide();
        $("#answerMulti").hide();
        $("#answerBit").show();
        $("#answerFill").hide();
    } else if (currentType == "fill") {
        $("#selectFormBox").hide();
        $("#answerSingle").hide();
        $("#answerMulti").hide();
        $("#answerBit").hide();
        $("#answerFill").show();
    }else if (currentType == "shortAnswer") {
        $("#selectFormBox").hide();
        $("#answerSingle").hide();
        $("#answerMulti").hide();
        $("#answerBit").hide();
        $("#answerFill").hide();
        $("#answerShortAnswer").show();
    }
}

//刷新选项
function refreshSubjectColumns() {
    //刷新选项
    var choices = ztsubject.getChoices();
    $("#Options_box .Options_box_item .choice").each(function (i, choice) {
        $(choice).html("{0}.".format(choices[i]));
        $(choice).attr("value", choices[i]);
    });

    //刷新答案
    if (currentType == "single") {
        var val = $("#answerSingle *[name='Answer']:checked").val();
        $("#answerSingle .answerBox").html("");
        $("#Options_box .Options_box_item .choice").each(function (i, choice) {
            $("#answerSingle .answerBox").append('<span class="item"><input name="Answer" type="radio" value="{0}"><span>{0}.</span></span>'.format($(choice).attr("value")));
        });
        if (val) {
            $("#answerSingle *[name='Answer']*[value='{0}']".format(val)).attr("checked", "checked");
        }
    }
    else if (currentType == "multi") {
        var vals = [];
        $("#answerMulti *[name='Answer']:checked").each(function (i, answer) {
            vals.push($(answer).val());
        });
        $("#answerMulti .answerBox").html("");
        $("#Options_box .Options_box_item .choice").each(function (i, choice) {
            $("#answerMulti .answerBox").append('<span class="item"><input name="Answer" type="checkbox" value="{0}"><span>{0}.</span></span>'.format($(choice).attr("value")));
        });
        if (vals.length) {
            $(vals).each(function (i, val) {
                $("#answerMulti *[name='Answer']*[value='{0}']".format(val)).attr("checked", "checked");
            });
        }
    }
}

//添加选项
function addColumn(a, data) {
    var item = $(a).closest(".Options_box_item");
    var column = createColumnHtml(data);

    if (item.length) {
        item.after(column);
    } else {
        $("#Options_box").append(column);
    }

    refreshSubjectColumns();
}

//创建一个可选项html
function createColumnHtml(data) {
    var dom = $(
        '<div class="Options_box_item">' +
        '<span class="choice"></span>' +
        '<div contenteditable="plaintext-only" class="width350 option_value"></div> ' +
        '<span style="position:relative;left:40px;">' +
        '<span class="btnAdd" style="margin-left:-30px;" ><a href="javascript:void(0);" style="color:blue" onclick="addColumn(this)">添加</a></span> ' +
        '<span style="margin-left:10px;"><a href="javascript:void(0);" style="color:blue" onclick="deleteColumn(this)">删除</a></span>' +
        '</span>' +
        '</div>');
    if (data) {
        dom.find(".option_value").html(data.value);
    }
    dom.find(".seniorInput").click(function () {
        $("#question-edit-dialog").dialog({
            width: 750,
            height: 350,
            closed: false,
            title: "添加",
            href: getUrl("Question", "SeniorInput", { area: "Adm" }),
            onBeforeClose: function () {
                KindEditor.remove('textarea[name="seniorInput_option"]');
            },
            onLoad: function () {
                $(KindEditor.instances).each(function (k, ke) {
                    if (ke.k_name == "seniorInput_option")
                        ke.html(dom.find(".option_value").html());
                });
            },
            buttons: [{
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {
                    $(KindEditor.instances).each(function (k, ke) {
                        if (ke.k_name == "seniorInput_option")
                            dom.find(".option_value").html(ke.html());
                    });
                    $('#question-edit-dialog').dialog('close');
                }
            }, {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#question-edit-dialog').dialog('close');
                }
            }]
        });
    });
    return dom;
}

//删除选项
function deleteColumn(a) {
    if ($("#Options_box .Options_box_item").length == 1) {
        $dialogShow({ Type: "error", Content: "当前不允许再删除了！" });
        return;
    }
    var item = $(a).closest(".Options_box_item");
    item.remove();

    refreshSubjectColumns();
}