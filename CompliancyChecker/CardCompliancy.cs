using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using CompliancyTool;

namespace EncoderCompliancy
{
    class CardCompliancy
    {
        String xpath = String.Empty;
        String value = String.Empty;
        String resolution = String.Empty;
        String audioTwo = String.Empty;
        String audioThree = String.Empty;
        String serviceName = String.Empty;
        String secondVideo = String.Empty;
        String downConvert = String.Empty;

        public CardCompliancy(string ipAddress, XmlDocument xmlConfig)
        {
            string config1080i = (Program.ncPath + "1080i.csv");
            string config720p = (Program.ncPath + "720p.csv");
            string config480i = (Program.ncPath + "480i.csv");
            string config1080iDV = (Program.ncPath + "1080i+DV.csv");
            string config720pDV = (Program.ncPath + "720p+DV.csv");
            string config480iDV = (Program.ncPath + "480i+DV.csv");
            string config1080iDC = (Program.ncPath + "1080iDC.csv");
            string config720pDC = (Program.ncPath + "720pDC.csv");
            string config1080iDCDV = (Program.ncPath + "1080iDC+DV.csv");
            string config720pDCDV = (Program.ncPath + "720pDC+DV.csv");
            int[] slots = { 3, 4, 5, 6 };
            string[] configLines;

            foreach (int intSlot in slots)
            {
                string slot = intSlot.ToString();
                xpath = "//slot[@displayName='Slot " + slot + "']/card/videoGroup/video/videoInput/videoStandard/@value";
                resolution = xmlConfig.SelectSingleNode(xpath).Value.Trim();
                xpath = "//slot[@displayName='Slot " + slot + "']/card/audioGroup/audio[@displayName='Audio 2']/audioInput/source/@value";
                audioTwo = xmlConfig.SelectSingleNode(xpath).Value.Trim();
                xpath = "//slot[@displayName='Slot " + slot + "']/card/audioGroup/audio[@displayName='Audio 3']/audioInput/source/@value";
                audioThree = xmlConfig.SelectSingleNode(xpath).Value.Trim();
                xpath = "//slot[@displayName='Slot " + slot + "']/card/videoGroup/video/videoEngineGroup/videoEngine[@displayName='Second Video']/encode/profile/@value";
                try
                {
                    secondVideo = xmlConfig.SelectSingleNode(xpath).Value.Trim();
                    if (secondVideo != "Off")
                    {
                        downConvert = "Yes";
                    }
                    else
                    {
                        downConvert = "No";
                    }
                }
                catch (Exception)
                {
                    downConvert = "No";
                }
                xpath = "//output[@displayName='IP Output: Data Port 3 - Data Port 4']/*//stream[@reference='/viper[1]/slotList[1]/slot[" + slot + "]/card[1]/videoGroup[1]/video[1]/videoEngineGroup[1]/videoEngine[1]/outputStreamList[1]/outputStream[1]']/../../../../../serviceList/service/sdtParams/serviceName/@value";
                try
                {
                    serviceName = xmlConfig.SelectSingleNode(xpath).Value.Trim();
                }
                catch (Exception)
                {
                    serviceName = "None";
                }
                Console.WriteLine("Slot: {0} Video Standard: {1} Down Convert: {2} Service Name: {3}", slot, resolution, downConvert, serviceName);
                if (serviceName == "None")
                {
                    string[] output = { ipAddress, "None" };
                    new Output.WriteToCsv(output);
                }
                else if (resolution != "1080i29.97" && resolution != "720p59.94" && resolution != "480i29.97")
                {
                    string[] output = { ipAddress, "Invalid resolution" };
                    new Output.WriteToCsv(output);
                }
                else
                {
                    if (resolution == "1080i29.97")
                    {
                        if (downConvert == "No")
                        {
                            if (audioTwo != "Off")
                            {
                                Console.WriteLine("config1080iDV");
                                configLines = File.ReadAllLines(config1080iDV, Encoding.UTF8);
                            }
                            else
                            {
                                Console.WriteLine("config1080i");
                                configLines = File.ReadAllLines(config1080i, Encoding.UTF8);
                            }
                        }
                        else
                        {
                            if (audioThree != "Off")
                            {
                                Console.WriteLine("config1080iDCDV");
                                configLines = File.ReadAllLines(config1080iDCDV, Encoding.UTF8);
                            }
                            else
                            {
                                Console.WriteLine("config1080iDC");
                                configLines = File.ReadAllLines(config1080iDC, Encoding.UTF8);
                            }
                        }
                    }
                    else if (resolution == "720p59.94")
                    {
                        if (downConvert == "No")
                        {
                            if (audioTwo != "Off")
                            {
                                Console.WriteLine("config720pDV");
                                configLines = File.ReadAllLines(config720pDV, Encoding.UTF8);
                            }
                            else
                            {
                                Console.WriteLine("config720p");
                                configLines = File.ReadAllLines(config720p, Encoding.UTF8);
                            }
                        }
                        else
                        {
                            if (audioThree != "Off")
                            {
                                Console.WriteLine("config720pDCDV");
                                configLines = File.ReadAllLines(config720pDCDV, Encoding.UTF8);
                            }
                            else
                            {
                                Console.WriteLine("config720pDC");
                                configLines = File.ReadAllLines(config720pDC, Encoding.UTF8);
                            }
                        }
                    }
                    else
                    {
                        if (audioTwo != "Off")
                        {
                            Console.WriteLine("config480iDV");
                            configLines = File.ReadAllLines(config480iDV, Encoding.UTF8);
                        }
                        else
                        {
                            Console.WriteLine("config480i");
                            configLines = File.ReadAllLines(config480i, Encoding.UTF8);
                        }
                    }
                    string[] newConfigLines = configLines.Select(x => x.Replace("slotNumberPlaceholder", slot)).ToArray();
                    new ConfigurationChecker(ipAddress, serviceName, newConfigLines, xmlConfig);
                }
            }
        }
    }
}
