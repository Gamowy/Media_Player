using System.Windows;

namespace Media_Player
{
    /// <summary>
    /// Interaction logic for AddTrackWindow.xaml
    /// </summary>
    public partial class AddTrackWindow : Window
    {
        public AddTrackWindow(string mode)
        {
            InitializeComponent();
            switch (mode)
            {
                case "dodaj":
                    this.Window.Title = "Dodaj nowy utwór";
                    this.WindowLabel.Content = "DODAJ NOWY UTWÓR";
                    this.AddEditButton.Content = "Dodaj";
                    break;
                case "edytuj":
                    this.Window.Title = "Edytuj utwór";
                    this.WindowLabel.Content = "EDYTUJ UTWÓR";
                    this.AddEditButton.Content = "Edytuj";
                    break;
            }
        }
    }
}
