using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakeProject.Server.Model;

namespace TakeProject.Server.Helpers
{
    public static class CommandHelper
    {
        public static List<Command> ValidCommands = new List<Command>
        {
            new Command { CommandText= "p", Parameters = 3, Description = "/p <target> <message> - Send a private message to target."},
            new Command { CommandText= "exit", Parameters = 1, Description = "/exit - Exits the system"},
            new Command { CommandText= "changenickname", Parameters = 2, Description = "/changenickname <new_nickname> - Changes the user nickname"},
            new Command { CommandText= "help", Parameters = 1, Description = "/help - Show the help list"}
        };
    }
}
