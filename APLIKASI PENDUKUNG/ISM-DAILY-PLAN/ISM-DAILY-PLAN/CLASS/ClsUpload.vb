Imports Excel = Microsoft.Office.Interop.Excel
Imports System.Text

Public Class ClsUpload
    Dim xlApp As Excel.Application
    Dim xlWorkBook As Excel.Workbook
    Dim xlWorkSheet As Excel.Worksheet
    Dim Konek As New ClsConnection
    Dim FirstDate As Date
    Dim LastDate As Date
    Dim QueryIntoDatabase(5000) As String
    Public ListItemNotFound As New List(Of String)()

    Public Sub New(ByVal ExcelFileSource As String, ByVal Dept As String, ByVal Kind As String, ByVal Tahun As Integer, ByVal Bulan As Integer, ByRef LoadingBar As ProgressBar)
        Try
            FirstDate = New DateTime(Tahun, Bulan, 1)
            LastDate = FirstDate.AddMonths(1).AddDays(-1)

            xlApp = New Excel.Application
            xlWorkBook = xlApp.Workbooks.Open(ExcelFileSource)
            xlWorkSheet = CType(xlWorkBook.Sheets("sheet1"), Excel.Worksheet)

            GetDataFromExcel(Dept, Kind, Tahun, Bulan, LoadingBar)

            xlWorkBook.Close(False, System.Reflection.Missing.Value, System.Reflection.Missing.Value)
            xlApp.DisplayAlerts = False

            'xlApp = Nothing
            'xlWorkBook = Nothing
            'xlWorkSheet = Nothing

        Catch ex As Exception
            Throw New System.Exception(ex.Message.ToString() & Environment.NewLine & ex.InnerException.ToString())
        Finally
            xlApp.Quit()
            releaseObject(xlApp)
            releaseObject(xlWorkSheet)
            releaseObject(xlWorkBook)
        End Try

    End Sub

    Private Function ItemNotFound(ByVal Dept As String, ByVal Dt As DataTable, ByVal ItemName As String) As Boolean
        Try
            Dim FlagCheck As Boolean = False
            Dim Dr_CheckItem As DataRow

            Dr_CheckItem = Dt.Select("ItemName = '" & ItemName & "' ").FirstOrDefault()
            If Dr_CheckItem Is Nothing Then
                FlagCheck = True
                ListItemNotFound.Add("Item : " & ItemName & " Not Found in Item Master")
            End If

            Return FlagCheck

        Catch ex As Exception
            Throw New System.Exception("FUNCTION ITEMNOTFOUND" & Environment.NewLine & ex.Message.ToString())
        End Try
    End Function

    Private Sub releaseObject(ByVal Obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(Obj)
            Obj = Nothing
        Catch ex As Exception
            Obj = Nothing
        Finally
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try
    End Sub

    Private Sub GetDataFromExcel(ByVal Dept As String, ByVal Kind As String, ByVal Tahun As Integer, ByVal Bulan As Integer, ByRef LoadingBar As ProgressBar)
        Try
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US")

            Dim rowStart As Integer = 8
            Dim rows As Integer = 0
            Dim SatuanUnit As String = ""
            Dim ItemCode As String = ""
            Dim Query As String = ""
            Dim _DeptActual As String = ""
            Dim DtItem As New DataTable

            If Dept = "SPINNING" And Kind = "Raw Material Consumption" Then
                Query = "select ItemCode, FrgnName as ItemName from CIM.istem_costing.dbo.v_SAPItem where U_Cost_ID like 'a1%' order by FrgnName"
                SatuanUnit = "LBS"
                _DeptActual = "RMC"
                DtItem = Konek.ExecuteQuerySQL(Query)
            End If

            If Dept = "SALES" And Kind = "Raw Material Warehouse" Then
                Query = "select ItemCode, FrgnName as ItemName from CIM.istem_costing.dbo.v_SAPItem where U_Cost_ID like 'a1%' order by FrgnName"
                SatuanUnit = "LBS"
                _DeptActual = "RMWH"
                DtItem = Konek.ExecuteQuerySQL(Query)
            End If


            If Dept = "SPINNING" And Kind = "Yarn Warehouse" Then
                Query = "SELECT " & _
                         "	DISTINCT b.FrgnName as ItemName, b.ItemCode " & _
                         "FROM " & _
                         "	CIM.istem_costing.dbo.tr_inv_in a " & _
                         "LEFT JOIN " & _
                         "	v_SAPItem b " & _
                         "ON " & _
                         "  a.mat_code collate database_default=b.ItemCode collate database_default " & _
                         "WHERE " & _
                         "	a.f_year>=2023 " & _
                         "	AND a.cost_sheet_id like 'a2%' " & _
                         "ORDER BY " & _
                         "	b.FrgnName"
                DtItem = Konek.ExecuteQuerySQL(Query)

                SatuanUnit = "LBS"
                _DeptActual = "SPINNING"
            End If

            If Dept = "WEAVING" And Kind = "Yarn Consumption" Then
                Query = "SELECT " & _
                             "	DISTINCT b.FrgnName as ItemName, b.ItemCode " & _
                             "FROM " & _
                             "	CIM.istem_costing.dbo.tr_inv_in a " & _
                             "LEFT JOIN " & _
                             "	CIM.istem_costing.dbo.v_SAPItem b " & _
                             "ON " & _
                             "  a.mat_code collate database_default=b.ItemCode collate database_default " & _
                             "WHERE " & _
                             "	a.f_year>=2023 " & _
                             "	AND a.cost_sheet_id like 'a2%' " & _
                             "ORDER BY " & _
                             "	b.FrgnName"

                DtItem = Konek.ExecuteQuerySQL(Query)
                SatuanUnit = "LBS"
                _DeptActual = "SPINNING"
            End If

            If Dept = "WEAVING" And Kind = "Grey Warehouse" Then
                SatuanUnit = "METER"
                _DeptActual = "WEAVING"
            End If

            If Dept = "DYEING" And Kind = "Dyeing Input" Then
                SatuanUnit = "METER"
                _DeptActual = "WEAVING"
            End If

            If Dept = "DYEING" And Kind = "Finish Good Warehouse" Then
                SatuanUnit = "METER"
                _DeptActual = "DYEING"
            End If

            If Dept = "SALES" And Kind = "Finish Good Delivery" Then
                SatuanUnit = "METER"
                _DeptActual = "DYEING"
            End If

            'Delete data and insert into history first
            Konek.ExecuteNonQuery("INSERT INTO [tr_daily_plan_qty_his] SELECT * FROM tr_daily_plan_qty WHERE year(qty_date) = '" & Tahun & "' and month(qty_date) = '" & Bulan & "' and dept = '" & Dept & "' " & _
                                  "and data_type = '" & Kind & "' and trans_type = 'PLAN'; " & _
                                  "Delete from tr_daily_plan_qty where year(qty_date) = '" & Tahun & "' and month(qty_date) = '" & Bulan & "' and dept = '" & Dept & "' " & _
                                  "and data_type = '" & Kind & "' and trans_type = 'PLAN' ")

            'get data item SAP
            Dim QueryForChecking As String = "select ItemCode, FrgnName as ItemName from CIM.istem_costing.dbo.v_SAPItem order by FrgnName"
            Dim DtItemSAP As New DataTable
            DtItemSAP = Konek.ExecuteQuerySQL(QueryForChecking)

            'get data fabric analysys master
            QueryForChecking = "select chop_no as ItemName from CIM.istem_sms.dbo.wv_fabric_analysis_master"
            Dim DtItemFAM As New DataTable
            DtItemFAM = Konek.ExecuteQuerySQL(QueryForChecking)

            QueryForChecking = "select grey_no as ItemName from CIM.istem_sms.dbo.wv_fabric_analysis_master"
            Dim DtItemGrey As New DataTable
            DtItemGrey = Konek.ExecuteQuerySQL(QueryForChecking)

            'get Item from costing (tr_inv_in)
            QueryForChecking = "Select mat_code as ItemName from CIM.istem_costing.dbo.tr_inv_in where f_year > = 2023 and cost_sheet_id like 'a3%'"
            Dim DtItemFromCosting As New DataTable
            DtItemFromCosting = Konek.ExecuteQuerySQL(QueryForChecking)

            Dim counter As Integer = 0
            Dim counter_j As Integer = 2

            With xlWorkSheet
                rows = .UsedRange.Rows.Count
                LoadingBar.Maximum = rows
                LoadingBar.Value = 0

                For i = rowStart To rows
                    If .Cells(i, 1).value = "Total" Then Exit For

                    '***Checking item validity from master'****
                    If Kind = "Raw Material Consumption" Or Kind = "Yarn Warehouse" Or Kind = "Yarn Consumption" Or Kind = "Raw Material Warehouse" Then
                        If ItemNotFound(Dept, DtItemSAP, .Cells(i, 1).Value) Then
                            Continue For
                        End If
                    End If

                    If Kind = "Grey Warehouse" Or Kind = "Dyeing Input" Then
                        If ItemNotFound(Dept, DtItemGrey, .Cells(i, 1).Value) Then
                            Continue For
                        End If
                    End If

                    If Kind = "Finish Good Warehouse" Or Kind = "Finish Good Delivery" Then
                        If ItemNotFound(Dept, DtItemFAM, .Cells(i, 1).Value) Then
                            Continue For
                        End If
                    End If
                    '***Checking item validity from master'****

                    For J = FirstDate.Day To LastDate.Day
                        Dim tanggal As Date = New Date(Tahun, Bulan, J)
                        If CInt(.Cells(i, counter_j).value) > 0 Then

                            'Search Itemcode                      
                            If Kind <> "Grey Warehouse" And Kind <> "Dyeing Input" And Kind <> "Finish Good Warehouse" And Kind <> "" And Kind <> "Finish Good Delivery" Then
                                Dim Dr As DataRow
                                Dr = DtItem.Select("ItemName = '" & .Cells(i, 1).Value & "' ").FirstOrDefault

                                If Not Dr Is Nothing Then
                                    ItemCode = Dr.Item("ItemCode")
                                Else
                                End If
                            End If

                            QueryIntoDatabase(counter) = "insert into [tr_daily_plan_qty] values ('" & Format(tanggal, "yyyy-MM-dd") & "', '" & Dept & "', '" & Kind & "', 'PLAN', '" & ItemCode & "', '" & .Cells(i, 1).Value & "', " & _
                                                            "  '" & .Cells(i, counter_j).value & "', '" & SatuanUnit & "',  '" & _DeptActual & "', '" & UserId & "', getdate(), '" & IpAddress & "'  )"
                        End If

                        counter_j = counter_j + 1
                        counter = counter + 1
                    Next
                    counter_j = 2
                    counter = counter + 1
                    LoadingBar.Value = LoadingBar.Value + 1
                Next i
            End With


            Dim lst = QueryIntoDatabase.Where(Function(s) Not String.IsNullOrEmpty(s)).ToArray()
            Konek.ExecuteNonQuery(lst)
            LoadingBar.Value = rows

            '    Dim sb As New StringBuilder()
            '    sb.Append(String.Join(Environment.NewLine, lst))
            '    System.IO.File.WriteAllText(Application.StartupPath + "\ERR.TXT", sb.ToString())
        Catch ex As Exception
            Throw New System.Exception("GETDATA" & Environment.NewLine & ex.Message.ToString() & ex.InnerException.ToString())
        End Try

    End Sub
End Class
