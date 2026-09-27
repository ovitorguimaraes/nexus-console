using Spectre.Console;
using Nexus.UI;

class Program
{
    public static string user = "BVITOR";
    public static string password = "1234";
    static void Main(string[] args)
    {
        if(args.Length != 0 && args[0] == "test")
            Test.Tests();

        Ui.SplashScreen();

        Login(user, password, 3);

        Ui.AppUi("clear", "");

        switch (Menu())
        {
            case "Run NFe App":
                foreach(var lineData in File.ReadAllLines("reportNFe.csv").Skip(1).Select((reportLine, index) => new { reportLine, index }))
                {
                    string[] columns = lineData.reportLine.Split(";");
                    NFe.ProcessNFe(columns[1], columns[2], columns[12], columns[13], lineData.index + 1);
                }
                Ui.NFeApp(NFe.ok, NFe.xmlNotFound, NFe.errors);
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
            Console.Clear();
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

    static void Login(string user, string password, int attempts)
    {
        string inputUser = null!;
        string inputPassword = null!;

        ConsoleKeyInfo key;

        Console.Clear();

        AnsiConsole.Write(
            new Panel("[bold]ACESSO AO SISTEMA[/]\n[gray58]Informe suas credenciais para continuar[/]" +
            $"{(attempts != 3 
                ? attempts == 2 
                    ? $"[gray58], [/][yellow]você tem {attempts} tentativas restantes[/]" 
                    : $"[gray58], [/][red]você tem {attempts} tentativa restante[/]" 
                : null)}")
            {
                Width = Console.WindowWidth - 2
            }
            .Header("[bold #4980cb]NEXUS[/]")
            .Border(BoxBorder.Rounded)
            .BorderStyle("#4980cb")
        );

        AnsiConsole.Markup("\n[gray58]Pressione ESC para encerrar.[/]\n");

        Console.WriteLine();

        AnsiConsole.Markup("[bold]Usuário: [/]");

        do{
            key = Console.ReadKey(true);

            if(key.Key == ConsoleKey.Escape)
            {
                Console.Clear();
                Environment.Exit(0);
            }

            if(key.Key == ConsoleKey.Backspace)
            {
                if(inputUser.Length > 0)
                {
                    inputUser = inputUser.Remove(inputUser.Length - 1);
                    Console.Write("\b \b");
                }

                continue;
            }

            if(key.Key != ConsoleKey.Enter)
            {
                char letter = char.ToUpper(key.KeyChar);

                Console.Write(letter);
                inputUser += letter;
            }
        
        } while(key.Key != ConsoleKey.Enter);

        Console.WriteLine();

        AnsiConsole.Markup("[bold]Senha: [/]");

        do
        {
            key = Console.ReadKey(true);

            if(key.Key == ConsoleKey.Escape)
            {
                Console.Clear();
                Environment.Exit(0);
            }

            if(key.Key == ConsoleKey.Backspace)
            {
                if(inputPassword.Length > 0)
                {
                    inputPassword = inputPassword.Remove(inputPassword.Length - 1);
                    Console.Write("\b \b");
                }

                continue;
            }

            if(key.Key != ConsoleKey.Enter)
            {
                AnsiConsole.Markup("[bold gray58]*[/]");
                inputPassword += char.ToUpper(key.KeyChar);
            }

        } while(key.Key != ConsoleKey.Enter);

        if(inputUser == user && inputPassword == password)
        {
            return;
        }

        if(--attempts == 0)
        {
            Console.Clear();
            Environment.Exit(0);
        }

        Console.WriteLine();

        Console.SetCursorPosition(0, 0);
        AnsiConsole.Write(
            new Panel("[bold]ACESSO AO SISTEMA NEGADO[/]\n[gray58]Informe suas credenciais novamente[/]")
            {
                Width = Console.WindowWidth - 2
            }
            .Header("[bold red]NEXUS[/]")
            .Border(BoxBorder.Rounded)
            .BorderStyle(Color.Red)
        );
    
        Thread.Sleep(3000);
        Login(user, password, attempts);
    }
}