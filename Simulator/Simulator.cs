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
}
