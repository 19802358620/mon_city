<?xml version="1.0"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:param name="domailUrl" />
  <!-- localized strings -->
  <xsl:variable name='PeriodicalPaper'>PeriodicalPaper</xsl:variable>
  <xsl:variable name='ThesisPaper'>ThesisPaper</xsl:variable>
  <xsl:variable name='ConferencePaper'>ConferencePaper</xsl:variable>
  <xsl:variable name='Patent'>Patent</xsl:variable>
  <xsl:variable name='Standard'>Standard</xsl:variable>
  <xsl:variable name='NSTLHY'>NSTLHYPaper</xsl:variable>
  <xsl:variable name='NSTLQK'>NSTLQKPaper</xsl:variable>
  <xsl:variable name='OAPaper'>OAPaper</xsl:variable>
  <xsl:variable name='dot'>.</xsl:variable>
  <xsl:variable name='newLine'>&lt;br/&gt;</xsl:variable>

  <xsl:output method="html" omit-xml-declaration="yes" indent="yes"/>
  <xsl:template match="/ResourceList">
    <xsl:for-each   select="*">
      <div class="daochu_mailcontent">
        <xsl:variable name="ResourceType" select="name()"></xsl:variable>
        <xsl:choose>
          <xsl:when test="$ResourceType=$PeriodicalPaper">
            <xsl:call-template name="TemplatePeriodical"/>
          </xsl:when>
     
        </xsl:choose>
        <br/>
      </div>
    </xsl:for-each>
  </xsl:template>

  <!--期刊-->
  <xsl:template name="TemplatePeriodical">
    <xsl:text>%0 Journal Article</xsl:text>
    <br/>
    <xsl:call-template name="TemplateAuthor"/>
    <xsl:call-template name="TemplateOrgan"/>
    <xsl:call-template name="TemplateIssn"/>
    <xsl:call-template name="TemplateTitle" />
    <xsl:call-template name="TemplateJournal"/>
    <xsl:call-template name="TemplateYearVolumeIssue">
      <xsl:with-param name="signExpression">,</xsl:with-param>
    </xsl:call-template>
    <xsl:call-template name="TemplatePage">
      <xsl:with-param name="signExpression" select="_"></xsl:with-param>
    </xsl:call-template>
    <xsl:call-template name="TemplateKeywords"/>
    <xsl:call-template name="TemplateAbstract"/>
  </xsl:template>


  <!--标准-->
  <xsl:template name="TemplateStandard">
    <xsl:text>%0 Legal Rule or Regulation</xsl:text>
    <br/>
    <xsl:call-template name="TemplateStanCode"/>
    <xsl:call-template name="TemplateTitle" />
    <xsl:call-template name="TemplateIssueDate" />
    <xsl:call-template name="TemplateKeywords"/>
    <xsl:call-template name="TemplateAbstract"/>
  </xsl:template>


  <!--标题-->
  <xsl:template name="TemplateTitle">
    <xsl:for-each select="Titles/Title/Text">
      <xsl:variable name="i" select="position()"></xsl:variable>
      <xsl:if test="$i=1">
        <xsl:text>%T </xsl:text>
        <xsl:value-of select="text()"></xsl:value-of>
        <br/>
      </xsl:if>
    </xsl:for-each>
  </xsl:template>

  <!--刊名-->
  <xsl:template name="TemplateJournal">
    <xsl:variable name="Name" select="Periodical/Name/text()"></xsl:variable>
    <xsl:variable name="NameEn" select="Periodical/NameEn/text()"></xsl:variable>
    <xsl:choose>
      <xsl:when test='string($Name) != ""'>
        <xsl:text>%J </xsl:text>
        <xsl:value-of select="$Name"/>
        <br/>
      </xsl:when>
      <xsl:when test='string($NameEn) != ""'>
        <xsl:text>%J </xsl:text>
        <xsl:value-of select="$NameEn"/>
        <br/>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <!-- 学位授予单位-->
  <xsl:template name="TemplateSchool">
    <xsl:variable name="School" select="School/text()"></xsl:variable>
    <xsl:if test='string($School) != ""'>
      <xsl:value-of select="$School"/>
      <xsl:text>,</xsl:text>
    </xsl:if>
  </xsl:template>

  <!--作者-->
  <xsl:template name="TemplateAuthor">
    <xsl:variable name="AuthorCount" select="count(Creators/Creator/Name)"></xsl:variable>
    <xsl:if test="$AuthorCount &gt; 0">
      <xsl:text>%A </xsl:text>
      <xsl:value-of select="Creators/Creator/Name['position()=1']/text()"/>
      <br/>
    </xsl:if>
  </xsl:template>

  <!--年，卷(期)-->
  <xsl:template name="TemplateYearVolumeIssue">
    <xsl:param name="signExpression"></xsl:param>
    <xsl:variable name="Year" select="PublishDate/text()"></xsl:variable>
    <xsl:variable name="Volum" select="Volum/text()"></xsl:variable>
    <xsl:variable name="Issue" select="Issue/text()"></xsl:variable>
    <xsl:variable name="Language" select="Language/text()"></xsl:variable>
    <xsl:if test='string($Language)!=""'>
      <xsl:text>%G </xsl:text>
      <xsl:value-of select="$Language"/>
      <br/>
    </xsl:if>
    <xsl:if test='string($Year)!=""'>
      <xsl:text>%D </xsl:text>
      <xsl:value-of select="$Year"/>
      <!--<xsl:value-of select="substring-before($Year, '-')"/>-->
      <br/>
    </xsl:if>
    <xsl:if test="$signExpression!=$dot">
      <xsl:if test='string($Issue)!=""'>
        <xsl:text>%N </xsl:text>
        <xsl:value-of select="$Issue"/>
        <br/>
      </xsl:if>
      <xsl:if test='string($Volum)!=""'>
        <xsl:text>%V </xsl:text>
        <xsl:value-of select="$Volum"/>
        <br/>
      </xsl:if>
    </xsl:if>
  </xsl:template>
  <!--页码-->
  <xsl:template name="TemplatePage">
    <xsl:param name="signExpression"></xsl:param>
    <xsl:variable name="PageNum" select="PageNum/text()"></xsl:variable>
    <xsl:if test='string($PageNum)!=""'>
      <xsl:text>%P </xsl:text>
      <xsl:value-of select='$PageNum'/>
      <br/>
    </xsl:if>
  </xsl:template>

  <!--标准编号-->
  <xsl:template name='TemplateStanCode'>
    <xsl:variable name='StanCode' select='StanCode/text()'></xsl:variable>
    <xsl:text>%Z </xsl:text>
    <xsl:value-of select='$StanCode'/>
    <br/>
  </xsl:template>

  <!--标准发布年-->
  <xsl:template name='TemplateIssueDate'>
    <xsl:variable name='IssueDate' select='IssueDate/text()'></xsl:variable>
    <xsl:if test='string($IssueDate)!=""'>
      <xsl:text>%D </xsl:text>
      <xsl:value-of select='substring-before($IssueDate, "-")'/>
      <br/>
      <xsl:text> </xsl:text>
    </xsl:if>
  </xsl:template>
  <!--会议母体文献-->
  <xsl:template name='TemplateSource'>
    <xsl:variable name='Source' select='Source/text()'></xsl:variable>
    <xsl:if test='string($Source)!=""'>
      <xsl:text>%B </xsl:text>
      <xsl:value-of select='$Source'/>
      <br/>
    </xsl:if>
  </xsl:template>

  <!--关键词-->
  <xsl:template name='TemplateKeywords'>
    <xsl:variable name="KeywordCount" select="count(Keywords/Keyword)"></xsl:variable>
    <xsl:if test="$KeywordCount &gt; 0">
      <xsl:text>%K </xsl:text>
    </xsl:if>
    <xsl:for-each select='Keywords/Keyword'>
      <xsl:value-of select='text()'/>
      <xsl:text> </xsl:text>
    </xsl:for-each>
    <xsl:if test="$KeywordCount &gt; 0">
      <br/>
    </xsl:if>
  </xsl:template>
  <!--摘要-->
  <xsl:template name="TemplateAbstract">
    <xsl:variable name="AbstractCount" select="count(Abstracts/Abstract)"></xsl:variable>
    <xsl:variable name="OtherAbstractCount" select="count(Abstract)"></xsl:variable>
    <xsl:choose>
      <xsl:when test="$AbstractCount &gt; 0">
        <xsl:text>%X </xsl:text>
        <xsl:for-each select="Abstracts/Abstract">
          <xsl:if test="position()=1">
            <xsl:value-of select="Text/text()"/>
          </xsl:if>
        </xsl:for-each>
        <br/>
      </xsl:when>
      <xsl:when test="$OtherAbstractCount &gt; 0">
        <xsl:text>%X </xsl:text>
        <xsl:value-of select="Abstract/text()"/>
        <br/>
      </xsl:when>
    </xsl:choose>
    <xsl:text>%U </xsl:text>
    <xsl:variable name="id" select="ID"></xsl:variable>
    <xsl:value-of select="concat($domailUrl,'asset/detail$exname?id=', $id)"/>
    
    <br/>
    <xsl:text>%W 重庆维普资讯有限公司</xsl:text>
    <br/>
  </xsl:template>

  <!--ISSN-->
  <xsl:template name="TemplateIssn">
    <xsl:variable name="ISSN" select="Periodical/ISSN/text()"></xsl:variable>
    <xsl:if test='string($ISSN)!=""'>
      <xsl:text>%@: </xsl:text>
      <xsl:value-of select="$ISSN"/>
      <br/>
    </xsl:if>
  </xsl:template>
  
  <!--机构-->
  <xsl:template name="TemplateOrgan">
    <xsl:variable name="OrganCount" select="count(Organs/Organ)"></xsl:variable>   
      <xsl:if test="$OrganCount &gt; 0">
        <xsl:text>%+: </xsl:text>
        <xsl:value-of select="Organs/Organ['position()=1']/text()"/>
        <br/>
      </xsl:if>
  </xsl:template>
</xsl:stylesheet>