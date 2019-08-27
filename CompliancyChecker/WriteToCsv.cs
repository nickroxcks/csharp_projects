using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Output
{
    class WriteToCsv
    {
        public WriteToCsv(string[] args)
        {
            string delimeter = ",";
            string outputLine = null;
            int i = 0;
            foreach (string arg in args)
            {
                outputLine = (outputLine + arg + delimeter);
                i++;
            }
            outputLine = (outputLine.TrimEnd(',') + "\r\n");
            File.AppendAllText(EncoderCompliancy.Program.outputFileName,outputLine);
        }
    }
}
