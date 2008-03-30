<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
   <xsl:output method="html"/>

   <xsl:template match="/">
      <xsl:variable name="modification.list" select="/cruisecontrol/modifications/modification"/>

      <div class="container">
         <span class="containerTitle">
            <xsl:if test="/cruisecontrol/exception">BUILD EXCEPTION </xsl:if>
            <xsl:if test="/cruisecontrol/build/@error">BUILD FAILED </xsl:if>
            <xsl:if test="not (/cruisecontrol/build/@error) and not (/cruisecontrol/exception)">BUILD SUCCESSFUL </xsl:if>
         </span>
         <span class="containerSubtitle"><xsl:value-of select="/cruisecontrol/build/@date"/></span>
         <div class="containerContents">
           <table class="summary" cellpadding="2" cellspacing="0">
               <xsl:if test="/cruisecontrol/exception">
                  <tr>
                     <th>Error Message:</th>
                     <td class="header-data-error"><xsl:value-of select="/cruisecontrol/exception"/></td>
                  </tr>
               </xsl:if>
               <tr>
                  <th>Project:</th>
                  <td><xsl:value-of select="/cruisecontrol/@project"/></td>
               </tr>
               <tr>
                  <th>Running time:</th>
                  <td><xsl:value-of select="/cruisecontrol/build/@buildtime"/></td>
               </tr>
               <tr>
                  <th>Integration Request:</th>
                  <td><xsl:value-of select="/cruisecontrol/request" /></td>
               </tr>
            </table>
         </div>
      </div>
   </xsl:template>
</xsl:stylesheet>
