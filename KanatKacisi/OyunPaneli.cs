namespace KanatKacisi;

/// <summary>Titremeyi önlemek için çift tamponlu çizim yapan oyun alanı paneli.</summary>
public class OyunPaneli : Panel
{
    public OyunPaneli()
    {
        DoubleBuffered = true;
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
    }
}
