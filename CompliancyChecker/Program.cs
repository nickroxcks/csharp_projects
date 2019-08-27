using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Net;
using EncoderCompliancy;

namespace CompliancyTool
{
    class Program
    {
        // global var
        public static string todaysDate = new GetDateTime.GetDateTime().TodaysDate();
        public static string ncPath = (@"..\EncoderCompliancyFiles\");
        public static string outputFileName = (@"..\EncoderCompliancyOutput\EncoderCompliancy_" + todaysDate + ".csv");
        public static string errorFileName = (@"..\EncoderCompliancyOutput\EncoderCompliancyErrorFile_" + todaysDate + ".txt");
        //
        static void Main(string[] args)
        {
            List<string> ipAddresses = new List<string>();
            Console.WriteLine("Enter IP Address:");
            string inputIpAddress = Console.ReadLine();
            if (inputIpAddress == "")
            {
                ipAddresses = new GetAddresses().GetAddressArray("AVP.csv");
            }
            else
            {
                ipAddresses.Add(inputIpAddress);
            }
            string[] headers = { "IP Address", "Service Name", "Feature Name", "Expected", "Actual" };
            new Output.WriteToCsv(headers);
            string baseConfig = (Program.ncPath + "Base.csv");
            string[] configLines = File.ReadAllLines(baseConfig, Encoding.UTF8);
            string alarmsReference = (Program.ncPath + "Alarm.csv");
            string[] alarmLines = File.ReadAllLines(alarmsReference, Encoding.UTF8);
            string licenseReference = (Program.ncPath + "License.csv");
            string[] licenseLines = File.ReadAllLines(licenseReference, Encoding.UTF8);
            foreach (string ipAddress in ipAddresses)
            {
                string xmlUrl = (@"http://" + ipAddress + @"/tcf?cgi=dcp&method=get&config=names");
                Console.WriteLine(ipAddress);
                XmlDocument xmlConfig = new HttpGet.HttpGet().HttpGetRequest(xmlUrl);
            //XmlReader reader = XmlReader.Create(ncPath + "AVP05.xml"); //Remove this line when removing comments from above line.
            //XmlDocument xmlConfig = new XmlDocument(); //Remove this line when removing comments from above line.
            //xmlConfig.Load(Program.ncPath + "AVP05.xml"); //Remove this line when removing comments from above line.
                if (xmlConfig != null)
                {
                    new ConfigurationChecker(ipAddress, "Chassis", configLines, xmlConfig);
                    new CardCompliancy(ipAddress, xmlConfig);
                    new AlarmCompliancy(ipAddress, alarmLines, xmlConfig);
                    new LicenseCompliancy(ipAddress, licenseLines, xmlConfig);
                }
                else
                {
                    Console.WriteLine("Error");
                }
            }
            //Console.WriteLine("Press Enter to quit");
            //Console.Read();
        }
    }
}
