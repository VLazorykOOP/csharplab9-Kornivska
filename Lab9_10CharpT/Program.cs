using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\nОберіть завдання:");
            Console.WriteLine("1. Перетворення з префіксної форми в постфіксну (Stack)");
            Console.WriteLine("2. Обробка даних про співробітників (Queue)");
            Console.WriteLine("3. Те саме з ArrayList, IComparable, ICloneable");
            Console.WriteLine("4. Каталог музичних дисків (Hashtable)");
            Console.WriteLine("5. Вийти");
            Console.Write("Ваш вибір: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1": PrefixToPostfix(); break;
                case "2": QueueEmployees(); break;
                case "3": ArrayListEmployees(); break;
                case "4": MusicCatalog(); break;
                case "5": return;
                default: Console.WriteLine("Невірний вибір."); break;
            }
        }
    }

    static void PrefixToPostfix()
    {
        Console.Write("Введіть префіксний вираз: ");
        string expr = Console.ReadLine();
        string[] tokens = expr.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        Stack<string> stack = new Stack<string>();

        for (int i = tokens.Length - 1; i >= 0; i--)
        {
            string token = tokens[i];
            if (IsOperator(token))
            {
                string op1 = stack.Pop();
                string op2 = stack.Pop();
                stack.Push($"{op1} {op2} {token}");
            }
            else
            {
                stack.Push(token);
            }
        }

        Console.WriteLine("Постфіксний вираз: " + stack.Pop());
    }

    static bool IsOperator(string token) => "+-*/".Contains(token);

    static void QueueEmployees()
    {
        Queue<string> men = new Queue<string>();
        Queue<string> women = new Queue<string>();

        foreach (var line in File.ReadAllLines("employees.txt"))
        {
            string[] parts = line.Split(',');
            if (parts[3].Trim().ToLower() == "ч")
                men.Enqueue(line);
            else
                women.Enqueue(line);
        }

        Console.WriteLine("Чоловіки:");
        foreach (var m in men) Console.WriteLine(m);
        Console.WriteLine("Жінки:");
        foreach (var w in women) Console.WriteLine(w);
    }

    static void ArrayListEmployees()
    {
        ArrayList list = new ArrayList();
        foreach (var line in File.ReadAllLines("employees.txt"))
            list.Add(new Employee(line));

        list.Sort();

        Console.WriteLine("Відсортований список:");
        foreach (Employee emp in list)
            Console.WriteLine(emp);
    }

    class Employee : IComparable, ICloneable
    {
        public string LastName;
        public string Gender;
        public int Salary;

        public Employee(string csv)
        {
            var parts = csv.Split(',');
            LastName = parts[0].Trim();
            Gender = parts[3].Trim();
            Salary = int.Parse(parts[5].Trim());
        }

        public int CompareTo(object obj)
        {
            return Salary.CompareTo(((Employee)obj).Salary);
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public override string ToString()
        {
            return $"{LastName}, {Gender}, {Salary}";
        }
    }

    static void MusicCatalog()
    {
        Hashtable catalog = new Hashtable();
        while (true)
        {
            Console.WriteLine("\n1. Додати диск\n2. Додати пісню\n3. Пошук по виконавцю\n4. Перегляд\n5. Назад");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Назва диска: ");
                    string dname = Console.ReadLine();
                    catalog[dname] = new List<string>();
                    break;
                case "2":
                    Console.Write("Назва диска: ");
                    string disk = Console.ReadLine();
                    if (catalog.ContainsKey(disk))
                    {
                        Console.Write("Введіть пісню (назва - виконавець): ");
                        ((List<string>)catalog[disk]).Add(Console.ReadLine());
                    }
                    else Console.WriteLine("Диск не знайдено.");
                    break;
                case "3":
                    Console.Write("Виконавець: ");
                    string singer = Console.ReadLine();
                    foreach (DictionaryEntry entry in catalog)
                    {
                        foreach (string song in (List<string>)entry.Value)
                            if (song.Contains(singer))
                                Console.WriteLine($"{entry.Key}: {song}");
                    }
                    break;
                case "4":
                    foreach (DictionaryEntry entry in catalog)
                    {
                        Console.WriteLine($"\nДиск: {entry.Key}");
                        foreach (string song in (List<string>)entry.Value)
                            Console.WriteLine($" - {song}");
                    }
                    break;
                case "5": return;
            }
        }
    }
}
