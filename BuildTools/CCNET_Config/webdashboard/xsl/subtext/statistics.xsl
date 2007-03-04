<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
   <xsl:output method="html"/>
   <xsl:param name="applicationPath"/>

   <xsl:template match="/">
      <xsl:apply-templates select="/statistics" />
   </xsl:template>
   
   <xsl:template name="header">
      <xsl:param name="day" />
      <xsl:param name="month" />
      <xsl:param name="year" />

      <xsl:variable name="totalCount" select="count(integration)"/>
      <xsl:variable name="successCount" select="count(integration[@status='Success'])"/>
      <xsl:variable name="failureCount" select="$totalCount - $successCount"/>
      <xsl:variable name="totalCountForTheDay" select="count(integration[@day=$day and @month=$month and @year=$year])"/>
      <xsl:variable name="successCountForTheDay" select="count(integration[@status='Success' and @day=$day and @month=$month and @year=$year])"/>
      <xsl:variable name="failureCountForTheDay" select="$totalCountForTheDay - $successCountForTheDay"/>

      <table class="section" rules="groups" cellpadding="2" cellspacing="0" border="0">
         <thead>
            <tr class="header2">
               <th>&#160;</th>
               <th>For today</th>
               <th>Overall</th>
            </tr>
         </thead>
         <tbody>
            <tr>
               <th class="shaded" align="left">Total Builds</th>
               <td><xsl:value-of select="$totalCountForTheDay"/></td>
               <td><xsl:value-of select="$totalCount"/></td>
            </tr>
            <tr>
               <th class="shaded" align="left">Number of Successful</th>
               <td><xsl:value-of select="$successCountForTheDay"/></td>
               <td><xsl:value-of select="$successCount"/></td>
            </tr>
            <tr>
               <th class="shaded" align="left">Number of Failed</th>
               <td><xsl:value-of select="$failureCountForTheDay"/></td>
               <td><xsl:value-of select="$failureCount"/></td>
            </tr>
         </tbody>
      </table>
   </xsl:template>
      
   <xsl:template match="/statistics">
      <xsl:variable name="day" select="//timestamp/@day"/>
      <xsl:variable name="month" select="//timestamp/@month"/>
      <xsl:variable name="year" select="//timestamp/@year"/>

      <style>
         *.pass{
            background-color: #33ff99;
         }
         *.fail{  
            background-color: #ff6600;
         }
         *.unknown{  
            background-color: #ffffcc;
         }
      </style>
   
   <div class="container">
      <span class="containerTitle">Project Statistics</span>
      <span class="containerSubtitle">As of <xsl:value-of select="$year"/>-<xsl:value-of select="$month"/>-<xsl:value-of select="$day"/></span>
      <div class="containerContents">
         <xsl:call-template name="header">
            <xsl:with-param name="day" select="$day" />
            <xsl:with-param name="month" select="$month" />
            <xsl:with-param name="year" select="$year" />
         </xsl:call-template>

         <br/>

         <table class="section" rules="groups" cellpadding="2" cellspacing="0" border="0">
            <tr>
               <td width="140"><b>Build Status Key:</b></td>
               <td align="left">
                  <div style="padding:2px 0px 2px 2px;display:inline;width:70px;height:16px"><img src="{$applicationPath}/images/error.png" class="statusimage"/> Failure</div>
                  <div style="padding:2px 0px 2px 2px;display:inline;width:78px;height:16px"><img src="{$applicationPath}/images/check.png" class="statusimage"/> Success</div>
                  <div style="padding:2px 0px 2px 2px;display:inline;width:85px;height:16px"><img src="{$applicationPath}/images/warning.png" class="statusimage"/> Exception</div>
                  <div style="padding:2px 0px 2px 2px;display:inline;width:90px;height:16px"><img src="{$applicationPath}/images/unknown.png" class="statusimage"/> Unknown</div>
               </td>
            </tr>
         </table>
         
         <table class="section" rules="groups" cellpadding="2" cellspacing="0" border="0">
            <thead>
               <tr class="header2">
                  <th>&#160;</th>
                  <th>Build Label</th>
                  <th>Start Time</th>
                  <th>Duration</th>
                  <th>Lines of Code</th>
               </tr>
            </thead>
            <xsl:apply-templates select="integration">
               <xsl:sort select="position()" data-type="number" order="descending"/>
            </xsl:apply-templates>
         </table>     
      </div>
   </div>
   </xsl:template>
   
   <xsl:template match="integration">
   <tbody>
      <tr>               
         <xsl:if test="position() mod 2 = 0">
            <xsl:attribute name="class">shaded</xsl:attribute>
         </xsl:if>
         <td>
            <img class="statusimage">
               <xsl:attribute name="src"> 
                  <xsl:choose>
                     <xsl:when test="./@status = 'Success'"><xsl:value-of select="$applicationPath"/>/images/check.png</xsl:when>
                     <xsl:when test="./@status = 'Failure'"><xsl:value-of select="$applicationPath"/>/images/error.png</xsl:when>
                     <xsl:when test="./@status = 'Exception'" ><xsl:value-of select="$applicationPath"/>/images/warning.png</xsl:when>
                     <xsl:otherwise><xsl:value-of select="$applicationPath"/>/images/unknown.png</xsl:otherwise>
                  </xsl:choose>
               </xsl:attribute> 
               <xsl:attribute name="alt"><xsl:value-of select="./@status"/></xsl:attribute>
            </img>
         </td>
         <td><xsl:value-of select="./@build-label"/></td>
         <td><xsl:value-of select="./statistic[@name='StartTime']"/></td>
         <td><xsl:value-of select="./statistic[@name='Duration']"/></td>
         <td><xsl:value-of select="./statistic[@name='Code Lines']"/></td>
      </tr>
      <xsl:if test="./@status = 'Failure'">
         <tr>
            <xsl:if test="position() mod 2 = 0">
               <xsl:attribute name="class">shaded</xsl:attribute>
            </xsl:if>
            <td>&#160;</td>
            <td colspan="4">
               <strong><xsl:value-of select="./statistic[@name='BuildErrorType']"/>:</strong>&#160;<xsl:value-of select="./statistic[@name='BuildErrorMessage']"/>
            </td>
         </tr>
      </xsl:if>
      <tr>
         <xsl:if test="position() mod 2 = 0">
            <xsl:attribute name="class">shaded</xsl:attribute>
         </xsl:if>
         <td>&#160;</td>
         <td colspan="4">
            <table width="100%">
               <tr>
                  <th align="left">Unit Tests</th>

                  <td align="right">Total:</td>
                  <td align="left"><xsl:value-of select="./statistic[@name='TestCountMb']"/></td>

                  <td align="right">Failures:</td>
                  <td><xsl:value-of select="./statistic[@name='TestFailuresMb']"/></td>

                  <td align="right">Ignored:</td>
                  <td><xsl:value-of select="./statistic[@name='TestIgnoredMb']"/></td>

                  <td align="right">Coverage:</td>
                  <td><xsl:value-of select="./statistic[@name='Coverage']"/></td>
               </tr>
               <tr>
                  <th align="left">FxCop</th>

                  <td align="right">Errors:</td>
                  <td><xsl:value-of select="./statistic[@name='FxCop Errors']"/></td>

                  <td align="right">Warnings:</td>
                  <td><xsl:value-of select="./statistic[@name='FxCop Warnings']"/></td>
               </tr>
            </table>
         </td>
      </tr>
      </tbody>
   </xsl:template>
   
   
</xsl:stylesheet>

  