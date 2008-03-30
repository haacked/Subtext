<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
    <xsl:output method="html"/>
    <xsl:param name="applicationPath"/>
    <xsl:variable name="modification.list" select="/cruisecontrol/modifications/modification"/>
    <xsl:key name="changeset" match="/cruisecontrol/modifications/modification" use="changeNumber/text()"/>

   <xsl:template match="/">
      <div class="container">
         <span class="containerSubtitle">Source Control Revision History</span>
         <div class="containerContents">
            <xsl:if test="count($modification.list) &gt; 0">
               <xsl:for-each select="/cruisecontrol/modifications/modification[generate-id(.)=generate-id(key('changeset', changeNumber/text())[1])]">
                  <xsl:sort select="changeNumber" order="descending" data-type="number"/>
                  <xsl:call-template name="changeset" />
               </xsl:for-each>
            </xsl:if>
            <xsl:if test="count($modification.list) = 0">There were no changes made since the last build. </xsl:if>
         </div>
      </div>
   </xsl:template>

   <!-- Changeset template -->
   <xsl:template name="changeset">
      <div class="buildcontainer">
         <span class="containerTitle">Changeset # 
            <a>
               <xsl:attribute name="href">
                  http://svn.sourceforge.net/subtext/?rev=<xsl:value-of select="changeNumber" />&amp;view=rev
               </xsl:attribute>
               <xsl:value-of select="changeNumber" />
            </a>
         </span>
         <div class="containerContents">
            <table class="section" rules="groups" cellpadding="2" cellspacing="0" border="0">
               <tbody>
                  <tr class="header2">
                     <th>Author: <xsl:value-of select="user"/></th>
                     <th>Date: <xsl:value-of select="date"/></th>
                  </tr>
                  <tr>
                     <td colspan="2"><em><xsl:value-of select="comment"/></em></td>
                  </tr>
                  <tr class="header2">
                     <th colspan="2">Changes</th>
                  </tr>
                  <xsl:for-each select="key('changeset', changeNumber/text())">
                     <xsl:call-template name="modification"/>
                  </xsl:for-each>
               </tbody>
            </table>
         </div>
      </div>
   </xsl:template>

   <!-- Modifications template -->
   <xsl:template name="modification">
      <tr>
         <xsl:if test="position() mod 2=0">
            <xsl:attribute name="class">shaded</xsl:attribute>
         </xsl:if>
         <td>
            <img class="statusimage">
               <xsl:attribute name="title"><xsl:value-of select="@type"/></xsl:attribute>
               <xsl:attribute name="src">
                  <xsl:choose>
                     <xsl:when test="@type = 'Added'"><xsl:value-of select="concat($applicationPath, '/images/add.png')"/></xsl:when>
                     <xsl:when test="@type = 'Modified'"><xsl:value-of select="concat($applicationPath, '/images/edit.png')"/></xsl:when>
                     <xsl:when test="@type = 'Removed'">{$applicationPath}/images/delete.png</xsl:when>
                     <xsl:otherwise>{$applicationPath}/images/document_text.png</xsl:otherwise>
                  </xsl:choose>
               </xsl:attribute>
            </img>&#160;<xsl:value-of select="@type"/>
         </td>
         <td>
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
                  <!-- Build the appropriate viewvc link -->
                  <a>
                     <xsl:attribute name="href">
                        http://subtext.svn.sourceforge.net/viewvc/subtext<xsl:value-of select="project" />/<xsl:value-of select="filename" />?view=markup&amp;pathrev=<xsl:value-of select="changeNumber" />
                     </xsl:attribute>
                     <xsl:if test="project != ''"><xsl:value-of select="project"/>/</xsl:if>
                     <xsl:value-of select="filename"/>
                  </a>
               </xsl:otherwise>
            </xsl:choose>
         </td>
      </tr>
   </xsl:template>
</xsl:stylesheet>
