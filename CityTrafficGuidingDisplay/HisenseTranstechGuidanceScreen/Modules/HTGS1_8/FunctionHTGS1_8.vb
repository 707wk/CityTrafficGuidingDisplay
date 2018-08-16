Imports System.IO
Imports System.Text
Imports System.Xml
Imports System.Xml.Serialization

Namespace HTGS1_8
    Module FunctionHTGS1_8
#Region "字符串反列化"
        Public Function Xml2Bin(ByVal XmlStr As String) As HiATMP
            Dim TmpHiATMP As HiATMP

            Try
                Dim XmlSerializer As XmlSerializer = New XmlSerializer(GetType(HiATMP))
                Using xmlStream As Stream = New MemoryStream(Encoding.UTF8.GetBytes(XmlStr))
                    Dim xmlReader As XmlReader = XmlReader.Create(xmlStream)
                    TmpHiATMP = XmlSerializer.Deserialize(xmlReader)
                End Using
            Catch ex As Exception
                Return Nothing
            End Try

            Return TmpHiATMP
        End Function
#End Region

#Region "字符串序列化"
        Public Function Bin2Xml(ByVal Value As HiATMP) As String
            Dim TmpString As String

            Try
                Dim XmlSerializer As XmlSerializer = New XmlSerializer(GetType(HiATMP))
                Dim sb As StringBuilder = New StringBuilder
                Dim sw As StringWriter = New StringWriter(sb)
                XmlSerializer.Serialize(sw, Value)
                TmpString = sb.ToString
            Catch ex As Exception
                Return Nothing
            End Try

            Return TmpString
        End Function
#End Region
    End Module
End Namespace