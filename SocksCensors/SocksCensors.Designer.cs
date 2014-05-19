namespace NSSocksCensors
{
    partial class SocksCensors
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbxLocalPort = new System.Windows.Forms.TextBox();
            this.tbxRemoteHost = new System.Windows.Forms.TextBox();
            this.tbxRemotePort = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslDataReceived = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslDataSent = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnOnOff = new System.Windows.Forms.Button();
            this.stateObjectDisposeTimer = new System.Windows.Forms.Timer(this.components);
            this.cbxSsl = new System.Windows.Forms.CheckBox();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Local Port:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Remote Host:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Remote Port:";
            // 
            // tbxLocalPort
            // 
            this.tbxLocalPort.Location = new System.Drawing.Point(91, 12);
            this.tbxLocalPort.Name = "tbxLocalPort";
            this.tbxLocalPort.Size = new System.Drawing.Size(74, 20);
            this.tbxLocalPort.TabIndex = 0;
            this.tbxLocalPort.Text = "8888";
            // 
            // tbxRemoteHost
            // 
            this.tbxRemoteHost.Location = new System.Drawing.Point(91, 39);
            this.tbxRemoteHost.Name = "tbxRemoteHost";
            this.tbxRemoteHost.Size = new System.Drawing.Size(171, 20);
            this.tbxRemoteHost.TabIndex = 1;
            // 
            // tbxRemotePort
            // 
            this.tbxRemotePort.Location = new System.Drawing.Point(91, 66);
            this.tbxRemotePort.Name = "tbxRemotePort";
            this.tbxRemotePort.Size = new System.Drawing.Size(74, 20);
            this.tbxRemotePort.TabIndex = 2;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.tsslDataReceived,
            this.toolStripStatusLabel3,
            this.tsslDataSent});
            this.statusStrip1.Location = new System.Drawing.Point(0, 100);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(370, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(57, 17);
            this.toolStripStatusLabel1.Text = "Received:";
            // 
            // tsslDataReceived
            // 
            this.tsslDataReceived.Name = "tsslDataReceived";
            this.tsslDataReceived.Size = new System.Drawing.Size(44, 17);
            this.tsslDataReceived.Text = "0 Bytes";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(33, 17);
            this.toolStripStatusLabel3.Text = "Sent:";
            // 
            // tsslDataSent
            // 
            this.tsslDataSent.Name = "tsslDataSent";
            this.tsslDataSent.Size = new System.Drawing.Size(44, 17);
            this.tsslDataSent.Text = "0 Bytes";
            // 
            // btnOnOff
            // 
            this.btnOnOff.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOnOff.Location = new System.Drawing.Point(284, 15);
            this.btnOnOff.Name = "btnOnOff";
            this.btnOnOff.Size = new System.Drawing.Size(74, 74);
            this.btnOnOff.TabIndex = 4;
            this.btnOnOff.Text = "Start";
            this.btnOnOff.UseVisualStyleBackColor = true;
            this.btnOnOff.Click += new System.EventHandler(this.btnOnOff_Click);
            // 
            // stateObjectDisposeTimer
            // 
            this.stateObjectDisposeTimer.Interval = 500;
            this.stateObjectDisposeTimer.Tick += new System.EventHandler(this.stateObjectDisposeTimer_Tick);
            // 
            // cbxSsl
            // 
            this.cbxSsl.AutoSize = true;
            this.cbxSsl.Checked = true;
            this.cbxSsl.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxSsl.Location = new System.Drawing.Point(182, 68);
            this.cbxSsl.Name = "cbxSsl";
            this.cbxSsl.Size = new System.Drawing.Size(46, 17);
            this.cbxSsl.TabIndex = 3;
            this.cbxSsl.Text = "SSL";
            this.cbxSsl.UseVisualStyleBackColor = true;
            this.cbxSsl.CheckedChanged += new System.EventHandler(this.cbxSsl_CheckedChanged);
            // 
            // SocksCensors
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 122);
            this.Controls.Add(this.cbxSsl);
            this.Controls.Add(this.btnOnOff);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tbxRemotePort);
            this.Controls.Add(this.tbxRemoteHost);
            this.Controls.Add(this.tbxLocalPort);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SocksCensors";
            this.Text = "Socks Censors";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbxLocalPort;
        private System.Windows.Forms.TextBox tbxRemoteHost;
        private System.Windows.Forms.TextBox tbxRemotePort;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel tsslDataReceived;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel tsslDataSent;
        private System.Windows.Forms.Button btnOnOff;
        private System.Windows.Forms.Timer stateObjectDisposeTimer;
        private System.Windows.Forms.CheckBox cbxSsl;
    }
}

