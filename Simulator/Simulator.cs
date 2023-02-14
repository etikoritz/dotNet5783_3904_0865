using BlApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Simulator;

public static class Simulator
{
    static readonly IBl bl = BlApi.Factory.Get();
    private static volatile bool _deactivateRequested = false;




    private static volatile bool Running;

    public delegate void SimulationCompleteEventHandler();
    public static event SimulationCompleteEventHandler Simulation_Completed;

    public delegate void UpdateEventHandler(BO.Order? order, DateTime newTime, int delay);
    public static event UpdateEventHandler Updated;
    public static Random random = new Random();
    public static BO.Order? order = new BO.Order();

    /// <summary>
    /// To activate the simulator
    /// </summary>
    public static void Activate()
    {
        new Thread(() =>
        {
            bool activeSimulator = true;
            Random random = new Random();
            while (activeSimulator)
            {
                int? oldestOrderID = bl.Order.GetOldestOrderID();
                BO.Order order = bl.Order.GetOrderDetails((int)oldestOrderID);

                //update the DELIVERY to be SHIPPED
                if (oldestOrderID != null)
                {
                    //how long it takes to collect/supply the order
                    int delay = random.Next(3, 11);
                    DateTime time = DateTime.Now + new TimeSpan(delay * 1000);

                    //report is an event
                    Report(oldestOrderID, time);
                    Thread.Sleep(delay * 1000);

                    if (order.Status.ToString() == "Confirmed")
                        bl.Order.UpdateOrderDelivery((int)oldestOrderID, time);
                    else if (order.Status.ToString() == "Shipped")
                        bl.Order.UpdateOrderSupply((int)oldestOrderID, time);

                    Report(finished);//to report that handeling the order has ended
                    
                }

                Thread.Sleep(1000);
            }

            Report(simulationFinished);//to report that simulation ended

        }).Start();
    }


    public static void StopSimulation()
    {
        Running = false;

    }

    public static void RegisterForSimulationCompleteEvent(SimulationCompleteEventHandler e_handler)
    {
        Simulation_Completed += e_handler;
    }

    public static void UnregisterFromSimulationCompleteEvent(SimulationCompleteEventHandler e_handler)
    {
        Simulation_Completed -= e_handler;
    }

    public static void RegisterForUpdateEvent(UpdateEventHandler e_handler)
    {
        Updated += e_handler;
    }

    public static void UnregisterFromUpdateEvent(UpdateEventHandler e_handler)
    {
        Updated -= e_handler;
    }




}
