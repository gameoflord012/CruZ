using Accessibility;
using CruZ.Editor.Binding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CruZ.Editor.Controls
{
    partial class Vector3InspectorControl : UserControl
    {
        //public event EventHandler<Vector3> OnInputValueChanged;

        public Vector3InspectorControl()
        {
            InitializeComponent();
            AutoValidate = AutoValidate.Disable;

            xValue.KeyDown += (sender, a) => { if (a.KeyCode == Keys.Enter) ValidateInput(); };
            yValue.KeyDown += (sender, a) => { if (a.KeyCode == Keys.Enter) ValidateInput(); };
            zValue.KeyDown += (sender, a) => { if (a.KeyCode == Keys.Enter) ValidateInput(); };

            xValue.LostFocus += (sender, a) => ValidateInput();
            yValue.LostFocus += (sender, a) => ValidateInput();
            zValue.LostFocus += (sender, a) => ValidateInput();
        }

        private void ValidateInput()
        {
            Vector3 v3;

            if (!float.TryParse(xValue.Text, out v3.X))
            {
                xValue.Text = _vector3.X.ToString();
                xValue.Update();
                return;
            }

            if (!float.TryParse(yValue.Text, out v3.Y))
            {
                yValue.Text = _vector3.Y.ToString();
                yValue.Update();
                return;
            }

            if (!float.TryParse(zValue.Text, out v3.Z))
            {
                zValue.Text = _vector3.Z.ToString();
                zValue.Update();
                return;
            }

            _vector3 = v3;
            _binding?.Set(_vector3);
        }

        public void SetValueText(Vector3 v3)
        {
            if (!xValue.Focused)
                xValue.Text = v3.X.ToString();

            if(!yValue.Focused)
                yValue.Text = v3.Y.ToString();

            if(!zValue.Focused)
                zValue.Text = v3.Z.ToString();

            Update();
        }

        public void SetPropertyName(string name)
        {
            propertyName_TextBox.Text = name;
        }

        public void SetBinding(Binding.PropertyBinding<Vector3> propertyBinding)
        {
            if(_binding != null) 
                _binding.UnregisterChangeEvent(Binding_OnDataChanged);

            _binding = propertyBinding;
            _binding.RegisterChangeEvent(Binding_OnDataChanged);

            SetValueText(_binding.Get());
        }

        private void Binding_OnDataChanged(object v)
        {
            SetValueText(_binding.Get());
        }

        PropertyBinding<Vector3>? _binding;
        Vector3 _vector3;
    }
}
