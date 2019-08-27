using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace EncoderCompliancy
{
    class ConfigurationChecker
    {
        public ConfigurationChecker(string ipAddress, string serviceName, string[] lines,XmlDocument xmlConfig)
        {
            int i = 1;
            while (i < lines.Length)
            {
                string line = lines[i];
                bool boolResult = line.StartsWith("!");
                if (boolResult == false)
                {
                    string[] lineArray = line.Split(',');
                    string name = lineArray[0].Trim();
                    string value = lineArray[1].Trim();
                    string xpath = lineArray[2].Trim();
                    int resultCount = xmlConfig.SelectNodes(xpath).Count;
                    if (resultCount == 1)
                    {
                        string result = xmlConfig.SelectSingleNode(xpath).Value.Trim();
                        if (result != value)
                        {
                            string[] output = { ipAddress, serviceName, name, value, result };
                            new Output.WriteToCsv(output);
                        }
                    }
                    else if (resultCount < 1)
                    {
                        string[] output = { ipAddress, serviceName, name + " - XPath Error!", "1 result", "No result" };
                        new Output.WriteToCsv(output);
                    }
                    else
                    {
                        string[] output = { ipAddress, serviceName, name + " - XPath Error!", "1 result", "More than 1 result" };
                        new Output.WriteToCsv(output);
                    }
                }
                i++;
            }
        }
    }
}
