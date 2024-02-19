using System.ComponentModel;
using System.Drawing.Design;

namespace CruZ.Editor
{
    class FileUITypeEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext? context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }
}
