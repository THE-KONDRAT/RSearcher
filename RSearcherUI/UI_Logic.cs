using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RSearcherUI
{
    public static class UI_Logic
    {
        public static void SetNewItem(Control control, TreeView treeView, string startFolder, string filePath)
        {
            if (treeView.Nodes.Count < 1)
            {
                CreateRootNode(control, treeView, startFolder);
            }

            String[] dirArray = FileSearcher.FileSearcher.GetFileDirectoriesArray(filePath);
            string filename = FileSearcher.FileSearcher.GetFileName(filePath);
            TreeNode currentNode = FindNodeByName(treeView.Nodes, startFolder);
            TreeNode nextNode = currentNode;
            if (currentNode == null)
            {
                //throw new exception?
                return;
            }

            if (dirArray != null && !string.IsNullOrWhiteSpace(filename))
            {
                for (int level = 1; level < dirArray.Length + 1; level++)
                {
                    CheckCreateNode(control, treeView, currentNode, ref nextNode, dirArray[level - 1]);
                    currentNode = nextNode;
                }
            }

            //Files
            CheckCreateNode(control, treeView, currentNode, ref nextNode, filename);
        }

        private static TreeNode FindNodeByName(TreeNodeCollection tnc, string name)
        {
            if (tnc == null) return null;
            TreeNode[] tn_arr = tnc.Find(name, false);
            if (tn_arr == null) return null;
            else if (tn_arr.Length < 1) return null;
            else return tn_arr[0];
        }

        private static void CheckCreateNode(Control control, TreeView treeView, TreeNode currentNode, ref TreeNode nextNode, string name)
        {
            nextNode = FindNodeByName(currentNode.Nodes, name);
            if (!string.IsNullOrWhiteSpace(name))
            {
                if (nextNode == null)
                {
                    UI_Logic.CreateNewNode(control, treeView, currentNode, name);
                    nextNode = FindNodeByName(currentNode.Nodes, name);
                }
            }
        }

        public static void CreateNewNode(Control control, TreeView treeView, TreeNode node, string name)
        {
            if (treeView.InvokeRequired)
            {
                control.Invoke(
                    new MethodInvoker(() => node.Nodes.Add(name, name))
                );
            }
            else
            {
                node.Nodes.Add(name, name);
            }
        }

        public static void CreateRootNode(Control control, TreeView treeView, string name)
        {

            if (treeView.InvokeRequired)
            {
                control.Invoke(
                    new MethodInvoker(() => treeView.Nodes.Add(name, name))
                );
            }
            else
            {
                treeView.Nodes.Add(name, name);
            }
        }

        #region Status (time left, files counter)
        public static string StatusString(Stopwatch sw, uint filesCounter)
        {
            return $"{FileSearcher.FileSearcher.GetElapsedTimeString(sw)}" + Environment.NewLine + 
                $"Files found: {FilesCountStr(filesCounter)}";
        }

        private static string FilesCountStr(uint filesCounter)
        {
            if (filesCounter > (Math.Pow(10, 5))) return Convert.ToString(filesCounter) + "K";
            else if (filesCounter > (Math.Pow(10, 5))) return Convert.ToString(filesCounter) + "M";
            else return Convert.ToString(filesCounter);
        }
        #endregion

        public static void SetObjProp(Control control, object sender, string propName, dynamic propValue)
        {
            if (sender.GetType().GetProperty(propName) != null)
            {
                if (sender.GetType().GetProperty("InvokeRequired") != null)
                {
                    if ((bool)sender.GetType().GetProperty("InvokeRequired").GetValue(sender) == true)
                    {
                        control.Invoke(new MethodInvoker(() => sender.GetType().GetProperty(propName).SetValue(sender, propValue)));
                    }
                    else
                    {
                        sender.GetType().GetProperty(propName).SetValue(sender, propValue);
                    }
                }
            }
        }

        public static dynamic GetObjProp(Control control ,object sender, string propName)
        {
            if (sender.GetType().GetProperty(propName) != null)
            {
                if (sender.GetType().GetProperty("InvokeRequired") != null)
                {
                    if ((bool)sender.GetType().GetProperty("InvokeRequired").GetValue(sender) == true)
                    {
                        return control.Invoke(new Func<dynamic>(() => sender.GetType().GetProperty(propName).GetValue(sender)));
                    }
                    else
                    {
                        return sender.GetType().GetProperty(propName).GetValue(sender);
                    }
                }
                else return null;
            }
            else return null;
        }

        public static string TrimString(string s)
        {
            if (!string.IsNullOrEmpty(s)) return s.Trim();
            else return s;
        }
    }

    class BufferedTreeView : TreeView
    {
        protected override void OnHandleCreated(EventArgs e)
        {
            SendMessage(this.Handle, TVM_SETEXTENDEDSTYLE, (IntPtr)TVS_EX_DOUBLEBUFFER, (IntPtr)TVS_EX_DOUBLEBUFFER);
            base.OnHandleCreated(e);
        }
        // Pinvoke:
        private const int TVM_SETEXTENDEDSTYLE = 0x1100 + 44;
        private const int TVM_GETEXTENDEDSTYLE = 0x1100 + 45;
        private const int TVS_EX_DOUBLEBUFFER = 0x0004;
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
    }
}
