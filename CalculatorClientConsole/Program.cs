using CalulatorServer;
using Grpc.Net.Client;
using System;
using System.Text.RegularExpressions;

namespace CalculatorClientConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {               
                string inputExpression = Console.ReadLine();

                Console.WriteLine(ClientProcessing(inputExpression));
            }
        }

        public static string ClientProcessing(string inputExpression)
        {
            switch (true)
            {
                case bool _ when Regex.IsMatch(inputExpression, @"[a-zA-Z]"):
                    return "Expression contain letters, please check your input";
                    
                case bool _ when Regex.IsMatch(inputExpression, @"[а-яА-ЯёЁ]"):
                    return "Выражение содержит буквы, пожалуйста проверьте ввод";

                case bool _ when Regex.IsMatch(inputExpression, @"[!@#$%^&№:?_='|~`><]"):
                    return "Expression contain special characters, please check your input";

                case bool _ when Regex.IsMatch(inputExpression, @"[0-9]"):
                    Console.WriteLine("Calculating ... ");
                    return ($"Answer:  {ServerInteractions(inputExpression)}");

                default:
                    return "The expression contains special characters or cannot be validated, please check your input";
            }
        }

        public static string ServerInteractions(string inputExpression)
        {
            var inputForServer = new Expression { ReceivedExpression = inputExpression };
            var channel = GrpcChannel.ForAddress("http://localhost:5000");
            var client = new Calculator.CalculatorClient(channel);
            var calculationResalt = client.GetExpressionResult(inputForServer);

            return calculationResalt.Result;            
        }
    }
}
