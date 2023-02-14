using BlApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            bool active = true;
            Random random = new Random();
            while (active)
            {
                int oldestOrderID = bl.Order.GetOldestOrderID("orderDate");
                int oldestShippingOrderID = bl.Order.GetOldestOrderID("shipDate");

                //update the DELIVERY to be SHIPPED
                if (oldestOrderID != null)
                {
                    BO.Order order = bl.Order.GetOrderDetails(oldestOrderID);
                    //how long it takes to collect/supply the order
                    int delay = random.Next(3, 11);
                    DateTime time = DateTime.Now + new TimeSpan(delay * 1000);
                    //report is an event
                    Report(oldestOrderID, time);
                    Thread.Sleep(delay * 1000);
                    Report(finished);//to report that handeling the order has ended
                    bl.Order.UpdateOrderDelivery(oldestOrderID, time);
                }

                //update the SHIPPING to be SUPPLIED
                if (oldestShippingOrderID != null)
                {
                    BO.Order order = bl.Order.GetOrderDetails(oldestShippingOrderID);
                    //how long it takes to collect/supply the order
                    int delay = random.Next(3, 11);
                    DateTime time = DateTime.Now + new TimeSpan(delay * 1000);
                    //report is an event
                    Report(oldestShippingOrderID, time);
                    Thread.Sleep(delay * 1000);
                    Report(finished);//to report that handeling the order has ended
                    bl.Order.UpdateOrderSupply(oldestShippingOrderID, time);
                }

                Thread.Sleep(1000);
            }


            Report(simulationFinished);//to report that simulation ended
        }).Start();
    }
}
