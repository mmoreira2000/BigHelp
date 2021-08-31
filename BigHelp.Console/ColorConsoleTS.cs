using System;

namespace BigHelp.Console
{
    /// <summary>
    /// Console colorido proprio para rodar em multithread.
    /// </summary>
    public static class ColorConsoleTS
    {
        public static object lockObject = new object();

        /// <summary>
        /// Escreve uma linha de texto com uma cor especifica.
        /// </summary>
        /// <param name="texto">Texto para escrever</param>
        /// <param name="cor">A cor do texto.</param>
        public static void WriteLine(string texto, ConsoleColor? cor = null)
        {
            lock (lockObject) ColorConsole.WriteLine(texto, cor);
        }

        /// <summary>
        /// Escreve uma linha de texto com uma cor especifica.
        /// </summary>
        /// <param name="texto">Texto para escrever</param>
        /// <param name="cor">A cor do texto. O nome da cor deve constar da coleção ConsoleColors (case insensitive)</param>
        public static void WriteLine(string texto, string cor)
        {
            lock (lockObject) ColorConsole.WriteLine(texto, cor);
        }

        /// <summary>
        /// Escreve o texto com uma cor especifica.
        /// </summary>
        /// <param name="texto">Texto para escrever</param>
        /// <param name="cor">A cor do texto.</param>
        public static void Write(string texto, ConsoleColor? cor = null)
        {
            lock (lockObject) ColorConsole.Write(texto, cor);
        }

        /// <summary>
        /// Escreve o texto com uma cor especifica.
        /// </summary>
        /// <param name="texto">Texto para escrever</param>
        /// <param name="cor">A cor do texto. O nome da cor deve constar da coleção ConsoleColors (case insensitive)</param>
        public static void Write(string texto, string cor)
        {
            lock (lockObject) ColorConsole.Write(texto, cor);
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
            lock (lockObject) ColorConsole.WriteWrappedHeader(textoCabecalho,caracterDaLinha,corCabecalho,corDaLinha);
        }

        /// <summary>
        /// Escreve uma linha de texto com partes dele coloridos. Exemplo:
        /// Esse texto é [red]vermelho[/red] e esse outro é [blue]azul[/blue].
        /// </summary>
        /// <param name="texto">Texto para escrever</param>
        /// <param name="corPadrao">Cor padrão do texto</param>
        public static void WriteEmbeddedColorLine(string texto, ConsoleColor? corPadrao = null)
        {
            lock (lockObject) ColorConsole.WriteEmbeddedColorLine(texto, corPadrao);
        }

        /// <summary>
        /// Escreve o texto com partes dele coloridos. Exemplo:
        /// Esse texto é [red]vermelho[/red] e esse outro é [blue]azul[/blue].
        /// </summary>
        /// <param name="texto">Texto para escrever</param>
        /// <param name="corPadrao">Cor padrão do texto</param>
        public static void WriteEmbeddedColor(string texto, ConsoleColor? corPadrao = null)
        {
            lock (lockObject) ColorConsole.WriteEmbeddedColor(texto, corPadrao);
        }

        /// <summary>
        /// Atalho: Escreve uma linha de sucesso em verde.
        /// </summary>
        /// <param name="texto">Texto a ser escrito</param>
        public static void WriteSuccess(string texto)
        {
            lock (lockObject) ColorConsole.WriteSuccess(texto);
        }

        /// <summary>
        /// Atalho: Escreve uma linha de erro em vermelho.
        /// </summary>
        /// <param name="texto">Texto a ser escrito</param>
        public static void WriteError(string texto)
        {
            lock (lockObject) ColorConsole.WriteError(texto);
        }

        /// <summary>
        /// Atalho: Escreve uma linha de alerta em amarelo.
        /// </summary>
        /// <param name="texto">Texto a ser escrito</param>
        public static void WriteWarning(string texto)
        {
            lock (lockObject) ColorConsole.WriteWarning(texto);
        }

        /// <summary>
        /// Atalho: Escreve uma linha de informação em ciano.
        /// </summary>
        /// <param name="texto">Texto a ser escrito</param>
        public static void WriteInfo(string texto)
        {
            lock (lockObject) ColorConsole.WriteInfo(texto);
        }
    }

}
