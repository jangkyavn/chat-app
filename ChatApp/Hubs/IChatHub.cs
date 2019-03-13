using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatApp.Hubs
{
    public interface IChatHub
    {
        Task ReceiveMessage(string user, string message);
        Task Joined(List<string> userNames);
        Task Leaved(string user, string message);
        Task Typing(string user, string message);
    }
}
