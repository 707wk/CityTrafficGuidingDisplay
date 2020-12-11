Imports System.IO
Imports System.Text
Imports System.Xml
Imports System.Xml.Serialization

Public Module ModuleStructure
#Region "密钥信息"
    ''' <summary>
    ''' 密钥信息
    ''' </summary>
    Public Class KeyInfo
        ''' <summary>
        ''' 程序名
        ''' </summary>
        Public AppName As String

#Region "机器码"
        ''' <summary>
        ''' 启用机器码
        ''' </summary>
        Public MachineCodeEnabled As Boolean

        ''' <summary>
        ''' cpu序列号
        ''' </summary>
        Public CPUID As String
        ''' <summary>
        ''' 硬盘名称
        ''' </summary>
        Public DiskDriveID As String
        ''' <summary>
        ''' 网卡MAC
        ''' </summary>
        Public NetworkMAC As String
#End Region

        ''' <summary>
        ''' 上次读取日期
        ''' </summary>
        Public LastReadDate As Date
        ''' <summary>
        ''' 总授权使用次数
        ''' </summary>
        Public LastCount As Integer
        ''' <summary>
        ''' 剩余次数(每天减1)
        ''' </summary>
        Public Count As Integer
    End Class
#End Region

#Region "系统配置"
    ''' <summary>
    ''' 系统配置
    ''' </summary>
    Public Structure SystemInfo

    End Structure
#End Region
End Module
