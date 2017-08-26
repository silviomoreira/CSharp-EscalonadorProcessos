namespace WindowsFormsApplication1
{
    partial class FrmTelaEntrada
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtNumProcessos = new System.Windows.Forms.TextBox();
            this.btnIniciar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtQuantum = new System.Windows.Forms.TextBox();
            this.btnAdicionarProcesso = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtNumCores = new System.Windows.Forms.TextBox();
            this.cbxAlgoritmo = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnPrepara = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtNumProcessos
            // 
            this.txtNumProcessos.Location = new System.Drawing.Point(82, 113);
            this.txtNumProcessos.Name = "txtNumProcessos";
            this.txtNumProcessos.Size = new System.Drawing.Size(100, 20);
            this.txtNumProcessos.TabIndex = 0;
            this.txtNumProcessos.TextChanged += new System.EventHandler(this.txtNumProcessos_TextChanged);
            this.txtNumProcessos.Validated += new System.EventHandler(this.txtNumProcessos_Validated);
            // 
            // btnIniciar
            // 
            this.btnIniciar.Location = new System.Drawing.Point(82, 230);
            this.btnIniciar.Name = "btnIniciar";
            this.btnIniciar.Size = new System.Drawing.Size(122, 23);
            this.btnIniciar.TabIndex = 1;
            this.btnIniciar.Text = "Iniciar Thread(s)";
            this.btnIniciar.UseVisualStyleBackColor = true;
            this.btnIniciar.Click += new System.EventHandler(this.btnIniciar_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(79, 97);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(170, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Número de processos inicial da fila";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(79, 136);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Quantum";
            // 
            // txtQuantum
            // 
            this.txtQuantum.Location = new System.Drawing.Point(82, 152);
            this.txtQuantum.Name = "txtQuantum";
            this.txtQuantum.Size = new System.Drawing.Size(100, 20);
            this.txtQuantum.TabIndex = 4;
            this.txtQuantum.Text = "5";
            this.txtQuantum.TextChanged += new System.EventHandler(this.txtQuantum_TextChanged);
            // 
            // btnAdicionarProcesso
            // 
            this.btnAdicionarProcesso.Location = new System.Drawing.Point(82, 267);
            this.btnAdicionarProcesso.Name = "btnAdicionarProcesso";
            this.btnAdicionarProcesso.Size = new System.Drawing.Size(122, 23);
            this.btnAdicionarProcesso.TabIndex = 6;
            this.btnAdicionarProcesso.Text = "Adicionar processo";
            this.btnAdicionarProcesso.UseVisualStyleBackColor = true;
            this.btnAdicionarProcesso.Click += new System.EventHandler(this.btnAdicionarProcesso_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(79, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(166, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Número de processadores (cores)";
            // 
            // txtNumCores
            // 
            this.txtNumCores.Location = new System.Drawing.Point(82, 73);
            this.txtNumCores.Name = "txtNumCores";
            this.txtNumCores.Size = new System.Drawing.Size(100, 20);
            this.txtNumCores.TabIndex = 7;
            this.txtNumCores.TextChanged += new System.EventHandler(this.txtNumCores_TextChanged);
            // 
            // cbxAlgoritmo
            // 
            this.cbxAlgoritmo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbxAlgoritmo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbxAlgoritmo.FormattingEnabled = true;
            this.cbxAlgoritmo.Items.AddRange(new object[] {
            "Least Time to Go (LTG)",
            "Round Robin",
            "Interval based Scheduling"});
            this.cbxAlgoritmo.Location = new System.Drawing.Point(82, 27);
            this.cbxAlgoritmo.Name = "cbxAlgoritmo";
            this.cbxAlgoritmo.Size = new System.Drawing.Size(163, 21);
            this.cbxAlgoritmo.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(79, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(140, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Algoritmo de escalonamento";
            // 
            // btnPrepara
            // 
            this.btnPrepara.Location = new System.Drawing.Point(82, 200);
            this.btnPrepara.Name = "btnPrepara";
            this.btnPrepara.Size = new System.Drawing.Size(122, 23);
            this.btnPrepara.TabIndex = 11;
            this.btnPrepara.Text = "Prepara";
            this.btnPrepara.UseVisualStyleBackColor = true;
            this.btnPrepara.Click += new System.EventHandler(this.btnPrepara_Click);
            // 
            // FrmTelaEntrada
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 302);
            this.Controls.Add(this.btnPrepara);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbxAlgoritmo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtNumCores);
            this.Controls.Add(this.btnAdicionarProcesso);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtQuantum);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnIniciar);
            this.Controls.Add(this.txtNumProcessos);
            this.Name = "FrmTelaEntrada";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Escalonador de Processos";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtNumProcessos;
        private System.Windows.Forms.Button btnIniciar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtQuantum;
        private System.Windows.Forms.Button btnAdicionarProcesso;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtNumCores;
        private System.Windows.Forms.ComboBox cbxAlgoritmo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnPrepara;
    }
}