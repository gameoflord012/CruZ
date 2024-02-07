//using System;
//using System.ComponentModel;
//using System.Drawing.Design;
//using System.Windows.Forms;
//using System.Windows.Forms.Design;

//namespace CruZ.Editor
//{
//    public class TrackBarUITypeEditor : UITypeEditor
//    {
//        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext? context)
//        {
//            return UITypeEditorEditStyle.DropDown;
//        }

//        public override object? EditValue(ITypeDescriptorContext? context, IServiceProvider provider, object? value)
//        {
//            IWindowsFormsEditorService? editorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
        
//            if(editorService != null)
//            {
//                TrackBar trackBar = new TrackBar();
//                trackBar.Minimum = 0;
//                trackBar.Maximum = 100; // You can adjust these values based on your needs
//                trackBar.Value = (int)((Vector3)value).X;

//                EventHandler valueChangedHandler = (sender, e) =>
//                {
//                    value = trackBar.Value;
//                };

//                trackBar.ValueChanged += valueChangedHandler;

//                editorService.DropDownControl(trackBar);

//                trackBar.ValueChanged -= valueChangedHandler;

//                return new Vector3((int)value);
//            }

//            return value;
//        }
//    }
//}