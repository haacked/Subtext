<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/TR/xhtml1/strict">
   <xsl:output method="html"/>
   <xsl:variable name="nunit2.result.list" select="//report-result/counter"/>
   <xsl:variable name="nunit2.testcount" select="sum($nunit2.result.list/@run-count)"/>
   <xsl:variable name="nunit2.failure" select="sum($nunit2.result.list/@failure-count)"/>
   <xsl:variable name="nunit2.success" select="sum($nunit2.result.list/@success-count)"/>
   <xsl:variable name="nunit2.ignore" select="sum($nunit2.result.list/@ignore-count)"/>
   <xsl:variable name="nunit2.skip" select="sum($nunit2.result.list/@skip-count)"/>
   <xsl:variable name="nunit2.assert" select="sum($nunit2.result.list/@assert-count)"/>
 
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
                     <thead>
                        <tr><td colspan="2">No tests run.</td></tr>
                        <tr><td colspan="2" class="error">This project doesn't have any tests.</td></tr>
                     </thead>
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

      <thead>
         <tr>
            <th width="375px">
               <xsl:choose>
                  <xsl:when test="$nunit2.failure = 0">All tests passed </xsl:when>
                  <xsl:when test="$nunit2.failure &gt; 0">Some tests passed </xsl:when>
                  <xsl:when test="$nunit2.success = 0">No tests passed </xsl:when>
               </xsl:choose>
               <span>(<xsl:value-of select="$nunit2.testcount" /> / <xsl:value-of select="$nunit2.success" /> / <xsl:value-of select="$nunit2.failure" /> / <xsl:value-of select="$nunit2.skip" /> / <xsl:value-of select="$nunit2.ignore" /> / <xsl:value-of select="$nunit2.assert" />)</span>
            </th>
            <th class="percent"><xsl:value-of select="concat(format-number($percentSuccess,'#0.0'), ' %')" /></th>
            <th>
               <xsl:call-template name="mbunit.detail.percent">
                  <xsl:with-param name="total" select="$nunit2.testcount" />
                  <xsl:with-param name="success" select="$nunit2.success" />
                  <xsl:with-param name="failure" select="$nunit2.failure" />
                  <xsl:with-param name="ignore" select="$nunit2.ignore" />
                  <xsl:with-param name="skip" select="$nunit2.skip" />
                  <xsl:with-param name="assert" select="$nunit2.assert" />
                  <xsl:with-param name="scale" select="200" />
               </xsl:call-template>
            </th>
         </tr>
      </thead>
      <tr>
         <td colspan="3">&#160;</td>
      </tr>
      <tbody>
         <tr class="header2"><th colspan="3">Test Fixtures</th></tr>
         <xsl:apply-templates select="//fixture">
            <xsl:sort select="@type"/>
         </xsl:apply-templates>
      </tbody>
   </xsl:template>

   <xsl:template match="fixture">
      <xsl:variable name="percentSuccess" select="$nunit2.success div $nunit2.testcount * 100"/>

      <tr>
         <xsl:if test="position() mod 2 = 0">
            <xsl:attribute name="class">shaded</xsl:attribute>
         </xsl:if>
         <xsl:attribute name="id"><xsl:value-of select="@type" />.<xsl:value-of select="@name" /></xsl:attribute>
         <td style="white-space:normal"><xsl:value-of select="@type" /></td>
         <td class="percent">
            <xsl:for-each select="counter">
               <xsl:value-of select="concat(format-number(@success-count div @run-count * 100,'#0.0'), ' %')" />
            </xsl:for-each>
         </td>
         <td>
            <xsl:for-each select="counter">
               <xsl:call-template name="mbunit.detail.percent">
                  <xsl:with-param name="total" select="@run-count" />
                  <xsl:with-param name="success" select="@success-count" />
                  <xsl:with-param name="failure" select="@failure-count" />
                  <xsl:with-param name="ignore" select="@ignore-count" />
                  <xsl:with-param name="skip" select="@skip-count" />
                  <xsl:with-param name="assert" select="@assert-count" />
                  <xsl:with-param name="scale" select="200" />
               </xsl:call-template>
            </xsl:for-each>
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
      <xsl:param name="assert" />
      <xsl:param name="scale" />
      
      <xsl:variable name="coverage" select="$success div $total * 100"/>

      <table cellpadding="0" cellspacing="0">
         <tbody>
            <tr>
               <xsl:attribute name="title">
                  <xsl:value-of select="$total" /> / <xsl:value-of select="$success" /> / <xsl:value-of select="$failure" /> / <xsl:value-of select="$skip" /> / <xsl:value-of select="$ignore" /> / <xsl:value-of select="$assert" />
               </xsl:attribute>

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
</xsl:stylesheet>
