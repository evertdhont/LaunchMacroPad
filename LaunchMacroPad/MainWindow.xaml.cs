using Commons.Music.Midi;
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

namespace LaunchMacroPad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IMidiAccess access;
        private IMidiInput input;
        private IMidiOutput output;
        private LaunchPadManager lpmngr;

        public MainWindow()
        {
            InitializeComponent();

            access = MidiAccessManager.Default;

            LaunchPadInputs.ItemsSource = access.Inputs.ToList().Select((dev) => { return dev;  });
            LaunchPadOutputs.ItemsSource = access.Outputs.ToList().Select((dev) => { return dev; });

            LaunchPadInputs.DisplayMemberPath = "Name";
            LaunchPadOutputs.DisplayMemberPath = "Name";

            LaunchPadInputs.SelectedValuePath = "Id";
            LaunchPadOutputs.SelectedValuePath = "Id";

            LaunchPadInputs.SelectedIndex = 0;
            LaunchPadOutputs.SelectedIndex = 1;

        }

        private void ConfirmButtonPressed(object sender, RoutedEventArgs e)
        {
            input = access.OpenInputAsync(LaunchPadInputs.SelectedValue.ToString()).Result;
            output = access.OpenOutputAsync(LaunchPadOutputs.SelectedValue.ToString()).Result;
            
            try
            {
                lpmngr = new LaunchPadManager(access, input, output);

                InitMacroGrid();

            } catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "LaunchPad Macros", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void InitMacroGrid()
        {
            int counter = 0;

            for (int i = 0; i < 8; i++)
            {
                MacroGrid.RowDefinitions.Add(new RowDefinition());
                for (int j = 0; j < 8; j++)
                {
                    if (i == 0) MacroGrid.ColumnDefinitions.Add(new ColumnDefinition());
                    var button = new Button
                    {
                        Tag = $"{counter + (i * 8)}",
                        Name = $"btn{counter++}",
                        Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#d6dbd7")),
                        Content = counter
                    };
                    button.Click += MacroButtonPressed;
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    MacroGrid.Children.Add(button);
                }
            }
        }

        private void MacroButtonPressed(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;

            lpmngr.SendNote(int.Parse(btn.Tag.ToString()), 84);
        }
    }
}
