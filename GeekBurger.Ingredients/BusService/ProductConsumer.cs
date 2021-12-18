using GeekBurger.Ingredients.Interface;
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
    public interface IProductConsumer
    {
        Task ReceiveMessages();
    }

    public class ProductConsumer : IProductConsumer
    {
        private readonly IConfiguration _configuration;
        private readonly IIngredientsService _ingredientsService;
        private const string TopicName = "productchanged";
        private const string SubscriptionName = "GRUPO666";

        public ProductConsumer(IIngredientsService ingredientsService,
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

        private static Task MessageHandler(Message message, CancellationToken arg2)
        {
            Console.WriteLine($"message Label: {message.Label}");
            Console.WriteLine($"CorrelationId: {message.CorrelationId}");
            var productChanged = Encoding.UTF8.GetString(message.Body);

            Console.WriteLine("Message Received");
            Console.WriteLine(productChanged);

            //TODO: Start another service
            //Modificar status do item
            //Deletar caso item seja deletado.
            //Delete(Sqlite)
            //Modify(Update(Sqlite))
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
