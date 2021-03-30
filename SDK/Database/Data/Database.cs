using SmartAssembly.Data;
using System;

namespace SmartAssembly.DatabaseSample.Data
{
	public class Database : SmartAssembly.SDK.Database
	{
		private Builds builds = null;
        private Projects projects = null;
        private ExceptionReports exceptionReports = null;
        private FeatureReports featureReports = null;

		public Builds Builds
		{
			get
			{
				if (builds == null) builds = new Builds(this);
				return builds;
			}
		}

		public Projects Projects
		{
			get
			{
				if (projects == null) projects = new Projects(this);
				return projects;
			}
		}

        public ExceptionReports ExceptionReports
        {
            get
            {
                if (exceptionReports == null) exceptionReports = new ExceptionReports(this);
                return exceptionReports;
            }
        }

        public FeatureReports FeatureReports
        {
            get
            {
                if (featureReports == null) featureReports = new FeatureReports(this);
                return featureReports;
            }
        }

		public Database()
		{
		}
	}
}
