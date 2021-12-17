using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GeekBurger.Ingredients.BusService
{
    public class ProductChanged
    {
        private const string TopicName = "productchanged";
        private static IConfiguration _configuration;
        private const string SubscriptionName = "GRUPO666";

        public static async Task ReceiveMessages(IConfiguration _configuration)
        {
            try
            {
                var connection = _configuration["serviceBus:connectionString"];
                var subscriptionClient = new SubscriptionClient(connection, TopicName, SubscriptionName);

                //await subscriptionClient.RemoveRuleAsync("$Default");

                //await subscriptionClient.AddRuleAsync(new RuleDescription
                //{
                //    Filter = new CorrelationFilter { Label = _storeId },
                //    Name = "filter-store"
                //});

                var mo = new MessageHandlerOptions(ExceptionHandler) { AutoComplete = true };

                subscriptionClient.RegisterMessageHandler(MessageHandler, mo);
            }
            catch (Exception x)
            {
                Console.WriteLine(x);
            }
        }

        private static Task MessageHandler(Message message, CancellationToken arg2)
        {
            Console.WriteLine($"message Label: {message.Label}");
            Console.WriteLine($"CorrelationId: {message.CorrelationId}");
            var productChanged = Encoding.UTF8.GetString(message.Body);

            Console.WriteLine("Message Received");
            Console.WriteLine(productChanged);

            //Thread.Sleep(40000);
            ///

            return Task.CompletedTask;
        }

        private static Task ExceptionHandler(ExceptionReceivedEventArgs arg)
        {
            Console.WriteLine($"Handler exception {arg.Exception}.");
            var context = arg.ExceptionReceivedContext;
            Console.WriteLine($"Endpoint: {context.Endpoint}, Path: { context.EntityPath}, Action: { context.Action}");
            return Task.CompletedTask;
        }
    }
}
