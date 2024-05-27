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

        public static void Input()
        {
            Console.WriteLine();
            Console.WriteLine("<--[Input]-->");
            Console.WriteLine();
        }

        public static void Output(StringBuilder builder)
        {
            Console.WriteLine();
            Console.WriteLine("<--[Output]-->");
            Console.WriteLine();
            Console.WriteLine(builder);
        }

        public static void Output(string value)
        {
            Output(new StringBuilder(value));
        }

        public static void Value(string value)
        {
            Console.WriteLine(value);
        }

        public static T ReadValue<T>(string value)
        {
            Console.WriteLine(value);
            return ReadValue<T>();
        }

        public static T ReadValue<T>()
        {
            return (T)Convert.ChangeType(Console.ReadLine(), typeof(T))!;
        }

        public static async Task Run(ITokenProvider tokenProvider, params IHandle[] handles)
        {
            Console.WriteLine("Client :: Hello, World!");

            using var client = await Sockets.CreateClient();

            var token = await tokenProvider.Handle(client);

            Console.WriteLine("----------------------------------------------------------------------------");
            Console.WriteLine("Select commands:");

            foreach(var handle in handles)
            {
                Console.WriteLine(handle.ConsoleCommand);
            }

            while(true)
            {
                Console.WriteLine("----------------------------------------------------------------------------");

                Console.WriteLine();
                var command = Console.ReadLine()!;

                foreach(var handle in handles)
                {
                    if(handle.ConsoleCommand == command)
                    {
                        Output(await handle.Handle(client, token));
                        break;
                    }
                }
            }
        }
    }
}