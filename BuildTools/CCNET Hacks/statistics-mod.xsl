<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="html"/>
	<xsl:template match="/statistics">
	<style>
		*.pass{
			background-color: #33ff99;
		}
		*.fail{
			background-color: #ff6600;
		}
		*.unknown{
			background-color: #ffffcc;
		}
		table.stats td {
			text-align:center;
		}

	</style>
		<p>
			Today is
			<xsl:variable name="day" select="//timestamp/@day"/>
			<xsl:variable name="month" select="//timestamp/@month"/>
			<xsl:variable name="year" select="//timestamp/@year"/>
			<xsl:value-of select="$day"/>/<xsl:value-of select="$month"/>/<xsl:value-of select="$year"/> <br />
			<xsl:variable name="totalCount" select="count(integration)"/>
			<xsl:variable name="successCount" select="count(integration[@status='Success'])"/>
			<xsl:variable name="failureCount" select="$totalCount - $successCount"/>
			<xsl:variable name="totalCountForTheDay" select="count(integration[@day=$day and @month=$month and @year=$year])"/>
			<xsl:variable name="successCountForTheDay" select="count(integration[@status='Success' and @day=$day and @month=$month and @year=$year])"/>
			<xsl:variable name="failureCountForTheDay" select="$totalCountForTheDay - $successCountForTheDay"/>
			<table border="1" cellpadding="0" cellspacing="0">
				<tr>
					<th>Integration Summary</th>
					<th>For today</th>
					<th>Overall</th>
				</tr>
				<tr>
					<th align="left">Total Builds</th>
					<td><xsl:value-of select="$totalCountForTheDay"/></td>
					<td><xsl:value-of select="$totalCount"/></td>
				</tr>
				<tr>
					<th align="left">Number of Successful</th>
					<td><xsl:value-of select="$successCountForTheDay"/></td>
					<td><xsl:value-of select="$successCount"/></td>
				</tr>
				<tr>
					<th align="left">Number of Failed</th>
					<td><xsl:value-of select="$failureCountForTheDay"/></td>
					<td><xsl:value-of select="$failureCount"/></td>
				</tr>
			</table>
		</p>
		<p><pre><strong>Note: </strong>Only builds run with the statistics publisher enabled will appear on this page!</pre></p>
		<table class="stats">
			<tr>
				<th>Build Label</th>
				<th>Status</th>
				<th>StartTime</th>
				<th>Duration</th>
				<th>TestCount</th>
				<th>TestFailures</th>
				<th>TestIgnored</th>
				<th>FxCop Errors</th>
				<th>FxCop Warnings</th>
				<th>% Coverage</th>
				<th>Code Lines</th>
			</tr>
			<xsl:for-each select="./integration">
				<xsl:sort select="position()" data-type="number" order="descending"/>
				<xsl:variable name="colorClass">
					<xsl:choose>
						<xsl:when test="./@status = 'Success'">pass</xsl:when>
						<xsl:when test="./@status = 'Unknown'" >unknown</xsl:when>
						<xsl:otherwise>fail</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<tr>
					<th>
						<xsl:value-of select="./@build-label"/>
					</th>
					<th class="{$colorClass}">
						<xsl:value-of select="./@status"/>
					</th>
						<td>
							<xsl:value-of select='./statistic[@name="StartTime"]'/>
						</td>
						<td>
							<xsl:value-of select='./statistic[@name="Duration"]'/>
						</td>
						<td>
							<xsl:value-of select='./statistic[@name="TestCountMb"]'/>
						</td>
						<td>
							<xsl:value-of select='./statistic[@name="TestFailuresMb"]'/>
						</td>
						<td>
							<xsl:value-of select='./statistic[@name="TestIgnoredMb"]'/>
						</td>
						<td>
							<xsl:value-of select='./statistic[@name="FxCop Errors"]'/>
						</td>
						<td>
							<xsl:value-of select='./statistic[@name="FxCop Warnings"]'/>
						</td>
						<td>
							<xsl:value-of select='./statistic[@name="Coverage"]'/>
						</td>
						<td>
							<xsl:value-of select='./statistic[@name="Code Lines"]'/>
						</td>
				</tr>
			</xsl:for-each>
		</table>
	</xsl:template>
</xsl:stylesheet>