Imports System.ComponentModel
Imports System.IO
Imports Apache.NMS
Imports Apache.NMS.ActiveMQ

Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = My.Application.Info.Title

        'NotifyIcon1.Icon = Icon.FromHandle(My.Resources.connect3.GetHicon)
        'NotifyIcon1.ContextMenuStrip = ContextMenuStrip1
        'NotifyIcon1.Visible = True
        '反序列化
        Dim fStream As FileStream = Nothing
        Dim sfFormatter As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Try
            fStream = New FileStream("./setting.db", FileMode.Open)
        Catch ex As Exception
            'screen.ini不存在会引发异常，但没关系
        Finally
            Try
                If fStream IsNot Nothing Then
                    sysinfo = sfFormatter.Deserialize(fStream)
                End If
            Catch ex As Exception
                MsgBox($"ERROR:配置文件读取失败")
                '打开版本不同或错误的文件则无法读取
            End Try
            Try
                fStream.Close()
            Catch ex As Exception
            End Try
        End Try

        'TextBox1.AppendText($"{sysinfo.userName}{vbCrLf}{sysinfo.userPassword}{vbCrLf}{sysinfo.MQServerUrl}{vbCrLf}{sysinfo.topicTitle}")
        TextBox1.Text = sysinfo.userName
        TextBox2.Text = sysinfo.userPassword

        Try
            If sysinfo.MQServerUrl.IndexOf("activemq:") = -1 Then
                sysinfo.MQServerUrl = "activemq:" & sysinfo.MQServerUrl
            End If
        Catch ex As Exception
        End Try

        TextBox3.Text = sysinfo.MQServerUrl
        TextBox4.Text = sysinfo.recTopicTitle
        TextBox5.Text = sysinfo.sendTopicTitle

    End Sub

    Dim factory As NMSConnectionFactory
    Dim connection As IConnection
    Dim session As ISession
    Dim consumer As IMessageConsumer
    Dim producer As IMessageProducer
    '连接
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If sysinfo.LinkFlage = False Then
            If TextBox1.TextLength = 0 Or
                TextBox2.TextLength = 0 Or
                TextBox3.TextLength = 0 Or
                TextBox4.TextLength = 0 Then
                Exit Sub
            End If

            '序列化
            Try
                sysinfo.userName = TextBox1.Text
                sysinfo.userPassword = TextBox2.Text
                sysinfo.MQServerUrl = TextBox3.Text
                If sysinfo.MQServerUrl.IndexOf("activemq:") = -1 Then
                    sysinfo.MQServerUrl = "activemq:" & sysinfo.MQServerUrl
                    TextBox3.Text = sysinfo.MQServerUrl
                End If
                sysinfo.recTopicTitle = TextBox4.Text
                sysinfo.sendTopicTitle = TextBox5.Text

                Dim fStream As New FileStream("./setting.db", FileMode.Create)
                Dim sfFormatter As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
                sfFormatter.Serialize(fStream, sysinfo)
                fStream.Close()
            Catch ex As Exception
                '不知道会不会引发异常，加个保险
            End Try

            factory = New NMSConnectionFactory(sysinfo.MQServerUrl)
            connection = factory.CreateConnection(sysinfo.userName, sysinfo.userPassword)
            connection.ClientId = $"{My.Application.Info.Title}"
            connection.Start()

            AddHandler connection.ExceptionListener, AddressOf connection_ExceptionListener
            AddHandler connection.ConnectionInterruptedListener, AddressOf connection_InterruptedListener
            AddHandler connection.ConnectionResumedListener, AddressOf connection_ResumedListener

            session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge)

            Dim destRec As IDestination = session.GetTopic(sysinfo.recTopicTitle)
            consumer = session.CreateConsumer(destRec)

            Dim destSend As IDestination = session.GetTopic(sysinfo.sendTopicTitle)
            producer = session.CreateProducer(destSend)

            '绑定接收事件
            AddHandler consumer.Listener, AddressOf consumer_Listener

            Button1.Text = "断开连接"
            sysinfo.LinkFlage = True
            Timer1.Interval = 1000
            Timer1.Start()
        Else
            Timer1.Stop()
            Button1.Text = "连接MQ服务器"
            sysinfo.LinkFlage = False

            '注销接收事件
            RemoveHandler consumer.Listener, AddressOf consumer_Listener

            consumer.Close()
            producer.Close()
            session.Close()
            connection.Close()
        End If

    End Sub

    Private Sub connection_ExceptionListener(ByVal ex As Exception)
        putInfo("ExceptionListener")
    End Sub
    Private Sub connection_InterruptedListener()
        putInfo("InterruptedListener")
    End Sub
    Private Sub connection_ResumedListener()
        putInfo("ResumedListener")
    End Sub


    '显示信息
    Public Delegate Sub putInfoCallback(ByVal text As String)
    Public Sub putInfo(ByVal text As String)
        If Me.InvokeRequired Then
            Dim d As New putInfoCallback(AddressOf putInfo)
            Me.Invoke(d, New Object() {text})
        Else
            TextBox6.AppendText($"{Format(Now(), "[yyyy-MM-dd HH:mm:ss]")} Receive: *{text}*{vbCrLf}")
        End If
    End Sub

    Private Sub consumer_Listener(message As IMessage)
        Dim msg As ITextMessage = message
        putInfo(msg.Text)

        producer.Send(session.CreateTextMessage("ok"))
    End Sub

    'Private Sub NotifyIcon1_MouseDoubleClick(sender As Object, e As MouseEventArgs)
    '    'Static iconFlage As Boolean
    '    'If iconFlage Then
    '    '    iconFlage = False
    '    '    NotifyIcon1.Icon = Icon.FromHandle(My.Resources.disConnect4.GetHicon)
    '    'Else
    '    '    iconFlage = True
    '    '    NotifyIcon1.Icon = Icon.FromHandle(My.Resources.connect4.GetHicon)
    '    'End If

    'End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        'putInfo(connection.)
    End Sub
End Class
