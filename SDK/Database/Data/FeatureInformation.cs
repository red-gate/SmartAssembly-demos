using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace SmartAssembly.DatabaseSample.Data
{
    class FeatureInformation
    {
        private string m_FeatureName;
        private int m_FeatureID;

        public string FeatureName
        {
            get { return m_FeatureName; }
        }
        public int FeatureID
        {
            get { return m_FeatureID; }
        }

        public FeatureInformation(DbDataReader reader)
        {
            m_FeatureName = string.Empty;
            m_FeatureID = -1;

            for (int i = 0; i < reader.FieldCount; i++)
            {
                object fieldValue = reader.GetValue(i);
                if (fieldValue == DBNull.Value) continue;

                switch (reader.GetName(i))
                {
                    case "Name":
                        m_FeatureName = (string)fieldValue;
                        break;

                    case "ID":
                        m_FeatureID = (int) fieldValue;
                        break;

                    default:
                        throw new ApplicationException("Unrecognized column " + reader.GetName(i) + " in row");
                }
            }

        }
    }
}