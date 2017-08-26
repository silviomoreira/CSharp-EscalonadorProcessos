using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
/*
 * Tarefas:
 * ===========
 * 
 * Problema: Compo Timer não funciona direito
 * ============================================
 * <OK>Retirar componente Timer e desenvolver classe q simula Timer
 * 
 * <OK> ver randomico p/ tempo de cada processo(demo threads)
 * <OK>Métodos insert/update/delete nas listviews
 * <OK>Atualizações insert/update/delete nas listviews 
 * <OK>Geração da prioridade
 * <OK>Deadline, Tempo restante e (Status) devem ser atualizados a cada segundo. 
 * <OK>Ordenação da fila de aptos por deadline.
 * <OK>Quando o deadline acabar, excluir processo da fila de aptos, se não tiver core livre p/ ele.
 * <OK>Algoritmo1: LTG
 * <OK>Corrigir erro de processos saírem antes do tempo total acabar, qdo. algoritmo <> LTG. 
 *     No RoundRobin tem dado certo, após algumas alterações sem muita significação.
 * <>Algoritmo2: Round Robin(ver quantum -> Novo tempo = tempo restante atual)
 * <>Bt. Adicionar Processo - teste de escalonamento
 * <>Numero de cores(processadores) variável
 * 
 */
namespace WindowsFormsApplication1
{
    public class Escalonador
    {
        private Thread[] aLista = new Thread[4];               // vetor
        private Processo[] aProcessos = new Processo[100];     // vetor
        private Thread[] aTimer = new Thread[100];             // vetor
        private List<Thread> fila = new List<Thread>();        // lista
        private List<int> filaIdsProcessos = new List<int>();  // lista
        private List<int> tempoDoStart = new List<int>();
        private int iTamanhoFila;
        private int iQuantum;
        private string sAlgoritmo;
        private FrmVisualizaCoresProc frm;
        public static int iCoreAtual;
        public static string sNumProcesso;
        public static string sNumProcessoAnt;
        public int iContadorProcessos = 0;
        private Thread processoThread;
        private Thread threadTrocaProcessos;
        private Thread threadAtualizaTela;
        private Thread threadTimer;
        private int iNumCoresUtilizados;
        private int iDeadlineMaiorJaCalculado;

        public Escalonador(int _tamanhoFila, int _quantum, string _algoritmo)
        {
            iTamanhoFila = _tamanhoFila;
            iNumCoresUtilizados = iTamanhoFila;
            iQuantum = _quantum*1000; // transforma em milisegundos
            sAlgoritmo = _algoritmo;
            frm = new FrmVisualizaCoresProc();
            frm.Visible = true;
            iniciaFila();
        }
        public void iniciaFila()
        {
            iDeadlineMaiorJaCalculado = 4;
            // inicia fila de processos
            for (int i=0; i < iTamanhoFila; i++)
            {
                adicionaProcessoNaFila(i);
            }
            threadTrocaProcessos = new Thread(new ThreadStart(substituiThreadNoCoreDoProcessador));
            threadTrocaProcessos.IsBackground = true; 
            //
            threadAtualizaTela = new Thread(new ThreadStart(updateCompletaListViews));
            threadAtualizaTela.IsBackground = true;
        }

        public void adicionaProcessoNaFila(int i)
        {
            ++iContadorProcessos;
            aProcessos[i] = new Processo(frm, iContadorProcessos, sAlgoritmo, ref iDeadlineMaiorJaCalculado);
            aProcessos[i]._intervalo = calculaIntervalo(aProcessos[i]._tempoTotal);
            // Thread q rege o processo
            processoThread = new Thread(new ThreadStart(aProcessos[i].Processar));
            processoThread.IsBackground = true;
            processoThread.Name = iContadorProcessos.ToString();
            fila.Add(processoThread); filaIdsProcessos.Add(iContadorProcessos); // adiciona processo à fila 
            frm.incluiNaListViewAptos(iContadorProcessos.ToString(), aProcessos[i]._deadline.ToString(),
                aProcessos[i]._tempoTotal.ToString(), aProcessos[i]._tempoTotal.ToString(), "pronto",
                aProcessos[i]._intervalo, aProcessos[i]._prioridade.ToString());
        }

        public void cargaInicialCoresProcessador()
        {
            // inicia os 4 cores com os 4 primeiros processos(threads)
            for (int i = 0; i < 4; i++)
            {
                if (fila.Count > 0)
                {
                    Thread.Sleep(500);
                    iCoreAtual = i;        // armazena em que Core está o processo iniciado
                    // Tira da fila, coloca na lista e starta processo
                    aLista[i] = fila.ElementAt(0);
                    try
                    {
                        sNumProcesso = aLista[i].Name;
                        // inicia Thread aLista[i]
                        aLista[i].Start(); 
                        // Inicializa Thread Timer
                        Temporizador temporizador = new Temporizador(aLista[i], aProcessos[i], frm);
                        threadTimer = new Thread(new ThreadStart(temporizador.Start));
                        threadTimer.IsBackground = true;
                        aTimer[i] = threadTimer;
                        aTimer[i].Start();
                        // Atualiza tela
                        frm.incluiNaListViewCores(sNumProcesso, i.ToString(), temporizador._intervaloSeg.ToString(), 
                            temporizador._intervaloSeg.ToString(), "executando", aProcessos[i]._prioridade.ToString());
                    }
                    finally
                    {
                        fila.RemoveAt(0);  filaIdsProcessos.RemoveAt(0); // remove processo da fila
                        // Atualiza tela
                        frm.deletaNaListViewAptos(sNumProcesso);
                    }
                }
            }
            threadTrocaProcessos.Start();
            threadAtualizaTela.Start();
        }

        public void substituiThreadNoCoreDoProcessador()
        {
            while (existeAlgumCoreRodando())
            { 
                while (fila.Count > 0)
                {
                    bool bConcluiu = false;
                    int i = -1;
                    // verifica se concluiu quantum do processo p/ colocar outro no core
                    for (int k = 0; k < 4; k++)
                    {
                        if (aLista[k].ThreadState == ThreadState.Aborted)
                        {
                            frm.mostraMsg("Processo: " + aLista[k].Name + " semi-concluído.", k); Application.DoEvents();
                            bConcluiu = true;
                            i = k;
                            break;
                        }
                    }
                    if (bConcluiu && fila.Count > 0)
                    {
                        iCoreAtual = i; // armazena em que Core está o processo iniciado
                        sNumProcessoAnt = aLista[i].Name;
                        // Tira da fila, coloca na lista e starta Thread c/ Processo, como tbém o Timer
                        aLista[i] = null; 
                        aLista[i] = fila.ElementAt(0);
                        try
                        {
                            int iNumProcesso = filaIdsProcessos.ElementAt(0);
                            sNumProcesso = aLista[i].Name;
                            // inicia Thread aLista[i]
                            aLista[i].Start(); 
                            // Inicializa Thread Timer
                            Temporizador temporizador = new Temporizador(aLista[i], aProcessos[iNumProcesso-1], frm);
                            threadTimer = new Thread(new ThreadStart(temporizador.Start));
                            threadTimer.IsBackground = true;
                            aTimer[iNumProcesso-1] = threadTimer;
                            aTimer[iNumProcesso-1].Start();
                            frm.updateNaListViewCores(sNumProcessoAnt, sNumProcesso, i.ToString(), temporizador._intervaloSeg.ToString(),
                                temporizador._intervaloSeg.ToString(), "executando", aProcessos[iNumProcesso - 1]._prioridade.ToString());
                        }
                        finally
                        {
                            if (fila.Count > 0)
                            {
                                fila.RemoveAt(0); filaIdsProcessos.RemoveAt(0); // remove processo da fila
                                frm.deletaNaListViewAptos(sNumProcesso);
                            }
                        }
                    }
                } // Fim <while (fila.Count > 0)>
                if (fila.Count == 0)
                {
                    int i = 0;
                    while (true)
                    {
                        // mostra msg concluído após terminar últimos processos
                        for (int k = 0; k < iNumCoresUtilizados; k++)
                        {
                            if (aLista[k] != null && aLista[k].ThreadState == ThreadState.Aborted)
                            {
                                Thread.Sleep(200);
                                i++;
                                frm.mostraMsg("1)Processo: " + aLista[k].Name + " concluído.", k); Application.DoEvents();
                                frm.deletaNaListViewCores(aLista[k].Name);
                            }
                        }
                        if (i >= iNumCoresUtilizados) // correto
                            { break; }
                    }
                } // Fim <if (fila.Count == 0)>
            } // Fim <while (existeAlgumCoreRodando())>
            stopCores();
            threadAtualizaTela.Abort();
            frm.mostraMsg("Fila vazia. Processos concluídos.");
        }

        public void updateCompletaListViews()
        {
            int iNumProcesso = 0;
            while (true)
            {
                for (int i = 0; i < iNumCoresUtilizados; i++)
                {
                    if (aLista[i] != null && aLista[i].ThreadState != ThreadState.Aborted)
                    {
                        iNumProcesso = int.Parse(aLista[i].Name);
                        frm.updateNaListViewCores(aLista[i].Name, i.ToString(),
                            aProcessos[iNumProcesso - 1]._tempoTotal.ToString(),
                            aProcessos[iNumProcesso - 1]._tempoRestante.ToString(), "executando",
                            aProcessos[iNumProcesso - 1]._prioridade.ToString());
                    }
                }
                for (int i = 0; i < fila.Count; i++)
                {
                    iNumProcesso = int.Parse(fila[i].Name);
                    if (aProcessos[iNumProcesso - 1]._deadline > 0)
                    {
                        aProcessos[iNumProcesso - 1]._deadline--;
                        frm.updateNaListViewAptos(iNumProcesso.ToString(),
                            aProcessos[iNumProcesso - 1]._deadline.ToString(),
                            aProcessos[iNumProcesso - 1]._tempoTotal.ToString(),
                            aProcessos[iNumProcesso - 1]._tempoRestante.ToString(), "executando",
                            aProcessos[iNumProcesso - 1]._intervalo,
                            aProcessos[iNumProcesso - 1]._prioridade.ToString());
                    }
                    else
                    {
                        // Se deadline zerar não executa processo
                        fila.RemoveAt(i); filaIdsProcessos.RemoveAt(i); // remove processo da fila
                        frm.deletaNaListViewAptos(iNumProcesso.ToString());
                    }
                }
                Thread.Sleep(1000);
            } // Fim <while (true)>
        }

        public bool existeAlgumCoreRodando()
        {
            int iCoresAbortados = 0;
            for (int i = 0; i < iNumCoresUtilizados; i++)
            {
                if (aLista[i] != null && aLista[i].ThreadState == ThreadState.Aborted)
                    iCoresAbortados++;
            }
            return (iCoresAbortados < iNumCoresUtilizados);
        }

        public void stopCores()
        {
            for (int i = 0; i < iNumCoresUtilizados; i++)
            {
                if (aLista[i] != null)
                {
                    aLista[i].Abort();
                    frm.mostraMsg("2)Processo: " + aLista[i].Name + " concluído.", i); Application.DoEvents();
                    frm.deletaNaListViewCores(aLista[i].Name);
                }
            }
           
        }

        public string calculaIntervalo(int iTempoTotal)
        {
            // "12:40:00 - 12:40:15"  |  "12:40 - 12:40"
            string sHoraInicial = DateTime.Now.ToLongTimeString().ToString();
            string sHoraFinal = "";
            bool bMinFinalNulo = (sHoraInicial.Substring(3, 2) == "00");
            int iMinSegFinal = int.Parse(sHoraInicial.Substring(3, 2) + sHoraInicial.Substring(6, 2))+iTempoTotal;
            string sMinSegFinal = iMinSegFinal.ToString();
            int iMinFinal = 0;
            int iSegFinal = 0;
            if (bMinFinalNulo)
            {
                if (sMinSegFinal.Length == 2)
                {
                    iMinFinal = 0;
                    iSegFinal = int.Parse(sMinSegFinal.Substring(0, 2));
                }
                else
                {
                    throw new NotImplementedException("implementar para 00 minutos.");
                }
            }
            else
            {
                if (sMinSegFinal.Length == 3)
                {
                    iMinFinal = int.Parse(sMinSegFinal.Substring(0, 1));
                    iSegFinal = int.Parse(sMinSegFinal.Substring(1, 2));
                }
                else if (sMinSegFinal.Length == 4)
                {
                    iMinFinal = int.Parse(sMinSegFinal.Substring(0, 2));
                    iSegFinal = int.Parse(sMinSegFinal.Substring(2, 2));
                }
                else
                {
                    throw  new NotImplementedException("implementar para 00 horas.");
                }
            }


            if (iSegFinal < 60) // verifica incremento de minuto
            {
                sHoraFinal = sHoraInicial.Substring(0, 2) + ":" + strZero(iMinFinal,2);
            } 
            else
            {
                int iMinutos = iMinFinal+1;
                sHoraFinal = sHoraInicial.Substring(0, 2)+":"+strZero(iMinutos,2);
                if (iMinutos == 60)
                {
                    throw new NotImplementedException("implementar virada de hora.");
                }
            }
            string sIntervalo = sHoraInicial.Substring(0, 5) + " - " + sHoraFinal;
            return sIntervalo;
        }

        private string strZero(int iValor, int iCasas)
        {
            string sRetorno = "";
            sRetorno = replicate("0", iCasas - iValor.ToString().Length) + iValor.ToString();
            return sRetorno;
        }
        private string replicate(string sValor, int iCasas)
        {
            string sRetorno = "";
            for (int i = 1; i <= iCasas; i++)
                sRetorno = sRetorno + sValor;
            return sRetorno;
        }
        
        public class Temporizador
        {
            private Thread thr;
            private Processo pro;
            private FrmVisualizaCoresProc frm;
            private string sNProcesso = Escalonador.sNumProcesso;
            public int _intervaloSeg;

            public Temporizador(Thread processoThread, Processo processo, FrmVisualizaCoresProc frmVisualiza)
            {
                thr = processoThread;
                pro = processo;
                frm = frmVisualiza;
                _intervaloSeg = pro._tempoTotal;
            }

            public void Start()
            {
                // Configura o intervalo de tempo 
                string sHoraInicial = DateTime.Now.ToLongTimeString().ToString();
                int iHoraInicial = int.Parse(sHoraInicial.Substring(3,2)+sHoraInicial.Substring(6,2));
                int iSegInicial = int.Parse(sHoraInicial.Substring(6, 2));
                while (true)
                {
                    string sHoraAtual = DateTime.Now.ToLongTimeString().ToString(); 
                    int iHoraAtual = int.Parse(sHoraAtual.Substring(3,2)+sHoraAtual.Substring(6,2));
                    int iSegAtual = int.Parse(sHoraAtual.Substring(6, 2));
                    pro._tempoRestante = _intervaloSeg - (iSegAtual - iSegInicial);
                    if (iHoraAtual > (iHoraInicial + _intervaloSeg))
                    {
                        Stop();
                        break;
                    }
                }

                // Processa todos os eventos na fila.
                Application.DoEvents();
            }

            private void Stop()
            {
                thr.Abort();
            }

        } // Fim <public class Temporizador>

    } // Fim <public class Escalonador>

}
