// ejercicio1.cs
public class ejercicio1
{
    public static void Correr()
    {
        Console.WriteLine("Hola desde mis ejercicios!");
        // todo tu código de prueba aquí

        // initialize variables - graded assignments 
        int currentAssignments = 5;

        int sophia1 = 93;
        int sophia2 = 87;
        int sophia3 = 98;
        int sophia4 = 95;
        int sophia5 = 100;

        int nicolas1 = 80;
        int nicolas2 = 83;
        int nicolas3 = 82;
        int nicolas4 = 88;
        int nicolas5 = 85;

        int zahirah1 = 84;
        int zahirah2 = 96;
        int zahirah3 = 73;
        int zahirah4 = 85;
        int zahirah5 = 79;

        int jeong1 = 90;
        int jeong2 = 92;
        int jeong3 = 98;
        int jeong4 = 100;
        int jeong5 = 97;

        int sophiaSum = sophia1 + sophia2 + sophia3 + sophia4 + sophia5;
        int nicolasSum = nicolas1 + nicolas2 + nicolas3 + nicolas4 + nicolas5;
        int zahirahSum = zahirah1 + zahirah2 + zahirah3 + zahirah4 + zahirah5;
        int jeongSum = jeong1 + jeong2 + jeong3 + jeong4 + jeong5;

        decimal sophiaScore = (decimal)sophiaSum / currentAssignments;
        decimal nicolasScore = (decimal)nicolasSum / currentAssignments;
        decimal zahirahScore = (decimal)zahirahSum / currentAssignments;
        decimal jeongScore = (decimal)jeongSum / currentAssignments;

        Console.WriteLine("Student\t\tGrade\n");
        Console.WriteLine("Sophia:\t\t" + sophiaScore + "\tA");
        Console.WriteLine("Nicolas:\t" + nicolasScore + "\tB");
        Console.WriteLine("Zahirah:\t" + zahirahScore + "\tB");
        Console.WriteLine("Jeong:\t\t" + jeongScore + "\tA");

        Random dice = new Random();
        int roll1 = dice.Next(1,7);
        int roll2 = dice.Next(1,7);
        int roll3 = dice.Next(1,7);
        int total = roll1 + roll2 + roll3;

        Console.WriteLine($"Dice roll: {roll1} + {roll2} + {roll3} = {total}");
        int varMayor =Math.Max(roll1, Math.Max(roll2, roll3));

        if (total > 14)
        {
            Console.WriteLine("You win!\t" + varMayor);
        }
        if (total < 15)
        {
            Console.WriteLine("Sorry, you lose.\t" + varMayor);
        }

        
    }
}