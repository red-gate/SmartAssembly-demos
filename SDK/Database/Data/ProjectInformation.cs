using SmartAssembly.DatabaseSample.Data;
using System;
using System.Data.Common;

namespace SmartAssembly.Data
{
	public struct ProjectInformation
	{
		private string id;
		private string name;

		public string ID
		{
			get
			{
				return id;
			}
		}

		public string Name
		{
			get
			{
				return name;
			}
		}

		internal ProjectInformation(DbDataReader reader)
		{
			id = string.Empty;
			name = string.Empty;

			for (int i=0; i<reader.FieldCount; i++)
			{
				object fieldValue = reader.GetValue(i);
				if (fieldValue == DBNull.Value) continue;

				switch (reader.GetName(i))
				{
					case "ID":
						id = fieldValue.ParseGuid().ToString("B");
						break;

					case "Name":
						name = (string)fieldValue;
						break;
				}
			}
		}
	}
}
