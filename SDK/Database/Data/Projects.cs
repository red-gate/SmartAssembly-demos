using System.Collections;
using System.Data.Common;
using SmartAssembly.Data;

namespace SmartAssembly.DatabaseSample.Data
{
	public class Projects
	{
		private Database database = null;

		public ProjectInformation GetProjectInformation(string projectID)
		{
			ProjectInformation projectInformation = new ProjectInformation();

			DbDataReader reader = database.ExecuteReader("SELECT * FROM Projects WHERE ID=@1", new System.Guid(projectID));
			if (reader.Read()) projectInformation = new ProjectInformation(reader);
			reader.Close();

			return projectInformation;
		}

		public string[] GetProjectIDs()
		{
			ArrayList IDs = new ArrayList();
			DbDataReader reader = database.ExecuteReader("SELECT ID FROM Projects ORDER BY Name");

			while(reader.Read())
			{
				IDs.Add(reader.GetGuid(0).ToString("B"));
			}

			reader.Close();
			return (string[]) IDs.ToArray(typeof(string));
		}

		public Projects(Database database)
		{
			this.database = database;
		}
	}
}
