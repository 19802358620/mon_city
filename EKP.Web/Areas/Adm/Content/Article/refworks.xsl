<?xml version="1.0" encoding="utf-16"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:param name="domailUrl" />
  <!-- localized strings -->
  <xsl:variable name='PeriodicalPaper'>PeriodicalPaper</xsl:variable>   <!--期刊文章-->
  <xsl:variable name='ThesisPaper'>ThesisPaper</xsl:variable>           <!--学位论文-->
  <xsl:variable name='ConferencePaper'>ConferencePaper</xsl:variable>   <!--会议论文-->
  <xsl:variable name='Patent'>Patent</xsl:variable>                     <!--专利-->
  <xsl:variable name='Standard'>Standard</xsl:variable>                 <!--标准-->
  <xsl:variable name='NSTLHY'>NSTLHYPaper</xsl:variable>                
  <xsl:variable name='NSTLQK'>NSTLQKPaper</xsl:variable>                
  <xsl:variable name='OAPaper'>OAPaper</xsl:variable> 
  <xsl:variable name='dot'>.</xsl:variable>
  <xsl:variable name='newLine'>&lt;br/&gt;</xsl:variable>

  <xsl:output method='html'  omit-xml-declaration="yes" indent="yes"/>
  <xsl:template match="/ResourceList">    
    <xsl:for-each   select="*">
      <div class="daochu_mailcontent">
        <xsl:variable name="ResourceType" select="name()"></xsl:variable>
        <xsl:choose>
          <xsl:when test="$ResourceType=$PeriodicalPaper">
            <xsl:call-template name="TemplatePeriodical"/>
          </xsl:when>
          <xsl:when test="$ResourceType=$ThesisPaper">
            <xsl:call-template name="TemplateThesis"/>
          </xsl:when>
          <xsl:when test="$ResourceType=$ConferencePaper">
            <xsl:call-template name="TemplateConference"/>
          </xsl:when>
          <xsl:when test="$ResourceType=$Patent">
            <xsl:call-template name="TemplatePatent"/>
          </xsl:when>
          <xsl:when test="$ResourceType=$Standard">
            <xsl:call-template name="TemplateStandard"/>
          </xsl:when>
          <xsl:when test="$ResourceType=$NSTLHY">
            <xsl:call-template name="TemplateConference"/>
          </xsl:when>
          <xsl:when test="$ResourceType=$NSTLQK">
            <xsl:call-template name="TemplatePeriodical"/>
          </xsl:when>
          <xsl:when test="$ResourceType=$OAPaper">
            <xsl:call-template name="TemplatePeriodical"/>
          </xsl:when>
        </xsl:choose>
        <br/>
      </div>
    </xsl:for-each>
  </xsl:template>

  <!--期刊-->
  <xsl:template name="TemplatePeriodical">
    <xsl:text>RT Journal</xsl:text>
    <br/>
    <xsl:call-template name="TemplateTitle" />
    <xsl:call-template name="TemplateJournal"/>
    <xsl:call-template name="TemplateYearVolumeIssue">
      <xsl:with-param name="ResourceType" select="$PeriodicalPaper"></xsl:with-param>
    </xsl:call-template>
    <xsl:call-template name="TemplatePage">
      <xsl:with-param name="signExpression">SP</xsl:with-param>
    </xsl:call-template>
    <xsl:call-template name="TemplateAbstract"/>
    <xsl:text>LK </xsl:text>
    <xsl:variable name="id" select="ID"></xsl:variable>
    <xsl:value-of select="concat($domailUrl,'asset/detail$exname?id=', $id)"/>
    <br/>
    <xsl:call-template name="TemplateAuthor"/>
    <xsl:call-template name="TemplateOrgan"/>
    <xsl:call-template name="TemplateIssn"/>
    <xsl:call-template name="TemplateCLC"/>   
    <xsl:call-template name="TemplateKeywords"/>
    <xsl:call-template name="TemplatePPDS"/>
    <xsl:call-template name="TemplateNO">
      <xsl:with-param name="ResourceType" select="$PeriodicalPaper"></xsl:with-param>
    </xsl:call-template>
  </xsl:template>

  <!--学位-->
  <xsl:template name="TemplateThesis">
    <xsl:text>RT Dissertation</xsl:text>
    <br/>
    <xsl:call-template name="TemplateTitle" />
    <xsl:call-template name="TemplateAuthor"/>
    <xsl:call-template name="TemplateDegree"/>
    <xsl:call-template name="TemplateYearVolumeIssue">
      <xsl:with-param name="ResourceType" select="$ThesisPaper"></xsl:with-param>
    </xsl:call-template>
    <xsl:call-template name="TemplateAbstract"/>
    <xsl:call-template name="TemplateCLC"/>
    <xsl:call-template name="TemplateKeywords"/>
    <xsl:call-template name="TemplateLanguage"/>
    <xsl:call-template name="TemplatePage">
      <xsl:with-param name="signExpression">OP</xsl:with-param>
    </xsl:call-template>
    <xsl:call-template name="TemplatePPDS"/>
    <xsl:call-template name="TemplateNO">
      <xsl:with-param name="ResourceType" select="$ThesisPaper"></xsl:with-param>
    </xsl:call-template>
  </xsl:template>

  <!--会议-->
  <xsl:template name="TemplateConference">
    <xsl:text>RT Conference Proceeding</xsl:text>
    <br/>
    <xsl:call-template name="TemplateAuthor"/>
    <xsl:call-template name="TemplateTitle" />
    <xsl:call-template name="TemplateSource"/>
    <xsl:call-template name="TemplateYearVolumeIssue">
      <xsl:with-param name="ResourceType" select="$ConferencePaper"></xsl:with-param>
    </xsl:call-template>
    <xsl:call-template name="TemplateLanguage"/>     
    <xsl:call-template name="TemplateKeywords"/>
    <xsl:call-template name="TemplateAbstract"/>
    <xsl:call-template name="TemplateURL"/>
    <xsl:call-template name="TemplateDBProvider"/>
    
  </xsl:template>

  <!--专利-->
  <xsl:template name="TemplatePatent">
    <xsl:text>RT Patent</xsl:text>
    <br/>
    <xsl:call-template name="TemplatePatentName" />
    <xsl:call-template name="TemplateApplicationNo" />
    <xsl:call-template name="TemplateApplicant"/>
    <xsl:call-template name="TemplatePublicationDate"/>
    <xsl:call-template name="TemplateKeywords"/>
    <xsl:call-template name="TemplateAbstract"/>
    <xsl:call-template name="TemplateURL"/>
    <xsl:call-template name="TemplateDBProvider"/>
  </xsl:template>

  <!--标准-->
  <xsl:template name="TemplateStandard">
    <xsl:text>RT Laws/Statutes</xsl:text>
    <br/>
    <xsl:call-template name="TemplateTitle" />
    <xsl:call-template name="TemplateStanCode"/>
    <xsl:call-template name="TemplateIssueDate" />
    <xsl:call-template name="TemplateKeywords"/>
    <xsl:call-template name="TemplateAbstract"/>
    <xsl:call-template name="TemplateURL"/>
    <xsl:call-template name="TemplateDBProvider"/>
  </xsl:template>


  <!--标题-->
  <xsl:template name="TemplateTitle">
    <xsl:for-each select="Titles/Title/Text">
      <xsl:variable name="i" select="position()"></xsl:variable>
      <xsl:if test="$i &lt; 3">
        <xsl:text>T</xsl:text>
        <xsl:value-of select="$i"/>
        <xsl:text> </xsl:text>
        <xsl:value-of select="text()"/>
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
        <xsl:text>JF </xsl:text>
        <xsl:value-of select="$Name"/>
        <br/>
      </xsl:when>
      <xsl:when test='string($NameEn) != ""'>
        <xsl:text>JF </xsl:text>
        <xsl:value-of select="$NameEn"/>
        <br/>
      </xsl:when>
    </xsl:choose>    
  </xsl:template>

  <!-- 学位授予单位-->
  <xsl:template name="TemplateDegree">    
    <xsl:variable name="Degree" select="Degree/text()"></xsl:variable>    
    <xsl:if test='string($Degree) != ""'>
      <xsl:text>ED </xsl:text>
      <xsl:value-of select="$Degree"/>
      <br/>
    </xsl:if>
  </xsl:template>

  <!--作者-->
  <xsl:template name="TemplateAuthor">
    <xsl:variable name="AuthorCount" select="count(Creators/Creator/Name)"></xsl:variable>
    <xsl:variable name="OrganizationCount" select="count(Creators/Creator/Organization)"></xsl:variable>
    <xsl:variable name="OrganizationText" select="Creators/Creator/Organization/text()"></xsl:variable>
    <xsl:variable name="SchoolText" select="School/text()"></xsl:variable>
    <xsl:if test="$AuthorCount &gt; 0">
      <xsl:text>A1 </xsl:text>
    </xsl:if>
    <xsl:for-each select="Creators/Creator/Name">
      <xsl:if test="string(text())!=''">
        <xsl:value-of select="text()"/>
        <xsl:text>;</xsl:text>
      </xsl:if>
    </xsl:for-each>
    <xsl:if test="$AuthorCount &gt; 0">
      <br/>
    </xsl:if>
    <xsl:if test='string($OrganizationText)!=""'>
      <xsl:text>AD </xsl:text>
    </xsl:if>
    <xsl:if test='string($SchoolText)!=""'>
      <xsl:text>AD </xsl:text>
      <xsl:value-of select="$SchoolText"/>
    </xsl:if>
    <xsl:for-each select="Creators/Creator/Organization">
      <xsl:if test="string(text())!=''">        
        <xsl:value-of select="text()"/>
        <xsl:text>;</xsl:text>
      </xsl:if>
    </xsl:for-each>
    <xsl:if test='string($OrganizationText)!=""'>
      <br/>
    </xsl:if>
    <xsl:if test='string($SchoolText)!=""'>
      <br/>
    </xsl:if>
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
          <xsl:text>YR </xsl:text>
          <xsl:value-of select="$Year"/>
          <!--<xsl:value-of select="substring-before($Year, '-')"/>-->
          <br/>
        </xsl:if>
        <xsl:if test='string($Volum)!=""'>
          <xsl:text>VO </xsl:text>
          <xsl:value-of select="$Volum"/>
          <br/>
        </xsl:if>
        <xsl:if test="string($Issue)!=&quot;&quot; and string($Issue)!='&quot;&quot;'">
          <xsl:text>IS </xsl:text>
          <xsl:value-of select="$Issue"/>
          <br/>
        </xsl:if>
      </xsl:when>
      <xsl:when test="$ResourceType=$ConferencePaper">
        <xsl:if test='string($Year)!=""'>
          <xsl:text>YR </xsl:text>
          <xsl:value-of select="substring-before($Year, '-')"/>
          <br/>
        </xsl:if>
      </xsl:when>
      <xsl:when test="$ResourceType=$ThesisPaper">
        <xsl:if test='string($Year)!=""'>
          <xsl:text>YR </xsl:text>
          <xsl:value-of select="substring-before($Year, '-')"/>
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
      <xsl:value-of select='$signExpression'/>
      <xsl:text> </xsl:text>
      <xsl:value-of select='$PageNum'/>
      <br/>
    </xsl:if>
  </xsl:template>

  <!--专利申请人-->
  <xsl:template name='TemplateApplicant'>
    <xsl:variable name='ApplicantCount' select='count(Applicants/Applicant)'></xsl:variable>
    <xsl:if test='$ApplicantCount &gt; 0'>
      <xsl:text>NO </xsl:text>
    </xsl:if>
    <xsl:for-each select='Applicants/Applicant'>
      <xsl:value-of select='text()'/>
      <xsl:text> </xsl:text>
    </xsl:for-each>
    <xsl:if test='$ApplicantCount &gt; 0'>
      <br/>
    </xsl:if>
  </xsl:template>

  <!--专利国别-->
  <xsl:template name='TemplateAreaCode'>
    <xsl:variable name='AreaCode' select='AreaCode/text()'></xsl:variable>
    <xsl:if test='string($AreaCode)!=""'>
      <xsl:text>%Z </xsl:text>
      <xsl:value-of select='substring-before($AreaCode, ";")'/>
      <br/>
    </xsl:if>
  </xsl:template>

  <!--专利题名-->
  <xsl:template name='TemplatePatentName'>
    <xsl:variable name='PatentName' select='PatentName/text()'></xsl:variable>
    <xsl:if test='string($PatentName)!=""'>
      <xsl:text>T1 </xsl:text>
      <xsl:value-of select='$PatentName'/>
      <br/>
    </xsl:if>
  </xsl:template>
  <!--专利号-->
  <xsl:template name='TemplateApplicationNo'>
    <xsl:variable name='ApplicationNo' select='ApplicationNo/text()'></xsl:variable>
    <xsl:text>NO </xsl:text>
    <xsl:value-of select='$ApplicationNo'/>
    <br/>
  </xsl:template>

  <!--专利公告日期-->
  <xsl:template name='TemplatePublicationDate'>
    <xsl:variable name='PublicationDate' select='PublicationDate/text()'></xsl:variable>
    <xsl:if test='string($PublicationDate)!=""'>
      <xsl:text>YR </xsl:text>
      <xsl:value-of select='substring-before($PublicationDate, "-")'/>
      <br/>
    </xsl:if>
  </xsl:template>
  <!--标准编号-->
  <xsl:template name='TemplateStanCode'>
    <xsl:variable name='StanCode' select='StanCode/text()'></xsl:variable>
    <xsl:text>NO </xsl:text>
    <xsl:value-of select='$StanCode'/>
    <br/>
  </xsl:template>

  <!--标准发布年-->
  <xsl:template name='TemplateIssueDate'>
    <xsl:variable name='IssueDate' select='IssueDate/text()'></xsl:variable>
    <xsl:if test='string($IssueDate)!=""'>
      <xsl:text>YR </xsl:text>
      <xsl:value-of select='substring-before($IssueDate, "-")'/>
      <br/>
      <xsl:text> </xsl:text>
    </xsl:if>
  </xsl:template>
  <!--会议母体文献-->
  <xsl:template name='TemplateSource'>
    <xsl:variable name='Source' select='Source/text()'></xsl:variable>
    <xsl:if test='string($Source)!=""'>
      <xsl:text>JF </xsl:text>
      <xsl:value-of select='$Source'/>
      <br/>
    </xsl:if>
  </xsl:template>

  <!--关键词-->
  <xsl:template name='TemplateKeywords'>
    <xsl:variable name="KeywordCount" select="count(Keywords/Keyword)"></xsl:variable>
    <xsl:if test="$KeywordCount &gt; 0">
      <xsl:text>K1 </xsl:text>
    </xsl:if>
    <xsl:for-each select='Keywords/Keyword'>
      <xsl:value-of select='text()'/>
      <xsl:text> </xsl:text>
    </xsl:for-each>
    <xsl:if test="$KeywordCount &gt; 0">
      <br/>
    </xsl:if>
  </xsl:template>
  <!--会议-->
  <xsl:template name="TemplateMeeting">
    <xsl:variable name="name" select="Conference/Name/text()"></xsl:variable>
    <xsl:variable name="date" select="Conference/Date/text()"></xsl:variable>
    <xsl:variable name="location" select="Conference/Locus/text()"></xsl:variable>
    <xsl:variable name="convener" select="Conference/Convener/text()"></xsl:variable>
    <xsl:if test='string($name)!=""'>
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
      <xsl:text>{Year}: </xsl:text>
      <xsl:value-of select='substring-before($date, "-")'/>
      <br/>
    </xsl:if>
  </xsl:template>
  <!--摘要及URL-->
  <xsl:template name="TemplateAbstract">
    <xsl:variable name="AbstractCount" select="count(Abstracts/Abstract)"></xsl:variable>
    <xsl:variable name="OtherAbstractCount" select="count(Abstract)"></xsl:variable>
    <xsl:choose>
      <xsl:when test="$AbstractCount &gt; 0">
        <xsl:for-each select="Abstracts/Abstract">
          <xsl:if test="position()=1">
            <xsl:text>AB </xsl:text>
            <xsl:value-of select="Text/text()"/>
            <br/>
          </xsl:if>
        </xsl:for-each>
      </xsl:when>
      <xsl:when test="$OtherAbstractCount &gt; 0">
        <xsl:text>AB </xsl:text>
        <xsl:value-of select="Abstract/text()"/>
        <br/>
      </xsl:when>
    </xsl:choose>    
  </xsl:template>
  <!--URL-->
  <xsl:template name="TemplateURL">
    <xsl:text>UL </xsl:text>
    <xsl:variable name="id" select="ID"></xsl:variable>
    <xsl:value-of select="concat($domailUrl, $id)"/>
    <xsl:text>$exname</xsl:text>
    <br/>
  </xsl:template>
  <!--DOI-->
  <xsl:template name="TemplateDOI">
    <xsl:variable name="doi" select="DOI/text()"></xsl:variable>
    <xsl:if test='string($doi)!=""'>
      <xsl:text>{DOI}: </xsl:text>
      <xsl:value-of select='$doi'/>
      <br/>
    </xsl:if>
  </xsl:template>
  <!--Database Provider-->
  <xsl:template name="TemplateDBProvider">
    <xsl:text>DB 重庆维普资讯有限公司</xsl:text>
    <br/>
  </xsl:template>
  <!--语言-->
  <xsl:template name="TemplateLanguage">
    <xsl:variable name="Language" select="Language/text()"></xsl:variable>
    <xsl:if test='string($Language)!=""'>
      <xsl:text>LA </xsl:text>
      <xsl:value-of select="$Language"/>
      <br/>
    </xsl:if>
  </xsl:template>
  <!--CLC-->
  <xsl:template name="TemplateCLC">
    <xsl:variable name="clcCount" select="count(CLCs/CLC)"></xsl:variable>
    <xsl:if test="$clcCount &gt; 0">
      <xsl:text>CL </xsl:text>
    </xsl:if>
    <xsl:for-each select="CLCs/CLC">
      <xsl:value-of select="Code/text()"/>
    </xsl:for-each>
    <xsl:if test="$clcCount &gt; 0">
      <br/>
    </xsl:if>
  </xsl:template>
  <!--PP 中国\nDS 重庆维普-->
  <xsl:template name="TemplatePPDS">
    <xsl:text>PP 中国</xsl:text>
    <br/>
    <xsl:text>DS 重庆维普</xsl:text>
    <br/>
  </xsl:template>
  <!--其他信息-->
  <xsl:template name="TemplateNO">
    <xsl:param name="ResourceType"></xsl:param>
    <xsl:text>NO </xsl:text>
    <xsl:if test="$ResourceType=$PeriodicalPaper">
      <xsl:variable name="AuthorCount" select="count(Creators/Creator/Name)"></xsl:variable>
      <xsl:if test="$AuthorCount &gt; 0">
        <xsl:text>作者个数:</xsl:text>
        <xsl:value-of select="$AuthorCount"/>
        <xsl:text>; 第一作者:</xsl:text>
        <xsl:value-of select="Creators/Creator/Name['position()=1']/text()"/>
        <xsl:text>; </xsl:text>
      </xsl:if>
    </xsl:if>
    <xsl:variable name="FundCount" select="count(Funds/Fund)"></xsl:variable>
      <xsl:if test="$FundCount &gt; 0">
        <xsl:text>基金项目:</xsl:text>
        <xsl:for-each select="Funds/Fund">
          <xsl:value-of select="text()"/>
          <xsl:text>; </xsl:text>
        </xsl:for-each>
    </xsl:if>
    <xsl:if test="$ResourceType=$ThesisPaper">
      <xsl:variable name="Tutor" select="Tutor/text()"></xsl:variable>      
      <xsl:variable name="Major" select="Major/text()"></xsl:variable>
      <xsl:if test='string($Major) != ""'>
        <xsl:text>作者专业:</xsl:text>
        <xsl:value-of select="$Major"/>
        <xsl:text>; </xsl:text>
      </xsl:if>
      <xsl:if test='string($Tutor) != ""'>
        <xsl:text>导师姓名:</xsl:text>
        <xsl:value-of select="$Tutor"/>
        <xsl:text>; </xsl:text>
      </xsl:if>
    </xsl:if>    
    <br/>
  </xsl:template>

  <!--ISSN-->
  <xsl:template name="TemplateIssn">
    <xsl:variable name="ISSN" select="Periodical/ISSN/text()"></xsl:variable>
    <xsl:if test='string($ISSN)!=""'>
      <xsl:text>SN </xsl:text>
      <xsl:value-of select="$ISSN"/>
      <br/>
    </xsl:if>
  </xsl:template>

  <!--机构-->
  <xsl:template name="TemplateOrgan">
    <xsl:for-each select="Organs/Organ">
      <xsl:if test='string(text())!=""'>
        <xsl:text>AD </xsl:text>
        <xsl:value-of select="text()"/>
        <br/>
      </xsl:if>
    </xsl:for-each>
  </xsl:template>
</xsl:stylesheet>