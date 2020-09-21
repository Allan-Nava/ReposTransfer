using System;
using System.Text;
using static System.Console;
using System.IO;

namespace ReposTransfer
{
    public class StatusChecker
    {
        CMDs _cmd = new CMDs();
        public void Status(string Cmd, string Source)
        {
            if(Cmd.EndsWith("dir"))
            {
                // check all ready files/directories 'add all'
                DirectoryInfo dir = new DirectoryInfo(Source);

                WriteLine("Uploaded Files");
                WriteLine("    (use 'reset' to unstage)");
                ForegroundColor = ConsoleColor.Green;

                FileInfo[] files = dir.GetFiles();
                DirectoryInfo[] dirs = dir.GetDirectories();
                foreach(FileInfo file in files)
                {
                    WriteLine(" ready: " + file);
                }

                foreach (DirectoryInfo subdir in dirs)
                {
                    WriteLine(" ready: " + subdir);
                }
            } 
            else
            {
                // check ready file 'add all'
                ForegroundColor = ConsoleColor.Green;
                string file = Path.GetFileName(Source);
                WriteLine("Uploaded File");
                WriteLine("    (use 'reset' to unstage)");
                WriteLine(" ready: " + file);
            }
        }
    }
}