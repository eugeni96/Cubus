using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adam.Core;
using Adam.Core.Classifications;
using Adam.Core.Indexer;
using Adam.Core.Records;

namespace CustomIndexer
{
    public class CubusIndexer
    {
        public static void AddIndexer(Application app)
        {
            IndexerTask indexer = new IndexerTask(app);
            if (indexer.TryLoad("Cubus") == TryLoadResult.NotFound)
            {
                indexer.AddNew();
                indexer.Name = "Cubus";
                indexer.ScanEngine = "FileSystemWatcherScan";
                indexer.ProcessEngine = "CubusProcessEngine";

                indexer.Path = "D:\\ADAMWorkingTraining\\Files";
                indexer.CatalogMode = CatalogMode.Capture;
                //indexer.ClassifyFolders = true;
                indexer.ClassifyFolders = false;
                //indexer.ClassifyFoldersDepth = 0;

                indexer.FileExtensionRestriction = Adam.Core.Records.FileExtensionRestriction.ExtensionRequired;
                indexer.FileTypeRestriction = Adam.Core.Records.FileTypeRestriction.None; //Будем регистрировать расширения поддерживаемых форматов?
                indexer.ActionOnSuccess = ActionOnSuccess.PermanentlyDeleteFile;

                //task.FileSelectionPatterns.Add(FileSelectionPatternMode.Exclusive, @"\.cm");
                //task.FileSelectionPatterns.Add(FileSelectionPatternMode.Inclusive, @"\.mp4");
                //task.FileSelectionPatterns.Add(FileSelectionPatternMode.Inclusive, @"\.mp3");

                indexer.EmailNotificationSettings = "<triggers> </triggers>";

                Classification classification = new Classification(app);
                if(classification.TryLoad("/Cubus") == TryLoadResult.NotFound)
                    throw new ItemNotFoundException("root catalog was not found");

                indexer.ClassifyFoldersRoot = classification.Id;
                indexer.Enabled = true;

                indexer.JobThresholdTimeout = new TimeSpan(0,0,10);
                
            }
        }

        private static void RegisterIndexer()
        {
            const string configPath = @"C:\Program Files\ADAM Software\ADAM\Adam.Core.Indexer.exe.config";
            const string registrationPattern = "<add registration=\"{0}\" userName=\"{1}\" password=\"{2}\" name=\"{3}\" />";
            var config = new XmlDocument();
            config.Load(configPath);
            config.GetElementsByTagName("Tasks").Item(0).InnerXml += String.Format(registrationPattern, registration, userName, password, indexerName);
            config.Save(configPath);
        }

        private static bool RunIndexer()
        {
            System.Diagnostics.Process indexer = new System.Diagnostics.Process();
            indexer.StartInfo.UseShellExecute = false;
            indexer.StartInfo.RedirectStandardOutput = true;
            indexer.StartInfo.FileName = @"C:\Program Files\ADAM Software\ADAM\Adam.Core.CommandLine.exe";

            indexer.StartInfo.Arguments = GetIndexerParams(registration, userName, password, "scanners");
            indexer.Start();
            String statusFailed = registration.ToUpperInvariant() + new String(' ', 26 - registration.Length) + indexerName + new String(' ', 26 - indexerName.Length) + "Failed";
            String statusRunning = registration.ToUpperInvariant() + new String(' ', 26 - registration.Length) + indexerName + new String(' ', 26 - indexerName.Length) + "Running";

            string output = indexer.StandardOutput.ReadToEnd();
            indexer.WaitForExit();


            if (output.Contains(statusRunning))
                return true;

            if (output.Contains(statusFailed))
            {
                indexer.StartInfo.Arguments = GetIndexerParams(registration, userName, password, "stop", indexerName);
                indexer.Start();
                indexer.WaitForExit();
            }

            indexer.StartInfo.Arguments = GetIndexerParams(registration, userName, password, "start", indexerName);
            indexer.Start();
            output = indexer.StandardOutput.ReadToEnd();
            indexer.WaitForExit();

            indexer.StartInfo.Arguments = GetIndexerParams(registration, userName, password, "scanners");
            indexer.Start();
            output = indexer.StandardOutput.ReadToEnd();
            indexer.WaitForExit();

            return output.Contains(statusRunning);
        }

        private static string GetIndexerParams(string registration, string userName, string password, string action, string indexerName = null)
        {
            indexerName = indexerName ?? "";
            if (String.IsNullOrEmpty(indexerName))
                indexerName = "-name=" + indexerName;
            return String.Format("Indexer -registration={0} -userName={1} -password={2} {3} -action={4}"
                , registration
                , userName
                , password
                , indexerName
                , action);
        }
    }
}
