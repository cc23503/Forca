using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Forca21
{
    public partial class Form1: Form
    {
        VetorDicionario vetDic = new VetorDicionario();
        int pos = 0;
        string caminhoArquivo = "dicionario.txt";

        public Form1()
        {
            InitializeComponent();
        }
        private void MostrarPalavra()
        {
            if (vetDic.Tamanho == 0) return;

            Dicionario atual = vetDic.GetTodos()[pos];
            txtPalavra.Text = atual.Palavra;
            txtDica.Text = atual.Dica;
        }

        private void AtualizarGrid()
        {
            dgvDicionario.Rows.Clear();
            foreach (Dicionario d in vetDic.GetTodos())
            {
                dgvDicionario.Rows.Add(d.Palavra, d.Dica);
            }
        }

        private void btnInicio_Click(object sender, EventArgs e)
        {
            pos = 0;
            MostrarPalavra();
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            if (pos > 0)
            {
                pos--;
                MostrarPalavra();
            }
        }

        private void btnProximo_Click(object sender, EventArgs e)
        {
            if (pos < vetDic.Tamanho - 1)
            {
                pos++;
                MostrarPalavra();
            }
        }

        private void btnUltimo_Click(object sender, EventArgs e)
        {
            pos = vetDic.Tamanho - 1;
            MostrarPalavra();
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            string palavra = txtPalavra.Text.Trim();
            int indice = vetDic.GetTodos().FindIndex(d => d.Palavra.Equals(palavra, StringComparison.OrdinalIgnoreCase));

            if (indice >= 0)
            {
                pos = indice;
                MostrarPalavra();
            }
            else
            {
                MessageBox.Show("Palavra não encontrada.");
            }
        }

        private void btnIncluir_Click(object sender, EventArgs e)
        {
            string palavra = txtPalavra.Text.Trim();
            string dica = txtDica.Text.Trim();

            if (string.IsNullOrEmpty(palavra))
            {
                MessageBox.Show("Informe a palavra.");
                return;
            }

            if (vetDic.Existe(palavra))
            {
                MessageBox.Show("Palavra já existente.");
                return;
            }

            Dicionario d = new Dicionario
            {
                Palavra = palavra,
                Dica = dica
            };

            vetDic.AdicionarOrdenado(d);
            pos = vetDic.GetTodos().FindIndex(x => x.Palavra == palavra);
            MostrarPalavra();
            AtualizarGrid();
            SalvarArquivo();

        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (vetDic.Tamanho == 0) return;

            string novaPalavra = txtPalavra.Text.Trim();
            string novaDica = txtDica.Text.Trim();

            if (string.IsNullOrEmpty(novaPalavra))
            {
                MessageBox.Show("Informe a nova palavra.");
                return;
            }

            vetDic.GetTodos()[pos].Palavra = novaPalavra;
            vetDic.GetTodos()[pos].Dica = novaDica;

            //manter ordem alfabética
            List<Dicionario> temp = vetDic.GetTodos();
            temp.Sort((a, b) => a.Palavra.CompareTo(b.Palavra));
            vetDic = new VetorDicionario();
            foreach (var item in temp)
                vetDic.AdicionarOrdenado(item);

            pos = vetDic.GetTodos().FindIndex(d => d.Palavra == novaPalavra);
            MostrarPalavra();
            AtualizarGrid();
            SalvarArquivo();

        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (vetDic.Tamanho == 0) return;

            vetDic.ExcluirAtual();

            if (pos >= vetDic.Tamanho) pos = vetDic.Tamanho - 1;
            MostrarPalavra();
            AtualizarGrid();
            SalvarArquivo();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void SalvarArquivo()
        {
            using (StreamWriter sw = new StreamWriter(caminhoArquivo, false, Encoding.UTF8))
            {
                foreach (var d in vetDic.GetTodos())
                {
                    sw.WriteLine(d.FormatarParaArquivo());
                }
            }
        }

        private void txtPalavra_TextChanged(object sender, EventArgs e)
        {
            //ignorar
        }

        private void txtDica_TextChanged(object sender, EventArgs e)
        {
            //ignorar
        }

        private void dgvDicionario_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //ignorar
        }

        private void tabCadastro_Click(object sender, EventArgs e)
        {
            //ignorar
        }

        // Criando Teclado Dinâmicamente
        private void CriarTeclado()
        {
            int startX = 10;
            int startY = 10;
            int largura = 40;
            int altura = 40;
            int margem = 5;
            int letrasPorLinha = 10;

            string alfabeto = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            for (int i = 0; i < alfabeto.Length; i++)
            {
                Button btn = new Button();
                btn.Text = alfabeto[i].ToString();
                btn.Width = largura;
                btn.Height = altura;
                btn.Left = startX + (i % letrasPorLinha) * (largura + margem);
                btn.Top = startY + (i / letrasPorLinha) * (altura + margem);
                btn.Click += BotaoLetra_Click;

                tecladoPanel.Controls.Add(btn); // Adiciona ao painel!
            }
        }

        private void BotaoLetra_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                string letra = btn.Text;
                MessageBox.Show($"Você clicou na letra: {letra}");
                btn.Enabled = false; // desativa a letra após clique, como em jogos de forca
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(caminhoArquivo))
            {
                StreamReader sr = new StreamReader(caminhoArquivo, Encoding.UTF8);
                while (!sr.EndOfStream)
                {
                    string linha = sr.ReadLine();
                    Dicionario d = new Dicionario();
                    d.LerLinha(linha);
                    vetDic.AdicionarOrdenado(d);
                }
                sr.Close();

                if (vetDic.Tamanho > 0)
                {
                    MostrarPalavra();
                }
            }

            AtualizarGrid();

            CriarTeclado();

        }
    }
}
