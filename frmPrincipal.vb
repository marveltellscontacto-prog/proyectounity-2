Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.IO
Imports System.Reflection
Imports System.Windows.Forms

Public Class frmPrincipal

    ' ============================================================
    ' COLORES
    ' ============================================================

    Private ReadOnly Fondo As Color = Color.FromArgb(10, 14, 13)
    Private ReadOnly PanelColor As Color = Color.FromArgb(18, 21, 19)
    Private ReadOnly PanelColor2 As Color = Color.FromArgb(23, 25, 22)
    Private ReadOnly Dorado As Color = Color.FromArgb(190, 145, 63)
    Private ReadOnly DoradoClaro As Color = Color.FromArgb(225, 190, 115)
    Private ReadOnly Texto As Color = Color.FromArgb(230, 215, 180)
    Private ReadOnly TextoSuave As Color = Color.FromArgb(180, 165, 135)
    Private ReadOnly Rojo As Color = Color.FromArgb(91, 25, 24)
    Private ReadOnly Verde As Color = Color.FromArgb(55, 72, 22)
    Private ReadOnly Azul As Color = Color.FromArgb(25, 52, 67)

    Private WithEvents TimerAnimacion As New Timer()

    ' ============================================================
    ' CARGA DEL FORMULARIO
    ' ============================================================

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.SetStyle(
            ControlStyles.AllPaintingInWmPaint Or
            ControlStyles.UserPaint Or
            ControlStyles.OptimizedDoubleBuffer,
            True)

        Me.UpdateStyles()

        Me.Opacity = 0
        Me.SuspendLayout()

        Me.Text = "DM ASSISTANT"
        Me.WindowState = FormWindowState.Maximized
        Me.MinimumSize = New Size(1200, 700)
        Me.BackColor = Fondo
        Me.DoubleBuffered = True

        ' ========================================================
        ' FONDO
        ' ========================================================

        Dim rutaPreferida As String = Path.Combine(
            Application.StartupPath,
            "Resources",
            "background_login.jpg")

        If File.Exists(rutaPreferida) Then

            Try

                Dim bytes = File.ReadAllBytes(rutaPreferida)

                Using ms As New MemoryStream(bytes)

                    Me.BackgroundImage = Image.FromStream(ms)

                End Using

            Catch

                Me.BackgroundImage = Nothing

            End Try

        Else

            Me.BackgroundImage = Nothing

        End If

        Me.BackgroundImageLayout = ImageLayout.Stretch

        ' ========================================================
        ' CREAR INTERFAZ
        ' ========================================================

        CrearInterfaz()

        TimerAnimacion.Interval = 100
        TimerAnimacion.Start()

        Me.ResumeLayout()
        Me.Opacity = 1

    End Sub

    ' ============================================================
    ' CREAR INTERFAZ
    ' ============================================================

    Private Sub CrearInterfaz()

        Me.Controls.Clear()

        ' ========================================================
        ' PANEL IZQUIERDO
        ' ========================================================

        Dim izquierda As New Panel With {
            .Name = "pnlIzquierda",
            .BackColor = Color.FromArgb(180, 12, 15, 14),
            .Dock = DockStyle.Left,
            .Width = 250
        }

        Me.Controls.Add(izquierda)

        ' ========================================================
        ' LOGO
        ' ========================================================

        Dim lblLogo As New Label With {
            .Text = "🎲 DM",
            .ForeColor = Color.FromArgb(205, 40, 35),
            .Font = New Font("Georgia", 36, FontStyle.Bold),
            .AutoSize = True,
            .Location = New Point(30, 35)
        }

        izquierda.Controls.Add(lblLogo)

        Dim lblAssistant As New Label With {
            .Text = "ASSISTANT",
            .ForeColor = DoradoClaro,
            .Font = New Font("Georgia", 25, FontStyle.Bold),
            .AutoSize = True,
            .Location = New Point(35, 88)
        }

        izquierda.Controls.Add(lblAssistant)

        Dim lblSlogan As New Label With {
            .Text = "TU MUNDO, TU HISTORIA.",
            .ForeColor = Dorado,
            .Font = New Font("Georgia", 9),
            .AutoSize = True,
            .Location = New Point(45, 130)
        }

        izquierda.Controls.Add(lblSlogan)

        CrearLineaDecorativa(izquierda, 25, 160, 200)

        ' ========================================================
        ' ICONOS
        ' ========================================================

        Dim tituloIconos As New Label With {
            .Text = "ICONOS",
            .ForeColor = DoradoClaro,
            .Font = New Font("Georgia", 10),
            .AutoSize = True,
            .Location = New Point(100, 175)
        }

        izquierda.Controls.Add(tituloIconos)

        Dim iconos() As String = {
            "📖", "👥", "💀",
            "⚔", "🎲", "🗺",
            "📕", "💰", "🧪",
            "👹", "♛", "📜",
            "🏆", "🧙", "🪶"
        }

        Dim posX As Integer = 25
        Dim posY As Integer = 215

        For i As Integer = 0 To iconos.Length - 1

            Dim b As New Button With {
                .Text = iconos(i),
                .Width = 50,
                .Height = 50,
                .Location = New Point(posX, posY),
                .FlatStyle = FlatStyle.Flat,
                .Font = New Font("Segoe UI Emoji", 19),
                .ForeColor = Me.Texto,
                .BackColor = Color.FromArgb(20, 23, 21)
            }

            b.FlatAppearance.BorderColor = Color.FromArgb(60, 53, 38)
            b.FlatAppearance.BorderSize = 1

            izquierda.Controls.Add(b)

            posX += 62

            If (i + 1) Mod 3 = 0 Then

                posX = 25
                posY += 62

            End If

        Next

        ' ========================================================
        ' BOTONES IZQUIERDOS
        ' ========================================================

        CrearLineaDecorativa(izquierda, 25, 535, 200)

        Dim tituloBotones As New Label With {
            .Text = "BOTONES",
            .ForeColor = DoradoClaro,
            .Font = New Font("Georgia", 10),
            .AutoSize = True,
            .Location = New Point(90, 550)
        }

        izquierda.Controls.Add(tituloBotones)

        CrearBoton(
            izquierda,
            "NUEVA CAMPAÑA",
            25, 580, 200, 40,
            Rojo,
            AddressOf NuevaCampaña)

        CrearBoton(
            izquierda,
            "NUEVO ENCUENTRO",
            25, 630, 200, 40,
            Color.FromArgb(20, 40, 48),
            AddressOf NuevoEncuentro)

        CrearBoton(
            izquierda,
            "GUARDAR",
            25, 680, 200, 40,
            Color.FromArgb(60, 70, 24),
            AddressOf Guardar)

        CrearBoton(
            izquierda,
            "CANCELAR",
            25, 730, 200, 40,
            Color.FromArgb(24, 24, 22),
            AddressOf Cancelar)

        ' ========================================================
        ' CONTENIDO PRINCIPAL
        ' ========================================================

        Dim contenido As New Panel With {
            .Name = "pnlContenido",
            .Dock = DockStyle.Fill,
            .BackColor = Color.Transparent
        }

        Me.Controls.Add(contenido)

        contenido.BringToFront()

        ' ========================================================
        ' BARRA SUPERIOR
        ' ========================================================

        Dim barraSuperior As New Panel With {
            .Dock = DockStyle.Top,
            .Height = 55,
            .BackColor = Color.FromArgb(14, 17, 16)
        }

        contenido.Controls.Add(barraSuperior)

        Dim lblTitulo As New Label With {
            .Text = "♢  DM ASSISTANT",
            .ForeColor = DoradoClaro,
            .Font = New Font("Georgia", 13),
            .AutoSize = True,
            .Location = New Point(18, 17)
        }

        barraSuperior.Controls.Add(lblTitulo)

        Dim btnCerrar As New Button With {
            .Text = "×",
            .ForeColor = Texto,
            .BackColor = Color.Transparent,
            .FlatStyle = FlatStyle.Flat,
            .Font = New Font("Segoe UI", 16),
            .Width = 40,
            .Height = 40,
            .Anchor = AnchorStyles.Top Or AnchorStyles.Right,
            .Location = New Point(barraSuperior.Width - 50, 5)
        }

        btnCerrar.FlatAppearance.BorderSize = 0

        barraSuperior.Controls.Add(btnCerrar)

        AddHandler btnCerrar.Click,
            Sub()
                Me.Close()
            End Sub

        ' ========================================================
        ' TITULO
        ' ========================================================

        Dim bienvenida As New Label With {
            .Text = "Bienvenido, Dungeon Master",
            .ForeColor = Texto,
            .Font = New Font("Georgia", 22),
            .AutoSize = True,
            .Location = New Point(30, 80)
        }

        contenido.Controls.Add(bienvenida)

        Dim subtitulo As New Label With {
            .Text = "¿Qué aventura prepararás hoy?",
            .ForeColor = TextoSuave,
            .Font = New Font("Georgia", 10),
            .AutoSize = True,
            .Location = New Point(32, 115)
        }

        contenido.Controls.Add(subtitulo)

        ' ========================================================
        ' CAMPAÑA ACTUAL
        ' ========================================================

        Dim campaña As Panel =
            CrearPanel(contenido, 30, 145, 260, 220)

        AgregarTitulo(campaña, "CAMPAÑA ACTUAL")

        Dim lblCampaña As New Label With {
            .Text = "Las Minas de Eldoria",
            .ForeColor = Texto,
            .Font = New Font("Georgia", 13),
            .AutoSize = True,
            .Location = New Point(15, 50)
        }

        campaña.Controls.Add(lblCampaña)

        Dim infoCampaña As New Label With {
            .Text = "Sesión 12" & vbCrLf &
                    "Jugadores: 5" & vbCrLf &
                    "Progreso",
            .ForeColor = TextoSuave,
            .Font = New Font("Segoe UI", 9),
            .AutoSize = True,
            .Location = New Point(15, 85)
        }

        campaña.Controls.Add(infoCampaña)

        Dim progreso As New ProgressBar With {
            .Minimum = 0,
            .Maximum = 100,
            .Value = 67,
            .Width = 220,
            .Height = 15,
            .Location = New Point(15, 155)
        }

        campaña.Controls.Add(progreso)

        CrearBoton(
            campaña,
            "VER DETALLES",
            65, 180, 130, 30,
            Color.FromArgb(35, 34, 27),
            Nothing)

        ' ========================================================
        ' PROXIMA SESION
        ' ========================================================

        Dim sesion As Panel =
            CrearPanel(contenido, 305, 145, 260, 220)

        AgregarTitulo(sesion, "PRÓXIMA SESIÓN")

        Dim lblSesion As New Label With {
            .Text = "El Bosque Encantado",
            .ForeColor = Texto,
            .Font = New Font("Georgia", 13),
            .AutoSize = True,
            .Location = New Point(15, 50)
        }

        sesion.Controls.Add(lblSesion)

        Dim detallesSesion As New Label With {
            .Text = "◆ Encuentro: 3 Lobos + 1 Alfa" & vbCrLf &
                    "◆ NPCs: Garen, El Anciano" & vbCrLf &
                    "◆ Lugar: Bosque del Norte",
            .ForeColor = TextoSuave,
            .Font = New Font("Segoe UI", 9),
            .AutoSize = True,
            .Location = New Point(15, 90)
        }

        sesion.Controls.Add(detallesSesion)

        CrearBoton(
            sesion,
            "VER SESIÓN",
            65, 180, 130, 30,
            Color.FromArgb(35, 34, 27),
            Nothing)

        ' ========================================================
        ' NOTAS
        ' ========================================================

        Dim notas As Panel =
            CrearPanel(contenido, 580, 145, 260, 220)

        AgregarTitulo(notas, "NOTAS RÁPIDAS")

        Dim textoNotas As New Label With {
            .Text = "• El amuleto solo se activa" & vbCrLf &
                    "  bajo la luna llena." & vbCrLf &
                    vbCrLf &
                    "• Garen sospecha del grupo." & vbCrLf &
                    vbCrLf &
                    "• La entrada secreta está" & vbCrLf &
                    "  detrás de la cascada.",
            .ForeColor = Texto,
            .Font = New Font("Segoe UI", 9),
            .AutoSize = True,
            .Location = New Point(15, 50)
        }

        notas.Controls.Add(textoNotas)

        ' ========================================================
        ' ACCESO RAPIDO
        ' ========================================================

        Dim acceso As Panel =
            CrearPanel(contenido, 30, 385, 810, 105)

        AgregarTitulo(acceso, "ACCESO RÁPIDO")

        Dim accesos() As String = {
            "👥" & vbCrLf & "NPCs",
            "💀" & vbCrLf & "Monstruos",
            "⚔" & vbCrLf & "Combate",
            "🎲" & vbCrLf & "Dados",
            "🗺" & vbCrLf & "Mapas",
            "📖" & vbCrLf & "Notas"
        }

        For i As Integer = 0 To accesos.Length - 1

            Dim boton As New Button With {
                .Text = accesos(i),
                .Width = 115,
                .Height = 65,
                .Location = New Point(10 + i * 132, 30),
                .FlatStyle = FlatStyle.Flat,
                .BackColor = Color.FromArgb(19, 22, 20),
                .ForeColor = Texto,
                .Font = New Font("Segoe UI Emoji", 10)
            }

            boton.FlatAppearance.BorderColor =
                Color.FromArgb(70, 58, 37)

            boton.FlatAppearance.BorderSize = 1

            acceso.Controls.Add(boton)

        Next

        ' ========================================================
        ' PARTIDA ONLINE / OFFLINE
        ' ========================================================

        CrearBotonGrande(
            contenido,
            "🌐   PARTIDA ONLINE",
            "Conéctate y juega con tus amigos en línea.",
            30, 505, 390, 70,
            Azul,
            AddressOf PartidaOnline)

        CrearBotonGrande(
            contenido,
            "👥   PARTIDA OFFLINE",
            "Juega sin conexión en tu mundo.",
            440, 505, 400, 70,
            Verde,
            AddressOf PartidaOffline)

        ' ========================================================
        ' CHAT IA
        ' ========================================================


        Dim aiControl As New AIChatControl()
        aiControl.Name = "aiChatControl"
        aiControl.Size = New Size(350, 310)
        aiControl.Location = New Point(850, 405)
        aiControl.Anchor = AnchorStyles.Top Or AnchorStyles.Right

        ' Opcional: asignar los colores exactamente como en frmPrincipal
        aiControl.GoldColor = Color.FromArgb(190, 145, 63)       ' Dorado (coincide con tu variable Dorado)
        aiControl.GoldLightColor = Color.FromArgb(225, 190, 115) ' Dorado claro (DoradoClaro)
        aiControl.TextColor = Color.FromArgb(230, 215, 180)      ' Texto
        aiControl.PanelDark = Color.FromArgb(12, 15, 14)         ' Fondo del panel derecho

        ' Por defecto el Provider es MockProvider. Si quieres usar OpenAI en producción:
        ' aiControl.Provider = New OpenAIProvider(Environment.GetEnvironmentVariable("OPENAI_API_KEY"), "gpt-4")

        contenido.Controls.Add(aiControl)
        aiControl.BringToFront()

        ' ========================================================
        ' HERRAMIENTAS DE CREACION
        ' ========================================================

        Dim herramientas As Panel =
            CrearPanel(contenido, 30, 590, 810, 125)

        AgregarTitulo(
            herramientas,
            "HERRAMIENTAS DE CREACIÓN")

        CrearHerramienta(
            herramientas,
            "🏰",
            "CREAR DUNGEON",
            "Diseña mazmorras épicas",
            10,
            AddressOf CrearDungeon)

        CrearHerramienta(
            herramientas,
            "📖",
            "CREAR CAMPAÑA",
            "Construye historias",
            170,
            AddressOf CrearCampaña)

        CrearHerramienta(
            herramientas,
            "🗺",
            "CREAR MAPA",
            "Dibuja mapas detallados",
            330,
            AddressOf CrearMapa)

        CrearHerramienta(
            herramientas,
            "🛡",
            "CREAR PERSONAJE",
            "Crea héroes y NPCs",
            490,
            AddressOf CrearPersonaje)

        CrearHerramienta(
            herramientas,
            "⚔",
            "CREAR ENCUENTRO",
            "Diseña combates",
            650,
            AddressOf NuevoEncuentro)

        ' ========================================================
        ' PANEL DERECHO
        ' ========================================================

        CrearPanelDerecho()

    End Sub

    ' ============================================================
    ' PANEL DERECHO
    ' ============================================================

    Private Sub CrearPanelDerecho()

        Dim derecho As New Panel With {
            .Name = "pnlDerecho",
            .Dock = DockStyle.Right,
            .Width = 210,
            .BackColor = Color.FromArgb(12, 15, 14)
        }

        Me.Controls.Add(derecho)

        derecho.BringToFront()

        Dim titulo As New Label With {
            .Text = "BOTONES EXTRA",
            .ForeColor = DoradoClaro,
            .Font = New Font("Georgia", 10),
            .AutoSize = True,
            .Location = New Point(35, 30)
        }

        derecho.Controls.Add(titulo)

        CrearBoton(
            derecho,
            "EDITAR",
            25, 60, 160, 35,
            Color.FromArgb(30, 30, 26),
            AddressOf Editar)

        CrearBoton(
            derecho,
            "ELIMINAR",
            25, 105, 160, 35,
            Rojo,
            AddressOf Eliminar)

        CrearBoton(
            derecho,
            "DUPLICAR",
            25, 150, 160, 35,
            Color.FromArgb(30, 30, 26),
            AddressOf Duplicar)

        CrearBoton(
            derecho,
            "EXPORTAR",
            25, 195, 160, 35,
            Color.FromArgb(30, 30, 26),
            AddressOf Exportar)

        Dim instancias As New Label With {
            .Text = "INSTANCIAS",
            .ForeColor = DoradoClaro,
            .Font = New Font("Georgia", 10),
            .AutoSize = True,
            .Location = New Point(35, 255)
        }

        derecho.Controls.Add(instancias)

        Dim tokens As New Label With {
            .Text = "TOKENS",
            .ForeColor = DoradoClaro,
            .Font = New Font("Georgia", 10),
            .AutoSize = True,
            .Location = New Point(35, 430)
        }

        derecho.Controls.Add(tokens)

        Dim tokenNombres() As String = {
            "👨", "👩", "🧙",
            "👹", "🐺", "👿"
        }

        Dim tx As Integer = 20
        Dim ty As Integer = 465

        For i As Integer = 0 To tokenNombres.Length - 1

            Dim token As New Button With {
                .Text = tokenNombres(i),
                .Width = 52,
                .Height = 52,
                .Location = New Point(tx, ty),
                .Font = New Font("Segoe UI Emoji", 18),
                .FlatStyle = FlatStyle.Flat,
                .BackColor = Color.FromArgb(28, 27, 24),
                .ForeColor = Texto
            }

            token.FlatAppearance.BorderColor = Dorado
            token.FlatAppearance.BorderSize = 1

            derecho.Controls.Add(token)

            tx += 60

            If (i + 1) Mod 3 = 0 Then

                tx = 20
                ty += 60

            End If

        Next

        Dim dados As New Label With {
            .Text = "DADOS",
            .ForeColor = DoradoClaro,
            .Font = New Font("Georgia", 10),
            .AutoSize = True,
            .Location = New Point(35, 600)
        }

        derecho.Controls.Add(dados)

        Dim dadosTexto() As String = {
            "d20", "d20", "d20",
            "d10", "d8", "d6"
        }

        tx = 20
        ty = 630

        For i As Integer = 0 To dadosTexto.Length - 1

            Dim dado As New Button With {
                .Text = dadosTexto(i),
                .Width = 52,
                .Height = 52,
                .Location = New Point(tx, ty),
                .Font = New Font("Georgia", 9),
                .FlatStyle = FlatStyle.Flat,
                .BackColor = Color.FromArgb(29, 27, 24),
                .ForeColor = Texto
            }

            dado.FlatAppearance.BorderColor = Dorado
            dado.FlatAppearance.BorderSize = 1

            derecho.Controls.Add(dado)

            tx += 60

            If (i + 1) Mod 3 = 0 Then

                tx = 20
                ty += 60

            End If

        Next

    End Sub

    ' ============================================================
    ' TITULOS
    ' ============================================================

    Private Sub AgregarTitulo(
        parent As Control,
        texto As String)

        Dim titulo As New Label With {
            .Text = texto,
            .ForeColor = DoradoClaro,
            .Font = New Font("Georgia", 10, FontStyle.Bold),
            .AutoSize = True,
            .Location = New Point(15, 15)
        }

        parent.Controls.Add(titulo)

    End Sub

    ' ============================================================
    ' CREAR PANEL
    ' ============================================================

    Private Function CrearPanel(
        parent As Control,
        x As Integer,
        y As Integer,
        ancho As Integer,
        alto As Integer) As Panel

        Dim p As New Panel With {
            .Location = New Point(x, y),
            .Size = New Size(ancho, alto),
            .BackColor = PanelColor
        }

        parent.Controls.Add(p)

        Return p

    End Function

    ' ============================================================
    ' CREAR BOTON
    ' ============================================================

    Private Sub CrearBoton(
        parent As Control,
        tituloBoton As String,
        x As Integer,
        y As Integer,
        ancho As Integer,
        alto As Integer,
        fondo As Color,
        accion As EventHandler)

        Dim b As New Button With {
            .Text = tituloBoton,
            .Location = New Point(x, y),
            .Size = New Size(ancho, alto),
            .BackColor = fondo,
            .ForeColor = Texto,
            .Font = New Font("Georgia", 9),
            .FlatStyle = FlatStyle.Flat,
            .Cursor = Cursors.Hand
        }

        b.FlatAppearance.BorderColor = Dorado
        b.FlatAppearance.BorderSize = 1

        parent.Controls.Add(b)

        If accion IsNot Nothing Then

            AddHandler b.Click, accion

        End If

    End Sub

    ' ============================================================
    ' BOTON GRANDE
    ' ============================================================

    Private Sub CrearBotonGrande(
        parent As Control,
        titulo As String,
        descripcion As String,
        x As Integer,
        y As Integer,
        ancho As Integer,
        alto As Integer,
        fondo As Color,
        accion As EventHandler)

        Dim b As New Button With {
            .Text = titulo & vbCrLf & descripcion,
            .Location = New Point(x, y),
            .Size = New Size(ancho, alto),
            .BackColor = fondo,
            .ForeColor = Texto,
            .Font = New Font("Georgia", 10),
            .FlatStyle = FlatStyle.Flat,
            .Cursor = Cursors.Hand
        }

        b.FlatAppearance.BorderColor = Dorado
        b.FlatAppearance.BorderSize = 1

        parent.Controls.Add(b)

        If accion IsNot Nothing Then

            AddHandler b.Click, accion

        End If

    End Sub

    ' ============================================================
    ' HERRAMIENTAS
    ' ============================================================

    Private Sub CrearHerramienta(
        parent As Control,
        icono As String,
        titulo As String,
        descripcion As String,
        x As Integer,
        accion As EventHandler)

        Dim b As New Button With {
            .Text = icono & vbCrLf &
                    titulo & vbCrLf &
                    descripcion,
            .Location = New Point(x, 35),
            .Size = New Size(150, 75),
            .BackColor = Color.FromArgb(19, 21, 19),
            .ForeColor = Texto,
            .Font = New Font("Segoe UI", 8),
            .FlatStyle = FlatStyle.Flat,
            .Cursor = Cursors.Hand
        }

        b.FlatAppearance.BorderColor = Dorado
        b.FlatAppearance.BorderSize = 1

        parent.Controls.Add(b)

        If accion IsNot Nothing Then

            AddHandler b.Click, accion

        End If

    End Sub

    ' ============================================================
    ' LINEA DECORATIVA
    ' ============================================================

    Private Sub CrearLineaDecorativa(
        parent As Control,
        x As Integer,
        y As Integer,
        ancho As Integer)

        Dim linea As New Panel With {
            .BackColor = Color.FromArgb(91, 69, 34),
            .Location = New Point(x, y),
            .Size = New Size(ancho, 1)
        }

        parent.Controls.Add(linea)

    End Sub

    ' ============================================================
    ' DECORACION
    ' ============================================================

    Private Sub DibujarMarco(g As Graphics)

        Using p As New Pen(
            Color.FromArgb(83, 63, 33),
            1)

            g.DrawRectangle(
                p,
                8,
                8,
                Me.ClientSize.Width - 16,
                Me.ClientSize.Height - 16)

        End Using

    End Sub

    Private Sub DibujarDecoracion(g As Graphics)

        Using p As New Pen(
            Color.FromArgb(86, 65, 35),
            1)

            Dim y As Integer = 20

            For i As Integer = 0 To 5

                g.DrawLine(
                    p,
                    20,
                    y,
                    150,
                    y)

                g.DrawLine(
                    p,
                    Me.ClientSize.Width - 150,
                    y,
                    Me.ClientSize.Width - 20,
                    y)

                y += 25

            Next

        End Using

    End Sub

    ' ============================================================
    ' ABRIR CAMPAÑAS
    ' ============================================================

    Private Sub NuevaCampaña(
        sender As Object,
        e As EventArgs)

        Try

            Dim ventana As New FrmCampanas()

            ventana.ShowDialog(Me)

        Catch ex As Exception

            MessageBox.Show(
                "No se pudo abrir el formulario de campañas." &
                vbCrLf & vbCrLf &
                ex.Message,
                "DM ASSISTANT",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error)

        End Try

    End Sub

    ' ============================================================
    ' ABRIR ENCUENTROS
    ' ============================================================

    Private Sub NuevoEncuentro(
        sender As Object,
        e As EventArgs)

        Try

            Dim ventana As New FrmEncuentros()

            ventana.ShowDialog(Me)

        Catch ex As Exception

            MessageBox.Show(
                "No se pudo abrir el formulario de encuentros." &
                vbCrLf & vbCrLf &
                ex.Message,
                "DM ASSISTANT",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error)

        End Try

    End Sub

    ' ============================================================
    ' GUARDAR
    ' ============================================================

    Private Sub Guardar(
        sender As Object,
        e As EventArgs)

        MessageBox.Show(
            "Partida guardada correctamente.",
            "DM ASSISTANT",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information)

    End Sub

    ' ============================================================
    ' CANCELAR
    ' ============================================================

    Private Sub Cancelar(
        sender As Object,
        e As EventArgs)

        Me.Close()

    End Sub

    ' ============================================================
    ' PARTIDA ONLINE
    ' ============================================================

    Private Sub PartidaOnline(
        sender As Object,
        e As EventArgs)

        Try

            Dim ventana As New FrmOnline()

            ventana.ShowDialog(Me)

        Catch ex As Exception

            MessageBox.Show(
                "No se pudo abrir el módulo ONLINE." &
                vbCrLf & vbCrLf &
                ex.Message,
                "DM ASSISTANT",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error)

        End Try

    End Sub

    ' ============================================================
    ' PARTIDA OFFLINE
    ' ============================================================

    Private Sub PartidaOffline(
        sender As Object,
        e As EventArgs)

        Try

            Dim ventana As New FrmOffline()

            ventana.ShowDialog(Me)

        Catch ex As Exception

            MessageBox.Show(
                "No se pudo abrir el módulo OFFLINE." &
                vbCrLf & vbCrLf &
                ex.Message,
                "DM ASSISTANT",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error)

        End Try

    End Sub

    ' ============================================================
    ' CREAR DUNGEON
    ' ============================================================

    Private Sub CrearDungeon(
        sender As Object,
        e As EventArgs)

        Try

            Dim ventana As New FrmDungeons()

            ventana.ShowDialog(Me)

        Catch ex As Exception

            MessageBox.Show(
                "No se pudo abrir el editor de Dungeons." &
                vbCrLf & vbCrLf &
                ex.Message,
                "DM ASSISTANT",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error)

        End Try

    End Sub

    ' ============================================================
    ' CREAR CAMPAÑA
    ' ============================================================

    Private Sub CrearCampaña(
        sender As Object,
        e As EventArgs)

        Try

            Dim ventana As New FrmCampanas()

            ventana.ShowDialog(Me)

        Catch ex As Exception

            MessageBox.Show(
                "No se pudo abrir el creador de campañas." &
                vbCrLf & vbCrLf &
                ex.Message,
                "DM ASSISTANT",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error)

        End Try

    End Sub

    ' ============================================================
    ' CREAR MAPA
    ' ============================================================

    Private Sub CrearMapa(
        sender As Object,
        e As EventArgs)

        Try

            Dim ventana As New FrmMaps

            ventana.ShowDialog(Me)

        Catch ex As Exception

            MessageBox.Show(
                "No se pudo abrir el editor de mapas." &
                vbCrLf & vbCrLf &
                ex.Message,
                "DM ASSISTANT",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error)

        End Try

    End Sub

    ' ============================================================
    ' CREAR PERSONAJE
    ' ============================================================

    Private Sub CrearPersonaje(
        sender As Object,
        e As EventArgs)

        Try

            Dim ventana As New FrmPersonajes()

            ventana.ShowDialog(Me)

        Catch ex As Exception

            MessageBox.Show(
                "No se pudo abrir el creador de personajes." &
                vbCrLf & vbCrLf &
                ex.Message,
                "DM ASSISTANT",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error)

        End Try

    End Sub

    ' ============================================================
    ' EDITAR
    ' ============================================================

    Private Sub Editar(
        sender As Object,
        e As EventArgs)

        MessageBox.Show(
            "Selecciona primero el elemento que deseas editar.",
            "DM ASSISTANT",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information)

    End Sub

    ' ============================================================
    ' ELIMINAR
    ' ============================================================

    Private Sub Eliminar(
        sender As Object,
        e As EventArgs)

        If MessageBox.Show(
            "¿Eliminar elemento seleccionado?",
            "DM ASSISTANT",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning) = DialogResult.Yes Then

            MessageBox.Show(
                "Elemento eliminado.",
                "DM ASSISTANT",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)

        End If

    End Sub

    ' ============================================================
    ' DUPLICAR
    ' ============================================================

    Private Sub Duplicar(
        sender As Object,
        e As EventArgs)

        MessageBox.Show(
            "Elemento duplicado.",
            "DM ASSISTANT",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information)

    End Sub

    ' ============================================================
    ' EXPORTAR
    ' ============================================================

    Private Sub Exportar(
        sender As Object,
        e As EventArgs)

        MessageBox.Show(
            "Módulo de exportación.",
            "DM ASSISTANT",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information)

    End Sub

End Class