using System;
using System.Collections.Generic;
using System.Data.Common;
using SmartAssembly.SDK;

namespace SmartAssembly.DatabaseSample.Data
{
    public class FeatureReports
    {
        private readonly Database m_Database;

        public FeatureReports(Database database)
        {
            m_Database = database;
        }

        public string GetFeatureNameFromFeatureID(int featureID)
        {
            string featureName = null;

            DbDataReader reader = m_Database.ExecuteReader("SELECT Name FROM Features WHERE ID=@1", featureID);
            while (reader.Read())
            {
                featureName = (string) reader.GetValue(0);
            }
            reader.Close();

            return featureName;
        }

        public List<FeatureUsageInformation> GetAllFeatureUsages()
        {
            List<FeatureUsageInformation> featureReports = new List<FeatureUsageInformation>();

            DbDataReader reader = m_Database.ExecuteReader("SELECT FeatureID, SUM(UsageCount) AS CountSum FROM FeatureReports GROUP BY FeatureID ORDER BY SUM(UsageCount) DESC");
            while (reader.Read())
            {
                featureReports.Add(new FeatureUsageInformation(reader));
            }
            reader.Close();

            return featureReports;
        }
    }
}
