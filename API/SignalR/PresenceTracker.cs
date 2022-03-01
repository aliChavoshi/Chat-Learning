using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.SignalR
{
    public class PresenceTracker
    {
        private readonly Dictionary<string, List<string>> onlineUsers = new Dictionary<string, List<string>>(); //username=>key,List connetionId=>value
        public Task UserConnected(string userName, string connectionId)
        {
            //async await
            lock (onlineUsers)
            {
                if (onlineUsers.ContainsKey(userName))
                    onlineUsers[userName].Add(connectionId);
                else
                    onlineUsers.Add(userName, new List<string> { connectionId });
            }
            return Task.CompletedTask;
        }
        public Task UserDisconnected(string userName, string connectionId)
        {
            lock (onlineUsers)
            {
                if (!onlineUsers.ContainsKey(userName)) return Task.CompletedTask;
                onlineUsers[userName].Remove(connectionId);
                if (onlineUsers[userName].Count == 0) //list connectionId
                    onlineUsers.Remove(userName);
            }
            return Task.CompletedTask;
        }

        public Task<string[]> GetOnlineUsers()
        {
            string[] userOnline;
            lock (onlineUsers)
                userOnline = onlineUsers.OrderBy(x => x.Key).Select(x => x.Key).ToArray(); //ley=>userName
            return Task.FromResult(userOnline);
        }
    }
}