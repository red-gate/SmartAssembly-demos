using System;
using System.Windows.Forms;
using Microsoft.Win32;
using SmartAssembly.SmartUsageCore;

namespace SmartAssembly.SmartUsageWithUI
{
    class UsageReporterWithUI : UsageReporter
    {
        internal static readonly string ApplicationName = "{1fe9e38e-05cc-46a3-ae48-6cda8fb62056}";
        internal static readonly string CompanyName = "{395edd3b-130e-4160-bb08-6931086cea46}";

        private const string SmartAssemblyReportUsageValue = "SmartAssemblyReportUsage";
        private const string FirstRunPlaceholder = "Unknown";

        private static bool? s_CanSendReportThisSession;

        public static void FeatureUsed(uint featureId)
        {
            UsageReporterWithUI reporter = new UsageReporterWithUI();
            reporter.RecordFeatureUsed(featureId);
        }

        public static void DynamicFeatureUsed(string featureName)
        {
            UsageReporterWithUI reporter = new UsageReporterWithUI();
            reporter.RecordFeatureUsed(featureName);
        }

        protected override void ReportUsage(UsageReportSender sender)
        {
            sender.SendReport();
        }

        protected override bool CanReportUsage()
        {
            return CheckAgreedToUsageReports();
        }

        private static bool CheckAgreedToUsageReports()
        {
            if (!s_CanSendReportThisSession.HasValue)
            {
                ChooseWhetherToSendReport();
            }

            return s_CanSendReportThisSession.Value;
        }

        private static void ChooseWhetherToSendReport()
        {
            RegistryKey key = null;
            try
            {
                string keyName = "Software\\" + CompanyName + "\\" + ApplicationName;
                key = Registry.CurrentUser.CreateSubKey(keyName);

                object saReportUsageValue = key.GetValue(SmartAssemblyReportUsageValue);
                if (saReportUsageValue == null)
                {
                    // Add a session of delay if we don't want a consent dialog on the first run
#if !CONSENT_FIRST_RUN
                    key.SetValue(SmartAssemblyReportUsageValue, FirstRunPlaceholder);
                    s_CanSendReportThisSession = false;
                    return;
                }

                if (FirstRunPlaceholder.Equals(saReportUsageValue))
                {
#endif
                    // set to false temporarily to try and prevent race conditions with other assembly feature usages...
                    key.SetValue(SmartAssemblyReportUsageValue, Boolean.FalseString);
                    bool reportUsage = AskUserForReportUsageConfirm();
                    key.SetValue(SmartAssemblyReportUsageValue, reportUsage.ToString(), RegistryValueKind.String);
                    s_CanSendReportThisSession = reportUsage;
                    return;
                }

                s_CanSendReportThisSession = Convert.ToBoolean(saReportUsageValue);
            }
            catch
            {
                // something went wrong!
                s_CanSendReportThisSession = false;
            }
            finally
            {
                if (key != null)
                    key.Close();
            }
        }

        private static bool AskUserForReportUsageConfirm()
        {
            using (ConfirmFeatureUsageReportingForm form = new ConfirmFeatureUsageReportingForm())
            {
                form.ShowDialog();
                return form.DialogResult == DialogResult.Yes;
            }
        }
    }
}