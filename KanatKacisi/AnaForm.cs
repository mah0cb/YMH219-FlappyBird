using System.Drawing.Drawing2D;
using KanatKacisi.Models;
using KanatKacisi.Services;

namespace KanatKacisi;

/// <summary>Oyunun ana penceresi; yalnızca çizim ve olay bağlama işini yapar.</summary>
public partial class AnaForm : Form
{
    private readonly OyunMotoru _motor = new();
    private bool _ziplamaTusuBasili;

    public AnaForm()
    {
        InitializeComponent();
        ActiveControl = pnlOyun;
        _motor.OyunBitti += Motor_OyunBitti;
        tmrOyunDongusu.Start();
    }

    private void Motor_OyunBitti(object? sender, EventArgs e)
    {
        lblDurum.Text = $"Oyun bitti. Skor: {_motor.Skor}. Yeniden başlatmak için R'ye basın.";
    }

    /// <summary>Oyunu sıfırlar ve klavye odağını oyun alanına geri verir.</summary>
    private void YeniOyunBaslat()
    {
        _motor.Sifirla();
        lblDurum.Text = "Hazır. Başlamak için Space / Yukarı ok.";
        pnlOyun.Focus();
    }

    private void tmrOyunDongusu_Tick(object? sender, EventArgs e)
    {
        _motor.Guncelle();
        lblSkor.Text = $"Skor: {_motor.Skor}";
        pnlOyun.Invalidate();
    }

    /// <summary>Yukarı ok tuşunun odak gezintisine gitmeyip KeyDown olayına düşmesini sağlar.</summary>
    private void pnlOyun_PreviewKeyDown(object? sender, PreviewKeyDownEventArgs e)
    {
        if (e.KeyCode == Keys.Up)
            e.IsInputKey = true;
    }

    private void AnaForm_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode is Keys.Space or Keys.Up)
        {
            // Tuş basılı tutulunca tekrar tekrar zıplamasın diye KeyUp gelene kadar yok sayılır.
            if (!_ziplamaTusuBasili)
            {
                if (_motor.Durum == OyunDurumu.Hazir)
                    lblDurum.Text = "Oynanıyor";
                _motor.ZiplaVeyaBaslat();
            }
            _ziplamaTusuBasili = true;
            e.SuppressKeyPress = true;
        }
        else if (e.KeyCode == Keys.R)
        {
            YeniOyunBaslat();
            e.SuppressKeyPress = true;
        }
    }

    private void AnaForm_KeyUp(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode is Keys.Space or Keys.Up)
            _ziplamaTusuBasili = false;
    }

    private void btnYeniOyun_Click(object? sender, EventArgs e) => YeniOyunBaslat();

    private void pnlOyun_Paint(object? sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        // Oyun sabit boyutlu bir alanda hesaplanır, panel boyutuna göre ölçeklenerek çizilir.
        g.ScaleTransform(pnlOyun.ClientSize.Width / (float)OyunMotoru.AlanGenisligi,
                         pnlOyun.ClientSize.Height / (float)OyunMotoru.AlanYuksekligi);

        ArkaPlaniCiz(g);
        foreach (var engel in _motor.Engeller)
            EngeliCiz(g, engel);
        OyuncuyuCiz(g, _motor.Oyuncu);
        DurumMesajiniCiz(g);
    }

    private static void ArkaPlaniCiz(Graphics g)
    {
        var gokyuzu = new Rectangle(0, 0, OyunMotoru.AlanGenisligi, OyunMotoru.ZeminY);
        using (var firca = new LinearGradientBrush(gokyuzu, Color.FromArgb(15, 20, 60), Color.FromArgb(70, 50, 120), LinearGradientMode.Vertical))
            g.FillRectangle(firca, gokyuzu);

        int zeminYuksekligi = OyunMotoru.AlanYuksekligi - OyunMotoru.ZeminY;
        g.FillRectangle(Brushes.SaddleBrown, 0, OyunMotoru.ZeminY, OyunMotoru.AlanGenisligi, zeminYuksekligi);
        g.FillRectangle(Brushes.Peru, 0, OyunMotoru.ZeminY, OyunMotoru.AlanGenisligi, 8);
    }

    private static void EngeliCiz(Graphics g, Engel engel)
    {
        SutunCiz(g, engel.UstSinirlar);
        SutunCiz(g, engel.AltSinirlar);
    }

    /// <summary>Taş sütun: gri gövde ve 20 piksel aralıklı taş derzleri.</summary>
    private static void SutunCiz(Graphics g, Rectangle r)
    {
        g.FillRectangle(Brushes.SlateGray, r);
        using var derz = new Pen(Color.DarkSlateGray, 2);
        for (int y = r.Top + 20; y < r.Bottom; y += 20)
            g.DrawLine(derz, r.Left, y, r.Right, y);
        g.DrawRectangle(derz, r);
    }

    /// <summary>Pırpır:mor yuvarlak gövde, açık mor kanat, büyük göz ve pembe yanak.</summary>
    private static void OyuncuyuCiz(Graphics g, Oyuncu oyuncu)
    {
        var s = oyuncu.Sinirlar;
        g.FillEllipse(Brushes.MediumPurple, s);
        g.DrawEllipse(Pens.Indigo, s);

        // Kanat yukarı hızda kalkık, aşağı hızda iner.
        int kanatY = oyuncu.DikeyHiz < 0 ? s.Y + 2 : s.Y + 12;
        g.FillEllipse(Brushes.Plum, s.X - 6, kanatY, 16, 11);

        g.FillEllipse(Brushes.White, s.X + 18, s.Y + 5, 11, 11);
        g.FillEllipse(Brushes.Black, s.X + 23, s.Y + 8, 5, 5);
        g.FillEllipse(Brushes.HotPink, s.X + 15, s.Y + 17, 7, 5);
    }

    private void DurumMesajiniCiz(Graphics g)
    {
        if (_motor.Durum == OyunDurumu.Hazir)
        {
            OrtaliYaziCiz(g, "Başlamak için Space / Yukarı ok", 16, 200);
        }
        else if (_motor.Durum == OyunDurumu.Bitti)
        {
            using (var karartma = new SolidBrush(Color.FromArgb(140, 0, 0, 0)))
                g.FillRectangle(karartma, 0, 0, OyunMotoru.AlanGenisligi, OyunMotoru.AlanYuksekligi);
            OrtaliYaziCiz(g, "Game Over", 36, 190);
            OrtaliYaziCiz(g, $"Skor: {_motor.Skor}", 20, 250);
            OrtaliYaziCiz(g, "Yeniden başlatmak için R veya Yeni Oyun", 13, 300);
        }
        else
        {
            OrtaliYaziCiz(g, _motor.Skor.ToString(), 32, 30);
        }
    }

    private static void OrtaliYaziCiz(Graphics g, string yazi, float boyut, int y)
    {
        using var yaziTipi = new Font("Segoe UI", boyut, FontStyle.Bold);
        var olcu = g.MeasureString(yazi, yaziTipi);
        float x = (OyunMotoru.AlanGenisligi - olcu.Width) / 2;
        g.DrawString(yazi, yaziTipi, Brushes.Black, x + 2, y + 2);
        g.DrawString(yazi, yaziTipi, Brushes.White, x, y);
    }
}
