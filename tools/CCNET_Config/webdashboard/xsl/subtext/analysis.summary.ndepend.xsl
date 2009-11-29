<?xml version="1.0" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/TR/xhtml1/strict">
   
   <xsl:output method="html" />
   
   <xsl:template match="/">
      <style type="text/css">
      <![CDATA[
        .title { background-color:#006; color:#FFF; }
        .subtitle { color: #883333; font-size: 10pt; font-weight: bold; background-color: #CCCCCC }
        .subtitleref { color: blue; font-size: 10pt }
        .info {color: black; font-size: 10pt}
        .biginfo {color: black; font-size: 10pt ; font-weight: bold}
        .infobold {color: black; font-size: 10pt ; font-weight: bold}
        .hdrcell_left  { color: #FFFFFF ; font-weight: bold; background-color: #B3B3B3; text-align: left;}
        .hdrcell_leftb  { color: #FFFFFF ; font-weight: bold; background-color: #939393; text-align: left;}
        .hdrcell_right { color: #FFFFFF ; font-weight: bold; background-color: #B3B3B3; text-align: right;}
        .hdrcell_rightb { color: #FFFFFF ; font-weight: bold; background-color: #939393; text-align: right;}
        .datacell_left0 { color: #000055; background-color: #DBDBDB; text-align: left; }
        .datacell_leftb0{ color: #000055; background-color: #BBBBBB; text-align: left; }
        .datacell_right0{ color: #000055; background-color: #DBDBDB; text-align: right; }
        .datacell_rightb0{ color: #000055; background-color: #BBBBBB; text-align: right; }
        .datacell_red0 { color: #000055; background-color: #FFBBBB; text-align: right; }
        .datacell_left1 { color: #000055; background-color: #EAEAEA; text-align: left; }
        .datacell_leftb1 { color: #000055; background-color: #CACACA; text-align: left; }
        .datacell_right1{ color: #000055; background-color: #EAEAEA; text-align: right; }
        .datacell_rightb1{ color: #000055; background-color: #CACACA; text-align: right; }
        .datacell_red1 { color: #000055; background-color: #FFCCCC; text-align: right; }
      ]]>
      </style>
      
      <div id="NDepend-report">
         <xsl:apply-templates select="//NDepend" />
      </div>
      
   </xsl:template>
   
   <xsl:template match="NDepend">
      <div class="title"><xsl:value-of select="ReportInfo/@AppName" /> Report</div>
      <p></p>
      <div class="biginfo">To get explanations concerning these metrics, go to the <A HREF="http://smacchia.chez.tiscali.fr/NDepend.html" target="_blank">NDepend home page.</A></div>
      <p></p>
      <p></p>
      <div class="subtitle">Application Metrics</div>
      <p></p>
      <xsl:apply-templates select="ApplicationMetrics" />
      <p></p>
      <p></p>
      <div class="subtitle">Assemblies Metrics</div>
      <p></p>
      <xsl:apply-templates select="AssembliesMetrics" />
      <p></p>
      <p></p>
      <div class="subtitle">Assemblies Dependencies</div>
      <p></p>
      <xsl:apply-templates select="AssemblyDependencies" />
   </xsl:template>
   
   <xsl:template match="ApplicationMetrics">
      <div class="info">Number of assemblies: <font color="#FF0000"><xsl:value-of select="@NAsm" /></font></div>
      <div class="info">Number of types: <font color="#FF0000"><xsl:value-of select="@NType" /></font></div>
      <div class="info">Number of classes: <font color="#FF0000"><xsl:value-of select="@NClass" /></font></div>
      <div class="info">Number of abstract classes: <font color="#FF0000"><xsl:value-of select="@NAbstractClass" /></font></div>
      <div class="info">Number of interfaces: <font color="#FF0000"><xsl:value-of select="@NInterface" /></font></div>
      <div class="info">Number of value types: <font color="#FF0000"><xsl:value-of select="@NValueType" /></font></div>
      <div class="info">Number of exception classes: <font color="#FF0000"><xsl:value-of select="@NExceptionType" /></font></div>
      <div class="info">Number of attribute classes: <font color="#FF0000"><xsl:value-of select="@NAttributeType" /></font></div>
      <div class="info">Number of delegate classes: <font color="#FF0000"><xsl:value-of select="@NDelegateType" /></font></div>
      <div class="info">Number of enumerations: <font color="#FF0000"><xsl:value-of select="@NEnumType" /></font></div>
      <div class="info">Number of IL instructions: <font color="#FF0000"><xsl:value-of select="@NILInstruction" /></font></div>
      <div class="info">Percentage of public types: <font color="#FF0000"><xsl:value-of select="@PercentPublicType" /></font>%</div>
      <div class="info">Percentage of public methods: <font color="#FF0000"><xsl:value-of select="@PercentPublicMethod" /></font>%</div>
      <div class="info">Percentage of classes with at least one public field: <font color="#FF0000"><xsl:value-of select="@PercentClassWithAtLeastOnePublicField" /></font>%</div>
   </xsl:template>
   
   <xsl:template match="AssembliesMetrics">
      <table border="1" cellpadding="3" cellspacing="0" bordercolor="white">
         <tr>
            <td class="hdrcell_left">Assembly</td>
            <td class="hdrcell_right"># Types</td>
            <td class="hdrcell_right"># Abstract Types</td>
            <td class="hdrcell_right"># IL Inst</td>
            <td class="hdrcell_right"><A HREF="http://smacchia.chez.tiscali.fr/NDepend.html#AsmCa" target="_blank">Afferent Coupling</A></td>
            <td class="hdrcell_right"><A HREF="http://smacchia.chez.tiscali.fr/NDepend.html#AsmCe" target="_blank">Efferent Coupling</A></td>
            <td class="hdrcell_right"><A HREF="http://smacchia.chez.tiscali.fr/NDepend.html#RC" target="_blank">Relational Cohesion</A></td>
            <td class="hdrcell_right"><A HREF="http://smacchia.chez.tiscali.fr/NDepend.html#Inst" target="_blank">Instability</A></td>
            <td class="hdrcell_right"><A HREF="http://smacchia.chez.tiscali.fr/NDepend.html#Abst" target="_blank">Abstractness</A></td>
            <td class="hdrcell_right"><A HREF="http://smacchia.chez.tiscali.fr/NDepend.html#Dist" target="_blank">Distance</A></td>
         </tr>
         <xsl:apply-templates select="Assembly" />
      </table>
      <p></p>
   </xsl:template>
   
   <xsl:template match="Assembly">
      <tr>
         <td class="datacell_left{position() mod 2}"><xsl:value-of select="@Assembly" /></td>
         <td class="datacell_right{position() mod 2}"><xsl:value-of select="@NTypes" /></td>
         <td class="datacell_right{position() mod 2}"><xsl:value-of select="@NAbstractTypes" /></td>
         <td class="datacell_right{position() mod 2}"><xsl:value-of select="@NILInstructionInAsm" /></td>
         <td class="datacell_right{position() mod 2}"><xsl:value-of select="@AfferentCoupling" /></td>
         <td class="datacell_right{position() mod 2}"><xsl:value-of select="@EfferentCoupling" /></td>
         <td class="datacell_right{position() mod 2}"><xsl:value-of select="@RelationalCohesion" /></td>
         <td class="datacell_right{position() mod 2}"><xsl:value-of select="@Instability" /></td>
         <td class="datacell_right{position() mod 2}"><xsl:value-of select="@Abstractness" /></td>
         <td class="datacell_right{position() mod 2}"><xsl:value-of select="@NormDistFrMainSeq" /></td>
      </tr>
   </xsl:template>
   
   <xsl:template match="AssemblyDependencies">
      <table border="1" cellpadding="3" cellspacing="0" bordercolor="white">
         <tr>
            <td class="hdrcell_left">Assembly</td>
            <td class="hdrcell_left">Depends on...</td>
            <td class="hdrcell_left">Is referenced by...</td>
         </tr>
         <xsl:apply-templates select="Dependencies_For" />
      </table>
      <p></p>
   </xsl:template>
   
   <xsl:template match="Dependencies_For">
      <tr>
         <td class="datacell_left{position() mod 2}">
            <xsl:element name="a">
               <xsl:attribute name="Name"><xsl:value-of select="@Assembly" /></xsl:attribute>
               <xsl:value-of select="@Assembly" />
            </xsl:element>
         </td>
         <td class="datacell_left{position() mod 2}">
            <xsl:choose>
               <xsl:when test="DependsOn"><xsl:apply-templates select="DependsOn" /></xsl:when>
               <xsl:otherwise> - </xsl:otherwise>
            </xsl:choose>
         </td>
         <td class="datacell_left{position() mod 2}">
            <xsl:choose>
               <xsl:when test="ReferencedBy"><xsl:apply-templates select="ReferencedBy" /></xsl:when>
               <xsl:otherwise> - </xsl:otherwise>
            </xsl:choose>
         </td>
      </tr>
   </xsl:template>
   
   <xsl:template match="DependsOn">
      <xsl:for-each select="DependsOn_Name">
         <xsl:element name="a">
            <xsl:attribute name="href">#<xsl:apply-templates /></xsl:attribute>
            <xsl:apply-templates />
         </xsl:element> ; 
      </xsl:for-each>
   </xsl:template>
   
   <xsl:template match="ReferencedBy">
      <xsl:for-each select="ReferencedBy_Name">
         <xsl:element name="a">
            <xsl:attribute name="href">#<xsl:apply-templates /></xsl:attribute>
            <xsl:apply-templates />
         </xsl:element> ; 
    </xsl:for-each>
   </xsl:template>
   
</xsl:stylesheet>