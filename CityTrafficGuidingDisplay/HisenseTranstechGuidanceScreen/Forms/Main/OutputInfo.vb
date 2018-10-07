Imports System.ComponentModel

Public Class OutputInfo
    Private Sub OutputInfo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = My.Application.Info.Description & " 日志输出"
        Me.KeyPreview = True

        Timer1.Interval = 500
        Timer2.Interval = 30 * 1000
    End Sub

    Private Sub OutputInfo_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Timer1.Start()
        Timer2.Start()
    End Sub

    ''' <summary>
    ''' 输入激活码
    ''' </summary>
    Private Sub FormMain_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Static password As String = Nothing
        password = password & Convert.ToChar(e.KeyValue)
        If password.Length > 128 Then
            password = Microsoft.VisualBasic.Right(password, 32)
        End If

        If password.ToLower().IndexOf("yestech") = -1 Then
            Exit Sub
        End If
        password = ""

        Dim tmpDialog As New RegisterDialog
        tmpDialog.ShowDialog()
    End Sub

    Private Sub OutputInfo_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        MDIParentMain.Close()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Dim TmpStr As String = ""

        Do While sysinfo.logsCache.Count > 0
            TmpStr = sysinfo.logsCache.Dequeue()

            TextBox1.AppendText(TmpStr)
        Loop
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        Dim lines As Integer = TextBox1.Height / TextBox1.Font.Height + 1
        Debug.WriteLine(TextBox1.Text.Split(vbCrLf).Count & " " & lines)

        If TextBox1.Text.Split(vbCrLf).Count > lines Then
            TextBox1.Text = TextBox1.Text.Remove(0, TextBox1.Text.IndexOf(vbCrLf) + 1)
        End If
    End Sub

#Region "检测守护程序"
    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        PutOut("检测守护程序")

        Try
            Dim ProcessList As Process() = System.Diagnostics.Process.GetProcessesByName("DaemonService")
            If ProcessList.Length = 0 Then
                Process.Start(".\DaemonService.exe")
            End If

            For Each j001 As Process In ProcessList
                j001.Dispose()
            Next
        Catch ex As Exception
            sysinfo.logger.LogThis(ex.Message, "检测守护程序", Wangk.Tools.LoggerModuleStructure.Loglevel.Level_WARN)
        End Try
    End Sub
#End Region

End Class