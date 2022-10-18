using CSharp.Functional.Constructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CSharp.Functional.Extensions.OptionExtension;

namespace TimeLinerOptimize.App.UI.Services
{
    public enum ResultMessageType
    {
        DONE,
        ERROR,
        WARNING
    }
    public class Services
    {
        public static Option<string> FolderSaveService()
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                    return Some(dialog.SelectedPath);
                else
                    return None;
            }
        }

        public static Option<string> FileOpenService()
        {
            var outputDialog = new Microsoft.Win32.OpenFileDialog();

            outputDialog.DefaultExt = ".csv";
            outputDialog.Filter = "CSV Files (.csv)|*.csv";
            var result = outputDialog.ShowDialog();
            if (result == true)
                return Some(outputDialog.FileName);
            else
                return None;
        }
    }
}
