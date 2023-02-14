﻿using System;
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
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for SimulatorWindow.xaml
    /// </summary>
    public partial class SimulatorWindow : Window
    {
        private BackgroundWorker simulationWorker;
        private bool simulationRunning = true;
        public SimulatorWindow()
        {
            InitializeComponent();
            simulationWorker = new BackgroundWorker();
            simulationWorker.DoWork += SimulationWorker_DoWork;
            simulationWorker.ProgressChanged += SimulationWorker_ProgressChanged;
            simulationWorker.WorkerReportsProgress = true;
            simulationWorker.WorkerSupportsCancellation = true;
            simulationWorker.RunWorkerAsync();
        }
    }
}
