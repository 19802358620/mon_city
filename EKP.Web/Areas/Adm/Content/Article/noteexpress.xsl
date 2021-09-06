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
  <xsl:variable name='newLine'><br/></xsl:variable>

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
      </div>
      <br/>
      <br/>
    </xsl:for-each>
  </xsl:template>

  <!--期刊-->
  <xsl:template name="TemplatePeriodical">
    <xsl:text>{Reference Type}: Journal Article</xsl:text>
    <br/>
    <xsl:call-template name="TemplateTitle" />
    <xsl:call-template name="TemplateAuthor"/>    
    <xsl:call-template name="TemplateOrgan"/>
    <xsl:call-template name="TemplateIssn"/>
    <xsl:call-template name="TemplateJournal"/>
    <xsl:call-template name="TemplateYearVolumeIssue">
      <xsl:with-param name="ResourceType" select="$PeriodicalPaper"></xsl:with-param>
    </xsl:call-template>
    <xsl:call-template name="TemplatePage">
      <xsl:with-param name="signExpression" select="_"></xsl:with-param>
    </xsl:call-template>
    <xsl:call-template name="TemplateKeywords"/>
    <xsl:call-template name="TemplateAbstract"/>
    <xsl:call-template name="TemplateDOI"/>
    <xsl:call-template name="TemplateDBProvider"/>
    <xsl:call-template name="TemplateLanguage"/>
  </xsl:template>


  <!--标准-->
  <xsl:template name="TemplateStandard">
    <xsl:text>{Reference Type}: Generic</xsl:text>
    <br/>
    <xsl:call-template name="TemplateTitle" />
    <xsl:call-template name="TemplateStanCode"/>
    <xsl:call-template name="TemplateIssueDate" />
    <xsl:call-template name="TemplateKeywords"/>
    <xsl:call-template name="TemplateAbstract"/>
  </xsl:template>


  <!--标题-->
  <xsl:template name="TemplateTitle">
    <xsl:for-each select="Titles/Title/Language">
      <xsl:variable name="i" select="position()"></xsl:variable>
      <xsl:variable name="lan" select="text()"></xsl:variable>
      <xsl:if test="$lan='chi'">
        <xsl:text>{Title}: </xsl:text>
        <xsl:value-of select="../Text['position()=$i']/text()"></xsl:value-of>
        <br/>
      </xsl:if>
      <xsl:if test="$lan='eng'">
        <xsl:text>{Translated Title}: </xsl:text>
        <xsl:value-of select="../Text['position()=$i']/text()"></xsl:value-of>
        <br/>
      </xsl:if>
    </xsl:for-each>
  </xsl:template>
  <!--标题-->
  <xsl:template name="CommonTemplateTitle">
    <xsl:for-each select="Titles/Title/Text">
        <xsl:text>{Title}: </xsl:text>
        <xsl:value-of select="text()"></xsl:value-of>
        <br/>
    </xsl:for-each>
  </xsl:template>
  <!--刊名-->
  <xsl:template name="TemplateJournal">
    <xsl:variable name="Name" select="Periodical/Name/text()"></xsl:variable>
    <xsl:variable name="NameEn" select="Periodical/NameEn/text()"></xsl:variable>    
      <xsl:if test='string($Name) != ""'>
        <xsl:text>{Journal}: </xsl:text>
        <xsl:value-of select="$Name"/>
        <br/>
      </xsl:if>
    <xsl:if test='string($NameEn) != ""'>
      <xsl:text>{Translated Journal}: </xsl:text>
      <xsl:value-of select="$NameEn"/>
      <br/>
    </xsl:if>   
  </xsl:template>

  <!-- 学位授予单位-->
  <xsl:template name="TemplateSchool">    
    <xsl:variable name="Tutor" select="Tutor/text()"></xsl:variable>
    <xsl:variable name="School" select="School/text()"></xsl:variable>
    <xsl:variable name="Major" select="Major/text()"></xsl:variable>
    <xsl:variable name="Degree" select="Degree/text()"></xsl:variable>
    <xsl:if test='string($Tutor) != ""'>
      <xsl:text>{Tertiary Author}: </xsl:text>
      <xsl:value-of select="$Tutor"/>
      <br/>
    </xsl:if>
    <xsl:if test='string($School) != ""'>
      <xsl:text>{Publisher}: </xsl:text>
      <xsl:value-of select="$School"/>
      <br/>
    </xsl:if>
    <xsl:if test='string($Major) != ""'>
      <xsl:text>{Section}: </xsl:text>
      <xsl:value-of select="$Major"/>
      <br/>
    </xsl:if>
    <xsl:if test='string($Degree) != ""'>
      <xsl:text>{Type of Work}: </xsl:text>
      <xsl:value-of select="$Degree"/>
      <br/>
    </xsl:if>
  </xsl:template>

  <!--作者-->
  <xsl:template name="TemplateAuthor">
    <xsl:variable name="AuthorCount" select="count(Creators/Creator/Name)"></xsl:variable>    
    <xsl:for-each select="Creators/Creator/Name">
      <xsl:if test="string(text())!=''">
      <xsl:text>{Author}: </xsl:text>      
      <xsl:value-of select="text()"/>
      <br/>
      </xsl:if>
    </xsl:for-each>      
    <xsl:for-each select="Creators/Creator/Organization">
      <xsl:if test="string(text())!=''">
        <xsl:text>{Author Address}: </xsl:text>
        <xsl:value-of select="text()"/>
        <br/>
      </xsl:if>
    </xsl:for-each>    
  </xsl:template>

  <!--年，卷(期)-->
  <xsl:template name="TemplateYearVolumeIssue">
    <xsl:param name="ResourceType"></xsl:param>
    <xsl:variable name="Year" select="PublishDate/text()"></xsl:variable>
    <xsl:variable name="Volum" select="Volum/text()"></xsl:variable>
    <xsl:variable name="Issue" select="Issue/text()"></xsl:variable>     
    <xsl:choose>
      <xsl:when test="$ResourceType=$PeriodicalPaper">
        <xsl:if test='string($Year)!=""'>
          <xsl:text>{Year}: </xsl:text>
          <xsl:value-of select="$Year"/>
          <!--<xsl:value-of select="substring-before($Year, '-')"/>-->
          <br/>
        </xsl:if>
        <xsl:if test='string($Volum)!=""'>
          <xsl:text>{Volume}: </xsl:text>
          <xsl:value-of select="$Volum"/>
          <br/>
        </xsl:if>
        <xsl:if test="string($Issue)!=&quot;&quot; and string($Issue)!='&quot;&quot;'">
          <xsl:text>{Issue}: </xsl:text>
          <xsl:value-of select="$Issue"/>
          <br/>
        </xsl:if>       
      </xsl:when>
      <xsl:when test="$ResourceType=$ConferencePaper">
        <xsl:if test='string($Year)!=""'>
          <xsl:text>{Date}: </xsl:text>
          <xsl:value-of select="substring-before($Year, '-')"/>
          <br/>
        </xsl:if>
      </xsl:when>
      <xsl:when test="$ResourceType=$ThesisPaper">
        <xsl:if test='string($Year)!=""'>
          <xsl:text>{Year}: </xsl:text>
          <xsl:value-of select="substring-before($Year, '-')"/>
          <br/>
          <xsl:text>{Date}: </xsl:text>
          <xsl:value-of select="substring-before($Year, 'T')"/>
          <br/>
        </xsl:if>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
  <!--页码-->
  <xsl:template name="TemplatePage">
    <xsl:param name="signExpression"></xsl:param>
    <xsl:variable name="PageNum" select="PageNum/text()"></xsl:variable>
    <xsl:if test='string($PageNum)!=""'>
      <xsl:text>{Pages}: </xsl:text>
      <xsl:value-of select='$PageNum'/>
      <br/>
    </xsl:if>
  </xsl:template>

  <!--标准编号-->
  <xsl:template name='TemplateStanCode'>
    <xsl:variable name='StanCode' select='StanCode/text()'></xsl:variable>
    <xsl:text>{Notes}: </xsl:text>
    <xsl:value-of select='$StanCode'/>
    <br/>
  </xsl:template>

  <!--标准发布年-->
  <xsl:template name='TemplateIssueDate'>
    <xsl:variable name='IssueDate' select='IssueDate/text()'></xsl:variable>
    <xsl:if test='string($IssueDate)!=""'>
      <xsl:text>{Year}: </xsl:text>
      <xsl:value-of select='substring-before($IssueDate, "-")'/>
      <br/>
      <xsl:text> </xsl:text>
    </xsl:if>
  </xsl:template>
  <!--会议母体文献-->
  <xsl:template name='TemplateSource'>
    <xsl:variable name='Source' select='Source/text()'></xsl:variable>
    <xsl:if test='string($Source)!=""'>
      <xsl:text>{Tertiary Title}: </xsl:text>
      <xsl:value-of select='$Source'/>
      <br/>
    </xsl:if>
  </xsl:template>

  <!--关键词-->
  <xsl:template name='TemplateKeywords'>
    <xsl:variable name="KeywordCount" select="count(Keywords/Keyword)"></xsl:variable>    
    <xsl:for-each select='Keywords/Keyword'>
      <xsl:text>{Keywords}: </xsl:text>
      <xsl:value-of select='text()'/>
      <br/>
    </xsl:for-each>    
  </xsl:template>
  <!--会议-->
  <xsl:template name="TemplateMeeting">
    <xsl:variable name="name" select="Conference/Name/text()"></xsl:variable>
    <xsl:variable name="date" select="Conference/Date/text()"></xsl:variable>
    <xsl:variable name="location" select="Conference/Locus/text()"></xsl:variable>
    <xsl:variable name="convener" select="Conference/Convener/text()"></xsl:variable>
    <xsl:if test="string($name)!=&quot;&quot; and string($name)!='&quot;&quot;'">
      <xsl:text>{Secondary Title}: </xsl:text>
      <xsl:value-of select='$name'/>
      <br/>
    </xsl:if>   
    <xsl:if test='string($location)!=""'>
      <xsl:text>{Place Published}: </xsl:text>
      <xsl:value-of select='$location'/>
      <br/>
    </xsl:if>
    <xsl:if test='string($convener)!=""'>
      <xsl:text>{Subsidiary Author}: </xsl:text>
      <xsl:value-of select='$convener'/>
      <br/>
    </xsl:if>
    <xsl:if test='string($date)!=""'>      
      <xsl:variable name='YearValue' select='substring-before($date, "-")'></xsl:variable>
      <xsl:if test='$YearValue!="0001"'>
        <xsl:text>{Year}: </xsl:text>
        <xsl:value-of select='$YearValue'/>
        <br/>
      </xsl:if>
    </xsl:if>
  </xsl:template>
  <!--摘要及URL-->
  <xsl:template name="TemplateAbstract">
    <xsl:variable name="AbstractCount" select="count(Abstracts/Abstract)"></xsl:variable>
    <xsl:variable name="OtherAbstractCount" select="count(Abstract)"></xsl:variable>
    <xsl:choose>
      <xsl:when test="$AbstractCount &gt; 0">        
        <xsl:for-each select="Abstracts/Abstract">
          <xsl:text>{Abstract}: </xsl:text>
            <xsl:value-of select="Text/text()"/>
            <br/>
        </xsl:for-each>        
      </xsl:when>
      <xsl:when test="$OtherAbstractCount &gt; 0">
        <xsl:text>{Abstract}: </xsl:text>
        <xsl:value-of select="Abstract/text()"/>
        <br/>
      </xsl:when>
    </xsl:choose>
    <xsl:text>{URL}: </xsl:text>
    <xsl:variable name="id" select="ID"></xsl:variable>
    <xsl:value-of select="concat($domailUrl,'asset/detail$exname?id=', $id)"/>
    <br/>
  </xsl:template>
  <!--DOI-->
  <xsl:template name="TemplateDOI">
    <xsl:variable name="doi" select="DOI/text()"></xsl:variable>
    <xsl:if test="string($doi)!=&quot;&quot; and string($doi)!='&quot;&quot;'">
      <xsl:text>{DOI}: </xsl:text>
      <xsl:value-of select='$doi'/>
      <br/>
    </xsl:if>
  </xsl:template>
  <!--Database Provider-->
  <xsl:template name="TemplateDBProvider">
    <xsl:text>{Database Provider}: 重庆维普资讯有限公司</xsl:text>
    <br/>
  </xsl:template>
  <!--语言-->
  <xsl:template name="TemplateLanguage">
    <xsl:variable name="Language" select="Language/text()"></xsl:variable>
    <xsl:if test='string($Language)!=""'>
      <xsl:text>{Language}: </xsl:text>
      <xsl:value-of select="$Language"/>
      <br/>
    </xsl:if>
  </xsl:template>

  <!--ISSN-->
  <xsl:template name="TemplateIssn">
    <xsl:variable name="ISSN" select="Periodical/ISSN/text()"></xsl:variable>
    <xsl:if test='string($ISSN)!=""'>
      <xsl:text>{ISBN/ISSN}: </xsl:text>
      <xsl:value-of select="$ISSN"/>
      <br/>
    </xsl:if>
  </xsl:template>

  <!--机构-->
  <xsl:template name="TemplateOrgan">
    <xsl:for-each select="Organs/Organ">
      <xsl:if test='string(text())!=""'>
        <xsl:text>{Author Address}: </xsl:text>
        <xsl:value-of select="text()"/>
        <br/>
      </xsl:if>
    </xsl:for-each>
  </xsl:template>
  
</xsl:stylesheet>
