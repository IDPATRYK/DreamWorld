#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

'Permission Is hereby granted, free Of charge, to any person obtaining a copy of this software
' And associated documentation files (the "Software"), to deal in the Software without restriction,
'including without limitation the rights To use, copy, modify, merge, publish, distribute, sublicense,
'And/Or sell copies Of the Software, And To permit persons To whom the Software Is furnished To
'Do so, subject To the following conditions:

'The above copyright notice And this permission notice shall be included In all copies Or '
'substantial portions Of the Software.

'THE SOFTWARE Is PROVIDED "AS IS", WITHOUT WARRANTY Of ANY KIND, EXPRESS Or IMPLIED,
' INCLUDING BUT Not LIMITED To THE WARRANTIES Of MERCHANTABILITY, FITNESS For A PARTICULAR
'PURPOSE And NONINFRINGEMENT.In NO Event SHALL THE AUTHORS Or COPYRIGHT HOLDERS BE LIABLE
'For ANY CLAIM, DAMAGES Or OTHER LIABILITY, WHETHER In AN ACTION Of CONTRACT, TORT Or
'OTHERWISE, ARISING FROM, OUT Of Or In CONNECTION With THE SOFTWARE Or THE USE Or OTHER
'DEALINGS IN THE SOFTWARE.Imports System

#End Region

Imports System.Text.RegularExpressions

Public Class FormMaps

#Region "Private Fields"

    Private ReadOnly Handler As New EventHandler(AddressOf Resize_page)
    Private _screenPosition As ScreenPos

#End Region

#Region "Public Properties"

    Public Property ScreenPosition As ScreenPos
        Get
            Return _screenPosition
        End Get
        Set(value As ScreenPos)
            _screenPosition = value
        End Set
    End Property

#End Region

#Region "Private Methods"

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles ViewMap.Click

        TextPrint(My.Resources.Clearing_Map_tiles_word)
        Dim f As String = Settings.OpensimBinPath & "Maptiles\00000000-0000-0000-0000-000000000000"
        Try
            DeleteDirectory(f, FileIO.DeleteDirectoryOption.DeleteAllContents)
            My.Computer.FileSystem.CreateDirectory(f)
        Catch ex As Exception
            BreakPoint.Show(ex.Message)
        End Try
        TextPrint(My.Resources.Maps_Erased)

    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles SmallMapButton.Click
        Dim webAddress As String = "http://" & Settings.PublicIP & ":" & CStr(Settings.ApachePort) & "/Metromap/index.php"
        Try
            Process.Start(webAddress)
        Catch ex As Exception
            BreakPoint.Show(ex.Message)

        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Dim X As Integer = Settings.MapCenterX
        Dim Y As Integer = Settings.MapCenterY

        Dim webAddress As String = "http://" & CStr(Settings.PublicIP) & ":" & CStr(Settings.HttpPort) & "/wifi/map.html?X=" & CStr(X) & "&Y=" & CStr(Y)

        Try
            Process.Start(webAddress)
        Catch ex As Exception
            BreakPoint.Show(ex.Message)
        End Try
    End Sub

    Private Sub DatabaseSetupToolStripMenuItem_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub IsClosed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed

        FormSetup.PropViewedSettings = True
        Settings.SaveSettings()

    End Sub

    Private Sub LargeMapButton_Click(sender As Object, e As EventArgs) Handles LargeMapButton.Click
        Dim webAddress As String = "http://" + Settings.PublicIP & ":" & Settings.ApachePort & "/Metromap/indexmax.php"
        Try
            Process.Start(webAddress)
        Catch ex As Exception
            BreakPoint.Show(ex.Message)
        End Try
    End Sub

    Private Sub Loaded(sender As Object, e As EventArgs) Handles Me.Load

        Button2.Text = Global.Outworldz.My.Resources.View_Maps

        GroupBox2.Text = Global.Outworldz.My.Resources.Maps_word
        Label1.Text = Global.Outworldz.My.Resources.Map_Center_Location_word
        Label2.Text = Global.Outworldz.My.Resources.X
        Label3.Text = Global.Outworldz.My.Resources.Y
        Label4.Text = Global.Outworldz.My.Resources.RenderMax
        Label5.Text = Global.Outworldz.My.Resources.RenderMin
        LargeMapButton.Text = Global.Outworldz.My.Resources.LargeMap
        MapBest.Text = Global.Outworldz.My.Resources.Best_Prims
        MapBetter.Text = Global.Outworldz.My.Resources.Better_Prims
        MapBox.Text = Global.Outworldz.My.Resources.Maps_word
        MapGood.Text = Global.Outworldz.My.Resources.Good_Warp3D_word

        MapNone.Text = Global.Outworldz.My.Resources.None
        MapSimple.Text = Global.Outworldz.My.Resources.Simple_but_Fast_word
        MenuStrip2.Text = Global.Outworldz.My.Resources._0
        SmallMapButton.Text = Global.Outworldz.My.Resources.Small_Map
        Text = Global.Outworldz.My.Resources.Maps_word
        ToolStripMenuItem30.Image = Global.Outworldz.My.Resources.question_and_answer
        ToolStripMenuItem30.Text = Global.Outworldz.My.Resources.Help_word
        ToolTip1.SetToolTip(Button2, Global.Outworldz.My.Resources.WifiMap)
        ToolTip1.SetToolTip(MapXStart, Global.Outworldz.My.Resources.CenterMap)
        ToolTip1.SetToolTip(MapYStart, Global.Outworldz.My.Resources.CenterMap)
        ToolTip1.SetToolTip(RenderMaxH, Global.Outworldz.My.Resources.Max4096)
        ToolTip1.SetToolTip(ViewMap, Global.Outworldz.My.Resources.Regen_Map)
        ViewMap.Text = Global.Outworldz.My.Resources.DelMaps

        If Settings.MapType = "None" Then
            MapNone.Checked = True
            MapPicture.Image = Global.Outworldz.My.Resources.blankbox
        ElseIf Settings.MapType = "Simple" Then
            MapSimple.Checked = True
            MapPicture.Image = Global.Outworldz.My.Resources.Simple
        ElseIf Settings.MapType = "Good" Then
            MapGood.Checked = True
            MapPicture.Image = Global.Outworldz.My.Resources.Good
        ElseIf Settings.MapType = "Better" Then
            MapBetter.Checked = True
            MapPicture.Image = Global.Outworldz.My.Resources.Better
        ElseIf Settings.MapType = "Best" Then
            MapBest.Checked = True
            MapPicture.Image = Global.Outworldz.My.Resources.Best
        End If

        If PropOpensimIsRunning Then
            Button2.Enabled = True
        Else
            Button2.Enabled = False
        End If

        If Settings.ApacheEnable Then
            MapYStart.Text = CStr(Settings.MapCenterY)
            MapXStart.Text = CStr(Settings.MapCenterX)
            MapYStart.Enabled = True
            MapXStart.Enabled = True
            LargeMapButton.Enabled = True
            SmallMapButton.Enabled = True
        Else
            MapYStart.Enabled = False
            MapXStart.Enabled = False
            LargeMapButton.Enabled = False
            SmallMapButton.Enabled = False
        End If

        RenderMaxH.Text = CStr(Settings.RenderMaxHeight)
        RenderMinH.Text = CStr(Settings.RenderMinHeight)

        HelpOnce("Maps")
        SetScreen()

    End Sub

    Private Sub MapBest_CheckedChanged(sender As Object, e As EventArgs) Handles MapBest.CheckedChanged

        Settings.MapType = "Best"
        MapPicture.Image = Global.Outworldz.My.Resources.Best

    End Sub

    Private Sub MapBetter_CheckedChanged(sender As Object, e As EventArgs) Handles MapBetter.CheckedChanged

        Settings.MapType = "Better"
        MapPicture.Image = Global.Outworldz.My.Resources.Better

    End Sub

    Private Sub MapGood_CheckedChanged(sender As Object, e As EventArgs) Handles MapGood.CheckedChanged

        Settings.MapType = "Good"

        MapPicture.Image = Global.Outworldz.My.Resources.Good

    End Sub

    Private Sub MapNone_CheckedChanged(sender As Object, e As EventArgs) Handles MapNone.CheckedChanged

        Settings.MapType = "None"
        MapPicture.Image = Global.Outworldz.My.Resources.blankbox

    End Sub

    Private Sub MapSimple_CheckedChanged(sender As Object, e As EventArgs) Handles MapSimple.CheckedChanged

        Settings.MapType = "Simple"
        MapPicture.Image = Global.Outworldz.My.Resources.Simple

    End Sub

    Private Sub MapXStart_TextChanged(sender As Object, e As EventArgs) Handles MapXStart.TextChanged

        Dim digitsOnly As Regex = New Regex("[^\d]")
        MapXStart.Text = digitsOnly.Replace(MapXStart.Text, "")
        If Not Integer.TryParse(MapXStart.Text, Settings.MapCenterX) Then
            MsgBox(My.Resources.Must_be_A_Number, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground)
        End If

    End Sub

    Private Sub MapYStart_TextChanged(sender As Object, e As EventArgs) Handles MapYStart.TextChanged

        Dim digitsOnly As Regex = New Regex("[^\d]")
        MapYStart.Text = digitsOnly.Replace(MapYStart.Text, "")
        If Not Integer.TryParse(MapYStart.Text, Settings.MapCenterY) Then
            MsgBox(My.Resources.Must_be_A_Number, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground)
        End If

    End Sub

    Private Sub RenderMaxH_TextChanged(sender As Object, e As EventArgs) Handles RenderMaxH.TextChanged

        Dim digitsOnly As Regex = New Regex("[^-\d]")
        RenderMaxH.Text = digitsOnly.Replace(RenderMaxH.Text, "")
        If Not Integer.TryParse(RenderMaxH.Text, CInt("0" & Settings.RenderMaxHeight)) Then
            MsgBox(My.Resources.Must_be_A_Number, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground)
        End If

    End Sub

    Private Sub RenderMinH_TextChanged(sender As Object, e As EventArgs) Handles RenderMinH.TextChanged
        Dim digitsOnly As Regex = New Regex("[^-\d]")
        RenderMinH.Text = digitsOnly.Replace(RenderMinH.Text, "")
        If Not Integer.TryParse(RenderMinH.Text, Settings.RenderMinHeight) Then
            MsgBox(My.Resources.Must_be_A_Number, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground)
        End If

    End Sub

    'The following detects  the location of the form in screen coordinates
    Private Sub Resize_page(ByVal sender As Object, ByVal e As System.EventArgs)
        'Me.Text = "Form screen position = " + Me.Location.ToString
        ScreenPosition.SaveXY(Me.Left, Me.Top)
    End Sub

    Private Sub SetScreen()
        Me.Show()
        ScreenPosition = New ScreenPos(Me.Name)
        AddHandler ResizeEnd, Handler
        Dim xy As List(Of Integer) = ScreenPosition.GetXY()
        Me.Left = xy.Item(0)
        Me.Top = xy.Item(1)
    End Sub

    Private Sub ToolStripMenuItem30_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem30.Click
        HelpManual("Maps")
    End Sub

#End Region

End Class
