using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace SnippetManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string baseTitle = "SnippetManager: ";
        private String currentFile;
        Snippets snippets = new Snippets();
        private bool fileHasChanged = false;
        private bool searchItemFound = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void menuExit_Click(object sender, RoutedEventArgs e)
        {
            if (fileHasChanged)
            {
                switch (MessageBox.Show("Save changes before exiting?", "Save Changes?", MessageBoxButton.YesNoCancel, MessageBoxImage.Question))
                {
                    case MessageBoxResult.Yes:
                        handleSave();
                        this.Close();
                        break;
                    case MessageBoxResult.No:
                        this.Close();
                        break;
                    case MessageBoxResult.Cancel:
                        break;

                }
            } else
            {
                this.Close();
            }
        }

        private void menuOpen_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
            openFileDlg.DefaultExt = ".snp";
            openFileDlg.Filter = "Snippet Files (*.snp)|*.snp";

            Nullable<bool> result = openFileDlg.ShowDialog();
            if (result == true)
            {
                currentFile = openFileDlg.FileName;
                windowMain.Title = baseTitle + currentFile;

                snippets = new Snippets();
                snippets.loadFromFile(currentFile);

                dataGridSnippets.ItemsSource = snippets.SnippetList;
                listBoxCategories.ItemsSource = snippets.getCategories();
                listBoxCategories.SelectedItem = null;
                menuSave.IsEnabled = true;
            }
        }

        private void categoryFilter(object sender, FilterEventArgs e)
        {
            var obj = e.Item as Snippet;
            if (obj != null)
            {
                if (listBoxCategories.SelectedItem == null || (obj.Category != null && obj.Category.Contains(listBoxCategories.SelectedItem.ToString())))
                    e.Accepted = true;
                else
                    e.Accepted = false;
            }
        }

        private void searchFilter(object sender, FilterEventArgs e)
        {
            var obj = e.Item as Snippet;

            if (obj != null)
            {
                if (obj.Content != null && obj.Content.Contains(textSearch.Text))
                {
                    e.Accepted = true;
                    searchItemFound = true;
                }
                else
                {
                    e.Accepted = false;
                }
            }
        }

        private void listBoxCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Collection which will take your Filter
            var _itemSourceList = new CollectionViewSource() { Source = snippets.SnippetList };

            //now we add our Filter
            _itemSourceList.Filter += new FilterEventHandler(categoryFilter);

            // ICollectionView the View/UI part 
            ICollectionView Itemlist = _itemSourceList.View;
            dataGridSnippets.ItemsSource = Itemlist;

            textSearch.Text = "";
            searchItemFound = false;
        }

        private void dataGridSnippets_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            Snippet selectedSnippet = dataGridSnippets.SelectedItem as Snippet;
            if (selectedSnippet != null && selectedSnippet.Content != null)
            {
                Clipboard.SetText(selectedSnippet.Content);
            }
        }

        private void menuSave_Click(object sender, RoutedEventArgs e)
        {
            handleSave();
        }

        private void dataGridSnippets_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            Snippet snippet = (((DataGrid) sender).CurrentItem as Snippet);
            if (snippet.Category == null)
            {
                snippet.Category = listBoxCategories.SelectedItem.ToString();
            }
        }

        private void dataGridSnippets_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            handleEndEdit();
        }

        private void dataGridSnippets_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            handleEndEdit();
        }

        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {
            searchItemFound = false;

            if (textSearch.Text.Length > 0)
            {
                // Collection which will take your Filter
                var _itemSourceList = new CollectionViewSource() { Source = snippets.SnippetList };

                //now we add our Filter
                _itemSourceList.Filter += new FilterEventHandler(searchFilter);

                // ICollectionView the View/UI part 
                ICollectionView Itemlist = _itemSourceList.View;
                dataGridSnippets.ItemsSource = Itemlist;

                if (!searchItemFound)
                {
                    MessageBox.Show("Matching text not found.", "Search Results.", MessageBoxButton.OK, MessageBoxImage.Warning);
                    textSearch.Text = "";
                    listBoxCategories.SelectedIndex = 0;
                }
            }
        }

        private void buttonReset_Click(object sender, RoutedEventArgs e)
        {
            textSearch.Text = "";
            listBoxCategories.SelectedIndex = 0;
        }

        private void handleSave()
        {
            snippets.saveToFile(currentFile);
            fileHasChanged = false;
            windowMain.Title = baseTitle + currentFile;
            menuSave.IsEnabled = false;
        }

        private void handleEndEdit()
        {
            if (fileHasChanged == false)
            {
                fileHasChanged = true;
                windowMain.Title = baseTitle + currentFile + "*";
                menuSave.IsEnabled = true;
            }
        }

        private void windowMain_Loaded(object sender, RoutedEventArgs e)
        {
            listBoxCategories.SelectedIndex = 0;
        }
    }
}
