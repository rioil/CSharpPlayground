using Spectre.Console;

namespace SpectreConsole
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            AnsiConsole.Write(
                new FigletText("Sample")
                    .LeftJustified()
                    .Color(Color.Wheat1));
            AnsiConsole.MarkupLine("[underline red]Hello[/] World!");

            await AnsiConsole.Progress().StartAsync(ExecuteTasksAsync);
            WriteCurrentDirectoryTree(3);
            WriteBarChart();
            SelectionPrompt();
        }

        private static async Task ExecuteTasksAsync(ProgressContext ctx)
        {
            var downloadTask = ctx.AddTask("[green]Download files[/]");
            var validationTask = ctx.AddTask("[green]Validate files[/]", false);
            var compileTask = ctx.AddTask("[green]Compile[/]", false);

            while (!ctx.IsFinished)
            {
                await Task.Delay(250);

                // Download
                if (!downloadTask.IsFinished)
                {
                    downloadTask.Increment(8);
                }

                // Validation
                if (validationTask.IsStarted)
                {
                    if (!validationTask.IsFinished)
                    {
                        validationTask.Increment(10);
                    }
                }
                else if (downloadTask.Percentage > 20)
                {
                    validationTask.StartTask();
                }

                // Compilation
                if (validationTask.IsFinished && !compileTask.IsStarted)
                {
                    compileTask.StartTask();
                }
                if (compileTask.IsStarted)
                {
                    compileTask.Increment(20);
                }
            }
        }

        private static void WriteCurrentDirectoryTree(int depth)
        {
            var root = CreateDirectoryTree(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), depth);
            AnsiConsole.Write(root);

            static Tree CreateDirectoryTree(string path, int depth)
            {
                ArgumentOutOfRangeException.ThrowIfLessThan(depth, 1);

                var dirName = Path.GetFileName(path);
                var rootTree = new Tree(FormatDirectoryName(dirName));

                // Directories
                rootTree.AddNodes(Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly)
                                           .Select(x => new DirectoryInfo(x))
                                           .Select(dir =>
                                           {
                                               if (depth == 1)
                                               {
                                                   return new Tree(FormatDirectoryName(dir.Name));
                                               }
                                               else
                                               {
                                                   return CreateDirectoryTree(dir.FullName, depth - 1);
                                               }
                                           }));

                // Files
                rootTree.AddNodes(Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly)
                                           .Select(x => new FileInfo(x))
                                           .Select(x => new Tree(x.Name.EscapeMarkup())));

                return rootTree;

                static string FormatDirectoryName(string name) => $"[blue]{name.EscapeMarkup()}/[/]";
            }
        }

        private static void WriteBarChart()
        {
            AnsiConsole.Write(new BarChart()
                .Width(60)
                .Label("[white]Performance[/]")
                .CenterLabel()
                .AddItem("C#", 10, Color.Blue)
                .AddItem("Rust", 8, Color.Red)
                .AddItem("Go", 6, Color.Yellow)
                .AddItem("Python", 4, Color.Green));
        }

        private static void SelectionPrompt()
        {
            var language = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select your favorite language")
                    .PageSize(4)
                    .AddChoices(["C#", "Rust", "Go", "Python"]));
            AnsiConsole.MarkupLine($"[bold]You selected[/]: {language}");
        }
    }
}
