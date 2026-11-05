using System.Globalization;
using TaskTracker.Models;
using TaskStatus = TaskTracker.Models.TaskStatus;
using TaskTracker.Services;

namespace TaskTracker;

internal static class Program
{
    private static readonly TaskManager TaskManager = new();

    private static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("====================================");
        Console.WriteLine("          TASK TRACKER");
        Console.WriteLine("====================================");

        bool isRunning = true;

        while (isRunning)
        {
            ShowMainMenu();
            string choice = ReadRequiredInput("Choose an option: ");
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    AddTask();
                    break;
                case "2":
                    UpdateTaskStatus();
                    break;
                case "3":
                    ViewTasksMenu();
                    break;
                case "4":
                    DeleteTask();
                    break;
                case "5":
                    isRunning = false;
                    Console.WriteLine("Your tasks have been saved. Goodbye!");
                    break;
                default:
                    Console.WriteLine("Invalid option. Please choose a number from 1 to 5.");
                    break;
            }

            if (isRunning)
            {
                Console.WriteLine();
                Console.WriteLine("Press any key to return to the main menu...");
                Console.ReadKey(intercept: true);
                Console.Clear();
            }
        }
    }

    private static void ShowMainMenu()
    {
        Console.WriteLine("--------------- MAIN MENU ---------------");
        Console.WriteLine("1. Add a task");
        Console.WriteLine("2. Update task status");
        Console.WriteLine("3. View tasks");
        Console.WriteLine("4. Delete a task");
        Console.WriteLine("5. Exit");
        Console.WriteLine("-----------------------------------------");
    }

    private static void AddTask()
    {
        Console.WriteLine("--------------- ADD TASK ---------------");

        string title = ReadRequiredInput("Title: ");
        string description = ReadRequiredInput("Description: ");
        DateTime dueDate = ReadDate("Due date (yyyy-MM-dd): ");
        PriorityLevel priority = ReadPriority();

        TaskItem newTask = TaskManager.AddTask(title, description, dueDate, priority);
        Console.WriteLine($"Task #{newTask.Id} was added successfully.");
    }

    private static void UpdateTaskStatus()
    {
        Console.WriteLine("------------ UPDATE TASK STATUS ------------");
        DisplayTasks(TaskManager.GetAllTasks());

        if (TaskManager.GetAllTasks().Count == 0)
        {
            return;
        }

        int id = ReadPositiveInteger("Enter the task ID to update: ");
        TaskItem? task = TaskManager.FindTaskById(id);

        if (task is null)
        {
            Console.WriteLine("No task was found with that ID.");
            return;
        }

        Console.WriteLine($"Selected task: {task.Title}");
        Console.WriteLine("1. Pending");
        Console.WriteLine("2. In Progress");
        Console.WriteLine("3. Completed");

        TaskStatus status = ReadStatus();
        TaskManager.UpdateTaskStatus(id, status);
        Console.WriteLine("Task status updated successfully.");
    }

    private static void ViewTasksMenu()
    {
        bool viewing = true;

        while (viewing)
        {
            Console.WriteLine("--------------- VIEW TASKS ---------------");
            Console.WriteLine("1. Active tasks");
            Console.WriteLine("2. Overdue tasks");
            Console.WriteLine("3. Completed tasks");
            Console.WriteLine("4. All tasks");
            Console.WriteLine("5. Back to main menu");

            string choice = ReadRequiredInput("Choose a category: ");
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("ACTIVE TASKS");
                    DisplayTasks(TaskManager.GetActiveTasks());
                    break;
                case "2":
                    Console.WriteLine("OVERDUE TASKS");
                    DisplayTasks(TaskManager.GetOverdueTasks());
                    break;
                case "3":
                    Console.WriteLine("COMPLETED TASKS");
                    DisplayTasks(TaskManager.GetCompletedTasks());
                    break;
                case "4":
                    Console.WriteLine("ALL TASKS");
                    DisplayTasks(TaskManager.GetAllTasks());
                    break;
                case "5":
                    viewing = false;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please choose a number from 1 to 5.");
                    break;
            }

            if (viewing)
            {
                Console.WriteLine();
                Console.WriteLine("Press any key to continue viewing...");
                Console.ReadKey(intercept: true);
                Console.Clear();
            }
        }
    }

    private static void DeleteTask()
    {
        Console.WriteLine("--------------- DELETE TASK ---------------");
        DisplayTasks(TaskManager.GetAllTasks());

        if (TaskManager.GetAllTasks().Count == 0)
        {
            return;
        }

        int id = ReadPositiveInteger("Enter the task ID to delete: ");
        TaskItem? task = TaskManager.FindTaskById(id);

        if (task is null)
        {
            Console.WriteLine("No task was found with that ID.");
            return;
        }

        Console.Write($"Are you sure you want to delete '{task.Title}'? (y/n): ");
        string confirmation = (Console.ReadLine() ?? string.Empty).Trim();

        if (confirmation.Equals("y", StringComparison.OrdinalIgnoreCase))
        {
            TaskManager.DeleteTask(id);
            Console.WriteLine("Task deleted successfully.");
        }
        else
        {
            Console.WriteLine("Delete cancelled.");
        }
    }

    private static void DisplayTasks(IReadOnlyList<TaskItem> tasks)
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("No tasks found in this category.");
            return;
        }

        Console.WriteLine(new string('-', 76));
        Console.WriteLine($"{"ID",-5} {"TITLE",-22} {"DUE DATE",-13} {"PRIORITY",-10} {"STATUS",-15}");
        Console.WriteLine(new string('-', 76));

        foreach (TaskItem task in tasks)
        {
            Console.WriteLine(
                $"{task.Id,-5} {Truncate(task.Title, 21),-22} {task.DueDate,-13:yyyy-MM-dd} {task.Priority,-10} {FormatStatus(task.Status),-15}");
            Console.WriteLine($"      Description: {task.Description}");
        }

        Console.WriteLine(new string('-', 76));
    }

    private static string FormatStatus(TaskStatus status)
    {
        return status == TaskStatus.InProgress ? "In Progress" : status.ToString();
    }

    private static string Truncate(string value, int maxLength)
    {
        if (value.Length <= maxLength)
        {
            return value;
        }

        return value[..(maxLength - 3)] + "...";
    }

    private static string ReadRequiredInput(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string value = (Console.ReadLine() ?? string.Empty).Trim();

            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            Console.WriteLine("This field cannot be empty. Please try again.");
        }
    }

    private static int ReadPositiveInteger(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = (Console.ReadLine() ?? string.Empty).Trim();

            if (int.TryParse(input, out int number) && number > 0)
            {
                return number;
            }

            Console.WriteLine("Please enter a valid positive whole number.");
        }
    }

    private static DateTime ReadDate(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = (Console.ReadLine() ?? string.Empty).Trim();

            if (DateTime.TryParseExact(
                    input,
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime dueDate))
            {
                return dueDate.Date;
            }

            Console.WriteLine("Invalid date. Use the format yyyy-MM-dd, for example 2026-12-31.");
        }
    }

    private static PriorityLevel ReadPriority()
    {
        while (true)
        {
            Console.WriteLine("Priority:");
            Console.WriteLine("1. Low");
            Console.WriteLine("2. Medium");
            Console.WriteLine("3. High");
            Console.Write("Choose priority: ");

            string input = (Console.ReadLine() ?? string.Empty).Trim();

            if (int.TryParse(input, out int choice) &&
                Enum.IsDefined(typeof(PriorityLevel), choice))
            {
                return (PriorityLevel)choice;
            }

            Console.WriteLine("Invalid priority. Please choose 1, 2, or 3.");
        }
    }

    private static TaskStatus ReadStatus()
    {
        while (true)
        {
            Console.Write("Choose status: ");
            string input = (Console.ReadLine() ?? string.Empty).Trim();

            if (int.TryParse(input, out int choice) &&
                Enum.IsDefined(typeof(TaskStatus), choice))
            {
                return (TaskStatus)choice;
            }

            Console.WriteLine("Invalid status. Please choose 1, 2, or 3.");
        }
    }
}
