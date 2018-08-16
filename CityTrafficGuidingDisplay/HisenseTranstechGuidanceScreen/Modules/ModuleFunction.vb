Imports System.IO
Imports System.Text
Imports System.Xml
Imports System.Xml.Serialization

Module ModuleFunction
#Region "读取配置"
    ''' <summary>
    ''' 读取配置
    ''' </summary>
    Public Function LoadSetting() As Boolean
        System.IO.Directory.CreateDirectory("./Data")

        '反序列化
        Try
            Using fStream As New FileStream("./Data/Setting.xml", FileMode.Open)
                Dim XmlSerializer As XmlSerializer = New XmlSerializer(GetType(SystemInfo))
                sysinfo = XmlSerializer.Deserialize(fStream)
            End Using
        Catch ex As Exception
            MsgBox(ex.Message,
                   MsgBoxStyle.Information,
                   "读取配置异常")
            Return False
        End Try

        Return True
    End Function
#End Region

#Region "保存配置"
    ''' <summary>
    ''' 保存配置
    ''' </summary>
    Public Function SaveSetting() As Boolean
        System.IO.Directory.CreateDirectory("./Data")

        '序列化
        Try
            Using fStream As New FileStream("./Data/Setting.xml", FileMode.Create)
                Dim ns As XmlSerializerNamespaces = New XmlSerializerNamespaces()
                ns.Add("", "") '删除命名空间
                '添加编码属性
                Dim tmpXmlTextWriter As XmlTextWriter = New XmlTextWriter(fStream, Encoding.UTF8) With {
                    .Formatting = Formatting.Indented '子节点缩进
                }
                Dim sfFormatter As New XmlSerializer(GetType(SystemInfo))
                sfFormatter.Serialize(tmpXmlTextWriter, sysinfo, ns)
            End Using

        Catch ex As Exception
            MsgBox(ex.Message,
                   MsgBoxStyle.Information,
                   "保存配置异常")
            Return False
        End Try

        Return True
    End Function
#End Region
End Module
