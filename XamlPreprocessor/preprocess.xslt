<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:param name="define_constants"/>

	<!-- Helper template to check if a directive exists -->
	<xsl:template name="is-defined">
		<xsl:param name="directive"/>
		<xsl:choose>
			<xsl:when test="contains(concat(';', $define_constants, ';'), concat(';', $directive, ';'))">true</xsl:when>
			<xsl:otherwise>false</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Copy all attributes -->
	<xsl:template match="@*">
		<xsl:copy/>
	</xsl:template>

	<!-- Main template to handle nodes -->
	<xsl:template match="node()">
		<xsl:variable name="current" select="."/>

		<!-- Find the preceding directive start comment -->
		<xsl:variable name="prev-directive" select="preceding-sibling::comment()[
                contains(., ':') and 
                not(starts-with(normalize-space(.), 'end '))
            ][1]"/>

		<xsl:choose>
			<!-- If we're between directive comments -->
			<xsl:when test="$prev-directive">
				<xsl:variable name="directive" select="normalize-space(substring-before($prev-directive, ':'))"/>
				<xsl:variable name="condition" select="normalize-space(substring-after($prev-directive, ':'))"/>

				<!-- Find corresponding end comment -->
				<xsl:variable name="end-comment" select="following-sibling::comment()[
                        contains(., concat('end ', $directive, ':', $condition))
                    ][1]"/>

				<!-- Check if we're between the start and end comments -->
				<xsl:if test="$end-comment">
					<xsl:variable name="is-defined">
						<xsl:call-template name="is-defined">
							<xsl:with-param name="directive" select="$directive"/>
						</xsl:call-template>
					</xsl:variable>

					<!-- Determine if we should keep this content -->
					<xsl:if test="($condition = 'true' and $is-defined = 'true') or
                                 ($condition = 'false' and $is-defined = 'false')">
						<xsl:copy>
							<xsl:apply-templates select="@*|node()"/>
						</xsl:copy>
					</xsl:if>
				</xsl:if>
			</xsl:when>
			<!-- Not in a directive section, copy normally -->
			<xsl:otherwise>
				<xsl:copy>
					<xsl:apply-templates select="@*|node()"/>
				</xsl:copy>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Skip directive comments themselves -->
	<xsl:template match="comment()[contains(., ':')]"/>

</xsl:stylesheet>