using System;
using System.IO;
using System.Threading;
using System.IO.Compression;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WSFilesArchivator
{
    public class ArchiveService
    {

        private readonly Timer timer;  
        private readonly string resourceFolderPath;
        private readonly string resultFolderPath;

        private Regex re = new Regex(@"([a-zA-Z]+)(\d+)");

        public ArchiveService()
        {
            timer = new Timer(Archivation);
            resourceFolderPath = @"e:\Mentoring\WindowsService\WSFilesArchivator\bin\Debug\ResourceFolder\";
            resultFolderPath = @"e:\Mentoring\WindowsService\WSFilesArchivator\bin\Debug\ResultFolder\";
        }

        public bool Start()
        {
            timer.Change(0, 300000);
            return true;
        }

        public bool Stop()
        {
            timer.Change(Timeout.Infinite, 0);
            return true;
        }

        public void Archivation(object target)
        {
            DirectoryInfo resourceDirectory = new DirectoryInfo(resourceFolderPath);
            List<FileInfo> batch = new List<FileInfo>();
            int fileIndex = 0;
            foreach (var file in resourceDirectory.GetFiles("*.jpg"))
            {
                // remove extension from the name
                string name = file.Name.Substring(0, file.Name.Length - 4);
                // get a number
                int row, a = getIndexofNumber(name);
                string number = file.Name.Substring(a, name.Length - a);
                row = Convert.ToInt32(number);

                if (row - fileIndex == 1 || fileIndex == 0)
                {
                    batch.Add(file);
                }
                else
                {
                    CreateArchive(batch);
                    batch.Clear();
                    batch.Add(file);
                }
                fileIndex = row;
            }
            CreateArchive(batch);
        }

        public void CreateArchive(List<FileInfo> files)
        {
            string zipName = $@"{resultFolderPath}{DateTime.Now.ToString("yyyyMMdd_hhmmss")}.zip";
            using (ZipArchive newFile = ZipFile.Open(zipName, ZipArchiveMode.Create))
            {
                foreach (var file in files)
                {
                    newFile.CreateEntryFromFile(file.FullName, file.Name);
                }
            }
        }

        private int getIndexofNumber(string cell)
        {
            int indexofNum = -1;
            foreach (char c in cell)
            {
                indexofNum++;
                if (Char.IsDigit(c))
                {
                    return indexofNum;
                }
            }
            return indexofNum;
        }
    }
}
