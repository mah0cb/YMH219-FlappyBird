using KanatKacisi.Models;

namespace KanatKacisi.Services;

/// <summary>Oyunun tüm kurallarını yöneten sınıf; arayüzden bağımsızdır.</summary>
public class OyunMotoru
{
    public const int AlanGenisligi = 480;
    public const int AlanYuksekligi = 600;
    public const int ZeminY = 540;
    public const int SeviyeBasinaSkor = 5;
    private const int OyuncuBaslangicX = 100;
    private const int EngelSayisi = 3;
    private const int EngelAraligi = 220;
    private const int BoslukKenarPayi = 50;
    private const int MaksimumBoslukFarki = 180;
    private const float SeviyeBasinaHizArtisi = 0.5f;
    private const float MaksimumHiz = 7f;

    private readonly Random _rastgele;
    private readonly List<Engel> _engeller = new();
    private Oyuncu _oyuncu = null!;
    private OyunDurumu _durum;
    private Zorluk _zorluk;
    private float _hiz;
    private int _skor;
    private int _enYuksekSkor;
    private bool _yeniRekor;

    /// <param name="rastgele">Test için sabit tohumlu Random verilebilir.</param>
    public OyunMotoru(Zorluk zorluk = Zorluk.Normal, Random? rastgele = null)
    {
        _rastgele = rastgele ?? new Random();
        Sifirla(zorluk);
    }

    /// <summary>Oyuncu bir yere çarpıp oyun bittiğinde tetiklenir.</summary>
    public event EventHandler? OyunBitti;

    public Oyuncu Oyuncu => _oyuncu;
    public IReadOnlyList<Engel> Engeller => _engeller;
    public OyunDurumu Durum => _durum;
    public Zorluk Zorluk => _zorluk;

    /// <summary>Engellerin kare başına sola kayma miktarı (piksel).</summary>
    public float Hiz => _hiz;

    /// <summary>Geçilen engel sayısı.</summary>
    public int Skor => _skor;

    /// <summary>Her 5 skorda bir artan seviye; 1'den başlar.</summary>
    public int Seviye => _skor / SeviyeBasinaSkor + 1;

    public int EnYuksekSkor => _enYuksekSkor;

    /// <summary>Son biten oyunda en yüksek skor geçildiyse true.</summary>
    public bool YeniRekor => _yeniRekor;

    /// <summary>Kayıttan okunan en yüksek skoru motora verir.</summary>
    public void EnYuksekSkoruAyarla(int skor) => _enYuksekSkor = Math.Max(0, skor);

    /// <summary>Oyunu seçilen zorlukla başlangıç durumuna döndürür.</summary>
    public void Sifirla(Zorluk zorluk)
    {
        _zorluk = zorluk;
        _oyuncu = new Oyuncu(OyuncuBaslangicX, (ZeminY - Oyuncu.Yukseklik) / 2f);
        _skor = 0;
        _hiz = BaslangicHizi();
        _yeniRekor = false;
        _durum = OyunDurumu.Hazir;

        _engeller.Clear();
        int? oncekiBoslukY = null;
        for (int i = 0; i < EngelSayisi; i++)
        {
            var engel = new Engel(AlanGenisligi + 100 + i * EngelAraligi, RastgeleBoslukY(oncekiBoslukY), BoslukBoyutu(), ZeminY);
            _engeller.Add(engel);
            oncekiBoslukY = engel.BoslukY;
        }
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
        if (_skor > _enYuksekSkor)
        {
            _enYuksekSkor = _skor;
            _yeniRekor = true;
        }
        OyunBitti?.Invoke(this, EventArgs.Empty);
    }

    private void EngelleriGuncelle()
    {
        foreach (var engel in _engeller)
        {
            engel.Hareket(_hiz);
            if (engel.GecisiKontrolEt(_oyuncu.X))
            {
                _skor++;
                _hiz = Math.Min(BaslangicHizi() + (Seviye - 1) * SeviyeBasinaHizArtisi, MaksimumHiz);
            }
        }

        foreach (var engel in _engeller)
        {
            // Ekrandan çıkan engel en sağdaki engelin arkasına taşınır.
            if (engel.EkrandanCiktiMi)
            {
                var enSagdaki = _engeller.MaxBy(e => e.X)!;
                engel.YenidenKonumla(enSagdaki.X + EngelAraligi, RastgeleBoslukY(enSagdaki.BoslukY));
            }
        }
    }

    private float BaslangicHizi() => _zorluk switch
    {
        Zorluk.Kolay => 2.5f,
        Zorluk.Zor => 3.5f,
        _ => 3f
    };

    private int BoslukBoyutu() => _zorluk switch
    {
        Zorluk.Kolay => 180,
        Zorluk.Zor => 125,
        _ => 150
    };

    /// <summary>Art arda iki boşluk arasındaki fark sınırlanır, böylece hız artınca da geçiş mümkün kalır.</summary>
    private int RastgeleBoslukY(int? oncekiBoslukY)
    {
        int enAz = BoslukKenarPayi;
        int enCok = ZeminY - BoslukKenarPayi - BoslukBoyutu();
        if (oncekiBoslukY is int onceki)
        {
            enAz = Math.Max(enAz, onceki - MaksimumBoslukFarki);
            enCok = Math.Min(enCok, onceki + MaksimumBoslukFarki);
        }
        return _rastgele.Next(enAz, enCok + 1);
    }
}
