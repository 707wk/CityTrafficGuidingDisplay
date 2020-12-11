Imports System.IO
Imports System.Text
Imports System.Xml
Imports System.Xml.Serialization

Namespace HTGS1_8
    Module FunctionHTGS1_8
#Region "字符串反列化"
        ''' <summary>
        ''' 字符串反列化
        ''' </summary>
        ''' <param name="XmlStr">不带BOM的UTF8编码字符串</param>
        ''' <returns></returns>
        Public Function Xml2Bin(ByVal XmlStr As String) As HiATMP
            Dim TmpHiATMP As HiATMP

            Try
                Dim XmlSerializer As XmlSerializer = New XmlSerializer(GetType(HiATMP))
                Using xmlStream As Stream = New MemoryStream(Encoding.UTF8.GetBytes(XmlStr))
                    Dim xmlReader As XmlReader = XmlReader.Create(xmlStream)
                    TmpHiATMP = XmlSerializer.Deserialize(xmlReader)
                End Using

            Catch ex As Exception
                sysinfo.logger.LogThis("Xml2Bin",
                                       ex.Message,
                                       Wangk.Tools.Loglevel.Level_WARN
                                       )
                Return Nothing
            End Try

            Return TmpHiATMP
        End Function
#End Region

#Region "字符串序列化"
        ''' <summary>
        ''' 字符串序列化
        ''' </summary>
        ''' <param name="Value"></param>
        ''' <returns>生成不带BOM的UTF8编码字符串</returns>
        Public Function Bin2Xml(ByVal Value As HiATMP) As String
            Dim TmpString As String = ""

            Try
                Using ms As New MemoryStream
                    Dim ns As XmlSerializerNamespaces = New XmlSerializerNamespaces()
                    ns.Add("", "") '删除命名空间
                    '添加编码属性
                    'New System.Text.UTF8Encoding(False) 不带BOM
                    Dim tmpXmlTextWriter As XmlTextWriter = New XmlTextWriter(ms, New System.Text.UTF8Encoding(False)) With {
                        .Formatting = Formatting.Indented'子节点缩进     
                    }
                    Dim sfFormatter As New XmlSerializer(GetType(HiATMP))
                    sfFormatter.Serialize(tmpXmlTextWriter, Value, ns)

                    TmpString = Encoding.UTF8.GetString(ms.ToArray)
                End Using

            Catch ex As Exception
                sysinfo.logger.LogThis("Bin2Xml",
                                       ex.Message,
                                       Wangk.Tools.Loglevel.Level_WARN
                                       )
                Return Nothing
            End Try

            Return TmpString
        End Function
#End Region
    End Module
End Namespace