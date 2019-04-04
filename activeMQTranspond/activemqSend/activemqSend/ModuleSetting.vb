Module ModuleSetting
    '系统配置
    <Serializable()>
    Structure systemInfo
        '用户名
        Dim userName As String
        '密码
        Dim userPassword As String
        '服务器地址
        Dim MQServerUrl As String
        '发送对话组标题
        Dim sendTopicTitle As String
        '返回对话组标题
        Dim recTopicTitle As String

        '连接状态
        <NonSerialized()>
        Dim LinkFlage As Boolean
    End Structure
    Public sysinfo As systemInfo

End Module
