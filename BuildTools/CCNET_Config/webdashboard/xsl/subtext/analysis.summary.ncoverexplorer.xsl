<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/TR/xhtml1/strict">
   <xsl:output method="html"/>
   <xsl:variable name="reportType" select="//coverageReport/@reportTitle" />
   <xsl:variable name="threshold" select="//coverageReport/project/@acceptable" />

   <xsl:template match="coverageReport">
      <div class="buildcontainer">
         <span class="containerTitle">Code Coverage: 
            <xsl:if test="./project/@coverage &lt; ./project/@acceptable"> Failed</xsl:if>
            <xsl:if test="./project/@coverage &gt;= ./project/@acceptable"> Passed</xsl:if>
         </span>
         <div class="containerContents">
            <table class="section" rules="groups" cellpadding="2" cellspacing="0" border="0">
               <xsl:call-template name="ncoverexplorer.project.summary">
                  <xsl:with-param name="threshold" select="$threshold" />
               </xsl:call-template>

               <xsl:if test="$reportType = 'Module Summary' or $reportType = 'Module Namespace Summary'">
                  <xsl:call-template name="ncoverexplorer.module.summary">
                     <xsl:with-param name="threshold" select="$threshold" />
                  </xsl:call-template>
               </xsl:if>        
            </table>
         </div>
      </div>
   </xsl:template>

   <!-- Project Summary -->
   <xsl:template name="ncoverexplorer.project.summary">
      <xsl:param name="threshold" />
      
      <tr class="header2">
         <th width="300px">Project</th>
         <th width="75px">Unvisited<br/> SeqPts</th>
         <th colspan="2">Coverage</th>
      </tr>
      <xsl:call-template name="ncoverexplorer.coverage.detail">
         <xsl:with-param name="name" select="./project/@name" />
         <xsl:with-param name="unvisitedPoints" select="./project/@unvisitedPoints" />
         <xsl:with-param name="sequencePoints" select="./project/@sequencePoints" />
         <xsl:with-param name="coverage" select="./project/@coverage" />
         <xsl:with-param name="threshold" select="$threshold" />
      </xsl:call-template>
   </xsl:template>
      
   <!-- Modules Summary -->
   <xsl:template name="ncoverexplorer.module.summary">
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
         <xsl:call-template name="ncoverexplorer.coverage.detail">
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
   <xsl:template name="ncoverexplorer.coverage.detail">
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
            <xsl:call-template name="ncoverexplorer.detail.percent">
               <xsl:with-param name="notVisited" select="$unvisitedPoints" />
               <xsl:with-param name="total" select="$sequencePoints" />
               <xsl:with-param name="threshold" select="$threshold" />
               <xsl:with-param name="scale" select="200" />
            </xsl:call-template>
         </td>
      </tr>
   </xsl:template>

   <!-- Draw % Green/Red/Yellow Bar -->
   <xsl:template name="ncoverexplorer.detail.percent">
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
