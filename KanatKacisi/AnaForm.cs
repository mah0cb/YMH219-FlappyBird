namespace KanatKacisi;

/// <summary>Oyunun ana penceresi; yalnızca çizim ve olay bağlama işini yapar.</summary>
public partial class AnaForm : Form
{
    public AnaForm()
    {
        InitializeComponent();
    }

    private void tmrOyunDongusu_Tick(object? sender, EventArgs e)
    {
    }

    private void pnlOyun_Paint(object? sender, PaintEventArgs e)
    {
        e.Graphics.Clear(Color.MidnightBlue);
    }

    private void AnaForm_KeyDown(object? sender, KeyEventArgs e)
    {
    }

    private void btnYeniOyun_Click(object? sender, EventArgs e)
    {
    }
}
