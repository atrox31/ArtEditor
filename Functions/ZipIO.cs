using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Windows.Forms;

namespace ArtCore_Editor.Functions
{
    // ReSharper disable once InconsistentNaming
     public static class ZipIO
    {
        /// <summary>
        /// Write string list as lines in file, return success state
        /// </summary>
        /// <param name="zipArchive">Name (path included) to ZIP archive</param>
        /// <param name="entryName">Name (path included) to file in ZIP archive</param>
        /// <param name="source">Source content to put in archive</param>
        /// <param name="replace">replace existing object?</param>
        public static bool WriteListToArchive(string zipArchive, string entryName, List<string> source, bool replace)
        {
            DialogResult result = DialogResult.Retry;
            while (result == DialogResult.Retry)
            {
                try
                {
                    using FileStream zipToOpen = new FileStream(zipArchive, FileMode.OpenOrCreate);
                    using ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update);

                    ZipArchiveEntry zipArchiveEntry = GetEntry(entryName, replace, archive);
                    if (zipArchiveEntry == null) return false;

                    using StreamWriter writer = new StreamWriter(zipArchiveEntry.Open());
                    foreach (string property in source)
                    {
                        writer.WriteLine(property);
                    }

                    writer.Close();
                    return true;
                }
                catch (FileNotFoundException)
                {
                    return false;
                }
                catch (InvalidDataException)
                {
                    MessageBox.Show(
                        $"Zip file '{zipArchive}' is corrupted. Delete it and retry action.",
                        "Cannot access file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                catch (IOException)
                {
                    result = MessageBox.Show(
                        $"Zip file '{zipArchive}' is open in another program. Close it and retry action.",
                        "Cannot access file", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    if (result == DialogResult.Cancel) return false;
                }
            }

            return false;
        }

        /// <summary>
        /// Write string list as lines in file, return success state
        /// </summary>
        /// <param name="zipArchive">Name (path included) to ZIP archive</param>
        /// <param name="entryName">Name (path included) to file in ZIP archive</param>
        /// <param name="source">Source content to put in archive</param>
        /// <param name="replace">replace existing object?</param>
        public static bool WriteLineToArchive(string zipArchive, string entryName, string source, bool replace)
        {
            DialogResult result = DialogResult.Retry;
            while (result == DialogResult.Retry)
            {
                try
                {
                    using FileStream zipToOpen = new FileStream(zipArchive, FileMode.OpenOrCreate);
                    using ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update);

                    ZipArchiveEntry zipArchiveEntry = GetEntry(entryName, replace, archive);
                    if (zipArchiveEntry == null) return false;

                    using StreamWriter writer = new StreamWriter(zipArchiveEntry.Open());
                    writer.WriteLine(source);
                    writer.Close();
                    return true;
                }
                catch (FileNotFoundException)
                {
                    return false;
                }
                catch (InvalidDataException)
                {
                    MessageBox.Show(
                        $"Zip file '{zipArchive}' is corrupted. Delete it and retry action.",
                        "Cannot access file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                catch (IOException)
                {
                    result = MessageBox.Show(
                        $"Zip file '{zipArchive}' is open in another program. Close it and retry action.",
                        "Cannot access file", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    if (result == DialogResult.Cancel) return false;
                }
            }

            return false;
        }


        /// <summary>
        /// Copy given file to zip archive, return success state
        /// </summary>
        /// <param name="zipArchive">Name (path included) to ZIP archive</param>
        /// <param name="entryName">Name (path included) to file in ZIP archive</param>
        /// <param name="source">Source content to put in archive</param>
        /// <param name="replace">replace existing object?</param>
        public static bool WriteFileToZipFile(string zipArchive, string entryName, string source, bool replace)
        {
            DialogResult result = DialogResult.Retry;
            while (result == DialogResult.Retry)
            {
                try
                {
                    using FileStream zipArchiveFileStream = new FileStream(zipArchive, FileMode.OpenOrCreate);
                    using ZipArchive archive = new ZipArchive(zipArchiveFileStream, ZipArchiveMode.Update);

                    ZipArchiveEntry zipArchiveEntry = GetEntry(entryName, replace, archive);
                    if (zipArchiveEntry == null) return false;

                    using StreamWriter writer = new StreamWriter(zipArchiveEntry.Open());
                    using StreamReader file = new StreamReader(source);
                    file.BaseStream.CopyTo(writer.BaseStream);
                    writer.Close();
                    file.Close();
                    return true;
                }
                catch (FileNotFoundException)
                {
                    return false;
                }
                catch (InvalidDataException)
                {
                    MessageBox.Show(
                        $"Zip file '{zipArchive}' is corrupted. Delete it and retry action.",
                        "Cannot access file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                catch (IOException)
                {
                    result = MessageBox.Show(
                        $"Zip file '{zipArchive}' is open in another program. Close it and retry action.",
                        "Cannot access file", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    if (result == DialogResult.Cancel) return false;
                }
            }

            return false;
        }

        private static ZipArchiveEntry GetEntry(string entryName, bool replace, ZipArchive archive)
        {
            ZipArchiveEntry testEntry = archive.GetEntry(entryName);
            if (testEntry == null)
            {
                return archive.CreateEntry(entryName);
            }
            else
            {
                if (replace)
                {
                    testEntry.Delete();
                    return archive.CreateEntry(entryName);
                }
                else
                {
                    return testEntry;
                }
            }
            //return null;
        }

        /// <summary>
        /// Copy given file to zip archive
        /// </summary>
        /// <param name="zipArchive">Name (path included) to ZIP archive</param>
        /// <param name="entryName">Name (path included) to file in ZIP archive</param>
        /// <param name="fileMustExists">If file not exists return on true => null or on false => String.Empty</param>
        public static string ReadFromZip(string zipArchive, string entryName, bool fileMustExists = false)
        {
            DialogResult result = DialogResult.Retry;
            while (result == DialogResult.Retry)
            {
                try
                {
                    using FileStream zipToOpen = new FileStream(zipArchive, FileMode.Open);
                    using ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read);

                    ZipArchiveEntry fileEntry = archive.GetEntry(entryName);

                    return fileEntry != null ? new StreamReader(fileEntry.Open()).ReadToEnd() : String.Empty;
                }
                catch (FileNotFoundException)
                {
                    return fileMustExists ? null : String.Empty;
                }
                catch (InvalidDataException)
                {
                    MessageBox.Show(
                        $"Zip file '{zipArchive}' is corrupted. Delete it and retry action.",
                        "Cannot access file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                catch (IOException)
                {
                    result = MessageBox.Show(
                        $"Zip file '{zipArchive}' is open in another program. Close it and retry action.",
                        "Cannot access file", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    if (result == DialogResult.Cancel) return null;
                }
            }

            return null;
        }
    }
}
