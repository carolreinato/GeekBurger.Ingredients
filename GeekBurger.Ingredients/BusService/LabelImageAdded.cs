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
    public static class LabelImageAdded 
    {
        private const string TopicName = "labelimageadded";
        private static IConfiguration _configuration;
        private const string SubscriptionName = "GRUPO666";
        private static readonly IServiceBusNamespace _serviceBusNamespace;
        private static readonly ServiceBusConfiguration _serviceBusConfiguration;    

        //public void EnsureTopicIsCreated()
        //{
        //    if (!_serviceBusNamespace.Topics.List().Any(t => t.Name.Equals(TopicName, StringComparison.InvariantCultureIgnoreCase)))
        //    {
        //        _serviceBusNamespace.Topics
        //            .Define(TopicName)
        //            .WithSizeInMB(1024)
        //            .Create();
        //    }
        //}

        //public void EnsureSubscriptionExist()
        //{
        //    var topic = _configuration.GetServiceBusNamespace().Topics.GetByName(TopicName);

        //    if (topic.Subscriptions.List()
        //      .Any(subscription => subscription.Name
        //      .Equals(SubscriptionName,
        //             StringComparison.InvariantCultureIgnoreCase)))
        //    {
        //        topic.Subscriptions.DeleteByName(SubscriptionName);
        //    }

        //    topic.Subscriptions
        //        .Define(SubscriptionName)
        //        .Create();
        //}

        public static async Task ReceiveMessages(IConfiguration _configuration)
        {
            try {
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
            catch(Exception x)
            {
                Console.WriteLine(x);
            }
        }

        private static Task MessageHandler(Message message, CancellationToken arg2)
        {
            Console.WriteLine($"message Label: {message.Label}");
            Console.WriteLine($"CorrelationId: {message.CorrelationId}");
            var labelImageAddedString = Encoding.UTF8.GetString(message.Body);

            Console.WriteLine("Message Received");
            Console.WriteLine(labelImageAddedString);

            //Thread.Sleep(40000);

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
