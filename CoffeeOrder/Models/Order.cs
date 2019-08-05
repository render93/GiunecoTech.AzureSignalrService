namespace CoffeeOrder.Models
{
    public class Order
    {
        public Product Product { get; set; }
        public string ProductString => Product.ToString();
        public Size Size { get; set; }
        public string SizeString => Size.ToString();
        public string SenderId { get; set; }
    }

    public enum Product
    {
        Espresso,
        Americano,
        Cappuccino,
        Latte,
        Cioccolata
    }

    public enum Size
    {
        Piccolo = 0,
        Medio = 1,
        Grande = 2,
        Gigantesco = 3
    }
}
