namespace KanatKacisi.Services;

/// <summary>En yüksek skoru bir metin dosyasında saklar. Dosya hataları çağırana iletilir, arayüz mesajı form tarafında gösterilir.</summary>
public class EnYuksekSkorDeposu
{
    private readonly string _dosyaYolu;

    public EnYuksekSkorDeposu(string dosyaYolu)
    {
        _dosyaYolu = dosyaYolu;
    }

    /// <summary>%AppData%\KanatKacisi\enyuksek.txt</summary>
    public static string VarsayilanDosyaYolu =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "KanatKacisi", "enyuksek.txt");

    public string DosyaYolu => _dosyaYolu;

    /// <summary>Dosya yoksa veya içeriği sayı değilse 0 döner.</summary>
    public int Oku()
    {
        if (!File.Exists(_dosyaYolu))
            return 0;

        return int.TryParse(File.ReadAllText(_dosyaYolu).Trim(), out int skor) && skor > 0 ? skor : 0;
    }

    public void Kaydet(int skor)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_dosyaYolu)!);
        File.WriteAllText(_dosyaYolu, skor.ToString());
    }
}
