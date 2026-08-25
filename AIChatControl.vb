Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports DNDORGANIZAR.IAprovider

Public Class AIChatControl
    Inherits UserControl

    ' Evitar que el diseñador intente serializar/mostrar esta propiedad
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property Provider As IAIProvider

    Private lblTitle As Label
    Private rtbHistory As RichTextBox
    Private txtInput As TextBox
    Private btnSend As Button
    Private pnlPresets As FlowLayoutPanel
    Private lblTyping As Label

    Public Sub New()
        Me.InitializeComponentUI()
        ' Por defecto: modo local MOCK (sin llamadas externas)
        Me.Provider = New MockProvider()
    End Sub

    Private Sub InitializeComponentUI()
        Me.Dock = DockStyle.Fill
        Me.BackColor = Color.FromArgb(20, 20, 20)

        lblTitle = New Label With {
            .Text = "✦  ASISTENTE DND",
            .ForeColor = Color.FromArgb(230, 200, 120),
            .Font = New Font("Georgia", 11, FontStyle.Bold),
            .AutoSize = False,
            .Height = 30,
            .Dock = DockStyle.Top,
            .TextAlign = ContentAlignment.MiddleLeft,
            .Padding = New Padding(8, 0, 0, 0)
        }
        Me.Controls.Add(lblTitle)

        ' Presets (botones rápidos)
        pnlPresets = New FlowLayoutPanel With {
            .Dock = DockStyle.Top,
            .Height = 36,
            .Padding = New Padding(6),
            .FlowDirection = FlowDirection.LeftToRight,
            .BackColor = Color.Transparent
        }
        Me.Controls.Add(pnlPresets)

        AddPresetButton("Generar Encuentro", AddressOf Preset_Encounter)
        AddPresetButton("Generar NPC", AddressOf Preset_NPC)
        AddPresetButton("Generar Loot", AddressOf Preset_Loot)
        AddPresetButton("Sugerir Ganchos", AddressOf Preset_Hooks)

        rtbHistory = New RichTextBox With {
            .Dock = DockStyle.Top,
            .Height = 220,
            .ReadOnly = True,
            .BackColor = Color.FromArgb(14, 14, 14),
            .ForeColor = Color.FromArgb(220, 220, 220),
            .BorderStyle = BorderStyle.None
        }
        Me.Controls.Add(rtbHistory)

        lblTyping = New Label With {
            .Text = "",
            .ForeColor = Color.LightGray,
            .Font = New Font("Segoe UI", 8),
            .Dock = DockStyle.Top,
            .Height = 18,
            .Padding = New Padding(8, 0, 0, 0),
            .Visible = False
        }
        Me.Controls.Add(lblTyping)

        Dim pnlBottom As New Panel With {
            .Dock = DockStyle.Bottom,
            .Height = 90,
            .BackColor = Color.Transparent,
            .Padding = New Padding(8)
        }

        txtInput = New TextBox With {
            .Multiline = True,
            .Height = 56,
            .Dock = DockStyle.Fill,
            .BackColor = Color.FromArgb(24, 24, 24),
            .ForeColor = Color.FromArgb(230, 230, 230),
            .BorderStyle = BorderStyle.FixedSingle
        }
        AddHandler txtInput.KeyDown, AddressOf TxtInput_KeyDown

        btnSend = New Button With {
            .Text = "➤",
            .Width = 48,
            .Dock = DockStyle.Right,
            .BackColor = Color.FromArgb(140, 40, 30),
            .ForeColor = Color.White,
            .FlatStyle = FlatStyle.Flat
        }
        btnSend.FlatAppearance.BorderSize = 0
        AddHandler btnSend.Click, AddressOf BtnSend_Click

        pnlBottom.Controls.Add(txtInput)
        pnlBottom.Controls.Add(btnSend)
        Me.Controls.Add(pnlBottom)
    End Sub

    Private Sub AddPresetButton(text As String, handler As EventHandler)
        Dim b As New Button With {
            .Text = text,
            .AutoSize = True,
            .BackColor = Color.FromArgb(34, 34, 34),
            .ForeColor = Color.White,
            .FlatStyle = FlatStyle.Flat,
            .Padding = New Padding(6)
        }
        b.FlatAppearance.BorderSize = 0
        AddHandler b.Click, handler
        pnlPresets.Controls.Add(b)
    End Sub

    ' Presets: generan prompts dirigidos a DnD
    Private Sub Preset_Encounter(sender As Object, e As EventArgs)
        txtInput.Text = "Genera un encuentro de nivel bajo en un bosque encantado: número y tipo de enemigos, dificultad aproximada, un gancho narrativo y botín temático."
    End Sub

    Private Sub Preset_NPC(sender As Object, e As EventArgs)
        txtInput.Text = "Crea un NPC interesante: nombre, breve trasfondo, motivación, rasgos de personalidad y cómo introducirlo en una sesión."
    End Sub

    Private Sub Preset_Loot(sender As Object, e As EventArgs)
        txtInput.Text = "Sugiere un botín interesante para un cofre encontrado en ruinas: objetos mágicos menores, monedas y una pista para futuras aventuras."
    End Sub

    Private Sub Preset_Hooks(sender As Object, e As EventArgs)
        txtInput.Text = "Dame 3 ganchos cortos (1 línea cada uno) para iniciar una aventura en un pueblo costero."
    End Sub

    Private Async Sub TxtInput_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Enter AndAlso Not e.Shift Then
            e.SuppressKeyPress = True
            Await SendCurrentInputAsync()
        End If
    End Sub

    Private Async Sub BtnSend_Click(sender As Object, e As EventArgs)
        Await SendCurrentInputAsync()
    End Sub

    Private Async Function SendCurrentInputAsync() As Task
        Dim userText = txtInput.Text.Trim()
        If String.IsNullOrEmpty(userText) Then Return

        AppendUserPrefixToHistory("Tú: ")
        AppendUserTextToHistory(userText)
        txtInput.Clear()

        lblTyping.Text = "Asistente está escribiendo..."
        lblTyping.Visible = True
        btnSend.Enabled = False
        txtInput.Enabled = False

        Try
            If Provider Is Nothing Then
                AppendSystemPrefixToHistory("Sistema: ")
                AppendSystemTextToHistory("Proveedor de IA no configurado. Usando modo local MOCK.")
                Provider = New MockProvider()
            End If

            Dim aiReply = Await Provider.QueryAsync(userText)
            If Not String.IsNullOrEmpty(aiReply) Then
                AppendAssistantPrefixToHistory("Asistente: ")
                AppendAssistantTextToHistory(aiReply)
            Else
                AppendSystemPrefixToHistory("Sistema: ")
                AppendSystemTextToHistory("La IA no devolvió respuesta.")
            End If
        Catch ex As Exception
            AppendSystemPrefixToHistory("Sistema: ")
            AppendSystemTextToHistory("Error: " & ex.Message)
        Finally
            lblTyping.Visible = False
            btnSend.Enabled = True
            txtInput.Enabled = True
            txtInput.Focus()
        End Try
    End Function

    ' ---------- Métodos renombrados para evitar ambigüedad ----------
    Private Sub AppendUserPrefixToHistory(prefix As String)
        AppendPrefixToHistory(prefix, Color.FromArgb(180, 220, 255), True)
    End Sub
    Private Sub AppendUserTextToHistory(text As String)
        AppendTextToHistory(text & vbCrLf & vbCrLf, Color.FromArgb(200, 200, 200), False)
    End Sub

    Private Sub AppendAssistantPrefixToHistory(prefix As String)
        AppendPrefixToHistory(prefix, Color.FromArgb(240, 210, 140), True)
    End Sub
    Private Sub AppendAssistantTextToHistory(text As String)
        AppendTextToHistory(text & vbCrLf & vbCrLf, Color.FromArgb(210, 210, 210), False)
    End Sub

    Private Sub AppendSystemPrefixToHistory(prefix As String)
        AppendPrefixToHistory(prefix, Color.OrangeRed, True)
    End Sub
    Private Sub AppendSystemTextToHistory(text As String)
        AppendTextToHistory(text & vbCrLf & vbCrLf, Color.LightGray, False)
    End Sub

    ' Prefijo con color y opcional en negrita
    Private Sub AppendPrefixToHistory(prefix As String, prefixColor As Color, isBoldPrefix As Boolean)
        rtbHistory.SelectionStart = rtbHistory.TextLength
        rtbHistory.SelectionColor = prefixColor
        rtbHistory.SelectionFont = If(isBoldPrefix, New Font(rtbHistory.Font, FontStyle.Bold), rtbHistory.Font)
        rtbHistory.AppendText(prefix)
    End Sub

    ' Texto normal (con opción de negrita)
    Private Sub AppendTextToHistory(text As String, textColor As Color, Optional bold As Boolean = False)
        rtbHistory.SelectionStart = rtbHistory.TextLength
        rtbHistory.SelectionColor = textColor
        rtbHistory.SelectionFont = If(bold, New Font(rtbHistory.Font, FontStyle.Bold), rtbHistory.Font)
        rtbHistory.AppendText(text)
        rtbHistory.ScrollToCaret()
    End Sub

End Class