using System;
using System.IO;
using System.Threading;
using System.IO.Compression;

namespace WSFilesArchivator
{
    public class ArchiveService
    {
        
        private readonly Timer timer;
        private readonly string resourceFolderPath;
        private readonly string resultFolderPath;

        public ArchiveService()
        {
            timer = new Timer(Archivation);
            resourceFolderPath = @"e:\Mentoring\WindowsService\WSFilesArchivator\bin\Debug\ResourceFolder\";
            resultFolderPath = @"e:\Mentoring\WindowsService\WSFilesArchivator\bin\Debug\ResultFolder\";
        }

        public bool Start()
        {
            timer.Change(0, 5000);
            return true;
        }

        public bool Stop()
        {
            timer.Change(Timeout.Infinite, 0);
            return true;
        }

        public void Archivation(object target)
        {
            string zipName = $@"{resultFolderPath}{DateTime.Now.ToString("yyyyMMdd_hhmmss")}.zip";
            string filepath1 = $@"{resourceFolderPath}IMG_001.jpg";
            string filepath2 = $@"{resourceFolderPath}IMG_001.jpg";

            using (ZipArchive newFile = ZipFile.Open(zipName, ZipArchiveMode.Create))
            {
                newFile.CreateEntryFromFile(filepath1, "IMG_001.jpg");
                newFile.CreateEntryFromFile(filepath2, "IMG_002.jpg");
            }
        } 
    }
}
