using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using PDF_Merger.Utilities;

namespace PDF_Merger
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            #region Variables and messages
            var noMessage = $"Abriré el directorio por ti ahora. Copie todos los documentos en formato PDF que desee combinar.\r\n";
            var noteMessage = $"NOTA: El orden de fusión se determina alfabéticamente a partir de los nombres de archivo (por ejemplo, FileA.pdf aparecerá antes de FileB.pdf).\r\n";
            var yesMessage = $"\r\nMuy bien, lo abriré ahora. Copie los documentos PDF que desea combinar.\r\n";
            var goMessage = $"Pulse [Entrar] cuando haya terminado de copiar los documentos PDF necesarios.\r\n";
            const string liarMessage = "Lo siento, pero no pude encontrar el directorio. Lo crearé ahora.";
            const string targetFileName = "ResultadoCombinado.pdf";
            const string workingDirectory = "PDF_Combinador";

            //Determining user's desktop filepath
            var rootPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            //Building working directory file path
            var workingPath = $"{rootPath}\\{workingDirectory}";
            #endregion
            
            #region Checking for correct directory structure
            WriteLine("¿Hola, como te llamas? ", ConsoleColor.Cyan);
            var name = Console.ReadLine();
            WriteLine($"\r\nHola {name}! ¿Tiene una carpeta en su escritorio titulada {workingDirectory}?", ConsoleColor.Cyan);

            var readLine = Console.ReadLine();
            if (readLine != null)
            {
                var response1 = readLine.ToLower();

                if (response1.Contains("n".ToLower()))
                {
                    WriteLine("\r\nNo hay problema. Crearé el directorio ahora...\r\n", ConsoleColor.Cyan);
                    System.Threading.Thread.Sleep(2000);
                    try
                    {
                        //Creating and initializing working directory
                        workingPath = $"{rootPath}\\{workingDirectory}";
                        Directory.CreateDirectory(workingPath);

                        System.Threading.Thread.Sleep(1000);

                        WriteLine(noMessage, ConsoleColor.Cyan);
                        WriteLine(noteMessage,ConsoleColor.Yellow);
                        System.Threading.Thread.Sleep(2000);

                        Process.Start(workingPath);
                    }
                    catch (Exception e)
                    {
                        WriteLine($"\r\nHuy! Algo salió mal: { e.Message}", ConsoleColor.Red);
                    }
                }
                else
                {
                    WriteLine(yesMessage, ConsoleColor.Cyan);
                    WriteLine(noteMessage, ConsoleColor.Yellow);
                    System.Threading.Thread.Sleep(2000);
                    try
                    {
                        Process.Start(workingPath);
                    }
                    catch (Exception e)
                    {
                        WriteLine(liarMessage, ConsoleColor.Cyan);

                        //Creating and initializing working directory
                        workingPath = $"{rootPath}\\{workingDirectory}";
                        Directory.CreateDirectory(workingPath);

                        System.Threading.Thread.Sleep(1000);

                        Process.Start(workingPath);
                    }


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
                if (!file.Name.Contains(targetFileName))
                    pdfList.Add(file.FullName);
            }


            if (pdfList.Count <= 1)
            {
                WriteLine(
                    $"Has proporcionado {pdfList.Count} documento PDF para combinar Debe proporcionar un mínimo de dos documentos PDF.\r\n",
                    ConsoleColor.Red);
                WriteLine($"Saliendo aplicacion...", ConsoleColor.Red);
                System.Threading.Thread.Sleep(5000);
                return;
            }
            else
            {

                WriteLine($"He encontrado {pdfList.Count} documentos PDF para combinar.\r\n\r\n", ConsoleColor.Cyan);
                System.Threading.Thread.Sleep(1000);
                WriteLine($"Procediendo con la combinación...\r\n", ConsoleColor.Yellow);
            }

            #endregion

            #region The merge
            WriteLine($"Empezado...", ConsoleColor.Green);
            System.Threading.Thread.Sleep(2000);

            MergeUtilities.MergePdfDocuments(targetPath, pdfList.ToArray());

            System.Threading.Thread.Sleep(2000);
            if (new FileInfo(targetPath).Length == 0)
            {
                WriteLine($"\r\nAlgo salió mal: No se encontraron datos en resultado combinado.", ConsoleColor.Red);
                WriteLine($"Saliendo aplicacion...", ConsoleColor.Red);
                System.Threading.Thread.Sleep(5000);
                return;
            }

            WriteLine($"Completado satisfactoriamente...", ConsoleColor.Green);
            #endregion

            #region Open file and close app
            System.Threading.Thread.Sleep(2000);
            WriteLine("\r\nPulse cualquier tecla para abrir el documento PDF combinado y cerrar la utilidad...", ConsoleColor.Cyan);
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