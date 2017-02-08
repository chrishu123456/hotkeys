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

namespace hotkeys
{
    /// <summary>
    /// Interaction logic for BarWindow.xaml
    /// </summary>
    public partial class BarWindow : Window
    {
        public static RoutedCommand mijnRoutectrlb = new RoutedCommand();
        public static RoutedCommand mijnRoutectrli = new RoutedCommand();

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

        private void Vet_Aan_Uit()
        {
            if (TextBoxVoorbeeld.FontWeight == FontWeights.Normal)
            {
                TextBoxVoorbeeld.FontWeight = FontWeights.Bold;
                MenuVet.IsChecked = true;
            }
            else
            {
                TextBoxVoorbeeld.FontWeight = FontWeights.Normal;
                MenuVet.IsChecked = false;
            }
        }

        private void Vet_Click(object sender, RoutedEventArgs e)
        {
            Vet_Aan_Uit();
        }

        private void Schuin_Aan_Uit()
        {
            if (TextBoxVoorbeeld.FontStyle == FontStyles.Normal)
            {
                TextBoxVoorbeeld.FontStyle = FontStyles.Italic;
                MenuSchuin.IsChecked = true;
            }
            else
            {
                TextBoxVoorbeeld.FontStyle = FontStyles.Normal;
                MenuSchuin.IsChecked = false;
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

            TextBoxVoorbeeld.FontFamily = 
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

    }
}
