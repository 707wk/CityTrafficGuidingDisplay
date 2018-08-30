Imports System.ComponentModel
Imports Apache.NMS

Public Class MDIParentMain
#Region "窗体初始化/关闭"
    Private Sub MDIParentMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
#Region "初始化变量"
        LoadSetting()

        LoadProgram()

        With sysinfo
            '日志
            .logger = New Wangk.Tools.Logger With {
                .writelevel = Wangk.Tools.Loglevel.Level_DEBUG,
                .saveDaysMax = 30
            }
            .logger.Init()

            .logsCache = New Queue(Of String)

            If .Program Is Nothing Then
                .Program = New HTGS1_8.ProgramInfo
            End If

            .KeyValid = True
        End With

        Timer1.Interval = 1000
        Timer2.Interval = 60 * 60 * 1000
        sysinfo.KeyValid = GetKeyState()

        PutOut("读取配置")
#End Region

#Region "启动守护进程"
        PutOut("启动守护进程")
        If System.Diagnostics.Process.GetProcessesByName("DaemonService").Length = 0 Then
            Process.Start(".\DaemonService.exe")
        End If
#End Region

#Region "样式设置"
        With NotifyIcon1
            .Text = My.Application.Info.Description
            .ContextMenuStrip = ContextMenuStrip1
        End With

        With TextBox1

        End With

        With PictureBox1

        End With

        With AxWindowsMediaPlayer1

        End With

        ScreenHide()
#End Region
    End Sub

#Region "创建Nova窗体"
    ''' <summary>
    ''' 创建Nova窗体
    ''' </summary>
    Public Sub CreatNovaThread()
        'sysinfo.NovaDialog = New Nova
        'sysinfo.NovaDialog.ShowDialog()
    End Sub
#End Region

    Private Sub MDIParentMain_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        With sysinfo.Setting.WindInfo
            Me.Location = .Location
            Me.Size = .Size
            Me.TopMost = .TopFlage

            TextBox1.Location = New Point(0, 0)
            TextBox1.Size = Me.Size

            PictureBox1.Location = New Point(0, 0)
            PictureBox1.Size = Me.Size

            AxWindowsMediaPlayer1.Location = New Point(0, 0)
            AxWindowsMediaPlayer1.Size = Me.Size
        End With

#Region "创建Nova窗体"
        'Dim TmpNova As New Threading.Thread(AddressOf CreatNovaThread) With {
        '    .IsBackground = True
        '}
        'TmpNova.Start()
#End Region

#Region "创建ActiveMQ连接"
        Dim TmpActiveMQServer As New Threading.Thread(AddressOf ActiveMQServerStart) With {
            .IsBackground = True
        }
        TmpActiveMQServer.Start()
#End Region

        If sysinfo.Program.OffScreen = 0 Then
            Timer1.Start()
        End If

        Timer2.Start()

        OutputInfo.Show()
    End Sub

    Private Sub 退出ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 退出ToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub MDIParentMain_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        ActiveMQServerStop()

        NotifyIcon1.Visible = False

        System.Environment.Exit(0)
    End Sub
#End Region

#Region "ActiveMQ收发线程"
#Region "通信变量"
    ''' <summary>
    ''' 创建
    ''' </summary>
    Dim MQFactory As NMSConnectionFactory
    ''' <summary>
    ''' 连接
    ''' </summary>
    Dim MQConnection As IConnection
    ''' <summary>
    ''' 会话
    ''' </summary>
    Dim MQSession As ISession
    ''' <summary>
    ''' 消费者
    ''' </summary>
    Dim MQConsumer As IMessageConsumer
    ''' <summary>
    ''' 生产者
    ''' </summary>
    Dim MQProducer As IMessageProducer
#End Region

#Region "ActiveMQ连接"
    Public Sub ActiveMQServerStart()
        PutOut("连接ActiveMQ")

        Try
            MQFactory = New NMSConnectionFactory(sysinfo.Setting.MQSInfo.Url)
            MQConnection = MQFactory.CreateConnection(sysinfo.Setting.MQSInfo.UserName, sysinfo.Setting.MQSInfo.Password)
            MQConnection.ClientId = $"{My.Application.Info.Title}{Guid.NewGuid.ToString}"
            MQConnection.Start()

            MQSession = MQConnection.CreateSession(AcknowledgementMode.AutoAcknowledge)

            Dim destRec As IDestination = MQSession.GetTopic(sysinfo.Setting.MQSInfo.ReceiveTopicTitle)
            MQConsumer = MQSession.CreateConsumer(destRec)

            Dim destSend As IDestination = MQSession.GetTopic(sysinfo.Setting.MQSInfo.SendTopicTitle)
            MQProducer = MQSession.CreateProducer(destSend)
        Catch ex As Exception
            PutOut(ex.Message)
        End Try

        PutOut("连接成功")

        '绑定接收事件
        AddHandler MQConnection.ExceptionListener, AddressOf MQExceptionListener
        AddHandler MQConnection.ConnectionInterruptedListener, AddressOf MQInterruptedListener
        AddHandler MQConnection.ConnectionResumedListener, AddressOf MQResumedListener
        AddHandler MQConsumer.Listener, AddressOf MQConsumerListener
    End Sub
#End Region

#Region "ActiveMQ断开"
    Public Sub ActiveMQServerStop()
        Try
            '注销接收事件
            RemoveHandler MQConsumer.Listener, AddressOf MQConsumerListener
            RemoveHandler MQConnection.ExceptionListener, AddressOf MQExceptionListener
            RemoveHandler MQConnection.ConnectionInterruptedListener, AddressOf MQInterruptedListener
            RemoveHandler MQConnection.ConnectionResumedListener, AddressOf MQResumedListener

            MQConsumer.Close()
            MQProducer.Close()
            MQSession.Close()
            MQConnection.Close()
        Catch ex As Exception
        End Try
    End Sub
#End Region

#Region "消息事件"
#Region "异常通知"
    ''' <summary>
    ''' 异常通知
    ''' </summary>
    Private Sub MQExceptionListener(ByVal Ex As Exception)
        sysinfo.logger.LogThis("监测",
                               "异常:" & Ex.Message,
                               Wangk.Tools.Loglevel.Level_WARN
                               )
        PutOut("异常:" & Ex.Message)
    End Sub
#End Region

#Region "断线通知"
    ''' <summary>
    ''' 断线通知
    ''' </summary>
    Private Sub MQInterruptedListener()
        sysinfo.logger.LogThis("监测",
                               "MQ服务断线",
                               Wangk.Tools.Loglevel.Level_WARN
                               )
        PutOut("MQ服务断线")

        Me.Invoke(New ScreenCloseCallback(AddressOf ScreenClose), New Object() {""})
    End Sub
#End Region

#Region "恢复通知"
    ''' <summary>
    ''' 恢复通知
    ''' </summary>
    Private Sub MQResumedListener()
        sysinfo.logger.LogThis("监测",
                               "MQ服务恢复",
                               Wangk.Tools.Loglevel.Level_WARN
                               )
        PutOut("MQ服务恢复")

        Me.Invoke(New ScreenOpenCallback(AddressOf ScreenOpen), New Object() {""})
    End Sub
#End Region

#Region "收到消息"
#Region "隐藏播放控件"
    ''' <summary>
    ''' 隐藏播放控件
    ''' </summary>
    Public Sub ScreenHide()
        TextBox1.Visible = False
        PictureBox1.Visible = False

        '暂停播放
        AxWindowsMediaPlayer1.Ctlcontrols.stop()
        AxWindowsMediaPlayer1.URL = ""
        AxWindowsMediaPlayer1.Visible = False
    End Sub
#End Region

#Region "播放素材"
    ''' <summary>.te
    ''' 播放素材
    ''' </summary>
    Public Sub PlayMedia()
        PutOut($"播放第{sysinfo.Program.MediaID + 1}个素材")

        ScreenHide()

        Try
            With sysinfo.Program.MediaList(sysinfo.Program.MediaID)
                If .text IsNot Nothing Then
                    '文本
                    TextBox1.ForeColor = .text.Fontcolor
                    TextBox1.Font = .text.FontFont
                    TextBox1.Text = .text.Value
                    TextBox1.Visible = True

                ElseIf .img IsNot Nothing Then
                    '图片
                    Dim TmpFile As String = System.IO.Path.GetFileName(.img.url)

                    If Not System.IO.File.Exists($"./Tmp/{TmpFile}") Then
                        PutOut($"下载图片")
                        My.Computer.Network.DownloadFile(.img.url, $"./Tmp/{TmpFile}")
                    ElseIf New System.IO.FileInfo($"./Tmp/{TmpFile}").Length = 0 Then
                        PutOut($"下载 图片")
                        System.IO.File.Delete($"./Tmp/{TmpFile}")
                        My.Computer.Network.DownloadFile(.img.url, $"./Tmp/{TmpFile}")
                    End If

                    PictureBox1.ImageLocation = $"./Tmp/{TmpFile}"
                    PictureBox1.Visible = True

                ElseIf .video IsNot Nothing Then
                    '视频
                    Dim TmpFile As String = System.IO.Path.GetFileName(.video.url)

                    If Not System.IO.File.Exists($"./Tmp/{TmpFile}") Then
                        PutOut($"下载视频")
                        My.Computer.Network.DownloadFile(.video.url, $"./Tmp/{TmpFile}")
                    ElseIf New System.IO.FileInfo($"./Tmp/{TmpFile}").Length = 0 Then
                        PutOut($"下载 视频")
                        System.IO.File.Delete($"./Tmp/{TmpFile}")
                        My.Computer.Network.DownloadFile(.video.url, $"./Tmp/{TmpFile}")
                    End If

                    AxWindowsMediaPlayer1.URL = $"./Tmp/{TmpFile}"
                    AxWindowsMediaPlayer1.Visible = True
                    AxWindowsMediaPlayer1.Ctlcontrols.play()

                Else
                    '其他操作
                End If
            End With
        Catch ex As Exception
            PutOut(ex.Message)

            sysinfo.logger.LogThis("播放素材",
                               "异常:" & ex.Message,
                               Wangk.Tools.Loglevel.Level_WARN
                               )
        End Try

    End Sub
#End Region

#Region "检测素材播放是否超时"
    ''' <summary>
    ''' 检测素材播放是否超时
    ''' </summary>
    Public Function IsMediaTimeOut() As Boolean
        With sysinfo.Program.MediaList(sysinfo.Program.MediaID)
            If .interval < sysinfo.Program.MediaTime Then
                Return True
            End If
        End With

        Return False
    End Function
#End Region

#Region "定时切换节目"
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If sysinfo.Program.OffScreen <> 0 Then
            Exit Sub
        End If

        If sysinfo.Program.MediaList Is Nothing Then
            Exit Sub
        End If

        '定时切换节目
        With sysinfo.Program
            If .MediaTime = 0 Then
                PlayMedia()
            End If
            .MediaTime += 1

            If IsMediaTimeOut() Then
                .MediaID = (.MediaID + 1) Mod .MediaList.Count
                .MediaTime = 0
            End If
        End With
    End Sub
#End Region

#Region "播放节目"
    ''' <summary>
    ''' 播放节目
    ''' </summary>
    Public Sub SetProgram(ByVal CmdID As String, ByVal Items As HTGS1_8.ITEM())
        With sysinfo.Program
            .MediaList = Items
            .MediaID = 0
            .MediaTime = 0
        End With

        Timer1.Stop()
        ScreenHide()
        SaveProgram()

        If Not sysinfo.Program.OffScreen Then
            Timer1.Start()
        End If

        MQGeneralResponse(CmdID, "0", "执行成功")
    End Sub
#End Region

#Region "管理屏幕"
#Region "开屏"
    Public Delegate Sub ScreenOpenCallback(ByVal CmdID As String)
    'Public Sub ScreenOpenCB(ByVal CmdID As String)
    '    If Me.InvokeRequired Then
    '        Me.Invoke(New ScreenOpenCallback(AddressOf ScreenOpenCB), New Object() {CmdID})
    '        Exit Sub
    '    End If

    '    ScreenOpen(CmdID)
    'End Sub
    ''' <summary>
    ''' 开屏
    ''' </summary>
    Public Sub ScreenOpen(ByVal CmdID As String)
        With sysinfo.Program
            .OffScreen = False
            .MediaTime = 0
        End With

        Timer1.Start()
        'SaveProgram()

        'MQGeneralResponse(CmdID, "0", "执行成功")
    End Sub
#End Region

#Region "关屏"
    Public Delegate Sub ScreenCloseCallback(ByVal CmdID As String)
    'Public Sub ScreenCloseCB(ByVal CmdID As String)
    '    If Me.InvokeRequired Then
    '        Me.Invoke(New ScreenCloseCallback(AddressOf ScreenCloseCB), New Object() {CmdID})
    '        Exit Sub
    '    End If

    '    ScreenClose(CmdID)
    'End Sub
    ''' <summary>
    ''' 关屏
    ''' </summary>
    Public Sub ScreenClose(ByVal CmdID As String)
        sysinfo.Program.OffScreen = True

        Timer1.Stop()

        ScreenHide()
        'SaveProgram()

        'MQGeneralResponse(CmdID, "0", "执行成功")
    End Sub
#End Region

#Region "清屏"
    ''' <summary>
    ''' 清屏
    ''' </summary>
    Public Sub ScreenClear(ByVal CmdID As String)
        sysinfo.Program.MediaList = Nothing

        ScreenHide()
        SaveProgram()

        MQGeneralResponse(CmdID, "0", "执行成功")
    End Sub
#End Region

#Region "屏幕状态"
    ''' <summary>
    ''' 屏幕状态
    ''' </summary>
    Public Sub ScreenState(ByVal CmdID As String)
        MQGeneralResponse(CmdID, $"{If(sysinfo.Program.OffScreen, 1, 0)}", "执行成功")
    End Sub
#End Region

    ''' <summary>
    ''' 管理屏幕
    ''' </summary>
    Public Sub ManageScreen(ByVal CmdID As String, ByVal Screen As HTGS1_8.SCREENInfo)
#Region "管理屏幕"
        If Screen.CMD IsNot Nothing Then
            Select Case Screen.CMD.type
                Case "on"
                    '开屏
                    PutOut("开屏")
                    ScreenOpen(CmdID)
                    MQGeneralResponse(CmdID, "0", "执行成功")

                Case "off"
                    '关屏
                    PutOut("关屏")
                    ScreenClose(CmdID)
                    MQGeneralResponse(CmdID, "0", "执行成功")

                Case "status"
                    '屏幕状态
                    PutOut("屏幕状态")
                    ScreenState(CmdID)

                Case "clear"
                    '清屏
                    PutOut("清屏")
                    ScreenClear(CmdID)

                Case Else
                    '其他操作
                    PutOut("其他操作")
                    MQGeneralResponse(CmdID, "1", "不支持的操作")
            End Select

        ElseIf Screen.PARA IsNot Nothing Then
            Select Case Screen.PARA.name
                Case "brightness"
                    If Val(Screen.PARA.value) = 0 Then
                        MQGeneralResponse(CmdID, "1", "不支持的操作")
                        Exit Sub
                    End If

                    Dim Result As Boolean
                    'sysinfo.NovaDialog.SetBrightness(Val(Screen.PARA.value) * 255 \ 16, Result)
                    MQGeneralResponse(CmdID, If(Result, "0", "1"), If(Result, "执行成功", "设置失败"))
                Case Else
                    '其他操作
                    MQGeneralResponse(CmdID, "1", "不支持的操作")
            End Select

        End If
#End Region
    End Sub
#End Region

#Region "分类处理"
    Public Delegate Sub DisposeVMSCallback(ByVal VMS As HTGS1_8.VMSInfo)
    ''' <summary>
    ''' 分类处理(GUI线程)
    ''' </summary>
    Private Sub DisposeVMS(ByVal VMS As HTGS1_8.VMSInfo)
        If Me.InvokeRequired Then
            Me.Invoke(New DisposeVMSCallback(AddressOf DisposeVMS), New Object() {VMS})
            Exit Sub
        End If

        PutOut("处理报文")

        If VMS.ITEMS IsNot Nothing Then
            '节目列表
            SetProgram(VMS.cmdid, VMS.ITEMS)

        ElseIf VMS.SCREEN IsNot Nothing Then
            '管理对象
            ManageScreen(VMS.cmdid, VMS.SCREEN)

        Else
            '其他操作
            MQGeneralResponse(VMS.cmdid, "1", "不支持的操作")
        End If
    End Sub
#End Region

#Region "响应报文"
    ''' <summary>
    ''' 响应报文
    ''' </summary>
    ''' <param name="CmdID">命令ID</param>
    ''' <param name="RESULT">结果</param>
    ''' <param name="MSG">提示信息</param>
    Public Sub MQGeneralResponse(ByVal CmdID As String, ByVal Result As String, ByVal Msg As String)
        PutOut("响应报文")

        Try
            Dim tmpHiATMP As New HTGS1_8.HiATMP
            With tmpHiATMP
                .VMS = New List(Of HTGS1_8.VMSInfo) From {
                    New HTGS1_8.VMSInfo With {
                    .id = sysinfo.Setting.DeviceID,
                    .cmdid = CmdID,
                    .CMD = New HTGS1_8.CMDInfo With {
                    .RESULT = If(sysinfo.KeyValid, Result, "1")},
                    .MSG = If(sysinfo.KeyValid, Msg, "软件信息需更新")}
                }
            End With

            MQProducer.Send(MQSession.CreateTextMessage(HTGS1_8.Bin2Xml(tmpHiATMP)))
        Catch ex As Exception
            PutOut("回复异常: " & ex.Message)
        End Try
    End Sub
#End Region

    ''' <summary>
    ''' 收到消息
    ''' </summary>
    Private Sub MQConsumerListener(ByVal Message As IMessage)
#Region "收到消息"
        Dim msg As ITextMessage = Message

        Dim tmpHiATMP As HTGS1_8.HiATMP = HTGS1_8.Xml2Bin(msg.Text)
        If tmpHiATMP Is Nothing Then
            sysinfo.logger.LogThis("未知报文",
                                   msg.Text,
                                   Wangk.Tools.Loglevel.Level_WARN
                                   )
            MQGeneralResponse("", "1", "未知报文")
            Exit Sub
        End If

        For Each i001 As HTGS1_8.VMSInfo In tmpHiATMP.VMS
            If i001.id <> sysinfo.Setting.DeviceID Then
                Continue For
            End If

            PutOut("收到报文")

            DisposeVMS(i001)
        Next
#End Region
    End Sub
#End Region
#End Region
#End Region

#Region "密钥有效性检测"
    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        sysinfo.KeyValid = GetKeyState()
    End Sub
#End Region

    '#Region "输入激活码"
    '    Private Sub 输入激活码ToolStripMenuItem_Click(sender As Object, e As EventArgs)
    '        Dim tmpDialog As New RegisterDialog
    '        tmpDialog.ShowDialog()
    '    End Sub
    '#End Region
End Class