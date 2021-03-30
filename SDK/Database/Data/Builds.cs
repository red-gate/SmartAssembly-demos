using System.Collections;
using System.Data.Common;
using SmartAssembly.Data;

namespace SmartAssembly.DatabaseSample.Data
{
	public class Builds
	{
		private Database database = null;

		public BuildInformation GetBuildInformation(string assemblyID)
		{
			BuildInformation buildInformation = new BuildInformation();

			DbDataReader reader = database.ExecuteReader("SELECT * FROM Builds WHERE AssemblyID=@1", new System.Guid(assemblyID));
			if (reader.Read()) buildInformation = new BuildInformation(reader);
			reader.Close();

			return buildInformation;
		}

		public string[] GetAllBuilds()
		{
			ArrayList IDs = new ArrayList();
			DbDataReader reader = database.ExecuteReader("SELECT AssemblyID FROM Builds ORDER BY LastAccessDate DESC");

			while(reader.Read())
			{
				IDs.Add(reader.GetGuid(0).ToString("B"));
			}

			reader.Close();
			return (string[]) IDs.ToArray(typeof(string));
		}

		public Builds(Database database)
		{
			this.database = database;
		}
	}
}