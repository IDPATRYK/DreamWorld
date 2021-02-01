﻿Module OAR
    Private _ForceMerge As Boolean
    Private _ForceParcel As Boolean
    Private _ForceTerrain As Boolean = True
    Private _UserName As String = ""

    Public Property PropForceMerge As Boolean
        Get
            Return _ForceMerge
        End Get
        Set(value As Boolean)
            _ForceMerge = value
        End Set
    End Property

    Public Property PropForceParcel As Boolean
        Get
            Return _ForceParcel
        End Get
        Set(value As Boolean)
            _ForceParcel = value
        End Set
    End Property

    Public Property PropForceTerrain As Boolean
        Get
            Return _ForceTerrain
        End Get
        Set(value As Boolean)
            _ForceTerrain = value
        End Set
    End Property

    Public Property PropUserName As String
        Get
            Return _UserName
        End Get
        Set(value As String)
            _UserName = value
        End Set
    End Property

    Public Sub LoadOar(RegionName As String)

        If RegionName Is Nothing Then Return
        If PropOpensimIsRunning() Then
            If RegionName.Length = 0 Then
                RegionName = ChooseRegion(True)
                If RegionName.Length = 0 Then Return
            End If

            Dim RegionUUID As String = PropRegionClass.FindRegionByName(RegionName)

            ' Create an instance of the open file dialog box. Set filter options and filter index.
            Using openFileDialog1 As OpenFileDialog = New OpenFileDialog With {
                .InitialDirectory = BackupPath(),
                .Filter = Global.Outworldz.My.Resources.OAR_Load_and_Save & "(*.OAR,*.GZ,*.TGZ)|*.oar;*.gz;*.tgz;*.OAR;*.GZ;*.TGZ|All Files (*.*)|*.*",
                .FilterIndex = 1,
                .Multiselect = False
                }

                ' Call the ShowDialog method to show the dialog box.
                Dim UserClickedOK As DialogResult = openFileDialog1.ShowDialog

                ' Process input if the user clicked OK.
                If UserClickedOK = DialogResult.OK Then

                    Dim offset = VarChooser(RegionName)

                    Dim thing = openFileDialog1.FileName
                    If thing.Length > 0 Then
                        thing = thing.Replace("\", "/")    ' because Opensim uses UNIX-like slashes, that's why

                        Dim Group = PropRegionClass.GroupName(RegionUUID)
                        For Each UUID In PropRegionClass.RegionUuidListByName(Group)

                            Dim ForceParcel As String = ""
                            If PropForceParcel() Then ForceParcel = " --force-parcels "
                            Dim ForceTerrain As String = ""
                            If PropForceTerrain Then ForceTerrain = " --force-terrain "
                            Dim ForceMerge As String = ""
                            If PropForceMerge Then ForceMerge = " --merge "
                            Dim UserName As String = ""
                            If PropUserName.Length > 0 Then UserName = " --default-user " & """" & PropUserName & """" & " "

                            ConsoleCommand(UUID, "load oar " & UserName & ForceMerge & ForceTerrain & ForceParcel & offset & """" & thing & """")
                        Next
                    End If
                End If

            End Using
        Else
            TextPrint(My.Resources.Not_Running)
        End If
    End Sub

    Public Function LoadOARContent(thing As String) As Boolean

        If Not PropOpensimIsRunning() Then
            TextPrint(My.Resources.Not_Running)
            Return False
        End If

        Dim region = ChooseRegion(True)
        If region.Length = 0 Then Return False

        Dim offset = VarChooser(region)
        If offset.Length = 0 Then Return False

        Dim backMeUp = MsgBox(My.Resources.Make_a_backup_word, vbYesNoCancel, Global.Outworldz.My.Resources.Backup_word)
        If backMeUp = vbCancel Then Return False

        Dim testRegionUUID As String = PropRegionClass.FindRegionByName(region)
        If testRegionUUID.Length = 0 Then
            MsgBox(My.Resources.Cannot_find_region_word)
            Return False
        End If
        Dim GroupName = PropRegionClass.GroupName(testRegionUUID)
        Dim once As Boolean = False
        For Each RegionUUID As String In PropRegionClass.RegionUuidListByName(GroupName)
            Try
                If Not once Then
                    TextPrint(My.Resources.Opensimulator_is_loading & " " & thing)
                    If thing IsNot Nothing Then thing = thing.Replace("\", "/")    ' because Opensim uses UNIX-like slashes, that's why

                    If backMeUp = vbYes Then
                        ConsoleCommand(RegionUUID, "change region " & region)
                        ConsoleCommand(RegionUUID, "alert " & Global.Outworldz.My.Resources.CPU_Intensive)
                        ConsoleCommand(RegionUUID, "save oar " & BackupPath() & "/" & region & "_" & DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss", Globalization.CultureInfo.InvariantCulture) & ".oar" & """")
                        ConsoleCommand(RegionUUID, "alert " & Global.Outworldz.My.Resources.New_Content)
                    End If

                    Dim ForceParcel As String = ""
                    If PropForceParcel() Then ForceParcel = " --force-parcels "
                    Dim ForceTerrain As String = ""
                    If PropForceTerrain Then ForceTerrain = " --force-terrain "
                    Dim ForceMerge As String = ""
                    If PropForceMerge Then ForceMerge = " --merge "
                    Dim UserName As String = ""
                    If PropUserName.Length > 0 Then UserName = " --default-user " & """" & PropUserName & """" & " "

                    ConsoleCommand(RegionUUID, "load oar " & UserName & ForceMerge & ForceTerrain & ForceParcel & offset & """" & thing & """")

                    once = True
                End If
            Catch ex As Exception
                BreakPoint.Show(ex.Message)
                ErrorLog(My.Resources.Error_word & ":" & ex.Message)
            End Try
            Application.DoEvents()
        Next

        Return True

    End Function

    Public Sub SaveOar(RegionName As String)

        If RegionName Is Nothing Then Return
        If PropOpensimIsRunning() Then

            If RegionName.Length = 0 Then
                RegionName = ChooseRegion(True)
                If RegionName.Length = 0 Then Return
            End If

            Dim RegionUUID As String = PropRegionClass.FindRegionByName(RegionName)

            Dim Message, title, defaultValue As String
            Dim myValue As String
            ' Set prompt.
            Message = Global.Outworldz.My.Resources.EnterName
            title = "Backup to OAR"
            defaultValue = RegionName & "_" & DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss", Globalization.CultureInfo.InvariantCulture) & ".oar"

            ' Display message, title, and default value.
            myValue = InputBox(Message, title, defaultValue)
            ' If user has clicked Cancel, set myValue to defaultValue
            If myValue.Length = 0 Then Return

            If myValue.EndsWith(".OAR", StringComparison.InvariantCulture) Or myValue.EndsWith(".oar", StringComparison.InvariantCulture) Then
                ' nothing
            Else
                myValue += ".oar"
            End If

            If PropRegionClass.IsBooted(RegionUUID) Then
                Dim Group = PropRegionClass.GroupName(RegionUUID)
                ConsoleCommand(RegionUUID, "alert CPU Intensive Backup Started")
                ConsoleCommand(RegionUUID, "change region " & RegionName)
                ConsoleCommand(RegionUUID, "save oar " & """" & BackupPath() & "/" & myValue)
            End If

            TextPrint(My.Resources.Saving_word & " " & BackupPath() & "/" & myValue)
        Else
            TextPrint(My.Resources.Not_Running)
        End If

    End Sub

End Module
