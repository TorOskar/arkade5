﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Tar;
using System.IO;
using Serilog;


namespace Arkivverket.Arkade.Util
{
    public class CompressionDecompressionTar : ICompressionDecompressionUtilities
    {

        // Tar file utilities built on nuget package SharpZipLib.0.86.0
        public int ExtractFolderFromArchive(string tarFileName, string targetFolderName)
        {
            int returnValue = 0;

            try
            {
                Stream inStream = File.OpenRead(tarFileName);
                TarArchive tarArchive = TarArchive.CreateInputTarArchive(inStream);
                tarArchive.ExtractContents(targetFolderName);
                tarArchive.Close();
                inStream.Close();
            }
            catch (Exception e)
            {
                returnValue = -1;
            }

            return returnValue;
        }



        public int CompressFolderContentToArchiveFile(string tarTargetFileName, string sourceFileFolder)
        {
            int returnValue = 0;

            try
            {
                Stream outStream = File.Create(tarTargetFileName);
                TarOutputStream tarOutputStream = new TarOutputStream(outStream);
                TarArchive tarArchive = TarArchive.CreateOutputTarArchive(tarOutputStream);

                // TODO: check difference between writing and not writing this 
                // Optionally, write an entry for the directory itself.
                //TarEntry tarEntry = TarEntry.CreateEntryFromFile(sourceFileFolder);
                //tarOutputStream.PutNextEntry(tarEntry);

                string[] filenames = Directory.GetFiles(sourceFileFolder, "*.*", System.IO.SearchOption.AllDirectories);

                // Remove source file path from filename and add each file to the TAR acrchive
                foreach (string filename in filenames)
                {
                    using (Stream inputStream = File.OpenRead(filename))
                    {
                        string tarName = filename.Replace(sourceFileFolder, "");
                        long fileSize = inputStream.Length;
                        TarEntry entry = TarEntry.CreateTarEntry(tarName);
                        entry.Size = fileSize;
                        tarOutputStream.PutNextEntry(entry);

                        byte[] localBuffer = new byte[32 * 1024];
                        while (true)
                        {
                            int numRead = inputStream.Read(localBuffer, 0, localBuffer.Length);
                            if (numRead <= 0)
                            {
                                break;
                            }
                            tarOutputStream.Write(localBuffer, 0, numRead);
                        }
                    }
                    tarOutputStream.CloseEntry();
                }
                tarOutputStream.Close();
            }
            catch (Exception e)
            {
                returnValue = -1;
            }
            return returnValue;
        }


    }
}
