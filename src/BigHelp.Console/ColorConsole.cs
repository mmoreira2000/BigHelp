using System;
using System.Text.RegularExpressions;

namespace BigHelp.Console
{
    public static class ColorConsole
    {
        private static readonly Lazy<Regex> RegExIdentificaCor = 
            new Lazy<Regex>(() => new Regex("\\[(?<cor>.*?)\\](?<texto>[^[]*)\\[/\\k<cor>\\]", RegexOptions.IgnoreCase),
            isThreadSafe: true);

        /// <summary>
        /// Escreve uma linha de texto com uma cor especifica.
        /// </summary>
        /// <param name="texto">Texto para escrever</param>
        /// <param name="cor">A cor do texto.</param>
        public static void WriteLine(string texto, ConsoleColor? cor = null)
        {
            if (cor.HasValue)
            {
                var oldColor = System.Console.ForegroundColor;
                if (cor == oldColor)
                    System.Console.WriteLine(texto);
                else
                {
                    System.Console.ForegroundColor = cor.Value;
                    System.Console.WriteLine(texto);
                    System.Console.ForegroundColor = oldColor;
                }
            }
            else
                System.Console.WriteLine(texto);
        }

        /// <summary>
        /// Escreve uma linha de texto com uma cor especifica.
        /// </summary>
        /// <param name="texto">Texto para escrever</param>
        /// <param name="cor">A cor do texto. O nome da cor deve constar da coleção ConsoleColors (case insensitive)</param>
        public static void WriteLine(string texto, string cor)
        {
            if (string.IsNullOrEmpty(cor))
            {
                WriteLine(texto);
                return;
            }

            if (!Enum.TryParse(cor, true, out ConsoleColor col))
            {
                WriteLine(texto);
            }
            else
            {
                WriteLine(texto, col);
            }
        }

        /// <summary>
        /// Escreve o texto com uma cor especifica.
        /// </summary>
        /// <param name="texto">Texto para escrever</param>
        /// <param name="cor">A cor do texto.</param>
        public static void Write(string texto, ConsoleColor? cor = null)
        {
            if (cor.HasValue)
            {
                var oldColor = System.Console.ForegroundColor;
                if (cor == oldColor)
                    System.Console.Write(texto);
                else
                {
                    System.Console.ForegroundColor = cor.Value;
                    System.Console.Write(texto);
                    System.Console.ForegroundColor = oldColor;
                }
            }
            else
                System.Console.Write(texto);
        }

        /// <summary>
        /// Escreve o texto com uma cor especifica.
        /// </summary>
        /// <param name="texto">Texto para escrever</param>
        /// <param name="cor">A cor do texto. O nome da cor deve constar da coleção ConsoleColors (case insensitive)</param>
        public static void Write(string texto, string cor)
        {
            if (string.IsNullOrEmpty(cor))
            {
                Write(texto);
                return;
            }

            if (!Enum.TryParse(cor, true, out ConsoleColor col))
            {
                Write(texto);
            }
            else
            {
                Write(texto, col);
            }
        }

        /// <summary>
        /// Escreve um cabeçalho envolto por linhas tracejadas:
        /// --------------------
        /// Exemplo de Cabeçalho
        /// --------------------
        /// permite que você selecione uma cor para o cabeçalho. As linhas tracejadas também serão coloridas.
        /// </summary>
        /// <param name="textoCabecalho">Texto do cabeçalho</param>
        /// <param name="caracterDaLinha">Caracter que forma o cabeçalho. O padrão é '-'</param>
        /// <param name="corCabecalho">Cor do texto do cabeçalho. O padrão é 'yellow'</param>
        /// <param name="corDaLinha">Cor das linhas do cabeçalho. O padrão é 'gray'</param>
        public static void WriteWrappedHeader(string textoCabecalho,
                                                char caracterDaLinha = '-',
                                                ConsoleColor corCabecalho = ConsoleColor.Yellow,
                                                ConsoleColor corDaLinha = ConsoleColor.DarkGray)
        {
            if (string.IsNullOrEmpty(textoCabecalho))
                return;

            string line = new string(caracterDaLinha, textoCabecalho.Length);

            WriteLine(line, corDaLinha);
            WriteLine(textoCabecalho, corCabecalho);
            WriteLine(line, corDaLinha);
        }

        /// <summary>
        /// Escreve uma linha de texto com partes dele coloridos. Exemplo:
        /// Esse texto é [red]vermelho[/red] e esse outro é [blue]azul[/blue].
        /// </summary>
        /// <param name="texto">Texto para escrever</param>
        /// <param name="corPadrao">Cor padrão do texto</param>
        public static void WriteEmbeddedColorLine(string texto, ConsoleColor? corPadrao = null)
        {
            WriteEmbeddedColor(texto,corPadrao);
            System.Console.WriteLine();
        }

        /// <summary>
        /// Escreve o texto com partes dele coloridos. Exemplo:
        /// Esse texto é [red]vermelho[/red] e esse outro é [blue]azul[/blue].
        /// </summary>
        /// <param name="texto">Texto para escrever</param>
        /// <param name="corPadrao">Cor padrão do texto</param>
        public static void WriteEmbeddedColor(string texto, ConsoleColor? corPadrao = null)
        {
            if (corPadrao == null)
                corPadrao = System.Console.ForegroundColor;

            if (string.IsNullOrEmpty(texto))
            {
                WriteLine(string.Empty);
                return;
            }

            int at = texto.IndexOf("[", StringComparison.Ordinal);
            int at2 = texto.IndexOf("]", StringComparison.Ordinal);
            if (at == -1 || at2 <= at)
            {
                WriteLine(texto, corPadrao);
                return;
            }

            while (true)
            {
                var match = RegExIdentificaCor.Value.Match(texto);
                if (match.Length < 1)
                {
                    Write(texto, corPadrao);
                    break;
                }

                // escreve o texto até encontrar o marcador de cor
                Write(texto.Substring(0, match.Index), corPadrao);

                // extrai as partes que serão escritas com uma cor diferente
                string highlightText = match.Groups["texto"].Value;
                string colorVal = match.Groups["cor"].Value;

                Write(highlightText, colorVal);

                //escreve o restante do texto até o próximo bloco, se houver.
                texto = texto.Substring(match.Index + match.Value.Length);
            }
        }

        /// <summary>
        /// Atalho: Escreve uma linha de sucesso em verde.
        /// </summary>
        /// <param name="texto">Texto a ser escrito</param>
        public static void WriteSuccess(string texto)
        {
            WriteLine(texto, ConsoleColor.Green);
        }

        /// <summary>
        /// Atalho: Escreve uma linha de erro em vermelho.
        /// </summary>
        /// <param name="texto">Texto a ser escrito</param>
        public static void WriteError(string texto)
        {
            WriteLine(texto, ConsoleColor.Red);
        }

        /// <summary>
        /// Atalho: Escreve uma linha de alerta em amarelo.
        /// </summary>
        /// <param name="texto">Texto a ser escrito</param>
        public static void WriteWarning(string texto)
        {
            WriteLine(texto, ConsoleColor.DarkYellow);
        }

        /// <summary>
        /// Atalho: Escreve uma linha de informação em ciano.
        /// </summary>
        /// <param name="texto">Texto a ser escrito</param>
        public static void WriteInfo(string texto)
        {
            WriteLine(texto, ConsoleColor.DarkCyan);
        }
    }
}
