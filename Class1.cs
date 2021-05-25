using System.Collections.Generic;

namespace ClassLibrary
{
    public class Storage
    {
        private int n; //number of products
        public List<Product> Products = new List<Product>() { };

        private int CounterMissing; //number of products that are not enough
        private List<Product> MissingProducts = new List<Product>() { };
        private List<int> MissingQuantity = new List<int>() { };

        public Storage(List<Product> Products, int n)
        {
            for (int i = 0; i < n; i++)
            {
                this.Products.Add(Products[i]);
            }
            this.n = n;
            CounterMissing = 0;
        }

        //check availability for one order
        public bool CheckForAvailability(List<Product> OrderProducts, List<int> Quantity, int NumberOfOrders)
        {
            bool Value = true;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < NumberOfOrders; j++)
                {
                    if (OrderProducts[j] == Products[i])
                    {
                        if (Products[i].Quantity < Quantity[j])
                        {
                            CounterMissing++;
                            Value = false;
                            AddToList(OrderProducts[j], Quantity[j]);
                        }
                    }
                }
            }

            if (Value)
            {
                DeliveryFromTheWarehouse(OrderProducts, Quantity, NumberOfOrders);
                return true;
            }
            return false;
        }

        //check availability for all orders
        public bool CheckForAvailability(List<List<Product>> OrderProducts, List<List<int>> Quantity, List<int> QuantityInOrder, int NumberOfOrders)
        {
            int Counter = 0;

            for (int i = 0; i < NumberOfOrders; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    for (int u = 0; u < QuantityInOrder[i]; u++)
                    {
                        if (Products[j] == OrderProducts[i][u])
                        {
                            if (Products[j].Quantity < Quantity[i][u])
                            {
                                Counter++;
                            }
                        }
                    }
                }
            }

            if (Counter == 0)
            {
                DeliveryFromTheWarehouse(OrderProducts, Quantity, QuantityInOrder, NumberOfOrders);
                return true;
            }

            return false;
        }

        private void AddToList(Product Product, int Quantity)
        {
            MissingProducts.Add(Product);
            MissingQuantity.Add(Quantity);
        }

        public void DeliveryToTheWarehouse()
        {
            for (int i = 0; i < n; i++)
            {
                Products[i].Quantity += 30;

                for (int j = 0; j < CounterMissing; j++)
                {
                    if (MissingProducts[j] == Products[i])
                    {
                        Products[i].Quantity = Products[i].Quantity + MissingQuantity[j];
                    }
                }
            }
        }

        //delivery from warehouse for one order
        private void DeliveryFromTheWarehouse(List<Product> OrderProducts, List<int> Quantity, int QuantityInOrder)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < QuantityInOrder; j++)
                {
                    if (OrderProducts[j] == Products[i])
                    {
                        Products[i].Quantity -= Quantity[j];
                    }
                }
            }
        }

        //delivery from the warehouse for all orders
        private void DeliveryFromTheWarehouse(List<List<Product>> OrderProducts, List<List<int>> Quantity, List<int> QuantityInOrder, int NumberOfOrders) //!!!
        {
            for (int j = 0; j < n; j++)
            {
                for (int i = 0; i < NumberOfOrders; i++)
                {
                    for (int u = 0; u < QuantityInOrder[i]; u++)
                    {
                        if (OrderProducts[i][u] == Products[j])
                        {
                            Products[j].Quantity -= Quantity[i][u];
                        }
                    }
                }
            }
        }
    }

    public class Order
    {
        public List<List<int>> OrderQuantity = new List<List<int>>();
        public List<List<Product>> OrderProducts = new List<List<Product>>();
        public List<int> QuantityInOrder = new List<int>() { };

        public int NumberOfOrders = 0;

        public Order()
        {
            OrderProducts.Add(new List<Product>());
            OrderQuantity.Add(new List<int>());
        }

        //add a new order
        public void New()
        {
            OrderProducts.Add(new List<Product>());
            OrderQuantity.Add(new List<int>());
            NumberOfOrders++;
        }

        //delete order
        public void Delete(int NumberOfOrders)
        {
            OrderProducts.Remove(OrderProducts[NumberOfOrders]);
            OrderQuantity.Remove(OrderQuantity[NumberOfOrders]);
            QuantityInOrder.Remove(QuantityInOrder[NumberOfOrders]);
            this.NumberOfOrders--;
        }

        public void AddProduct(Product Product)
        {
            OrderProducts[NumberOfOrders].Add(Product);
        }

        public void AddQuantity(int Number)
        {
            QuantityInOrder.Add(0);
            QuantityInOrder[NumberOfOrders]++;
            OrderQuantity[NumberOfOrders].Add(Number);
        }
    }

    public abstract class Product
    {
        public int Quantity { get; set; } 
        public double Price { get; set; }  
        public string Name { get; set; } 
        public string Manufacturer { get; set; } 
    }

    public class Table : Product
    {
        public Table(int Quantity, double Price, string Name, string Manufacturer)
        {
            this.Quantity = Quantity;
            this.Price = Price;
            this.Name = Name;
            this.Manufacturer = Manufacturer;
        }
    }

    public class Bed : Product
    {
        public Bed(int Quantity, double Price, string Name, string Manufacturer)
        {
            this.Quantity = Quantity;
            this.Price = Price;
            this.Name = Name;
            this.Manufacturer = Manufacturer;
        }
    }

    public class Chair : Product
    {
        public Chair(int Quantity, double Price, string Name, string Manufacturer)
        {
            this.Quantity = Quantity;
            this.Price = Price;
            this.Name = Name;
            this.Manufacturer = Manufacturer;
        }
    }
}
