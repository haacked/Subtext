<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/TR/xhtml1/strict">
   <xsl:output method="html"/>

   <xsl:variable name="fxcop.root" select="//FxCopReport"/>
   <xsl:variable name="fxcop.version" select="$fxcop.root/@Version" />
   <xsl:variable name="fxcop.lastAnalysis" select="$fxcop.root/@LastAnalysis"/>
   <xsl:variable name="fxcop.module.count" select="count($fxcop.root//Module)"/>
   <xsl:variable name="fxcop.rules.count" select="count($fxcop.root//Rule)"/>
   
   <xsl:variable name="issue.total.count" select="count($fxcop.root//Issue)"/>
   <xsl:variable name="issue.criticalerror.count" select="count($fxcop.root//Issue[@Level='CriticalError'])"/>
   <xsl:variable name="issue.criticalwarning.count" select="count($fxcop.root//Issue[@Level='CriticalWarning'])"/>
   <xsl:variable name="issue.error.count" select="count($fxcop.root//Issue[@Level='Error'])"/>
   <xsl:variable name="issue.warning.count" select="count($fxcop.root//Issue[@Level='Warning'])"/>
   <xsl:variable name="issue.information.count" select="count($fxcop.root//Issue[@Level='Information'])"/>

   <xsl:template match="//FxCopReport">
      <div class="buildcontainer">
         <span class="containerTitle">FxCop Static Analysis</span>
         <div class="containerContents">
            <table class="section" rules="groups" cellpadding="2" cellspacing="0" border="0">
               <xsl:choose>
                  <xsl:when test="$fxcop.module.count = 0">
                     <tr><td colspan="2">No analysis performed.</td></tr>
                  </xsl:when>
                  <xsl:otherwise>
                     <xsl:call-template name="fxcop.summary"/>
                     <xsl:call-template name="fxcop.issues"/>
                  </xsl:otherwise>
               </xsl:choose>
            </table>
         </div>
      </div>
   </xsl:template>

   <xsl:template name="fxcop.summary">
      <tr class="header2">
         <th width="300px">Total Assemblies</th>
         <th width="75px">Rules<br/> Violated</th>
         <th colspan="2">Total Issues Found</th>
      </tr>
      <tr valign="top">
         <td><xsl:value-of select="$fxcop.module.count" /></td>
         <td><xsl:value-of select="$fxcop.rules.count" /></td>
         <td colspan="2"><xsl:value-of select="$issue.total.count" /></td>
      </tr>
   </xsl:template>

   <xsl:template name="fxcop.issues">
      <tr>
         <td colspan="3">&#160;</td>
      </tr>
      <tr class="header2">
         <th width="250px">Error Level</th>
         <th width="75px">Issues<br/> Found</th>
         <th colspan="2">&#160;</th>
      </tr>
      <xsl:call-template name="fxcop.detail">
         <xsl:with-param name="name">Critical Errors</xsl:with-param>
         <xsl:with-param name="count" select="$issue.criticalerror.count" />
         <xsl:with-param name="total" select="$issue.total.count" />
      </xsl:call-template>
      <xsl:call-template name="fxcop.detail">
         <xsl:with-param name="name">Critical Warnings</xsl:with-param>
         <xsl:with-param name="count" select="$issue.criticalwarning.count" />
         <xsl:with-param name="total" select="$issue.total.count" />
      </xsl:call-template>
      <xsl:call-template name="fxcop.detail">
         <xsl:with-param name="name">Errors</xsl:with-param>
         <xsl:with-param name="count" select="$issue.error.count" />
         <xsl:with-param name="total" select="$issue.total.count" />
      </xsl:call-template>
      <xsl:call-template name="fxcop.detail">
         <xsl:with-param name="name">Warnings</xsl:with-param>
         <xsl:with-param name="count" select="$issue.warning.count" />
         <xsl:with-param name="total" select="$issue.total.count" />
      </xsl:call-template>
      <xsl:call-template name="fxcop.detail">
         <xsl:with-param name="name">Messages</xsl:with-param>
         <xsl:with-param name="count" select="$issue.information.count" />
         <xsl:with-param name="total" select="$issue.total.count" />
      </xsl:call-template>
   </xsl:template>

   <!-- Coverage detail row in main grid displaying a name, statistics and graph bar -->
   <xsl:template name="fxcop.detail">
      <xsl:param name="name" />
      <xsl:param name="count" />
      <xsl:param name="total" />

      <xsl:variable name="percent" select="$count div $total * 100"/>

      <tr>
         <td><xsl:value-of select="$name" /></td>
         <td><xsl:value-of select="$count" /></td>
         <td class="percent"><xsl:value-of select="concat(format-number($percent,'#0.0'), ' %')" /></td>
         <td>
            <xsl:call-template name="fxcop.detail.percent">
               <xsl:with-param name="total" select="$total" />
               <xsl:with-param name="count" select="$count" />
               <xsl:with-param name="scale" select="200" />
               <xsl:with-param name="threshold" select="50" />
            </xsl:call-template>
         </td>
      </tr>
   </xsl:template>

   <!-- Draw % Green/Red/Yellow Bar -->
   <xsl:template name="fxcop.detail.percent">
      <xsl:param name="total" />
      <xsl:param name="count" />
      <xsl:param name="scale" />
      <xsl:param name="threshold"/>

      <xsl:variable name="coverage" select="$count div $total * 100"/>

      <table cellpadding="0" cellspacing="0">
         <tbody>
            <tr>
               <xsl:if test="not ($count=0)">
                  <td height="14">
                     <xsl:attribute name="class">
                        <xsl:choose>
                           <xsl:when test="$coverage = 0">graphBarVisited</xsl:when>
                           <xsl:when test="$coverage &gt; $threshold">graphBarNotVisited</xsl:when>
                           <xsl:otherwise>graphBarSatisfactory</xsl:otherwise>
                        </xsl:choose>
                     </xsl:attribute>
                     <xsl:attribute name="width">
                        <xsl:value-of select="format-number($count div $total * $scale, '0')" />
                     </xsl:attribute>&#160;
                  </td>
               </xsl:if>
               <td height="14" class="graphBarVisited">
                  <xsl:attribute name="width">
                     <xsl:value-of select="format-number(((1 - $count div $total) ) * $scale, '0')" />
                  </xsl:attribute>&#160;
               </td>
            </tr>
         </tbody>
      </table>
   </xsl:template>
</xsl:stylesheet>
