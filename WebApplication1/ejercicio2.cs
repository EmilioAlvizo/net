public class ejercicio2
{
    public static void Correr()
    {
        Console.WriteLine("..............................................");
        Console.WriteLine("\t ejercicio2");
        Console.WriteLine("..............................................");

        //Para declarar una nueva matriz de cadenas que contengan tres elementos
        string[] fraudulentOrderIDs = new string[3];

        fraudulentOrderIDs[0] = "A123";
        fraudulentOrderIDs[1] = "B456";
        fraudulentOrderIDs[2] = "C789";

        Console.WriteLine($"First: {fraudulentOrderIDs[0]}");
        Console.WriteLine($"Second: {fraudulentOrderIDs[1]}");
        Console.WriteLine($"Third: {fraudulentOrderIDs[2]}\n");

        //string[] fraudulentOrderIDs = [ "X123", "Y456", "Z789" ];

        Console.WriteLine($"la longitud del arrray es: {fraudulentOrderIDs.Length} \n");

        string[] nombres = ["Rowena", "Robin", "Bao"];
        foreach (string nombre in nombres)
        {
            Console.WriteLine(nombre);
        }
        Console.WriteLine("\n");

        int[] inventory = { 200, 450, 700, 175, 250 };
        int sum = 0;
        int bin = 0;
        foreach (int items in inventory)
        {
            sum += items;
            bin++;
            Console.WriteLine($"Bin {bin} = {items} items (Running total: {sum})");
        }
        Console.WriteLine($"We have {sum} items in inventory.");


    }
}