<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html"/>
  <xsl:template match="/report">
    <html>
      <head>
        <style>
          .bold { font-weight: bold; }
          th { background-color: silver; font-weight: bold; }
          td, th { border: solid 1 darkgray; }
          tr.passed { background-color: springgreen; }
          tr.notpassed { background-color: tomato; }
        </style>
      </head>
      <body>
        <h1><xsl:value-of select="@title"/></h1>
        <table cellspacing="0">
          <tr>
            <th>Test Case</th>
            <th>Mode</th>
            <th>Error</th>
          </tr>
          <xsl:for-each select="item">
            <xsl:variable name="class">
              <xsl:choose>
                <xsl:when test="@passed = '1'">
                  <xsl:value-of select="'passed'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="'notpassed'"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            <tr class="{$class}">
              <td><xsl:value-of select="@name"/></td>
              <td class="bold"><xsl:value-of select="@mode"/></td>
              <td><pre><xsl:value-of select="."/></pre></td>
            </tr>
          </xsl:for-each>
        </table>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>