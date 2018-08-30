Public Class RegisterDialog
    Private Sub RegisterDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim tmpKeyInfo As New KeyInfo
        Dim KeyStr As String = LoadKey()
        Try
            tmpKeyInfo = Xml2Key(DecryptDes("" & KeyStr, "wfl9%URt", "gVhqi0@8"))
        Catch ex As Exception
        End Try

        Me.Text = $"输入 MQ 升级码"

        With tmpKeyInfo
            .AppName = "HTGS0978"
            .CPUID = Wangk.Resource.MachineCode.GetCPUID
            .DiskDriveID = Wangk.Resource.MachineCode.GetDiskDriveID
            .NetworkMAC = Wangk.Resource.MachineCode.GetNetworkMAC
        End With

        TextBox1.Text = EncryptDes(Key2Xml(tmpKeyInfo), "5b1EyLOt", "@7eqlB4o")
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim tmpKeyInfoOld As KeyInfo
        Dim KeyStr As String = LoadKey()
        Try
            tmpKeyInfoOld = Xml2Key(DecryptDes("" & KeyStr, "wfl9%URt", "gVhqi0@8"))
        Catch ex As Exception
        End Try

        Dim tmpKeyInfo As KeyInfo
        Try
            tmpKeyInfo = Xml2Key(DecryptDes("" & TextBox2.Text, "wfl9%URt", "gVhqi0@8"))
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End Try

#Region "机器码校验"
        Try
            If tmpKeyInfo.AppName <> "HTGS0978" Then
                MsgBox("无效的激活码", MsgBoxStyle.Information, "输入")
                Exit Sub
            End If

            If tmpKeyInfo.MachineCodeEnabled Then
                If tmpKeyInfo.CPUID <> tmpKeyInfoOld.CPUID Then
                    MsgBox("无效的激活码", MsgBoxStyle.Information, "输入")
                    Exit Sub
                End If

                If tmpKeyInfo.DiskDriveID <> tmpKeyInfoOld.DiskDriveID Then
                    MsgBox("无效的激活码", MsgBoxStyle.Information, "输入")
                    Exit Sub
                End If

                If tmpKeyInfo.NetworkMAC <> tmpKeyInfoOld.NetworkMAC Then
                    MsgBox("无效的激活码", MsgBoxStyle.Information, "输入")
                    Exit Sub
                End If

            End If

        Catch ex As Exception
            MsgBox(ex.Message,
                   MsgBoxStyle.Information,
                   Me.Text)
            Exit Sub
        End Try
#End Region

        'PutOut(tmpKeyInfo.LastCount)
        'PutOut(tmpKeyInfo.Count)
        'PutOut(tmpKeyInfoOld.LastCount)
        'PutOut(tmpKeyInfoOld.Count)

        If tmpKeyInfo.LastCount <= tmpKeyInfoOld.LastCount Then
            MsgBox("无效的激活码", MsgBoxStyle.Information, "输入")
            Exit Sub
        End If

        'Me.Text = $"输入激活码 剩余{tmpKeyInfo.Count}天"

        SaveKey(TextBox2.Text)
        MsgBox("激活成功", MsgBoxStyle.Information, "输入")
        Me.Close()
    End Sub
End Class