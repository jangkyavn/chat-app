using ChatApp.Common;
using ChatApp.Data;
using ChatApp.Data.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Security;

namespace ChatApp.Hubs
{
    public class ChatHub : Hub<IChatHub>
    {
        public void SendMessage(string user, string message)
        {
            using (var db = new DataContext())
            {
                var mes = new Message()
                {
                    Content = message,
                    DateCreated = DateTime.Now,
                    UserName = user
                };
                db.Messages.Add(mes);
                db.SaveChanges();

                Clients.All.ReceiveMessage(user, message);
            }
        }

        public void Typing(string user, string message)
        {
            Clients.OthersInGroup(CommonConstants.GROUP_NAME).Typing(user, message);
        }

        public override Task OnConnected()
        {
            //var ss = Context.User.Identity.Name;

            using (var db = new DataContext())
            {
                // Retrieve user.
                var user = db.Users.FirstOrDefault(u => u.UserName == Context.User.Identity.Name);

                // If user does not exist in database, must add.
                if (user == null)
                {
                    user = new User()
                    {
                        UserName = Context.User.Identity.Name,
                        Connected = true
                    };
                    db.Users.Add(user);
                    db.SaveChanges();
                }
                else
                {
                    user.Connected = true;
                    db.SaveChanges();
                }

                Groups.Add(Context.ConnectionId, CommonConstants.GROUP_NAME);
                var userNames = db.Users.Where(x => x.Connected).Select(x => x.UserName).ToList();
                Clients.Group(CommonConstants.GROUP_NAME).Joined(userNames);
            }

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            using (var db = new DataContext())
            {
                // Retrieve user.
                var user = db.Users.FirstOrDefault(u => u.UserName == Context.User.Identity.Name);
                user.Connected = false;
                db.SaveChanges();

                Groups.Remove(Context.ConnectionId, CommonConstants.GROUP_NAME);
                Clients.Group(CommonConstants.GROUP_NAME).Leaved(Context.User.Identity.Name, " đã rời khỏi.");
                FormsAuthentication.SignOut();
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}