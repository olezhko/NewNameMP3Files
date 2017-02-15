using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EditTags.Skins
{
    /// <summary>
    /// Interaction logic for EditFields.xaml
    /// </summary>
    public partial class EditFields : UserControl
    {
        public EditFields()
        {
            InitializeComponent();
        }

        #region Property
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(EditFields), new PropertyMetadata(default(string)));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }




        public static readonly DependencyProperty FieldProperty = DependencyProperty.Register(
            "Field", typeof(string), typeof(EditFields), new PropertyMetadata(default(string)));

        public string Field
        {
            get { return (string)GetValue(FieldProperty); }
            set { SetValue(FieldProperty, value); }
        }



        public static readonly DependencyProperty ChangedFieldProperty = DependencyProperty.Register(
            "ChangedField", typeof(string), typeof(EditFields), new PropertyMetadata(default(string)));

        public string ChangedField
        {
            get { return (string)GetValue(ChangedFieldProperty); }
            set { SetValue(ChangedFieldProperty, value); }
        }
        #endregion
        
    }
}
