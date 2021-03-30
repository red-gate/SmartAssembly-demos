using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SmartAssembly.SmartExceptionsCore;

namespace SmartAssembly.SmartUsageWithUI
{
    class ConfirmFeatureUsageReportingForm : Form
    {
        private Label m_Label_Desc;
        private Button m_SendUsageReportsBtn;
        private Button m_DoNotSendFeatureUsageReportsBtn;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfirmFeatureUsageReportingForm));
            this.m_Label_Desc = new System.Windows.Forms.Label();
            this.m_SendUsageReportsBtn = new System.Windows.Forms.Button();
            this.m_DoNotSendFeatureUsageReportsBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_Label_Desc
            // 
            this.m_Label_Desc.AutoSize = true;
            this.m_Label_Desc.Location = new System.Drawing.Point(12, 16);
            this.m_Label_Desc.MaximumSize = new System.Drawing.Size(480, 800);
            this.m_Label_Desc.Name = "m_Label_Desc";
            this.m_Label_Desc.Padding = new System.Windows.Forms.Padding(0, 0, 0, 92);
            this.m_Label_Desc.Size = new System.Drawing.Size(442, 159);
            this.m_Label_Desc.TabIndex = 1;
            this.m_Label_Desc.TabStop = true;
            this.m_Label_Desc.Text = resources.GetString("m_Label_Desc.Text");
            this.m_Label_Desc.UseCompatibleTextRendering = true;
            // 
            // m_SendUsageReportsBtn
            // 
            this.m_SendUsageReportsBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.m_SendUsageReportsBtn.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.m_SendUsageReportsBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.m_SendUsageReportsBtn.Location = new System.Drawing.Point(97, 105);
            this.m_SendUsageReportsBtn.Name = "m_SendUsageReportsBtn";
            this.m_SendUsageReportsBtn.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.m_SendUsageReportsBtn.Size = new System.Drawing.Size(299, 33);
            this.m_SendUsageReportsBtn.TabIndex = 2;
            this.m_SendUsageReportsBtn.Text = "Join the quality improvement program";
            this.m_SendUsageReportsBtn.UseVisualStyleBackColor = true;
            // 
            // m_DoNotSendFeatureUsageReportsBtn
            // 
            this.m_DoNotSendFeatureUsageReportsBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.m_DoNotSendFeatureUsageReportsBtn.DialogResult = System.Windows.Forms.DialogResult.No;
            this.m_DoNotSendFeatureUsageReportsBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.m_DoNotSendFeatureUsageReportsBtn.Location = new System.Drawing.Point(97, 144);
            this.m_DoNotSendFeatureUsageReportsBtn.Name = "m_DoNotSendFeatureUsageReportsBtn";
            this.m_DoNotSendFeatureUsageReportsBtn.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.m_DoNotSendFeatureUsageReportsBtn.Size = new System.Drawing.Size(299, 33);
            this.m_DoNotSendFeatureUsageReportsBtn.TabIndex = 3;
            this.m_DoNotSendFeatureUsageReportsBtn.Text = "Do not join the quality improvement program";
            this.m_DoNotSendFeatureUsageReportsBtn.UseVisualStyleBackColor = true;
            // 
            // ConfirmFeatureUsageReportingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(504, 190);
            this.ControlBox = false;
            this.Controls.Add(this.m_DoNotSendFeatureUsageReportsBtn);
            this.Controls.Add(this.m_SendUsageReportsBtn);
            this.Controls.Add(this.m_Label_Desc);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfirmFeatureUsageReportingForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "{0} Quality Improvement Program";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public ConfirmFeatureUsageReportingForm()
        {
            InitializeComponent();

            // Hack to work around anchor not working in high dpi mode
            m_DoNotSendFeatureUsageReportsBtn.Top = ClientSize.Height - 13 - m_DoNotSendFeatureUsageReportsBtn.Height;
            m_SendUsageReportsBtn.Top = m_DoNotSendFeatureUsageReportsBtn.Top - 6 - m_SendUsageReportsBtn.Height;

            m_SendUsageReportsBtn.Image = Resources.GetBitmap("ok");
            m_DoNotSendFeatureUsageReportsBtn.Image = Resources.GetBitmap("error");
            
            m_Label_Desc.Text = String.Format(m_Label_Desc.Text, UsageReporterWithUI.ApplicationName, UsageReporterWithUI.CompanyName);

            Text = string.Format(Text, UsageReporterWithUI.ApplicationName);
        }
    }
}
