using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Xml;

namespace SmartAssembly.DatabaseSample.Data
{
    public class ExceptionReport
    {
        private readonly Database m_Database;
        private readonly Guid m_ReportId;

        public bool TryGetReportInformation(ref ExceptionReportInformation exceptionReportInformation)
        {

            using (DbDataReader reader = m_Database.ExecuteReader("SELECT * FROM ExceptionReports WHERE ID=@1", m_ReportId))
            {
                if (reader != null && reader.Read())
                {
                    exceptionReportInformation = new ExceptionReportInformation(reader);
                    return true;
                }
            }

            return false;
        }

        public byte[] GetData()
        {
            using (
                DbDataReader reader = m_Database.ExecuteReader("SELECT Data FROM ExceptionReports WHERE ID=@1", m_ReportId))
            {
                if (reader.Read())
                {
                    int length = (int)reader.GetBytes(0, 0, null, 0, 0);
                    byte[] data = new byte[length];
                    reader.GetBytes(0, 0, data, 0, length);
                    return SDK.Helpers.Unzip(data);
                }

                return null;
            }
        }

        public Dictionary<string, string> GetCustomProperties()
        {
            Dictionary<string, string> hashtable = new Dictionary<string, string>();

            using (MemoryStream stream = new MemoryStream(GetData()))
            {
                XmlTextReader reader = new XmlTextReader(stream);

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "CustomProperties")
                    {
                        while (reader.Read())
                        {
                            if (reader.Name == "CustomProperty")
                            {
                                string propertyKey = reader.GetAttribute("Name");
                                string propertyValue = reader.GetAttribute("Value");
                                hashtable.Add(propertyKey, propertyValue);
                            }
                            else
                            {
                                break;
                            }
                        }

                        break;
                    }
                }

                reader.Close();
            }

            return hashtable;
        }

        public Dictionary<string, AttachedFile> GetAttachedFiles()
        {
            Dictionary<string, AttachedFile> hashtable = new Dictionary<string, AttachedFile>();

            using (MemoryStream stream = new MemoryStream(GetData()))
            {
                XmlTextReader reader = new XmlTextReader(stream);

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "AttachedFiles")
                    {
                        while (reader.Read())
                        {
                            if (reader.Name == "AttachedFile")
                            {
                                string propertyKey = reader.GetAttribute("Key");

                                AttachedFile attachedFile = new AttachedFile();
                                attachedFile.Key = propertyKey;
                                attachedFile.FileName = reader.GetAttribute("FileName");

                                byte[] zippedBytes = Convert.FromBase64String(reader.GetAttribute("Data"));
                                attachedFile.Bytes = SDK.Helpers.Unzip(zippedBytes);
                                hashtable.Add(propertyKey, attachedFile);
                            }
                            else
                            {
                                break;
                            }
                        }

                        break;
                    }
                }

                reader.Close();
            }


            return hashtable;
        }

        public ExceptionReport(Database db, string reportId)
        {
            m_Database = db;
            m_ReportId = new Guid(reportId);
        }
    }
}
