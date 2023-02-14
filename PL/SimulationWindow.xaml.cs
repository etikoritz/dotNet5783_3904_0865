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

namespace PL;

/// <summary>
/// Interaction logic for SimulationWindow.xaml
/// </summary>
public partial class SimulationWindow : Window
{
    public SimulationWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// click on button to end the simulation
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void EndSimulation_Click(object sender, RoutedEventArgs e)
    {

    }

    /// <summary>
    /// Method to block the option to close the window
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        bool _myClosing = false;
        if (!_myClosing)
        { // Won't allow to cancel the window!!! It is not me!!!
            e.Cancel = true;
            MessageBox.Show("Can't close window during simulation!");
        }
    }
}
