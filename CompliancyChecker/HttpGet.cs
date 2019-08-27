using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace HttpGet
{
    class HttpGet
    {
        public XmlDocument HttpGetRequest(string ipAddress)
        {
            try
            {
                Console.WriteLine("Trying http...");
                // Create a request for the URL. 		
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ipAddress);
                request.Headers.Add("Authorization: Basic cm9vdDp2aXBlcg==");
                // If required by the server,set the credentials.
                //request.Credentials = CredentialCache.DefaultCredentials;
                // Get the response.
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                // Display the status.
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                XmlDocument xmlConfig = new XmlDocument();
                xmlConfig.Load(dataStream);
                dataStream.Close();
                response.Close();
                Console.WriteLine("http Response");
                return xmlConfig;
            }
            catch (Exception error)
            {
                Console.WriteLine("http Request Failed");
                File.AppendAllText(EncoderCompliancy.Program.errorFileName,(ipAddress + "\r\n"));
                File.AppendAllText(EncoderCompliancy.Program.errorFileName,(error.ToString() + "\r\n\r\n"));
                return null;
            }
        }
    }
}
