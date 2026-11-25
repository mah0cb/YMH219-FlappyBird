using KanatKacisi.Models;

namespace KanatKacisi.Services;

/// <summary>Oyunun tüm kurallarını yöneten sınıf; arayüzden bağımsızdır.</summary>
public class OyunMotoru
{
    public const int AlanGenisligi = 480;
    public const int AlanYuksekligi = 600;
    public const int ZeminY = 540;
    private const int OyuncuBaslangicX = 100;

    private Oyuncu _oyuncu = null!;
    private OyunDurumu _durum;

    public OyunMotoru()
    {
        Sifirla();
    }

    public Oyuncu Oyuncu => _oyuncu;
    public OyunDurumu Durum => _durum;

    /// <summary>Oyunu başlangıç durumuna döndürür.</summary>
    public void Sifirla()
    {
        _oyuncu = new Oyuncu(OyuncuBaslangicX, (ZeminY - Oyuncu.Yukseklik) / 2f);
        _durum = OyunDurumu.Hazir;
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
    }
}
