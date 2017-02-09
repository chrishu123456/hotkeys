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
using Microsoft.Win32;
using System.IO;
using System.ComponentModel;


namespace hotkeys
{
    /// <summary>
    /// Interaction logic for BarWindow.xaml
    /// </summary>
    public partial class BarWindow : Window
    {
        public static RoutedCommand mijnRoutectrlb = new RoutedCommand();
        public static RoutedCommand mijnRoutectrli = new RoutedCommand();

        private double A4Breedte = 21 /2.54 * 96;
        private double A4Hoogte = 29.7 /2.54 * 96;
        private double vertPositie;

        public BarWindow()
        {
            InitializeComponent();

            CommandBinding mijnCtrlB = new CommandBinding(mijnRoutectrlb, ctrlbExecuted);
            this.CommandBindings.Add(mijnCtrlB);

            KeyGesture toetsCtrlB = new KeyGesture(Key.B, ModifierKeys.Control);
            KeyBinding mijnKeyCtrlB = new KeyBinding(mijnRoutectrlb, toetsCtrlB);
            this.InputBindings.Add(mijnKeyCtrlB);

            CommandBinding mijnCtrlI = new CommandBinding(mijnRoutectrli, ctrliExecuted);
            this.CommandBindings.Add(mijnCtrlI);

            KeyGesture toetsCtrlI = new KeyGesture(Key.I, ModifierKeys.Control);
            KeyBinding mijnKeyCtrlI = new KeyBinding(mijnRoutectrli, toetsCtrlI);
            this.InputBindings.Add(mijnKeyCtrlI);
        }

        private void Vet_Aan_Uit(Boolean wissel = false)
        {
            if (( wissel == true && TextBoxVoorbeeld.FontWeight == FontWeights.Bold) || (wissel == false && TextBoxVoorbeeld.FontWeight == FontWeights.Normal))
            {
                TextBoxVoorbeeld.FontWeight = FontWeights.Bold;
                Vet.IsChecked = true;
                ButtonVet.IsChecked = true;
                StatusVet.FontWeight = FontWeights.Bold;
            }
            else
            {
                TextBoxVoorbeeld.FontWeight = FontWeights.Normal;
                Vet.IsChecked = false;
                ButtonVet.IsChecked = false;
                StatusVet.FontWeight = FontWeights.Normal;
            }
        }

        private void Vet_Click(object sender, RoutedEventArgs e)
        {
            Vet_Aan_Uit();
        }

        private void Schuin_Aan_Uit(Boolean wissel = false)
        {
            if ((wissel == true && TextBoxVoorbeeld.FontStyle == FontStyles.Italic) || (wissel == false && TextBoxVoorbeeld.FontStyle == FontStyles.Normal ))
            {
                TextBoxVoorbeeld.FontStyle = FontStyles.Italic;
                Schuin.IsChecked = true;
                ButtonSchuin.IsChecked = true;
                StatusSchuin.FontStyle = FontStyles.Italic;
            }
            else
            {
                TextBoxVoorbeeld.FontStyle = FontStyles.Normal;
                Schuin.IsChecked = false;
                ButtonSchuin.IsChecked = false;
                StatusSchuin.FontStyle = FontStyles.Normal;
            }
        }

        private void Schuin_Click(object sender, RoutedEventArgs e)
        {
            Schuin_Aan_Uit();
        }

        private void Lettertype_Click(object sender, RoutedEventArgs e)
        {
            MenuItem hetLettertype = (MenuItem)sender;

            foreach (MenuItem huidig in Fontjes.Items)
            {
                huidig.IsChecked = false;
            }
            hetLettertype.IsChecked = true;

            LetterTypeCombobox.SelectedItem = 
                new FontFamily(hetLettertype.Header.ToString());
        }

        private void ctrlbExecuted(object sender, ExecutedRoutedEventArgs s)
        {
            Vet_Aan_Uit();
        }

        private void ctrliExecuted(object sender, ExecutedRoutedEventArgs s)
        {
            Schuin_Aan_Uit();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LetterTypeCombobox.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Source", System.ComponentModel.ListSortDirection.Ascending));
            LetterTypeCombobox.SelectedItem = new FontFamily(TextBoxVoorbeeld.FontFamily.ToString());
        }

        private void LetterTypeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (MenuItem huidig in Fontjes.Items)
            {
                if (LetterTypeCombobox.SelectedItem.ToString() == huidig.Header.ToString())
                    huidig.IsChecked = true;
                else
                    huidig.IsChecked = false;
            }
        }

        private void SaveExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.FileName = "Document";
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text Documents | *.txt";

                if (dlg.ShowDialog() == true)
                {
                    using (StreamWriter bestand = new StreamWriter(dlg.FileName))
                    {
                        
                        bestand.WriteLine(LetterTypeCombobox.SelectedValue);
                        bestand.WriteLine(TextBoxVoorbeeld.FontWeight.ToString());
                        bestand.WriteLine(TextBoxVoorbeeld.FontStyle.ToString());
                        bestand.WriteLine(TextBoxVoorbeeld.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Opslaan mislukt : " + ex.Message);
            }
        }

        private void OpenExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.FileName = "Document";
                dlg.DefaultExt = "*.txt";
                dlg.Filter = "Text Documents | *.txt";

             
                if (dlg.ShowDialog() == true)
                {
                    using (StreamReader bestand = new StreamReader(dlg.FileName))
                    {
                        LetterTypeCombobox.SelectedValue = new FontFamily(bestand.ReadLine());

                        TypeConverter convertBold = TypeDescriptor.GetConverter(typeof(FontWeight));
                        TextBoxVoorbeeld.FontWeight = (FontWeight)convertBold.ConvertFromString(bestand.ReadLine());
                        Vet_Aan_Uit(true);
                        TypeConverter convertStyle = TypeDescriptor.GetConverter(typeof(FontStyle));
                        TextBoxVoorbeeld.FontStyle = (FontStyle)convertStyle.ConvertFromString(bestand.ReadLine());
                        Schuin_Aan_Uit(true);

                        TextBoxVoorbeeld.Text = bestand.ReadLine();
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("openen mislukt : " + ex.Message);
            }
           
        }



        private void PrintExecuted(object sender, ExecutedRoutedEventArgs e)
        {

        }
    }
}
