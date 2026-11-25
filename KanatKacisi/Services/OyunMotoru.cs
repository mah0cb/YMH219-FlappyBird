using KanatKacisi.Models;

namespace KanatKacisi.Services;

/// <summary>Oyunun tüm kurallarını yöneten sınıf; arayüzden bağımsızdır.</summary>
public class OyunMotoru
{
    public const int AlanGenisligi = 480;
    public const int AlanYuksekligi = 600;
    public const int ZeminY = 540;
    private const int OyuncuBaslangicX = 100;
    private const int EngelSayisi = 3;
    private const int EngelAraligi = 220;
    private const int BoslukKenarPayi = 50;
    private const int BoslukBoyutu = 150;
    private const float BaslangicHizi = 3f;

    private readonly Random _rastgele;
    private readonly List<Engel> _engeller = new();
    private Oyuncu _oyuncu = null!;
    private OyunDurumu _durum;
    private float _hiz;

    /// <param name="rastgele">Test için sabit tohumlu Random verilebilir.</param>
    public OyunMotoru(Random? rastgele = null)
    {
        _rastgele = rastgele ?? new Random();
        Sifirla();
    }

    /// <summary>Oyuncu bir yere çarpıp oyun bittiğinde tetiklenir.</summary>
    public event EventHandler? OyunBitti;

    public Oyuncu Oyuncu => _oyuncu;
    public IReadOnlyList<Engel> Engeller => _engeller;
    public OyunDurumu Durum => _durum;

    /// <summary>Engellerin kare başına sola kayma miktarı (piksel).</summary>
    public float Hiz => _hiz;

    /// <summary>Oyunu başlangıç durumuna döndürür.</summary>
    public void Sifirla()
    {
        _oyuncu = new Oyuncu(OyuncuBaslangicX, (ZeminY - Oyuncu.Yukseklik) / 2f);
        _hiz = BaslangicHizi;
        _durum = OyunDurumu.Hazir;

        _engeller.Clear();
        for (int i = 0; i < EngelSayisi; i++)
            _engeller.Add(new Engel(AlanGenisligi + 100 + i * EngelAraligi, RastgeleBoslukY(), BoslukBoyutu, ZeminY));
    }

    /// <summary>Hazır durumdaysa oyunu başlatır, oyun sürüyorsa karakteri zıplatır.</summary>
    public void ZiplaVeyaBaslat()
    {
        if (_durum == OyunDurumu.Bitti)
            return;

        _durum = OyunDurumu.Oynaniyor;
        _oyuncu.Zipla();
    }

    /// <summary>Oyun döngüsünün bir karesi.</summary>
    public void Guncelle()
    {
        if (_durum != OyunDurumu.Oynaniyor)
            return;

        _oyuncu.Guncelle();
        EngelleriGuncelle();

        if (CarpismaVarMi())
            Bitir();
    }

    /// <summary>Oyuncu tavana, zemine veya bir engele değdiyse true döner.</summary>
    public bool CarpismaVarMi()
    {
        var sinirlar = _oyuncu.Sinirlar;
        if (sinirlar.Top < 0 || sinirlar.Bottom >= ZeminY)
            return true;

        return _engeller.Any(e => sinirlar.IntersectsWith(e.UstSinirlar) || sinirlar.IntersectsWith(e.AltSinirlar));
    }

    private void Bitir()
    {
        _durum = OyunDurumu.Bitti;
        OyunBitti?.Invoke(this, EventArgs.Empty);
    }

    private void EngelleriGuncelle()
    {
        foreach (var engel in _engeller)
            engel.Hareket(_hiz);

        foreach (var engel in _engeller)
        {
            // Ekrandan çıkan engel en sağdaki engelin arkasına taşınır.
            if (engel.EkrandanCiktiMi)
                engel.YenidenKonumla(_engeller.Max(e => e.X) + EngelAraligi, RastgeleBoslukY());
        }
    }

    private int RastgeleBoslukY() =>
        _rastgele.Next(BoslukKenarPayi, ZeminY - BoslukKenarPayi - BoslukBoyutu + 1);
}
