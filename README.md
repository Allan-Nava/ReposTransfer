# ReposTransfer

NOTE: This is an Beta version and it's probably unstable.
<br>
Local Network authentication for Backup/Transfer Files without Git commands.

[![Build Status](https://dev.azure.com/zhouyintong/ReposTransfer%20GitHub/_apis/build/status/yintong-zhou.ReposTransfer?branchName=master)](https://dev.azure.com/zhouyintong/ReposTransfer%20GitHub/_build/latest?definitionId=6&branchName=master)

Source Directory: Directory about all source that will be transferred.
<br>
If the '.reposinfo.txt' exists the system automatically will connect to the Server else enter the init command by the list.

## Commands List [IMPORTANT: start with 'rtn' comand]
- **init //0.0.0.0/folders** *(Establish the connection to Local Server/Machine)*
- **add filename.extension** *(Makes to ready a single file)*
- **add-all** *(Makes to ready all files and subDirectories)*
- **status file** *(Checks the single file status before transferred)*
- **status dir** *(Checks all files and subDirectories status before transferred)*
- **push** *(Backup selected files/directories to the Server)*
- **pull** *(Restore selected files/directories by the Server) [not working]* 

<br>
Others commands probably coming soon...
