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

        private TextBlock Regel(string tekst)
        {
            TextBlock deRegel = new TextBlock();

            deRegel.Text = tekst;
            deRegel.FontFamily = TextBoxVoorbeeld.FontFamily;
            deRegel.FontSize = TextBoxVoorbeeld.FontSize;
            deRegel.FontStyle = TextBoxVoorbeeld.FontStyle;
            deRegel.FontWeight = TextBoxVoorbeeld.FontWeight;
            deRegel.Margin = new Thickness(96, vertPositie, 96, 96);
            vertPositie += 30;

            return deRegel;
        }

        private FixedDocument StelAfdrukSamen()
        {
            FixedDocument document = new FixedDocument();
            document.DocumentPaginator.PageSize = new System.Windows.Size(A4Breedte, A4Hoogte);

            PageContent inhoud = new PageContent();
            document.Pages.Add(inhoud);  // (fixeddocument) document heeft een pagesize en pages (inhoud)

            FixedPage page = new FixedPage();
            inhoud.Child = page;  // (pagecontent) inhoud heeft een page

            page.Width = A4Breedte;
            page.Height = A4Hoogte;
            vertPositie = 96;

            page.Children.Add(Regel("Gebruikte lettertype : " + TextBoxVoorbeeld.FontFamily.ToString()));  // (fixedpage) page heeft sizes en regels
            page.Children.Add(Regel("Gewicht van het lettertype : " + TextBoxVoorbeeld.FontWeight.ToString()));
            page.Children.Add(Regel("Stijl van  het lettertype : " + TextBoxVoorbeeld.FontStyle.ToString()));
            page.Children.Add(Regel("Dikte van het lettertype : " + TextBoxVoorbeeld.FontSize.ToString()));
            page.Children.Add(Regel(" "));
            page.Children.Add(Regel("Tekst"));
            for (int i = 0; i < TextBoxVoorbeeld.LineCount;i++)
            {
                page.Children.Add(Regel(TextBoxVoorbeeld.GetLineText(i)));
            }

            return document;
            






        }
        private void PrintExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            PrintDialog afdrukken = new PrintDialog();

            if (afdrukken.ShowDialog() == true)
            {
                afdrukken.PrintDocument(StelAfdrukSamen().DocumentPaginator, "tekstbox");
            }
        }

        private void PrintPreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Afdrukvoorbeeld preview = new Afdrukvoorbeeld();

            preview.Owner = this;
            preview.AfdrukDocument = StelAfdrukSamen();
            preview.ShowDialog();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (MessageBox.Show("Programma afsluiten ?", "Close", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No) e.Cancel = true; 
        }

        private void CloseExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
