namespace KanatKacisi;

static class Program
{
    /// <summary>Uygulamanın giriş noktası.</summary>
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new AnaForm());
    }
}
