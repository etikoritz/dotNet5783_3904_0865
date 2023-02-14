using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Simulator;
namespace PL;

/// <summary>
/// Interaction logic for Simulator.xaml
/// </summary>

public partial class SimulationWindow : Window
{
    int DelayMain = 0;
    int r = 0;

    private Stopwatch stopWatch;
    private volatile bool isTimerRun;
    BackgroundWorker timerworker;
    public SimulationWindow()
    {
        InitializeComponent();
        stopWatch = new Stopwatch();
        timerworker = new BackgroundWorker();
        timerworker.DoWork += Worker_DoWork!;
        timerworker.ProgressChanged += Worker_ProgressChanged!;
        timerworker.WorkerReportsProgress = true;
        timerworker.WorkerSupportsCancellation = true;
        timerworker.RunWorkerAsync();
        isTimerRun = true;


    }
    //protected override void OnClosing(CancelEventArgs e)
    //{
    //    e.Cancel= true; 
    //}

    private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
        if (e.ProgressPercentage == 0)
        {

            string timerText = stopWatch.Elapsed.ToString();
            timerText = timerText.Substring(0, 8);
            this.TimerBlock.Text = timerText;
            if (DelayMain != 0)
            {
                progresPer = (DelayMain - r + 1) * (100 / DelayMain);
                r--;
            }

        }
        else
        {
            var args = (Tuple<BO.Order?, DateTime, int>)e.UserState!;
            ID = args.Item1!.ID;
            OldStatus = args.Item1?.Status;

            if (args.Item1?.Status == BO.Enum.OrderStatus.Confirmed)
            {
                StartTime = DateTime.Now;
                NewStatus = BO.Enum.OrderStatus.Shipped;
            }
            else
            {
                NewStatus = BO.Enum.OrderStatus.Delivered;
                StartTime = DateTime.Now;
            }
            ExpectedDate = args.Item2;
            DelayMain = args.Item3;
            r = DelayMain;
            progresPer = 0;
        }
    }




    private void Worker_DoWork(object sender, DoWorkEventArgs e)
    {
        Simulator.Simulator.RegisterForSimulationCompleteEvent(HandleSimulationComplete);
        Simulator.Simulator.RegisterForUpdateEvent(HandleSimulationUpdate);
        Simulator.Simulator.Activate();
        stopWatch.Start();
        while (isTimerRun)
        {
            int index = DelayMain;
            timerworker.ReportProgress(0);
            Thread.Sleep(1000);

        }
    }

    private void stop_simulation(object sender, RoutedEventArgs e)
    {
        Simulator.Simulator.UnregisterFromUpdateEvent(HandleSimulationUpdate);
        Simulator.Simulator.StopSimulation();
        if (isTimerRun)
        {
            stopWatch.Stop();
            isTimerRun = false;
        }
        Close();
    }


    private void HandleSimulationComplete()
    {
        timerworker.CancelAsync();
    }

    private void HandleSimulationUpdate(BO.Order? order, DateTime newTime, int delay)
    {
        var ta = new Tuple<BO.Order?, DateTime, int>(order, newTime, delay);
        timerworker.ReportProgress(1, ta);
    }


    public int ID
    {
        get { return (int)GetValue(idProperty); }
        set { SetValue(idProperty, value); }
    }


    public static readonly DependencyProperty idProperty =
        DependencyProperty.Register("ID", typeof(int), typeof(SimulationWindow), new PropertyMetadata(null));
    

    public int progresPer
    {
        get { return (int)GetValue(progresPerProperty); }
        set { SetValue(progresPerProperty, value); }
    }

    public static readonly DependencyProperty progresPerProperty =
        DependencyProperty.Register("progresPer", typeof(int), typeof(SimulationWindow), new PropertyMetadata(null));

    public BO.Enum.OrderStatus? OldStatus
    {
        get { return (BO.Enum.OrderStatus?)GetValue(OldStatusProperty); }
        set { SetValue(OldStatusProperty, value); }
    }

    public static readonly DependencyProperty OldStatusProperty =
        DependencyProperty.Register("OldStatus", typeof(BO.Enum.OrderStatus?), typeof(SimulationWindow), new PropertyMetadata(null));

    public BO.Enum.OrderStatus? NewStatus
    {
        get { return (BO.Enum.OrderStatus?)GetValue(NewStatusProperty); }
        set { SetValue(NewStatusProperty, value); }
    }

    public static readonly DependencyProperty NewStatusProperty =
        DependencyProperty.Register("NewStatus", typeof(BO.Enum.OrderStatus?), typeof(SimulationWindow), new PropertyMetadata(null));

    public DateTime? ExpectedDate
    {
        get { return (DateTime?)GetValue(ExpectedDateProperty); }
        set { SetValue(ExpectedDateProperty, value); }
    }

    public static readonly DependencyProperty ExpectedDateProperty =
        DependencyProperty.Register("ExpectedDate", typeof(DateTime?), typeof(SimulationWindow), new PropertyMetadata(null));

    public DateTime? StartTime
    {
        get { return (DateTime?)GetValue(StartTimeProperty); }
        set { SetValue(StartTimeProperty, value); }
    }

    public static readonly DependencyProperty StartTimeProperty =
        DependencyProperty.Register("StartTime", typeof(DateTime?), typeof(SimulationWindow), new PropertyMetadata(null));
}