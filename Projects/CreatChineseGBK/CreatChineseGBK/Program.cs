using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CreatChineseGBK
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] first = { "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G" };
            string[] second = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };
            string patch = @"F:\Temp\GBK.txt";
            FileStream fileStream = new FileStream(patch, FileMode.Create);
            StreamWriter streamWriter = new StreamWriter(fileStream);
            int index = 0;
            for (int i = 0; i < first.Length; i++)
            {
                string starta = first[i];
                string enda = "";
                string endb = "";

                if (i + 1 < first.Length)
                {
                    for (int x = 0; x < second.Length; x++)
                    {
                        string startb = "";
                        startb = first[i];
                        enda = second[x];
                        if (x + 1 < second.Length)
                        {
                            endb = second[x + 1];
                        }
                        else
                        {
                            startb = first[i + 1];
                            endb = second[0];
                        }

                        streamWriter.WriteLine("class GBK" + index.ToString());
                        streamWriter.WriteLine("{");
                        streamWriter.WriteLine("start = 0x" + starta + enda + "00;");
                        streamWriter.WriteLine("end = 0x" + startb + endb + "00;");
                        streamWriter.WriteLine("};");
                        index++;
                    }
                }
                else
                {
                    break;
                }
            }
            streamWriter.Close();
            fileStream.Close();
        }
    }
}
