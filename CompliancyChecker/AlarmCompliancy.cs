using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Net;

namespace EncoderCompliancy
{
    class AlarmCompliancy
    {
        public AlarmCompliancy(string ipAddress, string[] lines, XmlDocument xmlConfig)
        {
            int i = 1;
            while (i < lines.Length)
            {
                string line = lines[i];
                string[] lineArray = line.Split(',');
                string name = lineArray[0].Trim();
                string severity = lineArray[5].Trim();
                string alarmID = lineArray[2].Trim();
                string xpathSeverity = ("//alarm/alarmId[@value='" + alarmID + "']/following-sibling::severityLevel/@value");
                int resultCount = xmlConfig.SelectNodes(xpathSeverity).Count;
                if (resultCount == 1)
                {
                    string result = xmlConfig.SelectSingleNode(xpathSeverity).Value.Trim();
                    if (result != severity)
                    {
                        name = name + "_" + alarmID;
                        string[] output = { ipAddress, "Chassis", name, severity, result };
                        new Output.WriteToCsv(output);
                    }
                }
                else if (resultCount < 1)
                {
                    string[] output = { ipAddress, "Chassis", name + "_" + alarmID + "_" + "XPath Error", "1 result", "No result" };
                    new Output.WriteToCsv(output);
                }
                else
                {
                    string[] output = { ipAddress, "Chassis", name + "_" + alarmID + "_" + "XPath Error", "1 result", "More than 1 result" };
                    new Output.WriteToCsv(output);
                }
                i++;
            }
        }
    }
}
