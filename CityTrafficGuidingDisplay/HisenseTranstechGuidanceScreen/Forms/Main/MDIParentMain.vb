Imports System.ComponentModel

Public Class MDIParentMain
#Region "窗体初始化/关闭"
    Private Sub MDIParentMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
#Region "启动守护进程"
        'If System.Diagnostics.Process.GetProcessesByName("DaemonService").Length = 0 Then
        '    Process.Start(".\DaemonService.exe")
        'End If
#End Region

        LoadSetting()
        'With sysinfo
        '    .DeviceID = "430103000000100010"
        '    .MQSInfo.UserName = "cshiatmp"
        '    .MQSInfo.Password = "cshiatmp"
        '    .MQSInfo.Url = "activemq:failover:(tcp://192.168.56.2:61616,tcp://127.0.0.1:61616)?randomize=false"
        '    .MQSInfo.ReceiveTopicTitle = "HIATMP.HISENSE.VMS.NEWVMSPUB"
        '    .MQSInfo.SendTopicTitle = "HIATMP.HISENSE.VMS.NEWVMSPUBBAK"
        'End With
    End Sub

    Private Sub MDIParentMain_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        SaveSetting()
    End Sub
#End Region
End Class