<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
   <xsl:output method="html"/>

   <xsl:template match="/">
      <xsl:variable name="messages" select="/cruisecontrol//buildresults//message" />
      
      <xsl:if test="count($messages) > 0">      
         <xsl:variable name="error.messages" select="$messages[(contains(text(), 'error ')) or @level='Error'] | /cruisecontrol//builderror/message | /cruisecontrol//internalerror/message" />
         <xsl:variable name="error.messages.count" select="count($error.messages)" />
         <xsl:variable name="warning.messages" select="$messages[(contains(text(), 'warning ')) or @level='Warning']" />
         <xsl:variable name="warning.messages.count" select="count($warning.messages)" />
         <xsl:variable name="total" select="count($error.messages) + count($warning.messages)"/>

         <div class="container">
            <span class="containerSubtitle">Compilation Errors and Warnings</span>
            <div class="containerContents">
               <xsl:if test="$error.messages.count = 0 and $warning.messages.count = 0">
                  <p>There were no errors or warnings encountered during compilation.</p>
               </xsl:if>

               <xsl:if test="$error.messages.count > 0">
                  <div class="buildcontainer">
                     <span class="containerTitle">Errors: <xsl:value-of select="$error.messages.count"/></span>
                     <div class="containerContents">
                        <table class="section" rules="groups" cellpadding="2" cellspacing="0" border="0">
                           <xsl:apply-templates select="$error.messages" />
                        </table>
                     </div>
                  </div>
               </xsl:if>
               
               <xsl:if test="$warning.messages.count > 0">
                  <div class="buildcontainer">
                     <span class="containerTitle">Warnings: <xsl:value-of select="$warning.messages.count"/></span>
                     <div class="containerContents">
                        <table class="section" rules="groups" cellpadding="2" cellspacing="0" border="0">
                           <xsl:apply-templates select="$warning.messages" />
                        </table>
                     </div>
                  </div>
               </xsl:if>
            </div>
         </div>
      </xsl:if>
   </xsl:template>

   <xsl:template match="message">
      <tbody>
         <tr class="error">
            <xsl:if test="position() mod 2=0">
               <xsl:attribute name="class">shaded</xsl:attribute>
            </xsl:if>

            <td><xsl:value-of select="text()"/></td>
         </tr>
      </tbody>
   </xsl:template>
</xsl:stylesheet>



