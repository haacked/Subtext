<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
    <xsl:output method="html"/>
    <xsl:variable name="modification.list" select="/cruisecontrol/modifications/modification"/>

   <xsl:template match="/">
      <div class="container">
         <span class="containerSubtitle">Source Control Revision History</span>
         <div class="containerContents">
            <xsl:if test="count($modification.list) &gt; 0">
               <div class="buildcontainer">
                  <span class="containerTitle">Modifications: <xsl:value-of select="count($modification.list)"/></span>
                  <div class="containerContents">
                     <table class="section" rules="groups" cellpadding="2" cellspacing="0" border="0">
                        <xsl:apply-templates select="$modification.list">
                           <xsl:sort select="date" order="descending" data-type="text" />
                        </xsl:apply-templates>
                     </table>
                  </div>
               </div>
            </xsl:if>
            <xsl:if test="count($modification.list) = 0">There were no changes made since the last build. </xsl:if>
         </div>
      </div>
   </xsl:template>

   <!-- Modifications template -->
   <xsl:template match="modification">
      <tbody>
         <tr>
            <xsl:if test="position() mod 2=0">
               <xsl:attribute name="class">shaded</xsl:attribute>
            </xsl:if>
            <td colspan="3">
               <xsl:choose>
                  <xsl:when test="count(url) = 1 ">
                     <a>
                        <xsl:attribute name="href">
                           <xsl:value-of select="url" />
                        </xsl:attribute>
                        <xsl:if test="project != ''"><xsl:value-of select="project"/>/</xsl:if>
                        <xsl:value-of select="filename"/>
                     </a>
                  </xsl:when>
                  <xsl:otherwise>
                     <xsl:if test="project != ''"><xsl:value-of select="project"/>/</xsl:if>
                     <xsl:value-of select="filename"/>
                  </xsl:otherwise>
               </xsl:choose>
            </td>
         </tr>
         <tr>
            <xsl:if test="position() mod 2=0">
               <xsl:attribute name="class">shaded</xsl:attribute>
            </xsl:if>
            <td colspan="3"><em><xsl:value-of select="comment"/></em></td>
         </tr>
         <tr>
            <xsl:if test="position() mod 2=0">
               <xsl:attribute name="class">shaded</xsl:attribute>
            </xsl:if>

            <td><xsl:value-of select="date"/></td>
            <td><xsl:value-of select="@type"/></td>
            <td><xsl:value-of select="user"/></td>
         </tr>
      </tbody>
   </xsl:template>
</xsl:stylesheet>
