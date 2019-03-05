using System.Windows;
using System.Windows.Controls;
using PcMonitor.DataObjects;

namespace PcMonitor.Ui.Controls
{
    /// <summary>
    /// Interaction logic for DiskControl.xaml
    /// </summary>
    public partial class DiskControl : UserControl
    {
        /// <summary>
        /// Creates a new instance of the <see cref="DiskControl"/>
        /// </summary>
        public DiskControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The dependency property for <see cref="DiskValue"/>
        /// </summary>
        public static readonly DependencyProperty DiskValueProperty = DependencyProperty.Register(
            nameof(DiskValue), typeof(DiskModel), typeof(DiskControl), new PropertyMetadata(default(DiskModel)));

        /// <summary>
        /// Gets or sets the disk value
        /// </summary>
        public DiskModel DiskValue
        {
            get => (DiskModel) GetValue(DiskValueProperty);
            set => SetValue(DiskValueProperty, value);
        }
    }
}
