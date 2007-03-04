<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
    <xsl:output method="html"/>

    <xsl:template match="/">
      <xsl:variable name="buildresults" select="/cruisecontrol/build/buildresults" />
      <div class="container">
         <span class="containerTitle">NAnt Build Log</span>
         <span class="containerSubtitle">
            <xsl:if test="count($buildresults) > 0">
               <xsl:choose>
                  <xsl:when test="$buildresults/failure">
                     Build Failed (<a href="#failure">click to see error</a>)
                  </xsl:when>
                  <xsl:otherwise>
                     Build Succeeded
                  </xsl:otherwise>
               </xsl:choose>
            </xsl:if>
         </span>
         <div class="containerContents">
            <xsl:choose>
               <xsl:when test="count($buildresults) > 0">
                  <xsl:apply-templates select="$buildresults" />
               </xsl:when>
               <xsl:otherwise>
                  <h2>Log does not contain any Xml output from NAnt.</h2>
                  <p>Please make sure that NAnt is executed using the XmlLogger (use the argument: <b>-logger:NAnt.Core.XmlLogger</b>).</p>
               </xsl:otherwise>
            </xsl:choose>
         </div>
      </div>
   </xsl:template>
   
   <xsl:template match="buildresults">
      <div>
         <xsl:apply-templates />
         <a href="#top">Back to top</a>
      </div>
   </xsl:template>
   
   <xsl:template match="target">
      <span class="buildtarget">
         <p><strong><xsl:value-of select="@name"/></strong>:
         <ul>
            <xsl:apply-templates />
         </ul>
         </p>
      </span>
   </xsl:template>
   
   <xsl:template match="task">
      <xsl:if test="@name = 'echo' and count(child::message) = 0"><em>[<xsl:value-of select="@name"/>]</em><br /></xsl:if>
      <xsl:apply-templates /> 
   </xsl:template>
   
   <xsl:template match="message">
      <li>
      <span>
         <xsl:attribute name="class">
            <xsl:choose>
               <xsl:when test="@level='Error'">error</xsl:when>
               <xsl:when test="@level='Warning'">warning</xsl:when>
            </xsl:choose>
         </xsl:attribute>
         <xsl:if test="../@name != ''"><em>[<xsl:value-of select="../@name"/>]</em>&#160;</xsl:if>
         <xsl:value-of select="text()" />
      </span>
      </li>
   </xsl:template>
   
   <xsl:template match="failure">
      <a name="failure">
         <xsl:apply-templates />
      </a>
   </xsl:template>
   
   <xsl:template match="builderror">
      <br/>
      <span class="error">Build Error: <xsl:value-of select="type"/><br/>
      <xsl:value-of select="message"/><br/>     
      <xsl:apply-templates select="location" />
      <pre>
         <xsl:value-of select="stacktrace" />
      </pre>
      </span>     
   </xsl:template>
   
   <xsl:template match="internalerror">
      <br/>
      <span class="error">Internal Error: <xsl:value-of select="type"/><br/>
         <xsl:value-of select="message"/><br/>     
         <xsl:apply-templates select="location" />
         <pre>
            <xsl:value-of select="stacktrace" />
         </pre>
      </span>     
   </xsl:template>
   
   <xsl:template match="location">
      in <xsl:value-of select="filename"/>
      line: <xsl:value-of select="linenumber"/>
      col: <xsl:value-of select="columnnumber"/><br/>
   </xsl:template>   
   
   <xsl:template match="duration" />
</xsl:stylesheet>
