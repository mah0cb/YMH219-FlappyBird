namespace KanatKacisi.Models;

/// <summary>Ortasında geçiş boşluğu olan, sağdan sola kayan taş sütun çifti.</summary>
public class Engel
{
    public const int Genislik = 60;

    private readonly int _zeminY;
    private float _x;
    private int _boslukY;
    private bool _gecildiMi;

    public Engel(float x, int boslukY, int boslukBoyutu, int zeminY)
    {
        _x = x;
        _boslukY = boslukY;
        BoslukBoyutu = boslukBoyutu;
        _zeminY = zeminY;
    }

    public float X => _x;

    /// <summary>Boşluğun üst kenarının Y konumu.</summary>
    public int BoslukY => _boslukY;
    public int BoslukBoyutu { get; }
    public bool GecildiMi => _gecildiMi;
    public bool EkrandanCiktiMi => _x + Genislik < 0;

    public Rectangle UstSinirlar => new((int)_x, 0, Genislik, _boslukY);
    public Rectangle AltSinirlar => new((int)_x, _boslukY + BoslukBoyutu, Genislik, _zeminY - _boslukY - BoslukBoyutu);

    /// <summary>Oyuncu engeli ilk kez tamamen geçtiğinde true döner; aynı engel için bir daha true dönmez.</summary>
    public bool GecisiKontrolEt(int oyuncuX)
    {
        if (_gecildiMi || _x + Genislik >= oyuncuX)
            return false;

        _gecildiMi = true;
        return true;
    }

    /// <summary>Engeli verilen hız kadar sola kaydırır.</summary>
    public void Hareket(float hiz) => _x -= hiz;

    /// <summary>Ekrandan çıkan engeli yeni konuma ve yeni boşluk yüksekliğine taşır.</summary>
    public void YenidenKonumla(float x, int boslukY)
    {
        _x = x;
        _boslukY = boslukY;
        _gecildiMi = false;
    }
}
