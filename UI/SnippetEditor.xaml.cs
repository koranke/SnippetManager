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
using System.Windows.Shapes;

namespace SnippetManager
{
    /// <summary>
    /// Interaction logic for SnippetEditor.xaml
    /// </summary>
    public partial class SnippetEditor : Window
    {
        public bool isSaveAction = false;

        public SnippetEditor()
        {
            InitializeComponent();
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            //some save action
            isSaveAction = true;
            this.Close();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
