<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/TR/xhtml1/strict">
   <xsl:include href="analysis.summary.ncoverexplorer.xsl"/>
   <xsl:include href="analysis.summary.mbunit.xsl"/>
   <xsl:include href="analysis.summary.fxcop.xsl"/>
   <xsl:include href="analysis.summary.ndepend.xsl"/>

   <xsl:output method="html"/>

   <xsl:template match="/">
      <div class="container">
         <span class="containerSubtitle">Code Analysis and Testing</span>
         <div class="containerContents">
            <xsl:apply-templates select="//coverageReport" />
            <xsl:apply-templates select="//report-result" />
            <xsl:apply-templates select="//FxCopReport" />
         </div>
      </div>
   </xsl:template>
</xsl:stylesheet>
