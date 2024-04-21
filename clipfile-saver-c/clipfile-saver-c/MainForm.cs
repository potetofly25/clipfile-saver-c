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
            // フォルダ選択ダイアログを作成
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                // ダイアログを表示し、OKボタンが押された場合
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
                MessageBox.Show("フォルダがありません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                SaveImageFile();
            }
        }

        private void WatcherRadioButton_Click(object sender, EventArgs e)
        {
            // 監視切り替え
            WatcherRadioButton.Checked = !WatcherRadioButton.Checked;
            if (WatcherRadioButton.Checked)
            {
                WatcherRadioButton.BackColor = Color.Salmon;
                AddClipboardFormatListener(Handle);
                isHandling = true;
                LogLabel.Text = "監視開始";
            }
            else
            {
                WatcherRadioButton.BackColor = Color.Gray;
                if (isHandling)
                {
                    RemoveClipboardFormatListener(Handle);
                    isHandling = false;
                    LogLabel.Text = "監視終了";
                }
            }
        }

        // クリップボードのデータが変更された
        private void OnClipboardUpdate()
        {
            if (Clipboard.ContainsImage())
            {
                SaveImageFile();
            }
        }

        // フォルダ設定
        private void SetDirectory(string dirPath)
        {
            string selectedFolderPath = dirPath;
            // 選択されたフォルダ直下のフォルダ名を取得
            string[] subDirectories = Directory.GetDirectories(selectedFolderPath, "*", SearchOption.TopDirectoryOnly);
            // コンボボックスをクリアしてから、取得したフォルダ名を追加
            SubDirNameComboBox.Items.Clear();
            foreach (var subDirPath in subDirectories)
            {
                SubDirNameComboBox.Items.Add(System.IO.Path.GetFileName(subDirPath));
            }
            // 選択されたフォルダのパスをテキストボックスにセット
            PathTextBox.Text = selectedFolderPath;
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Clear();
            config.AppSettings.Settings.Add("DIRECTORY_PATH", selectedFolderPath);
            config.Save();
        }

        // 新しいファイルパス
        private (string newFilePath, string newFileNumber) NewFile(string dir, string fileName)
        {
            // ディレクトリのパスを受け取って最高値+1のファイル名を返す
            // 同名ファイルのナンバリング最高値を求める
            string[] dirList = Directory.GetFiles(dir);

            if (dirList.Length == 0)  // ディレクトリ内にファイルがない場合は00001
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
                    maxNum = Regex.Replace(maxNum, fileName, ""); // ファイル名を削除
                    string newFile = $"{int.Parse(maxNum) + 1:D5}"; // ファイル名に+1して５ケタでゼロパディングする
                    return (Path.Combine(dir, $"{fileName}{newFile}.jpg"), newFile);
                }
            }
            else
            {
                return (Path.Combine(dir, $"{fileName}00001.jpg"), "00001");
            }
        }

        // フォルダ設定
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

        // フォルダ設定
        private void CreateDirectory(string dirPath)
        {
            if (!System.IO.Directory.Exists(dirPath))
            {
                System.IO.Directory.CreateDirectory(dirPath);
            }
        }

        // 保存
        private void SaveImageFile()
        {
            // クリップボードからデータを取得
            if (Clipboard.ContainsImage())
            {
                // 画像を取得
                Image? image = Clipboard.GetImage();
                if (image != null)
                {
                    // 画像を指定されたパスに保存
                    string folderPath = GetSaveDirectoryPath();
                    CreateDirectory(folderPath);
                    var (newFilePath, newFileNumber) = NewFile(folderPath, FileNameTextBox.Text);
                    image.Save(newFilePath);
                    LogLabel.Text = $"画像保存: {System.IO.Path.GetFileName(newFilePath)}";
                }
                else
                {
                    LogLabel.Text = "クリップボードに画像がありません: null";
                }
            }
            else
            {
                MessageBox.Show("クリップボードに画像がありません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogLabel.Text = "クリップボードに画像がありません";
            }
        }

    }
}