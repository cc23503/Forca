using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forca21
{
    public class Dicionario
    {
        public string Palavra { get; set; }
        public string Dica { get; set; }

        public Dicionario() { }

        public Dicionario(string linha)
        {
            LerLinha(linha);
        }

        public void LerLinha(string linha)
        {
            var partes = linha.Split(';');
            if (partes.Length == 2)
            {
                Palavra = partes[0].Trim();
                Dica = partes[1].Trim();
            }
        }

        public string FormatarParaArquivo()
        {
            return $"{Palavra};{Dica}";
        }

    }
}
