﻿#Region "Copyright"

' Copyright 2014 Fred Beckhusen for outworldz.com https://opensource.org/licenses/AGPL

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

Public Class FormVoice

#Region "ScreenSize"

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

#Region "Private Methods"

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles VivoxEnable.CheckedChanged
        Settings.VivoxEnabled = VivoxEnable.Checked
        Settings.SaveSettings()
    End Sub

    Private Sub IsClosed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed

        Form1.PropViewedSettings = True
        Settings.SaveSettings()

    End Sub

    Private Sub Loaded(sender As Object, e As EventArgs) Handles Me.Load
        Me.Text = Global.Outworldz.My.Resources.Voice_Settings_Word
        VivoxEnable.Checked = Settings.VivoxEnabled
        VivoxPassword.Text = Settings.VivoxPassword
        VivoxUserName.Text = Settings.VivoxUserName
        VivoxPassword.UseSystemPasswordChar = True
        SetScreen()
        HelpOnce("Vivox")
    End Sub

    Private Sub RequestPassword_Click(sender As Object, e As EventArgs) Handles RequestPassword.Click
        Dim webAddress As String = "https://opensim.vivox.com/opensim/"
        Try
            Process.Start(webAddress)

        Catch ex As Exception

            BreakPoint.Show(ex.Message)
        End Try
    End Sub

    Private Sub RunOnBoot_Click(sender As Object, e As EventArgs) Handles RunOnBoot.Click
        HelpManual("Vivox")
    End Sub

    Private Sub ToolStripMenuItem30_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem30.Click
        HelpManual("Vivox")
    End Sub

    Private Sub VivoxPassword_Clicked(sender As Object, e As EventArgs) Handles VivoxPassword.Click
        VivoxPassword.UseSystemPasswordChar = False
    End Sub

    Private Sub VivoxPassword_TextChanged(sender As Object, e As EventArgs) Handles VivoxPassword.TextChanged
        VivoxPassword.UseSystemPasswordChar = False
        Settings.VivoxPassword = VivoxPassword.Text
        Settings.SaveSettings()
    End Sub

    Private Sub VivoxUserName_TextChanged(sender As Object, e As EventArgs) Handles VivoxUserName.TextChanged
#Disable Warning BC30456 ' 'Vivox_UserName' is not a member of 'MySettings'.
        Settings.VivoxUserName = VivoxUserName.Text
#Enable Warning BC30456 ' 'Vivox_UserName' is not a member of 'MySettings'.
        Settings.SaveSettings()
    End Sub

#End Region

End Class
