using System;
using SmartAssembly.SmartUsageCore;

namespace SmartAssembly.SmartUsageWithoutUI
{
    class UsageReporterWithoutUI : UsageReporter
    {
        public static void FeatureUsed(uint featureId)
        {
            UsageReporterWithoutUI reporter = new UsageReporterWithoutUI();
            reporter.RecordFeatureUsed(featureId);
        }

        public static void DynamicFeatureUsed(string featureName)
        {
            UsageReporterWithoutUI reporter = new UsageReporterWithoutUI();
            reporter.RecordFeatureUsed(featureName);
        }

        protected override void ReportUsage(UsageReportSender sender)
        {
            // Nothing special to do, just send the report
            sender.SendReport();
        }

        protected override bool CanReportUsage()
        {
            // You can implement your own mechanism to let end-users decide
            // if they want to opt-in or opt-out from Feature Usage Reporting
            // and store their decision as appropriate.
            return true;
        }
    }
}
