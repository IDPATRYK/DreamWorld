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

Public Class FormServerType

#Region "Private Fields"

    Dim BaseHostName As String = ""
    Dim Changed As Boolean
    Dim DNSName As String = ""
    Dim initted As Boolean
    Dim ServerType As String = ""

#End Region

#Region "ScreenSize"

    Private ReadOnly Handler As New EventHandler(AddressOf Resize_page)

    'The following detects  the location of the form in screen coordinates
    Private _screenPosition As ScreenPos

    Public Property ScreenPosition As ScreenPos
        Get
            Return _screenPosition
        End Get
        Set(value As ScreenPos)
            _screenPosition = value
        End Set
    End Property

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

    Private Sub Form_exit() Handles Me.Closed
        If Changed Then
            Dim result = MsgBox(My.Resources.Save_changes_word, MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground)
            If result = vbYes Then
                FormSetup.PropViewedSettings = True
                SaveAll()
                DoGridCommon()
            End If
        End If
    End Sub

    Private Sub Loaded(sender As Object, e As EventArgs) Handles Me.Load

        GridRegionButton.Text = Global.Outworldz.My.Resources.Region_Server_word
        GridServerButton.Text = Global.Outworldz.My.Resources.Grid_Server_With_Robust_word
        GroupBox1.Text = Global.Outworldz.My.Resources.Server_Type_word
        HelpToolStripMenuItem.Image = Global.Outworldz.My.Resources.question_and_answer
        HelpToolStripMenuItem.Text = Global.Outworldz.My.Resources.Help_word
        MetroRadioButton2.Text = Global.Outworldz.My.Resources.MetroOrg
        SaveButton.Text = Global.Outworldz.My.Resources.Save_word
        ServerTypeToolStripMenuItem.Image = Global.Outworldz.My.Resources.about
        ServerTypeToolStripMenuItem.Text = Global.Outworldz.My.Resources.Server_Type_word
        Text = Global.Outworldz.My.Resources.Server_Type_word
        osGridRadioButton1.Text = Global.Outworldz.My.Resources.OSGrid_Region_Server

        SetScreen()

        Select Case Settings.ServerType
            Case "Robust"
                GridServerButton.Checked = True
            Case "Region"
                GridRegionButton.Checked = True
            Case "OsGrid"
                osGridRadioButton1.Checked = True
            Case "Metro"
                MetroRadioButton2.Checked = True
            Case Else
                GridServerButton.Checked = True
        End Select

        initted = True

        HelpOnce("ServerType")
    End Sub

    Private Sub SaveAll()

        Settings.ServerType = ServerType
        Settings.BaseHostName = BaseHostName
        Settings.DNSName = DNSName

        FormSetup.PropViewedSettings = True
        Settings.SaveSettings()
        Changed = False ' do not trigger the save a second time

    End Sub

#End Region

#Region "Radio Buttons"

    Private Sub GridRegionButton_CheckedChanged_1(sender As Object, e As EventArgs) Handles GridRegionButton.CheckedChanged

        If Not initted Then Return
        If Not GridRegionButton.Checked Then Return

        ServerType = "Region"
        ' do not override for grid servers
        Changed = True

    End Sub

    Private Sub GridServerButton_CheckedChanged_1(sender As Object, e As EventArgs) Handles GridServerButton.CheckedChanged

        If Not initted Then Return
        If Not GridServerButton.Checked Then Return

        Changed = True
        ServerType = "Robust"

    End Sub

    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs) Handles GroupBox1.Enter

    End Sub

    Private Sub MetroRadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles MetroRadioButton2.CheckedChanged

        If Not initted Then Return
        If Not MetroRadioButton2.Checked Then Return

        ServerType = "Metro"
        DNSName = "hg.metro.land"
        BaseHostName = "hg.metro.land"

        Changed = True

    End Sub

    Private Sub OsGridRadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles osGridRadioButton1.CheckedChanged

        If Not initted Then Return
        If Not osGridRadioButton1.Checked Then Return

        ServerType = "OsGrid"
        DNSName = "hg.osgrid.org"
        BaseHostName = "hg.osgrid.org"

        Changed = True

    End Sub

    Private Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click

        SaveAll()
        Close()

    End Sub

    Private Sub ServerTypeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ServerTypeToolStripMenuItem.Click

        HelpManual("ServerType")

    End Sub

#End Region

End Class
