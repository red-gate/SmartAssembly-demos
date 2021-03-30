using SmartAssembly.DatabaseSample.Data;
using System;
using System.Data.Common;

namespace SmartAssembly.Data
{
	public struct BuildInformation
	{
		private string assemblyID;
		private string projectID;
        private DateTime buildDate;
		private DateTime lastAccessDate;
        private Version buildVersion;

		public string AssemblyID
		{
			get
			{
				return assemblyID;
			}
		}

		public string ProjectID
		{
			get
			{
				return projectID;
			}
		}

	    public DateTime BuildDate
	    {
	        get
	        {
                return buildDate;
	        }
	    }

		public DateTime LastAccessDate
		{
			get
			{
				return lastAccessDate;
			}
		}

	    public Version BuildVersion
	    {
	        get
	        {
                return buildVersion;
	        }
	    }

		public BuildInformation(DbDataReader reader)
		{
			assemblyID = string.Empty;
			projectID = string.Empty;
            buildDate = DateTime.Now;
			lastAccessDate = DateTime.Now;
            buildVersion = new Version();

			for (int i=0; i<reader.FieldCount; i++)
			{
				object fieldValue = reader.GetValue(i);				
				if (fieldValue == DBNull.Value) continue;

				switch (reader.GetName(i))
				{
					case "AssemblyID":
						assemblyID = fieldValue.ParseGuid().ToString("B");
						break;

					case "ProjectID":
						projectID = fieldValue.ParseGuid().ToString("B");
						break;

                    case "BuildDate":
                        buildDate = (DateTime)fieldValue;
                        break;

					case "LastAccessDate":
						lastAccessDate = (DateTime)fieldValue;
						break;

                    case "BuildVersion":
                        buildVersion = new Version((string)fieldValue);
                        break;

					case "Map":
						break;
				}
			}
		}
	}
}
