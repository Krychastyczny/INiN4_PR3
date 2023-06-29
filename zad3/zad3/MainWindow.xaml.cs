using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace zad3
{
    public enum MediaType
    {
        VHS,
        DVD,
        BlueRay,
        Kaseta,
        CD,
        Pendrive
    }

    public class Item : INotifyPropertyChanged
    {
        private string title;
        private string director;
        private string publisher;
        private MediaType mediaType;
        private TimeSpan duration;

        public string Title
        {
            get 
            { 
                return title; 
            }
            set
            {
                title = value;
                NotifyPropertyChanged();
            }
        }

        public string Director
        {
            get 
            { 
                return director; 
            }
            set
            {
                director = value;
                NotifyPropertyChanged();
            }
        }

        public string Publisher
        {
            get 
            { 
                return publisher; 
            }
            set
            {
                publisher = value;
                NotifyPropertyChanged();
            }
        }

        public MediaType MediaType
        {
            get 
            {
                return mediaType; 
            }
            set
            {
                mediaType = value;
                NotifyPropertyChanged();
            }
        }

        public TimeSpan Duration
        {
            get 
            { 
                return duration; 
            }
            set
            {
                duration = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ItemList : ObservableCollection<Item>
    {
        public void Export(string fileName)
        {
            using (FileStream stream = File.Create(fileName))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
            }
        }

        public static ItemList Import(string fileName)
        {
            using (FileStream stream = File.OpenRead(fileName))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (ItemList)formatter.Deserialize(stream);
            }
        }
    }

    public partial class MainWindow : Window
    {
        private ItemList itemList;

        public MainWindow()
        {
            InitializeComponent();
            itemList = new ItemList();
            DataContext = itemList;
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            Item item = new Item();
            EditWindow editWindow = new EditWindow(item);
            editWindow.Owner = this;

            if (editWindow.ShowDialog() == true)
            {
                itemList.Add(item);
            }
        }

        private void EditButtonClick(object sender, RoutedEventArgs e)
        {
            Item selectedItem = (Item)listBox.SelectedItem;

            if (selectedItem != null)
            {
                EditWindow editWindow = new EditWindow(selectedItem);
                editWindow.Owner = this;
                editWindow.ShowDialog();
            }
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            Item selectedItem = (Item)listBox.SelectedItem;
            if (selectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz usunąć ten element?", "Potwierdzenie usunięcia", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    itemList.Remove(selectedItem);
                }
            }
        }

        private void ImportButtonClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Plik listy (*.dat)|*.dat";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    ItemList importedList = ItemList.Import(openFileDialog.FileName);
                    itemList.Clear();

                    foreach (Item item in importedList)
                    {
                        itemList.Add(item);
                    }

                    MessageBox.Show("Importowanie zakończone pomyślnie.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Błąd podczas importowania: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ExportButtonClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "Plik listy (*.dat)|*.dat";

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    itemList.Export(saveFileDialog.FileName);
                    MessageBox.Show("Eksportowanie zakończone pomyślnie.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Błąd podczas eksportowania: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }

    public partial class EditWindow : Window, INotifyPropertyChanged
    {
        private Item item;

        public EditWindow(Item item)
        {
            InitializeComponent();
            this.item = item;
            DataContext = this.item;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SaveButton(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButton(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void InitializeComponent()
        {
            this.Title = "Edytuj";
            this.Width = 350;
            this.Height = 300;

            Grid grid = new Grid();
            this.Content = grid;

            ColumnDefinition col1 = new ColumnDefinition();
            ColumnDefinition col2 = new ColumnDefinition();
            grid.ColumnDefinitions.Add(col1);
            grid.ColumnDefinitions.Add(col2);

            RowDefinition row1 = new RowDefinition();
            row1.Height = new GridLength(30);
            RowDefinition row2 = new RowDefinition();
            row2.Height = new GridLength(30);
            grid.RowDefinitions.Add(row1);
            grid.RowDefinitions.Add(row2);
            RowDefinition row3 = new RowDefinition();
            row3.Height = new GridLength(30);
            grid.RowDefinitions.Add(row3);
            RowDefinition row4 = new RowDefinition();
            row4.Height = new GridLength(30);
            grid.RowDefinitions.Add(row4);
            RowDefinition row5 = new RowDefinition();
            row5.Height = new GridLength(30);
            grid.RowDefinitions.Add(row5);
            RowDefinition row6 = new RowDefinition();
            row6.Height = new GridLength(30);
            grid.RowDefinitions.Add(row6);

            Label titleLabel = new Label();
            titleLabel.Content = "Tytuł:";
            Grid.SetRow(titleLabel, 0);
            Grid.SetColumn(titleLabel, 0);
            grid.Children.Add(titleLabel);

            Label directorLabel = new Label();
            directorLabel.Content = "Reżyser/Autor:";
            Grid.SetRow(directorLabel, 1);
            Grid.SetColumn(directorLabel, 0);
            grid.Children.Add(directorLabel);

            Label publisherLabel = new Label();
            publisherLabel.Content = "Wydawca/Studio:";
            Grid.SetRow(publisherLabel, 2);
            Grid.SetColumn(publisherLabel, 0);
            grid.Children.Add(publisherLabel);

            Label mediaTypeLabel = new Label();
            mediaTypeLabel.Content = "Nośnik:";
            Grid.SetRow(mediaTypeLabel, 3);
            Grid.SetColumn(mediaTypeLabel, 0);
            grid.Children.Add(mediaTypeLabel);

            Label durationLabel = new Label();
            durationLabel.Content = "Długość:";
            Grid.SetRow(durationLabel, 4);
            Grid.SetColumn(durationLabel, 0);
            grid.Children.Add(durationLabel);

            TextBox titleTextBox = new TextBox();
            titleTextBox.SetBinding(TextBox.TextProperty, new Binding("Title") { Mode = BindingMode.TwoWay });
            Grid.SetRow(titleTextBox, 0);
            Grid.SetColumn(titleTextBox, 1);
            grid.Children.Add(titleTextBox);

            TextBox directorTextBox = new TextBox();
            directorTextBox.SetBinding(TextBox.TextProperty, new Binding("Director") { Mode = BindingMode.TwoWay });
            Grid.SetRow(directorTextBox, 1);
            Grid.SetColumn(directorTextBox, 1);
            grid.Children.Add(directorTextBox);

            TextBox publisherTextBox = new TextBox();
            publisherTextBox.SetBinding(TextBox.TextProperty, new Binding("Publisher") { Mode = BindingMode.TwoWay });
            Grid.SetRow(publisherTextBox, 2);
            Grid.SetColumn(publisherTextBox, 1);
            grid.Children.Add(publisherTextBox);

            ComboBox mediaTypeComboBox = new ComboBox();
            mediaTypeComboBox.ItemsSource = Enum.GetValues(typeof(MediaType));
            mediaTypeComboBox.SetBinding(ComboBox.SelectedItemProperty, new Binding("MediaType") { Mode = BindingMode.TwoWay });
            Grid.SetRow(mediaTypeComboBox, 3);
            Grid.SetColumn(mediaTypeComboBox, 1);
            grid.Children.Add(mediaTypeComboBox);

            TextBox durationTextBox = new TextBox();
            durationTextBox.SetBinding(TextBox.TextProperty, new Binding("Duration") { Mode = BindingMode.TwoWay });
            Grid.SetRow(durationTextBox, 4);
            Grid.SetColumn(durationTextBox, 1);
            grid.Children.Add(durationTextBox);

            Button saveButton = new Button();
            saveButton.Content = "Zapisz";
            saveButton.Click += SaveButton;
            Grid.SetRow(saveButton, 5);
            Grid.SetColumn(saveButton, 0);
            grid.Children.Add(saveButton);

            Button cancelButton = new Button();
            cancelButton.Content = "Anuluj";
            cancelButton.Click += CancelButton;
            Grid.SetRow(cancelButton, 5);
            Grid.SetColumn(cancelButton, 1);
            grid.Children.Add(cancelButton);
        }
    }
}