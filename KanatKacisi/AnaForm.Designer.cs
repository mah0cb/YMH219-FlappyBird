namespace KanatKacisi;

partial class AnaForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        pnlUst = new Panel();
        lblSeviye = new Label();
        lblEnYuksek = new Label();
        lblSkor = new Label();
        cmbZorluk = new ComboBox();
        btnYeniOyun = new Button();
        pnlOyun = new OyunPaneli();
        stsDurum = new StatusStrip();
        lblDurum = new ToolStripStatusLabel();
        tmrOyunDongusu = new System.Windows.Forms.Timer(components);
        pnlUst.SuspendLayout();
        stsDurum.SuspendLayout();
        SuspendLayout();
        //
        // pnlUst
        //
        pnlUst.Controls.Add(lblSeviye);
        pnlUst.Controls.Add(lblEnYuksek);
        pnlUst.Controls.Add(lblSkor);
        pnlUst.Controls.Add(cmbZorluk);
        pnlUst.Controls.Add(btnYeniOyun);
        pnlUst.Location = new Point(0, 0);
        pnlUst.Name = "pnlUst";
        pnlUst.Size = new Size(480, 40);
        pnlUst.TabIndex = 0;
        //
        // btnYeniOyun
        //
        btnYeniOyun.Location = new Point(6, 7);
        btnYeniOyun.Name = "btnYeniOyun";
        btnYeniOyun.Size = new Size(90, 26);
        btnYeniOyun.TabIndex = 0;
        btnYeniOyun.TabStop = false;
        btnYeniOyun.Text = "Yeni Oyun";
        btnYeniOyun.UseVisualStyleBackColor = true;
        btnYeniOyun.Click += btnYeniOyun_Click;
        //
        // cmbZorluk
        //
        cmbZorluk.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbZorluk.Location = new Point(102, 8);
        cmbZorluk.Name = "cmbZorluk";
        cmbZorluk.Size = new Size(80, 23);
        cmbZorluk.TabIndex = 1;
        cmbZorluk.TabStop = false;
        //
        // lblSkor
        //
        lblSkor.AutoSize = true;
        lblSkor.Location = new Point(192, 12);
        lblSkor.Name = "lblSkor";
        lblSkor.Text = "Skor: 0";
        //
        // lblEnYuksek
        //
        lblEnYuksek.AutoSize = true;
        lblEnYuksek.Location = new Point(262, 12);
        lblEnYuksek.Name = "lblEnYuksek";
        lblEnYuksek.Text = "En Yüksek: 0";
        //
        // lblSeviye
        //
        lblSeviye.AutoSize = true;
        lblSeviye.Location = new Point(362, 12);
        lblSeviye.Name = "lblSeviye";
        lblSeviye.Text = "Seviye: 1";
        //
        // pnlOyun
        //
        pnlOyun.Location = new Point(0, 40);
        pnlOyun.Name = "pnlOyun";
        pnlOyun.Size = new Size(480, 600);
        pnlOyun.TabIndex = 2;
        pnlOyun.Paint += pnlOyun_Paint;
        pnlOyun.PreviewKeyDown += pnlOyun_PreviewKeyDown;
        //
        // stsDurum
        //
        stsDurum.Items.AddRange(new ToolStripItem[] { lblDurum });
        stsDurum.Name = "stsDurum";
        stsDurum.TabIndex = 3;
        //
        // lblDurum
        //
        lblDurum.Name = "lblDurum";
        lblDurum.Text = "Hazır";
        //
        // tmrOyunDongusu
        //
        tmrOyunDongusu.Interval = 16;
        tmrOyunDongusu.Tick += tmrOyunDongusu_Tick;
        //
        // AnaForm
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(480, 662);
        Controls.Add(pnlOyun);
        Controls.Add(pnlUst);
        Controls.Add(stsDurum);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        KeyPreview = true;
        MaximizeBox = false;
        Name = "AnaForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Kanat Kaçışı";
        KeyDown += AnaForm_KeyDown;
        KeyUp += AnaForm_KeyUp;
        pnlUst.ResumeLayout(false);
        pnlUst.PerformLayout();
        stsDurum.ResumeLayout(false);
        stsDurum.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Panel pnlUst;
    private Button btnYeniOyun;
    private ComboBox cmbZorluk;
    private Label lblSkor;
    private Label lblEnYuksek;
    private Label lblSeviye;
    private OyunPaneli pnlOyun;
    private StatusStrip stsDurum;
    private ToolStripStatusLabel lblDurum;
    private System.Windows.Forms.Timer tmrOyunDongusu;
}
