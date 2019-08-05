using System;
using System.Threading;
using System.Threading.Tasks;
using CoffeeOrder.Helpers;
using Microsoft.AspNetCore.SignalR;

namespace CoffeeOrder.Hubs
{
    public class CoffeeOrderHub : Hub
    {
        private readonly OrderChecker _orderChecker;

        public CoffeeOrderHub(OrderChecker orderChecker)
        {
            _orderChecker = orderChecker;
        }

        public async Task GetUpdateForOrder()
        {
            await Clients.Caller.SendAsync("ProceedOrder");
            CheckResult result;
            var rnd = new Random(DateTime.Now.Millisecond);
            do
            {
                result = _orderChecker.GetUpdate();
                Thread.Sleep(rnd.Next(1000, 3000));
                if (result.New)
                    await Clients.Caller.SendAsync("ReceiveOrderUpdate",
                        result.Update);
            } while (!result.Finished);

            await Clients.Caller.SendAsync("OrderComplete");
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("ConnectionStarter", Context.ConnectionId);
            await Clients.Others.SendAsync("NewClientConnected");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.Others.SendAsync("OtherClientDisconnected");
        }
    }
}
