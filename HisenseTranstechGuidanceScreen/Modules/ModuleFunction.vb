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
                Dim XmlSerializer As XmlSerializer = New XmlSerializer(GetType(Setting))
                sysinfo.Setting = XmlSerializer.Deserialize(fStream)
            End Using
        Catch ex As Exception
            'MsgBox(ex.Message,
            '       MsgBoxStyle.Information,
            '       "读取配置异常")
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
                Dim tmpXmlTextWriter As XmlTextWriter = New XmlTextWriter(fStream, New System.Text.UTF8Encoding(False)) With {
                    .Formatting = Formatting.Indented '子节点缩进
                }
                Dim sfFormatter As New XmlSerializer(GetType(Setting))
                sfFormatter.Serialize(tmpXmlTextWriter, sysinfo.Setting, ns)
            End Using
        Catch ex As Exception
            'MsgBox(ex.Message,
            '       MsgBoxStyle.Information,
            '       "保存配置异常")
            Return False
        End Try

        Return True
    End Function
#End Region

#Region "读取播放列表"
    ''' <summary>
    ''' 读取播放列表
    ''' </summary>
    Public Function LoadProgram() As Boolean
        System.IO.Directory.CreateDirectory("./Tmp")

        '反序列化
        Try
            Using fStream As New FileStream("./Tmp/Program.xml", FileMode.Open)
                Dim XmlSerializer As XmlSerializer = New XmlSerializer(GetType(HTGS1_8.ProgramInfo))
                sysinfo.Program = XmlSerializer.Deserialize(fStream)
            End Using
        Catch ex As Exception
            'MsgBox(ex.Message,
            '       MsgBoxStyle.Information,
            '       "读取播放列表异常")
            Return False
        End Try

        Return True
    End Function
#End Region

#Region "保存播放列表"
    ''' <summary>
    ''' 保存播放列表
    ''' </summary>
    Public Function SaveProgram() As Boolean
        Try
            System.IO.Directory.CreateDirectory("./Tmp")
            For Each i001 As String In System.IO.Directory.GetFiles("./Tmp")
                System.IO.File.Delete(i001)
            Next
        Catch ex As Exception
            PutOut(ex.Message)
        End Try

        '序列化
        Try
            Using fStream As New FileStream("./Tmp/Program.xml", FileMode.Create)
                Dim ns As XmlSerializerNamespaces = New XmlSerializerNamespaces()
                ns.Add("", "") '删除命名空间
                '添加编码属性
                Dim tmpXmlTextWriter As XmlTextWriter = New XmlTextWriter(fStream, New System.Text.UTF8Encoding(False)) With {
                    .Formatting = Formatting.Indented '子节点缩进
                }
                Dim sfFormatter As New XmlSerializer(GetType(HTGS1_8.ProgramInfo))
                sfFormatter.Serialize(tmpXmlTextWriter, sysinfo.Program, ns)
            End Using
        Catch ex As Exception
            'MsgBox(ex.Message,
            '       MsgBoxStyle.Information,
            '       "保存播放列表异常")
            Return False
        End Try

        Return True
    End Function
#End Region

#Region "日志输出"
    Public Sub PutOut(ByVal Str As String)
        sysinfo.logsCache.Enqueue($"{Now().ToString("HH:mm:ss.fff")}: {Str}{vbCrLf}")
    End Sub
#End Region

#Region "授权"
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

#Region "Key序列化"
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
#End Region

#Region "读取/保存Key"
    ''' <summary>
    ''' 读取Key
    ''' </summary>
    Public Function LoadKey() As String
        Try
            Dim Path As String = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)
            Return File.ReadAllText(Path & "\Hunan Yestech\HTGS\{e38be42d-f742-4d96-a51a-1775fb1a7a43}")
        Catch ex As Exception
            Return "+Wn+mCIwXTICbXwB39sGO2TuWFO7nqEXd2hABEsJ42SqOLEysdP0F05wS7LU2tITMyB7llD8Lj6TqcABe4TNEQkE5gRBKO2yA1WwvIJTBZTJOdnOLsoKx7Wwj+HMw6HkMMNkUWu5rERLPXdALbr5UgeWdXE2nv9/C6a3SjYk67z65JgfypfLvENWSVCDIhv37ROwC8ai52rilTXGX4Sv0mWDemZ8wjRyW88/MSGr0UrTY0ysgpjIRdTFmRxabLm0QqfleTCfyI3KG3obg7zTCFQ5uVGQ1xZ7FQFXOG7jQq09vtsysiQz1hYMtm2w1Xfl1ocBtyrs4+4="
        End Try
    End Function

    ''' <summary>
    ''' 保存Key
    ''' </summary>
    Public Sub SaveKey(ByVal Str As String)
        Try
            Dim Path As String = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)
            System.IO.Directory.CreateDirectory(Path & "\Hunan Yestech")
            System.IO.Directory.CreateDirectory(Path & "\Hunan Yestech\HTGS")
            System.IO.File.WriteAllText(Path & "\Hunan Yestech\HTGS\{e38be42d-f742-4d96-a50a-1775fb1a7a43}", Str)
        Catch ex As Exception
            PutOut(ex.Message)
        End Try
    End Sub
#End Region

#Region "获取密钥状态"
    ''' <summary>
    ''' 获取密钥状态
    ''' </summary>
    Public Function GetKeyState() As Boolean
        Dim tmpKeyInfo As KeyInfo
        'PutOut("获取密钥状态")
        Dim KeyStr As String = LoadKey()

        Try
            tmpKeyInfo = Xml2Key(DecryptDes("" & KeyStr, "wfl9%URt", "gVhqi0@8"))
        Catch ex As Exception
            Return False
        End Try

        ''低于授权日期
        'If tmpKeyInfo.StartDate > Now Then
        '    'PutOut("本地时间错误")
        '    Return False
        '    Exit Function
        'End If

        ''时间超过
        'Dim ts001 As TimeSpan = Now - tmpKeyInfo.StartDate
        'If ts001.Days > tmpKeyInfo.Days Then
        '    'PutOut("超过使用期")
        '    Return False
        '    Exit Function
        'End If
#Region "机器码校验"
        Try
            If tmpKeyInfo.AppName <> "HTGS0978" Then
                Return False
                Exit Function
            End If

            If tmpKeyInfo.MachineCodeEnabled Then
                If tmpKeyInfo.CPUID <> Wangk.Resource.MachineCode.GetCPUID Then
                    Return False
                    Exit Function
                End If

                If tmpKeyInfo.DiskDriveID <> Wangk.Resource.MachineCode.GetDiskDriveID Then
                    Return False
                    Exit Function
                End If

                If tmpKeyInfo.NetworkMAC <> Wangk.Resource.MachineCode.GetNetworkMAC Then
                    Return False
                    Exit Function
                End If

            End If

        Catch ex As Exception
            Return False
            Exit Function
        End Try
#End Region

        '次数超过
        If tmpKeyInfo.Count < 0 Then
            'PutOut("使用次数为0")
            Return False
            Exit Function
        End If

        '每天次数减1
        Try
            Dim ts002 As TimeSpan = Now - tmpKeyInfo.LastReadDate
            If ts002.Days > 0 Then
                tmpKeyInfo.LastReadDate = Now
                tmpKeyInfo.Count -= 1

                KeyStr = EncryptDes(Key2Xml(tmpKeyInfo), "wfl9%URt", "gVhqi0@8")
                SaveKey(KeyStr)
            End If
        Catch ex As Exception
        End Try

        'PutOut("未过期")
        Return True
    End Function
#End Region

#Region "设置密钥"
    ''' <summary>
    ''' 设置密钥
    ''' </summary>
    Public Sub SetKey(ByVal Str As String)
        SaveKey(Str)

        sysinfo.KeyValid = GetKeyState()
    End Sub
#End Region
#End Region
End Module
