using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitX;


namespace UnitX
{
    class ProgramB
    {
        private static Queuer MyQueue = new Queuer();
        public static List<Product> Products = new List<Product>();
        static void Main(string[] args)
        {
            Console.WriteLine("Started !");
            Products.Add(new Product(new Guid("cc69c7a0-72ca-4a48-9bbf-be4310ffe2a2")));          

            MyQueue.ReadQueue("Solds", SoldFunction);
            Console.ReadLine();
            Console.WriteLine("Bye !");
        }

       
        public static object SoldFunction(BasicDeliverEventArgs ea)
        {
            try
            {
                var body = ea.Body.ToArray();
                var id = Encoding.UTF8.GetString(body);
                var target = (from p in Products where p.Id.ToString("N") == id select p).FirstOrDefault();
                if (target == null) return false;

                target.Sold(DateTime.Now);
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
        
    }
}
