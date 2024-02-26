using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

using CruZ.Resource;

namespace CruZ.Editor
{
    class FileUITypeEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext? context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object? EditValue(ITypeDescriptorContext? context, IServiceProvider provider, object? value)
        {
            OpenFileDialog openFileDialog = new()
            {
                InitialDirectory = ResourceManager.User.ResourceRoot,
                RestoreDirectory = true,
                Title = "Select File",
                Filter = "All Files|*.*",
                Multiselect = false,
            };

            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog.FileName;
            }

            return value;
        }
    }
}
