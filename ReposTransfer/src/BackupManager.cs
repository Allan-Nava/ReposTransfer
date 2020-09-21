using System.IO;
using System;
using static System.Console;

namespace ReposTransfer
{
    public class BackupManager
    {
        ProgressBar bar = new ProgressBar();
        int FilesCount;
        int failCount;
        decimal rate;

        public void OneFile(string sourceFile, string destFile)
        {
            string destDir = Path.GetDirectoryName(destFile);

            if (!Directory.Exists(destDir)) Directory.CreateDirectory(destDir);

            if (File.Exists(sourceFile))
            {
                // transfer single file and overwrite it
                try
                {
                    File.Copy(sourceFile, destFile, true);
                    WriteLine("Transfer objects 100%, done.");
                    WriteLine("Counting objects (1/1), done.");
                    WriteLine("remote: Transfer complete.");
                    WriteLine("To " + destFile);
                }
                catch(Exception ex)
                {
                    WriteLine("ERROR: Transfer failure.");
                    WriteLine(ex.Message);
                }
            }
            else WriteLine(" Source file does not exist or could not be found: "+ sourceFile);
        }

        public void FullDirectory(string sourceDir, string destDir, bool copySubDirs)
        {
            FilesCount = 0;
            failCount = 0;
            rate = 0;

            DirectoryInfo dir = new DirectoryInfo(sourceDir);

            if(!dir.Exists)
            {
                WriteLine(" Source directory does not exist or could not be found: "
                    + sourceDir
                );
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            if(!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }

            FileInfo[] files = dir.GetFiles();
            foreach(FileInfo file in files)
            {
                string tempPath = Path.Combine(destDir, file.Name);
                try
                {
                    file.CopyTo(tempPath, true);
                    bar.ShowPercentProgress("Transfer objects ", FilesCount, files.Length);
                    FilesCount++;
                }
                catch
                {
                    failCount++;
                }
                
            }

            if(copySubDirs)
            {
                string tempPath;
               
                foreach (DirectoryInfo subdir in dirs)
                {
                    tempPath = Path.Combine(destDir, subdir.Name);
                    try
                    {
                        FullDirectory(subdir.FullName, tempPath, copySubDirs);
                        FilesCount++;   
                    }
                    catch
                    {
                        failCount++;
                    }
                }
            }
        }
        public void FullDirectoryStatus(string destDir)
        {
            try
            {
                int diff = FilesCount - failCount;
                WriteLine(Environment.NewLine + "Counting objects " + "(" + diff + "/" + FilesCount + ")" + ", done.");

                if (failCount < 1)
                    rate = 100;
                else rate = (failCount / FilesCount) * 100;
                
                WriteLine("remote: Transfer complete.");
                WriteLine("To " + destDir);
            }
            catch(Exception ex)
            {
                WriteLine(ex.Message.ToString());
            }
        }
    }
}