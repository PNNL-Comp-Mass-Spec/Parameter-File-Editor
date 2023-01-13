using System;
using System.Drawing;
using System.Windows.Forms;

namespace ParamGenTest
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            txtOutputPath = new TextBox();
            txtDMSConnectionString = new TextBox();
            cboFileTypes = new ComboBox();
            lblOutputPath = new Label();
            lblPickList = new Label();
            lblConnectionString = new Label();
            lblParamFileType = new Label();
            cboAvailableParams = new ComboBox();
            txtResults = new TextBox();
            cmdDoIt = new Button();
            txtFASTAPath = new TextBox();
            txtDatasetID = new TextBox();
            lblDatasetID = new Label();
            txtParamFileName = new TextBox();
            lblParamFileName = new Label();
            lblFASTAFilePath = new Label();
            SuspendLayout();
            //
            // txtOutputPath
            //
            txtOutputPath.Location = new Point(156, 20);
            txtOutputPath.Name = "txtOutputPath";
            txtOutputPath.Size = new Size(400, 20);
            txtOutputPath.TabIndex = 0;
            txtOutputPath.Text = @"F:\Temp\";
            txtOutputPath.TextChanged += new EventHandler(txtOutputPath_TextChanged);
            //
            // txtDMSConnectionString
            //
            txtDMSConnectionString.Location = new Point(156, 125);
            txtDMSConnectionString.Name = "txtDMSConnectionString";
            txtDMSConnectionString.Size = new Size(400, 20);
            txtDMSConnectionString.TabIndex = 2;
            txtDMSConnectionString.Text = "Data Source=gigasax;Initial Catalog=DMS5;Integrated Security=SSPI;";
            txtDMSConnectionString.TextChanged += new EventHandler(txtDMSConnectionString_TextChanged);
            //
            // cboFileTypes
            //
            cboFileTypes.DropDownStyle = ComboBoxStyle.DropDownList;
            cboFileTypes.Location = new Point(156, 165);
            cboFileTypes.Name = "cboFileTypes";
            cboFileTypes.Size = new Size(400, 21);
            cboFileTypes.TabIndex = 3;
            cboFileTypes.SelectedIndexChanged += new EventHandler(cboFileTypes_SelectedIndexChanged);
            //
            // lblOutputPath
            //
            lblOutputPath.Location = new Point(16, 24);
            lblOutputPath.Name = "lblOutputPath";
            lblOutputPath.Size = new Size(120, 23);
            lblOutputPath.TabIndex = 4;
            lblOutputPath.Text = "Output Path";
            //
            // lblPickList
            //
            lblPickList.Location = new Point(16, 64);
            lblPickList.Name = "lblPickList";
            lblPickList.Size = new Size(120, 17);
            lblPickList.TabIndex = 5;
            lblPickList.Text = "Param File Pick List";
            //
            // lblConnectionString
            //
            lblConnectionString.Location = new Point(16, 125);
            lblConnectionString.Name = "lblConnectionString";
            lblConnectionString.Size = new Size(120, 23);
            lblConnectionString.TabIndex = 6;
            lblConnectionString.Text = "Connect String";
            //
            // lblParamFileType
            //
            lblParamFileType.Location = new Point(16, 169);
            lblParamFileType.Name = "lblParamFileType";
            lblParamFileType.Size = new Size(120, 23);
            lblParamFileType.TabIndex = 7;
            lblParamFileType.Text = "Param File Type";
            //
            // cboAvailableParams
            //
            cboAvailableParams.DropDownStyle = ComboBoxStyle.DropDownList;
            cboAvailableParams.Location = new Point(156, 60);
            cboAvailableParams.Name = "cboAvailableParams";
            cboAvailableParams.Size = new Size(400, 21);
            cboAvailableParams.Sorted = true;
            cboAvailableParams.TabIndex = 8;
            cboAvailableParams.SelectedIndexChanged += new EventHandler(cboAvailableParams_SelectedIndexChanged);
            //
            // txtResults
            //
            txtResults.Location = new Point(24, 253);
            txtResults.Multiline = true;
            txtResults.Name = "txtResults";
            txtResults.Size = new Size(384, 84);
            txtResults.TabIndex = 9;
            //
            // cmdDoIt
            //
            cmdDoIt.Location = new Point(436, 305);
            cmdDoIt.Name = "cmdDoIt";
            cmdDoIt.Size = new Size(124, 32);
            cmdDoIt.TabIndex = 10;
            cmdDoIt.Text = "&Go";
            cmdDoIt.Click += new EventHandler(cmdDoIt_Click);
            //
            // txtFASTAPath
            //
            txtFASTAPath.Location = new Point(160, 209);
            txtFASTAPath.Name = "txtFASTAPath";
            txtFASTAPath.Size = new Size(396, 20);
            txtFASTAPath.TabIndex = 11;
            txtFASTAPath.Text = @"C:\DMS_Temp_Org\bsa.fasta";
            txtFASTAPath.TextChanged += new EventHandler(txtFASTAPath_TextChanged);
            //
            // txtDatasetID
            //
            txtDatasetID.Location = new Point(436, 279);
            txtDatasetID.Name = "txtDatasetID";
            txtDatasetID.Size = new Size(120, 20);
            txtDatasetID.TabIndex = 12;
            txtDatasetID.Text = "101986";
            txtDatasetID.TextAlign = HorizontalAlignment.Right;
            //
            // lblDatasetID
            //
            lblDatasetID.AutoSize = true;
            lblDatasetID.Location = new Point(436, 260);
            lblDatasetID.Name = "lblDatasetID";
            lblDatasetID.Size = new Size(121, 15);
            lblDatasetID.TabIndex = 13;
            lblDatasetID.Text = "(Optional) Dataset ID";
            //
            // txtParamFileName
            //
            txtParamFileName.Location = new Point(156, 87);
            txtParamFileName.Name = "txtParamFileName";
            txtParamFileName.Size = new Size(400, 20);
            txtParamFileName.TabIndex = 14;
            //
            // lblParamFileName
            //
            lblParamFileName.Location = new Point(16, 87);
            lblParamFileName.Name = "lblParamFileName";
            lblParamFileName.Size = new Size(120, 20);
            lblParamFileName.TabIndex = 15;
            lblParamFileName.Text = "Param File Name";
            //
            // lblFASTAFilePath
            //
            lblFASTAFilePath.Location = new Point(16, 203);
            lblFASTAFilePath.Name = "lblFASTAFilePath";
            lblFASTAFilePath.Size = new Size(120, 41);
            lblFASTAFilePath.TabIndex = 16;
            lblFASTAFilePath.Text = "FASTA File Path (only for SEQUEST)";
            //
            // Main
            //
            AutoScaleBaseSize = new Size(5, 13);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 346);
            Controls.Add(lblFASTAFilePath);
            Controls.Add(lblParamFileName);
            Controls.Add(txtParamFileName);
            Controls.Add(lblDatasetID);
            Controls.Add(txtDatasetID);
            Controls.Add(txtFASTAPath);
            Controls.Add(txtResults);
            Controls.Add(txtDMSConnectionString);
            Controls.Add(txtOutputPath);
            Controls.Add(cmdDoIt);
            Controls.Add(cboAvailableParams);
            Controls.Add(lblParamFileType);
            Controls.Add(lblConnectionString);
            Controls.Add(lblPickList);
            Controls.Add(lblOutputPath);
            Controls.Add(cboFileTypes);
            this.Name = "Main";
            Text = "DMS Param File Generator";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        
        private TextBox txtOutputPath;
        private TextBox txtDMSConnectionString;
        private ComboBox cboFileTypes;
        private ComboBox cboAvailableParams;
        private TextBox txtResults;
        private Button cmdDoIt;
        private TextBox txtFASTAPath;
        private Label lblOutputPath;
        private Label lblPickList;
        private Label lblConnectionString;
        private TextBox txtDatasetID;
        private Label lblDatasetID;
        private TextBox txtParamFileName;
        private Label lblParamFileName;
        private Label lblFASTAFilePath;
        private Label lblParamFileType;
    }
}

