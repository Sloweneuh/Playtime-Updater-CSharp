using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using Microsoft.Win32;
using System.Data;
using System.IO;

namespace PlaytimeUpdater
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.updateButton = new System.Windows.Forms.Button();
            this.fileSelectionGroup = new System.Windows.Forms.GroupBox();
            this.selectFile = new System.Windows.Forms.Button();
            this.filePathLabel = new Label();
            this.SuspendLayout();

            // updateButton
            this.updateButton.Location = new System.Drawing.Point(100, 200);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(200, 50);
            this.updateButton.TabIndex = 0;
            this.updateButton.Text = "Update Playtime";
            this.updateButton.UseVisualStyleBackColor = true;
            this.updateButton.Click += new System.EventHandler(this.UpdateButton_Click);
            
            // filePathLabel
            this.filePathLabel.Location = new System.Drawing.Point(10, 20);
            this.filePathLabel.Name = "filePath";
            this.filePathLabel.Text = "File Path: " + file_path;
            Size textSize = TextRenderer.MeasureText(filePathLabel.Text, filePathLabel.Font);
            this.filePathLabel.Size = new System.Drawing.Size(textSize.Width, textSize.Height);
            
            // selectFile
            this.selectFile.Location = new System.Drawing.Point(textSize.Width + 10, 20);
            this.selectFile.Name = "selectFile";
            this.selectFile.Size = new System.Drawing.Size(50, 30);
            this.selectFile.TabIndex = 1;
            this.selectFile.Text = "...";
            this.selectFile.UseVisualStyleBackColor = true;
            
            this.selectFile.Click += new System.EventHandler(this.SelectFile);

            // fileSelectionGroup
            this.fileSelectionGroup.Location = new System.Drawing.Point(100, 250);
            this.fileSelectionGroup.Name = "fileSelectionGroup";
            this.fileSelectionGroup.Size = new System.Drawing.Size(filePathLabel.Size.Width + selectFile.Size.Width + 30, selectFile.Size.Height + 30);

            this.fileSelectionGroup.Controls.Add(this.filePathLabel);
            this.fileSelectionGroup.Controls.Add(this.selectFile);

            // MainForm
            this.ClientSize = new System.Drawing.Size(this.fileSelectionGroup.Size.Width+10, 300);
            this.Controls.Add(this.updateButton);
            this.Controls.Add(this.fileSelectionGroup);
            this.Name = "MainForm";
            this.Text = "Playtime Updater";
            this.ResumeLayout(false);

            // Initialize group boxes
            InitializeGroupBoxes();

            // Set visibility and position of group boxes based on gameList
            SetGroupBoxVisibilityAndPosition();

            GetPlaytimes();
        }

        private void InitializeGroupBoxes()
        {
            genshinGroup = CreateGroupBox("Genshin Impact", genshin_registry_path, file_path, 0, out genshinRegistryRadioButton, out genshinTextRadioButton);
            starrailGroup = CreateGroupBox("Star Rail", starrail_registry_path, file_path, 1, out starrailRegistryRadioButton, out starrailTextRadioButton);
            zzzGroup = CreateGroupBox("Zenless Zone Zero", zzz_registry_path, file_path, 2, out zzzRegistryRadioButton, out zzzTextRadioButton);
            honkaiGroup = CreateGroupBox("Honkai Impact 3rd", honkai_registry_path, file_path, 3, out honkaiRegistryRadioButton, out honkaiTextRadioButton);

            this.Controls.Add(genshinGroup);
            this.Controls.Add(starrailGroup);
            this.Controls.Add(zzzGroup);
            this.Controls.Add(honkaiGroup);
        }

        private GroupBox CreateGroupBox(string gameName, string registryPath, string filePath, int line, out RadioButton registryRadioButton, out RadioButton textRadioButton)
        {
            GroupBox groupBox = new GroupBox();
            groupBox.Name = gameName;
            groupBox.Text = gameName;
            groupBox.Size = new Size(250, 110);

            registryRadioButton = new RadioButton();
            registryRadioButton.Location = new Point(10, 30);
            registryRadioButton.Text = RegistryHelper.GetRegistryPlaytimeDisplay(registryPath);
            Size registryTextSize = MeasureTextSize(registryRadioButton.Text, registryRadioButton.Font);
            registryRadioButton.Size = new Size(registryTextSize.Width + 100, registryTextSize.Height + 10);
            registryRadioButton.UseVisualStyleBackColor = true;
            registryRadioButton.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);

            textRadioButton = new RadioButton();
            textRadioButton.Location = new Point(10, 70);
            textRadioButton.Text = FileHelper.GetTextFilePlaytimeDisplay(line, filePath);
            Size textSize = MeasureTextSize(textRadioButton.Text, textRadioButton.Font);
            textRadioButton.Size = new Size(textSize.Width + 100, textSize.Height + 10);
            textRadioButton.UseVisualStyleBackColor = true;
            textRadioButton.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);
            if (registryTextSize.Width > textSize.Width)
            {
                groupBox.Size = new Size(registryRadioButton.Size.Width + 120, 110);
            }
            else
            {
                groupBox.Size = new Size(textRadioButton.Size.Width + 120, 110);
            }

            groupBox.Controls.Add(registryRadioButton);
            groupBox.Controls.Add(textRadioButton);

            return groupBox;
        }

        private void SetGroupBoxVisibilityAndPosition()
        {
            int yOffset = 50; // Initial Y offset
            int ySpacing = 110; // Spacing between group boxes

            // Set visibility and position for Genshin Impact
            if (gameList.Any(game => game.Item1 == "Genshin Impact"))
            {
                genshinGroup.Visible = true;
                genshinGroup.Location = new System.Drawing.Point(100, yOffset);
                yOffset += ySpacing;
            }
            else
            {
                genshinGroup.Visible = false;
            }

            // Set visibility and position for Star Rail
            if (gameList.Any(game => game.Item1 == "Star Rail"))
            {
                starrailGroup.Visible = true;
                starrailGroup.Location = new System.Drawing.Point(100, yOffset);
                yOffset += ySpacing;
            }
            else
            {
                starrailGroup.Visible = false;
            }

            // Set visibility and position for Zenless Zone Zero
            if (gameList.Any(game => game.Item1 == "Zenless Zone Zero"))
            {
                zzzGroup.Visible = true;
                zzzGroup.Location = new System.Drawing.Point(100, yOffset);
                yOffset += ySpacing;
            }
            else
            {
                zzzGroup.Visible = false;
            }

            // Set visibility and position for Honkai Impact 3rd
            if (gameList.Any(game => game.Item1 == "Honkai Impact 3rd"))
            {
                honkaiGroup.Visible = true;
                honkaiGroup.Location = new System.Drawing.Point(100, yOffset);
                yOffset += ySpacing;
            }
            else
            {
                honkaiGroup.Visible = false;
            }

            UpdateRadioButtonsAndGroups();

            // Set position for Update Button
            updateButton.Location = new System.Drawing.Point(100, yOffset+10);
            yOffset += updateButton.Height + 10;

            // Set position for File Selection Group
            fileSelectionGroup.Location = new System.Drawing.Point(100, yOffset);

            // Set form height based on number of visible group boxes
            this.ClientSize = new System.Drawing.Size(fileSelectionGroup.Size.Width+200, yOffset + 100);
        }

        void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            string selected = "";

            if (sender is not RadioButton rb)
            {
                MessageBox.Show("Sender is not a RadioButton");
                return;
            }

            // Ensure that the RadioButton.Checked property
            // changed to true.
            if (rb.Checked)
            {
                string rbtext = rb.Text.Split(' ')[0];
                if (rbtext == "Registry")
                {
                    selected = "R";
                }
                if (rbtext == "File")
                {
                    selected = "T";
                }
                
                if (rb.Parent.Name == "Genshin Impact")
                {
                    genshin_selected = selected;
                }
                else if (rb.Parent.Name == "Star Rail")
                {
                    starrail_selected = selected;
                }
                else if (rb.Parent.Name == "Zenless Zone Zero")
                {
                    zzz_selected = selected;
                }
                else if (rb.Parent.Name == "Honkai Impact 3rd")
                {
                    honkai_selected = selected;
                }
            }
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            if (genshin_selected == "R")
            {
                FileHelper.SavePlaytimeToTextFile(genshin_registry_playtime, 0, file_path);
            }
            else if (genshin_selected == "T")
            {
                RegistryHelper.SavePlaytimeToRegistry(genshin_registry_path, genshin_text_playtime);
            }

            if (starrail_selected == "R")
            {
                FileHelper.SavePlaytimeToTextFile(starrail_registry_playtime, 1, file_path);
            }
            else if (starrail_selected == "T")
            {
                RegistryHelper.SavePlaytimeToRegistry(starrail_registry_path, starrail_text_playtime);
            }

            if (zzz_selected == "R")
            {
                FileHelper.SavePlaytimeToTextFile(zzz_registry_playtime, 2, file_path);
            }
            else if (zzz_selected == "T")
            {
                RegistryHelper.SavePlaytimeToRegistry(zzz_registry_path, zzz_text_playtime);
            }

            if (honkai_selected == "R")
            {
                FileHelper.SavePlaytimeToTextFile(honkai_registry_playtime, 3, file_path);
            }
            else if (honkai_selected == "T")
            {
                RegistryHelper.SavePlaytimeToRegistry(honkai_registry_path, honkai_text_playtime);
            }
            UpdateRadioButtonsAndGroups();
        }

public void UpdateRadioButtonsAndGroups()
{
    foreach (var group in this.Controls.OfType<GroupBox>())
    {
        if (group.Controls.Count < 2) continue; // Ensure there are at least two controls (radio buttons) in the group

        RadioButton registryRadioButton = group.Controls[0] as RadioButton;
        RadioButton textRadioButton = group.Controls[1] as RadioButton;

        if (registryRadioButton == null || textRadioButton == null) continue; // Ensure the controls are radio buttons

        // Update the text of the radio buttons
        if (group.Name == "Genshin Impact")
        {
            registryRadioButton.Text = RegistryHelper.GetRegistryPlaytimeDisplay(genshin_registry_path);
            textRadioButton.Text = FileHelper.GetTextFilePlaytimeDisplay(0, file_path);
        }
        else if (group.Name == "Star Rail")
        {
            registryRadioButton.Text = RegistryHelper.GetRegistryPlaytimeDisplay(starrail_registry_path);
            textRadioButton.Text = FileHelper.GetTextFilePlaytimeDisplay(1, file_path);
        }
        else if (group.Name == "Zenless Zone Zero")
        {
            registryRadioButton.Text = RegistryHelper.GetRegistryPlaytimeDisplay(zzz_registry_path);
            textRadioButton.Text = FileHelper.GetTextFilePlaytimeDisplay(2, file_path);
        }
        else if (group.Name == "Honkai Impact 3rd")
        {
            registryRadioButton.Text = RegistryHelper.GetRegistryPlaytimeDisplay(honkai_registry_path);
            textRadioButton.Text = FileHelper.GetTextFilePlaytimeDisplay(3, file_path);
        }

        int registryPlaytime = RegistryHelper.ReadPlaytimeFromRegistry(gameList.First(game => game.Item1 == group.Text).Item2);
        int textPlaytime = FileHelper.ReadPlaytimeFromTextFile(gameList.First(game => game.Item1 == group.Text).Item3, file_path);

        // Update the colors based on playtime comparison
        if (registryPlaytime > textPlaytime)
        {
            registryRadioButton.ForeColor = Color.Green;
            textRadioButton.ForeColor = Color.Black;
        }
        else if (registryPlaytime < textPlaytime)
        {
            textRadioButton.ForeColor = Color.Green;
            registryRadioButton.ForeColor = Color.Black;
        }
        else
        {
            registryRadioButton.ForeColor = Color.Black;
            textRadioButton.ForeColor = Color.Black;
        }

        // Measure the text size and update the size of the radio buttons
        Size registryTextSize = MeasureTextSize(registryRadioButton.Text, registryRadioButton.Font);
        Size textTextSize = MeasureTextSize(textRadioButton.Text, textRadioButton.Font);
        registryRadioButton.Size = new Size(registryTextSize.Width + 100, registryTextSize.Height + 10);
        textRadioButton.Size = new Size(textTextSize.Width + 100, textTextSize.Height + 10);

        // Update the size of the group box based on the larger radio button
        int maxWidth = Math.Max(registryTextSize.Width, textTextSize.Width);
        group.Size = new Size(maxWidth + 120, group.Height);
    }
}

        public static List<(string, string, int)> CreateGameList()
        {
            List<(string, string, int)> gameList = new();
            if (RegistryHelper.CheckRegistryEntry(genshin_registry_path))
            {
                gameList.Add(("Genshin Impact", genshin_registry_path, 0));
            }
            if (RegistryHelper.CheckRegistryEntry(starrail_registry_path))
            {
                gameList.Add(("Star Rail", starrail_registry_path, 1));
            }
            if (RegistryHelper.CheckRegistryEntry(zzz_registry_path))
            {
                gameList.Add(("Zenless Zone Zero", zzz_registry_path, 2));
            }
            if (RegistryHelper.CheckRegistryEntry(honkai_registry_path))
            {
                gameList.Add(("Honkai Impact 3rd", honkai_registry_path, 3));
            }
            return gameList;
        }

        private void SelectFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = file_path;
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                file_path = openFileDialog.FileName;
                SaveFilePath(file_path);
                filePathLabel.Text = "File Path: " + file_path;
                filePathLabel.Size = TextRenderer.MeasureText(filePathLabel.Text, filePathLabel.Font);
                selectFile.Location = new Point(filePathLabel.Size.Width + 10, 20);
                fileSelectionGroup.Size = new Size(filePathLabel.Size.Width + selectFile.Size.Width + 30, selectFile.Size.Height + 30);
                SetGroupBoxVisibilityAndPosition();
                GetPlaytimes();
                UpdateRadioButtonsAndGroups();

            }
        }

        private void SaveFilePath(string filePath)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(RegistryKeyPath);
            key.SetValue(RegistryValueName, filePath);
            key.Close();
        }


        public static string GetFilePath()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath);
            if (key != null)
            {
                string filePath = key.GetValue(RegistryValueName).ToString();
                key.Close();
                if (!File.Exists(filePath))
                {
                    return default_file_path;
                }
                return filePath;
            }
            return default_file_path;
        }

        public static void GetPlaytimes()
        {
            genshin_registry_playtime = RegistryHelper.ReadPlaytimeFromRegistry(genshin_registry_path);
            genshin_text_playtime = FileHelper.ReadPlaytimeFromTextFile(0, file_path);
            starrail_registry_playtime = RegistryHelper.ReadPlaytimeFromRegistry(starrail_registry_path);
            starrail_text_playtime = FileHelper.ReadPlaytimeFromTextFile(1, file_path);
            zzz_registry_playtime = RegistryHelper.ReadPlaytimeFromRegistry(zzz_registry_path);
            zzz_text_playtime = FileHelper.ReadPlaytimeFromTextFile(2, file_path);
            honkai_registry_playtime = RegistryHelper.ReadPlaytimeFromRegistry(honkai_registry_path);
            honkai_text_playtime = FileHelper.ReadPlaytimeFromTextFile(3, file_path);
        }

        private Size MeasureTextSize(string text, Font font)
        {
            using (Graphics g = this.CreateGraphics())
            {
                return g.MeasureString(text, font).ToSize();
            }
        }

        private System.Windows.Forms.GroupBox genshinGroup;
        private System.Windows.Forms.RadioButton genshinRegistryRadioButton;
        private System.Windows.Forms.RadioButton genshinTextRadioButton;
        private System.Windows.Forms.GroupBox starrailGroup;
        private System.Windows.Forms.RadioButton starrailRegistryRadioButton;
        private System.Windows.Forms.RadioButton starrailTextRadioButton;
        private System.Windows.Forms.GroupBox zzzGroup;
        private System.Windows.Forms.RadioButton zzzRegistryRadioButton;
        private System.Windows.Forms.RadioButton zzzTextRadioButton;
        private System.Windows.Forms.GroupBox honkaiGroup;
        private System.Windows.Forms.RadioButton honkaiRegistryRadioButton;
        private System.Windows.Forms.RadioButton honkaiTextRadioButton;
        private System.Windows.Forms.Button updateButton;
        private System.Windows.Forms.GroupBox fileSelectionGroup;
        private System.Windows.Forms.Button selectFile;
        private System.Windows.Forms.Label filePathLabel;
        private const string RegistryKeyPath = @"SOFTWARE\PlaytimeUpdater";
        private const string RegistryValueName = "FilePath";
        

        public static string genshin_registry_path = @"Software\miHoYo\Genshin Impact";
        public static string starrail_registry_path = @"Software\Cognosphere\Star Rail";
        public static string zzz_registry_path = @"Software\miHoYo\ZenlessZoneZero";
        public static string honkai_registry_path = @"Software\miHoYo\Honkai Impact 3rd";

        public static string default_text_folder_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\PlaytimeUpdater\";
        public static string default_file_path = default_text_folder_path + "playtime.txt";

        public static string file_path = GetFilePath();

        public static List<(string, string, int)> gameList = CreateGameList();

        public static int genshin_registry_playtime;
        public static int genshin_text_playtime;
        public static int starrail_registry_playtime;
        public static int starrail_text_playtime;
        public static int zzz_registry_playtime;
        public static int zzz_text_playtime;
        public static int honkai_registry_playtime;
        public static int honkai_text_playtime;

        private string genshin_selected="";
        private string starrail_selected="";
        private string zzz_selected="";
        private string honkai_selected="";
    }
}