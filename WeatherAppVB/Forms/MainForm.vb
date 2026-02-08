Imports System.Windows.Forms
Imports System.Drawing
Imports WeatherAppVB.Services
Imports WeatherAppVB.Models

Namespace WeatherAppVB.Forms
    ''' <summary>
    ''' Main application form - Weather display with search functionality
    ''' </summary>
    Public Class MainForm
        Inherits Form

        ' Menu
        Private menuStrip As MenuStrip
        Private settingsMenu As ToolStripMenuItem
        Private apiKeyMenuItem As ToolStripMenuItem

        ' Search controls
        Private WithEvents txtSearch As TextBox

        ' Weather display controls
        Private pnlWeatherCircle As Panel
        Private lblWeatherIcon As Label
        Private lblTemperature As Label
        Private lblCondition As Label
        Private lblLocationName As Label

        ' Details panel
        Private pnlDetails As Panel
        Private pnlFeelsLike As Panel
        Private pnlHumidity As Panel
        Private pnlWind As Panel
        Private lblFeelsLikeValue As Label
        Private lblFeelsLikeLabel As Label
        Private lblHumidityValue As Label
        Private lblHumidityLabel As Label
        Private lblWindValue As Label
        Private lblWindLabel As Label

        ' Error display
        Private lblError As Label

        ' Services
        Private ReadOnly _weatherService As WeatherService

        ' Colors (matching React app)
        Private ReadOnly PrimaryBlue As Color = Color.FromArgb(0, 122, 255)
        Private ReadOnly TextPrimary As Color = Color.FromArgb(26, 26, 46)
        Private ReadOnly TextSecondary As Color = Color.FromArgb(58, 58, 74)
        Private ReadOnly TextTertiary As Color = Color.FromArgb(106, 106, 122)

        Public Sub New()
            _weatherService = New WeatherService()
            InitializeComponent()
            LoadDefaultData()
        End Sub

        Private Sub InitializeComponent()
            Me.Text = "Weather App"
            Me.Size = New Size(450, 650)
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.BackColor = Color.White
            Me.Font = New Font("Segoe UI", 10)
            Me.FormBorderStyle = FormBorderStyle.FixedSingle
            Me.MaximizeBox = False

            InitializeMenu()
            InitializeSearchBox()
            InitializeErrorLabel()
            InitializeWeatherCircle()
            InitializeDetailsPanel()
        End Sub

        Private Sub InitializeMenu()
            menuStrip = New MenuStrip()
            menuStrip.BackColor = Color.White

            settingsMenu = New ToolStripMenuItem("Settings")
            apiKeyMenuItem = New ToolStripMenuItem("API Key...")
            AddHandler apiKeyMenuItem.Click, AddressOf ApiKeyMenuItem_Click

            settingsMenu.DropDownItems.Add(apiKeyMenuItem)
            menuStrip.Items.Add(settingsMenu)

            Me.MainMenuStrip = menuStrip
            Me.Controls.Add(menuStrip)
        End Sub

        Private Sub InitializeSearchBox()
            txtSearch = New TextBox()
            txtSearch.Location = New Point(40, 50)
            txtSearch.Size = New Size(355, 40)
            txtSearch.Font = New Font("Segoe UI", 12)
            txtSearch.BorderStyle = BorderStyle.FixedSingle
            txtSearch.PlaceholderText = "Search location..."
            Me.Controls.Add(txtSearch)
        End Sub

        Private Sub InitializeErrorLabel()
            lblError = New Label()
            lblError.Location = New Point(40, 100)
            lblError.Size = New Size(355, 40)
            lblError.Font = New Font("Segoe UI", 10)
            lblError.ForeColor = Color.Red
            lblError.BackColor = Color.FromArgb(255, 240, 240)
            lblError.TextAlign = ContentAlignment.MiddleCenter
            lblError.Visible = False
            Me.Controls.Add(lblError)
        End Sub

        Private Sub InitializeWeatherCircle()
            ' Outer circle panel
            pnlWeatherCircle = New Panel()
            pnlWeatherCircle.Location = New Point(85, 150)
            pnlWeatherCircle.Size = New Size(265, 265)
            pnlWeatherCircle.BackColor = Color.FromArgb(248, 248, 250)
            AddHandler pnlWeatherCircle.Paint, AddressOf PnlWeatherCircle_Paint
            Me.Controls.Add(pnlWeatherCircle)

            ' Weather icon
            lblWeatherIcon = New Label()
            lblWeatherIcon.Location = New Point(95, 25)
            lblWeatherIcon.Size = New Size(75, 55)
            lblWeatherIcon.Font = New Font("Segoe UI Emoji", 32)
            lblWeatherIcon.TextAlign = ContentAlignment.MiddleCenter
            lblWeatherIcon.BackColor = Color.Transparent
            pnlWeatherCircle.Controls.Add(lblWeatherIcon)

            ' Temperature
            lblTemperature = New Label()
            lblTemperature.Location = New Point(30, 80)
            lblTemperature.Size = New Size(205, 70)
            lblTemperature.Font = New Font("Segoe UI Light", 48)
            lblTemperature.ForeColor = PrimaryBlue
            lblTemperature.TextAlign = ContentAlignment.MiddleCenter
            lblTemperature.BackColor = Color.Transparent
            pnlWeatherCircle.Controls.Add(lblTemperature)

            ' Condition
            lblCondition = New Label()
            lblCondition.Location = New Point(30, 155)
            lblCondition.Size = New Size(205, 30)
            lblCondition.Font = New Font("Segoe UI Semibold", 12)
            lblCondition.ForeColor = TextSecondary
            lblCondition.TextAlign = ContentAlignment.MiddleCenter
            lblCondition.BackColor = Color.Transparent
            pnlWeatherCircle.Controls.Add(lblCondition)

            ' Location name
            lblLocationName = New Label()
            lblLocationName.Location = New Point(30, 185)
            lblLocationName.Size = New Size(205, 25)
            lblLocationName.Font = New Font("Segoe UI", 10)
            lblLocationName.ForeColor = PrimaryBlue
            lblLocationName.TextAlign = ContentAlignment.MiddleCenter
            lblLocationName.BackColor = Color.Transparent
            pnlWeatherCircle.Controls.Add(lblLocationName)
        End Sub

        Private Sub InitializeDetailsPanel()
            pnlDetails = New Panel()
            pnlDetails.Location = New Point(40, 440)
            pnlDetails.Size = New Size(355, 100)
            pnlDetails.BackColor = Color.Transparent
            Me.Controls.Add(pnlDetails)

            ' Feels Like card
            pnlFeelsLike = CreateDetailCard(0)
            lblFeelsLikeValue = CType(pnlFeelsLike.Controls(0), Label)
            lblFeelsLikeLabel = CType(pnlFeelsLike.Controls(1), Label)
            lblFeelsLikeLabel.Text = "FEELS LIKE"
            pnlDetails.Controls.Add(pnlFeelsLike)

            ' Humidity card
            pnlHumidity = CreateDetailCard(120)
            lblHumidityValue = CType(pnlHumidity.Controls(0), Label)
            lblHumidityLabel = CType(pnlHumidity.Controls(1), Label)
            lblHumidityLabel.Text = "HUMIDITY"
            pnlDetails.Controls.Add(pnlHumidity)

            ' Wind card
            pnlWind = CreateDetailCard(240)
            lblWindValue = CType(pnlWind.Controls(0), Label)
            lblWindLabel = CType(pnlWind.Controls(1), Label)
            lblWindLabel.Text = "WIND MPH"
            pnlDetails.Controls.Add(pnlWind)
        End Sub

        Private Function CreateDetailCard(xOffset As Integer) As Panel
            Dim card As New Panel()
            card.Location = New Point(xOffset, 0)
            card.Size = New Size(110, 90)
            card.BackColor = Color.FromArgb(248, 248, 250)
            AddHandler card.Paint, AddressOf DetailCard_Paint

            Dim valueLabel As New Label()
            valueLabel.Location = New Point(5, 20)
            valueLabel.Size = New Size(100, 35)
            valueLabel.Font = New Font("Segoe UI Semibold", 18)
            valueLabel.ForeColor = PrimaryBlue
            valueLabel.TextAlign = ContentAlignment.MiddleCenter
            valueLabel.BackColor = Color.Transparent
            card.Controls.Add(valueLabel)

            Dim textLabel As New Label()
            textLabel.Location = New Point(5, 55)
            textLabel.Size = New Size(100, 20)
            textLabel.Font = New Font("Segoe UI", 8)
            textLabel.ForeColor = TextTertiary
            textLabel.TextAlign = ContentAlignment.MiddleCenter
            textLabel.BackColor = Color.Transparent
            card.Controls.Add(textLabel)

            Return card
        End Function

        Private Sub PnlWeatherCircle_Paint(sender As Object, e As PaintEventArgs)
            Dim g As Graphics = e.Graphics
            g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

            ' Draw circular background
            Using brush As New SolidBrush(Color.FromArgb(248, 248, 250))
                g.FillEllipse(brush, 0, 0, pnlWeatherCircle.Width - 1, pnlWeatherCircle.Height - 1)
            End Using

            ' Draw border
            Using pen As New Pen(Color.FromArgb(230, 230, 235), 1)
                g.DrawEllipse(pen, 0, 0, pnlWeatherCircle.Width - 1, pnlWeatherCircle.Height - 1)
            End Using
        End Sub

        Private Sub DetailCard_Paint(sender As Object, e As PaintEventArgs)
            Dim panel As Panel = CType(sender, Panel)
            Dim g As Graphics = e.Graphics
            g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

            ' Draw rounded rectangle
            Dim rect As New Rectangle(0, 0, panel.Width - 1, panel.Height - 1)
            Dim radius As Integer = 15

            Using path As New Drawing2D.GraphicsPath()
                path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90)
                path.AddArc(rect.Right - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90)
                path.AddArc(rect.Right - radius * 2, rect.Bottom - radius * 2, radius * 2, radius * 2, 0, 90)
                path.AddArc(rect.X, rect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90)
                path.CloseFigure()

                Using brush As New SolidBrush(Color.FromArgb(248, 248, 250))
                    g.FillPath(brush, path)
                End Using

                Using pen As New Pen(Color.FromArgb(230, 230, 235), 1)
                    g.DrawPath(pen, path)
                End Using
            End Using
        End Sub

        Private Sub LoadDefaultData()
            ' Default data matching React app (San Francisco)
            lblWeatherIcon.Text = "🌤️"
            lblTemperature.Text = "68°"
            lblCondition.Text = "PARTLY CLOUDY"
            lblLocationName.Text = "San Francisco"
            lblFeelsLikeValue.Text = "65°"
            lblHumidityValue.Text = "72%"
            lblWindValue.Text = "12"
        End Sub

        Private Sub ApiKeyMenuItem_Click(sender As Object, e As EventArgs)
            Using dialog As New ApiKeyDialog(_weatherService.GetApiKey())
                If dialog.ShowDialog(Me) = DialogResult.OK Then
                    _weatherService.SetApiKey(dialog.ApiKey)
                    If _weatherService.HasApiKey() Then
                        MessageBox.Show("API key saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If
            End Using
        End Sub

        Private Async Sub txtSearch_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtSearch.KeyPress
            If e.KeyChar = ChrW(Keys.Enter) Then
                e.Handled = True
                Await SearchLocationAsync()
            End If
        End Sub

        Private Async Function SearchLocationAsync() As Task
            Dim location As String = txtSearch.Text.Trim()

            If String.IsNullOrWhiteSpace(location) Then
                Return
            End If

            ' Clear error
            lblError.Visible = False

            ' Check for API key
            If Not _weatherService.HasApiKey() Then
                ShowError("Please configure your API key first. Go to Settings > API Key...")
                Return
            End If

            Try
                txtSearch.Enabled = False
                Dim weatherData As WeatherData = Await _weatherService.GetWeatherAsync(location)
                UpdateWeatherDisplay(weatherData)
                txtSearch.Text = String.Empty
            Catch ex As Exception
                ShowError(ex.Message)
            Finally
                txtSearch.Enabled = True
                txtSearch.Focus()
            End Try
        End Function

        Private Sub UpdateWeatherDisplay(data As WeatherData)
            If data Is Nothing Then Return

            ' Update weather icon
            Dim condition As String = If(data.Weather?.Count > 0, data.Weather(0).Main, "Clear")
            lblWeatherIcon.Text = WeatherService.GetWeatherIcon(condition)

            ' Update temperature
            lblTemperature.Text = $"{Math.Round(data.Main.Temp)}°"

            ' Update condition
            lblCondition.Text = condition.ToUpper()

            ' Update location
            lblLocationName.Text = data.Name

            ' Update details
            lblFeelsLikeValue.Text = $"{Math.Round(data.Main.FeelsLike)}°"
            lblHumidityValue.Text = $"{data.Main.Humidity}%"
            lblWindValue.Text = $"{Math.Round(data.Wind.Speed)}"
        End Sub

        Private Sub ShowError(message As String)
            lblError.Text = message
            lblError.Visible = True
        End Sub
    End Class
End Namespace
