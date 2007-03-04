<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/TR/xhtml1/strict">
   <xsl:output method="html"/>
   <xsl:variable name="nunit2.result.list" select="//report-result/counter"/>
   <xsl:variable name="nunit2.testcount" select="sum($nunit2.result.list/@run-count)"/>
   <xsl:variable name="nunit2.failure" select="sum($nunit2.result.list/@failure-count)"/>
   <xsl:variable name="nunit2.success" select="sum($nunit2.result.list/@success-count)"/>
   <xsl:variable name="nunit2.ignore" select="sum($nunit2.result.list/@ignore-count)"/>
   <xsl:variable name="nunit2.skip" select="sum($nunit2.result.list/@skip-count)"/>
   <xsl:variable name="nunit2.assert" select="sum($nunit2.result.list/@assert-count)"/>

   <!--<xsl:variable name="totalErrorsAndFailures" select="sum($nunit2.failure)"/>-->
  
   <xsl:template match="//report-result">
      <div class="buildcontainer">
         <span class="containerTitle">Unit Tests:
            <xsl:choose>
               <xsl:when test="$nunit2.failure &gt; 0"> Failed</xsl:when>
               <xsl:otherwise> Passed</xsl:otherwise>
            </xsl:choose>
         </span>
         <div class="containerContents">
            <table class="section" rules="groups" cellpadding="2" cellspacing="0" border="0">
               <xsl:choose>
                  <xsl:when test="$nunit2.testcount = 0">
                     <tr><td colspan="2">No tests run.</td></tr>
                     <tr><td colspan="2" class="error">This project doesn't have any tests.</td></tr>
                  </xsl:when>

                  <xsl:otherwise>
                     <xsl:call-template name="mbunit.summary"/>
                  </xsl:otherwise>
               </xsl:choose>

            </table>
         </div>
      </div>
   </xsl:template>

   <!-- Project Summary -->
   <xsl:template name="mbunit.summary">
      <xsl:variable name="percentSuccess" select="$nunit2.success div $nunit2.testcount * 100"/>

      <tr>
         <td width="375px">
            <xsl:choose>
               <xsl:when test="$nunit2.failure = 0">All tests passed </xsl:when>
               <xsl:when test="$nunit2.failure &gt; 0">Some tests passed </xsl:when>
               <xsl:when test="$nunit2.success = 0">No tests passed </xsl:when>
            </xsl:choose>
            <span>(<xsl:value-of select="$nunit2.testcount" /> / <xsl:value-of select="$nunit2.success" /> / <xsl:value-of select="$nunit2.failure" /> / <xsl:value-of select="$nunit2.skip" /> / <xsl:value-of select="$nunit2.ignore" /> / <xsl:value-of select="$nunit2.assert" />)</span>
         </td>
         <td class="percent"><xsl:value-of select="concat(format-number($percentSuccess,'#0.0'), ' %')" /></td>
         <td>
            <xsl:call-template name="mbunit.detail.percent">
               <xsl:with-param name="total" select="$nunit2.testcount" />
               <xsl:with-param name="success" select="$nunit2.success" />
               <xsl:with-param name="failure" select="$nunit2.failure" />
               <xsl:with-param name="ignore" select="$nunit2.ignore" />
               <xsl:with-param name="skip" select="$nunit2.skip" />
               <xsl:with-param name="scale" select="200" />
            </xsl:call-template>
         </td>
      </tr>
   </xsl:template>

   <!-- Draw % Green/Red/Yellow Bar -->
   <xsl:template name="mbunit.detail.percent">
      <xsl:param name="total" />
      <xsl:param name="success" />
      <xsl:param name="failure" />
      <xsl:param name="ignore" />
      <xsl:param name="skip" />
      <xsl:param name="scale" />
      
      <xsl:variable name="coverage" select="$success div $total * 100"/>

      <table cellpadding="0" cellspacing="0">
         <tbody>
            <tr>
               <xsl:if test="not ($success=0)">
                  <td class="graphBarVisited" height="14">
                     <xsl:attribute name="width">
                        <xsl:value-of select="format-number($coverage div 100 * $scale, '0')" />
                     </xsl:attribute>&#160;
                  </td>
               </xsl:if>
               <xsl:if test="not($ignore=0)">
                  <td height="14" class="graphBarSatisfactory">
                     <xsl:attribute name="width">
                        <xsl:value-of select="format-number($ignore div $total * $scale, '0')" />
                     </xsl:attribute>&#160;
                  </td>
               </xsl:if>
               <xsl:if test="not($skip=0)">
                  <td height="14" style="background: orange;">
                     <xsl:attribute name="width">
                        <xsl:value-of select="format-number($skip div $total * $scale, '0')" />
                     </xsl:attribute>&#160;
                  </td>
               </xsl:if>
               <xsl:if test="not($failure=0)">
                  <td height="14" class="graphBarNotVisited">
                     <xsl:attribute name="width">
                        <xsl:value-of select="format-number($failure div $total * $scale, '0')" />
                     </xsl:attribute>&#160;
                  </td>
               </xsl:if>
            </tr>
         </tbody>
      </table>
   </xsl:template>

  
   <xsl:template match="coverageReport">
      <div class="buildcontainer">
         <span class="containerTitle">Code Coverage: 
            <xsl:if test="./project/@coverage &lt; ./project/@acceptable"> Failed</xsl:if>
            <xsl:if test="./project/@coverage &gt;= ./project/@acceptable"> Passed</xsl:if>
         </span>
         <div class="containerContents">
            <table class="section" rules="groups" cellpadding="2" cellspacing="0" border="0">
               <xsl:call-template name="projectSummary">
                  <xsl:with-param name="threshold" select="$threshold" />
               </xsl:call-template>

               <xsl:if test="$reportType = 'Module Summary' or $reportType = 'Module Namespace Summary'">
                  <xsl:call-template name="moduleSummary">
                     <xsl:with-param name="threshold" select="$threshold" />
                  </xsl:call-template>
               </xsl:if>        
            </table>
         </div>
      </div>
   </xsl:template>

   <!-- Project Summary -->
   <xsl:template name="projectSummary">
      <xsl:param name="threshold" />
         <tr class="header2">
            <th width="300px">Project</th>
            <th width="75px">Unvisited<br/> SeqPts</th>
            <th colspan="2">Coverage</th>
         </tr>
         <xsl:call-template name="coverageDetail">
            <xsl:with-param name="name" select="./project/@name" />
            <xsl:with-param name="unvisitedPoints" select="./project/@unvisitedPoints" />
            <xsl:with-param name="sequencePoints" select="./project/@sequencePoints" />
            <xsl:with-param name="coverage" select="./project/@coverage" />
            <xsl:with-param name="threshold" select="$threshold" />
         </xsl:call-template>
   </xsl:template>
      
   <!-- Modules Summary -->
   <xsl:template name="moduleSummary">
      <xsl:param name="threshold" />
         <tr>
            <td colspan="3">&#160;</td>
         </tr>
         <tr class="header2">
            <th>Modules</th>
            <th>Unvisited<br/> SeqPts</th>
            <th colspan="2">Coverage</th>
         </tr>          
         <xsl:for-each select="//coverageReport/modules/module">
            <xsl:call-template name="coverageDetail">
               <xsl:with-param name="name" select="./@name" />
               <xsl:with-param name="unvisitedPoints" select="./@unvisitedPoints" />
               <xsl:with-param name="sequencePoints" select="./@sequencePoints" />
               <xsl:with-param name="coverage" select="./@coverage" />
               <xsl:with-param name="threshold" select="$threshold" />
               <xsl:with-param name="shaded" select="position() mod 2=0" />
            </xsl:call-template>
         </xsl:for-each>
   </xsl:template>

   <!-- Coverage detail row in main grid displaying a name, statistics and graph bar -->
   <xsl:template name="coverageDetail">
      <xsl:param name="name" />
      <xsl:param name="unvisitedPoints" />
      <xsl:param name="sequencePoints" />
      <xsl:param name="coverage" />
      <xsl:param name="threshold" />
      <xsl:param name="shaded" />
      <tr>
         <xsl:if test="$shaded">
            <xsl:attribute name="class">shaded</xsl:attribute>
         </xsl:if>

         <td><xsl:value-of select="$name" /></td>
         <td><xsl:value-of select="$unvisitedPoints" /></td>
         <td class="percent"><xsl:value-of select="concat(format-number($coverage,'#0.0'), ' %')" /></td>
         <td>
            <xsl:call-template name="detailPercent">
               <xsl:with-param name="notVisited" select="$unvisitedPoints" />
               <xsl:with-param name="total" select="$sequencePoints" />
               <xsl:with-param name="threshold" select="$threshold" />
               <xsl:with-param name="scale" select="200" />
            </xsl:call-template>
         </td>
      </tr>
   </xsl:template>

   <!-- Draw % Green/Red/Yellow Bar -->
   <xsl:template name="detailPercent">
      <xsl:param name="notVisited" />
      <xsl:param name="total" />
      <xsl:param name="threshold" />
      <xsl:param name="scale" />
      <xsl:variable name="visited" select="$total - $notVisited" />
      <xsl:variable name="coverage" select="$visited div $total * 100"/>
      <table cellpadding="0" cellspacing="0">
      <tbody>
         <tr>
            <xsl:if test="not ($visited=0)">
               <td class="graphBarVisited" height="14">
                  <xsl:attribute name="width">
                     <xsl:value-of select="format-number($coverage div 100 * $scale, '0')" />
                  </xsl:attribute>&#160;
               </td>
            </xsl:if>
              <xsl:if test="not($notVisited=0)">
               <td height="14">
                  <xsl:attribute name="class">
                     <xsl:if test="$coverage &gt;= $threshold">graphBarSatisfactory</xsl:if>
                     <xsl:if test="$coverage &lt; $threshold">graphBarNotVisited</xsl:if>
                  </xsl:attribute>
                  <xsl:attribute name="width">
                     <xsl:value-of select="format-number($notVisited div $total * $scale, '0')" />
                  </xsl:attribute>&#160;
               </td>
            </xsl:if>
         </tr>
         </tbody>
      </table>
   </xsl:template>

</xsl:stylesheet>
