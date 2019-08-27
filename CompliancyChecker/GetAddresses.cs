using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace EncoderCompliancy
{
    class GetAddresses
    {
        public List<string> GetAddressArray(string fileName)
        {
            string filePath = @"..\IPAdresses\";
            string fullPath = filePath + fileName;
            List<string> addresses = new List<string>();
            try
            {
                string[] lines = File.ReadAllLines(fullPath,Encoding.UTF8);
                foreach (string line in lines)
                {
                    string[] lineArray = line.Split(',');
                    string address = lineArray[1] + "." + lineArray[2] + "." + lineArray[3] + "." + lineArray[4];
                    addresses.Add(address);
                }
                addresses.Remove(addresses[0]);
                foreach (string address in addresses)
                {
                    Console.WriteLine(address);
                }
                return addresses;
            }
            catch(Exception error)
            {
                Console.WriteLine("Error getting addresses from source file: "+fullPath);
                File.AppendAllText(Program.errorFileName + Program.todaysDate + ".txt",(error.ToString() + "\r\n\r\n"));
                List<string> exception = new List<string>();
                exception.Add("Exception");
                return exception;
            }
        }
            
    }
}