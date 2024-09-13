Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Net

Public Class ClsConnection
    Protected Friend konSql As SqlConnection
    Protected ConnStr As String
    Protected CmdSQL As SqlCommand
    Protected DaSQL As SqlDataAdapter
    Protected Ds As DataSet
    Protected Dt As DataTable
    Protected transaksi As SqlTransaction

    Protected Friend konOracle As OleDb.OleDbConnection
    Protected CmdOR As OleDb.OleDbCommand
    Protected DaOR As OleDb.OleDbDataAdapter

    Protected Friend Function SetIpAdress() As String
        Try
            Dim c_ip_address As String = ""
            Dim myHost As String = Dns.GetHostName
            Dim ipEntry As IPHostEntry = Dns.GetHostEntry(myHost)

            For Each tmpIpAddress As IPAddress In ipEntry.AddressList
                If tmpIpAddress.AddressFamily = Sockets.AddressFamily.InterNetwork Then
                    Dim ipAddress As String = tmpIpAddress.ToString
                    c_ip_address = ipAddress
                    Exit For
                End If
            Next
            Return c_ip_address
        Catch ex As Exception
            Throw New System.Exception(ex.Message)
        End Try
    End Function

    Protected Friend Sub CloseConn()
        If Not IsNothing(konSql) Then konSql.Close() : konSql = Nothing
    End Sub

    Protected Friend Sub CloseConnOracle()
        If Not IsNothing(konOracle) Then konOracle.Close() : konOracle = Nothing
    End Sub

    Protected Friend Function OpenConnOracle() As Boolean
        ConnStr = ConfigurationManager.ConnectionStrings("ORA").ConnectionString
        konOracle = New OleDb.OleDbConnection(ConnStr)
        konOracle.Open()

        If konOracle.State <> ConnectionState.Open Then
            Return False
        Else
            Return True
        End If
    End Function

    Protected Friend Function OpenConn() As Boolean
        ConnStr = ConfigurationManager.ConnectionStrings("LOCAL").ConnectionString
        konSql = New SqlConnection(ConnStr)
        konSql.Open()

        If konSql.State <> ConnectionState.Open Then
            Return False
        Else
            Return True
        End If
    End Function

    Protected Friend Function ExecuteQuerySQL(ByVal Query As String) As DataTable
        If Not OpenConn() Then
            MsgBox("Koneksi Gagal..!!", MsgBoxStyle.Critical, "Access Failed")
            Return Nothing
            Exit Function
        End If
        CmdSQL = New SqlCommand(Query, konSql)
        DaSQL = New SqlDataAdapter : DaSQL.SelectCommand = CmdSQL
        Ds = New Data.DataSet : DaSQL.Fill(Ds) : Dt = Ds.Tables(0) : Return Dt
        Dt = Nothing : Ds = Nothing : DaSQL = Nothing : CmdSQL = Nothing
        CloseConn()
    End Function

    Protected Friend Function ExecuteQueryOracle(ByVal Query As String) As DataTable
        If Not OpenConnOracle() Then
            MsgBox("Koneksi Gagal..!!", MsgBoxStyle.Critical, "Access Failed")
            Return Nothing
            Exit Function
        End If
        CmdOR = New OleDb.OleDbCommand(Query, konOracle)
        DaOR = New OleDb.OleDbDataAdapter : DaOR.SelectCommand = CmdOR
        Ds = New Data.DataSet : DaOR.Fill(Ds) : Dt = Ds.Tables(0) : Return Dt
        Dt = Nothing : Ds = Nothing : DaOR = Nothing : CmdOR = Nothing
        CloseConn()
    End Function

    Protected Friend Sub ExecuteNonQuery(ByVal Query() As String)
        Dim perintah As New SqlCommand
        Dim ErrorQuery As String = ""
        Dim ErrExecuteNonQuery As String = ""

        Try
            If Not OpenConn() Then
                MsgBox("Koneksi Gagal..!!", MsgBoxStyle.Critical, "Access Failed")
            End If
            perintah.Connection = konSql
            transaksi = konSql.BeginTransaction
            perintah.Transaction = transaksi

            For i = 0 To Query.Length - 1
                perintah.CommandType = CommandType.Text
                perintah.CommandTimeout = 10000
                perintah.CommandText = Query(i)
                ErrorQuery = Query(i)
                perintah.ExecuteNonQuery()
            Next
            transaksi.Commit()

        Catch ex As Exception
            Try
                transaksi.Rollback()
            Catch ex2 As Exception
                ErrExecuteNonQuery = "RollBack NonQuery" & vbCr & ex2.Message
            End Try
            Throw New System.Exception(ErrExecuteNonQuery & vbCr & "NONQUERY : " & ex.Message & vbCr & ErrorQuery)
        Finally
            CloseConn()
        End Try
    End Sub

    Protected Friend Sub ExecuteNonQuery(ByVal Query As String)
        Dim perintah As New SqlCommand
        Dim ErrorQuery As String = ""
        Dim ErrExecuteNonQuery As String = ""

        Try
            If Not OpenConn() Then
                MsgBox("Koneksi Gagal..!!", MsgBoxStyle.Critical, "Access Failed")
            End If
            perintah.Connection = konSql
            transaksi = konSql.BeginTransaction
            perintah.Transaction = transaksi

            perintah.CommandType = CommandType.Text
            perintah.CommandTimeout = 10000
            perintah.CommandText = Query
            ErrorQuery = Query
            perintah.ExecuteNonQuery()

            transaksi.Commit()

        Catch ex As Exception
            Try
                transaksi.Rollback()
            Catch ex2 As Exception
                ErrExecuteNonQuery = "RollBack NonQuery" & vbCr & ex2.Message
            End Try
            Throw New System.Exception(ErrExecuteNonQuery & vbCr & "NONQUERY : " & ex.Message & vbCr & ErrorQuery)
        Finally
            CloseConn()
        End Try
    End Sub

    Protected Friend Function ExecuteSkalar(ByVal query As String) As Object
        Dim obj As Object
        If Not OpenConn() Then
            MsgBox("Koneksi Gagal..!!", MsgBoxStyle.Critical, "Access Failed")
            Return Nothing
            Exit Function
        End If

        CmdSQL = New SqlCommand(query, konSql)
        obj = CmdSQL.ExecuteScalar()
        CloseConn()
        Return obj
        CmdSQL = Nothing
    End Function
End Class

Public Module Helper
    Public UserId As String = ""
    Public IpAddress As String = ""
    Public Dept As String = ""
    'Public ListItemNotFound As New List(Of String)()
End Module