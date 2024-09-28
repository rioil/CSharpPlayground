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

namespace WpfUserControl.Controls
{
    /// <summary>
    /// RangeInputBox.xaml の相互作用ロジック
    /// </summary>
    public partial class RangeInputBox : UserControl
    {
        public RangeInputBox()
        {
            InitializeComponent();
        }

        public bool AllowOutOfRange
        {
            get { return (bool)GetValue(AllowOutOfRangeProperty); }
            set { SetValue(AllowOutOfRangeProperty, value); }
        }
        public static readonly DependencyProperty AllowOutOfRangeProperty =
            DependencyProperty.Register("AllowOutOfRange", typeof(bool), typeof(RangeInputBox), new PropertyMetadata(0));


    }
}
