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
                indexer.ClassifyFolders = false;
                //indexer.ClassifyFoldersDepth = 0;

                Classification classification = new Classification(app);
                if(classification.TryLoad(new ClassificationPath("Cubus")) == TryLoadResult.NotFound)
                    throw new ItemNotFoundException("root catalog was not found");

                indexer.ClassifyFoldersRoot = classification.Id;
                indexer.Enabled = true;

                indexer.JobPriority = 8;
                indexer.JobThresholdTimeout = new TimeSpan(0,0,10);

                indexer.FileTypeRestriction = FileTypeRestriction.OnlyKnown;

                indexer.Save();
                
            }
            
        }
        
    }
}
