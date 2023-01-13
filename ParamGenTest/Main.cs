using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using ParamFileGenerator;
using ParamFileGenerator.MakeParams;
using PRISMDatabaseUtils;

namespace ParamGenTest
{
    public partial class Main : Form
    {
        // Ignore Spelling: SEQUEST

        public Main() : base()
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            // Add any initialization after the InitializeComponent() call

            mSupportedParamFileTypeIDs = new SortedSet<int>()
            {
                1000,   // SEQUEST
                1008,   // X_Tandem
                1018,   // MSGFPlus
                1019,   // MSAlign
                1022,   // MSAlignHistone
                1025,   // MSPathFinder
                1032,   // TopPIC
                1033,   // MSFragger
                1034    // MaxQuant
            };

            mDMSConnectString = txtDMSConnectionString.Text;
            mOutputPath = txtOutputPath.Text;
            mFASTAPath = txtFASTAPath.Text;
            mDMS = new MakeParameterFile();

            LoadParamFileTypes();
        }

        private string mOutputPath;
        private string mDMSConnectString;
        private IGenerateFile.ParamFileType mParamFileType;
        private int mParamTypeID;
        private string mParamFileName;
        private string mFASTAPath;
        private DataTable mAvailableParamFiles;

        private string mCurrentConnectionString;
        private IDBTools mCurrentDBTools;

        private readonly SortedSet<int> mSupportedParamFileTypeIDs;

        private readonly IGenerateFile mDMS;

        private void cmdDoIt_Click(object sender, EventArgs e)
        {
            //if (mDMS is null)
            //{
            //    mDMS = new ParamFileGenerator.MakeParams.MakeParameterFile;
            //}

            if (txtParamFileName.TextLength > 0)
            {
                mParamFileName = txtParamFileName.Text;
            }
            else
            {
                PopulateParamFileNameTextBox();
            }

            int datasetID;
            if (!string.IsNullOrEmpty(txtDatasetID.Text))
            {
                datasetID = Params.SafeCastInt(txtDatasetID.Text);
            }
            else
            {
                datasetID = -1;
            }
            bool success = mDMS.MakeFile(mParamFileName, mParamFileType, mFASTAPath, mOutputPath, mDMSConnectString, datasetID);
            if (success)
            {
                txtResults.Text = "File successfully written to: " + Path.Combine(mOutputPath, mParamFileName);
            }
            else
            {
                txtResults.Text = "Error!";
                if (!string.IsNullOrWhiteSpace(mDMS.LastError))
                {
                    txtResults.Text += " " + mDMS.LastError;
                }
            }
        }

        private void txtOutputPath_TextChanged(object sender, EventArgs e)
        {
            mOutputPath = txtOutputPath.Text;
        }

        private void cboAvailableParams_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateParamFileNameTextBox();
        }

        private void txtDMSConnectionString_TextChanged(object sender, EventArgs e)
        {
            mDMSConnectString = txtDMSConnectionString.Text;
        }

        private void cboFileTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            mParamTypeID = Params.SafeCastInt(cboFileTypes.SelectedValue);

            switch (mParamTypeID)
            {
                case 1000:
                    mParamFileType = IGenerateFile.ParamFileType.BioWorks_32;
                    break;
                case 1008:
                    mParamFileType = IGenerateFile.ParamFileType.X_Tandem;
                    break;
                case 1018:
                    mParamFileType = IGenerateFile.ParamFileType.MSGFPlus;
                    break;
                case 1019:
                    mParamFileType = IGenerateFile.ParamFileType.MSAlign;
                    break;
                case 1022:
                    mParamFileType = IGenerateFile.ParamFileType.MSAlignHistone;
                    break;
                case 1025:
                    mParamFileType = IGenerateFile.ParamFileType.MSPathFinder;
                    break;
                case 1032:
                    mParamFileType = IGenerateFile.ParamFileType.TopPIC;
                    break;
                case 1033:
                    mParamFileType = IGenerateFile.ParamFileType.MSFragger;
                    break;
                case 1034:
                    mParamFileType = IGenerateFile.ParamFileType.MaxQuant;
                    break;
            }

            LoadParamNames(mParamTypeID);
        }

        private void ValidateDBTools()
        {
            if (mCurrentDBTools is null || string.IsNullOrWhiteSpace(mCurrentConnectionString) || !mCurrentConnectionString.Equals(mDMSConnectString))
            {
                mCurrentConnectionString = string.Copy(mDMSConnectString);

                string connectionStringToUse = DbToolsFactory.AddApplicationNameToConnectionString(mCurrentConnectionString, "ParameterFileEditor_ParamGenTest");

                mCurrentDBTools = DbToolsFactory.GetDBTools(connectionStringToUse);
            }
        }

        private void LoadParamNames(int TypeID = 0)
        {
            if (mAvailableParamFiles is null)
            {
                ValidateDBTools();
                mAvailableParamFiles = mDMS.GetAvailableParamSetTable(mCurrentDBTools);
            }

            DataRow[] foundRows;

            if (TypeID > 1)
            {
                foundRows = mAvailableParamFiles.Select("Type_ID = " + TypeID.ToString());
            }
            else
            {
                foundRows = mAvailableParamFiles.Select();
            }

            cboAvailableParams.BeginUpdate();
            cboAvailableParams.Items.Clear();
            txtParamFileName.Text = string.Empty;

            foreach (DataRow dr in foundRows)
                cboAvailableParams.Items.Add(new ParamFileEntry(Params.SafeCastInt(dr["ID"]), dr["Filename"].ToString()));

            cboAvailableParams.DisplayMember = "Description";
            cboAvailableParams.ValueMember = "Value";

            if (cboAvailableParams.Items.Count > 0)
            {
                cboAvailableParams.SelectedIndex = 0;
            }

            cboAvailableParams.EndUpdate();
        }

        private void LoadParamFileTypes()
        {
            ValidateDBTools();

            var paramFileTypes = mDMS.GetAvailableParamFileTypes(mCurrentDBTools);

            var supportedParamFileTypes = paramFileTypes.Clone();

            foreach (DataRow currentRow in paramFileTypes.Rows)
            {
                int paramFileTypeID = int.Parse(currentRow[0].ToString());

                if (!mSupportedParamFileTypeIDs.Contains(paramFileTypeID))
                {
                    continue;
                }

                supportedParamFileTypes.Rows.Add(currentRow.ItemArray);
            }

            cboFileTypes.DisplayMember = "Type";
            cboFileTypes.ValueMember = "ID";
            cboFileTypes.DataSource = supportedParamFileTypes;
            cboFileTypes.Text = "MSGFDB";

            //cboFileTypes.DataSource = System.Enum.GetValues(GetType(ParamFileGenerator.MakeParams.IGenerateFile.ParamFileType));
        }

        private void PopulateParamFileNameTextBox()
        {
            var entry = (ParamFileEntry)cboAvailableParams.SelectedItem;

            txtParamFileName.Text = entry.Description;
            mParamFileName = txtParamFileName.Text;
        }

        private void txtFASTAPath_TextChanged(object sender, EventArgs e)
        {
            mFASTAPath = txtFASTAPath.Text;
        }

        public class ParamFileEntry
        {
            public ParamFileEntry(int value, string description)
            {
                Value = value;
                Description = description;
            }

            public int Value { get; set; }

            public string Description { get; set; }
        }
    }
}