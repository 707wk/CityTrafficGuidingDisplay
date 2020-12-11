Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim tmpKeyInfo As New KeyInfo
        Try
            tmpKeyInfo = Xml2Key(DecryptDes(TextBox2.Text, "5b1EyLOt", "@7eqlB4o"))
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "机器码")
            Exit Sub
        End Try

        With tmpKeyInfo
            .AppName = "HTGS0978"
            .MachineCodeEnabled = CheckBox1.Checked
            .LastCount += NumericUpDown2.Value
            .Count = NumericUpDown2.Value
        End With

        TextBox1.Text = EncryptDes(Key2Xml(tmpKeyInfo), "wfl9%URt", "gVhqi0@8")
    End Sub
End Class
