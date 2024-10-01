using System.ComponentModel;
using System.Windows.Forms;

namespace ParamGenTest
{
    partial class Main
    {
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
            this.txtOutputPath = new System.Windows.Forms.TextBox();
            this.txtDMSConnectionString = new System.Windows.Forms.TextBox();
            this.cboFileTypes = new System.Windows.Forms.ComboBox();
            this.lblOutputPath = new System.Windows.Forms.Label();
            this.lblPickList = new System.Windows.Forms.Label();
            this.lblConnectionString = new System.Windows.Forms.Label();
            this.lblParamFileType = new System.Windows.Forms.Label();
            this.cboAvailableParams = new System.Windows.Forms.ComboBox();
            this.txtResults = new System.Windows.Forms.TextBox();
            this.cmdDoIt = new System.Windows.Forms.Button();
            this.txtFASTAPath = new System.Windows.Forms.TextBox();
            this.txtDatasetID = new System.Windows.Forms.TextBox();
            this.lblDatasetID = new System.Windows.Forms.Label();
            this.txtParamFileName = new System.Windows.Forms.TextBox();
            this.lblParamFileName = new System.Windows.Forms.Label();
            this.lblFASTAFilePath = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtOutputPath
            // 
            this.txtOutputPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputPath.Location = new System.Drawing.Point(156, 20);
            this.txtOutputPath.Name = "txtOutputPath";
            this.txtOutputPath.Size = new System.Drawing.Size(876, 20);
            this.txtOutputPath.TabIndex = 0;
            this.txtOutputPath.Text = "F:\\Temp\\";
            this.txtOutputPath.TextChanged += new System.EventHandler(this.txtOutputPath_TextChanged);
            // 
            // txtDMSConnectionString
            // 
            this.txtDMSConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDMSConnectionString.Location = new System.Drawing.Point(156, 125);
            this.txtDMSConnectionString.Name = "txtDMSConnectionString";
            this.txtDMSConnectionString.Size = new System.Drawing.Size(876, 20);
            this.txtDMSConnectionString.TabIndex = 2;
            this.txtDMSConnectionString.Text = "Data Source=gigasax;Initial Catalog=DMS5;Integrated Security=SSPI;";
            this.txtDMSConnectionString.TextChanged += new System.EventHandler(this.txtDMSConnectionString_TextChanged);
            // 
            // cboFileTypes
            // 
            this.cboFileTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboFileTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFileTypes.Location = new System.Drawing.Point(156, 165);
            this.cboFileTypes.Name = "cboFileTypes";
            this.cboFileTypes.Size = new System.Drawing.Size(876, 21);
            this.cboFileTypes.TabIndex = 3;
            this.cboFileTypes.SelectedIndexChanged += new System.EventHandler(this.cboFileTypes_SelectedIndexChanged);
            // 
            // lblOutputPath
            // 
            this.lblOutputPath.Location = new System.Drawing.Point(16, 24);
            this.lblOutputPath.Name = "lblOutputPath";
            this.lblOutputPath.Size = new System.Drawing.Size(120, 23);
            this.lblOutputPath.TabIndex = 4;
            this.lblOutputPath.Text = "Output Path";
            // 
            // lblPickList
            // 
            this.lblPickList.Location = new System.Drawing.Point(16, 64);
            this.lblPickList.Name = "lblPickList";
            this.lblPickList.Size = new System.Drawing.Size(120, 17);
            this.lblPickList.TabIndex = 5;
            this.lblPickList.Text = "Param File Pick List";
            // 
            // lblConnectionString
            // 
            this.lblConnectionString.Location = new System.Drawing.Point(16, 125);
            this.lblConnectionString.Name = "lblConnectionString";
            this.lblConnectionString.Size = new System.Drawing.Size(120, 23);
            this.lblConnectionString.TabIndex = 6;
            this.lblConnectionString.Text = "Connect String";
            // 
            // lblParamFileType
            // 
            this.lblParamFileType.Location = new System.Drawing.Point(16, 169);
            this.lblParamFileType.Name = "lblParamFileType";
            this.lblParamFileType.Size = new System.Drawing.Size(120, 23);
            this.lblParamFileType.TabIndex = 7;
            this.lblParamFileType.Text = "Param File Type";
            // 
            // cboAvailableParams
            // 
            this.cboAvailableParams.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboAvailableParams.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAvailableParams.Location = new System.Drawing.Point(156, 60);
            this.cboAvailableParams.Name = "cboAvailableParams";
            this.cboAvailableParams.Size = new System.Drawing.Size(876, 21);
            this.cboAvailableParams.Sorted = true;
            this.cboAvailableParams.TabIndex = 8;
            this.cboAvailableParams.SelectedIndexChanged += new System.EventHandler(this.cboAvailableParams_SelectedIndexChanged);
            // 
            // txtResults
            // 
            this.txtResults.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtResults.Location = new System.Drawing.Point(24, 253);
            this.txtResults.Multiline = true;
            this.txtResults.Name = "txtResults";
            this.txtResults.Size = new System.Drawing.Size(860, 84);
            this.txtResults.TabIndex = 9;
            // 
            // cmdDoIt
            // 
            this.cmdDoIt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdDoIt.Location = new System.Drawing.Point(912, 305);
            this.cmdDoIt.Name = "cmdDoIt";
            this.cmdDoIt.Size = new System.Drawing.Size(124, 32);
            this.cmdDoIt.TabIndex = 10;
            this.cmdDoIt.Text = "&Go";
            this.cmdDoIt.Click += new System.EventHandler(this.cmdDoIt_Click);
            // 
            // txtFASTAPath
            // 
            this.txtFASTAPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFASTAPath.Location = new System.Drawing.Point(160, 209);
            this.txtFASTAPath.Name = "txtFASTAPath";
            this.txtFASTAPath.Size = new System.Drawing.Size(872, 20);
            this.txtFASTAPath.TabIndex = 11;
            this.txtFASTAPath.Text = "C:\\DMS_Temp_Org\\bsa.fasta";
            this.txtFASTAPath.TextChanged += new System.EventHandler(this.txtFASTAPath_TextChanged);
            // 
            // txtDatasetID
            // 
            this.txtDatasetID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDatasetID.Location = new System.Drawing.Point(912, 279);
            this.txtDatasetID.Name = "txtDatasetID";
            this.txtDatasetID.Size = new System.Drawing.Size(120, 20);
            this.txtDatasetID.TabIndex = 12;
            this.txtDatasetID.Text = "101986";
            this.txtDatasetID.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblDatasetID
            // 
            this.lblDatasetID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDatasetID.AutoSize = true;
            this.lblDatasetID.Location = new System.Drawing.Point(912, 260);
            this.lblDatasetID.Name = "lblDatasetID";
            this.lblDatasetID.Size = new System.Drawing.Size(121, 15);
            this.lblDatasetID.TabIndex = 13;
            this.lblDatasetID.Text = "(Optional) Dataset ID";
            // 
            // txtParamFileName
            // 
            this.txtParamFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtParamFileName.Location = new System.Drawing.Point(156, 87);
            this.txtParamFileName.Name = "txtParamFileName";
            this.txtParamFileName.Size = new System.Drawing.Size(876, 20);
            this.txtParamFileName.TabIndex = 14;
            // 
            // lblParamFileName
            // 
            this.lblParamFileName.Location = new System.Drawing.Point(16, 87);
            this.lblParamFileName.Name = "lblParamFileName";
            this.lblParamFileName.Size = new System.Drawing.Size(120, 20);
            this.lblParamFileName.TabIndex = 15;
            this.lblParamFileName.Text = "Param File Name";
            // 
            // lblFASTAFilePath
            // 
            this.lblFASTAFilePath.Location = new System.Drawing.Point(16, 203);
            this.lblFASTAFilePath.Name = "lblFASTAFilePath";
            this.lblFASTAFilePath.Size = new System.Drawing.Size(120, 41);
            this.lblFASTAFilePath.TabIndex = 16;
            this.lblFASTAFilePath.Text = "FASTA File Path (only for SEQUEST)";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1052, 357);
            this.Controls.Add(this.lblFASTAFilePath);
            this.Controls.Add(this.lblParamFileName);
            this.Controls.Add(this.txtParamFileName);
            this.Controls.Add(this.lblDatasetID);
            this.Controls.Add(this.txtDatasetID);
            this.Controls.Add(this.txtFASTAPath);
            this.Controls.Add(this.txtResults);
            this.Controls.Add(this.txtDMSConnectionString);
            this.Controls.Add(this.txtOutputPath);
            this.Controls.Add(this.cmdDoIt);
            this.Controls.Add(this.cboAvailableParams);
            this.Controls.Add(this.lblParamFileType);
            this.Controls.Add(this.lblConnectionString);
            this.Controls.Add(this.lblPickList);
            this.Controls.Add(this.lblOutputPath);
            this.Controls.Add(this.cboFileTypes);
            this.Name = "Main";
            this.Text = "DMS Param File Generator";
            this.ResumeLayout(false);
            this.PerformLayout();

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

