using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;

namespace WindowsFormsApplication1
{
    public partial class FrmTelaEntrada : Form
    {
        private Escalonador esc;

        public FrmTelaEntrada()
        {
            InitializeComponent();
            btnIniciar.Enabled = false;
        }

        #region Validacoes
        private void txtNumCores_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtNumCores.Text))
            {
                int iNumCores = int.Parse(txtNumCores.Text);
                if (iNumCores < 1 || iNumCores > 4)
                    MessageBox.Show("Número de processadores inválido !");
            }
        }

        private void txtQuantum_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtQuantum.Text))
            {
                int iQuantum = int.Parse(txtQuantum.Text);
                if (iQuantum < 2 || iQuantum > 20)
                    MessageBox.Show("Quantum inválido !");
            }
        }

        private void txtNumProcessos_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtNumProcessos.Text))
            {
                int iNumProcessos = int.Parse(txtNumProcessos.Text);
                if (iNumProcessos == 0)
                    MessageBox.Show("Número de processos inválido !");
            }
        }

        private void txtNumProcessos_Validated(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(cbxAlgoritmo.Text) && !string.IsNullOrWhiteSpace(txtNumCores.Text) &&
                String.IsNullOrWhiteSpace(txtNumProcessos.Text))
            {
                MessageBox.Show("Digite o número de processos.");
                return;
            }
        }
        #endregion Validacoes

        private void btnPrepara_Click(object sender, EventArgs e)
        {
            esc = new Escalonador(int.Parse(txtNumProcessos.Text), int.Parse(txtQuantum.Text), cbxAlgoritmo.Text);
            btnIniciar.Enabled = true;
            btnPrepara.Enabled = false;
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbxAlgoritmo.Text))
            {
                MessageBox.Show("Selecione o algoritmo.");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtNumCores.Text))
            {
                txtNumCores.Text = "4";
            }
            esc.cargaInicialCoresProcessador();
            btnPrepara.Enabled = true;
            btnIniciar.Enabled = false;
        }

        private void btnAdicionarProcesso_Click(object sender, EventArgs e)
        {
            // inclui processo na fila e atualiza Tela
            esc.adicionaProcessoNaFila(esc.iContadorProcessos-1);
        }

    } // Fim partial class

}
