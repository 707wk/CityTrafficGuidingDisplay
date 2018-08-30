Imports System.IO
Imports System.Text
Imports System.Xml
Imports System.Xml.Serialization

Module ModuleFunction
#Region "字符串反列化"
    ''' <summary>
    ''' 字符串反列化
    ''' </summary>
    ''' <param name="XmlStr">不带BOM的UTF8编码字符串</param>
    ''' <returns></returns>
    Public Function Xml2Key(ByVal XmlStr As String) As KeyInfo
        Dim TmpKeyInfo As KeyInfo

        Try
            Dim XmlSerializer As XmlSerializer = New XmlSerializer(GetType(KeyInfo))
            Using xmlStream As Stream = New MemoryStream(Encoding.UTF8.GetBytes(XmlStr))
                Dim xmlReader As XmlReader = XmlReader.Create(xmlStream)
                TmpKeyInfo = XmlSerializer.Deserialize(xmlReader)
            End Using

        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            Return Nothing
        End Try

        Return TmpKeyInfo
    End Function
#End Region

#Region "字符串序列化"
    ''' <summary>
    ''' 字符串序列化
    ''' </summary>
    ''' <param name="Value"></param>
    ''' <returns>生成不带BOM的UTF8编码字符串</returns>
    Public Function Key2Xml(ByVal Value As KeyInfo) As String
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
                Dim sfFormatter As New XmlSerializer(GetType(KeyInfo))
                sfFormatter.Serialize(tmpXmlTextWriter, Value, ns)

                TmpString = Encoding.UTF8.GetString(ms.ToArray)
            End Using

        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            Return Nothing
        End Try

        Return TmpString
    End Function
#End Region

#Region "DES对称加密"
#Region "加密"
    ''' <summary>
    ''' 加密
    ''' </summary>
    Public Function EncryptDes(ByVal SourceStr As String, ByVal myKey As String, ByVal myIV As String) As String
        Dim inputByteArray As Byte()
        inputByteArray = System.Text.Encoding.Default.GetBytes(SourceStr)

        'Dim DES As New System.Security.Cryptography.TripleDESCryptoServiceProvider'TripleDES算法
        Dim des As New System.Security.Cryptography.DESCryptoServiceProvider With {
            .Key = System.Text.Encoding.UTF8.GetBytes(myKey), 'myKey DES用8个字符，TripleDES要24个字符
            .IV = System.Text.Encoding.UTF8.GetBytes(myIV) 'myIV DES用8个字符，TripleDES要24个字符
            }
        Dim ms As New System.IO.MemoryStream
        Dim cs As New System.Security.Cryptography.CryptoStream(ms, des.CreateEncryptor(), System.Security.Cryptography.CryptoStreamMode.Write)
        Dim sw As New System.IO.StreamWriter(cs)
        sw.Write(SourceStr)
        sw.Flush()
        cs.FlushFinalBlock()
        ms.Flush()

        Return Convert.ToBase64String(ms.GetBuffer(), 0, ms.Length)
    End Function
#End Region

#Region "解密"
    ''' <summary>
    ''' 解密
    ''' </summary>
    Public Function DecryptDes(ByVal SourceStr As String, ByVal myKey As String, ByVal myIV As String) As String
        'Dim DES As New System.Security.Cryptography.TripleDESCryptoServiceProvider'TripleDES算法
        Dim des As New System.Security.Cryptography.DESCryptoServiceProvider With {
            .Key = System.Text.Encoding.UTF8.GetBytes(myKey), 'myKey DES用8个字符，TripleDES要24个字符
            .IV = System.Text.Encoding.UTF8.GetBytes(myIV) 'myIV DES用8个字符，TripleDES要24个字符
            }
        Dim buffer As Byte() = Convert.FromBase64String(SourceStr)
        Dim ms As New System.IO.MemoryStream(buffer)
        Dim cs As New System.Security.Cryptography.CryptoStream(ms, des.CreateDecryptor(), System.Security.Cryptography.CryptoStreamMode.Read)
        Dim sr As New System.IO.StreamReader(cs)

        Return sr.ReadToEnd()
    End Function
#End Region
#End Region
End Module
