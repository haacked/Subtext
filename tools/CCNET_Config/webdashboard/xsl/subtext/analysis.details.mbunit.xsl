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
            <xsl:value-of select="counter/@skip-count" /> skipped,                    
            <xsl:value-of select="counter/@ignore-count" /> ignored,
         </span>
         <div class="containerContents">
            <xsl:choose>
               <xsl:when test="counter/@run-count = 0 and (counter/@success-count = 0 and counter/@failure-count = 0 and counter/@ignore-count = 0 and counter/@skip-count = 0)">
                  <p>No tests run.</p>
                  <p>This project doesn't have any tests.</p>
               </xsl:when>
               <xsl:otherwise>
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
                  </xsl:for-each>
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
      <table class="section" rules="groups" border="0" cellpadding="0" cellspacing="0">
         <thead>
         <tr class="header2"><th><xsl:value-of select="parent::namespace/@name" /></th></tr>
         </thead>
         <tbody>
         <tr><td><xsl:apply-templates select="fixture" /></td></tr>
         </tbody>
      </table>
   </xsl:template>
   
   <xsl:template match="fixture">
      <xsl:variable name="count-failure" select="count(runs/descendant::run[@result = 'failure'])"/>
      <xsl:variable name="count-skip" select="count(runs/descendant::run[@result = 'skip'])"/>
      <xsl:variable name="count-ignore" select="count(runs/descendant::run[@result = 'ignore'])"/>
      
      <table width="100%" border="0" cellpadding="2" cellspacing="0">
         <thead>
            <tr>
               <xsl:if test="position() mod 2=0">
                  <xsl:attribute name="class">shaded</xsl:attribute>
               </xsl:if>
               <xsl:attribute name="id"><xsl:value-of select="@type" />.<xsl:value-of select="@name" /></xsl:attribute>
               <td style="white-space:normal" width="375px"><strong><xsl:value-of select="@name"/></strong></td>
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
         </thead>

         <xsl:if test="$count-failure &gt; 0 or $count-skip &gt; 0 or $count-ignore &gt; 0">
         <tbody>
            <tr>
               <xsl:if test="position() mod 2=0">
                  <xsl:attribute name="class">shaded</xsl:attribute>
               </xsl:if>
               <td colspan="3">
                  <div style="overflow:scroll;width:600px">
                     <xsl:if test="$count-failure &gt; 0">
                        <div style="margin-left:12px; margin-right:6px"><strong>Failed Tests</strong>
                           <ol style="margin-top:0px;">
                              <xsl:apply-templates select="runs/descendant::run[@result = 'failure']">
                              </xsl:apply-templates>
                           </ol>
                        </div>
                     </xsl:if>
                     <xsl:if test="$count-skip &gt; 0">
                        <div style="margin-left:12px; margin-right:6px"><strong>Skipped Tests</strong>
                           <ol style="margin-top:0px;">
                              <xsl:apply-templates select="runs/descendant::run[@result = 'skip']">
                              </xsl:apply-templates>
                           </ol>
                        </div>
                     </xsl:if>
                     <xsl:if test="$count-ignore &gt; 0">
                        <div style="margin-left:12px; margin-right:6px"><strong>Ignored Tests</strong>
                           <ol style="margin-top:0px;">
                              <xsl:apply-templates select="runs/descendant::run[@result = 'ignore']">
                              </xsl:apply-templates>
                           </ol>
                        </div>
                     </xsl:if>
                  </div>
               </td>
            </tr>        
         </tbody>
         </xsl:if>
      </table>
   </xsl:template>
      
   <xsl:template match="runs">
      <xsl:apply-templates select="*" />
   </xsl:template>

   <xsl:template match="run">
      <xsl:variable name="p"><xsl:value-of select="parent::fixture/@name"/>.</xsl:variable>
      <xsl:if test="@result != 'success'">
         <li>
            <xsl:value-of select="substring-after(@name, $p)"/>
            <xsl:if test="@result = 'failure'">
               <xsl:call-template name="exception-log"/>
            </xsl:if>
            <xsl:call-template name="console-output">
               <xsl:with-param name="shaded" select="position() mod 2=0"/>
            </xsl:call-template> 
         </li>
      </xsl:if>
   </xsl:template>

   <xsl:template name="exception-log">
      <xsl:apply-templates select="exception" />
   </xsl:template>
   
   <xsl:template match="exception">
      <p style="margin-top:0px;">
      <strong>Exception:</strong>&#160;<xsl:value-of select="@type"/>
      <br/>
      <strong>Message:</strong>&#160;<xsl:value-of select="message"/>
      <br/>
      <strong>Source:</strong>&#160;<xsl:value-of select="source"/>
      <br/>
      <xsl:for-each select="properties/property">
         <strong><xsl:value-of select="@name"/>:</strong>&#160;<xsl:value-of select="@value"/>
         <br/>
      </xsl:for-each>            
      <strong>Stack Trace:</strong>
      <br/>
      <xsl:call-template name="br-replace">
         <xsl:with-param name="word"><xsl:value-of select="stack-trace"/></xsl:with-param>
      </xsl:call-template>
      </p>
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
         <br/>
         <strong><xsl:value-of select="$name"/>:</strong>
         <br/>
         <p style="margin-top:0px;">
         <xsl:call-template name="br-replace">
            <xsl:with-param name="word"><xsl:value-of select="text()"/></xsl:with-param>
         </xsl:call-template>
         </p>
      </xsl:if>
   </xsl:template>
</xsl:stylesheet>
