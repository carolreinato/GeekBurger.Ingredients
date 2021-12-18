using GeekBurger.Ingredients.Interface;
using Microsoft.Azure.Management.ServiceBus.Fluent;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GeekBurger.Ingredients.BusService
{
    public interface ILabelImageConsumer
    {
        Task ReceiveMessages();
    }

    public class LabelImageConsumer : ILabelImageConsumer
    {
        private readonly IConfiguration _configuration;
        private readonly IIngredientsService _ingredientsService;
        private const string TopicName = "labelimageadded";
        private const string SubscriptionName = "GRUPO666";

        public LabelImageConsumer(IIngredientsService ingredientsService,
            IConfiguration configuration)
        {
            _configuration = configuration;
            _ingredientsService = ingredientsService;
        }

        public async Task ReceiveMessages()
        {
            try
            {
                var connection = _configuration["serviceBus:connectionString"];
                var subscriptionClient = new SubscriptionClient(connection, TopicName, SubscriptionName);

                var mo = new MessageHandlerOptions(ExceptionHandler) { AutoComplete = true };

                subscriptionClient.RegisterMessageHandler(MessageHandler, mo);
            }
            catch (Exception x)
            {
                Console.WriteLine(x);
            }
        }

        private Task MessageHandler(Message message, CancellationToken arg2)
        {
            Console.WriteLine($"message Label: {message.Label}");
            Console.WriteLine($"CorrelationId: {message.CorrelationId}");
            var labelImageAddedString = Encoding.UTF8.GetString(message.Body);

            Console.WriteLine("Message Received");
            Console.WriteLine(labelImageAddedString);

            _ = _ingredientsService.MergeProductAndIngredients(message);

            // Message -> Item + Ingredients
            // GET PRODUCT PRODUCT + ITEM
            // Product<ItemIngredient>
            // 
            // Save<Sqlite> <-
            // Update<Sqlite> <- 
            // 

            //Thread.Sleep(40000);
            ///

            return Task.CompletedTask;
        }

        private Task ExceptionHandler(ExceptionReceivedEventArgs arg)
        {
            Console.WriteLine($"Handler exception {arg.Exception}.");
            var context = arg.ExceptionReceivedContext;
            Console.WriteLine($"Endpoint: {context.Endpoint}, Path: { context.EntityPath}, Action: { context.Action}");
            return Task.CompletedTask;
        }

    }
}
