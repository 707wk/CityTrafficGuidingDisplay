Imports System.Xml.Serialization

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

#Region "配置参数"
    ''' <summary>
    ''' 配置参数
    ''' </summary>
    Public Structure Setting
        ''' <summary>
        ''' 设备编号
        ''' </summary>
        Dim DeviceID As String

        ''' <summary>
        ''' 窗体信息
        ''' </summary>
        Dim WindInfo As WindowInfo

        ''' <summary>
        ''' 服务器信息
        ''' </summary>
        Dim MQSInfo As MQServerInfo
    End Structure
#End Region

#Region "密钥信息"
    ''' <summary>
    ''' 密钥信息
    ''' </summary>
    Public Class KeyInfo
        ''' <summary>
        ''' 程序名
        ''' </summary>
        Public AppName As String

#Region "机器码"
        ''' <summary>
        ''' 启用机器码
        ''' </summary>
        Public MachineCodeEnabled As Boolean

        ''' <summary>
        ''' cpu序列号
        ''' </summary>
        Public CPUID As String
        ''' <summary>
        ''' 硬盘名称
        ''' </summary>
        Public DiskDriveID As String
        ''' <summary>
        ''' 网卡MAC
        ''' </summary>
        Public NetworkMAC As String
#End Region

        ''' <summary>
        ''' 上次读取日期
        ''' </summary>
        Public LastReadDate As Date
        ''' <summary>
        ''' 总授权使用次数
        ''' </summary>
        Public LastCount As Integer
        ''' <summary>
        ''' 剩余次数(每天减1)
        ''' </summary>
        Public Count As Integer
    End Class
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
        Dim LinkFlage As Boolean

        ''' <summary>
        ''' 持久化日志记录
        ''' </summary>
        Dim logger As Wangk.Tools.Logger

        ''' <summary>
        ''' 临时日志
        ''' </summary>
        Dim logsCache As Queue(Of String)
#End Region

#Region "Nova参数"
        '''' <summary>
        '''' Nova窗口
        '''' </summary>
        '<XmlIgnore>
        'Dim NovaDialog As Nova
#End Region

#Region "运行参数"
        ''' <summary>
        ''' 配置信息
        ''' </summary>
        Dim Setting As Setting

        ''' <summary>
        ''' 播放信息
        ''' </summary>
        Dim Program As HTGS1_8.ProgramInfo

        ''' <summary>
        ''' 密钥有效性
        ''' </summary>
        Dim KeyValid As Boolean
#End Region
    End Structure
#End Region
End Module
