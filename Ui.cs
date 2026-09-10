using System;
using System.Text;
using Spectre.Console;

class Ui
{
    public static void SplashScreen()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.CursorVisible = false;

        string[] logo =
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
            new Rows(
                new Rule()
                .RuleStyle("#4980cb"),

                new Rule()
                .RuleStyle("#4980cb")
            )
        );

        AnsiConsole.Write(
            new Markup(@"[white]_ __   _____  ___   _ ___
| '_ \ / _ \ \/ / | | / __|
| | | |  __/>  <| |_| \__ \
|_| |_|\___/_/\_\\__,_|___/[/]")
            .Centered()
        );

        AnsiConsole.Write(
            new Markup("[bold #4980cb]by vitor guimaraes[/]")
            .Centered()           
        );
    }
}