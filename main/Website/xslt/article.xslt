﻿<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <xsl:output method="html" indent="yes"/>

  <xsl:param name="id"/>
  <xsl:param name="classId"/>

  <xsl:template match="root">
    <div id="{$id}" class="{$classId}">
      <xsl:value-of select="data/text()"/>
    </div>
  </xsl:template>
</xsl:stylesheet>
