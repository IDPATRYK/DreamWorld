﻿Module FreezeThaw

#Region "FreezeThaw"

    ''' <summary>
    ''' Suspends region
    ''' </summary>
    ''' <param name="RegionUUID">RegionUUID</param>
    Public Sub PauseRegion(RegionUUID As String)

        If Settings.Smart_Start Then
            BreakPoint.Print($"Pausing {Region_Name(RegionUUID)}")
        End If

        FreezeThaw(RegionUUID, "-pid " & ProcessID(RegionUUID))

    End Sub

    ''' <summary>
    ''' Resumes Region from frozen state
    ''' </summary>
    ''' <param name="RegionUUID">RegionUUID</param>
    ''' <returns>0 if success</returns>
    Public Function ResumeRegion(RegionUUID As String) As Boolean

        If ProcessID(RegionUUID) = 0 Then
            ProcessID(RegionUUID) = GetPIDofWindow(Group_Name(RegionUUID))
            If ProcessID(RegionUUID) = 0 Then
                Return True
            End If
        End If

        FreezeThaw(RegionUUID, "-rpid " & ProcessID(RegionUUID))
        ReBoot(RegionUUID)
        TeleportAgents()
        Return True

    End Function

    Private Function FreezeThaw(RegionUUID As String, Arg As String) As Boolean

        ShowDOSWindow(GetHwnd(Group_Name(RegionUUID)), MaybeHideWindow())
        Dim result As Boolean
        Using SuspendProcess As New Process()
            Dim pi = New ProcessStartInfo With {
                .Arguments = Arg,
                .FileName = """" & IO.Path.Combine(Settings.CurrentDirectory, "NtSuspendProcess64.exe") & """"
            }

            pi.CreateNoWindow = True
            pi.WindowStyle = ProcessWindowStyle.Hidden

            SuspendProcess.StartInfo = pi

            Try
                SuspendProcess.Start()
            Catch ex As Exception
                result = True
            End Try
        End Using

        PokeRegionTimer(RegionUUID)
        PropUpdateView = True ' make form refresh

        If Arg.Contains("-rpid") Then
            If Settings.Smart_Start Then
                TextPrint($"{Region_Name(RegionUUID)} Resumed")
            End If

            RegionStatus(RegionUUID) = SIMSTATUSENUM.Booted
        Else
            If Settings.Smart_Start Then
                TextPrint($"{Region_Name(RegionUUID)} Suspended")
            End If

            RegionStatus(RegionUUID) = SIMSTATUSENUM.Suspended
        End If

        Return result

    End Function

#End Region

End Module
