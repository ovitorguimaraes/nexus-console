using Spectre.Console;

class Program
{
    public static string user = "BVITOR";
    public static string password = "1234";
    static void Main(string[] args)
    {
        Ui.SplashScreen();

        Ui.Header();

        Login(user, password);

        Ui.Header();

        switch (Menu())
        {
            case "Run NFe App":
                NFe.AppNFe();
                break;

            case "Run CTe App":
                CTe.AppCTe();
                break;

            case "Run NFSe App":
                NFSe.AppNFSe();
                break;

            case "Run NFAg App":
                NFAg.AppNFAg();
                break;

            case "Run NF3e App":
                NF3e.AppNF3e();
                break;

            case "Run NFCom App":
                NFCom.AppNFCom();
                break;

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

    static string Menu()
    {
        string option = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .AddChoices("Run NFe App", "Run CTe App", "Run NFSe App", "Run NFAg App", "Run NF3e App", "Run NFCom App", "Sair")
        );

        return option;
    }

    static void Login(string user, string password)
    {
        Ui.Header();

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

            Thread.Sleep(3000);

            Login(user, password);
        }
    }
}