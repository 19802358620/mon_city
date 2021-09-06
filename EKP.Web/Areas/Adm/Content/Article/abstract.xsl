<?xml version="1.0"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:param name="domailUrl" />
  <!-- localized strings -->
  <xsl:variable name='PeriodicalPaper'>PeriodicalPaper</xsl:variable>
  <xsl:variable name='dot'>.</xsl:variable>

  <xsl:output method="html" omit-xml-declaration="yes" indent="yes"/>
  <xsl:template match="/ResourceList">
    <xsl:for-each   select="//PeriodicalPaper">
      <div class="daochu_mailcontent">
        <xsl:text>[</xsl:text>
        <xsl:value-of select="position()"/>
        <xsl:text>]</xsl:text>
        <xsl:if test="Type=3">
          <xsl:call-template name="TemplateJournalPaper"/>
        </xsl:if>
        <xsl:if test="Type=2">
          <xsl:call-template name="TemplateJournal"/>
        </xsl:if>
        <xsl:if test="Type=4">
          <xsl:call-template name="TemplateDegree"/>
        </xsl:if>
        <!--<xsl:if test="Type=3">
                    <xsl:call-template name="TemplateConference"/>
                </xsl:if>
                <xsl:if test="Type=4">
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
        <br/>
      </div>
    </xsl:for-each>
  </xsl:template>


  <!--作者-->
  <xsl:template name="TemplateAuthor">
    <xsl:variable name="ResourceType" select="name()"></xsl:variable>
    <xsl:variable name="AuthorCount" select="count(Creators/Creator/Name)"></xsl:variable>
    <xsl:variable name="icount"/>
    <xsl:for-each select="Creators/Creator/Name">
      <xsl:value-of select="text()"/>
      <xsl:choose>
        <xsl:when test="position() &lt; last()">
          <xsl:text>,</xsl:text>
        </xsl:when>
       <xsl:otherwise>
            <xsl:text>.</xsl:text>
       </xsl:otherwise>
      </xsl:choose>
    </xsl:for-each>
    <!--<xsl:if test="$AuthorCount>0">
      <xsl:text>.</xsl:text>
    </xsl:if>-->
  </xsl:template>
<!--年，卷(期)-->
  <xsl:template name="TemplateYearVolumeIssue">
    <xsl:param name="signExpression"></xsl:param>
    <xsl:variable name="Year" select="PublishDate/text()"></xsl:variable>
    <xsl:variable name="Volum" select="Volum/text()"></xsl:variable>
    <xsl:variable name="Issue" select="Issue/text()"></xsl:variable>
    <xsl:if test='string($Year)!=""'>
      <xsl:value-of select="substring-before($Year, '-')"/>
      <!--<xsl:value-of select="$signExpression"/>-->
    </xsl:if>
    <xsl:if test="$signExpression!=$dot">
      <xsl:if test='string($Volum)!=""'>
        <xsl:value-of select="$signExpression"/>
        <xsl:value-of select="$Volum"/>
      </xsl:if>
      <xsl:if test="string($Issue)!=&quot;&quot; and string($Issue)!='&quot;&quot;'">
        <xsl:text>(</xsl:text>
        <xsl:value-of select="$Issue"/>
        <xsl:text>)</xsl:text>
      </xsl:if>
    </xsl:if>
  </xsl:template>
  <!--页码-->
  <xsl:template name="TemplatePage">
    <xsl:param name="signExpression"></xsl:param>
    <xsl:variable name="PageNum" select="Page/text()"></xsl:variable>
    <xsl:if test='string($PageNum)!=""'>
      <xsl:text>:</xsl:text>
      <xsl:value-of select='$signExpression'/>
      <xsl:value-of select='$PageNum'/>
      <xsl:text>.</xsl:text>
    </xsl:if>
  </xsl:template>

  <!--专著-->
  <xsl:template name="TemplateBooks">
    <xsl:call-template name="TemplateAuthor">
    </xsl:call-template>
    <xsl:value-of select="Titles/Title/Text/text()"></xsl:value-of>
    <xsl:text>[M]</xsl:text>
    <xsl:variable name="Name" select="Periodical/Name/text()"></xsl:variable>
    <!--<xsl:if test='string($Name) != ""'>
        <xsl:text>.</xsl:text>
        <xsl:value-of select="$Name"/>
        <xsl:text>,</xsl:text>
      </xsl:if>-->
    <xsl:text>.</xsl:text>
    <!--<xsl:call-template name="TemplateYearVolumeIssue">
        <xsl:with-param name="signExpression">,</xsl:with-param>
      </xsl:call-template>-->
    <!--<xsl:call-template name="TemplatePage">
        <xsl:with-param name="signExpression" select="_"></xsl:with-param>
      </xsl:call-template>-->
    <xsl:value-of select="Organs/Organ"/>
    <xsl:text>,</xsl:text>
    <xsl:value-of select="PublishDate"/>
    <xsl:text>.</xsl:text>
  </xsl:template>

  <!--会议论文-->
  <xsl:template name="TemplateConference">
    <xsl:call-template name="TemplateAuthor"></xsl:call-template>
    <xsl:value-of select="Titles/Title/Text/text()"></xsl:value-of>
    <xsl:text>[A]</xsl:text>
    <xsl:if test="Hyname!=''">
      <xsl:text>.</xsl:text>
      <xsl:value-of select="Hyname"/>
      <xsl:text>[C]</xsl:text>
    </xsl:if>
    <xsl:if test="Hypressplace!=''">
      <xsl:if test="Hypressorganization!=''">
        <xsl:text>.</xsl:text>
        <xsl:value-of  select="Hypressplace"></xsl:value-of>
        <xsl:text>：</xsl:text>
        <xsl:value-of  select="Hypressorganization"></xsl:value-of>
      </xsl:if>
    </xsl:if>
    <xsl:if test="PublishDate!=''">
      <xsl:text>,</xsl:text>
      <xsl:value-of  select="PublishDate"></xsl:value-of>
    </xsl:if>
    <xsl:if test="Page!=''">
      <xsl:text>:</xsl:text>
      <xsl:value-of  select="Page"></xsl:value-of>
    </xsl:if>
    <xsl:text>.</xsl:text>
  </xsl:template>

  <!--期刊-->
  <xsl:template name="TemplateJournal">
    <xsl:call-template name="TemplateAuthor"/>
    <xsl:value-of select="Titles/Title/Text/text()"></xsl:value-of>
    <xsl:text>[J]</xsl:text>
    <xsl:variable name="Name" select="Periodical/Name/text()"></xsl:variable>
    <xsl:if test='string($Name) != ""'>
      <xsl:text>.</xsl:text>
      <xsl:value-of select="$Name"/>
      <xsl:text>,</xsl:text>
    </xsl:if>
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

  <!--期刊文章-->
  <xsl:template name="TemplateJournalPaper">
    <xsl:call-template name="TemplateAuthor"/>
    <xsl:value-of select="Titles/Title/Text/text()"></xsl:value-of>
    <xsl:text>[J]</xsl:text>
    <xsl:variable name="Name" select="Periodical/Name/text()"></xsl:variable>
    <xsl:if test='string($Name) != ""'>
      <xsl:text>.</xsl:text>
      <xsl:value-of select="$Name"/>
      <xsl:text>,</xsl:text>
    </xsl:if>
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

  <!--学位论文-->
  <xsl:template name="TemplateDegree">
    <xsl:call-template name="TemplateAuthor">
    </xsl:call-template>
    <xsl:value-of select="Titles/Title/Text/text()"></xsl:value-of>
    <xsl:text>[D]</xsl:text>
    <xsl:if test="Organs/Organ!=''">
      <xsl:text>.</xsl:text>
      <xsl:value-of select="Organs/Organ"/>
    </xsl:if>
    <xsl:if test="PublishDate!=''">
      <xsl:text>,</xsl:text>
      <xsl:value-of  select="PublishDate"></xsl:value-of>
      <xsl:text>.</xsl:text>
    </xsl:if>
    <!--<xsl:text>:</xsl:text>-->
  </xsl:template>

  <!--标准-->
  <xsl:template name="TemplateStandard">
    <xsl:if test="StanCode!=''">
      <xsl:value-of  select="StanCode"></xsl:value-of>
      <xsl:text>.</xsl:text>
    </xsl:if>
    <xsl:value-of select="Titles/Title/Text/text()"></xsl:value-of>
    <xsl:text>[S]</xsl:text>
    <xsl:if test="Bzissued!=''">
      <xsl:text>.</xsl:text>
      <xsl:value-of select="Bzissued"/>
    </xsl:if>
    <xsl:if test="IssueDate!=''">
      <xsl:text>,</xsl:text>
      <xsl:value-of  select="IssueDate"></xsl:value-of>
    </xsl:if>
    <xsl:if test="Organs/Organ!=''">
      <xsl:text>.</xsl:text>
      <xsl:value-of select="Organs/Organ"/>
    </xsl:if>
    <xsl:text>,</xsl:text>
    <xsl:value-of  select="PublishDate"></xsl:value-of>
    <xsl:text>.</xsl:text>
    <!--<xsl:text>:</xsl:text>-->
  </xsl:template>

  <!--专利-->
  <xsl:template name="TemplatePatent">
    <xsl:call-template name="TemplateAuthor">
    </xsl:call-template>
    <xsl:value-of select="Titles/Title/Text/text()"></xsl:value-of>
    <xsl:text>[P].</xsl:text>
    <xsl:value-of select="AreaCode"/>
    <xsl:if test="ApplicationNo!=''">
      <xsl:text>：</xsl:text>
      <xsl:value-of  select="ApplicationNo"></xsl:value-of>
    </xsl:if>
    <xsl:if test="PublishDate!=''">
      <xsl:text>,</xsl:text>
      <xsl:value-of  select="PublishDate"></xsl:value-of>
    </xsl:if>
    <xsl:text>:</xsl:text>
  </xsl:template>

  <!--科技成果-->
  <xsl:template  name="TemplateAchievement">
    <xsl:call-template name="TemplateAuthor">
    </xsl:call-template>
    <xsl:value-of select="Titles/Title/Text/text()"></xsl:value-of>
    <xsl:text>[Z]</xsl:text>
    <xsl:if test="Organ!=''">
      <xsl:text>.</xsl:text>
      <xsl:value-of select="Organ"/>
    </xsl:if>
    <xsl:if test="Years!=''">
      <xsl:text>.</xsl:text>
      <xsl:value-of select="Years"/>
    </xsl:if>
    <xsl:text>:</xsl:text>
  </xsl:template>

  <!--产品样本-->
  <xsl:template name="TemplateProduct">
    <xsl:value-of select="Titles/Title/Text/text()"></xsl:value-of>
    <xsl:text>[Z].</xsl:text>
    <xsl:value-of select="Cpcountry"/>
    <xsl:value-of select="Organ"/>
  </xsl:template>

  <!--科技报告-->
  <xsl:template name="TemplateTechnicalReport">
    <xsl:call-template name="TemplateAuthor">
    </xsl:call-template>
    <xsl:value-of select="Titles/Title/Text/text()"></xsl:value-of>
    <xsl:text>[Z]</xsl:text>
    <xsl:if test="Organ!=''">
      <xsl:text>.</xsl:text>
      <xsl:value-of select="Organ"/>
    </xsl:if>
    <xsl:if test="Years!=''">
      <xsl:text>.</xsl:text>
      <xsl:value-of select="Years"/>
    </xsl:if>
    <xsl:text>:</xsl:text>
  </xsl:template>

  <!--政策法规-->
  <xsl:template name="TemplatePolicy">
    <xsl:value-of select="Titles/Title/Text/text()"></xsl:value-of>
    <xsl:text>[Z]</xsl:text>
    <xsl:if test="ContClass!=''">
      <xsl:text>.[</xsl:text>
      <xsl:value-of select="ContClass"/>
      <xsl:if test="ContDetClass!=''">
        <xsl:text>--</xsl:text>
        <xsl:value-of select="ContDetClass"/>
      </xsl:if>
      <xsl:text>]</xsl:text>
    </xsl:if>
    <xsl:if test="Years!=''">
      <xsl:text>.</xsl:text>
      <xsl:value-of select="Years"/>
    </xsl:if>
    <xsl:text>:</xsl:text>
  </xsl:template>
</xsl:stylesheet>