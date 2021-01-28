
using Newtonsoft.Json;
using System;
using System.IO;

namespace SendBODToIMS
{
    public class Project
    {
        public string messageID { get; set; }
        public string fromLogicalID { get; set; }
        public string toLogicalID { get; set; }
        public string BOD { get; set; }

        public void Save(string aPath)
        {
            try
            {
                // serialize JSON directly to a file
                using (StreamWriter file = File.CreateText(aPath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, this);
                }
            }
            catch(Exception)
            {

            }
        }

        public string Load(string aPath)
        {
            string result = null;
            using (StreamReader sr = new StreamReader(aPath))
            {
                string fileContents = sr.ReadToEnd();

                if (null != fileContents && fileContents.Length > 0)
                {
                    try
                    {
                        JsonConvert.PopulateObject(fileContents, this);
                    }
                    catch (Exception ex)
                    {
                        result = ex.Message + Environment.NewLine + ex.StackTrace;
                    }
                }
                sr.Close();
            }
            return (result);
        }
    }
}
