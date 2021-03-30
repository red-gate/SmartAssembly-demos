using System;
using System.Data.Common;
using System.Diagnostics;

namespace SmartAssembly.DatabaseSample.Data
{
    public struct FeatureUsageInformation
    {
        private int m_FeatureID;
        private long m_UsageCount;

        public int FeatureID
        {
            get
            {
                return m_FeatureID;
            }
        }
        public long UsageCount
        {
            get
            {
                return m_UsageCount;
            }
        }

        public FeatureUsageInformation(DbDataReader reader)
        {
            m_FeatureID = -1;
            m_UsageCount = -1;

            for (int i = 0; i < reader.FieldCount; i++)
            {
                object fieldValue = reader.GetValue(i);
                if (fieldValue == DBNull.Value) continue;

                switch (reader.GetName(i))
                {
                    case "FeatureID":
                        m_FeatureID = Convert.ToInt32(fieldValue);
                        break;
                    case "CountSum":
                        m_UsageCount = Convert.ToInt64(fieldValue);
                        break;

                    default:
                        throw new ApplicationException("Unrecognized column " + reader.GetName(i) + " in row");
                }
            }
        }
    }
}
