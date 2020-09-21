using System.IO;
using System.Text;
using System;
using static System.Console;

namespace ReposTransfer
{
    public class CoverPwd
    {
        public string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo info = ReadKey(true);
            while (info.Key != ConsoleKey.Enter)
            {
                if (info.Key != ConsoleKey.Backspace)
                {
                    Write("*");
                    password += info.KeyChar;
                }
                else if (info.Key == ConsoleKey.Backspace)
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        // remove one character from the list of password characters
                        password = password.Substring(0, password.Length - 1);
                        // get the location of the cursor
                        int pos = CursorLeft;
                        // move the cursor to the left by one character
                        SetCursorPosition(pos - 1, CursorTop);
                        // replace it with space
                        Write(" ");
                        // move the cursor to the left by one character again
                        SetCursorPosition(pos - 1, CursorTop);
                    }
                }
                info = ReadKey(true);
            }

            // add a new line because user pressed enter at the end of their password
            WriteLine();
            return password;
        }

    }
}