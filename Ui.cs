using System;
using System.Text;
using Spectre.Console;

namespace Nexus.UI;

class Ui
{
    static string[] logo =
    {
        "╔══════════════════════════════════════════════╗",
        "",
        "███╗   ██╗███████╗██╗  ██╗██╗   ██╗███████╗",
        "████╗  ██║██╔════╝╚██╗██╔╝██║   ██║██╔════╝",
        "██╔██╗ ██║█████╗   ╚███╔╝ ██║   ██║███████╗",
        "██║╚██╗██║██╔══╝   ██╔██╗ ██║   ██║╚════██║",
        "██║ ╚████║███████╗██╔╝ ██╗╚██████╔╝███████║",
        "╚═╝  ╚═══╝╚══════╝╚═╝  ╚═╝ ╚═════╝ ╚══════╝",
        "",
        "╚══════════════════════════════════════════════╝"
    };

    public static void SplashScreen()
    {
        Console.Clear();

        Console.OutputEncoding = Encoding.UTF8;
        Console.CursorVisible = false;

        int heightConsole = AnsiConsole.Profile.Height;
        int widthConsole = AnsiConsole.Profile.Width;

        int startY = (heightConsole - logo.Length) / 2;

        if (startY < 0)
            startY = 0;

        int longestLine = 0;

        for (int i = 0; i < logo.Length; i++)
        {
            if (logo[i].Length > longestLine)
                longestLine = logo[i].Length;
        }

        int maxDistance = longestLine / 2 + 1;

        for (int distance = 0; distance <= maxDistance; distance++)
        {
            for (int i = 0; i < logo.Length; i++)
            {
                string line = logo[i];

                if (line.Length == 0)
                    continue;

                int startX = (widthConsole - line.Length) / 2;

                if (startX < 0)
                    startX = 0;

                int centerLeft = (line.Length - 1) / 2;
                int centerRight = line.Length / 2;

                int leftPosition = centerLeft - distance;
                int rightPosition = centerRight + distance;

                if (leftPosition >= 0 &&
                    startX + leftPosition < widthConsole &&
                    startY + i < heightConsole)
                {
                    Console.SetCursorPosition(
                        startX + leftPosition,
                        startY + i
                    );

                    AnsiConsole.Markup(
                        $"[#4980cb]{Markup.Escape(
                            line[leftPosition].ToString()
                        )}[/]"
                    );
                }

                if (rightPosition < line.Length &&
                    rightPosition != leftPosition &&
                    startX + rightPosition < widthConsole &&
                    startY + i < heightConsole)
                {
                    Console.SetCursorPosition(
                        startX + rightPosition,
                        startY + i
                    );

                    AnsiConsole.Markup(
                        $"[#4980cb]{Markup.Escape(
                            line[rightPosition].ToString()
                        )}[/]"
                    );
                }
            }

            Thread.Sleep(25);
        }

        Thread.Sleep(2000);
    }

    static void Header()
    {
        Console.SetCursorPosition(0, 2);
        for(int i = 2; i < logo.Length - 1; i++)
        {
            AnsiConsole.Write(
                new Markup(logo[i])
                .Centered()
            );
        }
    }

    static void Border()
    {
        int width = Console.WindowWidth;
        int height = Console.WindowHeight;

        Console.SetCursorPosition(0, 0);
        Console.Write("╔");

        Console.SetCursorPosition(width - 1, 0);
        Console.Write("╗");

        Console.SetCursorPosition(0, height - 1);
        Console.Write("╚");

        Console.SetCursorPosition(width - 1, height - 1);
        Console.Write("╝");

        for (int i = 1; i < width - 1; i++)
        {
            Console.SetCursorPosition(i, 0);
            Console.Write("═");

            Console.SetCursorPosition(i, height - 1);
            Console.Write("═");
        }

        for (int i = 1; i < height - 1; i++)
        {
            Console.SetCursorPosition(0, i);
            Console.Write("║");

            Console.SetCursorPosition(width - 1, i);
            Console.Write("║");
        }
        Console.Write("\x1b[0m");
    }

    public static void AppUi(string clear, string color)
    {
        if(clear == "clear")
            Console.Clear();
        Header();

        if(color == "error")
            Console.ForegroundColor = ConsoleColor.Red;
        else
            Console.Write("\x1b[38;2;73;128;203m");
        Border();
    }

    public static void NFeApp(int checkedLines, List<int> xmlNotFound, List<Error> errors)
    {
        List<int> errorLines = errors.Select(error => error.Line).Distinct().ToList();

        AnsiConsole.Write(
            new BreakdownChart()
            .AddItem("Documentos validados", checkedLines, Color.Green)
            .AddItem("Documentos com erros", errorLines.Count, Color.Red)
            .AddItem("Documentos não encontrados", xmlNotFound.Count, Color.Yellow)
        );

        Console.WriteLine();
        AnsiConsole.Write(
            new Markup("[bold red]RELAÇÃO DE NOTAS FISCAIS ESCRITURADAS COM ERROS[/]")
        );
        Console.WriteLine();

        Table reportErrors = new Table()
            .BorderColor(Color.Red)
            .AddColumn("[bold grey]SERIE[/]")
            .AddColumn("[bold grey]NÚMERO[/]")
            .AddColumn("[bold grey]CNPJ FORNECEDOR[/]")
            .AddColumn("[bold grey]DATA EMISSÃO[/]")
            .AddColumn("[bold grey]CFOP ENTRADA[/]")
            .AddColumn("[bold grey]VALOR DOS PRODUTOS[/]")
            .AddColumn("[bold grey]VALOR TOTAL DA NOTA[/]")
            .AddColumn("[bold grey]VALOR A PAGAR[/]")
            .AddColumn("[bold grey]VALOR IPI[/]")
            .AddColumn("[bold grey]VALOR ICMS[/]")
            .AddColumn("[bold grey]VALOR PIS[/]")
            .AddColumn("[bold grey]VALOR COFINS[/]")
            .AddColumn("[bold grey]CONTA CONTÁBIL[/]")
            .AddColumn("[bold grey]CENTRO DE CUSTO[/]");
        
        foreach (int line in errorLines)
        {
            string[] columns = NFeRules.report[line].Split(';');

            foreach (Error error in errors.Where(error => error.Line == line))
            {
                columns[error.Column] =
                    $"[bold red1]{Markup.Escape(columns[error.Column])}[/]";
            }

            reportErrors.AddRow(columns);
        }
        
        AnsiConsole.Write(reportErrors);

        Console.WriteLine();
        AnsiConsole.Write(
            new Markup("[bold yellow]RELAÇÃO DE NOTAS FISCAIS EM QUE O DOCUMENTO XML NÃO FOI ENCONTRADO[/]")
        );
        Console.WriteLine();

        Table reportNotFounds = new Table()
            .BorderColor(Color.Yellow)
            .AddColumn("[bold]SERIE[/]")
            .AddColumn("[bold]NÚMERO[/]")
            .AddColumn("[bold]CNPJ FORNECEDOR[/]")
            .AddColumn("[bold]DATA EMISSÃO[/]")
            .AddColumn("[bold]CFOP ENTRADA[/]")
            .AddColumn("[bold]VALOR DOS PRODUTOS[/]")
            .AddColumn("[bold]VALOR TOTAL DA NOTA[/]")
            .AddColumn("[bold]VALOR A PAGAR[/]")
            .AddColumn("[bold]VALOR IPI[/]")
            .AddColumn("[bold]VALOR ICMS[/]")
            .AddColumn("[bold]VALOR PIS[/]")
            .AddColumn("[bold]VALOR COFINS[/]")
            .AddColumn("[bold]CONTA CONTÁBIL[/]")
            .AddColumn("[bold]CENTRO DE CUSTO[/]");

        foreach (int line in xmlNotFound)
        {
            string[] columns = NFeRules.report[line].Split(';');

            reportNotFounds.AddRow(columns);
        }

        AnsiConsole.Write(reportNotFounds);

        Console.ReadKey();

        Environment.Exit(0);
    }
}