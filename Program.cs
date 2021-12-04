using System;
using NLog.Web;
using System.IO;
using System.Linq;
using NorthWindConsole.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace NorthWindConsole
{
    class Program
    {
        // create static instance of Logger
        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            logger.Info("Program started");
            Console.ForegroundColor = ConsoleColor.White;

            try
                {
                    string choice;
                    do
                        {
                            Console.WriteLine("1) Display Categories");
                            Console.WriteLine("2) Add Category");
                            Console.WriteLine("3) Display Category and related products");
                            Console.WriteLine("4) Display all Categories and their related products");
                            Console.WriteLine("5) Add Product");
                            Console.WriteLine("6) Edit Product");
                            Console.WriteLine("7) Display all Products");
                            Console.WriteLine("8) Display specific Product");
                            Console.WriteLine("\"q\" to quit");
                            choice = Console.ReadLine();
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            logger.Info($"Option {choice} selected");
                            Console.ForegroundColor = ConsoleColor.White;
                            if (choice == "1")
                                {
                                var db = new Northwind_88_MJKContext();
                                var query = db.Categories.OrderBy(p => p.CategoryName);

                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"{query.Count()} records returned");
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                foreach (var item in query)
                                    {
                                        Console.WriteLine($"{item.CategoryName} - {item.Description}");
                                    }
                                Console.ForegroundColor = ConsoleColor.White;
                                }
                            else if (choice == "2")
                                {
                                Category category = new Category();
                                Console.WriteLine("Enter Category Name:");
                                category.CategoryName = Console.ReadLine();
                                Console.WriteLine("Enter the Category Description:");
                                category.Description = Console.ReadLine();
                                
                                ValidationContext context = new ValidationContext(category, null, null);
                                List<ValidationResult> results = new List<ValidationResult>();

                                var isValid = Validator.TryValidateObject(category, context, results, true);
                                if (isValid)
                                    {
                                    var db = new Northwind_88_MJKContext();
                                    // check for unique name
                                    if (db.Categories.Any(c => c.CategoryName == category.CategoryName))
                                        {
                                        // generate validation error
                                        isValid = false;
                                        results.Add(new ValidationResult("Name exists", new string[] { "CategoryName" }));
                                        }
                                    else
                                        {
                                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                                        logger.Info("Validation passed");
                                        Console.ForegroundColor = ConsoleColor.White;
                                        // TODO: save category to db
                                        }
                                    }
                                if (!isValid)
                                    {
                                    foreach (var result in results)
                                        {
                                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                                            logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                                            Console.ForegroundColor = ConsoleColor.White;
                                        }
                                    }
                                }
                                else if (choice == "3")
                                {
                                    var db = new Northwind_88_MJKContext();
                                    var query = db.Categories.OrderBy(p => p.CategoryId);

                                    Console.WriteLine("Select the category whose products you want to display:");
                                    Console.ForegroundColor = ConsoleColor.DarkRed;
                                    foreach (var item in query)
                                    {
                                        Console.WriteLine($"{item.CategoryId}) {item.CategoryName}");
                                    }
                                    Console.ForegroundColor = ConsoleColor.White;
                                    int id = int.Parse(Console.ReadLine());
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                                    logger.Info($"CategoryId {id} selected");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Category category = db.Categories.Include("Products").FirstOrDefault(c => c.CategoryId == id);
                                    Console.WriteLine($"{category.CategoryName} - {category.Description}");
                                    foreach (Product p in category.Products)
                                    {
                                        Console.WriteLine(p.ProductName);
                                    }
                                }
                                else if (choice == "4")
                                {
                                    var db = new Northwind_88_MJKContext();
                                    var query = db.Categories.Include("Products").OrderBy(p => p.CategoryId);
                                    foreach (var item in query)
                                    {
                                        Console.WriteLine($"{item.CategoryName}");
                                        foreach (Product p in item.Products)
                                        {
                                            Console.WriteLine($"\t{p.ProductName}");
                                        }
                                    }
                                }
                                else if(choice=="5"){
                                Product product = new Product();
                                Console.WriteLine("Enter Product Name:");
                                product.ProductName = Console.ReadLine();
                                Console.WriteLine("Enter the Supplier ID:");
                                product.SupplierId = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine("Enter the Category ID:");
                                product.CategoryId = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine("Enter Quantity Per Unit:");
                                product.QuantityPerUnit = Console.ReadLine();
                                Console.WriteLine("Enter the Unit Price:");
                                product.UnitPrice = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine("Enter Units in Stock:");
                                product.UnitsInStock = Convert.ToInt16(Console.ReadLine());
                                Console.WriteLine("Enter the Units on Order:");
                                product.UnitsOnOrder = Convert.ToInt16(Console.ReadLine());
                                Console.WriteLine("Enter the Reorder Level:");
                                product.ReorderLevel = Convert.ToInt16(Console.ReadLine());
                                Console.WriteLine("Enter Discontinued:");
                                product.Discontinued = Convert.ToBoolean(Console.ReadLine());
                                
                                ValidationContext context = new ValidationContext(product, null, null);
                                List<ValidationResult> results = new List<ValidationResult>();

                                var isValid = Validator.TryValidateObject(product, context, results, true);
                                if (isValid)
                                    {
                                    var db = new Northwind_88_MJKContext();
                                    // check for unique name
                                    if (db.Products.Any(p => p.ProductName == product.ProductName))
                                        {
                                        // generate validation error
                                        isValid = false;
                                        results.Add(new ValidationResult("Name exists", new string[] { "ProductName" }));
                                        }
                                    else
                                        {
                                        logger.Info("Validation passed");
                                        // TODO: save category to db
                                        }
                                    }
                                if (!isValid)
                                    {
                                    foreach (var result in results)
                                        {
                                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                                            logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                                            Console.ForegroundColor = ConsoleColor.White;
                                        }
                                    }
                                }
                                else if(choice=="6"){

                                }
                                else if(choice=="7"){
                                
                                string choice2;
                                // Console.WriteLine("1) Display all Products");
                                // Console.WriteLine("2) Display discontinued Products");
                                // Console.WriteLine("3) Display active Products");
                                // choice2 = Console.ReadLine();
                                // Console.Clear();
                                // Console.ForegroundColor = ConsoleColor.DarkYellow;
                                // logger.Info($"Option {choice2} selected");
                                // Console.ForegroundColor = ConsoleColor.White;

                                
                                var db = new Northwind_88_MJKContext();
                                var query = db.Products.OrderBy(p => p.ProductName);

                                // Console.ForegroundColor = ConsoleColor.DarkGreen;
                                // Console.WriteLine($"{query.Count()} records returned");
                                // Console.ForegroundColor = ConsoleColor.White;

                                //string choice2;
                                Console.WriteLine("1) Display all Products");
                                Console.WriteLine("2) Display discontinued Products");
                                Console.WriteLine("3) Display active Products");
                                choice2 = Console.ReadLine();
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                logger.Info($"Option {choice2} selected");
                                Console.ForegroundColor = ConsoleColor.White;
                                if(choice2=="1"){
                                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                                    Console.WriteLine($"{query.Count()} records returned");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    foreach (var item in query)
                                    {
                                        if(item.Discontinued==true){
                                            Console.ForegroundColor = ConsoleColor.Red;
                                        }
                                        else{
                                            Console.ForegroundColor = ConsoleColor.Green;
                                        }
                                        Console.WriteLine($"{item.ProductName}");
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                Console.ForegroundColor = ConsoleColor.White;
                                }
                                else if(choice2=="2"){
                                    int amount1=0;
                                    foreach(var item in query){
                                        if(item.Discontinued==true){
                                            amount1++;
                                        }
                                    }
                                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                                    Console.WriteLine($"{amount1} records returned");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    
                                foreach (var item in query)
                                    {
                                        if(item.Discontinued==true){
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine($"{item.ProductName}");
                                            Console.ForegroundColor = ConsoleColor.White;
                                        }

                                    }
                                }
                                else if(choice2=="3"){
                                    int amount2=0;
                                    foreach(var item in query){
                                        if(item.Discontinued==false){
                                            amount2++;
                                        }
                                    }
                                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                                    Console.WriteLine($"{amount2} records returned");
                                    Console.ForegroundColor = ConsoleColor.White;

                                     foreach (var item in query)
                                    {
                                        if(item.Discontinued==false){
                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.WriteLine($"{item.ProductName}");
                                            Console.ForegroundColor = ConsoleColor.White;
                                        }

                                    }   
                                }
                                Console.ForegroundColor = ConsoleColor.White;
                                }

                                else if(choice=="8"){
                                string choice3;

                                var db = new Northwind_88_MJKContext();
                                var query = db.Products.OrderBy(p => p.ProductName);

                                Console.WriteLine("Enter Product Name");
                                choice3=Console.ReadLine();
                                foreach(var item in query){
                                    if(item.ProductName==choice3){
                                        if(item.Discontinued==true){Console.ForegroundColor = ConsoleColor.Red;}
                                        else{Console.ForegroundColor = ConsoleColor.Green;}
                                    Console.WriteLine($"Product Name: {item.ProductName} | ProductID: {item.ProductId} | SupplierID: {item.SupplierId} | CategoryID: {item.CategoryId} | QuantityPerUnit: {item.QuantityPerUnit} | UnitPrice: {item.UnitPrice} | UnitsInStock: {item.UnitsInStock} | UnitsOnOrder: {item.UnitsOnOrder} | ReorderLevel: {item.ReorderLevel} | Discontinued: {item.Discontinued}");
                                    Console.ForegroundColor = ConsoleColor.White;}
                                }

                                }
                            Console.WriteLine();

                        } while (choice.ToLower() != "q");
                }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                logger.Error(ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            logger.Info("Program ended");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}