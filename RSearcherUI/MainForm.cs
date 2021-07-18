using FileSearcher;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RSearcherUI
{
    public partial class MainForm : Form
    {
        private bool _searching = false;
        public bool Searching
        {
            get { return _searching; }
            set
            {
                _searching = value;
                if (_searching)
                {
                    UI_Logic.SetObjProp(this, btnStartStop, "Text", "Stop");
                }
                else
                {
                    UI_Logic.SetObjProp(this, btnStartStop, "Text", "Start");
                }
                if (tbStartFolder != null)
                {
                    UI_Logic.SetObjProp(this, btnStartStop, "ReadOnly", value);
                }
            }
        }

        //private string _startFolder;
        public String StartFolder
        {
            get
            {
                return UI_Logic.TrimString((string)UI_Logic.GetObjProp(this, tbStartFolder, "Text"));
            }
            set
            {
                if (!Searching)
                {
                    UI_Logic.SetObjProp(this, tbStartFolder, "Text", UI_Logic.TrimString(value));
                }   
            }
        }
        public String RegexStr
        {
            get
            {
                return UI_Logic.TrimString((string)UI_Logic.GetObjProp(this, rtbRegexMask, "Text"));
            }
            set
            {
                UI_Logic.SetObjProp(this, rtbRegexMask, "Text", UI_Logic.TrimString(value));
            }
        }
        private uint filesCounter = 0;
        FileSearcher.UserParams userParams;

        Stopwatch swElapsed;
        FileSearcher.FileSearcher fs;
        public MainForm()
        {
            InitializeComponent();
            
            CreateFS();

            cbDeleteSets.Checked = false;
            swElapsed = new Stopwatch();
            userParams = new UserParams();
            LoadVariables();
            UpdateStatus();
            foreach (Control c in this.Controls)
            {
                if (c.GetType() == typeof(Button)) c.Click += Button_Click;
            }
            this.FormClosing += MainForm_Closing;
            Searching = false;
        }

        private void Button_Click(object sender, EventArgs e)
        {
            switch (((Control)sender).Name)
            {
                case "btnOpenFolder":
                    OpenFolder();
                    break;
                case "btnStartStop":
                    StartSearchAsync();
                    break;
            }
        }

        private void OpenFolder()
        {
            if ((DialogResult)FBD.ShowDialog() == DialogResult.OK) StartFolder = FBD.SelectedPath; 
        }

        private async void StartSearchAsync()
        {   
            if (!Searching)
            {
                fs.Cancel = false;
                try
                {
                    StartFolder = FileSearcher.FileSearcher.GetFullPath(StartFolder);
                    if (string.IsNullOrWhiteSpace(RegexStr))
                    {
                        throw new Exception("Regex field must not be empty");
                    }
                    Searching = true;

                    FileSearcher.FileSearcher.ValidatePath(StartFolder);

                    treeView.Nodes.Clear();

                    filesCounter = 0;

                    swElapsed.Restart();
                    statusTim.Interval = 1;
                    statusTim.Start();

                    SaveVariables();
                    //fs.StartSearch(StartFolder, RegexStr);
                    await Task.Run(() => fs.StartSearch(StartFolder, RegexStr));
                    //await Task t = Task.Run(() => fs.StartSearch(StartFolder, RegexStr));
                    //Task t = new Task()
                }
                catch (Exception e)
                {
                    Searching = false;
                    FinishSearch();
                    ShowException(e);
                }
            }
            else
            {
                if (fs != null) fs.Cancel = true;
            }
        }

        private void FinishSearch()
        {
            swElapsed.Stop();
            statusTim.Stop();
            UpdateStatus();
            Searching = false;
        }

        private void CancelSearch()
        {
            if (Searching)
            {
                FinishSearch();
                MessageBox.Show("Cancelled by user");
            }
        }

        public void CreateFS()
        {
            fs = new FileSearcher.FileSearcher();
            fs.OnSetFile += SetFile;
            fs.OnFinish += FinishSearch;
            fs.OnCancel += CancelSearch;
        }

        private void SetFile(String FilePath)
        {
            filesCounter++;
            UI_Logic.SetNewItem(this, treeView, StartFolder, FilePath);
            UpdateStatus();
        }

        #region Status (time left, files counter)
        private void UpdateStatus()
        {
            if (treeView.InvokeRequired)
            {
                this.Invoke(
                    new MethodInvoker(
                        delegate ()
                            {
                                lbStatus.Text = UI_Logic.StatusString(swElapsed, filesCounter);
                            }
                        )
                    );
            }
            else lbStatus.Text = UI_Logic.StatusString(swElapsed, filesCounter);
        }

        private void statusTim_Tick(object sender, EventArgs e)
        {
            if (swElapsed.Elapsed.Minutes > 1) statusTim.Interval = 1000;
            UpdateStatus();
        }
        #endregion

        #region Load/Save params
        private void LoadVariables()
        {
            userParams.LoadParams();
            StartFolder = userParams.StartFolder;
            RegexStr = userParams.RegexMask;
            userParams.SaveParams();
        }

        private void SaveVariables()
        {
            userParams.StartFolder = StartFolder;
            userParams.RegexMask = RegexStr;
            userParams.SaveParams();
        }

        private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (cbDeleteSets.Checked)
            {
                UserParams.DeleteSettingsFile();
            }
        }
        #endregion

        private void ShowException(Exception e)
        {
            MessageBox.Show(e.Message.ToString(), "Error");
        }
    }
}
