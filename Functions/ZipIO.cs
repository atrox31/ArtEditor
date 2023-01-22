using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

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
        public static bool WriteToZipArchiveFromStream(string zipArchive, string entryName, Stream source, bool replace)
        {
            if(source == null)
            {
                MessageBox.Show(
                       "Source is null. Can not copy file to archive",
                       "Null source", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
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
                    if(source.Position == source.Length)
                    {
                        source.Position = 0;
                    }
                    using StreamReader file = new StreamReader(source);
                    file.BaseStream.CopyTo(writer.BaseStream);
                    writer.Flush();
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
        }

        /// <summary>
        /// Read string from file in archive
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

                    return fileEntry != null ? new StreamReader(fileEntry.Open()).ReadToEnd() : string.Empty;
                }
                catch (FileNotFoundException)
                {
                    return fileMustExists ? null : string.Empty;
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

        /// <summary>
        /// Get image from archive
        /// </summary>
        /// <param name="zipArchive">Name (path included) to ZIP archive</param>
        /// <param name="entryName">Name (path included) to image file in ZIP archive</param>
        /// <returns>object Bitmap or null on error</returns>
        public static Bitmap ReadImageFromArchive(string zipArchive, string entryName)
        {
            DialogResult result = DialogResult.Retry;
            while (result == DialogResult.Retry)
            {
                try
                {
                    using FileStream zipToOpen = new FileStream(zipArchive, FileMode.Open);
                    using ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read);

                    ZipArchiveEntry fileEntry = archive.GetEntry(entryName);

                    return fileEntry != null ? new Bitmap(fileEntry.Open()) : null;
                }
                catch (FileNotFoundException)
                {
                    return null;
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
        /// <summary>
        /// Get stream from archive
        /// </summary>
        /// <param name="zipArchive">Name (path included) to ZIP archive</param>
        /// <param name="entryName">Name (path included) to image file in ZIP archive</param>
        /// <returns>object Bitmap or null on error</returns>
        public static Stream ReadStreamFromArchive(string zipArchive, string entryName)
        {
            DialogResult result = DialogResult.Retry;
            while (result == DialogResult.Retry)
            {
                try
                {
                    using FileStream zipToOpen = new FileStream(zipArchive, FileMode.Open);
                    using ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read);

                    ZipArchiveEntry fileEntry = archive.GetEntry(entryName);

                    if (fileEntry == null) return null;

                    Stream returnStream = new MemoryStream();
                    fileEntry.Open().CopyTo(returnStream);
                    return returnStream;
                }
                catch (FileNotFoundException)
                {
                    return null;
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

        /// <summary>
        /// Copy Bitmap (Image) from one archive to another
        /// </summary>
        /// <param name="zipArchiveInput">Name (path included) to ZIP archive</param>
        /// <param name="entryNameInput">Name (path included) to image file in ZIP archive</param>
        /// <param name="zipArchiveOutput">Name (path included) to ZIP archive</param>
        /// <param name="entryNameOutput">Name (path included) to image file in ZIP archive</param>
        /// <returns>success status</returns>
        public static bool CopyImageToArchive(string zipArchiveInput, string entryNameInput,
            string zipArchiveOutput, string entryNameOutput )
        {
            if (!File.Exists(zipArchiveInput))
            {
                MessageBox.Show(
                        $"File '{zipArchiveInput}' not found.",
                        "Input file not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!File.Exists(zipArchiveOutput))
            {
                MessageBox.Show(
                        $"File '{zipArchiveOutput}' not found.",
                        "Output file not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


            DialogResult result = DialogResult.Retry;
            while (result == DialogResult.Retry)
            {
                try
                {
                    using FileStream inputZip = new FileStream(zipArchiveInput, FileMode.Open);
                    using ZipArchive inputArchive = new ZipArchive(inputZip, ZipArchiveMode.Read);
                    
                    using FileStream outputZip = new FileStream(zipArchiveOutput, FileMode.OpenOrCreate);
                    using ZipArchive outputArchive = new ZipArchive(outputZip, ZipArchiveMode.Update);

                    ZipArchiveEntry inputEntry = inputArchive.GetEntry(entryNameInput);
                    if (inputEntry == null)
                    {
                        MessageBox.Show(
                        $"File '{entryNameInput}' not found in archive '{zipArchiveInput}'",
                        "Input file not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    ZipArchiveEntry outputEntry = GetEntry(entryNameOutput, true, outputArchive);
                    if (outputEntry == null) return false;

                    using StreamReader inputStream  = new StreamReader(inputEntry.Open());
                    using StreamWriter outputStream = new StreamWriter(outputEntry.Open());

                    inputStream.BaseStream.CopyTo(outputStream.BaseStream);
                    outputStream.Flush();

                    inputStream.Close();
                    outputStream.Close();
                    return true;
                }
                catch (FileNotFoundException)
                {
                    return false;
                }
                catch (InvalidDataException e)
                {
                    MessageBox.Show(
                        $"Zip file '{zipArchiveInput}' or '{zipArchiveOutput}' is corrupted. Delete it and retry action.\n" +
                        $"{e.Message}",
                        "Cannot access file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                catch (IOException e)
                {
                    result = MessageBox.Show(
                        $"Zip file '{zipArchiveInput}' or '{zipArchiveOutput}' is open in another program. Close it and retry action.\n" +
                         $"{e.Message}",
                        "Cannot access file", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    if (result == DialogResult.Cancel) return false;
                }
                catch (Exception e)
                {
                    MessageBox.Show(
                        "Error thrown, message:\n" +
                        $"{e.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            return false;
        }
    }
}
