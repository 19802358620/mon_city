<?xml version="1.0"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:param name="domailUrl" />
  <!-- localized strings -->
  <xsl:variable name='PeriodicalPaper' select='//PeriodicalPaper'></xsl:variable>

  <xsl:output method="html" omit-xml-declaration="yes" indent="yes"/>
  <xsl:template  name="main" priority="1"  match="/">
    <xsl:variable name="ResourceType" select="//PeriodicalPaper"></xsl:variable>
    <xsl:choose>
      <xsl:when test="$PeriodicalPaper=$ResourceType">
        <xsl:call-template name="ArticlesTemplete"></xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:call-template name="ObjectTemplate"></xsl:call-template>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/" name="ArticlesTemplete">
    <xsl:for-each   select="//PeriodicalPaper">
      <div class="daochu_mailcontent">
        <xsl:text>[</xsl:text>
        <xsl:value-of select="position()"/>
        <xsl:text>]</xsl:text>
        <xsl:if test="Type=3">
          <xsl:call-template name="TemplatePeriodicalPaper"/>
        </xsl:if>
        <xsl:if test="Type=2">
          <xsl:call-template name="TemplatePeriodical"/>
        </xsl:if>
        <xsl:if test="Type=4">
          <xsl:call-template name="TemplateDegree"/>
        </xsl:if>
        <!--<xsl:if test="Type=3">
                    <xsl:call-template name="TemplateConference"/>
                </xsl:if>-->
        <!--<xsl:if test="Type=4">
                    <xsl:call-template name="TemplatePatent"/>
                </xsl:if>-->
        <xsl:if test="Type=5">
          <xsl:call-template name="TemplateStandard"/>
        </xsl:if>
        <xsl:if test="Type=6">
          <xsl:call-template name="TemplateAchievement"/>
        </xsl:if>
        <xsl:if test="Type=1">
          <xsl:call-template name="TemplateBooks"/>
        </xsl:if>
        <xsl:if test="Type=8">
          <xsl:call-template name="TemplateProduct"/>
        </xsl:if>
        <xsl:if test="Type=9">
          <xsl:call-template name="TemplateTechnicalReport"/>
        </xsl:if>
        <xsl:if test="Type=10">
          <xsl:call-template name="TemplatePolicy"/>
        </xsl:if>
        <xsl:call-template name="TemplateAbstract"/>
        <br/>
      </div>
    </xsl:for-each>
  </xsl:template>

  <!--政策法规-->
  <xsl:template name="TemplatePolicy">
    <xsl:call-template name="TemplateTitle" />
    <xsl:text>.</xsl:text>
    <xsl:variable name="ContClass" select="ContClass/text()"></xsl:variable>
    <xsl:variable name="ContDetClass" select="ContDetClass/text()"></xsl:variable>
    <span style="color:green">
      <xsl:if test='string($ContClass)!=""'>
        <xsl:text>[</xsl:text>
        <xsl:value-of select="$ContClass"/>
        <xsl:if test='string($ContDetClass)!=""'>
          <xsl:text>--</xsl:text>
          <xsl:value-of  select="$ContDetClass"/>
        </xsl:if>
        <xsl:text>]</xsl:text>
      </xsl:if>
      <xsl:value-of select="Years"/>
    </span>
  </xsl:template>

  <!--科技报告-->
  <xsl:template name="TemplateTechnicalReport">
    <xsl:call-template name="TemplateAuthor"/>
    <xsl:call-template name="TemplateTitle" />
    <xsl:text>.</xsl:text>
    <xsl:variable name="ClassName" select="ClassName/text()"></xsl:variable>
    <span style="color:green">
      <xsl:if test='string($ClassName)!=""'>
        <xsl:text>[</xsl:text>
        <xsl:value-of select="$ClassName"/>
        <xsl:text>]</xsl:text>
      </xsl:if>
      <xsl:value-of select="Organ"/>
      <xsl:text>.</xsl:text>
      <xsl:value-of select="Years"/>
    </span>
  </xsl:template>

  <!--产品样本-->
  <xsl:template name="TemplateProduct">
    <xsl:call-template name="TemplateTitle" />
    <xsl:text>.</xsl:text>
    <span style="color:green">
      <xsl:value-of  select="Organ"/>
      <xsl:text>，</xsl:text>
      <xsl:value-of select="Cpcountry"/>
    </span>
  </xsl:template>
  <!--成果-->
  <xsl:template name="TemplateAchievement">
    <xsl:call-template name="TemplateAuthor"/>
    <xsl:call-template name="TemplateTitle" />
    <xsl:text>.</xsl:text>
    <span style="color:green">
      <xsl:value-of  select="Organ"/>
      <xsl:text>,</xsl:text>
      <xsl:value-of select="Years"/>
    </span>
  </xsl:template>
  <!--专著-->
  <xsl:template  name="TemplateBooks">
    <xsl:call-template name="TemplateAuthor"/>
    <xsl:call-template name="TemplateTitle" />[M]
    <xsl:text>.</xsl:text>
    <!--<xsl:call-template name="TemplateJournal"/>
        <xsl:call-template name="TemplateYearVolumeIssue">
            <xsl:with-param name="signExpression">,</xsl:with-param>
        </xsl:call-template>-->
    <!--<xsl:call-template name="TemplatePage">
            <xsl:with-param name="signExpression" select="_"></xsl:with-param>
        </xsl:call-template>-->
    <xsl:value-of select="Organs/Organ"/>
  </xsl:template>
  <!--会议论文-->
  <xsl:template  name="TemplateConference">
    <xsl:call-template name="TemplateAuthor"/>
    <xsl:call-template name="TemplateTitle" />
    <xsl:text>.</xsl:text>
    <xsl:call-template name="TemplateJournal"/>
    <xsl:call-template name="TemplateYearVolumeIssue">
      <xsl:with-param name="signExpression">,</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <!--学位论文-->
  <xsl:template  name="TemplateDegree">
    <xsl:call-template name="TemplateAuthor"/>
    <xsl:call-template name="TemplateTitle" />[D]
    <!--<xsl:text>.</xsl:text>
        <span style="color:green">
            <xsl:value-of select="Organ"/>
            <xsl:text>,</xsl:text>
        </span>
        <xsl:call-template name="TemplateYearVolumeIssue">
            <xsl:with-param name="signExpression">,</xsl:with-param>
        </xsl:call-template>-->
  </xsl:template>
  <!--标准-->
  <xsl:template  name="TemplateStandard">
    <xsl:call-template name="TemplateAuthor"/>
    <xsl:call-template name="TemplateTitle" />
    <xsl:text>[S]</xsl:text>
    <!--<xsl:text>.</xsl:text>
        <span style="color:green">
            <xsl:text>[</xsl:text>
            <xsl:value-of select="BzmainType"/>
            <xsl:text>]</xsl:text>
            <xsl:value-of select="Organ"/>
            <xsl:text>,</xsl:text>
            <xsl:value-of select="IssueDate"/>
            <xsl:text>,</xsl:text>
            <xsl:value-of select="BzStatus"/>
        </span>-->
  </xsl:template>
  <!--专利-->
  <xsl:template  name="TemplatePatent">
    <xsl:call-template name="TemplateAuthor"/>
    <xsl:call-template name="TemplateTitle" />
    <xsl:text>.</xsl:text>
    <span style="color:green">
      <xsl:text>[</xsl:text>
      <xsl:value-of select="Zlmaintype"/>
      <xsl:text>]</xsl:text>
      <xsl:for-each select="Applicants/Applicant">
        <xsl:value-of select="text()"/>
      </xsl:for-each>
    </span>
    <xsl:text>,</xsl:text>
    <xsl:value-of select="Zlapplicationdata"/>
  </xsl:template>
  <!--期刊文章-->
  <xsl:template name="TemplatePeriodicalPaper">

    <xsl:call-template name="TemplateAuthor"/>
    <xsl:call-template name="TemplateTitle" />[J]
    <xsl:text>.</xsl:text>
    <xsl:call-template name="TemplateJournal"/>
    <xsl:value-of select="PublishDate"/>
    <!--<xsl:text>.</xsl:text>-->
    <xsl:call-template name="TemplateYearVolumeIssue">
      <xsl:with-param name="signExpression">,</xsl:with-param>
    </xsl:call-template>
    <xsl:call-template name="TemplatePage">
      <xsl:with-param name="signExpression" select="_"></xsl:with-param>
    </xsl:call-template>
    <xsl:value-of select="Organs/Organ"/>
  </xsl:template>

  <!--期刊-->
  <xsl:template name="TemplatePeriodical">

    <xsl:call-template name="TemplateAuthor"/>
    <xsl:call-template name="TemplateTitle" />[J]
    <xsl:text>.</xsl:text>
    <xsl:call-template name="TemplateJournal"/>
    <xsl:value-of select="PublishDate"/>
    <xsl:text>.</xsl:text>
    <xsl:call-template name="TemplateYearVolumeIssue">
      <xsl:with-param name="signExpression">,</xsl:with-param>
    </xsl:call-template>
    <xsl:call-template name="TemplatePage">
      <xsl:with-param name="signExpression" select="_"></xsl:with-param>
    </xsl:call-template>
    <xsl:value-of select="Organs/Organ"/>
  </xsl:template>

  <!--标题-->
  <xsl:template name="TemplateTitle">
    <xsl:for-each select="Titles/Title/Text">
      <span style="color:#034481">
        <xsl:variable name="i" select="position()"></xsl:variable>
        <xsl:if test="$i=1">
          <xsl:value-of select="text()"></xsl:value-of>
        </xsl:if>
      </span>
    </xsl:for-each>
  </xsl:template>

  <!--刊名，中英文分开来-->
  <xsl:template name="TemplateJournal">
    <xsl:variable name="Name" select="Periodical/Name/text()"></xsl:variable>
    <span style="color:green">
      <xsl:choose>
        <xsl:when test='string($Name) != ""'>
          <xsl:value-of select="$Name"/>
          <xsl:text>,</xsl:text>
        </xsl:when>
      </xsl:choose>
    </span>
  </xsl:template>

  <!--作者-->
  <xsl:template name="TemplateAuthor">
    <xsl:variable name="ResourceType" select="name()"></xsl:variable>
    <xsl:variable name="AuthorCount" select="count(Creators/Creator/Name)"></xsl:variable>
    <xsl:for-each select="Creators/Creator/Name">
      <span style="color:green" data="writer">
        <xsl:value-of select="text()"/>
        <xsl:choose>
          <xsl:when test="position() &lt; last()">
            <xsl:text>,</xsl:text>
          </xsl:when>
          <xsl:otherwise>
            <xsl:text>.</xsl:text>
          </xsl:otherwise>
        </xsl:choose>
      </span>
    </xsl:for-each>
  </xsl:template>

  <!--年，卷(期)-->
  <xsl:template name="TemplateYearVolumeIssue">
    <xsl:param name="signExpression"></xsl:param>
    <xsl:variable name="Year" select="PublishDate/text()"></xsl:variable>
    <xsl:variable name="Volum" select="Volum/text()"></xsl:variable>
    <xsl:variable name="Issue" select="Issue/text()"></xsl:variable>
    <span style="color:green">
      <xsl:if test='string($Year)!=""'>
        <xsl:value-of select="substring-before($Year, '-')"/>
        <!--<xsl:value-of select="$signExpression"/>-->
      </xsl:if>
      <xsl:text>,</xsl:text>
      <xsl:if test="$signExpression!='.'">
        <xsl:if test='string($Volum)!=""'>
          <!--<xsl:value-of select="$signExpression"/>-->
          <xsl:value-of select="$Volum"/>
        </xsl:if>
        <xsl:if test="string($Issue)!=&quot;&quot; and string($Issue)!='&quot;&quot;'">
          <xsl:text>(</xsl:text>
          <xsl:value-of select="$Issue"/>
          <xsl:text>)</xsl:text>
        </xsl:if>
      </xsl:if>
    </span>
  </xsl:template>
  <!--页码-->
  <xsl:template name="TemplatePage">
    <xsl:param name="signExpression"></xsl:param>
    <xsl:variable name="PageNum" select="Page/text()"></xsl:variable>
    <span style="color:green">
      <xsl:if test='string($PageNum)!=""'>
        <xsl:text>:</xsl:text>
        <xsl:value-of select='$signExpression'/>
        <xsl:value-of select='$PageNum'/>
      </xsl:if>
    </span>
  </xsl:template>

  <!--摘要-->
  <xsl:template name="TemplateAbstract">
    <xsl:variable name="Abstract" select="Abstracts/Abstract/Text/text()"></xsl:variable>
    <xsl:if test="$Abstract!=''">
      <br/>
      <span class="daochu_text">
        <xsl:text>摘要：</xsl:text><xsl:value-of select="$Abstract"/>
      </span>
    </xsl:if>
  </xsl:template>


  <xsl:template match="/" name="ObjectTemplate">
    <xsl:for-each   select="//Record">
      <div class="daochu_mailcontent">
        <xsl:variable name ="index" select="position()"></xsl:variable>
        <xsl:text>【</xsl:text>
        <!--引用变量值输出-->
        <xsl:value-of select="$index"/>
        <xsl:text>】</xsl:text>
        <br/>
        <xsl:if test="ObjectType=1">
          <xsl:call-template name="DomainTemplate"></xsl:call-template>
        </xsl:if>
        <xsl:if test="ObjectType=2">
          <xsl:call-template name="FundTemplate"></xsl:call-template>
        </xsl:if>
        <xsl:if test="ObjectType=3">
          <xsl:call-template name="MediaTemplate"></xsl:call-template>
        </xsl:if>
        <xsl:if test="ObjectType=4">
          <xsl:call-template name="OrganTemplate"></xsl:call-template>
        </xsl:if>
        <xsl:if test="ObjectType=5">
          <xsl:call-template name="SubjectTemplate"></xsl:call-template>
        </xsl:if>
        <xsl:if test="ObjectType=6">
          <xsl:call-template name="WriterTemplate"></xsl:call-template>
        </xsl:if>
        <xsl:if test="ObjectType=8">
          <xsl:call-template name="AreaTemplate"></xsl:call-template>
        </xsl:if>
        <br/>
      </div>
    </xsl:for-each>
  </xsl:template>

  <xsl:template name="WriterTemplate">
    <xsl:text>【作 者 名】</xsl:text>
    <span style="color:#034481">
      <xsl:value-of select="Titles/Title/Text"></xsl:value-of>
    </span>
    <xsl:call-template name="ObjectCommonTemplate"></xsl:call-template>
    <xsl:text>【职    称】</xsl:text>
    <xsl:value-of  select="Position"></xsl:value-of>
    <br/>
    <xsl:text>【供职机构】</xsl:text>
    <xsl:value-of  select="OrganOfWriter"></xsl:value-of>
    <br/>
    <xsl:text>【研究主题】</xsl:text>
    <xsl:value-of  select="SubjectsOfWriter"></xsl:value-of>
    <br/>
  </xsl:template>

  <xsl:template name ="OrganTemplate">
    <xsl:text>【机 构 名】</xsl:text>
    <span style="color:#034481">
      <xsl:value-of select="Titles/Title/Text"></xsl:value-of>
    </span>
    <xsl:call-template name="ObjectCommonTemplate"></xsl:call-template>
    <xsl:text>【研究主题】</xsl:text>
    <xsl:value-of  select="SubjectsOfOrgan"></xsl:value-of>
    <br/>
  </xsl:template>

  <xsl:template name="SubjectTemplate">
    <xsl:text>【主 题 名】</xsl:text>
    <span style="color:#034481">
      <xsl:value-of select="Titles/Title/Text"></xsl:value-of>
    </span>
    <xsl:call-template name="ObjectCommonTemplate"></xsl:call-template>
    <xsl:text>【相关学科】</xsl:text>
    <xsl:value-of  select="DomainsOfSubject"></xsl:value-of>
    <br/>
    <xsl:text>【相似主题】</xsl:text>
    <xsl:value-of  select="SimilarSubjects"></xsl:value-of>
    <br/>
  </xsl:template>

  <xsl:template name="FundTemplate">
    <xsl:text>【资 助 名】</xsl:text>
    <span style="color:#034481">
      <xsl:value-of select="Titles/Title/Text"></xsl:value-of>
    </span>
    <xsl:call-template name="ObjectCommonTemplate"></xsl:call-template>
    <xsl:text>【相关学科】</xsl:text>
    <xsl:value-of  select="DomainsOfFund"></xsl:value-of>
    <br/>
  </xsl:template>

  <xsl:template name="MediaTemplate">
    <xsl:text>【传 媒 名】</xsl:text>
    <span style="color:#034481">
      <xsl:value-of select="Titles/Title/Text"></xsl:value-of>
    </span>
    <xsl:call-template name="ObjectCommonTemplate"></xsl:call-template>
    <xsl:text>【研究领域】</xsl:text>
    <xsl:value-of  select="DomainsOfMedia"></xsl:value-of>
    <br/>
  </xsl:template>

  <xsl:template name="DomainTemplate">
    <xsl:text>【学科名】</xsl:text>
    <span style="color:#034481">
      <xsl:value-of select="Titles/Title/Text"></xsl:value-of>
    </span>
    <xsl:call-template name="ObjectCommonTemplate"></xsl:call-template>
  </xsl:template>

  <xsl:template name="ObjectCommonTemplate">
    <br/>
    <xsl:text>【作 品 数】</xsl:text>
    <xsl:value-of select="FwCount"></xsl:value-of>
    <br/>
    <xsl:text>【被 引 量】</xsl:text>
    <xsl:value-of  select="ByCount"></xsl:value-of>
    <br/>
    <xsl:text>【H 指  数】</xsl:text>
    <xsl:value-of  select="Hindex"></xsl:value-of>
    <br/>
  </xsl:template>

  <xsl:template name="AreaTemplate">
    <xsl:text>【地 区】</xsl:text>
    <span style="color:#034481">
      <xsl:value-of select="Titles/Title/Text"></xsl:value-of>
    </span>
    <xsl:call-template name="ObjectCommonTemplate"></xsl:call-template>
    <xsl:text>【研究主题】</xsl:text>
    <xsl:value-of  select="SubjectsOfArea"></xsl:value-of>
    <br/>
    <xsl:text>【相关学科】</xsl:text>
    <xsl:value-of  select="DomainsOfArea"></xsl:value-of>
    <br/>
  </xsl:template>
</xsl:stylesheet>