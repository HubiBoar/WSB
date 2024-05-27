using System.Text;
using Shared;

namespace Server.Public;

public static partial class Logic
{
    public static class Client
    {
        public interface ITokenProvider
        {
            public Task<Sockets.Token> Handle(Sockets.Handler socket);
        }

        public interface IHandle
        {
            string ConsoleCommand { get; }

            public Task<StringBuilder> Handle(Sockets.Handler socket, Sockets.Token token);
        }

        public interface IAdminHandle : IHandle
        {
        }

        public static async Task Run(ITokenProvider tokenProvider, params IHandle[] handles)
        {
            Print.Line("Client :: Hello, World!");

            using var client = await Sockets.CreateClient();

            var token = await tokenProvider.Handle(client);

            Print.Separator();
            Print.Line("[Select Command]");
            Print.Separator();

            foreach(var handle in handles)
            {
                if(handle is IAdminHandle && token.IsAdmin == false)
                {
                    continue;
                }

                Print.Line($"--> {handle.ConsoleCommand}");
            }

            while(true)
            {
                Print.Separator();
                var command = Print.ReadValue<string>();

                bool found = false;
                foreach(var handle in handles)
                {
                    if((handle.ConsoleCommand != command) || (handle is IAdminHandle && token.IsAdmin == false))
                    {
                        continue;
                    }

                    found = true;
                    Print.Output(await handle.Handle(client, token));
                    break;
                }

                if(found == false)
                {
                    Print.Line("[Command not found]");
                }
            }
        }
    }
}