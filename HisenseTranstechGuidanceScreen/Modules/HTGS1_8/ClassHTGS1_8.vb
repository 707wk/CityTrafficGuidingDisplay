Imports System.Xml.Serialization

Namespace HTGS1_8
#Region "设备信息"
#Region "光带信息"
    ''' <summary>
    ''' 光带信息
    ''' </summary>
    Public Class LINK
        ''' <summary>
        ''' 路段编号
        ''' </summary>
        <XmlAttribute>
        Public id As Integer

        ''' <summary>
        ''' 发布时间
        ''' </summary>
        <XmlAttribute>
        Public time As String

        ''' <summary>
        ''' 颜色
        ''' </summary>
        <XmlAttribute>
        Public color As Integer
    End Class
#End Region

#Region "节目信息"
#Region "文字"
    ''' <summary>
    ''' 文字
    ''' </summary>
    Public Class TextInfo
        '''' <summary>
        '''' 缓存文件
        '''' </summary>
        '<XmlIgnore>
        'Public TmpFile As String

        ''' <summary>
        ''' 字体颜色
        ''' </summary>
        <XmlIgnore>
        Public Fontcolor As Color = Drawing.Color.Red
#Region "字体颜色"
        ''' <summary>
        ''' 字体颜色 1红 2黄 3绿
        ''' </summary>
        <XmlAttribute>
        Public Property color As Integer

            Get
                Select Case Fontcolor
                    Case Drawing.Color.Red
                        Return 1
                    Case Drawing.Color.Yellow
                        Return 2
                    Case Drawing.Color.Green
                        Return 3
                End Select

                Return 0
            End Get
            Set(value As Integer)
                Select Case value
                    Case 1
                        Fontcolor = Drawing.Color.Red
                    Case 2
                        Fontcolor = Drawing.Color.Yellow
                    Case 3
                        Fontcolor = Drawing.Color.Green
                End Select
            End Set
        End Property
#End Region

#Region "字体大小"
        ''' <summary>
        ''' 字体大小
        ''' </summary>
        <XmlAttribute>
        Public Property size As Integer
            Get
                Return FontFont.Size
            End Get
            Set(value As Integer)
                FontFont = New Font(FontFont.Name, value)
            End Set
        End Property
#End Region

        ''' <summary>
        ''' 展示样式
        ''' </summary>
        <XmlAttribute>
        Public style As Integer

        ''' <summary>
        ''' 发布时间
        ''' </summary>
        <XmlIgnore>
        Public _time As DateTime
#Region "发布时间"
        ''' <summary>
        ''' 发布时间
        ''' </summary>
        <XmlAttribute>
        Public Property time As String
            Get
                Return _time.ToString("yyyy-MM-dd HH:mm:ss")
            End Get
            Set(value As String)
                _time = Convert.ToDateTime(value)
            End Set
        End Property
#End Region

        ''' <summary>
        ''' 字体
        ''' </summary>
        <XmlIgnore>
        Public FontFont As New Font("微软雅黑", 10)
#Region "字体名"
        ''' <summary>
        ''' 字体名
        ''' </summary>
        <XmlAttribute>
        Public Property font As String
            Get
                Return FontFont.Name
            End Get
            Set(value As String)
                FontFont = New Font(value, FontFont.Size)
            End Set
        End Property
#End Region

        ''' <summary>
        ''' 文字内容
        ''' </summary>
        <XmlText>
        Public Value As String
    End Class
#End Region

#Region "媒体"
    ''' <summary>
    ''' 媒体
    ''' </summary>
    Public Class MediaInfo
        '''' <summary>
        '''' 缓存文件
        '''' </summary>
        '<XmlIgnore>
        'Public TmpFile As String

        ''' <summary>
        ''' 备注
        ''' </summary>
        <XmlAttribute>
        Public name As String
        ''' <summary>
        ''' 文件 URL
        ''' </summary>
        <XmlAttribute>
        Public url As String
    End Class
#End Region

    ''' <summary>
    ''' 节目信息
    ''' </summary>
    Public Class ITEM
        ''' <summary>
        ''' 节目类型
        ''' </summary>
        <XmlAttribute>
        Public type As Integer
        ''' <summary>
        ''' 播放时间 (s)
        ''' </summary>
        <XmlAttribute>
        Public interval As Integer

        ''' <summary>
        ''' 文字
        ''' </summary>
        Public text As TextInfo
        ''' <summary>
        ''' 图片
        ''' </summary>
        Public img As MediaInfo
        ''' <summary>
        ''' 视频
        ''' </summary>
        Public video As MediaInfo
    End Class
#End Region

#Region "管理对象"
#Region "屏幕控制"
    ''' <summary>
    ''' 屏幕控制
    ''' </summary>
    Public Class CMDInfo
        ''' <summary>
        ''' 命令
        ''' </summary>
        <XmlAttribute>
        Public type As String

        ''' <summary>
        ''' 执行结果
        ''' </summary>
        <XmlAttribute>
        Public RESULT As String
    End Class
#End Region

#Region "内置参数"
    ''' <summary>
    ''' 内置参数
    ''' </summary>
    Public Class PARAInfo
        ''' <summary>
        ''' 参数名
        ''' </summary>
        <XmlAttribute>
        Public name As String
        ''' <summary>
        ''' 参数值 值为空时回读
        ''' </summary>
        <XmlAttribute>
        Public value As String
    End Class
#End Region

#Region "节目回读"
#Region "文件信息"
    ''' <summary>
    ''' 文件信息
    ''' </summary>
    Public Class FILEInfo
        ''' <summary>
        ''' 文件名
        ''' </summary>
        <XmlAttribute>
        Public name As String
    End Class
#End Region

    ''' <summary>
    ''' 节目回读
    ''' </summary>
    Public Class ECHOInfo
        <XmlAttribute>
        Public type As String

        ''' <summary>
        ''' 文件信息
        ''' </summary>
        Public FILE As FILEInfo
    End Class
#End Region

    ''' <summary>
    ''' 管理对象
    ''' </summary>
    Public Class SCREENInfo
        ''' <summary>
        ''' 屏幕控制
        ''' </summary>
        Public CMD As CMDInfo

        ''' <summary>
        ''' 内置参数
        ''' </summary>
        Public PARA As PARAInfo

        '''' <summary>
        '''' 节目回读
        '''' </summary>
        'Public ECHO As ECHOInfo
    End Class
#End Region

    ''' <summary>
    ''' 设备信息
    ''' </summary>
    Public Class VMSInfo
        ''' <summary>
        ''' 设备编号
        ''' </summary>
        <XmlAttribute>
        Public id As String
        ''' <summary>
        ''' 命令字
        ''' </summary>
        <XmlAttribute>
        Public cmdid As String

        ''' <summary>
        ''' 光带列表
        ''' </summary>
        Public LINKS As LINK()
        ''' <summary>
        ''' 节目列表
        ''' </summary>
        Public ITEMS As ITEM()

        ''' <summary>
        ''' 管理对象
        ''' </summary>
        Public SCREEN As SCREENInfo

        ''' <summary>
        ''' 执行结果
        ''' </summary>
        Public CMD As CMDInfo
        ''' <summary>
        ''' 提示信息
        ''' </summary>
        Public MSG As String
    End Class
#End Region

#Region "报文"
    ''' <summary>
    ''' 报文
    ''' </summary>
    Public Class HiATMP
#Region "类型"
        ''' <summary>
        ''' 类型
        ''' </summary>
        <XmlAttribute>
        Public Property type As String
            Get
                Return NameOf(VMS)
            End Get
            Set(value As String)
                '_type = value
            End Set
        End Property
#End Region

        ''' <summary>
        ''' 设备列表
        ''' </summary>
        <XmlElement>
        Public VMS As List(Of VMSInfo)
    End Class
#End Region

#Region "播放信息"
    ''' <summary>
    ''' 播放信息
    ''' </summary>
    Public Class ProgramInfo
        ''' <summary>
        ''' 文件列表
        ''' </summary>
        Public MediaList As ITEM()

        ''' <summary>
        ''' 播放的文件序号
        ''' </summary>
        Public MediaID As Integer
        ''' <summary>
        ''' 已播放时长
        ''' </summary>
        Public MediaTime As Integer

        ''' <summary>
        ''' 关屏
        ''' </summary>
        Public OffScreen As Integer
    End Class
#End Region

    Public Class ClassHTGS1_8

    End Class
End Namespace