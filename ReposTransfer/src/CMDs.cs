using System;
using System.Collections.Generic;

namespace ReposTransfer
{
    public class CMDs
    {
        public string Start() => "rtn"; // ReposTransfer start comand
        public string Init() =>  "init"; // init connection with host
        public string AddOne() => "add"; // select a single file
        public string AddAll() => "add-all"; // select all directory
        public string Push() => "push"; // backup local project to host
        public string Status() => "status"; // show local project status
        public string Pull() => "pull"; // restore local project from another host
        public string Help() => "help"; // show comands

        public string HelpInfo() // show all comand
        {
            string comands = string.Empty;
            string nl = Environment.NewLine;

            comands = $"usage: rtn [--version] [--help] <commands>{nl}" +
                $"These are common ReposTranfer used in various situantions:{nl}" +
                $"{nl}" +
                $"  {Init()} === {"Establish the connection to Local Server/Machine"}{nl}" +
                $"  {AddOne()} === {"Makes to ready a single file"}{nl}" +
                $"  {AddAll()} === {"Makes to ready all files and subDirectories"}{nl}" +
                $"  {Push()} === {"Backup selected files/directories to the Server"}{nl}" +
                $"  {Status()} === {"Checks the single file with 'status file' otherwise use 'status dir' before transferred"}{nl}" +
                $"  {Pull()} === {"Restore selected files/directories by the Server"}{nl}" +
                $"{nl}" +
                $"'rtn help' list available commands." ;

            return comands;
        }
    }
}