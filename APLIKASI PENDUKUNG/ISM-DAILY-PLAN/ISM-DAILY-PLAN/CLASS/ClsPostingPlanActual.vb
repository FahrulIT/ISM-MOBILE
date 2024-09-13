Imports System.Net

Public Class ClsPostingPlanActual
    Dim sqCon As New SqlClient.SqlConnection("data source=192.168.100.62\TINDEV;Database=ISM_MOBILE;MultipleActiveResultSets=True;UID=sa;Password=fid123!! ;max pool size=1000")
    Dim sqCmd As New SqlClient.SqlCommand
    Dim transaksi As SqlClient.SqlTransaction
    Dim errormsg As String = ""
    Dim StartDate As Date, EndDate As Date

    Public Sub New(ByRef PbPosting As ProgressBar)
        Try
            PbPosting.Maximum = 100
            PostingDataToWeb0()
            PostingDataToWeb1(PbPosting)
        Catch ex As Exception
            Throw New System.Exception(ex.Message)
        End Try
    End Sub

    Private Sub PostingDataToWeb0()
        Try
            sqCon.Open()
            sqCmd.Connection = sqCon
            transaksi = sqCon.BeginTransaction
            sqCmd.Transaction = transaksi
            sqCmd.CommandType = CommandType.StoredProcedure
            sqCmd.CommandTimeout = 10000
            sqCmd.CommandText = "DX_Download_Inv_FG_Qty"
            sqCmd.Parameters.Clear()

            Dim Helper As New ClsConnection
            'for double check in case data from previous month is missing or not update
            If DateTime.Now.Day = 1 Then
                Dim PrevDate As Date = Now.AddDays(-1)
                StartDate = CDate(New DateTime(PrevDate.Year, PrevDate.Month, 1))
                EndDate = CDate(New DateTime(PrevDate.Year, PrevDate.Month, PrevDate.Day))
                sqCmd.Parameters.Add("@End_to", SqlDbType.Date).Value = Format(EndDate, "yyyy-MM-dd")
            End If
            StartDate = CDate(New DateTime(DateTime.Now.Year, DateTime.Now.Month, 1))
            EndDate = DateTime.Now
            sqCmd.Parameters.Add("@End_to", SqlDbType.Date).Value = Format(EndDate, "yyyy-MM-dd")
            sqCmd.ExecuteNonQuery()

            transaksi.Commit()
        Catch ex As Exception
            Try
                transaksi.Rollback()
            Catch ex2 As Exception
                errormsg = "ROLLBACKissue" & Environment.NewLine & ex2.Message.ToString()
            End Try
            Throw New System.Exception(ex.Message & Environment.NewLine & errormsg)
        Finally
            errormsg = ""
            sqCon.Close()
        End Try
    End Sub
    Private Sub PostingDataToWeb1(ByRef PbPosting As ProgressBar)
        Try
            System.Threading.Thread.Sleep(500)
            PbPosting.Value = 0
            System.Threading.Thread.Sleep(500)
            PbPosting.Value = 10
            System.Threading.Thread.Sleep(500)
            sqCon.Open()
            sqCmd.Connection = sqCon
            transaksi = sqCon.BeginTransaction
            sqCmd.Transaction = transaksi
            sqCmd.CommandType = CommandType.StoredProcedure
            sqCmd.CommandTimeout = 10000
            sqCmd.CommandText = "DX_Download_Actual_Qty"
            sqCmd.Parameters.Clear()

            ''''''''FOR TEST ONLY''''''''
            'StartDate = New DateTime(2023, 11, 1)
            'EndDate = New DateTime(2023, 11, 30)
            'sqCmd.Parameters.Add("@Start_FROM", SqlDbType.Date).Value = Format(StartDate, "yyyy-MM-dd")
            'sqCmd.Parameters.Add("@End_to", SqlDbType.Date).Value = Format(EndDate, "yyyy-MM-dd")
            'sqCmd.Parameters.Add("@IP", SqlDbType.Date).Value = SetIpAdress()
            ''''''''FOR TEST ONLY''''''''

            Dim Helper As New ClsConnection
            PbPosting.Value = 20
            System.Threading.Thread.Sleep(500)
            PbPosting.Value = 50
            System.Threading.Thread.Sleep(500)
            'for double check in case data from previous month is missing or not update
            If DateTime.Now.Day = 1 Then
                Dim PrevDate As Date = Now.AddDays(-1)
                StartDate = CDate(New DateTime(PrevDate.Year, PrevDate.Month, 1))
                EndDate = CDate(New DateTime(PrevDate.Year, PrevDate.Month, PrevDate.Day))
                sqCmd.Parameters.Add("@Start_FROM", SqlDbType.Date).Value = Format(StartDate, "yyyy-MM-dd")
                sqCmd.Parameters.Add("@End_to", SqlDbType.Date).Value = Format(EndDate, "yyyy-MM-dd")
                sqCmd.Parameters.Add("@Ip_Address", SqlDbType.Char).Value = Helper.SetIpAdress()
            End If
            PbPosting.Value = 70
            System.Threading.Thread.Sleep(500)
            StartDate = CDate(New DateTime(DateTime.Now.Year, DateTime.Now.Month, 1))
            EndDate = DateTime.Now
            sqCmd.Parameters.Add("@Start_FROM", SqlDbType.Date).Value = Format(StartDate, "yyyy-MM-dd")
            sqCmd.Parameters.Add("@End_to", SqlDbType.Date).Value = Format(EndDate, "yyyy-MM-dd")
            sqCmd.Parameters.Add("@Ip_Address", SqlDbType.Char).Value = Helper.SetIpAdress()

            sqCmd.ExecuteNonQuery()
            PbPosting.Value = 80
            System.Threading.Thread.Sleep(500)
            PbPosting.Value = 90
            System.Threading.Thread.Sleep(500)

            transaksi.Commit()
        Catch ex As Exception
            Try
                transaksi.Rollback()
            Catch ex2 As Exception
                errormsg = "ROLLBACKissue" & Environment.NewLine & ex2.Message.ToString()
            End Try
            Throw New System.Exception(ex.Message & Environment.NewLine & errormsg)
        Finally
            errormsg = ""
            sqCon.Close()
            PbPosting.Value = 100
        End Try
    End Sub
End Class
