using Spectre.Console;

class Program
{
    public static string user = "BVITOR";
    public static string password = "1234";
    static void Main(string[] args)
    {
        Login(user, password);

        Ui();

        switch (Menu())
        {
            case "Sair":

            Console.WriteLine();

            AnsiConsole.Write(
                Align.Center(
                    new Panel("[red]Saindo...[/] Até a próxima!")
                    .BorderColor(Color.Red)
                    .Border(BoxBorder.Rounded)
                )
            );

            Thread.Sleep(2000);

                break;
        }
    }

    static void Ui()
    {
        Console.Clear();

        AnsiConsole.Write(
            new Rows(
                new Rule(),
                new Rule()
            )
        );

        AnsiConsole.Write(
            new FigletText("nexus")
            .Centered()
            .Color(Color.Blue)
        );

        AnsiConsole.Write(
            new Markup("[bold white]S Riko Automotive Hose[/]")
            .Centered()           
        );
    }

    static string Menu()
    {
        string option = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .AddChoices("Consultar", "Cadastrar", "Sair")
        );

        return option;
    }

    static void Login(string user, string password)
    {
        Ui();

        Console.WriteLine();

        string readUser = AnsiConsole.Ask<string>(
            "Usuário: "
        ).ToUpper();

        string readPass = AnsiConsole.Ask<string>(
            "Senha: "
        );

        if(readUser == user && readPass == password)
        {
            return;
        }

        else
        {
            AnsiConsole.Write(
                Align.Center(
                    new Panel("[bold red]ACESSO NEGADO[/]")
                    .BorderColor(Color.Red)
                    .Border(BoxBorder.Rounded)
                )
            );

            Console.CursorVisible = false;
            Thread.Sleep(3000);
            Console.CursorVisible = true;

            Login(user, password);
        }
    }
}