using System.Configuration;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace clipfile_saver_c
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private const int WM_CLIPBOARDUPDATE = 0x31D;

        [DllImport("user32.dll", SetLastError = true)]
        private extern static void AddClipboardFormatListener(IntPtr hwnd);

        [DllImport("user32.dll", SetLastError = true)]
        private extern static void RemoveClipboardFormatListener(IntPtr hwnd);

        bool isHandling = false;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_CLIPBOARDUPDATE)
            {
                OnClipboardUpdate();
                m.Result = IntPtr.Zero;
            }
            else
                base.WndProc(ref m);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            WatcherRadioButton.BackColor = Color.Gray;

            string? selectedFolderPath = "";
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            foreach (var key in config.AppSettings.Settings.AllKeys)
            {
                switch (key)
                {
                    case "DIRECTORY_PATH":
                        selectedFolderPath = config.AppSettings.Settings["DIRECTORY_PATH"].Value;
                        break;
                }
            }

            if (selectedFolderPath != null && selectedFolderPath != "" && System.IO.Directory.Exists(selectedFolderPath))
            {
                SetDirectory(selectedFolderPath);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isHandling)
            {
                RemoveClipboardFormatListener(Handle);
            }
        }

        private void SelectButton_Click(object sender, EventArgs e)
        {
            // �t�H���_�I���_�C�A���O���쐬
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                // �_�C�A���O��\�����AOK�{�^���������ꂽ�ꍇ
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    SetDirectory(folderDialog.SelectedPath);
                }
            }
        }

        private void MainDirOpenButton_Click(object sender, EventArgs e)
        {
            string selectedFolderPath = PathTextBox.Text;
            if (System.IO.Directory.Exists(selectedFolderPath))
            {
                Process.Start("explorer.exe", selectedFolderPath);
            }
        }

        private void SubDirOpenButton_Click(object sender, EventArgs e)
        {
            string selectedFolderPath = PathTextBox.Text;
            string subName = SubDirNameComboBox.Text;
            string folderName = selectedFolderPath;
            if (subName != "")
            {
                folderName = System.IO.Path.Combine(folderName, subName);
            }
            if (System.IO.Directory.Exists(folderName))
            {
                Process.Start("explorer.exe", folderName);
            }
        }

        private void ManualSaveButton_Click(object sender, EventArgs e)
        {
            string selectedFolderPath = PathTextBox.Text;
            if (selectedFolderPath == "" || !System.IO.Directory.Exists(selectedFolderPath))
            {
                MessageBox.Show("�t�H���_������܂���B", "�G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                SaveImageFile();
            }
        }

        private void WatcherRadioButton_Click(object sender, EventArgs e)
        {
            // �Ď��؂�ւ�
            WatcherRadioButton.Checked = !WatcherRadioButton.Checked;
            if (WatcherRadioButton.Checked)
            {
                WatcherRadioButton.BackColor = Color.Salmon;
                AddClipboardFormatListener(Handle);
                isHandling = true;
                LogLabel.Text = "�Ď��J�n";
            }
            else
            {
                WatcherRadioButton.BackColor = Color.Gray;
                if (isHandling)
                {
                    RemoveClipboardFormatListener(Handle);
                    isHandling = false;
                    LogLabel.Text = "�Ď��I��";
                }
            }
        }

        // �N���b�v�{�[�h�̃f�[�^���ύX���ꂽ
        private void OnClipboardUpdate()
        {
            if (Clipboard.ContainsImage())
            {
                SaveImageFile();
            }
        }

        // �t�H���_�ݒ�
        private void SetDirectory(string dirPath)
        {
            string selectedFolderPath = dirPath;
            // �I�����ꂽ�t�H���_�����̃t�H���_�����擾
            string[] subDirectories = Directory.GetDirectories(selectedFolderPath, "*", SearchOption.TopDirectoryOnly);
            // �R���{�{�b�N�X���N���A���Ă���A�擾�����t�H���_����ǉ�
            SubDirNameComboBox.Items.Clear();
            foreach (var subDirPath in subDirectories)
            {
                SubDirNameComboBox.Items.Add(System.IO.Path.GetFileName(subDirPath));
            }
            // �I�����ꂽ�t�H���_�̃p�X���e�L�X�g�{�b�N�X�ɃZ�b�g
            PathTextBox.Text = selectedFolderPath;
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Clear();
            config.AppSettings.Settings.Add("DIRECTORY_PATH", selectedFolderPath);
            config.Save();
        }

        // �V�����t�@�C���p�X
        private (string newFilePath, string newFileNumber) NewFile(string dir, string fileName)
        {
            // �f�B���N�g���̃p�X���󂯎���čō��l+1�̃t�@�C������Ԃ�
            // �����t�@�C���̃i���o�����O�ō��l�����߂�
            string[] dirList = Directory.GetFiles(dir);

            if (dirList.Length == 0)  // �f�B���N�g�����Ƀt�@�C�����Ȃ��ꍇ��00001
            {
                return (Path.Combine(dir, $"{fileName}00001.jpg"), "00001");
            }

            List<string> matchingFiles = dirList.Where(s => Path.GetFileNameWithoutExtension(s).StartsWith(fileName)).ToList();
            if (matchingFiles.Any() && matchingFiles != null)
            {
                string? maxFile = matchingFiles.Max();
                if (maxFile == null)
                {
                    return (Path.Combine(dir, $"{fileName}00001.jpg"), "00001");
                }
                else
                {
                    string maxNum = System.IO.Path.GetFileNameWithoutExtension(maxFile);
                    maxNum = Regex.Replace(maxNum, fileName, ""); // �t�@�C�������폜
                    string newFile = $"{int.Parse(maxNum) + 1:D5}"; // �t�@�C������+1���ĂT�P�^�Ń[���p�f�B���O����
                    return (Path.Combine(dir, $"{fileName}{newFile}.jpg"), newFile);
                }
            }
            else
            {
                return (Path.Combine(dir, $"{fileName}00001.jpg"), "00001");
            }
        }

        // �t�H���_�ݒ�
        private string GetSaveDirectoryPath()
        {
            string selectedFolderPath = PathTextBox.Text;
            string subName = SubDirNameComboBox.Text;
            string folderName = selectedFolderPath;
            if (subName != "")
            {
                folderName = System.IO.Path.Combine(folderName, subName);
            }
            return folderName;
        }

        // �t�H���_�ݒ�
        private void CreateDirectory(string dirPath)
        {
            if (!System.IO.Directory.Exists(dirPath))
            {
                System.IO.Directory.CreateDirectory(dirPath);
            }
        }

        // �ۑ�
        private void SaveImageFile()
        {
            // �N���b�v�{�[�h����f�[�^���擾
            if (Clipboard.ContainsImage())
            {
                // �摜���擾
                Image? image = Clipboard.GetImage();
                if (image != null)
                {
                    // �摜���w�肳�ꂽ�p�X�ɕۑ�
                    string folderPath = GetSaveDirectoryPath();
                    CreateDirectory(folderPath);
                    var (newFilePath, newFileNumber) = NewFile(folderPath, FileNameTextBox.Text);
                    image.Save(newFilePath);
                    LogLabel.Text = $"�摜�ۑ�: {System.IO.Path.GetFileName(newFilePath)}";
                }
                else
                {
                    LogLabel.Text = "�N���b�v�{�[�h�ɉ摜������܂���: null";
                }
            }
            else
            {
                MessageBox.Show("�N���b�v�{�[�h�ɉ摜������܂���B", "�G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogLabel.Text = "�N���b�v�{�[�h�ɉ摜������܂���";
            }
        }

    }
}