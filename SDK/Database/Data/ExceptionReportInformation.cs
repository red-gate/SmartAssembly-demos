using System;
using System.Data.Common;
using SmartAssembly.Data;

namespace SmartAssembly.DatabaseSample.Data
{
	public struct ExceptionReportInformation
	{
		private string id;
		private string projectID;
		private string assemblyID;
		private string userID;
		private DateTime creationDate;
		private string exceptionType;
		private string exceptionMessage;
		private string typeName;
		private string methodName;
		private bool unread;
		private bool isFixed;
		private Flag flag;

		public string ID
		{
			get
			{
				return id;
			}
		}

		public string ProjectID
		{
			get
			{
				return projectID;
			}
		}

		public string UserID
		{
			get
			{
				return userID;
			}
		}

		public string AssemblyID
		{
			get
			{
				return assemblyID;
			}
		}

		public DateTime CreationDate
		{
			get
			{
				return creationDate;
			}
		}

		public string ExceptionType
		{
			get
			{
				return exceptionType;
			}
		}

		public string ExceptionMessage
		{
			get
			{
				return exceptionMessage;
			}
		}

		public string TypeName
		{
			get
			{
				return typeName;
			}
		}

		public string MethodName
		{
			get
			{
				return methodName;
			}
		}

		public bool Unread
		{
			get
			{
				return unread;
			}
		}

		public bool Fixed
		{
			get
			{
				return isFixed;
			}
		}

		public bool Flagged
		{
			get
			{
				return (int)flag > 0;
			}
		}

		public Flag Flag
		{
			get
			{
				return flag;
			}
		}

		public ExceptionReportInformation(DbDataReader reader)
		{
			id = string.Empty;
			projectID = string.Empty;
			assemblyID = string.Empty;
			userID = string.Empty;
			creationDate = DateTime.Now;
			exceptionType = string.Empty;
			exceptionMessage = string.Empty;
			typeName = string.Empty;
			methodName = string.Empty;
			unread = true;
			isFixed = false;
			flag = 0;

			for (int i=0; i<reader.FieldCount; i++)
			{
				object fieldValue = reader.GetValue(i);
				if (fieldValue == DBNull.Value) continue;

				switch (reader.GetName(i))
				{
					case "ID":
						id = fieldValue.ParseGuid().ToString("B");
						break;

					case "ProjectID":
						projectID = fieldValue.ParseGuid().ToString("B");
						break;

					case "AssemblyID":
						assemblyID = fieldValue.ParseGuid().ToString("B");
						break;

					case "CreationDate":
						creationDate = (DateTime)fieldValue;
						break;

					case "ExceptionType":
						exceptionType = (string)fieldValue;
						break;

					case "ExceptionMessage":
						exceptionMessage = (string)fieldValue;
						break;

					case "TypeName":
						typeName = (string)fieldValue;
						break;

					case "MethodName":
						methodName = (string)fieldValue;
						break;

					case "Unread":
						unread = Convert.ToBoolean(fieldValue);
						break;

					case "Fixed":
						isFixed = Convert.ToBoolean(fieldValue);
						break;

					case "Flag":
						flag = (Flag)Convert.ToByte(fieldValue);
						break;

					case "UserID":
						userID = fieldValue.ParseGuid().ToString("B");
						break;

					case "Data":
						break;
				}
			}
		}
	}
}
