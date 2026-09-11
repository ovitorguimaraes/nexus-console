using System;
using System.Text;
using Spectre.Console;
class Ui
{
    public static string[] logo =
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
        Console.OutputEncoding = Encoding.UTF8;
        Console.CursorVisible = false;

        Console.Clear();

        int alturaConsole = AnsiConsole.Profile.Height;
        int larguraConsole = AnsiConsole.Profile.Width;

        int inicioY = (alturaConsole - logo.Length) / 2;

        if (inicioY < 0)
            inicioY = 0;

        int maiorLinha = 0;

        for (int i = 0; i < logo.Length; i++)
        {
            if (logo[i].Length > maiorLinha)
                maiorLinha = logo[i].Length;
        }

        int maxDistancia = maiorLinha / 2 + 1;

        for (int distancia = 0; distancia <= maxDistancia; distancia++)
        {
            for (int i = 0; i < logo.Length; i++)
            {
                string linha = logo[i];

                if (linha.Length == 0)
                    continue;

                int inicioX = (larguraConsole - linha.Length) / 2;

                if (inicioX < 0)
                    inicioX = 0;

                int centroEsquerdo = (linha.Length - 1) / 2;
                int centroDireito = linha.Length / 2;

                int posicaoEsquerda = centroEsquerdo - distancia;
                int posicaoDireita = centroDireito + distancia;

                if (posicaoEsquerda >= 0 &&
                    inicioX + posicaoEsquerda < larguraConsole &&
                    inicioY + i < alturaConsole)
                {
                    Console.SetCursorPosition(
                        inicioX + posicaoEsquerda,
                        inicioY + i
                    );

                    AnsiConsole.Markup(
                        $"[#4980cb]{Markup.Escape(
                            linha[posicaoEsquerda].ToString()
                        )}[/]"
                    );
                }

                if (posicaoDireita < linha.Length &&
                    posicaoDireita != posicaoEsquerda &&
                    inicioX + posicaoDireita < larguraConsole &&
                    inicioY + i < alturaConsole)
                {
                    Console.SetCursorPosition(
                        inicioX + posicaoDireita,
                        inicioY + i
                    );

                    AnsiConsole.Markup(
                        $"[#4980cb]{Markup.Escape(
                            linha[posicaoDireita].ToString()
                        )}[/]"
                    );
                }
            }

            Thread.Sleep(25);
        }

        Thread.Sleep(2000);
    }

    public static void Header()
    {
        Console.Clear();

        AnsiConsole.Write(
            new Markup("[#4980cb]╔═════════════════════════════════════════════════════════════════════════════════════╗[/]")
            .Centered()
        );

        for(int i = 2; i < logo.Length - 1; i++)
        {
            AnsiConsole.Write(
                new Markup(logo[i])
                .Centered()
            );
        }

        AnsiConsole.Write(
            new Markup("[bold #4980cb]by vitor guimaraes[/]")
            .Centered()           
        );

        Footer();
    }

    public static void Footer()
    {
        int posicaoX = Console.CursorLeft;
        int posicaoY = Console.CursorTop;

        int alturaTerminal = AnsiConsole.Profile.Height;

        Console.SetCursorPosition(0, alturaTerminal - 3);

        AnsiConsole.Write(
            new Rule()
                .RuleStyle("#4980cb")
        );

        Console.SetCursorPosition(0, alturaTerminal - 2);

        AnsiConsole.Write(
            new Rule()
                .RuleStyle("#4980cb")
        );

        Console.SetCursorPosition(posicaoX, posicaoY);
    }
}