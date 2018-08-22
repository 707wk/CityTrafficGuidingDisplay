Imports System.Xml.Serialization
Imports Nova.Mars.SDK

Public Module ModuleStructure
#Region "窗体信息"
    ''' <summary>
    ''' 窗体信息
    ''' </summary>
    Public Structure WindowInfo
        ''' <summary>
        ''' 位置
        ''' </summary>
        Dim Location As Point
        ''' <summary>
        ''' 大小
        ''' </summary>
        Dim Size As Size

        ''' <summary>
        ''' 窗口置顶
        ''' </summary>
        Dim TopFlage As Boolean
    End Structure
#End Region

#Region "MQ服务器信息"
    ''' <summary>
    ''' MQ服务器信息
    ''' </summary>
    Public Structure MQServerInfo
        ''' <summary>
        ''' 用户名
        ''' </summary>
        Dim UserName As String
        ''' <summary>
        ''' 密码
        ''' </summary>
        Dim Password As String
        ''' <summary>
        ''' 服务器地址
        ''' </summary>
        Dim Url As String
        ''' <summary>
        ''' 接收信息组标题
        ''' </summary>
        Dim ReceiveTopicTitle As String
        ''' <summary>
        ''' 发送信息组标题
        ''' </summary>
        Dim SendTopicTitle As String
    End Structure
#End Region

#Region "系统配置"
    ''' <summary>
    ''' 系统配置
    ''' </summary>
    Public Structure SystemInfo
#Region "系统参数"
        ''' <summary>
        ''' 连接状态
        ''' </summary>
        <XmlIgnore>
        Dim LinkFlage As Boolean

        ''' <summary>
        ''' 设备编号
        ''' </summary>
        Dim DeviceID As String

        ''' <summary>
        ''' 日志记录
        ''' </summary>
        <XmlIgnore>
        Dim logger As Wangk.Tools.Logger
#End Region

#Region "Nova参数"
        ''' <summary>
        ''' Nova窗口
        ''' </summary>
        <XmlIgnore>
        Dim NovaDialog As Nova
#End Region

#Region "运行参数"
        ''' <summary>
        ''' 窗体信息
        ''' </summary>
        Dim WindInfo As WindowInfo

        ''' <summary>
        ''' 服务器信息
        ''' </summary>
        Dim MQSInfo As MQServerInfo

        ''' <summary>
        ''' 播放信息
        ''' </summary>
        <XmlIgnore>
        Dim Program As HTGS1_8.ProgramInfo
#End Region
    End Structure
#End Region
End Module
