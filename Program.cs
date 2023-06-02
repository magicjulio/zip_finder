using System;
using System.IO;

namespace ConsoleZipFinder
{
    /// <summary>
    /// Geht durch ein angegbenen ordner und all seine sub ordner und sucht nach .zip oder .iso datein die entpackt wurden.
    /// Um speicher zu sparen, werden treffer dann im terminal angegeben und in einer csv dokumentiert.
    /// </summary>
    internal class Program
    {
        /// <summary
        /// pfad wird abgefragt
        /// </summary>
        /// <param name="args">wird nicht benötig aber geht nicht ohne irgendwie</param>

        static void Main(string[] args)
        {
            
            Console.WriteLine("Enter the root folder path:");
            string rootFolderPath = Console.ReadLine();

            ProcessFolder(rootFolderPath);
            
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderPath">pfad zum root ordner der durchsucht werden soll</param>
        static void ProcessFolder(string folderPath)
        {
            string[] zipFiles = Directory.GetFiles(folderPath, "*.*");
            
            // für jede datei im ordner
            foreach (string zipFile in zipFiles)
            {   
                
                string extractedFileName = Path.GetFileNameWithoutExtension(zipFile);
                string extractedFilePath = Path.Combine(folderPath, extractedFileName);

                // Wenn der pfad des ordners und der datei exsistirt, dann haben wir ein "duplikant"
                if (Directory.Exists(extractedFilePath) || File.Exists(extractedFilePath))
                {

                    // infos zum treffer ausgeben und an csv anhängen
                    addRecord(Path.GetFileName(zipFile), folderPath, @"C:\Users\großerj\Desktop\data.csv");

                    Console.WriteLine("Match found:");
                    Console.WriteLine("ZIP File: " + zipFile);
                    Console.WriteLine("Extracted File/Folder: " + extractedFilePath);
                    Console.WriteLine();

                }
            }
            // weiter gehts mit den sub ordern (rekursiv)
            string[] subfolders = Directory.GetDirectories(folderPath);

            foreach (string subfolder in subfolders)
            {
                ProcessFolder(subfolder);
            }
        }

        public static void addRecord(string fileName, string path, string csv_path)
        {
            // try falls es ein datei fehler gibt
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@csv_path, true))
                {
                    // schreibe ein neuen eintrag mit datei name und pfad
                    file.WriteLine(fileName + ";"+ path);
                }

            }
            catch(Exception ex)
            {
                throw new AggregateException("Opssi Woobsi, ", ex);
            }
        }
    }
}
