using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using PDF_Merger.Utilities;

namespace PDF_Merger
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            #region Variables and messages
            var noMessage = $"Abriré el directorio por ti ahora. Copie todos los documentos en formato PDF que desee fusionar.\r\n";
            var noteMessage = $"NOTA: El orden de fusión se determina alfabéticamente a partir de los nombres de archivo (por ejemplo, FileA.pdf aparecerá antes de FileB.pdf).\r\n";
            var yesMessage = $"\r\nMuy bien, lo abriré ahora. Copie los documentos PDF que desea fusionar.\r\n";
            var goMessage = $"Pulse [Entrar] cuando haya terminado de copiar los archivos necesarios.\r\n";

            //Determining user's desktop filepath
            var rootPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            //Building working directory file path
            var workingPath = $"{rootPath}\\PDF_Merge";
            #endregion
            
            #region Checking for correct directory structure
            WriteLine("Hey, what's your name?", ConsoleColor.Cyan);
            var name = Console.ReadLine();
            WriteLine($"\r\nHey {name}, do you have a folder on your desktop entitled PDF_Merge?", ConsoleColor.Cyan);

            var readLine = Console.ReadLine();
            if (readLine != null)
            {
                var response1 = readLine.ToLower();

                if (response1.Contains("n".ToLower()))
                {
                    WriteLine("\r\nNot a problem. I'll create the directory now...\r\n", ConsoleColor.Cyan);
                    System.Threading.Thread.Sleep(2000);
                    try
                    {
                        //Creating and initializing working directory
                        workingPath = $"{rootPath}\\PDF_Merge";
                        Directory.CreateDirectory(workingPath);

                        System.Threading.Thread.Sleep(1000);

                        WriteLine(noMessage, ConsoleColor.Cyan);
                        WriteLine(noteMessage,ConsoleColor.Yellow);
                        System.Threading.Thread.Sleep(2000);

                        Process.Start(workingPath);
                    }
                    catch (Exception e)
                    {
                        WriteLine($"\r\nOops...something went wrong: {e.Message}", ConsoleColor.Red);
                    }
                }
                else
                {
                    WriteLine(yesMessage, ConsoleColor.Cyan);
                    WriteLine(noteMessage, ConsoleColor.Yellow);
                    System.Threading.Thread.Sleep(2000);

                    Process.Start(workingPath);

                    System.Threading.Thread.Sleep(2000);
                }
            }

            System.Threading.Thread.Sleep(2000);
            WriteLine(goMessage, ConsoleColor.Cyan);

            var response2 = Console.ReadKey();
            if (response2.Key != ConsoleKey.Enter)
            {
                //Do nothing
            }

            //Creating file path for target
            const string targetFileName = "MergeResult.pdf";
            var targetPath = string.Join("/", workingPath, targetFileName);

            //Creating new empty list to populate with pdf filepaths
            var pdfList = new List<string>();

            //Instantiating directory info class against working path
            var d = new DirectoryInfo(workingPath);
            #endregion

            #region Build list of pdf documents

            //Looping through pdf documents
            for (var item = 0; item < d.GetFiles($"*.pdf").Length; item++)
            {
                //Returns all .pdf file info
                FileInfo file = d.GetFiles($"*.pdf")[item];

                //Ensuring merge result file is not included in list
                if (!file.Name.Contains("MergeResult"))
                    pdfList.Add(file.FullName);
            }


            if (pdfList.Count <= 1)
            {
                WriteLine(
                    $"You've provided {pdfList.Count} pdf document for merging. You must provide a minimum of two pdf files.\r\n",
                    ConsoleColor.Red);
                
            }

            WriteLine($"I have found {pdfList.Count} PDF files for merging.\r\n\r\n", ConsoleColor.Cyan);
            System.Threading.Thread.Sleep(1000);
            WriteLine($"Proceeding with merge...\r\n",ConsoleColor.Yellow);
            #endregion

            #region The merge
            WriteLine($"Merge started...", ConsoleColor.Green);
            System.Threading.Thread.Sleep(2000);
            MergeUtilities.MergePdfDocuments(targetPath, pdfList.ToArray());

            System.Threading.Thread.Sleep(2000);
            if (new FileInfo(targetPath).Length == 0)
            {
                WriteLine($"Something went wrong.", ConsoleColor.Red);
                throw new Exception(message: "No data found in merge result file.");
            }

            WriteLine($"Merge completed successfully...", ConsoleColor.Green);
            #endregion

            #region Open file and close app
            System.Threading.Thread.Sleep(2000);
            WriteLine("\r\nPress any key to open the file and close the merge utility...", ConsoleColor.Cyan);
            Console.ReadKey();
            Process.Start(targetPath);
            #endregion
        }

        #region ConsoleUtility
        private static void WriteLine(string value, ConsoleColor consoleColor = ConsoleColor.White)
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine(value);
            Console.ResetColor();
        }
        #endregion
    }
}