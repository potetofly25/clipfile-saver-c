namespace clipfile_saver_c
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            SelectButton = new Button();
            label1 = new Label();
            PathTextBox = new TextBox();
            label2 = new Label();
            SubDirNameComboBox = new ComboBox();
            panel1 = new Panel();
            PathOpenButton = new Button();
            SubDirOpenButton = new Button();
            ManualSaveButton = new Button();
            WatcherRadioButton = new RadioButton();
            LogLabel = new Label();
            FileNameTextBox = new TextBox();
            label3 = new Label();
            SuspendLayout();
            // 
            // SelectButton
            // 
            SelectButton.Location = new Point(94, 9);
            SelectButton.Name = "SelectButton";
            SelectButton.Size = new Size(75, 26);
            SelectButton.TabIndex = 0;
            SelectButton.Text = "選択";
            SelectButton.UseVisualStyleBackColor = true;
            SelectButton.Click += SelectButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(15, 46);
            label1.Name = "label1";
            label1.Size = new Size(73, 15);
            label1.TabIndex = 1;
            label1.Text = "サブフォルダ：";
            // 
            // PathTextBox
            // 
            PathTextBox.Location = new Point(175, 10);
            PathTextBox.Name = "PathTextBox";
            PathTextBox.Size = new Size(281, 23);
            PathTextBox.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(34, 14);
            label2.Name = "label2";
            label2.Size = new Size(54, 15);
            label2.TabIndex = 4;
            label2.Text = "フォルダ：";
            // 
            // SubDirNameComboBox
            // 
            SubDirNameComboBox.FormattingEnabled = true;
            SubDirNameComboBox.Location = new Point(94, 42);
            SubDirNameComboBox.Name = "SubDirNameComboBox";
            SubDirNameComboBox.Size = new Size(362, 23);
            SubDirNameComboBox.TabIndex = 5;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Location = new Point(13, 106);
            panel1.Name = "panel1";
            panel1.Size = new Size(524, 3);
            panel1.TabIndex = 6;
            // 
            // PathOpenButton
            // 
            PathOpenButton.Location = new Point(462, 9);
            PathOpenButton.Name = "PathOpenButton";
            PathOpenButton.Size = new Size(75, 26);
            PathOpenButton.TabIndex = 7;
            PathOpenButton.Text = "開く";
            PathOpenButton.UseVisualStyleBackColor = true;
            PathOpenButton.Click += MainDirOpenButton_Click;
            // 
            // SubDirOpenButton
            // 
            SubDirOpenButton.Location = new Point(462, 40);
            SubDirOpenButton.Name = "SubDirOpenButton";
            SubDirOpenButton.Size = new Size(75, 26);
            SubDirOpenButton.TabIndex = 8;
            SubDirOpenButton.Text = "開く";
            SubDirOpenButton.UseVisualStyleBackColor = true;
            SubDirOpenButton.Click += SubDirOpenButton_Click;
            // 
            // ManualSaveButton
            // 
            ManualSaveButton.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            ManualSaveButton.Location = new Point(94, 115);
            ManualSaveButton.Name = "ManualSaveButton";
            ManualSaveButton.Size = new Size(168, 52);
            ManualSaveButton.TabIndex = 9;
            ManualSaveButton.Text = "手動保存";
            ManualSaveButton.UseVisualStyleBackColor = true;
            ManualSaveButton.Click += ManualSaveButton_Click;
            // 
            // WatcherRadioButton
            // 
            WatcherRadioButton.Appearance = Appearance.Button;
            WatcherRadioButton.AutoCheck = false;
            WatcherRadioButton.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            WatcherRadioButton.Location = new Point(291, 115);
            WatcherRadioButton.Name = "WatcherRadioButton";
            WatcherRadioButton.Size = new Size(165, 50);
            WatcherRadioButton.TabIndex = 10;
            WatcherRadioButton.Text = "クリップボード監視";
            WatcherRadioButton.TextAlign = ContentAlignment.MiddleCenter;
            WatcherRadioButton.UseVisualStyleBackColor = true;
            WatcherRadioButton.Click += WatcherRadioButton_Click;
            // 
            // LogLabel
            // 
            LogLabel.Location = new Point(15, 175);
            LogLabel.Name = "LogLabel";
            LogLabel.Size = new Size(522, 15);
            LogLabel.TabIndex = 11;
            LogLabel.Text = "ログ";
            // 
            // FileNameTextBox
            // 
            FileNameTextBox.Location = new Point(94, 74);
            FileNameTextBox.Name = "FileNameTextBox";
            FileNameTextBox.Size = new Size(362, 23);
            FileNameTextBox.TabIndex = 12;
            FileNameTextBox.Text = "PIC_";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(23, 77);
            label3.Name = "label3";
            label3.Size = new Size(65, 15);
            label3.TabIndex = 13;
            label3.Text = "ファイル名：";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(549, 201);
            Controls.Add(label3);
            Controls.Add(FileNameTextBox);
            Controls.Add(LogLabel);
            Controls.Add(WatcherRadioButton);
            Controls.Add(ManualSaveButton);
            Controls.Add(SubDirOpenButton);
            Controls.Add(PathOpenButton);
            Controls.Add(panel1);
            Controls.Add(SubDirNameComboBox);
            Controls.Add(label2);
            Controls.Add(PathTextBox);
            Controls.Add(label1);
            Controls.Add(SelectButton);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "クリップボード画像監視保存";
            TopMost = true;
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button SelectButton;
        private Label label1;
        private TextBox PathTextBox;
        private Label label2;
        private ComboBox SubDirNameComboBox;
        private Panel panel1;
        private Button PathOpenButton;
        private Button SubDirOpenButton;
        private Button ManualSaveButton;
        private RadioButton WatcherRadioButton;
        private Label LogLabel;
        private TextBox FileNameTextBox;
        private Label label3;
    }
}