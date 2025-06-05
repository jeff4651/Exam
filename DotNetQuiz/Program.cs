using System.Text.Json;

class Program
{
    static readonly string StatsFile = "stats.json";

    static List<Question> Questions = new()
    {
        new Question
        {
            Id = 1,
            Chapter = 1,
            Text = "What is 2 + 2?",
            Answers = new() { "3", "4", "5" },
            CorrectIndex = 1
        },
        new Question
        {
            Id = 2,
            Chapter = 1,
            Text = "What is the capital of France?",
            Answers = new() { "Berlin", "Paris", "London" },
            CorrectIndex = 1
        },
        new Question
        {
            Id = 3,
            Chapter = 2,
            Text = "Which type is the base of all types in C#?",
            Answers = new() { "object", "int", "string" },
            CorrectIndex = 0
        }
    };

    static Stats stats = new();

    static void Main()
    {
        LoadStats();
        Console.Write("Enter employee ID: ");
        string? employeeId = Console.ReadLine();

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine();
            Console.WriteLine("Select mode:");
            Console.WriteLine("1) Quiz by chapter");
            Console.WriteLine("2) Quiz by frequent mistakes");
            Console.WriteLine("3) Full quiz");
            Console.WriteLine("4) Exit");
            Console.Write("Choice: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    QuizByChapter();
                    break;
                case "2":
                    QuizFrequentMistakes();
                    break;
                case "3":
                    RunQuiz(Questions);
                    break;
                case "4":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }
        }
        SaveStats();
        Console.WriteLine($"Total quizzes taken: {stats.TotalTests}");
    }

    static void QuizByChapter()
    {
        Console.Write("Chapter number: ");
        if (int.TryParse(Console.ReadLine(), out int chapter))
        {
            var qs = Questions.Where(q => q.Chapter == chapter).ToList();
            if (qs.Count == 0)
            {
                Console.WriteLine("No questions in that chapter.");
            }
            else
            {
                RunQuiz(qs);
            }
        }
    }

    static void QuizFrequentMistakes()
    {
        var qs = Questions
            .Where(q => stats.IncorrectCounts.ContainsKey(q.Id) && stats.IncorrectCounts[q.Id] > 0)
            .OrderByDescending(q => stats.IncorrectCounts[q.Id])
            .Take(5)
            .ToList();
        if (qs.Count == 0)
        {
            Console.WriteLine("No mistakes recorded yet.");
        }
        else
        {
            RunQuiz(qs);
        }
    }

    static void RunQuiz(List<Question> qs)
    {
        int correct = 0;
        foreach (var q in qs)
        {
            Console.WriteLine();
            Console.WriteLine(q.Text);
            for (int i = 0; i < q.Answers.Count; i++)
            {
                Console.WriteLine($"{i + 1}) {q.Answers[i]}");
            }
            Console.Write("Answer: ");
            if (int.TryParse(Console.ReadLine(), out int ans) && ans - 1 == q.CorrectIndex)
            {
                Console.WriteLine("Correct!");
                correct++;
            }
            else
            {
                Console.WriteLine($"Wrong. Correct answer: {q.Answers[q.CorrectIndex]}");
                if (!stats.IncorrectCounts.ContainsKey(q.Id))
                    stats.IncorrectCounts[q.Id] = 0;
                stats.IncorrectCounts[q.Id]++;
            }
        }
        stats.TotalTests++;
        Console.WriteLine($"Score: {correct} / {qs.Count}");
    }

    static void LoadStats()
    {
        if (File.Exists(StatsFile))
        {
            string json = File.ReadAllText(StatsFile);
            stats = JsonSerializer.Deserialize<Stats>(json) ?? new Stats();
        }
    }

    static void SaveStats()
    {
        string json = JsonSerializer.Serialize(stats, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(StatsFile, json);
    }
}
