<?xml version='1.0'?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:strip-space elements="*" />

	<xsl:template match="/">
		<xsl:apply-templates select="child::SalesOrderRequest"/>
	</xsl:template>

	<xsl:template match="SalesOrderRequest">
		<xsl:apply-templates select="child::SalesOrder"/>
	</xsl:template>
	
	<xsl:template match="SalesOrder">
		<order>
			<orderRef>
				<xsl:value-of select="stockCode" />
			</orderRef>
			<orderDate>
				<xsl:value-of select="orderDate" />
			</orderDate>
			<currency>
				<xsl:value-of select="currency" />
			</currency>
			<shipDate>
				<xsl:value-of select="shipDate" />
			</shipDate>
			<categoryCode>
				<xsl:value-of select="categoryCode" />
			</categoryCode>
		</order>
	</xsl:template>

</xsl:stylesheet>