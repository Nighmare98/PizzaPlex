using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PizzaPlex
{
    class Program
    {
        static void Main()
        {
            var bakery = new Bakery(10, 5);
            bakery.Start();

            Console.ReadLine();
        }
    }

    class Bakery
    {
        private readonly int _maxOrders;
        private readonly int _maxStorage;
        private readonly List<Order> _orders = new List<Order>();
        private readonly List<Pizza> _storage = new List<Pizza>();
        private readonly List<Pizza> _delivery = new List<Pizza>();
        private readonly List<Thread> _bakers = new List<Thread>();
        private readonly List<Thread> _couriers = new List<Thread>();

        public Bakery(int maxOrders, int maxStorage)
        {
            _maxOrders = maxOrders;
            _maxStorage = maxStorage;
        }

        public void Start()
        {
            for (int i = 0; i < 5; i++)
            {
                _bakers.Add(new Thread(BakePizza));
                _bakers[i].Start();
            }

            for (int i = 0; i < 3; i++)
            {
                _couriers.Add(new Thread(DeliverPizza));
                _couriers[i].Start();
            }
        }

        private void BakePizza()
        {
            while (true)
            {
                if (_orders.Count > 0)
                {
                    var order = _orders[0];
                    order.Bake();

                    if (order.IsReady)
                    {
                        if (_storage.Count < _maxStorage)
                        {
                            _storage.Add(order.Pizza);
                            Console.WriteLine($"Order {order.Id} is ready and stored.");
                        }
                        else
                        {
                            Console.WriteLine($"Order {order.Id} is ready, but storage is full.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Order {order.Id} is not ready yet.");
                    }
                }
                else
                {
                    Console.WriteLine("No orders to bake.");
                }
            }
        }

        private void DeliverPizza()
        {
            while (true)
            {
                if (_storage.Count > 0)
                {
                    var pizza = _storage[0];
                    _delivery.Add(pizza);
                    _storage.RemoveAt(0);
                    Console.WriteLine($"Order {pizza.OrderId} is delivered.");
                }
                else
                {
                    Console.WriteLine("No pizzas to deliver.");
                }
            }
        }
    }

    class Order
    {
        public int Id { get; }
        public Pizza Pizza { get; }
        public bool IsReady { get; private set; }

        public Order(int id)
        {
            Id = id;
            Pizza = new Pizza(id);
        }

        public void Bake()
        {
            Thread.Sleep(1000); // время приготовления пиццы
            IsReady = true;
        }
    }

    class Pizza
    {
        public int OrderId { get; }

        public Pizza(int orderId)
        {
            OrderId = orderId;
        }
    }
}