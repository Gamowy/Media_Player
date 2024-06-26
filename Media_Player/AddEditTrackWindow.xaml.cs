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
            switch(mode) 
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
