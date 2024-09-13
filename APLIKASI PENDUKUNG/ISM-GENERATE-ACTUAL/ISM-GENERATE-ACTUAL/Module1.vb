Imports System.Net

Module Module1R
    'Dim sqCon As New SqlClient.SqlConnection("data source=TIN-064\SQLSERVER2014;Database=ISM_MOBILE;MultipleActiveResultSets=True;UID=nodejs;Password=nodejs ;max pool size=1000")
    Dim sqCon As New SqlClient.SqlConnection("data source=192.168.100.62\TINDEV;Database=ISM_MOBILE;MultipleActiveResultSets=True;UID=sa;Password=fid123!! ;max pool size=1000")
    'Dim sqCon As New SqlClient.SqlConnection("data source=192.168.21.10\ISTEM_CIM;Database=istem_costing;MultipleActiveResultSets=True;UID=costing;Password=ism123!! ;max pool size=1000")
    Dim sqCmd As New SqlClient.SqlCommand
    Dim transaksi As SqlClient.SqlTransaction
    Dim errormsg As String = ""


    Private Sub DownloadCurrentFgInv()
        Try
            Dim StartDate As Date, EndDate As Date

            Console.WriteLine("Downloading FG Current Inventory.....")
            sqCon.Open()
            sqCmd.Connection = sqCon
            transaksi = sqCon.BeginTransaction
            sqCmd.Transaction = transaksi
            sqCmd.CommandType = CommandType.StoredProcedure
            sqCmd.CommandTimeout = 10000
            sqCmd.CommandText = "DX_Download_Inv_FG_Qty"
            sqCmd.Parameters.Clear()

            ''''''''FOR TEST ONLY''''''''
            'StartDate = New DateTime(2023, 11, 1)
            'EndDate = New DateTime(2023, 11, 30)
            'sqCmd.Parameters.Add("@Start_FROM", SqlDbType.Date).Value = Format(StartDate, "yyyy-MM-dd")
            'sqCmd.Parameters.Add("@End_to", SqlDbType.Date).Value = Format(EndDate, "yyyy-MM-dd")
            'sqCmd.Parameters.Add("@IP", SqlDbType.Date).Value = SetIpAdress()
            ''''''''FOR TEST ONLY''''''''

            'for double check in case data from previous month is missing or not update
            If DateTime.Now.Day = 1 Then
                Dim PrevDate As Date = Now.AddDays(-1)
                StartDate = CDate(New DateTime(PrevDate.Year, PrevDate.Month, 1))
                EndDate = CDate(New DateTime(PrevDate.Year, PrevDate.Month, PrevDate.Day))
                sqCmd.Parameters.Add("@End_to", SqlDbType.Date).Value = Format(EndDate, "yyyy-MM-dd")
                sqCmd.ExecuteNonQuery()
                sqCmd.Parameters.Clear()
            End If

            StartDate = CDate(New DateTime(DateTime.Now.Year, DateTime.Now.Month, 1))
            EndDate = DateTime.Now
            sqCmd.Parameters.Add("@End_to", SqlDbType.Date).Value = Format(EndDate, "yyyy-MM-dd")
            
            Console.WriteLine("Please Wait.....")
            sqCmd.ExecuteNonQuery()
            transaksi.Commit()
            Console.WriteLine("Generate Success.....until " & Format(EndDate, "yyyy-MM-dd") & " (YYYY-MM-DD)" & Environment.NewLine)
            'Console.ReadKey()
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

    Private Sub DownloadPlanActual()
        Try
            Dim StartDate As Date, EndDate As Date

            Console.WriteLine("Generating Dx Mobile Plan.....")
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

            'for double check in case data from previous month is missing or not update
            If DateTime.Now.Day = 1 Then
                Dim PrevDate As Date = Now.AddDays(-1)
                StartDate = CDate(New DateTime(PrevDate.Year, PrevDate.Month, 1))
                EndDate = CDate(New DateTime(PrevDate.Year, PrevDate.Month, PrevDate.Day))
                sqCmd.Parameters.Add("@Start_FROM", SqlDbType.Date).Value = Format(StartDate, "yyyy-MM-dd")
                sqCmd.Parameters.Add("@End_to", SqlDbType.Date).Value = Format(EndDate, "yyyy-MM-dd")
                sqCmd.Parameters.Add("@Ip_Address", SqlDbType.Char).Value = SetIpAdress()
                sqCmd.ExecuteNonQuery()
                sqCmd.Parameters.Clear()
            End If

            StartDate = CDate(New DateTime(DateTime.Now.Year, DateTime.Now.Month, 1))
            EndDate = DateTime.Now
            sqCmd.Parameters.Add("@Start_FROM", SqlDbType.Date).Value = Format(StartDate, "yyyy-MM-dd")
            sqCmd.Parameters.Add("@End_to", SqlDbType.Date).Value = Format(EndDate, "yyyy-MM-dd")
            sqCmd.Parameters.Add("@Ip_Address", SqlDbType.Char).Value = SetIpAdress()

            Console.WriteLine("Please Wait.....")
            sqCmd.ExecuteNonQuery()
            transaksi.Commit()
            Console.WriteLine("Generate Success.....FROM " & Format(StartDate, "yyyy-MM-dd") & " TO " & Format(EndDate, "yyyy-MM-dd") & " (YYYY-MM-DD)")
            Console.WriteLine("Press any key/character to exit.....")
            'Console.ReadKey()
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

    Sub main()
        Try
            DownloadCurrentFgInv()
            DownloadPlanActual()
        Catch ex As Exception
            Console.WriteLine(ex.Message)

        End Try
       
    End Sub

    Public Function SetIpAdress() As String
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
    End Function
End Module
