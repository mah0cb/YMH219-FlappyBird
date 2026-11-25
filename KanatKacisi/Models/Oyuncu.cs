namespace KanatKacisi.Models;

/// <summary>Oyuncu karakteri Pırpır: zıplar ve yer çekimiyle aşağı düşer.</summary>
public class Oyuncu
{
    public const int Genislik = 34;
    public const int Yukseklik = 28;
    private const float YerCekimi = 0.5f;
    private const float ZiplamaHizi = -8f;
    private const float MaksimumDusmeHizi = 10f;

    private readonly int _x;
    private float _y;
    private float _dikeyHiz;

    public Oyuncu(int x, float y)
    {
        _x = x;
        _y = y;
    }

    public int X => _x;
    public float Y => _y;

    /// <summary>Negatif değer yukarı, pozitif değer aşağı hareket demektir.</summary>
    public float DikeyHiz => _dikeyHiz;

    /// <summary>Çarpışma kontrolünde kullanılan dikdörtgen.</summary>
    public Rectangle Sinirlar => new(_x, (int)_y, Genislik, Yukseklik);

    /// <summary>Dikey hızı yukarı yönde sabit bir değere ayarlar.</summary>
    public void Zipla() => _dikeyHiz = ZiplamaHizi;

    /// <summary>Her karede yer çekimini uygular ve konumu günceller.</summary>
    public void Guncelle()
    {
        _dikeyHiz = Math.Min(_dikeyHiz + YerCekimi, MaksimumDusmeHizi);
        _y += _dikeyHiz;
    }
}
