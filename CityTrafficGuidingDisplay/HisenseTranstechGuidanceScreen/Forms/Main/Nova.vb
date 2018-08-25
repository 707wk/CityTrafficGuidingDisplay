Imports System.ComponentModel
Imports System.Threading
Imports Nova.Mars.SDK

Public Class Nova
#Region "Nova参数"
    '''' <summary>
    '''' Nova连接变量
    '''' </summary>
    'Dim RootClass As MarsHardwareEnumerator
    '''' <summary>
    '''' Nova配置变量
    '''' </summary>
    'Dim MainClass As MarsControlSystem
#End Region

    Private Sub Nova_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Size = New Size(0, 0)
    End Sub

    Private Sub Nova_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Me.Visible = False

        '        '启动Nova服务
        '        If System.Diagnostics.Process.GetProcessesByName("MarsServerProvider").Length = 0 Then
        '            Dim tmpProcessHwnd As Process = Process.Start($".\Nova\Server\MarsServerProvider.exe")
        '            Thread.Sleep(5000)
        '        End If

        '        '连接Nova服务
        '        RootClass = New MarsHardwareEnumerator
        '        Do
        '            If RootClass.Initialize() Then
        '                Exit Do
        '            End If

        '            Thread.Sleep(1000)
        '        Loop

        '        '查找控制系统
        '        Do
        '            If RootClass.CtrlSystemCount() > 0 Then
        '                Exit Do
        '            End If

        '            Thread.Sleep(1000)
        '        Loop

        '#Region "连接第一个串口的控制器"
        '        MainClass = New MarsControlSystem(RootClass)
        '        Dim screenCount As Integer
        '        Dim senderCount As Integer
        '        Dim tmpstr As String = Nothing

        '        Do
        '            If RootClass.GetComNameOfControlSystem(0, tmpstr) Then
        '                Exit Do
        '            End If

        '            Thread.Sleep(1000)
        '        Loop

        '        Do
        '            If MainClass.Initialize(tmpstr, screenCount, senderCount) Then
        '                Exit Do
        '            End If

        '            Thread.Sleep(1000)
        '        Loop
        '#End Region
    End Sub

    Private Sub Nova_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing

    End Sub

#Region "亮度设置"
    Public Delegate Sub GetBrightnessCallback(ByRef Value As Byte, ByRef Result As Boolean)
    ''' <summary>
    ''' 获取亮度
    ''' </summary>
    Public Sub GetBrightness(ByRef Value As Byte, ByRef Result As Boolean)
        If Me.InvokeRequired Then
            Me.Invoke(New GetBrightnessCallback(AddressOf GetBrightness), New Object() {Value, Result})
            Exit Sub
        End If

        Dim red As Byte
        Dim blue As Byte
        Dim green As Byte
        Try
            'Result = MainClass.GetBrightness(0, Value, red, blue, green)
        Catch ex As Exception
            Result = False
        End Try
    End Sub

    Public Delegate Sub SetBrightnessCallback(ByVal Value As Byte, ByRef Result As Boolean)
    ''' <summary>
    ''' 设置亮度
    ''' </summary>
    Public Sub SetBrightness(ByVal Value As Integer, ByRef Result As Boolean)
        If Me.InvokeRequired Then
            Me.Invoke(New SetBrightnessCallback(AddressOf SetBrightness), New Object() {Value, Result})
            Exit Sub
        End If

        Try
            'Result = MainClass.SetBrightness(0, Value)
        Catch ex As Exception
            Result = False
        End Try
    End Sub
#End Region
End Class