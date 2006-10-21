<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/TR/xhtml1/strict">
<xsl:output method="html"/>
	<xsl:template match="/">
		<xsl:apply-templates select="//coverageReport" />					
	</xsl:template>
	
	<xsl:template match="coverageReport">
        <table class="section-table" cellpadding="2" cellspacing="0" border="0" width="98%">
            <tr>
                <td class="sectionheader" colspan="3">
                   NCoverExplorer Code Coverage Summary:
                </td>
            </tr>
            <tr>
				<td>
					Total coverage: <xsl:value-of select="round(./project/@coverage)"/>%
				</td>
 				<td>
					Unvisited points: <xsl:value-of select="./project/@unvisitedPoints"/>
				</td>
				<td>
					Files: <xsl:value-of select="./project/@files"/>
				</td>
            </tr>
            <tr class="section-oddrow">
				<td>
					Acceptance threshold: <xsl:value-of select="./project/@acceptable"/>%
				</td>
				<td>
					Total points: <xsl:value-of select="./project/@sequencePoints"/>
				</td>
 				<td>
					Classes: <xsl:value-of select="./project/@classes"/>
				</td>
           </tr>
            <tr>
                <td>
                   Coverage verdict:
                   <xsl:if test="./project/@coverage &lt; ./project/@acceptable">
					  <span style="color:red">FAIL</span>
                   </xsl:if>
                   <xsl:if test="./project/@coverage &gt;= ./project/@acceptable">
					  <span style="color:green">PASS</span>
                   </xsl:if>
                </td>
				<td>
					Non Comment Lines: <xsl:value-of select="./project/@nonCommentLines"/>
				</td>
 				<td>
					Members: <xsl:value-of select="./project/@members"/>
				</td>
            </tr>
		</table>
	</xsl:template>
</xsl:stylesheet>
