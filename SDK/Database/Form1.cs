using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using SmartAssembly.Data;
using SmartAssembly.DatabaseSample.Data;
using SmartAssembly.SDK;
using Database = SmartAssembly.DatabaseSample.Data.Database;

namespace SmartAssembly.DatabaseSample
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class Form1 : Form
    {
        #region FeedbackEventArgs & FeedbackEventHandler

        public delegate void FeedbackEventHandler(object sender, FeedbackEventArgs e);

        public class FeedbackEventArgs : EventArgs
        {
            private readonly string m_InformationText = string.Empty;

            internal FeedbackEventArgs(string informationText)
            {
                m_InformationText = informationText;
            }

            public string InformationText
            {
                get { return m_InformationText; }
            }
        }

        #endregion

        private const int c_Limit = 100;

        private readonly Database m_Database = new Database();
        private ExceptionReport[] m_AllReports;

        private ColumnHeader m_ColumnHeader1;
        private ColumnHeader m_ColumnHeader11;
        private ColumnHeader m_ColumnHeader12;
        private ColumnHeader m_ColumnHeader13;
        private ColumnHeader m_ColumnHeader14;
        private ColumnHeader m_ColumnHeader15;
        private ColumnHeader m_ColumnHeader16;
        private ColumnHeader m_ColumnHeader17;
        private ColumnHeader m_ColumnHeader18;
        private ColumnHeader m_ColumnHeader19;
        private ColumnHeader m_ColumnHeader2;
        private ColumnHeader m_ColumnHeader20;
        private ColumnHeader m_ColumnHeader5;
        private ColumnHeader m_ColumnHeader6;
        private Button m_DecodeStackTrace;
        private Button m_DownloadNewReports;
        private Process m_DownloadReportsProcess;
        private Button m_GetBuilds;
        private Button m_GetProjects;
        private Button m_GetReports;
        private Label m_Label1;
        private Label m_Label2;
        private ListBox m_ListBox1;
        private ListBox m_ListBox2;
        private ListBox m_ListBox3;
        private ListView m_ListView1;
        private ListView m_ListView2;
        private ListView m_ListView3;
        private SaveFileDialog m_SaveFileDialog1;
        private Button m_SaveReport;
        private TabControl m_TabControl1;
        private TabPage m_TabPage1;
        private TabPage m_TabPage2;
        private TabPage m_TabPage3;
        private TabPage m_TabPage4;
        private TabPage m_TabPage5;
        private TextBox m_TextBox1;
        private TextBox m_TextBox2;
        private TabPage m_TabPage6;
        private ListView m_ListView_Features;
        private ColumnHeader m_Column_FeatureName;
        private ColumnHeader m_Column_UsageCount;
        private Button m_Button_GetFeatures;
        private Button m_ViewStackTrace;

        public Form1()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        protected override void OnClosed(EventArgs e)
        {
            if (m_DownloadReportsProcess != null && !m_DownloadReportsProcess.HasExited)
            {
                m_DownloadReportsProcess.Kill();
                m_DownloadReportsProcess.Dispose();
            }
            base.OnClosed(e);
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new Form1());
        }

        private void GetProjectsClick(object sender, EventArgs e)
        {
            m_ListView1.Items.Clear();

            string[] projectIDs = m_Database.Projects.GetProjectIDs();
            for (int index = 0; index < projectIDs.Length && index < c_Limit; index++)
            {
                string projectId = projectIDs[index];
                ProjectInformation projectInformation = m_Database.Projects.GetProjectInformation(projectId);
                m_ListView1.Items.Add(new ListViewItem(new[] {projectInformation.ID, projectInformation.Name}));
            }
        }

        private void m_Button_GetFeatures_Click(object sender, EventArgs e)
        {
            m_ListView_Features.Items.Clear();

            List<FeatureUsageInformation> allFeatureUsages = m_Database.FeatureReports.GetAllFeatureUsages();
            for (int i = 0; i < allFeatureUsages.Count && i < c_Limit; i++)
            {
                FeatureUsageInformation usageInformation = allFeatureUsages[i];
                string featureName = m_Database.FeatureReports.GetFeatureNameFromFeatureID(usageInformation.FeatureID);
                m_ListView_Features.Items.Add(new ListViewItem(new[] {featureName, usageInformation.UsageCount.ToString()}));
            }
        }

        private void GetBuildsClick(object sender, EventArgs e)
        {
            m_ListView2.Items.Clear();

            string[] allBuilds = m_Database.Builds.GetAllBuilds();
            for (int index = 0; index < allBuilds.Length && index < c_Limit; index++)
            {
                string assemblyId = allBuilds[index];
                BuildInformation buildInformation = m_Database.Builds.GetBuildInformation(assemblyId);
                string projectName = m_Database.Projects.GetProjectInformation(buildInformation.ProjectID).Name;
                m_ListView2.Items.Add(
                    new ListViewItem(new[]
                                         {
                                             projectName, buildInformation.BuildDate.ToString("g")
                                         }));
            }
        }

        private void GetReportsClick(object sender, EventArgs e)
        {
            //Array is indexed by tags in listview, so empying listview first stops out of bounds access in the case of an exception
            m_ListView3.Items.Clear();
            m_AllReports = m_Database.ExceptionReports.GetAllReports();

            for (int i = 0; i < m_AllReports.GetLength(0) && i < c_Limit; i++)
            {
                ExceptionReport thisReport = m_AllReports[i];
                //Get info, skip if no info found
                var reportInformation = new ExceptionReportInformation();
                if (!thisReport.TryGetReportInformation(ref reportInformation)) continue;

                string projectName = m_Database.Projects.GetProjectInformation(reportInformation.ProjectID).Name;
                var item =
                    new ListViewItem(new[]
                                         {
                                             projectName, reportInformation.CreationDate.ToString("g"),
                                             reportInformation.ExceptionType, reportInformation.ExceptionMessage,
                                             reportInformation.TypeName, reportInformation.MethodName,
                                             reportInformation.Unread.ToString(), reportInformation.Fixed.ToString(),
                                             reportInformation.Flag.ToString(), reportInformation.UserID
                                         }) {Tag = i};

                m_ListView3.Items.Add(item);
            }
        }

        private void SaveReportClick(object sender, EventArgs e)
        {
            if (m_ListView3.SelectedItems.Count > 0 && m_SaveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var reportId = (int) m_ListView3.SelectedItems[0].Tag;

                byte[] data = m_AllReports[reportId].GetData();

                FileStream stream = File.Create(m_SaveFileDialog1.FileName);
                stream.Write(data, 0, data.Length);
                stream.Close();
            }
        }

        private void ListView3SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_ListView3.SelectedItems.Count > 0)
            {
                m_ListBox1.Items.Clear();
                m_ListBox2.Items.Clear();

                var reportId = (int) m_ListView3.SelectedItems[0].Tag;
                Dictionary<string, string> customProperties = m_AllReports[reportId].GetCustomProperties();

                foreach (string propertyName in customProperties.Keys)
                {
                    string propertyValue = customProperties[propertyName];
                    m_ListBox1.Items.Add(propertyName + "=" + propertyValue);
                }

                Dictionary<string, AttachedFile> attachedFiles = m_AllReports[reportId].GetAttachedFiles();

                foreach (string key in attachedFiles.Keys)
                {
                    AttachedFile attachedFile = attachedFiles[key];
                    m_ListBox2.Items.Add(attachedFile);
                }
            }
        }

        private void DecodeStackTraceClick(object sender, EventArgs e)
        {
            m_TextBox2.Text = Helpers.DecodeStackTrace(m_TextBox1.Text);
        }

        private void ListBox2DoubleClick(object sender, EventArgs e)
        {
            var attachedFile = m_ListBox2.SelectedItem as AttachedFile;
            if (attachedFile != null)
            {
                string tempFileName = Path.GetTempPath() + Guid.NewGuid().ToString("N") +
                                      Path.GetExtension(attachedFile.FileName);

                FileStream stream = File.OpenWrite(tempFileName);
                stream.Write(attachedFile.Bytes, 0, attachedFile.Bytes.Length);
                stream.Close();

                Process.Start(tempFileName);
            }
        }

        private void ViewStackTraceClick(object sender, EventArgs e)
        {
            if (m_ListView3.SelectedItems.Count > 0)
            {
                var reportId = (int) m_ListView3.SelectedItems[0].Tag;

                var memoryStream = new MemoryStream(m_AllReports[reportId].GetData());
                var xmlDocument = new XmlDocument();
                var sb = new StringBuilder();

                xmlDocument.Load(memoryStream);

                foreach (XmlElement node in xmlDocument.GetElementsByTagName("StackFrame"))
                {
                    sb.Append(node.Attributes["Method"].Value);
                    sb.Append(Environment.NewLine);

                    if (node.Attributes["DocumentURL"] != null)
                    {
                        sb.Append(node.Attributes["DocumentURL"].Value);
                        if (node.Attributes["LineNumber"] != null)
                        {
                            sb.Append(" : line ");
                            sb.Append(node.Attributes["LineNumber"].Value);
                        }
                        sb.Append(Environment.NewLine);
                    }

                    sb.Append(Environment.NewLine);
                }

                memoryStream.Close();
                MessageBox.Show(sb.ToString());
            }
        }

        private void ReadOutput()
        {
            try
            {
                while (m_DownloadReportsProcess != null)
                {
                    string line = m_DownloadReportsProcess.StandardOutput.ReadLine();
                    if (line != null)
                        Invoke(new FeedbackEventHandler(OnFeedback), new object[] {this, new FeedbackEventArgs(line)});
                    if (m_DownloadReportsProcess.HasExited) break;
                }
            }
            catch (InvalidOperationException)
            {
            }
        }

        private void OnFeedback(object sender, FeedbackEventArgs e)
        {
            m_ListBox3.Items.Add(e.InformationText);
        }

        private void DownloadNewReportsClick(object sender, EventArgs e)
        {
            if (m_DownloadReportsProcess != null && !m_DownloadReportsProcess.HasExited) return;

            var processStartInfo =
                new ProcessStartInfo(Path.Combine(Helpers.GetSmartAssemblyPath(), "SmartAssembly.com"),
                                     "/downloadnewreports")
                    {
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        RedirectStandardOutput = true
                    };

            m_DownloadReportsProcess = new Process {StartInfo = processStartInfo};

            m_DownloadReportsProcess.Start();
            m_ListBox3.Items.Clear();
            (new Thread(ReadOutput)).Start();
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.m_TabControl1 = new System.Windows.Forms.TabControl();
            this.m_TabPage1 = new System.Windows.Forms.TabPage();
            this.m_ListView1 = new System.Windows.Forms.ListView();
            this.m_ColumnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.m_ColumnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.m_GetProjects = new System.Windows.Forms.Button();
            this.m_TabPage2 = new System.Windows.Forms.TabPage();
            this.m_ListView2 = new System.Windows.Forms.ListView();
            this.m_ColumnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.m_ColumnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.m_GetBuilds = new System.Windows.Forms.Button();
            this.m_TabPage3 = new System.Windows.Forms.TabPage();
            this.m_ViewStackTrace = new System.Windows.Forms.Button();
            this.m_ListBox2 = new System.Windows.Forms.ListBox();
            this.m_Label2 = new System.Windows.Forms.Label();
            this.m_ListBox1 = new System.Windows.Forms.ListBox();
            this.m_SaveReport = new System.Windows.Forms.Button();
            this.m_ListView3 = new System.Windows.Forms.ListView();
            this.m_ColumnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.m_ColumnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.m_ColumnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.m_ColumnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.m_ColumnHeader15 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.m_ColumnHeader16 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.m_ColumnHeader17 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.m_ColumnHeader18 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.m_ColumnHeader19 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.m_ColumnHeader20 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.m_GetReports = new System.Windows.Forms.Button();
            this.m_Label1 = new System.Windows.Forms.Label();
            this.m_TabPage4 = new System.Windows.Forms.TabPage();
            this.m_TextBox2 = new System.Windows.Forms.TextBox();
            this.m_DecodeStackTrace = new System.Windows.Forms.Button();
            this.m_TextBox1 = new System.Windows.Forms.TextBox();
            this.m_TabPage5 = new System.Windows.Forms.TabPage();
            this.m_DownloadNewReports = new System.Windows.Forms.Button();
            this.m_ListBox3 = new System.Windows.Forms.ListBox();
            this.m_TabPage6 = new System.Windows.Forms.TabPage();
            this.m_SaveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.m_ListView_Features = new System.Windows.Forms.ListView();
            this.m_Column_FeatureName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.m_Column_UsageCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.m_Button_GetFeatures = new System.Windows.Forms.Button();
            this.m_TabControl1.SuspendLayout();
            this.m_TabPage1.SuspendLayout();
            this.m_TabPage2.SuspendLayout();
            this.m_TabPage3.SuspendLayout();
            this.m_TabPage4.SuspendLayout();
            this.m_TabPage5.SuspendLayout();
            this.m_TabPage6.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_TabControl1
            // 
            this.m_TabControl1.Controls.Add(this.m_TabPage1);
            this.m_TabControl1.Controls.Add(this.m_TabPage2);
            this.m_TabControl1.Controls.Add(this.m_TabPage3);
            this.m_TabControl1.Controls.Add(this.m_TabPage4);
            this.m_TabControl1.Controls.Add(this.m_TabPage5);
            this.m_TabControl1.Controls.Add(this.m_TabPage6);
            this.m_TabControl1.Location = new System.Drawing.Point(8, 8);
            this.m_TabControl1.Name = "m_TabControl1";
            this.m_TabControl1.SelectedIndex = 0;
            this.m_TabControl1.Size = new System.Drawing.Size(704, 584);
            this.m_TabControl1.TabIndex = 3;
            // 
            // m_TabPage1
            // 
            this.m_TabPage1.Controls.Add(this.m_ListView1);
            this.m_TabPage1.Controls.Add(this.m_GetProjects);
            this.m_TabPage1.Location = new System.Drawing.Point(4, 22);
            this.m_TabPage1.Name = "m_TabPage1";
            this.m_TabPage1.Size = new System.Drawing.Size(696, 558);
            this.m_TabPage1.TabIndex = 0;
            this.m_TabPage1.Text = "Projects";
            this.m_TabPage1.UseVisualStyleBackColor = true;
            // 
            // m_ListView1
            // 
            this.m_ListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.m_ColumnHeader1,
            this.m_ColumnHeader2});
            this.m_ListView1.Location = new System.Drawing.Point(16, 48);
            this.m_ListView1.Name = "m_ListView1";
            this.m_ListView1.Size = new System.Drawing.Size(664, 496);
            this.m_ListView1.TabIndex = 1;
            this.m_ListView1.UseCompatibleStateImageBehavior = false;
            this.m_ListView1.View = System.Windows.Forms.View.Details;
            // 
            // m_ColumnHeader1
            // 
            this.m_ColumnHeader1.Text = "Project ID";
            this.m_ColumnHeader1.Width = 270;
            // 
            // m_ColumnHeader2
            // 
            this.m_ColumnHeader2.Text = "Project Name";
            this.m_ColumnHeader2.Width = 300;
            // 
            // m_GetProjects
            // 
            this.m_GetProjects.Location = new System.Drawing.Point(16, 8);
            this.m_GetProjects.Name = "m_GetProjects";
            this.m_GetProjects.Size = new System.Drawing.Size(104, 32);
            this.m_GetProjects.TabIndex = 0;
            this.m_GetProjects.Text = "Get Projects";
            this.m_GetProjects.Click += new System.EventHandler(this.GetProjectsClick);
            // 
            // m_TabPage2
            // 
            this.m_TabPage2.Controls.Add(this.m_ListView2);
            this.m_TabPage2.Controls.Add(this.m_GetBuilds);
            this.m_TabPage2.Location = new System.Drawing.Point(4, 22);
            this.m_TabPage2.Name = "m_TabPage2";
            this.m_TabPage2.Size = new System.Drawing.Size(696, 558);
            this.m_TabPage2.TabIndex = 1;
            this.m_TabPage2.Text = "Builds";
            this.m_TabPage2.UseVisualStyleBackColor = true;
            this.m_TabPage2.Visible = false;
            // 
            // m_ListView2
            // 
            this.m_ListView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.m_ColumnHeader5,
            this.m_ColumnHeader6});
            this.m_ListView2.Location = new System.Drawing.Point(16, 48);
            this.m_ListView2.Name = "m_ListView2";
            this.m_ListView2.Size = new System.Drawing.Size(664, 496);
            this.m_ListView2.TabIndex = 3;
            this.m_ListView2.UseCompatibleStateImageBehavior = false;
            this.m_ListView2.View = System.Windows.Forms.View.Details;
            // 
            // m_ColumnHeader5
            // 
            this.m_ColumnHeader5.Text = "Project";
            this.m_ColumnHeader5.Width = 250;
            // 
            // m_ColumnHeader6
            // 
            this.m_ColumnHeader6.Text = "Build Date";
            this.m_ColumnHeader6.Width = 250;
            // 
            // m_GetBuilds
            // 
            this.m_GetBuilds.Location = new System.Drawing.Point(16, 8);
            this.m_GetBuilds.Name = "m_GetBuilds";
            this.m_GetBuilds.Size = new System.Drawing.Size(104, 32);
            this.m_GetBuilds.TabIndex = 2;
            this.m_GetBuilds.Text = "Get Builds";
            this.m_GetBuilds.Click += new System.EventHandler(this.GetBuildsClick);
            // 
            // m_TabPage3
            // 
            this.m_TabPage3.Controls.Add(this.m_ViewStackTrace);
            this.m_TabPage3.Controls.Add(this.m_ListBox2);
            this.m_TabPage3.Controls.Add(this.m_Label2);
            this.m_TabPage3.Controls.Add(this.m_ListBox1);
            this.m_TabPage3.Controls.Add(this.m_SaveReport);
            this.m_TabPage3.Controls.Add(this.m_ListView3);
            this.m_TabPage3.Controls.Add(this.m_GetReports);
            this.m_TabPage3.Controls.Add(this.m_Label1);
            this.m_TabPage3.Location = new System.Drawing.Point(4, 22);
            this.m_TabPage3.Name = "m_TabPage3";
            this.m_TabPage3.Size = new System.Drawing.Size(696, 558);
            this.m_TabPage3.TabIndex = 2;
            this.m_TabPage3.Text = "Exception Reports";
            this.m_TabPage3.UseVisualStyleBackColor = true;
            this.m_TabPage3.Visible = false;
            // 
            // m_ViewStackTrace
            // 
            this.m_ViewStackTrace.Location = new System.Drawing.Point(552, 438);
            this.m_ViewStackTrace.Name = "m_ViewStackTrace";
            this.m_ViewStackTrace.Size = new System.Drawing.Size(128, 32);
            this.m_ViewStackTrace.TabIndex = 12;
            this.m_ViewStackTrace.Text = "View Stack Trace";
            this.m_ViewStackTrace.Click += new System.EventHandler(this.ViewStackTraceClick);
            // 
            // m_ListBox2
            // 
            this.m_ListBox2.Location = new System.Drawing.Point(16, 503);
            this.m_ListBox2.Name = "m_ListBox2";
            this.m_ListBox2.Size = new System.Drawing.Size(440, 43);
            this.m_ListBox2.TabIndex = 10;
            this.m_ListBox2.DoubleClick += new System.EventHandler(this.ListBox2DoubleClick);
            // 
            // m_Label2
            // 
            this.m_Label2.Location = new System.Drawing.Point(16, 479);
            this.m_Label2.Name = "m_Label2";
            this.m_Label2.Size = new System.Drawing.Size(432, 16);
            this.m_Label2.TabIndex = 11;
            this.m_Label2.Text = "Attached Files for selected report: (dbl_click to open)";
            // 
            // m_ListBox1
            // 
            this.m_ListBox1.Location = new System.Drawing.Point(16, 424);
            this.m_ListBox1.Name = "m_ListBox1";
            this.m_ListBox1.Size = new System.Drawing.Size(440, 43);
            this.m_ListBox1.TabIndex = 8;
            // 
            // m_SaveReport
            // 
            this.m_SaveReport.Location = new System.Drawing.Point(552, 400);
            this.m_SaveReport.Name = "m_SaveReport";
            this.m_SaveReport.Size = new System.Drawing.Size(128, 32);
            this.m_SaveReport.TabIndex = 7;
            this.m_SaveReport.Text = "Save Selected Report";
            this.m_SaveReport.Click += new System.EventHandler(this.SaveReportClick);
            // 
            // m_ListView3
            // 
            this.m_ListView3.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.m_ColumnHeader11,
            this.m_ColumnHeader12,
            this.m_ColumnHeader13,
            this.m_ColumnHeader14,
            this.m_ColumnHeader15,
            this.m_ColumnHeader16,
            this.m_ColumnHeader17,
            this.m_ColumnHeader18,
            this.m_ColumnHeader19,
            this.m_ColumnHeader20});
            this.m_ListView3.FullRowSelect = true;
            this.m_ListView3.Location = new System.Drawing.Point(16, 48);
            this.m_ListView3.Name = "m_ListView3";
            this.m_ListView3.Size = new System.Drawing.Size(664, 341);
            this.m_ListView3.TabIndex = 5;
            this.m_ListView3.UseCompatibleStateImageBehavior = false;
            this.m_ListView3.View = System.Windows.Forms.View.Details;
            this.m_ListView3.SelectedIndexChanged += new System.EventHandler(this.ListView3SelectedIndexChanged);
            // 
            // m_ColumnHeader11
            // 
            this.m_ColumnHeader11.Text = "Project";
            this.m_ColumnHeader11.Width = 100;
            // 
            // m_ColumnHeader12
            // 
            this.m_ColumnHeader12.Text = "Date";
            this.m_ColumnHeader12.Width = 100;
            // 
            // m_ColumnHeader13
            // 
            this.m_ColumnHeader13.Text = "Exception Type";
            this.m_ColumnHeader13.Width = 120;
            // 
            // m_ColumnHeader14
            // 
            this.m_ColumnHeader14.Text = "Exception Message";
            this.m_ColumnHeader14.Width = 200;
            // 
            // m_ColumnHeader15
            // 
            this.m_ColumnHeader15.Text = "Type Name";
            this.m_ColumnHeader15.Width = 150;
            // 
            // m_ColumnHeader16
            // 
            this.m_ColumnHeader16.Text = "Method Name";
            this.m_ColumnHeader16.Width = 150;
            // 
            // m_ColumnHeader17
            // 
            this.m_ColumnHeader17.Text = "Unread";
            // 
            // m_ColumnHeader18
            // 
            this.m_ColumnHeader18.Text = "Fixed";
            // 
            // m_ColumnHeader19
            // 
            this.m_ColumnHeader19.Text = "Flag";
            // 
            // m_ColumnHeader20
            // 
            this.m_ColumnHeader20.Text = "UserID";
            this.m_ColumnHeader20.Width = 120;
            // 
            // m_GetReports
            // 
            this.m_GetReports.Location = new System.Drawing.Point(16, 8);
            this.m_GetReports.Name = "m_GetReports";
            this.m_GetReports.Size = new System.Drawing.Size(104, 32);
            this.m_GetReports.TabIndex = 4;
            this.m_GetReports.Text = "Get Reports";
            this.m_GetReports.Click += new System.EventHandler(this.GetReportsClick);
            // 
            // m_Label1
            // 
            this.m_Label1.Location = new System.Drawing.Point(16, 400);
            this.m_Label1.Name = "m_Label1";
            this.m_Label1.Size = new System.Drawing.Size(432, 16);
            this.m_Label1.TabIndex = 9;
            this.m_Label1.Text = "Custom Properties for selected report:";
            // 
            // m_TabPage4
            // 
            this.m_TabPage4.Controls.Add(this.m_TextBox2);
            this.m_TabPage4.Controls.Add(this.m_DecodeStackTrace);
            this.m_TabPage4.Controls.Add(this.m_TextBox1);
            this.m_TabPage4.Location = new System.Drawing.Point(4, 22);
            this.m_TabPage4.Name = "m_TabPage4";
            this.m_TabPage4.Size = new System.Drawing.Size(696, 558);
            this.m_TabPage4.TabIndex = 3;
            this.m_TabPage4.Text = "Decode Stack-Trace";
            this.m_TabPage4.UseVisualStyleBackColor = true;
            // 
            // m_TextBox2
            // 
            this.m_TextBox2.Location = new System.Drawing.Point(8, 304);
            this.m_TextBox2.Multiline = true;
            this.m_TextBox2.Name = "m_TextBox2";
            this.m_TextBox2.ReadOnly = true;
            this.m_TextBox2.Size = new System.Drawing.Size(680, 240);
            this.m_TextBox2.TabIndex = 3;
            // 
            // m_DecodeStackTrace
            // 
            this.m_DecodeStackTrace.Location = new System.Drawing.Point(16, 264);
            this.m_DecodeStackTrace.Name = "m_DecodeStackTrace";
            this.m_DecodeStackTrace.Size = new System.Drawing.Size(112, 32);
            this.m_DecodeStackTrace.TabIndex = 2;
            this.m_DecodeStackTrace.Text = "Decode";
            this.m_DecodeStackTrace.Click += new System.EventHandler(this.DecodeStackTraceClick);
            // 
            // m_TextBox1
            // 
            this.m_TextBox1.Location = new System.Drawing.Point(8, 16);
            this.m_TextBox1.Multiline = true;
            this.m_TextBox1.Name = "m_TextBox1";
            this.m_TextBox1.Size = new System.Drawing.Size(680, 240);
            this.m_TextBox1.TabIndex = 0;
            this.m_TextBox1.Text = "<copy the obfuscated stack trace here>";
            // 
            // m_TabPage5
            // 
            this.m_TabPage5.Controls.Add(this.m_DownloadNewReports);
            this.m_TabPage5.Controls.Add(this.m_ListBox3);
            this.m_TabPage5.Location = new System.Drawing.Point(4, 22);
            this.m_TabPage5.Name = "m_TabPage5";
            this.m_TabPage5.Size = new System.Drawing.Size(696, 558);
            this.m_TabPage5.TabIndex = 4;
            this.m_TabPage5.Text = "Download Reports";
            this.m_TabPage5.UseVisualStyleBackColor = true;
            // 
            // m_DownloadNewReports
            // 
            this.m_DownloadNewReports.Location = new System.Drawing.Point(13, 14);
            this.m_DownloadNewReports.Name = "m_DownloadNewReports";
            this.m_DownloadNewReports.Size = new System.Drawing.Size(144, 32);
            this.m_DownloadNewReports.TabIndex = 1;
            this.m_DownloadNewReports.Text = "Download New Reports";
            this.m_DownloadNewReports.UseVisualStyleBackColor = true;
            this.m_DownloadNewReports.Click += new System.EventHandler(this.DownloadNewReportsClick);
            // 
            // m_ListBox3
            // 
            this.m_ListBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ListBox3.FormattingEnabled = true;
            this.m_ListBox3.Location = new System.Drawing.Point(13, 62);
            this.m_ListBox3.Name = "m_ListBox3";
            this.m_ListBox3.Size = new System.Drawing.Size(663, 485);
            this.m_ListBox3.TabIndex = 0;
            // 
            // m_TabPage6
            // 
            this.m_TabPage6.Controls.Add(this.m_ListView_Features);
            this.m_TabPage6.Controls.Add(this.m_Button_GetFeatures);
            this.m_TabPage6.Location = new System.Drawing.Point(4, 22);
            this.m_TabPage6.Name = "m_TabPage6";
            this.m_TabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.m_TabPage6.Size = new System.Drawing.Size(696, 558);
            this.m_TabPage6.TabIndex = 5;
            this.m_TabPage6.Text = "Feature Usage";
            this.m_TabPage6.UseVisualStyleBackColor = true;
            // 
            // m_SaveFileDialog1
            // 
            this.m_SaveFileDialog1.DefaultExt = "sareport";
            this.m_SaveFileDialog1.Filter = "SmartAssembly Exception Report|*.sareport";
            this.m_SaveFileDialog1.Title = "Save Report";
            // 
            // m_ListView_Features
            // 
            this.m_ListView_Features.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.m_Column_FeatureName,
            this.m_Column_UsageCount});
            this.m_ListView_Features.Location = new System.Drawing.Point(16, 51);
            this.m_ListView_Features.Name = "m_ListView_Features";
            this.m_ListView_Features.Size = new System.Drawing.Size(664, 496);
            this.m_ListView_Features.TabIndex = 3;
            this.m_ListView_Features.UseCompatibleStateImageBehavior = false;
            this.m_ListView_Features.View = System.Windows.Forms.View.Details;
            // 
            // m_Column_FeatureName
            // 
            this.m_Column_FeatureName.Text = "FeatureName";
            this.m_Column_FeatureName.Width = 270;
            // 
            // m_Column_UsageCount
            // 
            this.m_Column_UsageCount.Text = "UsageCount";
            this.m_Column_UsageCount.Width = 300;
            // 
            // m_Button_GetFeatures
            // 
            this.m_Button_GetFeatures.Location = new System.Drawing.Point(16, 11);
            this.m_Button_GetFeatures.Name = "m_Button_GetFeatures";
            this.m_Button_GetFeatures.Size = new System.Drawing.Size(104, 32);
            this.m_Button_GetFeatures.TabIndex = 2;
            this.m_Button_GetFeatures.Text = "Get Features";
            this.m_Button_GetFeatures.Click += new System.EventHandler(this.m_Button_GetFeatures_Click);
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(720, 598);
            this.Controls.Add(this.m_TabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.m_TabControl1.ResumeLayout(false);
            this.m_TabPage1.ResumeLayout(false);
            this.m_TabPage2.ResumeLayout(false);
            this.m_TabPage3.ResumeLayout(false);
            this.m_TabPage4.ResumeLayout(false);
            this.m_TabPage4.PerformLayout();
            this.m_TabPage5.ResumeLayout(false);
            this.m_TabPage6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        }
}