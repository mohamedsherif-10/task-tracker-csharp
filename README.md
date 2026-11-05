# Task Tracker (C# Console Application)

A beginner-friendly task management application built with C# and .NET 8.

## Features

- Add tasks with a title, description, due date, and priority.
- Change a task's status: Pending, In Progress, or Completed.
- View Active, Overdue, Completed, or All tasks.
- Delete tasks after confirmation.
- Save tasks automatically to `tasks.json`, so they are available the next time the app runs.
- Validate user input, including dates, task IDs, priorities, and menu choices.

## Requirements

- .NET 8 SDK or newer
- Windows, macOS, or Linux terminal

## Run the application

1. Open a terminal in the folder containing `TaskTracker.csproj`.
2. Run:

   ```bash
   dotnet run
   ```

The application creates `tasks.json` in the current working directory when the first task is saved.

## Task categories

- **Active:** Tasks that are not completed and have a due date of today or later.
- **Overdue:** Tasks that are not completed and have a due date before today.
- **Completed:** Tasks whose status is Completed.

A task moves between categories automatically when its status or due date changes.

## Project structure

```text
TaskTracker/
├── Models/
│   ├── PriorityLevel.cs
│   ├── TaskItem.cs
│   └── TaskStatus.cs
├── Services/
│   └── TaskManager.cs
├── Program.cs
├── TaskTracker.csproj
├── .gitignore
└── README.md
```

## Concepts practiced

- Classes and objects
- Enums
- Lists and collections
- Methods and return values
- Loops and `switch` statements
- Conditional statements
- Input validation and exception handling
- LINQ filtering and sorting
- Reading and writing JSON files

## Publish to GitHub

1. Sign in to GitHub and create a new empty repository named `task-tracker-csharp`.
2. Open a terminal in this project folder.
3. Run the following commands, replacing `YOUR-USERNAME` with your GitHub username:

   ```bash
   git init
   git add .
   git commit -m "Create C# Task Tracker application"
   git branch -M main
   git remote add origin https://github.com/YOUR-USERNAME/task-tracker-csharp.git
   git push -u origin main
   ```
4. Submit your repository URL:

   `https://github.com/YOUR-USERNAME/task-tracker-csharp`

Do not commit `bin/`, `obj/`, or your personal `tasks.json` file; they are excluded by `.gitignore`.
