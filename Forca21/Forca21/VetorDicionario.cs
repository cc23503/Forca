using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forca21
{
    public class VetorDicionario
    {
        private List<Dicionario> lista = new List<Dicionario>();
        public int PosicaoAtual { get; private set; } = -1;

        public void AdicionarOrdenado(Dicionario d)
        {
            int i = 0;
            while (i < lista.Count && string.Compare(lista[i].Palavra, d.Palavra, StringComparison.OrdinalIgnoreCase) < 0)
            {
                i++;
            }
            lista.Insert(i, d);
            PosicaoAtual = i;
        }

        public void ExcluirAtual()
        {
            if (PosicaoAtual >= 0 && PosicaoAtual < lista.Count)
                lista.RemoveAt(PosicaoAtual);
        }

        public bool Existe(string palavra)
        {
            return lista.Exists(d => d.Palavra.Equals(palavra, StringComparison.OrdinalIgnoreCase));
        }

        public int Tamanho => lista.Count;

        public Dicionario GetAtual()
        {
            if (PosicaoAtual >= 0 && PosicaoAtual < lista.Count)
                return lista[PosicaoAtual];
            return null;
        }

        public void Primeiro() => PosicaoAtual = 0;

        public void Ultimo() => PosicaoAtual = lista.Count - 1;

        public void Proximo()
        {
            if (PosicaoAtual < lista.Count - 1) PosicaoAtual++;
        }

        public void Anterior()
        {
            if (PosicaoAtual > 0) PosicaoAtual--;
        }

        public List<Dicionario> GetTodos()
        {
            return new List<Dicionario>(lista);
        }
    }

}
