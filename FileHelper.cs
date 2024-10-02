using System;
using System.Data;
using System.IO;

namespace PlaytimeUpdater
{
    public static class FileHelper
    {
        public static int ReadPlaytimeFromTextFile(int line, string filePath)
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath);
                if (lines.Length > line)
                {
                    return int.Parse(lines[line]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading playtime from text file: {ex.Message}");
            }
            return 0;
        }

        public static void SavePlaytimeToTextFile(int value, int line, string filePath)
        {
            try
            {
                if ( filePath == MainForm.default_file_path && !File.Exists(filePath))
                {
                    CreateDefaultFile();
                }
                else if (!File.Exists(filePath))
                {
                    File.CreateText(filePath).Close();
                }
                string[] lines = File.ReadAllLines(filePath);
                if (lines.Length <= line)
                {
                    Array.Resize(ref lines, line + 1);
                }
                lines[line] = value.ToString();
                File.WriteAllLines(filePath, lines);
                Console.WriteLine($"Successfully wrote to {filePath.Split('\\')[filePath.Split('\\').Length - 1]}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving playtime to text file: {ex.Message}");
            }
        }

        public static string GetTextFilePlaytimeDisplay(int line, string filePath)
        {
            int playtime = ReadPlaytimeFromTextFile(line, filePath); //in seconds
            int hours = playtime / 3600;
            int minutes = (playtime % 3600) / 60;
            return $"File : {hours}h {minutes}m";
        }

        public static void CreateDefaultFile()
        {
            if (!Directory.Exists(MainForm.default_text_folder_path))
            {
                Directory.CreateDirectory(MainForm.default_text_folder_path);
            }
            File.CreateText(MainForm.default_file_path).Close();
        }
    }
}