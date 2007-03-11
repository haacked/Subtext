<?xml version="1.0" encoding="iso-8859-1"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">    
   <xsl:template name="br-replace">
      <xsl:param name="word"/>
      <xsl:choose>
         <xsl:when test="contains($word,'&#xA;')">
            <xsl:value-of select="substring-before($word,'&#xA;')"/>
            <br/>
            <xsl:call-template name="br-replace">
               <xsl:with-param name="word" select="substring-after($word,'&#xA;')"/>
            </xsl:call-template>
         </xsl:when>
         <xsl:otherwise>
            <xsl:value-of select="$word"/>
         </xsl:otherwise>
      </xsl:choose>
   </xsl:template>

   <xsl:template name="scale">
      <xsl:param name="origLength"/>
      <xsl:param name="targetLength"/>
      <xsl:param name="value"/>
      <xsl:value-of select="($value div $origLength) * $targetLength"/>
   </xsl:template>

   <xsl:template name="counter-progressbar">
      <xsl:param name="width"/>
      <xsl:param name="height"/>
      <div>
         <xsl:attribute name="style">position:relative;background-color:#DDDDDD;border-color:black;width:<xsl:value-of select="$width"/>px;height:<xsl:value-of select="$height"/>px;</xsl:attribute>
         <!-- success -->
         <xsl:if test="@success-count > 0">
         <div>
            <xsl:attribute name="style">position:absolute;top:0px;left:0px;height:<xsl:value-of select="$height"/>px;background-color:lightgreen;font-size:1px;width:<xsl:call-template name="scale">
                  <xsl:with-param name="origLength" select="@run-count"/>
                  <xsl:with-param name="targetLength" select="$width"/>
                  <xsl:with-param name="value" select="@success-count"/>
               </xsl:call-template>px;</xsl:attribute>
         </div>
         </xsl:if>
         <!-- failure -->
         <xsl:if test="@failure-count > 0">
         <div>
            <xsl:attribute name="style">position:absolute;top:0px;left:<xsl:call-template name="scale">
                  <xsl:with-param name="origLength" select="@run-count"/>
                  <xsl:with-param name="targetLength" select="$width"/>
                  <xsl:with-param name="value" select="@success-count"/>
               </xsl:call-template>px;height:<xsl:value-of select="$height"/>px;background-color:red;font-size:1px;width:<xsl:call-template name="scale">
                  <xsl:with-param name="origLength" select="@run-count"/>
                  <xsl:with-param name="targetLength" select="$width"/>
                  <xsl:with-param name="value" select="@failure-count"/>
               </xsl:call-template>px;</xsl:attribute>
         </div>
         </xsl:if>
         <!-- skip -->
         <xsl:if test="@skip-count > 0">
         <div>
            <xsl:attribute name="style">position:absolute;top:0px;left:<xsl:call-template name="scale">
                  <xsl:with-param name="origLength" select="@run-count"/>
                  <xsl:with-param name="targetLength" select="$width"/>
                  <xsl:with-param name="value" select="@success-count+@failure-count"/>
               </xsl:call-template>px;height:<xsl:value-of select="$height"/>px;background-color:blueviolet;font-size:1px;width:<xsl:call-template name="scale">
                  <xsl:with-param name="origLength" select="@run-count"/>
                  <xsl:with-param name="targetLength" select="$width"/>
                  <xsl:with-param name="value" select="@skip-count"/>
               </xsl:call-template>px;</xsl:attribute>
         </div>
         </xsl:if>
         <!-- ignore -->
         <xsl:if test="@ignore-count > 0">
         <div>
            <xsl:attribute name="style">position:absolute;top:0px;left:<xsl:call-template name="scale">
                  <xsl:with-param name="origLength" select="@run-count"/>
                  <xsl:with-param name="targetLength" select="$width"/>
                  <xsl:with-param name="value" select="@success-count+@failure-count+@skip-count"/>
               </xsl:call-template>px;height:<xsl:value-of select="$height"/>px;background-color:orange;font-size:1px;width:<xsl:call-template name="scale">
                  <xsl:with-param name="origLength" select="@run-count"/>
                  <xsl:with-param name="targetLength" select="$width"/>
                  <xsl:with-param name="value" select="@ignore-count"/>
               </xsl:call-template>px;</xsl:attribute>
         </div>
         </xsl:if>
         <div>
            <xsl:attribute name="style">position:absolute;top:0px;left:<xsl:value-of select="$width +2"/>px;height:<xsl:value-of select="$height"/>px;font-size:<xsl:value-of select="$height - 2"/>px;font-family:Verdana;</xsl:attribute><xsl:value-of select="@run-count"/>/<xsl:value-of select="@success-count"/>/<xsl:value-of select="@failure-count"/>/<xsl:value-of select="@skip-count"/>/<xsl:value-of select="@ignore-count"/>/<xsl:value-of select="@assert-count"/></div>
      </div>
   </xsl:template>

   <xsl:template name="counter-literal">
      <xsl:value-of select="@run-count"/> tests,
      <xsl:value-of select="@success-count"/> success,
      <xsl:value-of select="@failure-count"/> failures,
      <xsl:value-of select="@skip-count" /> skipped,
      <xsl:value-of select="@ignore-count"/> ignored,
      <xsl:value-of select="@assert-count"/> asserts
   </xsl:template>

   <xsl:template name="assemblyid">
      <xsl:param name="name"/>
      as<xsl:value-of select="$name"/>
   </xsl:template>

   <xsl:template name="exceptionid">
      <xsl:param name="name"/>
      ex<xsl:value-of select="$name"/>
   </xsl:template>

    <!--
        MbUnit main method.
    -->
    <xsl:template match="/">
      <xsl:apply-templates select="//report-result" />
    </xsl:template>
    
   <xsl:template match="report-result">
      <div class="container">
         <span class="containerTitle">Unit Tests</span>
         <span class="containerSubtitle">Tests run:
            <xsl:value-of select="counter/@run-count" /> tests,
            <xsl:value-of select="counter/@success-count" /> success,
            <xsl:value-of select="counter/@failure-count" /> failure,
            <xsl:value-of select="counter/@ignore-count" /> ignored,
            <xsl:value-of select="counter/@skip-count" /> skipped,                    
            <xsl:value-of select="format-number(counter/@duration,'##.##')" /> s         
         </span>
         <div class="containerContents">
            <xsl:choose>
               <xsl:when test="counter/@run-count = 0 and (counter/@success-count = 0 and counter/@failure-count = 0 and counter/@ignore-count = 0 and counter/@skip-count = 0)">
                  <p>No tests run.</p>
                  <p>This project doesn't have any tests.</p>
               </xsl:when>
               <xsl:otherwise>
                  <!-- <xsl:call-template name="report-summary" /> -->
                  <xsl:if test="count(//warnings)>0">
                     <xsl:call-template name="warnings" />
                  </xsl:if> 
                  <xsl:call-template name="assemblies" />
               </xsl:otherwise>
            </xsl:choose>
         </div>
      </div>
   </xsl:template>

   <xsl:template name="warnings">
      <div class="buildcontainer">
         <span class="containerTitle">Warnings</span>
         <div class="containerContents">
            <table class="section" rules="groups" cellpadding="2" cellspacing="0" border="0">
               <xsl:for-each select="//warning">
                  <tr>
                     <xsl:if test="position() mod 2=0">
                        <xsl:attribute name="class">shaded</xsl:attribute>
                     </xsl:if>
                     <td> <xsl:value-of select="ancestor::run/@name" /></td>
                     <td><xsl:value-of select="text()"/></td>
                  </tr>
               </xsl:for-each>
            </table>
         </div>
      </div>
   </xsl:template>

   <xsl:template name="fixture-id">
      <xsl:param name="name" /><xsl:value-of select="$name" />
   </xsl:template>
    
   <xsl:template name="assemblies">
      <xsl:for-each select="assemblies/assembly">
         <xsl:call-template name="assembly-detail" />
      </xsl:for-each>
   </xsl:template>

   <xsl:template name="assembly-detail">
      <div class="buildcontainer">
         <xsl:attribute name="id"><xsl:value-of select="@name"/>Assembly</xsl:attribute>
         <span class="containerTitle"><xsl:value-of select="@name"/></span>
         <span class="containerSubtitle">
                  <xsl:for-each select="counter">
                     <xsl:call-template name="counter-literal"/>
                  </xsl:for-each>, <xsl:value-of select="format-number(counter/@duration,'0.00')"/>s
          </span>
         <div class="containerContents">
            <xsl:if test="set-up|tear-down">
               <table border="0" cellpadding="0" cellspacing="0">
               <tr class="assemblysetupteardown" ><td>Assembly SetUp and TearDown</td></tr>
               <xsl:apply-templates select="set-up"/>
               <xsl:apply-templates select="tear-down"/>
               </table>
            </xsl:if>
            <xsl:call-template name="namespaces-table" />
         </div>
      </div>
   </xsl:template>
   
   <xsl:template name="namespaces-table">
      <xsl:apply-templates select="namespaces">
         <xsl:sort select="@name"/>
      </xsl:apply-templates>
      <xsl:apply-templates select="namespace">
         <xsl:sort select="@name"/>
      </xsl:apply-templates>
   </xsl:template>
   
   <xsl:template match="namespaces">
      <xsl:apply-templates select="namespace">
         <xsl:sort select="@name"/>
      </xsl:apply-templates>
   </xsl:template>
   
   <xsl:template match="namespace">
      <xsl:apply-templates select="namespaces">
         <xsl:sort select="@name"/>
      </xsl:apply-templates>
      <xsl:apply-templates select="fixtures" />
   </xsl:template>
   
   <xsl:template match="fixtures">
      <table class="section" rules="groups" border="1" cellpadding="0" cellspacing="0">
         <thead>
         <tr class="header2"><th><xsl:value-of select="parent::namespace/@name" /></th></tr>
         </thead>
         <tbody>
         <tr><td><xsl:apply-templates select="fixture" /></td></tr>
         </tbody>
      </table>
   </xsl:template>
   
   <xsl:template match="fixture">
      <table width="96%" border="0" cellpadding="2" cellspacing="0">
         <thead>
            <tr>
               <xsl:attribute name="id"><xsl:value-of select="@type" />.<xsl:value-of select="@name" /></xsl:attribute>
               <td><strong><xsl:value-of select="@name"/></strong></td>
               <td width="60px"><strong><xsl:value-of select="format-number(counter/@duration,'0.00')"/>s</strong></td>
               <td width="250px"><strong>
                  <xsl:for-each select="counter">
                     <xsl:call-template name="counter-progressbar">
                        <xsl:with-param name="width" select="100" />
                        <xsl:with-param name="height" select="10" />
                     </xsl:call-template>
                  </xsl:for-each></strong>
               </td>
            </tr>
         </thead>

         <xsl:if test="set-up|tear-down">
            <tbody>
               <tr><th colspan="3">Fixture SetUp and TearDown</th></tr>
               <xsl:apply-templates select="set-up"/>
               <xsl:apply-templates select="tear-down"/>
            </tbody>
         </xsl:if>

         <tbody>
            <tr>
               <td colspan="3">
                  <xsl:apply-templates select="runs" />
               </td>
            </tr>
         </tbody>
      </table>
   </xsl:template>
   
   <xsl:template match="runs">
      <table width="96%" rules="groups" border="0" cellpadding="0" cellspacing="0">
         <xsl:apply-templates select="*" />
      </table>
   </xsl:template>

   <xsl:template match="run">
      <xsl:variable name="p"><xsl:value-of select="parent::fixture/@name"/>.</xsl:variable>
      
      <tr>
         <xsl:if test="position() mod 2=0">
            <xsl:attribute name="class">shaded</xsl:attribute>
         </xsl:if>
         <td colspan="3"><strong><xsl:value-of select="substring-after(@name, $p)"/></strong></td>
      </tr>
      <tr>
         <xsl:if test="position() mod 2=0">
            <xsl:attribute name="class">shaded</xsl:attribute>
         </xsl:if>
         <td><xsl:value-of select="format-number(@duration * 1000,'0.000')"/>ms</td>
         <td><xsl:value-of select="format-number(@memory * 0.0001,'0.00')"/>Kb</td>
         <td><xsl:value-of select="@assert-count" /></td>
      </tr>
      
      <xsl:call-template name="console-output">
         <xsl:with-param name="shaded" select="position() mod 2=0"/>
      </xsl:call-template>      
   </xsl:template>

   <xsl:template match="set-up">
      <xsl:call-template name="set-up-or-tear-down" />
   </xsl:template>

   <xsl:template match="tear-down">
      <xsl:call-template name="set-up-or-tear-down" />
   </xsl:template>

   <xsl:template name="set-up-or-tear-down">
      <tr>
         <td> <xsl:value-of select="@name"/> </td>
         <td> <xsl:value-of select="format-number(@duration * 1000,'0.000')"/>ms </td>
         <td> <xsl:value-of select="format-number(@memory * 0.0001,'0.00')"/>Kb </td>
      </tr>

      <!-- Adding execption log -->
      <xsl:if test="@result = 'failure'">
         <xsl:call-template name="exception-log"/>
      </xsl:if>

      <!-- Adding console out, error -->
      <xsl:call-template name="console-output" />
   </xsl:template>

   <xsl:template name="exception-log">
      <tr class="failure-exception">
         <td colspan="3">
            <table width="96%" border="0" cellpadding="1" cellspacing="1">
               <xsl:apply-templates select="exception" />
            </table>
         </td>
      </tr>
   </xsl:template>
   
   <xsl:template match="exception">
      <tr class="exceptionType">
         <td><strong>Type:</strong></td>
         <td><xsl:value-of select="@type"/></td>
      </tr>
      <tr>
         <td><strong>Message:</strong></td>
         <td><xsl:value-of select="message"/></td>
      </tr>
      <tr>
         <td><strong>Source:</strong></td>
         <td><xsl:value-of select="source"/></td>
      </tr>
      <xsl:for-each select="properties/property">
         <tr>
            <td><strong><xsl:value-of select="@name"/>:</strong></td>
            <td><xsl:value-of select="@value"/></td>
         </tr>
      </xsl:for-each>            
      <tr>
         <td colspan="2"><strong>StackTrace:</strong>
            <br/>
            <xsl:call-template name="br-replace">
               <xsl:with-param name="word"><xsl:value-of select="stack-trace"/></xsl:with-param>
            </xsl:call-template>
         </td>
      </tr>
      <xsl:apply-templates select="exception" />
   </xsl:template>
   
   <xsl:template name="console-output">
      <xsl:param name="shaded" />

      <xsl:apply-templates select="console-out">
         <xsl:with-param name="shaded" select="$shaded"/>
      </xsl:apply-templates>
      
      <xsl:apply-templates select="console-error">
         <xsl:with-param name="shaded" select="$shaded"/>
      </xsl:apply-templates>
   </xsl:template>
   
   <xsl:template match="console-out">
      <xsl:param name="shaded" />

      <xsl:call-template name="console">
         <xsl:with-param name="name">Console Output</xsl:with-param>
         <xsl:with-param name="shaded" select="$shaded"/>
      </xsl:call-template>
   </xsl:template>
   
   <xsl:template match="console-error">
      <xsl:param name="shaded" />

      <xsl:call-template name="console">
         <xsl:with-param name="name">Console Error</xsl:with-param>
         <xsl:with-param name="shaded" select="$shaded"/>
      </xsl:call-template>
   </xsl:template>
   
   <xsl:template name="console">
      <xsl:param name="shaded" />
      <xsl:param name="name" />
      
      <xsl:if test="string-length( text() ) != 0">
         <tr>
            <xsl:if test="$shaded">
               <xsl:attribute name="class">shaded</xsl:attribute>
            </xsl:if>
            <td colspan="3">
               <strong><xsl:value-of select="$name"/>:</strong>
               <br/>
               <xsl:call-template name="br-replace">
                  <xsl:with-param name="word"><xsl:value-of select="text()"/></xsl:with-param>
               </xsl:call-template>
            </td>
         </tr>
      </xsl:if>
   </xsl:template>
</xsl:stylesheet>
