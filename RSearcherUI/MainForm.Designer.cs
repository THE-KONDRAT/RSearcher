
namespace RSearcherUI
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tbStartFolder = new System.Windows.Forms.TextBox();
            this.lbStertFolder = new System.Windows.Forms.Label();
            this.btnOpenFolder = new System.Windows.Forms.Button();
            this.lbRegexMask = new System.Windows.Forms.Label();
            this.rtbRegexMask = new System.Windows.Forms.RichTextBox();
            this.lbStatus = new System.Windows.Forms.Label();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.FBD = new System.Windows.Forms.FolderBrowserDialog();
            this.statusTim = new System.Windows.Forms.Timer(this.components);
            this.treeView = new RSearcherUI.BufferedTreeView();
            this.cbDeleteSets = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // tbStartFolder
            // 
            this.tbStartFolder.Location = new System.Drawing.Point(59, 20);
            this.tbStartFolder.Name = "tbStartFolder";
            this.tbStartFolder.Size = new System.Drawing.Size(174, 20);
            this.tbStartFolder.TabIndex = 0;
            // 
            // lbStertFolder
            // 
            this.lbStertFolder.AutoSize = true;
            this.lbStertFolder.Location = new System.Drawing.Point(12, 27);
            this.lbStertFolder.Name = "lbStertFolder";
            this.lbStertFolder.Size = new System.Drawing.Size(41, 13);
            this.lbStertFolder.TabIndex = 1;
            this.lbStertFolder.Text = "Find in:";
            // 
            // btnOpenFolder
            // 
            this.btnOpenFolder.Location = new System.Drawing.Point(239, 18);
            this.btnOpenFolder.Name = "btnOpenFolder";
            this.btnOpenFolder.Size = new System.Drawing.Size(75, 23);
            this.btnOpenFolder.TabIndex = 2;
            this.btnOpenFolder.Text = "Open folder";
            this.btnOpenFolder.UseVisualStyleBackColor = true;
            // 
            // lbRegexMask
            // 
            this.lbRegexMask.AutoSize = true;
            this.lbRegexMask.Location = new System.Drawing.Point(12, 70);
            this.lbRegexMask.Name = "lbRegexMask";
            this.lbRegexMask.Size = new System.Drawing.Size(86, 13);
            this.lbRegexMask.TabIndex = 3;
            this.lbRegexMask.Text = "RegEx file mask:";
            // 
            // rtbRegexMask
            // 
            this.rtbRegexMask.Location = new System.Drawing.Point(15, 97);
            this.rtbRegexMask.Name = "rtbRegexMask";
            this.rtbRegexMask.Size = new System.Drawing.Size(299, 252);
            this.rtbRegexMask.TabIndex = 4;
            this.rtbRegexMask.Text = "";
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Location = new System.Drawing.Point(12, 393);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(77, 26);
            this.lbStatus.TabIndex = 5;
            this.lbStatus.Text = "Time left: 0000\r\nFiles found: ";
            // 
            // btnStartStop
            // 
            this.btnStartStop.Location = new System.Drawing.Point(15, 355);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(75, 23);
            this.btnStartStop.TabIndex = 6;
            this.btnStartStop.Text = "Start/Stop";
            this.btnStartStop.UseVisualStyleBackColor = true;
            // 
            // statusTim
            // 
            this.statusTim.Tick += new System.EventHandler(this.statusTim_Tick);
            // 
            // treeView
            // 
            this.treeView.Location = new System.Drawing.Point(320, 18);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(468, 420);
            this.treeView.TabIndex = 7;
            // 
            // cbDeleteSets
            // 
            this.cbDeleteSets.AutoSize = true;
            this.cbDeleteSets.Location = new System.Drawing.Point(166, 421);
            this.cbDeleteSets.Name = "cbDeleteSets";
            this.cbDeleteSets.Size = new System.Drawing.Size(148, 17);
            this.cbDeleteSets.TabIndex = 8;
            this.cbDeleteSets.Text = "Delete settings after close";
            this.cbDeleteSets.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cbDeleteSets);
            this.Controls.Add(this.treeView);
            this.Controls.Add(this.btnStartStop);
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.rtbRegexMask);
            this.Controls.Add(this.lbRegexMask);
            this.Controls.Add(this.btnOpenFolder);
            this.Controls.Add(this.lbStertFolder);
            this.Controls.Add(this.tbStartFolder);
            this.Name = "MainForm";
            this.Text = "RSearcher";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbStartFolder;
        private System.Windows.Forms.Label lbStertFolder;
        private System.Windows.Forms.Button btnOpenFolder;
        private System.Windows.Forms.Label lbRegexMask;
        private System.Windows.Forms.RichTextBox rtbRegexMask;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.FolderBrowserDialog FBD;
        private System.Windows.Forms.Timer statusTim;
        private BufferedTreeView treeView;
        private System.Windows.Forms.CheckBox cbDeleteSets;
    }
}

