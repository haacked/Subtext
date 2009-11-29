<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt">
   <xsl:output method="html"/>

   <xsl:template match="/">
      <div class="container">
         <span class="containerTitle">Code Analysis and Testing</span>
         <span class="containerSubtitle">Code Coverage Report</span>
         <div class="containerContents">
            <xsl:apply-templates select="//coverageReport" />
         </div>
      </div>
   </xsl:template>

   <xsl:template match="coverageReport">
      <xsl:variable name="reportType" select="//coverageReport/@reportTitle" />
      <xsl:variable name="threshold" select="//coverageReport/project/@acceptable" />

      <table class="section" rules="groups" cellpadding="2" cellspacing="0">
         <xsl:call-template name="header" />
      </table>
      <table class="section" rules="groups" cellpadding="2" cellspacing="0">
         <tbody>
            <xsl:call-template name="projectSummary">
               <xsl:with-param name="threshold" select="$threshold" />
            </xsl:call-template>
         </tbody>
         <tbody>
            <xsl:if test="$reportType = 'Module Summary' or $reportType = 'Module Namespace Summary'">
               <xsl:call-template name="moduleSummary">
                  <xsl:with-param name="threshold" select="$threshold" />
               </xsl:call-template>
            </xsl:if>
            
            <xsl:if test="$reportType = 'Module Namespace Summary'">
               <xsl:call-template name="moduleNamespaceSummary">
                  <xsl:with-param name="threshold" select="$threshold" />
               </xsl:call-template>
            </xsl:if>
         </tbody>
         <tbody>
            <xsl:if test="$reportType = 'Namespace Summary'">
               <xsl:call-template name="namespaceSummary">
                  <xsl:with-param name="threshold" select="$threshold" />
               </xsl:call-template>
            </xsl:if>
         </tbody>
      </table>
      <table class="section" rules="groups" cellpadding="2" cellspacing="0">
         <tbody>
            <xsl:if test="count(./exclusions) != 0">
               <xsl:call-template name="exclusionsSummary" />
            </xsl:if>
         </tbody>
      </table>
      <xsl:call-template name="footer" />
   </xsl:template>
   
   <!-- Report Header -->
   <xsl:template name="header">
      <tr>
         <th>Files:</th>
         <td ><xsl:value-of select="./project/@files" /></td>
         <th>Total non-comment lines:</th>
         <td><xsl:value-of select="./project/@nonCommentLines" /></td>
      </tr>
      <tr>
         <th>Classes:</th>
         <td><xsl:value-of select="./project/@classes" /></td>
         <th>Total sequence points:</th>
         <td><xsl:value-of select="./project/@sequencePoints" /></td>
      </tr>
      <tr>
         <th>Members:</th>
         <td><xsl:value-of select="./project/@members" /></td>
         <th>Total unvisited sequence points:</th>
         <td><xsl:value-of select="./project/@unvisitedPoints" /></td>
      </tr>
   </xsl:template>
   
   <!-- Project Summary -->
   <xsl:template name="projectSummary">
      <xsl:param name="threshold" />

      <tr>
         <td colspan="4">&#160;</td>
      </tr>
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
         <xsl:with-param name="shaded" select="position() mod 2=0" />
      </xsl:call-template>
   </xsl:template>
      
   <!-- Modules Summary -->
   <xsl:template name="moduleSummary">
      <xsl:param name="threshold" />

      <tr>
         <td colspan="4">&#160;</td>
      </tr>
      <tr class="header2">
         <th width="300px">Modules</th>
         <th width="75px">Unvisited<br/> SeqPts</th>
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
      
   <!-- Namespaces per Module Summary -->
   <xsl:template name="moduleNamespaceSummary">
      <xsl:param name="threshold" />

      <xsl:for-each select="//coverageReport/modules/module">
         <tr>
            <td colspan="4">&#160;</td>
         </tr>
         <tr class="header2">
            <th width="300px">Module</th>
            <th width="75px">Unvisited<br/> SeqPts</th>
            <th colspan="2">Coverage</th>
         </tr>
         <xsl:call-template name="coverageDetailSecondary">
            <xsl:with-param name="name" select="./@name" />
            <xsl:with-param name="unvisitedPoints" select="./@unvisitedPoints" />
            <xsl:with-param name="sequencePoints" select="./@sequencePoints" />
            <xsl:with-param name="coverage" select="./@coverage" />
            <xsl:with-param name="threshold" select="$threshold" />
            <xsl:with-param name="shaded" select="position() mod 2=0" />
         </xsl:call-template>
         <tr class="header3">
            <th colspan="4">Namespaces</th>
         </tr>          
         <xsl:for-each select="./namespace">
            <xsl:call-template name="coverageIndentedDetail">
               <xsl:with-param name="name" select="./@name" />
               <xsl:with-param name="unvisitedPoints" select="./@unvisitedPoints" />
               <xsl:with-param name="sequencePoints" select="./@sequencePoints" />
               <xsl:with-param name="coverage" select="./@coverage" />
               <xsl:with-param name="threshold" select="$threshold" />
               <xsl:with-param name="shaded" select="position() mod 2=0" />
            </xsl:call-template>
         </xsl:for-each>
      </xsl:for-each>
   </xsl:template>
      
   <!-- Namespaces Summary -->
   <xsl:template name="namespaceSummary">
      <xsl:param name="threshold" />

      <tr>
         <td colspan="4">&#160;</td>
      </tr>
      <tr class="header2">
         <th width="300px">Namespaces</th>
         <th width="75px">Unvisited<br/> SeqPts</th>
         <th colspan="2">Coverage</th>
      </tr>
      <xsl:for-each select="//coverageReport/namespaces/namespace">
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
   
   <!-- Coverage detail row in secondary grid header displaying a name, statistics and graph bar -->
   <xsl:template name="coverageDetailSecondary">
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
         <td >
            <xsl:call-template name="detailPercent">
               <xsl:with-param name="notVisited" select="$unvisitedPoints" />
               <xsl:with-param name="total" select="$sequencePoints" />
               <xsl:with-param name="threshold" select="$threshold" />
               <xsl:with-param name="scale" select="200" />
            </xsl:call-template>
         </td>
      </tr>
   </xsl:template>
   
   <!-- Coverage detail row with indented item name and shrunk graph bar -->
   <xsl:template name="coverageIndentedDetail">
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
               <xsl:with-param name="scale" select="170" />
            </xsl:call-template>
         </td>
      </tr>
   </xsl:template>
      
   <!-- Exclusions Summary -->
   <xsl:template name="exclusionsSummary">
      <tr>
         <td>Excluded From Coverage Results</td>
         <td>All Code Within</td>
      </tr>
      <xsl:for-each select="//coverageReport/exclusions/exclusion">
         <tr>
            <td><xsl:value-of select="@name" /></td>
            <td><xsl:value-of select="@category" /></td>
         </tr>
      </xsl:for-each>
   </xsl:template>
   
   <!-- Footer -->
   <xsl:template name="footer">
      <tr>
         <td colspan="4">&#160;</td>
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