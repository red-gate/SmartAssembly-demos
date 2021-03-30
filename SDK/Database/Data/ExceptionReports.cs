using System.Collections.Generic;
using System.Data.Common;

namespace SmartAssembly.DatabaseSample.Data
{
    public class ExceptionReports
    {
        private readonly Database m_Database;

        public ExceptionReports(Database database)
        {
            m_Database = database;
        }

        public ExceptionReport[] GetAllReports()
        {
            var IDs = new List<ExceptionReport>();

            using (
                DbDataReader reader =
                    m_Database.ExecuteReader("SELECT ID FROM ExceptionReports ORDER BY CreationDate DESC")) 
            {
                while (reader.Read())
                {
                    IDs.Add(new ExceptionReport(m_Database, reader.GetGuid(0).ToString("B")));
                }
            }

            return IDs.ToArray();
        }
    }
}