using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace UnitX
{
    public class Queuer
    {
        private ConnectionFactory qFactory;
        public IConnection Connection;
        private static IConfigurationRoot Configuration = new ConfigurationBuilder()
                   .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                   .AddJsonFile("appsettings.json").Build();

        public Dictionary<string, IModel> Queues = new Dictionary<string, IModel>();
        public string HostName
        {
            get
            {
                return Configuration.GetValue<string>("MySettings:HostName");
            }
        }

        public Queuer()
            {
                var qFactory = new ConnectionFactory() { HostName = this.HostName };
                Console.WriteLine("Host:" + qFactory.HostName);
                Connection = qFactory.CreateConnection();
            }


        public bool PushIt(string data, string toQueue, bool closeQueue = false)
        {
            try
            {
                if (!Queues.ContainsKey(toQueue))
                {
                    var newQ = Connection.CreateModel();
                    newQ.QueueDeclare(queue: toQueue,
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);
                    Queues.Add(toQueue, newQ); 
                        
                }
                
                var Queue = Queues[toQueue];
                {
                    
                    
                    var body = Encoding.UTF8.GetBytes(data);

                    Queue.BasicPublish(exchange: toQueue,
                                         routingKey: data,
                                         basicProperties: null,
                                         body: body);

                    Console.WriteLine(" [x] Sent {0}", data);
                }

                if (closeQueue)
                {
                    Queue.Close();
                    Queues.Remove(toQueue);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public bool ReadQueue(string QueueName, Func<BasicDeliverEventArgs,object> eventFunction )
        {
            try
            {
                if (!Queues.ContainsKey(QueueName))
                {
                    var newQ = Connection.CreateModel();
                    newQ.QueueDeclare(queue: QueueName,
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);
                    Queues.Add(QueueName, newQ);

                }

                var consumer = new EventingBasicConsumer(Queues[QueueName]);
                consumer.Received += (model, ea) =>
                {
                    eventFunction(ea);
                };
                Queues[QueueName].BasicConsume(queue: QueueName,
                                     autoAck: true,
                                     consumer: consumer);
                return true;
            }

            catch (Exception ex)
            {

                Console.WriteLine(ex);
                return false;
            }
           
         
        }
    }
}
