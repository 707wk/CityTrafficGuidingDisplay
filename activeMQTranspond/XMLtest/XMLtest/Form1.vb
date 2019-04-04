Imports System.Xml

Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim XMLstr As String = "<?xml version=""1.0"" encoding=""UTF-8""?>
<HiATMP type=""VMS"">
<VMS id=""430103000000100010"">
<ITEMS>
<ITEM interval=""10"" type=""1"">
<img url=""ftp://zhdd:zhdd123@43.5.18.7/vms4.0temp/vmsftp_1488190780521_mkimg.jpg"" name=""430103000000100010"">
</img>
</ITEM>
<ITEM type=""0"" interval=""5"">
<text color="""" size="""" style=""2"" time="""" font=""1"">珍惜生命，远离酒驾</text>
</ITEM>
</ITEMS>
</VMS>
</HiATMP>"
        '"<?xml version=""1.0"" encoding=""UTF-8""?>
        '<HiATMP type=""VMS"">
        '<VMS id="""" cmdid="""">
        '<ITEMS>
        '<ITEM type=""0"" interval=""10"">
        '<text color="""" size="""" style=""1""  time="""" font=""1"">
        '雨天请注意安全
        '</text>
        '</ITEM>
        '<ITEM type=""0"" interval=""5"">
        '<text color="""" size="""" style=""2"" time="""" font=""1"">
        '珍惜生命，远离酒驾
        '</text>
        '</ITEM>
        '</ITEMS>
        '</VMS>
        '</HiATMP>"

        'Dim root As XElement = XElement.Parse(XMLstr)
        Dim doc As XmlDocument = New XmlDocument()
        doc.LoadXml(XMLstr)
        Dim rootNode As XmlNode = doc.DocumentElement

        TextBox1.AppendText($"{rootNode.Name}:{rootNode.Attributes("type").Value}{vbCrLf}")

        '遍历设备节点
        For Each HiATMPChildNode As XmlNode In rootNode.ChildNodes
            TextBox1.AppendText($"{HiATMPChildNode.Name} id:{HiATMPChildNode.Attributes("id").Value.ToString} {HiATMPChildNode.ChildNodes.Count}{vbCrLf}")

            For Each VMSChildNode As XmlNode In HiATMPChildNode.ChildNodes
                If VMSChildNode.Name = "LINKS" Then
                    '实时节目对象
                    '光带
                    TextBox1.AppendText($"LINKS{vbCrLf}")
                ElseIf VMSChildNode.Name = "ITEMS" Then
                    '实时节目对象
                    '文字/图片/视频
                    TextBox1.AppendText($"ITEMS{vbCrLf}")

                    For Each i As XmlNode In VMSChildNode.ChildNodes
                        TextBox1.AppendText($"ITEM{i.Name} {i.Attributes("type").Value}{vbCrLf}")

                        Select Case i.Attributes("type").Value
                            Case 0
                                TextBox1.AppendText($"text:[{i.FirstChild.InnerText}]{vbCrLf}")
                            Case 1
                                TextBox1.AppendText($"image:{i.FirstChild.Attributes("url").Value}{vbCrLf}")
                                'TextBox1.AppendText($"image{vbCrLf}")
                            Case 2
                                TextBox1.AppendText($"video{vbCrLf}")
                        End Select
                    Next
                ElseIf VMSChildNode.Name = "SYSTEM" Then
                    '内置节目对象
                ElseIf VMSChildNode.Name = "SCREEN" Then
                    '屏幕管理
                ElseIf VMSChildNode.Name = "ECHO" Then
                    '内置节目回读
                End If
            Next
        Next
        'Dim attList As IEnumerable(Of XAttribute) = root.Element("VMS").Element("ITEMS").Element("ITEM").Element("text").Attributes()
        'TextBox1.AppendText($"{attList.Count}{attList(0).Name} : {attList(0).Value}{vbCrLf}")
        'For Each att In attList
        '    'Console.WriteLine(att)
        '    'TextBox1.AppendText($"{att.Name}:{att.Value}{vbCrLf}")
        'Next
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)

    End Sub
End Class
