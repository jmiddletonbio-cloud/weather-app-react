Imports System.Windows.Forms

Namespace WeatherAppVB
    Module Program
        <STAThread>
        Sub Main()
            Application.SetHighDpiMode(HighDpiMode.SystemAware)
            Application.EnableVisualStyles()
            Application.SetCompatibleTextRenderingDefault(False)
            Application.Run(New MainForm())
        End Sub
    End Module
End Namespace
