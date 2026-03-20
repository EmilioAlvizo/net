public class ejercicio3
{
    public static void Correr()
    {
        Console.WriteLine("..............................................");
        Console.WriteLine("\t ejercicio3");
        Console.WriteLine("..............................................");

        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine(i);
            if (i == 7) break;
        }

        // do while
        Console.WriteLine("\ndo");

        Random random = new Random();
        int current = 0;

        do
        {
            current = random.Next(1, 21);

            // salta el numero 10
            if (current >= 10) continue;

            Console.WriteLine(current);
        } while (current != 7);

        // while
        Console.WriteLine("\nwhile");

        int current2 = random.Next(1, 11);

        while (current2 >= 3)
        {
            Console.WriteLine(current2);
            current2 = random.Next(1, 11);
        }
        Console.WriteLine($"Last number: {current2}");
    }
}