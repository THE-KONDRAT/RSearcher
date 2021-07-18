using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Serialization;

namespace FileSearcher
{
    public class FileSearcher
    {
        CancellationTokenSource _cancelSource;
        private Regex _regex = null;
        public delegate void SetFileEventHandler(String FilePath);
        public SetFileEventHandler OnSetFile;

        public delegate void FinishProcessEventHandler();
        public FinishProcessEventHandler OnFinish;

        public delegate void CancelProcessEventHandler();
        public FinishProcessEventHandler OnCancel;

        public bool Cancel
        {
            set
            {
                if (_cancelSource == null)
                {
                    _cancelSource = new CancellationTokenSource();
                }
                if (value == true)
                {
                    _cancelSource.Cancel();
                }
                else
                {
                    _cancelSource.Dispose();
                    _cancelSource = new CancellationTokenSource();
                }
            }
        }

        public FileSearcher()
        {
            _cancelSource = new CancellationTokenSource();
        }

        public void StartSearch(string startFolder, string regexMask)
        {
            _regex = new Regex(regexMask);
            SearchFiles(startFolder);
        }

        private void GetDirectories(string folder)
        {
            DirectoryInfo d = new DirectoryInfo(folder);
            DirectoryInfo[] d_arr = d.GetDirectories();
            if (d_arr != null)
            {
                if (d_arr.Length > 0)
                {
                    foreach (DirectoryInfo di in d_arr)
                    {
                        if (CheckCancellation()) return;

                        try
                        {
                            GetDirectoryFiles(d);
                            GetDirectories(di.FullName);
                        }
                        catch (Exception e)
                        {
                            //can't access
                        }
                    }
                }
                else GetDirectoryFiles(d);
            }
            else
            {
                GetDirectoryFiles(d);
            }
        }

        private void GetDirectoryFiles(DirectoryInfo di)
        {
            FileInfo[] f = di.GetFiles();
            if (f != null)
            {
                foreach (FileInfo fi in f)
                {
                    if (CheckCancellation()) return;
                    CheckMatch(fi);
                }
            }
        }

        private void CheckMatch(FileInfo fi)
        {
            if (_regex != null)
            {
                if (_regex.IsMatch(fi.Name)) SetFile(fi);
            }
        }

        private void SetFile(FileInfo fi)
        {
            OnSetFile?.Invoke(fi.FullName);
        }

        private void SearchFiles(string startFolder)
        {
            GetDirectories(startFolder);

            OnFinish?.Invoke();
        }

        private bool CheckCancellation()
        {
            if (_cancelSource.IsCancellationRequested)
            {
                OnCancel?.Invoke();
                return true;
            }
            else return false;
        }

        #region FilePath methods
        public static string[] GetFileDirectoriesArray(string filePath)
        {
            if (Path.IsPathRooted(filePath))
            {
                string s = filePath.Replace(Path.GetPathRoot(filePath), "")
                    .Replace(Path.GetFileName(filePath), "")
                    .TrimEnd(Path.DirectorySeparatorChar);

                if (s.Contains(Path.DirectorySeparatorChar.ToString()))
                {
                    return s.Split(Path.DirectorySeparatorChar);
                }
                else
                {
                    return null;
                }
                
            }
            else
            {
                return filePath.Replace(Path.GetFileName(filePath), "")
                    .TrimEnd(Path.DirectorySeparatorChar).Split(Path.DirectorySeparatorChar);
            }
        }

        public static string GetFullPath(string path)
        {
            return Path.GetFullPath(path);
        }

        public static string GetFileName(string filePath)
        {
            return Path.GetFileName(filePath);
        }

        public static void ValidatePath(string path)
        {
            bool success = true;
            string err = null;

            FileAttributes attr;
            try
            {
                attr = File.GetAttributes(path);
                if ((attr & FileAttributes.Directory) != FileAttributes.Directory)
                {
                    err = "Selected path is not folder";
                    success = false;
                }
            }
            catch
            {
                
            }
            

            if (!Directory.Exists(path))
            {
                err = "Selected folder is not exists";
                success = false;
            }
            //if (path == path.Replace(Path.GetFileName(path), ""))

            if (!success) throw new Exception(err);
        }
        #endregion

        public static string GetElapsedTimeString(Stopwatch t)
        {
            string s = "Time elapsed: ";

            if (t.Elapsed.Hours < 1) s += $"{t.Elapsed.Minutes}m:{t.Elapsed.Seconds}s:{t.Elapsed.Milliseconds}ms";
            else if (t.Elapsed.Hours < 24) s += $"{t.Elapsed.Hours}h:{t.Elapsed.Minutes}m:{t.Elapsed.Seconds}s";
            else if (t.Elapsed.Days < 365) s += $"{t.Elapsed.Days}d {t.Elapsed.Hours}h:{t.Elapsed.Minutes}m";
            else s += t.Elapsed.Days + "d";
            return s;
        }
    }

    [XmlRootAttribute("Settings", Namespace = "", IsNullable = false)]
    public class UserParams { 

        private static string filepath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\sets.xml";
        public String StartFolder { get; set; }
        public String RegexMask { get; set; }

        public void SaveParams()
        {
            ObjectXMLSerializer<UserParams>.Save(this, filepath);
        }

        public void LoadParams()
        {
            if (File.Exists(filepath))
            {
                UserParams up = ObjectXMLSerializer<UserParams>.Load(filepath);
                if (up != null)
                {
                    StartFolder = up.StartFolder;
                    RegexMask = up.RegexMask;
                }
            }
        }

        public static void DeleteSettingsFile()
        {
            try
            {
                if (File.Exists(filepath)) File.Delete(filepath);
            }
            catch
            {

            }
        }
    }
}
