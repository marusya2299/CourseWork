using System;
using System.Collections.Generic;
using ClassLibrary;

namespace Store
{
    class Interface
    {
        static void Main(string[] args)
        {
            int n = 4; //number of products
            var Products = new List<Product>(n) //list of products
            {
                new Table(21, 99.9, "Table Judit", "RIVAL"),
                new Bed(31, 599.9, "Bed Eden", "Hettich"),
                new Chair(34, 59.9, "Chair ISO", "VISTA"),
                new Bed(10, 999.9, "Bed POP", "Hettich")
            };

            //add products to the warehouse
            Storage Operation = new Storage(Products, n);
            Operation.Maximum = 50;

            //show menu
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("MENU\n");
            for(int i = 0; i < n; i++)
            {
                Console.WriteLine($"{i + 1} - Name: {Products[i].Name}\nPrice: {Products[i].Price}\nManufacturer: {Products[i].Manufacturer}\n");
            }
            Console.ResetColor();

            bool Flag = true, Value; 
            int Number = 0;
            int Stage = 4;
            string Input;
            int Counter = 0;
            int p = 0;
            List<int> Check = new List<int>() { };

            Order Order = new Order();


            while (Flag)
            {
                //entering product number
                if (Stage == 0)
                {
                    Console.Write($"Enter corresponding product number: ");

                    Input = Console.ReadLine();

                    if (Int32.TryParse(Input, out Number) && Number - 1 < n && Number - 1 >= 0 && !Check.Contains(Number - 1))
                    {
                        Check.Add(Number - 1);
                        Order.AddProduct(Products[Number - 1]);
                        p++;
                        Stage = 1;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Invalid input. Try again...");
                        Console.ResetColor();
                    }
                }

                //entering the required amount of product
                if (Stage == 1)
                {
                    Console.Write("Enter quantity of the product: ");

                    Input = Console.ReadLine();

                    if (Int32.TryParse(Input, out Number) && Number > 0)
                    {
                         Order.AddQuantity(Number);
                         Stage = 2;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Invalid input. Try again...");
                        Console.ResetColor();
                    }
                }

                //continue or complete the order
                if (Stage == 2)
                {
                    Console.WriteLine("Continue your order? (Yes - 1, No - 0)");
                    Console.Write("--->");

                    Input = Console.ReadLine();

                    if (Int32.TryParse(Input, out Number) && (Number == 1 || Number == 0))
                    {
                        if (Number == 0)
                        {
                            Check.Clear();
                            Stage = 3;
                        }
                        else if (Number == 1 && p < n)
                        {
                            Stage = 0;
                        }
                        else if(Number == 1 && p == n)
                        {
                            Stage = 3;
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Invalid input. Try again...");
                        Console.ResetColor();
                    }
                }

                if (Stage == 3)
                {
                    Value = Operation.CheckForAvailability(Order.OrderProducts[Order.NumberOfOrders], Order.OrderQuantity[Order.NumberOfOrders], Order.QuantityInOrder[Order.NumberOfOrders]);

                    if (Value)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine($"||||||||||||||||||\nOrder is executed.\n||||||||||||||||||");
                        Console.ResetColor();

                        Order.Delete(Order.NumberOfOrders);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||\nThere isn't enough product in storage, your order was put in queue.\n|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
                        Console.ResetColor();
                    }

                    Stage = 4;
                }

                if (Stage == 4)
                {

                    Console.WriteLine("Finish work - 0\nCreate new order - 1");
                    Console.Write("--->");
                    Input = Console.ReadLine();

                    if (Int32.TryParse(Input, out Number) && (Number == 1 || Number == 0))
                    {
                        if (Number == 0 && Counter != 0)
                        {
                            Operation.DeliveryToTheWarehouse();
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("||||||||||||||||||||||||||||||\nWarehouse has been replenished\n||||||||||||||||||||||||||||||");

                            Value = Operation.CheckForAvailability(Order.OrderProducts, Order.OrderQuantity, Order.QuantityInOrder, Order.NumberOfOrders + 1);

                            if(Value)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                for (int i = 0; i < Order.NumberOfOrders + 1; i++)
                                {
                                    for (int j = 0; j < Order.QuantityInOrder[i]; j++)
                                    {
                                        Console.WriteLine($"Sold {Order.OrderProducts[i][j]} in quantity {Order.OrderQuantity[i][j]}\n");
                                    }
                                }
                                Console.ResetColor();
                            }

                            Console.WriteLine("|||||||||||||||\nWork completed.\n|||||||||||||||");
                            Flag = false;
                        }
                        else if (Number == 0 && Counter == 0)
                        {
                            Console.WriteLine("|||||||||||||||\nWork completed.\n|||||||||||||||");
                            Flag = false;
                        }
                        else if (Number == 1 && Counter != 0)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("|||||||||\nNew order\n|||||||||");
                            Console.ResetColor();
                            Counter++;
                            Order.New();
                            Stage = 0;
                        }
                        else if(Number == 1 && Counter == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("|||||||||\nNew order\n|||||||||");
                            Console.ResetColor();
                            Counter++;
                            Stage = 0;
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Invalid input. Try again...");
                        Console.ResetColor();
                    }
                }
            }
        }
    }
}
