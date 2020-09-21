using System;
using static System.Console;
using System.IO;
using System.Net;

namespace ReposTransfer
{
    class Program
    {
        #region Variables
        static ReposInfo repos = new ReposInfo();
        static KeysGenerator generator = new KeysGenerator();
        static CoverPwd _cover = new CoverPwd();
        static BackupManager _backup = new BackupManager();
        static CMDs _cmds = new CMDs();
        static StatusChecker _checker = new StatusChecker();
        static NetworkCredential netCredential;
        static NetworkConnection netConn;

        static string newL = Environment.NewLine;
        static string netDir, source, dest, input;
        static string user, pwd;
        static string version = "1.0.0";
        #endregion

        static void Main(string[] args)
        {
            #region Init Terminal
            Title = "rtn";
            WriteLine($"Welcome to REPOS TRANSFER v{version}" + newL);
            Write("Source Directory: ");

            string sourceDir = ReadLine();
            bool reposEx = repos.InfoFileFinder(sourceDir);
            if (reposEx)
            {
                var netStr = repos.ReadInfoFile(sourceDir);
                var netSplit = netStr.Split(';');
                netDir = netSplit[0].ToString();
                var user = netSplit[1].ToString();
                var pwd = netSplit[2].ToString();

                try
                {
                    netCredential = new NetworkCredential(user, pwd);
                    netConn = new NetworkConnection(netDir, netCredential);
                    WriteLine("Connected to " + netDir);
                }
                catch (Exception ex)
                {
                    WriteLine(ex.Message.ToString());
                }
            }
            else
            { 
                WriteLine("Write " + "init \\\\0.0.0.0\\dir\\subdir".ToUpper() + " for connect to the Server");
            }
        #endregion

        InputCMD:
            Write(newL + sourceDir + ">");
            input = ReadLine();

            var command = input.Split(' ');
            
            if (input.StartsWith(_cmds.Start()))
            {
                #region Init Connection
                if (command[1] == _cmds.Init())
                {
                    Write(newL + "Username: ");
                    user = ReadLine();
                    Write("Password: ");
                    pwd = _cover.ReadPassword();

                    var token = generator.HashCode(64);
                    var remote = input.Split(' '); // input: init \\\\192.168.1.242\\swlab aggiungendo la parte rtn, il netPath diventa remote[2]

                    if (command.Length > 2)
                    {
                        var netAuth = remote[2] + ";" + user + ";" + pwd + ";" + token;
                        repos.CreateInfoFile(sourceDir, netAuth);
                        WriteLine("Initialized remote {0}", remote[2]);

                        netCredential = new NetworkCredential(user, pwd);
                        try
                        {
                            netConn = new NetworkConnection(remote[2], netCredential);
                            WriteLine(newL + "Connect to " + remote[2]);
                        }
                        catch (Exception ex)
                        {
                            WriteLine(">>ERROR: Connection failure.");
                            WriteLine(ex.Message.ToString());
                        }
                    }
                    else
                    {
                        WriteLine(">>INFO: Please insert rtn init 'network root'.");
                        goto InputCMD;
                    }
                }
                #endregion

                #region ADD ONE
                if (command[1] ==_cmds.AddOne())
                {
                    #region Add One to Push
                    if (command.Length > 2) source = sourceDir + "\\" + command[2];
                    else WriteLine(">>INFO: Please insert the filename.extension");
                   
                    string[] dirName = source.Split('\\');
                    int i = dirName.Length;
                    i--;

                    string fileName = Path.GetFileName(source);
                    dest = Path.Combine(netDir + "\\", dirName[1] + "\\" + fileName);

                CMDone:
                    ResetColor();
                    Write(newL + sourceDir + ">");
                    input = ReadLine();
                    command = input.Split(' ');

                    try
                    {
                        if (command[0] == _cmds.Start())
                        {
                            if(command.Length > 1)
                            {
                                if (command[1] == _cmds.Push())
                                {
                                    _backup.OneFile(source, dest);
                                }
                                else if (command[1] == _cmds.Status())
                                {
                                    _checker.Status(command[1], source);
                                    goto CMDone;
                                }
                                else
                                {
                                    WriteLine(">>INFO: Invalid comand.");
                                    goto CMDone;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLine(ex.Message.ToString());
                    }
                    #endregion

                }
                #endregion

                #region ADD ALL
                if (command[1] == _cmds.AddAll())
                {
                    #region Add All to Push
                    source = sourceDir;
                    string[] dirName = source.Split('\\');
                    int i = dirName.Length;
                    i--;
                    dest = Path.Combine(netDir + "\\", dirName[i]);

                CMDall:
                    ResetColor();
                    Write(newL + sourceDir + ">");
                    input = ReadLine();
                    command = input.Split(' ');

                    try
                    {
                        if (command[0] == _cmds.Start())
                        {
                            if (command.Length > 1)
                            {
                                if (command[1] == _cmds.Push())
                                {
                                    _backup.FullDirectory(source, dest, true);
                                    _backup.FullDirectoryStatus(dest);
                                }
                                else if (command[1] == _cmds.Status())
                                {
                                    _checker.Status(command[1], source);
                                    goto CMDall;
                                }
                                else
                                {
                                    WriteLine(">>INFO: Invalid comand.");
                                    goto CMDall;
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        WriteLine(ex.Message.ToString());
                    }
                    #endregion
                }
                #endregion

                #region PULL
                Write(newL + sourceDir + ">");
                input = ReadLine();
                command = input.Split(' ');
                if (command[1] == _cmds.Pull())
                {
                    try
                    {
                        string localRepos = repos.ReadInfoFile(sourceDir);
                        var localInfo = localRepos.Split(';');
                        var localKey = localInfo[3];

                        var subDir = Path.GetFileName(sourceDir);
                        var remoteDir = localInfo[0] + "\\" + subDir;

                        string remoteRepos = repos.ReadInfoFile(remoteDir);
                        var remoteInfo = remoteRepos.Split(';');
                        var remoteKey = remoteInfo[3];

                        if (localKey != remoteKey)
                        {
                            WriteLine(">>ERROR: ID Key is corrupted.");
                        }
                        else
                        {
                            _backup.FullDirectory(remoteDir, sourceDir, true);
                            _backup.FullDirectoryStatus(sourceDir);
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLine(ex.Message.ToString());
                    }
                }
                #endregion

                #region HELP
                if (command[1] == _cmds.Help())
                {
                    WriteLine(_cmds.HelpInfo());
                }
                #endregion
            }
            else
            {
                WriteLine(">>INFO: Try start with 'rtn' comand.");
            }

            goto InputCMD;
        }
    }
}
