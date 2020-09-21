using System;
using System.IO;
using static System.Console;

namespace ReposTransfer
{
    
    public class ReposInfo
    {
        Cryptography aes = new Cryptography();
        public void CreateInfoFile(string sourceDir, string contents)
        {
            try
            {
                string aesContents = aes.EncryptText(contents);
                string filePath = sourceDir + @"\.reposinfo.txt";
                File.WriteAllText(filePath, aesContents);
            }
            catch(Exception ex)
            {
                WriteLine(ex.Message.ToString());
            }
        }
        
        public string ReadInfoFile(string sourceDir)
        {
            string Contents = "";
            string filePath;
            try
            {
                if(sourceDir.EndsWith("\\"))
                    filePath = sourceDir + @".reposinfo.txt";
                else filePath = sourceDir + @"\.reposinfo.txt";

                var aesContents = File.ReadAllLines(filePath);
                Contents = aes.DecryptText(aesContents[0].ToString());
            }
            catch(Exception ex)
            {
                WriteLine(ex.Message.ToString());
            }

            return Contents;
        }

        public bool InfoFileFinder(string sourceDir)
        {
            bool exist = true;

            try
            {
                string filePath = sourceDir + @"\.reposinfo.txt";
                exist = File.Exists(filePath);
            }
            catch(Exception ex)
            {
                WriteLine(ex.Message.ToString());
            }
            
            return exist; 
        }
    }
}
