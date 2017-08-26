using System.Threading;
using System;

namespace WindowsFormsApplication1
{
    public class Processo
    {
        private FrmVisualizaCoresProc _frmVisualizaCoresProc;
        private bool _bInterrompe = false;
        public int _iContadorProcessos;
        public int _iCoreDoProcesso;
        public int _deadline = 500; // p/ uso no algoritmo LTG / p/ os outros é atribuído um valor inatingível
        public int _tempoTotal = 0;
        public int _tempoRestante = 0;
        public string _intervalo;
        public int _prioridade = 0;
        public string _algoritmo;
        public int _quantum = 0;

        public bool bInterrompe
        {
            get { return _bInterrompe; } set { _bInterrompe = value; }
        }

        public Processo(FrmVisualizaCoresProc frmVisualizaCoresProc, int iContadorProcessos, string sAlgoritmo, ref int iDeadlineMaiorJaCalculado)
        {
            _frmVisualizaCoresProc = frmVisualizaCoresProc;
            _iContadorProcessos = iContadorProcessos;
            _algoritmo = sAlgoritmo;
            _tempoTotal = retornaRandomico2(4, 20);
            _tempoRestante = _tempoTotal;
            _prioridade = retornaRandomico(0, 3);
            if (sAlgoritmo.Contains("LTG"))
            {
                _deadline = 0;
                while (_deadline < iDeadlineMaiorJaCalculado)
                {
                    _deadline = retornaRandomico2(4, 19);
                }
                if (_deadline > 20)
                    _deadline = 20;
                iDeadlineMaiorJaCalculado = _deadline;
            }
        }

        public void Processar()
        {
            string _sNumProcesso = _iContadorProcessos.ToString(); 
            _iCoreDoProcesso = Escalonador.iCoreAtual;

            while (!_bInterrompe)
            {
                _frmVisualizaCoresProc.mostraMsg("Processo " + _sNumProcesso + " rodando...", _iCoreDoProcesso);
                Thread.Sleep(700);
            }
        }

        private int retornaRandomico(int i, int f)
        {
            Random rnd = new Random();
            int intervalo = rnd.Next(i, f);
            return intervalo;
        }
        private int retornaRandomico2(int i, int f)
        {
            Random rnd = new Random();
            int intervalo = rnd.Next(i*1000, f*1000);
            int _intervalo = (intervalo+1000) / 1000;
            return _intervalo;
        }
    }
}