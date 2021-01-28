﻿#Region "Copyright AGPL3.0"

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

Public Class FormLogging

#Region "Private Fields"

    Dim initted As Boolean

#End Region

#Region "FormPos"

    Private _screenPosition As ScreenPos
    Private Handler As New EventHandler(AddressOf Resize_page)

    Public Property ScreenPosition As ScreenPos
        Get
            Return _screenPosition
        End Get
        Set(value As ScreenPos)
            _screenPosition = value
        End Set
    End Property

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

#End Region

#Region "Start/Stop"

    Private Sub Close_form(sender As Object, e As EventArgs) Handles Me.Closed

        Settings.SaveSettings()
        SendMsg(Settings.LogLevel.ToUpperInvariant)

    End Sub

    Private Sub Loaded(sender As Object, e As EventArgs) Handles Me.Load

        GroupBox1.Text = Global.Outworldz.My.Resources.Logging_word
        HelpToolStripMenuItem.Image = Global.Outworldz.My.Resources.question_and_answer
        HelpToolStripMenuItem.Text = Global.Outworldz.My.Resources.Help_word
        LoggingToolStripMenuItem.Image = Global.Outworldz.My.Resources.window_environment
        LoggingToolStripMenuItem.Text = Global.Outworldz.My.Resources.Logging_word
        RadioDebug.Text = Global.Outworldz.My.Resources.Debug_word
        RadioError.Text = Global.Outworldz.My.Resources.Error_word
        RadioFatal.Text = Global.Outworldz.My.Resources.Fatal_word
        RadioInfo.Text = Global.Outworldz.My.Resources.Info_word
        RadioOff.Text = Global.Outworldz.My.Resources.Off
        RadioWarn.Text = Global.Outworldz.My.Resources.Warn_word
        Text = Global.Outworldz.My.Resources.Logging_word

        SetScreen()

        Select Case Settings.LogLevel.ToUpperInvariant
            Case "OFF"
                RadioOff.Checked = True
            Case "DEBUG"
                RadioDebug.Checked = True
            Case "INFO"
                RadioInfo.Checked = True
            Case "WARN"
                RadioWarn.Checked = True
            Case "ERROR"
                RadioError.Checked = True
            Case "FATAL"
                RadioFatal.Checked = True
            Case Else
                RadioInfo.Checked = True
        End Select

        HelpOnce("Logging")
        initted = True

    End Sub

#End Region

#Region "Help"

    Private Sub HelpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem.Click
        HelpManual("Logging")
    End Sub

    Private Sub LoggingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoggingToolStripMenuItem.Click
        HelpManual("Logging")
    End Sub

#End Region

#Region "SetLogging"

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioOff.CheckedChanged
        If Not initted Then Return
        Settings.LogLevel = "OFF"
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioDebug.CheckedChanged
        If Not initted Then Return
        Settings.LogLevel = "DEBUG"
    End Sub

    Private Sub RadioButton3_CheckedChanged(sender As Object, e As EventArgs) Handles RadioInfo.CheckedChanged
        If Not initted Then Return
        Settings.LogLevel = "INFO"
    End Sub

    Private Sub RadioButton4_CheckedChanged(sender As Object, e As EventArgs) Handles RadioWarn.CheckedChanged
        If Not initted Then Return
        Settings.LogLevel = "WARN"
    End Sub

    Private Sub RadioButton5_CheckedChanged(sender As Object, e As EventArgs) Handles RadioError.CheckedChanged
        If Not initted Then Return
        Settings.LogLevel = "ERROR"
    End Sub

    Private Sub RadioButton6_CheckedChanged(sender As Object, e As EventArgs) Handles RadioFatal.CheckedChanged
        If Not initted Then Return
        Settings.LogLevel = "FATAL"
    End Sub

#End Region

End Class
