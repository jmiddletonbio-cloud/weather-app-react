Imports System.Windows.Forms

Namespace WeatherAppVB.Forms
    ''' <summary>
    ''' Dialog for entering/editing the OpenWeatherMap API key
    ''' </summary>
    Public Class ApiKeyDialog
        Inherits Form

        Private WithEvents txtApiKey As TextBox
        Private WithEvents btnSave As Button
        Private WithEvents btnCancel As Button
        Private lblInstruction As Label

        ''' <summary>
        ''' Gets the entered API key
        ''' </summary>
        Public Property ApiKey As String

        Public Sub New(currentApiKey As String)
            ApiKey = currentApiKey
            InitializeComponent()
            txtApiKey.Text = currentApiKey
        End Sub

        Private Sub InitializeComponent()
            Me.Text = "API Key Settings"
            Me.Size = New Drawing.Size(450, 200)
            Me.StartPosition = FormStartPosition.CenterParent
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.BackColor = Drawing.Color.White

            lblInstruction = New Label()
            lblInstruction.Text = "Enter your OpenWeatherMap API key:" & vbCrLf & "(Get one free at openweathermap.org)"
            lblInstruction.Location = New Drawing.Point(20, 20)
            lblInstruction.Size = New Drawing.Size(400, 40)
            lblInstruction.Font = New Drawing.Font("Segoe UI", 10)
            lblInstruction.ForeColor = Drawing.Color.FromArgb(60, 60, 60)

            txtApiKey = New TextBox()
            txtApiKey.Location = New Drawing.Point(20, 70)
            txtApiKey.Size = New Drawing.Size(395, 30)
            txtApiKey.Font = New Drawing.Font("Segoe UI", 11)
            txtApiKey.BorderStyle = BorderStyle.FixedSingle

            btnSave = New Button()
            btnSave.Text = "Save"
            btnSave.Location = New Drawing.Point(230, 115)
            btnSave.Size = New Drawing.Size(90, 35)
            btnSave.Font = New Drawing.Font("Segoe UI", 10)
            btnSave.BackColor = Drawing.Color.FromArgb(0, 122, 255)
            btnSave.ForeColor = Drawing.Color.White
            btnSave.FlatStyle = FlatStyle.Flat
            btnSave.FlatAppearance.BorderSize = 0
            btnSave.Cursor = Cursors.Hand

            btnCancel = New Button()
            btnCancel.Text = "Cancel"
            btnCancel.Location = New Drawing.Point(325, 115)
            btnCancel.Size = New Drawing.Size(90, 35)
            btnCancel.Font = New Drawing.Font("Segoe UI", 10)
            btnCancel.BackColor = Drawing.Color.FromArgb(200, 200, 200)
            btnCancel.ForeColor = Drawing.Color.FromArgb(60, 60, 60)
            btnCancel.FlatStyle = FlatStyle.Flat
            btnCancel.FlatAppearance.BorderSize = 0
            btnCancel.Cursor = Cursors.Hand

            Me.Controls.Add(lblInstruction)
            Me.Controls.Add(txtApiKey)
            Me.Controls.Add(btnSave)
            Me.Controls.Add(btnCancel)

            Me.AcceptButton = btnSave
            Me.CancelButton = btnCancel
        End Sub

        Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
            ApiKey = txtApiKey.Text.Trim()
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End Sub

        Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
        End Sub
    End Class
End Namespace
