namespace KanatKacisi;

/// <summary>Titremeyi önlemek için çift tamponlu çizim yapan, klavye odağı alabilen oyun alanı paneli.</summary>
public class OyunPaneli : Panel
{
    public OyunPaneli()
    {
        DoubleBuffered = true;
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
        SetStyle(ControlStyles.Selectable, true);
        TabStop = true;
    }
}
