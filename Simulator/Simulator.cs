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

    private static volatile bool activeSimulator = false;

    public delegate void SimulationCompleteEventHandler();
    public static event SimulationCompleteEventHandler SimulationFinished;

    public delegate void UpdateEventHandler(BO.Order? order, DateTime newTime, int delay);
    public static event UpdateEventHandler Updated;

    public static Random random = new Random();
    public static BO.Order? order = new BO.Order();

    /// <summary>
    /// Thread to activate the simulator
    /// </summary>
    public static void Activate()
    {
        new Thread(() =>
        {
            activeSimulator = true;
            while (activeSimulator)
            {
                RunSimulation();
                Thread.Sleep(1000);
            }
            SimulationFinished();  //to report that simulation ended
        }).Start();
    }

    private static void RunSimulation()
    {
        int? oldestOrderID = bl.Order.GetOldestOrderID();
        BO.Order order = bl.Order.GetOrderDetails((int)oldestOrderID);

        //update the DELIVERY to be SHIPPED
        if (order != null)
        {
            //how long it takes to collect/supply the order
            int delay = random.Next(3, 11);
            DateTime time = DateTime.Now + new TimeSpan(delay * 1000);

            //report is an event
            Updated(order, time, delay);
            Thread.Sleep(delay * 1000);

            if (order.Status.ToString() == "Confirmed")
                bl.Order.UpdateOrderDelivery((int)oldestOrderID, time);
            else if (order.Status.ToString() == "Shipped")
                bl.Order.UpdateOrderSupply((int)oldestOrderID, time);

            //Report(finished);//to report that handeling the order has ended
        }
    }

    /// <summary>
    /// To stop the simulation
    /// </summary>
    public static void StopSimulation()
    {
        activeSimulator = false;
    }

    /// <summary>
    /// Register for simulation complete event
    /// </summary>
    /// <param name="e_handler"></param>
    public static void RegisterForSimulationCompleteEvent(SimulationCompleteEventHandler e_handler)
    {
        SimulationFinished += e_handler;
    }

    /// <summary>
    /// Unregister from simulation complete event
    /// </summary>
    /// <param name="e_handler"></param>
    public static void UnregisterFromSimulationCompleteEvent(SimulationCompleteEventHandler e_handler)
    {
        SimulationFinished -= e_handler;
    }

    /// <summary>
    /// Register for update event
    /// </summary>
    /// <param name="e_handler"></param>
    public static void RegisterForUpdateEvent(UpdateEventHandler e_handler)
    {
        Updated += e_handler;
    }

    /// <summary>
    /// Unregister from update event
    /// </summary>
    /// <param name="e_handler"></param>
    public static void UnregisterFromUpdateEvent(UpdateEventHandler e_handler)
    {
        Updated -= e_handler;
    }
}