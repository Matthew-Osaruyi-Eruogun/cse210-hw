using System;
using System.Collections.Generic;
using System.Text;

// Address class encapsulates street, city, state/province, country
class Address
{
    private string StreetAddress;
    private string City;
    private string StateOrProvince;
    private string Country;

    public Address(string streetAddress, string city, string stateOrProvince, string country)
    {
        StreetAddress = streetAddress;
        City = city;
        StateOrProvince = stateOrProvince;
        Country = country;
    }

    // Returns true if the address is in the USA
    public bool IsInUSA()
    {
        return Country.ToUpper() == "USA";
    }

    // Returns a formatted string representing the full address
    public override string ToString()
    {
        return $"{StreetAddress}\n{City}, {StateOrProvince}\n{Country}";
    }
}

// Customer class encapsulates name and an Address object
class Customer
{
    private string Name;
    private Address Address;

    public Customer(string name, Address address)
    {
        Name = name;
        Address = address;
    }

    // Returns true if the customer's address is in the USA
    public bool LivesInUSA()
    {
        return Address.IsInUSA();
    }

    // Returns the customer's name
    public string GetName()
    {
        return Name;
    }

    // Returns the customer's full address string
    public string GetAddressString()
    {
        return Address.ToString();
    }
}

// Product class encapsulates name, id, price, quantity
class Product
{
    private string Name;
    private string ProductId;
    private double Price;
    private int Quantity;

    public Product(string name, string productId, double price, int quantity)
    {
        Name = name;
        ProductId = productId;
        Price = price;
        Quantity = quantity;
    }

    // Returns the total cost for this product (price * quantity)
    public double GetTotalCost()
    {
        return Price * Quantity;
    }

    // Returns the product name
    public string GetName()
    {
        return Name;
    }

    // Returns the product id
    public string GetProductId()
    {
        return ProductId;
    }
}

// Order class encapsulates a list of products and a customer
class Order
{
    private List<Product> Products;
    private Customer Customer;

    public Order(Customer customer)
    {
        Customer = customer;
        Products = new List<Product>();
    }

    // Add a product to this order
    public void AddProduct(Product product)
    {
        Products.Add(product);
    }

    // Calculates the total cost of the order (product totals + shipping)
    public double CalculateTotalCost()
    {
        double totalCost = 0;
        foreach (var product in Products)
        {
            totalCost += product.GetTotalCost();
        }
        // Add shipping cost
        totalCost += Customer.LivesInUSA() ? 5 : 35;
        return totalCost;
    }

    // Returns a string packing label listing name and product id of products
    public string GetPackingLabel()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var product in Products)
        {
            sb.AppendLine($"{product.GetName()} (ID: {product.GetProductId()})");
        }
        return sb.ToString();
    }

    // Returns a string shipping label listing customer's name and address
    public string GetShippingLabel()
    {
        return $"{Customer.GetName()}\n{Customer.GetAddressString()}";
    }
}

// Main program to demonstrate usage
class Program
{
    static void Main()
    {
        // Create addresses
        Address addr1 = new Address("123 Elm St", "Springfield", "IL", "USA");
        Address addr2 = new Address("456 Maple Ave", "Toronto", "ON", "Canada");

        // Create customers
        Customer customer1 = new Customer("John Doe", addr1);
        Customer customer2 = new Customer("Jane Smith", addr2);

        // Create products
        Product prod1 = new Product("Laptop", "LP1001", 899.99, 1);
        Product prod2 = new Product("Wireless Mouse", "WM2002", 25.99, 2);
        Product prod3 = new Product("Keyboard", "KB3003", 45.50, 1);
        Product prod4 = new Product("Monitor", "MN4004", 150.00, 1);

        // Create orders
        Order order1 = new Order(customer1);
        order1.AddProduct(prod1);
        order1.AddProduct(prod2);

        Order order2 = new Order(customer2);
        order2.AddProduct(prod3);
        order2.AddProduct(prod4);

        // Display order 1 info
        Console.WriteLine("Order 1 Packing Label:");
        Console.WriteLine(order1.GetPackingLabel());
        Console.WriteLine("Order 1 Shipping Label:");
        Console.WriteLine(order1.GetShippingLabel());
        Console.WriteLine($"Order 1 Total Price: ${order1.CalculateTotalCost():F2}");
        Console.WriteLine();

        // Display order 2 info
        Console.WriteLine("Order 2 Packing Label:");
        Console.WriteLine(order2.GetPackingLabel());
        Console.WriteLine("Order 2 Shipping Label:");
        Console.WriteLine(order2.GetShippingLabel());
        Console.WriteLine($"Order 2 Total Price: ${order2.CalculateTotalCost():F2}");
    }
}
