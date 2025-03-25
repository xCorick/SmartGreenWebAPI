using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGreenAPI.Data.hub
{
    public class InverStatusHub : Hub
    {
        public async Task SubscribeToInvernadero(string idInvernadero)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, idInvernadero);
        }
    }
}
