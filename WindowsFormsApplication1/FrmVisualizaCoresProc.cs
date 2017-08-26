using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;
using System.Windows.Forms.VisualStyles;

namespace WindowsFormsApplication1
{
    public partial class FrmVisualizaCoresProc : Form
    {
        public FrmVisualizaCoresProc()
        {
            InitializeComponent();
            inicializaListViewCores();
            inicializaListViewAptos();
        }

        delegate void SetControlValueCallback(Control oControl, string propName, object propValue);
        private void SetControlPropertyValue(Control oControl, string propName, object propValue)
        {
            if (oControl.InvokeRequired)
            {
                SetControlValueCallback d = new SetControlValueCallback(SetControlPropertyValue);
                oControl.Invoke(d, new object[] { oControl, propName, propValue });
            }
            else
            {
                Type t = oControl.GetType();
                PropertyInfo[] props = t.GetProperties();
                foreach (PropertyInfo p in props)
                {
                    if (p.Name.ToUpper() == propName.ToUpper())
                    {
                        p.SetValue(oControl, propValue, null); Application.DoEvents();
                    }
                }
            }
        }

        delegate string GetListViewValueCallBack(Control oControl, int posItem);
        private string GetListViewPropertyValue(Control oControl, int posItem)
        {
            string sRetorno = "";
            if (oControl.InvokeRequired)
            {
                GetListViewValueCallBack d = new GetListViewValueCallBack(GetListViewPropertyValue);
                object returnedCallBackObject = oControl.Invoke(d, new object[] { oControl, posItem });
                sRetorno = (string) returnedCallBackObject;
            }
            else
            {
                sRetorno = (string)((ListView)oControl).Items[posItem].Text; Application.DoEvents();
            }
            return sRetorno;
        }

        private delegate void DeleteListViewValueCallBack(Control oControl, int posItem);
        private void DeleteListViewPropertyValue(Control oControl, int posItem)
        {
            if (oControl.InvokeRequired)
            {
                DeleteListViewValueCallBack d = new DeleteListViewValueCallBack(DeleteListViewPropertyValue);
                oControl.Invoke(d, new object[] { oControl, posItem });
            }
            else
            {
                ((ListView)oControl).Items.RemoveAt(posItem); Application.DoEvents(); 
            }
        }

        private delegate void AddListViewValueCallBack(Control oControl, ListViewItem lvItem);
        private void AddListViewPropertyValue(Control oControl, ListViewItem lvItem)
        {
            if (oControl.InvokeRequired)
            {
                AddListViewValueCallBack d = new AddListViewValueCallBack(AddListViewPropertyValue);
                oControl.Invoke(d, new object[] { oControl, lvItem });
            }
            else
            {
                ((ListView)oControl).Items.Add(lvItem); Application.DoEvents();
            }
        }

        private delegate void InsertListViewValueCallBack(Control oControl, ListViewItem lvItem, int posItem);
        private void InsertListViewPropertyValue(Control oControl, ListViewItem lvItem, int posItem)
        {
            if (oControl.InvokeRequired)
            {
                InsertListViewValueCallBack d = new InsertListViewValueCallBack(InsertListViewPropertyValue);
                oControl.Invoke(d, new object[] { oControl, lvItem, posItem });
            }
            else
            {
                ((ListView)oControl).Items.Insert(posItem, lvItem); Application.DoEvents(); 
            }
        }

        public void mostraMsg(string pMsg,int iCore)
        {
            return;
            TextBox textBox = textBox0;
            switch (iCore)
            {
                case 0:
                    {
                        textBox = textBox0;
                        break;
                    }
                case 1:
                    {
                        textBox = textBox1;
                        break;
                    }
                case 2:
                    {
                        textBox = textBox2;
                        break;
                    }
                case 3:
                    {
                        textBox = textBox3;
                        break;
                    }
            }
            //
            if (iCore<4)
                SetControlPropertyValue(textBox, "Text", pMsg);
        }

        public void mostraMsg(string pMsg)
        {
            SetControlPropertyValue(lblStatus, "Text", pMsg);
        }

        public void inicializaListViewCores()
        {
            ColumnHeader header1 = this.lvwCores.Columns.Add("idProcesso", 9 * Convert.ToInt32(lvwCores.Font.SizeInPoints), HorizontalAlignment.Center);
            ColumnHeader header2 = this.lvwCores.Columns.Add("core", 7 * Convert.ToInt32(lvwCores.Font.SizeInPoints), HorizontalAlignment.Center);
            ColumnHeader header3 = this.lvwCores.Columns.Add("tempo total(s)", 10 * Convert.ToInt32(lvwCores.Font.SizeInPoints), HorizontalAlignment.Center);
            ColumnHeader header4 = this.lvwCores.Columns.Add("tempo restante(s)", 12 * Convert.ToInt32(lvwCores.Font.SizeInPoints), HorizontalAlignment.Center);
            ColumnHeader header5 = this.lvwCores.Columns.Add("status", 13 * Convert.ToInt32(lvwCores.Font.SizeInPoints), HorizontalAlignment.Center);
            ColumnHeader header6 = this.lvwCores.Columns.Add("prior", 6 * Convert.ToInt32(lvwCores.Font.SizeInPoints), HorizontalAlignment.Center);
        }
        public void inicializaListViewAptos()
        {
            ColumnHeader header1 = this.lvwAptos.Columns.Add("idProcesso", 8 * Convert.ToInt32(lvwAptos.Font.SizeInPoints), HorizontalAlignment.Center);
            ColumnHeader header2 = this.lvwAptos.Columns.Add("deadline", 9 * Convert.ToInt32(lvwAptos.Font.SizeInPoints), HorizontalAlignment.Center);
            ColumnHeader header3 = this.lvwAptos.Columns.Add("tempo total(s)", 10 * Convert.ToInt32(lvwAptos.Font.SizeInPoints), HorizontalAlignment.Center);
            ColumnHeader header4 = this.lvwAptos.Columns.Add("tempo restante(s)", 12 * Convert.ToInt32(lvwAptos.Font.SizeInPoints), HorizontalAlignment.Center);
            ColumnHeader header5 = this.lvwAptos.Columns.Add("status", 12 * Convert.ToInt32(lvwAptos.Font.SizeInPoints), HorizontalAlignment.Center);
            ColumnHeader header6 = this.lvwAptos.Columns.Add("intervalo", 15 * Convert.ToInt32(lvwAptos.Font.SizeInPoints), HorizontalAlignment.Center);
            ColumnHeader header7 = this.lvwAptos.Columns.Add("prior", 6 * Convert.ToInt32(lvwAptos.Font.SizeInPoints), HorizontalAlignment.Center);
        }

        public void incluiNaListViewCores(string idprocesso, string core, string tempoTotal, string tempoRestante, string status, string prioridade)
        {
            // cria os subitens para incluir na lista
            string[] mItems = new string[]
            {
                idprocesso, core, tempoTotal, tempoRestante, status, prioridade
            };
            ListViewItem lvi = new ListViewItem(mItems);

            // inclui todos os itens no listview na ultima linha disponivel
            AddListViewPropertyValue(lvwCores, lvi);
        }
        public void incluiNaListViewAptos(string idprocesso, string deadline, string tempoTotal, string tempoRestante, string status, string intervalo, string prioridade)
        {
            // cria os subitens para incluir na lista
            string[] mItems = new string[]
            {
                idprocesso, deadline, tempoTotal, tempoRestante, status, intervalo, prioridade
            };
            ListViewItem lvi = new ListViewItem(mItems);

            // inclui todos os itens no listview na ultima linha disponivel
            AddListViewPropertyValue(lvwAptos, lvi);
        }
        public void updateNaListViewCores(string idprocessoAnt, string idprocesso, string core, string tempoTotal, string tempoRestante, string status, string prioridade)
        {
            // cria os subitens para incluir na lista, após deletar o registro a ser alterado
            string[] mItems = new string[]
            {
                idprocesso, core, tempoTotal, tempoRestante, status, prioridade
            };
            var iPos = localizaProcessoNaListView(idprocessoAnt, lvwCores);
            if (iPos > -1)
            {
                DeleteListViewPropertyValue(lvwCores, iPos);
                ListViewItem lvi = new ListViewItem(mItems);
                InsertListViewPropertyValue(lvwCores, lvi, iPos);
            }
        }
        public void updateNaListViewCores(string idprocesso, string core, string tempoTotal, string tempoRestante, string status, string prioridade)
        {
            // cria os subitens para incluir na lista, após deletar o registro a ser alterado
            string[] mItems = new string[]
            {
                idprocesso, core, tempoTotal, tempoRestante, status, prioridade
            };
            var iPos = localizaProcessoNaListView(idprocesso, lvwCores);
            if (iPos > -1)
            {
                DeleteListViewPropertyValue(lvwCores, iPos);
                ListViewItem lvi = new ListViewItem(mItems);
                InsertListViewPropertyValue(lvwCores, lvi, iPos);
            }
        }
        public void updateNaListViewAptos(string idprocesso, string deadline, string tempoTotal, string tempoRestante, string status, string intervalo, string prioridade)
        {
            // cria os subitens para incluir na lista, após deletar o registro a ser alterado
            string[] mItems = new string[]
            {
                idprocesso, deadline, tempoTotal, tempoRestante, status, intervalo, prioridade
            };
            var iPos = localizaProcessoNaListView(idprocesso, lvwAptos);
            if (iPos > -1)
            {
                DeleteListViewPropertyValue(lvwAptos, iPos);
                ListViewItem lvi = new ListViewItem(mItems);
                InsertListViewPropertyValue(lvwAptos, lvi, iPos);
            }
        }
        public void deletaNaListViewCores(string idprocesso)
        {
            var iPos = localizaProcessoNaListView(idprocesso, lvwCores);
            if (iPos > -1)
                DeleteListViewPropertyValue(lvwCores, iPos);
        }
        public void deletaNaListViewAptos(string idprocesso)
        {
            var iPos = localizaProcessoNaListView(idprocesso, lvwAptos);
            if (iPos > -1)
                DeleteListViewPropertyValue(lvwAptos, iPos);
        }
        public void deletaNaListView(string idprocesso, ListView listview)
        {
            var iPos = localizaProcessoNaListView(idprocesso, listview);
            listview.Items.RemoveAt(iPos);
        }

        private int localizaProcessoNaListView(string idprocesso, ListView listview)
        {
            int iPos = -1;
            string item = "";
            for (int i = 0; i < listview.Items.Count; i++)
            {
                item = GetListViewPropertyValue(listview, i); 
                if (item == idprocesso)
                {
                    iPos = i;
                    break;
                }
            }
            return iPos;
        }


        public string retornaNumProcessoTopoFila()
        {
            string idprocesso = "0";
            foreach (ListViewItem lvw in lvwAptos.Items)
            {
                idprocesso = lvw.Text;
                break;
            }
            return idprocesso;
        }

    } // Fim <public partial class FrmVisualizaCoresProc>

}
