using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Net;

namespace EncoderCompliancy
{
    class LicenseCompliancy
    {
        public LicenseCompliancy(string ipAddress, string[] lines, XmlDocument xmlConfig)
        {
            int i = 1;
            while (i < lines.Length)
            {
                string line = lines[i];
                string[] lineArray = line.Split(',');
                string featureId = lineArray[0].Trim();
                string code = lineArray[1].Trim();
                string available = lineArray[2].Trim();
                string xpathAvailable = ("//licenseList/group/license/code[@value='" + code + "']/following-sibling::available/@value");
                int resultCount = xmlConfig.SelectNodes(xpathAvailable).Count;
                if (resultCount == 1)
                {
                    string result = xmlConfig.SelectSingleNode(xpathAvailable).Value.Trim();
                    if (result != available)
                    {
                        string[] output = { ipAddress, "Chassis", code, available, result };
                        new Output.WriteToCsv(output);
                    }
                }
                else if (resultCount < 1)
                {
                    string[] output = { ipAddress, "Chassis", code + "_" + "XPath Error", "1 result", "No result" };
                    new Output.WriteToCsv(output);
                }
                else
                {
                    string[] output = { ipAddress, "Chassis", code + "_" + "XPath Error", "1 result", "More than 1 result" };
                    new Output.WriteToCsv(output);
                }
                i++;
            }
        }
    }
}
