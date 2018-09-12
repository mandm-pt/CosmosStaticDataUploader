using System;
using System.Collections.Generic;
using System.Text;

namespace CosmosStaticDataUploader
{
    internal static class Utils
    {
        internal static void Write(string text, ConsoleColor textColor = ConsoleColor.Gray)
        {
            Console.ForegroundColor = textColor;
            Console.Write(text);
        }

        internal static void WriteLine(string text, ConsoleColor textColor = ConsoleColor.Gray)
        {
            Console.ForegroundColor = textColor;
            Console.WriteLine(text);
        }
    }
}
