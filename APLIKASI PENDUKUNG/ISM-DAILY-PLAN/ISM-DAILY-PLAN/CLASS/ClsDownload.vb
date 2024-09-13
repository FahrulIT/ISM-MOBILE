Imports Excel = Microsoft.Office.Interop.Excel

Public Class ClsDownload
    Dim counter As Integer = 0
    Dim StrQuery As String = ""
    Dim konek As New ClsConnection

    Public Function ExcelColName(ByVal Col As Integer) As String
        If Col < 0 And Col > 256 Then
            MsgBox("Invalid Argument", MsgBoxStyle.Critical)
            Return Nothing
            Exit Function
        End If
        Dim i As Int16
        Dim r As Int16
        Dim S As String
        If Col <= 26 Then
            S = Chr(Col + 64)
        Else
            r = Col Mod 26
            i = System.Math.Floor(Col / 26)
            If r = 0 Then
                r = 26
                i = i - 1
            End If
            S = Chr(i + 64) & Chr(r + 64)
        End If
        ExcelColName = S
    End Function

#Region "Download to Excel"

    Public Sub New(ByVal dept As String, ByVal KindData As String, ByVal Tahun As Integer, ByVal Bulan As Integer, ByVal ExcelFile As Excel.Worksheet, ByRef LoadingBar As ProgressBar)
        Select Case dept
           
            Case Is = "SPINNING"
                Select Case KindData
                    Case Is = "Raw Material Consumption"
                        Data_RMC(dept, KindData, Tahun, Bulan, ExcelFile, LoadingBar)
                    Case Is = "Yarn Warehouse"
                        Data_YWH(dept, KindData, Tahun, Bulan, ExcelFile, LoadingBar)

                End Select

            Case Is = "WEAVING"
                Select Case KindData
                    Case Is = "Yarn Consumption"
                        Data_YC(dept, KindData, Tahun, Bulan, ExcelFile, LoadingBar)
                    Case Is = "Grey Warehouse"
                        Data_GW(dept, KindData, Tahun, Bulan, ExcelFile, LoadingBar)

                End Select

            Case Is = "DYEING"
                Select Case KindData
                    Case Is = "Dyeing Input"
                        Data_GC(dept, KindData, Tahun, Bulan, ExcelFile, LoadingBar)
                    Case Is = "Finish Good Warehouse"
                        Data_FGWH(dept, KindData, Tahun, Bulan, ExcelFile, LoadingBar)
                End Select

            Case Is = "SALES"
                Select Case KindData
                    Case Is = "Finish Good Delivery"
                        Data_FGD(dept, KindData, Tahun, Bulan, ExcelFile, LoadingBar)
                    Case Is = "Raw Material Warehouse"
                        Data_RMWH(dept, KindData, Tahun, Bulan, ExcelFile, LoadingBar)
                End Select
        End Select
    End Sub

    Private Sub Data_RMWH(ByVal dept As String, ByVal Kind As String, ByVal Tahun As Integer, ByVal Bulan As Integer, ByVal ExcelWs As Excel.Worksheet, ByRef LoadingBar As ProgressBar)
        Dim DtTemp As New DataTable
        Dim KolomSubtotAkhir As String = ""

        Dim FirstDate As Date = New DateTime(Tahun, Bulan, 1)
        Dim LastDate As Date = FirstDate.AddMonths(1).AddDays(-1)

        LoadingBar.Value = 10
        StrQuery = "SELECT " & _
                    "	ItemCode COLLATE DATABASE_DEFAULT as [ItemCode], " & _
                    "	FrgnName COLLATE DATABASE_DEFAULT as [FrgnName] " & _
                    "FROM " & _
                    "	CIM.istem_costing.dbo.v_SAPItem " & _
                    "WHERE " & _
                    "	U_Cost_ID like 'a1%' " & _
                    "UNION " & _
                    "SELECT " & _
                    "	item_code, item_name as FrgnName " & _
                    "FROM " & _
                    "	tr_daily_plan_qty " & _
                    "WHERE " & _
                    "	qty_date <= '" & Format(LastDate, "yyyy-MM-dd") & "' and dept = '" & dept & "' and data_type = '" & Kind & "' and trans_type = 'PLAN' " & _
                    "ORDER BY " & _
                    "	FrgnName"

        DtTemp = konek.ExecuteQuerySQL(StrQuery)

        Dim ListItemSpinning As List(Of ClsItem) = New List(Of ClsItem)()
        ListItemSpinning = (From dr In DtTemp.Rows
                    Select New ClsItem() With
                    {
                        .ItemCode = dr("ItemCode").ToString(),
                        .ItemName = dr("FrgnName").ToString()
                    }).ToList()

        LoadingBar.Maximum = ListItemSpinning.Count + 10

        With ExcelWs
            '.Cells.Select()
            '.Cells.Delete()
            .Cells(1, 1).Value = "RAW MATERIAL WAREHOUSE"
            .Cells(2, 1).Value = "Department :" : .Cells(2, 2).Value = ""
            .Cells(3, 1).Value = "Year :" : .Cells(3, 2).Value = Tahun
            .Cells(4, 1).Value = "Month :" : .Cells(4, 2).Value = Bulan
            .Cells(5, 1).Value = "Unit :" : .Cells(5, 2).Value = "LBS"

            'setup header column
            .Cells(7, 1).Value = "Item :"
            counter = 2

            'Setup Header Column (Tanggal)
            For i = FirstDate.Day To LastDate.Day
                .Cells(7, counter).Value = i
                counter += 1
            Next
            .Cells(7, counter).Value = "Total"

            'Setup Data column                    
            StrQuery = "SELECT " & _
                            "qty_date, item_code, item_name, SUM(QTY) AS QTY " & _
                            ", YEAR(qty_date) as [Year], MONTH(qty_date) as [Month], Day(qty_date) as [Date] " & _
                        "FROM " & _
                            "tr_daily_plan_qty " & _
                        "WHERE " & _
                            "month(qty_date) = '" & Bulan & "' and dept = '" & dept & "' and data_type= '" & Kind & "' and year(qty_date) = '" & Tahun & "' and trans_type = 'PLAN'   " & _
                       "GROUP BY  " & _
                             "qty_date, item_code, item_name"

            DtTemp = Nothing
            DtTemp = konek.ExecuteQuerySQL(StrQuery)

            Dim Counter_j As Integer = 2
            counter = 8
            Dim InitialCounterForSubtotal = counter
            Dim RowStartForTableDraw As Integer = counter - 1


            For Each ItemData In ListItemSpinning
                .Cells(counter, 1).Value = ItemData.ItemName

                For J = FirstDate.Day To LastDate.Day
                    Dim FilterRow As DataRow
                    FilterRow = DtTemp.Select("item_code = '" & ItemData.ItemCode.ToString().Trim & "' and Date = '" & J & "'  ").FirstOrDefault
                    If Not FilterRow Is Nothing Then
                        .Cells(counter, Counter_j).Value = FilterRow.Item("QTY")
                    End If
                    Counter_j += 1
                Next

                Counter_j = 2
                counter += 1
                LoadingBar.Value = LoadingBar.Value + 1
            Next

            .Cells(counter, 1).Value = "Total"

            SetupFormulaSubTotal(LastDate, ExcelWs, counter, "B", InitialCounterForSubtotal, False)
            SetupFormulaSubTotal(LastDate, ExcelWs, counter, "B", InitialCounterForSubtotal, True)

            Dim KolomAkhir = ExcelColName(LastDate.Day + 2)
            Dim BarisTerakhir As Integer = counter

            FinishingLayoutExcel(RowStartForTableDraw, KolomAkhir, BarisTerakhir, ExcelWs)

        End With

        ExcelWs.Visible = True

        StrQuery = ""
        DtTemp = Nothing

    End Sub

    Private Sub Data_RMC(ByVal dept As String, ByVal Kind As String, ByVal Tahun As Integer, ByVal Bulan As Integer, ByVal ExcelWs As Excel.Worksheet, ByRef LoadingBar As ProgressBar)
        Dim DtTemp As New DataTable
        Dim KolomSubtotAkhir As String = ""

        Dim FirstDate As Date = New DateTime(Tahun, Bulan, 1)
        Dim LastDate As Date = FirstDate.AddMonths(1).AddDays(-1)

        LoadingBar.Value = 10
        StrQuery = "SELECT " & _
                    "	ItemCode COLLATE DATABASE_DEFAULT as [ItemCode], " & _
                    "	FrgnName COLLATE DATABASE_DEFAULT as [FrgnName] " & _
                    "FROM " & _
                    "	CIM.istem_costing.dbo.v_SAPItem " & _
                    "WHERE " & _
                    "	U_Cost_ID like 'a1%' " & _
                    "UNION " & _
                    "SELECT " & _
                    "	item_code, item_name as FrgnName " & _
                    "FROM " & _
                    "	tr_daily_plan_qty " & _
                    "WHERE " & _
                    "	qty_date <= '" & Format(LastDate, "yyyy-MM-dd") & "' and dept = '" & dept & "' and data_type = '" & Kind & "' and trans_type = 'PLAN' " & _
                    "ORDER BY " & _
                    "	FrgnName"

        DtTemp = konek.ExecuteQuerySQL(StrQuery)

        Dim ListItemSpinning As List(Of ClsItem) = New List(Of ClsItem)()
        ListItemSpinning = (From dr In DtTemp.Rows
                    Select New ClsItem() With
                    {
                        .ItemCode = dr("ItemCode").ToString(),
                        .ItemName = dr("FrgnName").ToString()
                    }).ToList()

        LoadingBar.Maximum = ListItemSpinning.Count + 10

        With ExcelWs
            '.Cells.Select()
            '.Cells.Delete()
            .Cells(1, 1).Value = "RAW MATERIAL (STAPLE) CONSUMPTION"
            .Cells(2, 1).Value = "Department :" : .Cells(2, 2).Value = dept
            .Cells(3, 1).Value = "Year :" : .Cells(3, 2).Value = Tahun
            .Cells(4, 1).Value = "Month :" : .Cells(4, 2).Value = Bulan
            .Cells(5, 1).Value = "Unit :" : .Cells(5, 2).Value = "LBS"

            'setup header column
            .Cells(7, 1).Value = "Item :"
            counter = 2

            'Setup Header Column (Tanggal)
            For i = FirstDate.Day To LastDate.Day
                .Cells(7, counter).Value = i
                counter += 1
            Next
            .Cells(7, counter).Value = "Total"

            'Setup Data column                    
            StrQuery = "SELECT " & _
                            "qty_date, item_code, item_name, SUM(QTY) AS QTY " & _
                            ", YEAR(qty_date) as [Year], MONTH(qty_date) as [Month], Day(qty_date) as [Date] " & _
                        "FROM " & _
                            "tr_daily_plan_qty " & _
                        "WHERE " & _
                            "month(qty_date) = '" & Bulan & "' and dept = '" & dept & "' and data_type= '" & Kind & "' and year(qty_date) = '" & Tahun & "' and trans_type = 'PLAN'   " & _
                       "GROUP BY  " & _
                             "qty_date, item_code, item_name"

            DtTemp = Nothing
            DtTemp = konek.ExecuteQuerySQL(StrQuery)

            Dim Counter_j As Integer = 2
            counter = 8
            Dim InitialCounterForSubtotal = counter
            Dim RowStartForTableDraw As Integer = counter - 1


            For Each ItemData In ListItemSpinning
                .Cells(counter, 1).Value = ItemData.ItemName

                For J = FirstDate.Day To LastDate.Day
                    Dim FilterRow As DataRow
                    FilterRow = DtTemp.Select("item_code = '" & ItemData.ItemCode.ToString().Trim & "' and Date = '" & J & "'  ").FirstOrDefault
                    If Not FilterRow Is Nothing Then
                        .Cells(counter, Counter_j).Value = FilterRow.Item("QTY")
                    End If
                    Counter_j += 1
                Next

                Counter_j = 2
                counter += 1
                LoadingBar.Value = LoadingBar.Value + 1
            Next

            .Cells(counter, 1).Value = "Total"

            SetupFormulaSubTotal(LastDate, ExcelWs, counter, "B", InitialCounterForSubtotal, False)
            SetupFormulaSubTotal(LastDate, ExcelWs, counter, "B", InitialCounterForSubtotal, True)

            Dim KolomAkhir = ExcelColName(LastDate.Day + 2)
            Dim BarisTerakhir As Integer = counter

            FinishingLayoutExcel(RowStartForTableDraw, KolomAkhir, BarisTerakhir, ExcelWs)

        End With

        ExcelWs.Visible = True

        StrQuery = ""
        DtTemp = Nothing
    End Sub

    Private Sub Data_YWH(ByVal dept As String, ByVal Kind As String, ByVal Tahun As Integer, ByVal Bulan As Integer, ByVal ExcelWs As Excel.Worksheet, ByRef LoadingBar As ProgressBar)
        Dim DtTemp As New DataTable
        Dim KolomSubtotAkhir As String = ""

        Dim FirstDate As Date = New DateTime(Tahun, Bulan, 1)
        Dim LastDate As Date = FirstDate.AddMonths(1).AddDays(-1)
        LoadingBar.Value = 10

        StrQuery = "SELECT " & _
                    "	DISTINCT FrgnName collate database_default as itemNAME " & _
                    "FROM " & _
                    "	cim.istem_costing.dbo.tr_inv_in a " & _
                    "LEFT JOIN " & _
                    "	v_SAPItem b " & _
                    "ON " & _
                    "  a.mat_code collate database_default=b.ItemCode collate database_default " & _
                    "WHERE " & _
                    "	a.f_year>=2023 " & _
                    "	AND a.cost_sheet_id like 'a2%' " & _
                    "UNION " & _
                    "SELECT " & _
                    "	DISTINCT item_name " & _
                    "FROM " & _
                    "	tr_daily_plan_qty " & _
                    "WHERE " & _
                    "   qty_date <= '" & Format(LastDate, "yyyy-MM-dd") & "' and dept = '" & dept & "' and data_type = '" & Kind & "' and trans_type = 'PLAN' "

        DtTemp = konek.ExecuteQuerySQL(StrQuery)

        Dim ListItemSpinning As List(Of ClsItem) = New List(Of ClsItem)()

        ListItemSpinning = (From dr In DtTemp.Rows
                    Select New ClsItem() With
                    {
                        .ItemName = dr("itemNAME").ToString()
                    }).ToList()

        LoadingBar.Maximum = ListItemSpinning.Count + 10
        With ExcelWs
            '.Cells.Select()
            '.Cells.Delete()
            .Cells(1, 1).Value = "YARN WAREHOUSE PLAN"
            .Cells(2, 1).Value = "Department :" : .Cells(2, 2).Value = dept
            .Cells(3, 1).Value = "Year :" : .Cells(3, 2).Value = Tahun
            .Cells(4, 1).Value = "Month :" : .Cells(4, 2).Value = Bulan
            .Cells(5, 1).Value = "Unit :" : .Cells(5, 2).Value = "LBS"

            'setup header column
            .Cells(7, 1).Value = "Item :"
            counter = 2

            For i = FirstDate.Day To LastDate.Day
                .Cells(7, counter).Value = i
                counter += 1
            Next
            .Cells(7, counter).Value = "Total"
            StrQuery = "SELECT " & _
                            "qty_date, item_code, item_name, SUM(QTY) AS QTY " & _
                            ", YEAR(qty_date) as [Year], MONTH(qty_date) as [Month], Day(qty_date) as [Date] " & _
                        "FROM " & _
                            "tr_daily_plan_qty " & _
                        "WHERE " & _
                            "year(qty_date) = '" & Tahun & "' and month(qty_date) = '" & Bulan & "' and dept = '" & dept & "' and data_type= '" & Kind & "' and trans_type = 'PLAN' " & _
                        "GROUP BY  " & _
                            "qty_date, item_code, item_name"

            DtTemp = Nothing
            DtTemp = konek.ExecuteQuerySQL(StrQuery)

            Dim Counter_j As Integer = 2
            counter = 8
            Dim InitialCounterForSubtotal = counter
            Dim RowStartForTableDraw As Integer = counter - 1

            For Each ItemData In ListItemSpinning
                .Cells(counter, 1).Value = ItemData.ItemName

                For J = FirstDate.Day To LastDate.Day
                    Dim FilterRow As DataRow
                    FilterRow = DtTemp.Select("item_name ='" & ItemData.ItemName.ToString().Trim & "' AND Date = '" & J & "'  ").FirstOrDefault
                    If Not FilterRow Is Nothing Then
                        Dim sumQTY As Decimal = IIf(IsDBNull(DtTemp.Compute("SUM(QTY)", "item_name = '" & ItemData.ItemName.ToString().Trim & "' AND Date = '" & J & "'  ")), 0, DtTemp.Compute("SUM(QTY)", "item_name = '" & ItemData.ItemName.ToString().Trim & "' AND Date = '" & J & "'  "))
                        .Cells(counter, Counter_j).Value = sumQTY
                    End If
                    Counter_j += 1
                Next

                Counter_j = 2
                counter += 1
                LoadingBar.Value = LoadingBar.Value + 1
            Next

            .Cells(counter, 1).Value = "Total"
            SetupFormulaSubTotal(LastDate, ExcelWs, counter, "B", InitialCounterForSubtotal, False)
            SetupFormulaSubTotal(LastDate, ExcelWs, counter, "B", InitialCounterForSubtotal, True)

            Dim KolomAkhir = ExcelColName(LastDate.Day + 2)
            Dim BarisTerakhir As Integer = counter

            FinishingLayoutExcel(RowStartForTableDraw, KolomAkhir, BarisTerakhir, ExcelWs)
            'ExcelWs.UsedRange.EntireColumn.AutoFit()
        End With

        ExcelWs.Visible = True

        StrQuery = ""
        DtTemp = Nothing
    End Sub

    Private Sub Data_YC(ByVal dept As String, ByVal Kind As String, ByVal Tahun As Integer, ByVal Bulan As Integer, ByVal ExcelWs As Excel.Worksheet, ByRef LoadingBar As ProgressBar)
        Dim DtTemp As New DataTable
        Dim KolomSubtotAkhir As String = ""

        Dim FirstDate As Date = New DateTime(Tahun, Bulan, 1)
        Dim LastDate As Date = FirstDate.AddMonths(1).AddDays(-1)
        LoadingBar.Value = 10

        StrQuery = "SELECT " & _
                    "	DISTINCT b.FrgnName collate database_default as itemNAME " & _
                    "FROM " & _
                    "	cim.istem_costing.dbo.tr_inv_in a " & _
                    "LEFT JOIN " & _
                    "	CIM.istem_costing.dbo.v_SAPItem b " & _
                    "ON " & _
                    "  a.mat_code collate database_default=b.ItemCode collate database_default " & _
                    "WHERE " & _
                    "	a.f_year>=2023 " & _
                    "	AND a.cost_sheet_id like 'a2%' " & _
                    "UNION " & _
                    "SELECT " & _
                    "	DISTINCT item_name " & _
                    "FROM " & _
                    "	tr_daily_plan_qty " & _
                    "WHERE " & _
                    "   qty_date <= '" & Format(LastDate, "yyyy-MM-dd") & "' and dept = '" & dept & "' and data_type = '" & Kind & "' and trans_type = 'PLAN' "

        DtTemp = konek.ExecuteQuerySQL(StrQuery)

        Dim ListItemWV As List(Of ClsItem) = New List(Of ClsItem)()

        ListItemWV = (From dr In DtTemp.Rows
                    Select New ClsItem() With
                    {
                        .ItemName = dr("itemNAME").ToString()
                    }).ToList()

        LoadingBar.Maximum = ListItemWV.Count + 10
        With ExcelWs
            '.Cells.Select()
            '.Cells.Delete()
            .Cells(1, 1).Value = "YARN CONSUMPTION PLAN"
            .Cells(2, 1).Value = "Department :" : .Cells(2, 2).Value = dept
            .Cells(3, 1).Value = "Year :" : .Cells(3, 2).Value = Tahun
            .Cells(4, 1).Value = "Month :" : .Cells(4, 2).Value = Bulan
            .Cells(5, 1).Value = "Unit :" : .Cells(5, 2).Value = "LBS"

            'setup header column
            .Cells(7, 1).Value = "Item :"
            counter = 2

            For i = FirstDate.Day To LastDate.Day
                .Cells(7, counter).Value = i
                counter += 1
            Next
            .Cells(7, counter).Value = "Total"
            StrQuery = "SELECT " & _
                            "qty_date, item_code, item_name, SUM(QTY) AS QTY " & _
                            ", YEAR(qty_date) as [Year], MONTH(qty_date) as [Month], Day(qty_date) as [Date] " & _
                        "FROM " & _
                            "tr_daily_plan_qty " & _
                        "WHERE " & _
                            "year(qty_date) = '" & Tahun & "' and month(qty_date) = '" & Bulan & "' and dept = '" & dept & "' and data_type= '" & Kind & "' and trans_type = 'PLAN' " & _
                        "GROUP BY  " & _
                            "qty_date, item_code, item_name"

            DtTemp = Nothing
            DtTemp = konek.ExecuteQuerySQL(StrQuery)

            Dim Counter_j As Integer = 2
            counter = 8
            Dim InitialCounterForSubtotal = counter
            Dim RowStartForTableDraw As Integer = counter - 1

            For Each ItemData In ListItemWV
                .Cells(counter, 1).Value = ItemData.ItemName

                For J = FirstDate.Day To LastDate.Day
                    Dim FilterRow As DataRow
                    FilterRow = DtTemp.Select("item_name ='" & ItemData.ItemName.ToString().Trim & "' AND Date = '" & J & "'  ").FirstOrDefault
                    If Not FilterRow Is Nothing Then
                        Dim sumQTY As Decimal = IIf(IsDBNull(DtTemp.Compute("SUM(QTY)", "item_name = '" & ItemData.ItemName.ToString().Trim & "' AND Date = '" & J & "'  ")), 0, DtTemp.Compute("SUM(QTY)", "item_name = '" & ItemData.ItemName.ToString().Trim & "' AND Date = '" & J & "'  "))
                        .Cells(counter, Counter_j).Value = sumQTY
                    End If
                    Counter_j += 1
                Next

                Counter_j = 2
                counter += 1
                LoadingBar.Value = LoadingBar.Value + 1
            Next

            .Cells(counter, 1).Value = "Total"
            SetupFormulaSubTotal(LastDate, ExcelWs, counter, "B", InitialCounterForSubtotal, False)
            SetupFormulaSubTotal(LastDate, ExcelWs, counter, "B", InitialCounterForSubtotal, True)

            Dim KolomAkhir = ExcelColName(LastDate.Day + 2)
            Dim BarisTerakhir As Integer = counter

            FinishingLayoutExcel(RowStartForTableDraw, KolomAkhir, BarisTerakhir, ExcelWs)
            'ExcelWs.UsedRange.EntireColumn.AutoFit()
        End With

        ExcelWs.Visible = True

        StrQuery = ""
        DtTemp = Nothing
    End Sub

    Private Sub Data_GW(ByVal dept As String, ByVal Kind As String, ByVal Tahun As Integer, ByVal Bulan As Integer, ByVal ExcelWs As Excel.Worksheet, ByRef LoadingBar As ProgressBar)
        Dim DtTemp As New DataTable
        Dim KolomSubtotAkhir As String = ""

        Dim FirstDate As Date = New DateTime(Tahun, Bulan, 1)
        Dim LastDate As Date = FirstDate.AddMonths(1).AddDays(-1)
        LoadingBar.Value = 10
        StrQuery = "SELECT " & _
                    "	distinct mat_code as ItemName " & _
                    "FROM " & _
                    "	cim.istem_costing.dbo.tr_inv_in " & _
                    "WHERE " & _
                    "	f_year>=2023 " & _
                    "	and cost_sheet_id like 'a3%' " & _
                    "	and mat_usage='w' " & _
                    "UNION " & _
                    "SELECT " & _
                    "	DISTINCT item_name " & _
                    "FROM " & _
                    "	tr_daily_plan_qty " & _
                    "WHERE " & _
                    "	qty_date <= '" & Format(LastDate, "yyyy-MM-dd") & "' and dept = '" & dept & "' and data_type = '" & Kind & "' and trans_type = 'PLAN' "

        DtTemp = konek.ExecuteQuerySQL(StrQuery)

        Dim ListItemWV As List(Of ClsItem) = New List(Of ClsItem)()
        ListItemWV = (From dr In DtTemp.Rows
                    Select New ClsItem() With
                    {
                        .ItemName = dr("ItemName").ToString()
                    }).ToList()

        LoadingBar.Maximum = ListItemWV.Count + 10
        With ExcelWs
            '.Cells.Select()
            '.Cells.Delete()
            .Cells(1, 1).Value = "GREY WAREHOUSE PLAN"
            .Cells(2, 1).Value = "Department :" : .Cells(2, 2).Value = dept
            .Cells(3, 1).Value = "Year :" : .Cells(3, 2).Value = Tahun
            .Cells(4, 1).Value = "Month :" : .Cells(4, 2).Value = Bulan
            .Cells(5, 1).Value = "Unit :" : .Cells(5, 2).Value = "METER"

            'setup header column
            .Cells(7, 1).Value = "Item :"
            counter = 2

            For i = FirstDate.Day To LastDate.Day
                .Cells(7, counter).Value = i
                counter += 1
            Next
            .Cells(7, counter).Value = "Total"
            StrQuery = "SELECT " & _
                            "qty_date, item_code, item_name, SUM(QTY) AS QTY " & _
                            ", YEAR(qty_date) as [Year], MONTH(qty_date) as [Month], Day(qty_date) as [Date] " & _
                        "FROM " & _
                            "tr_daily_plan_qty " & _
                        "WHERE " & _
                            "dept = '" & dept & "' and data_type= 'GREY WAREHOUSE'  and year(qty_date) = '" & Tahun & "' and month(qty_date) = '" & Bulan & "' and trans_type = 'PLAN' " & _
                        "GROUP BY  " & _
                            "qty_date, item_code, item_name"

            DtTemp = Nothing
            DtTemp = konek.ExecuteQuerySQL(StrQuery)

            Dim Counter_j As Integer = 2
            counter = 8
            Dim InitialCounterForSubtotal = counter
            Dim RowStartForTableDraw As Integer = counter - 1


            For Each ItemData In ListItemWV
                .Cells(counter, 1).Value = ItemData.ItemName

                For J = FirstDate.Day To LastDate.Day
                    Dim FilterRow As DataRow
                    FilterRow = DtTemp.Select("item_name ='" & ItemData.ItemName.ToString().Trim & "' AND Date = '" & J & "'  ").FirstOrDefault
                    If Not FilterRow Is Nothing Then
                        Dim sumQTY As Decimal = IIf(IsDBNull(DtTemp.Compute("SUM(QTY)", "item_name = '" & ItemData.ItemName.ToString().Trim & "' AND Date = '" & J & "'  ")), 0, DtTemp.Compute("SUM(QTY)", "item_name = '" & ItemData.ItemName.ToString().Trim & "' AND Date = '" & J & "'  "))
                        .Cells(counter, Counter_j).Value = sumQTY
                    End If
                    Counter_j += 1
                Next

                Counter_j = 2
                counter += 1
                LoadingBar.Value = LoadingBar.Value + 1
            Next

            .Cells(counter, 1).Value = "Total"
            SetupFormulaSubTotal(LastDate, ExcelWs, counter, "B", InitialCounterForSubtotal, False)
            SetupFormulaSubTotal(LastDate, ExcelWs, counter, "B", InitialCounterForSubtotal, True)

            Dim KolomAkhir = ExcelColName(LastDate.Day + 2)
            Dim BarisTerakhir As Integer = counter

            FinishingLayoutExcel(RowStartForTableDraw, KolomAkhir, BarisTerakhir, ExcelWs)
            'ExcelWs.UsedRange.EntireColumn.AutoFit()
        End With

        ExcelWs.Visible = True

        StrQuery = ""
        DtTemp = Nothing

    End Sub

    Private Sub Data_GC(ByVal dept As String, ByVal Kind As String, ByVal Tahun As Integer, ByVal Bulan As Integer, ByVal ExcelWs As Excel.Worksheet, ByRef LoadingBar As ProgressBar)
        Dim DtTemp As New DataTable
        Dim KolomSubtotAkhir As String = ""

        Dim FirstDate As Date = New DateTime(Tahun, Bulan, 1)
        Dim LastDate As Date = FirstDate.AddMonths(1).AddDays(-1)
        LoadingBar.Value = 10
        StrQuery = "SELECT " & _
                    "	distinct mat_code as ItemName " & _
                    "FROM " & _
                    "	cim.istem_costing.dbo.tr_inv_in " & _
                    "WHERE " & _
                    "	f_year>=2023 " & _
                    "	and cost_sheet_id like 'a3%' " & _
                    "	and mat_usage='w' " & _
                    "UNION " & _
                    "SELECT " & _
                    "	DISTINCT item_name " & _
                    "FROM " & _
                    "	tr_daily_plan_qty " & _
                    "WHERE " & _
                    "	qty_date <= '" & Format(LastDate, "yyyy-MM-dd") & "' and dept = '" & dept & "' and data_type = '" & Kind & "' and trans_type = 'PLAN' "

        DtTemp = konek.ExecuteQuerySQL(StrQuery)

        Dim ListItemDY As List(Of ClsItem) = New List(Of ClsItem)()
        ListItemDY = (From dr In DtTemp.Rows
                    Select New ClsItem() With
                    {
                        .ItemName = dr("ItemName").ToString()
                    }).ToList()

        LoadingBar.Maximum = ListItemDY.Count + 10
        With ExcelWs
            '.Cells.Select()
            '.Cells.Delete()
            .Cells(1, 1).Value = "DYEING INPUT PLAN"
            .Cells(2, 1).Value = "Department :" : .Cells(2, 2).Value = dept
            .Cells(3, 1).Value = "Year :" : .Cells(3, 2).Value = Tahun
            .Cells(4, 1).Value = "Month :" : .Cells(4, 2).Value = Bulan
            .Cells(5, 1).Value = "Unit :" : .Cells(5, 2).Value = "METER"

            'setup header column
            .Cells(7, 1).Value = "Item :"
            counter = 2

            For i = FirstDate.Day To LastDate.Day
                .Cells(7, counter).Value = i
                counter += 1
            Next
            .Cells(7, counter).Value = "Total"
            StrQuery = "SELECT " & _
                            "qty_date, item_code, item_name, SUM(QTY) AS QTY " & _
                            ", YEAR(qty_date) as [Year], MONTH(qty_date) as [Month], Day(qty_date) as [Date] " & _
                        "FROM " & _
                            "tr_daily_plan_qty " & _
                        "WHERE " & _
                            "dept = '" & dept & "' and data_type= '" & Kind & "'  and year(qty_date) = '" & Tahun & "' and month(qty_date) = '" & Bulan & "' and trans_type = 'PLAN'  " & _
                        "GROUP BY  " & _
                            "qty_date, item_code, item_name"

            DtTemp = Nothing
            DtTemp = konek.ExecuteQuerySQL(StrQuery)

            Dim Counter_j As Integer = 2
            counter = 8
            Dim InitialCounterForSubtotal = counter
            Dim RowStartForTableDraw As Integer = counter - 1

            For Each ItemData In ListItemDY
                .Cells(counter, 1).Value = ItemData.ItemName

                For J = FirstDate.Day To LastDate.Day
                    Dim FilterRow As DataRow
                    FilterRow = DtTemp.Select("item_name ='" & ItemData.ItemName.ToString().Trim & "' AND Date = '" & J & "'  ").FirstOrDefault
                    If Not FilterRow Is Nothing Then
                        Dim sumQTY As Decimal = IIf(IsDBNull(DtTemp.Compute("SUM(QTY)", "item_name = '" & ItemData.ItemName.ToString().Trim & "' AND Date = '" & J & "'  ")), 0, DtTemp.Compute("SUM(QTY)", "item_name = '" & ItemData.ItemName.ToString().Trim & "' AND Date = '" & J & "'  "))
                        .Cells(counter, Counter_j).Value = sumQTY
                    End If
                    Counter_j += 1
                Next

                Counter_j = 2
                counter += 1
                LoadingBar.Value += 1
            Next

            .Cells(counter, 1).Value = "Total"
            SetupFormulaSubTotal(LastDate, ExcelWs, counter, "B", InitialCounterForSubtotal, False)
            SetupFormulaSubTotal(LastDate, ExcelWs, counter, "B", InitialCounterForSubtotal, True)

            Dim KolomAkhir = ExcelColName(LastDate.Day + 2)
            Dim BarisTerakhir As Integer = counter

            FinishingLayoutExcel(RowStartForTableDraw, KolomAkhir, BarisTerakhir, ExcelWs)
            'ExcelWs.UsedRange.EntireColumn.AutoFit()
        End With

        ExcelWs.Visible = True

        StrQuery = ""
        DtTemp = Nothing

    End Sub

    Private Sub Data_FGWH(ByVal dept As String, ByVal Kind As String, ByVal Tahun As Integer, ByVal Bulan As Integer, ByVal ExcelWs As Excel.Worksheet, ByRef LoadingBar As ProgressBar)
        Dim DtTemp As New DataTable
        Dim KolomSubtotAkhir As String = ""

        Dim FirstDate As Date = New DateTime(Tahun, Bulan, 1)
        Dim LastDate As Date = FirstDate.AddMonths(1).AddDays(-1)
        LoadingBar.Value = 10
        StrQuery = "SELECT " & _
                    "	DISTINCT b.chop_no as ItemName " & _
                    "FROM " & _
                    "	cim.istem_costing.dbo.tr_fg_balance a " & _
                    "LEFT JOIN " & _
                    "	cim.istem_sms.dbo.wv_fabric_analysis_master b " & _
                    "ON " & _
                    "	a.item_code=b.grey_no " & _
                    "WHERE " & _
                    "	a.f_year>=2023 " & _
                    "	and a.bussines_cat='f' " & _
                    "UNION " & _
                    "SELECT " & _
                    "	DISTINCT item_name " & _
                    "FROM " & _
                    "	tr_daily_plan_qty " & _
                    "WHERE " & _
                    "	qty_date <= '" & Format(LastDate, "yyyy-MM-dd") & "' and dept = '" & dept & "' and data_type = '" & Kind & "' and trans_type = 'PLAN' "

        DtTemp = konek.ExecuteQuerySQL(StrQuery)

        Dim ListItemDY As List(Of ClsItem) = New List(Of ClsItem)()
        ListItemDY = (From dr In DtTemp.Rows
                    Select New ClsItem() With
                    {
                        .ItemName = dr("ItemName").ToString()
                    }).ToList()

        LoadingBar.Maximum = ListItemDY.Count + 10
        With ExcelWs
            '.Cells.Select()
            '.Cells.Delete()
            .Cells(1, 1).Value = "FINISH GOOD WAREHOUSE PLAN"
            .Cells(2, 1).Value = "Department :" : .Cells(2, 2).Value = dept
            .Cells(3, 1).Value = "Year :" : .Cells(3, 2).Value = Tahun
            .Cells(4, 1).Value = "Month :" : .Cells(4, 2).Value = Bulan
            .Cells(5, 1).Value = "Unit :" : .Cells(5, 2).Value = "METER"

            'setup header column
            .Cells(7, 1).Value = "Item :"
            counter = 2

            For i = FirstDate.Day To LastDate.Day
                .Cells(7, counter).Value = i
                counter += 1
            Next
            .Cells(7, counter).Value = "Total"
            StrQuery = "SELECT " & _
                            "qty_date, item_code, item_name, SUM(QTY) AS QTY " & _
                            ", YEAR(qty_date) as [Year], MONTH(qty_date) as [Month], Day(qty_date) as [Date] " & _
                        "FROM " & _
                            "tr_daily_plan_qty " & _
                        "WHERE " & _
                            "dept = '" & dept & "' and data_type= '" & Kind & "'  and year(qty_date) = '" & Tahun & "' and month(qty_date) = '" & Bulan & "' and trans_type = 'PLAN'   " & _
                        "GROUP BY  " & _
                            "qty_date, item_code, item_name"

            DtTemp = Nothing
            DtTemp = konek.ExecuteQuerySQL(StrQuery)

            Dim Counter_j As Integer = 2
            counter = 8
            Dim InitialCounterForSubtotal = counter
            Dim RowStartForTableDraw As Integer = counter - 1

            For Each ItemData In ListItemDY
                .Cells(counter, 1).Value = ItemData.ItemName

                For J = FirstDate.Day To LastDate.Day
                    Dim FilterRow As DataRow
                    FilterRow = DtTemp.Select("item_name ='" & ItemData.ItemName.ToString().Trim & "' AND Date = '" & J & "'  ").FirstOrDefault
                    If Not FilterRow Is Nothing Then
                        Dim sumQTY As Decimal = IIf(IsDBNull(DtTemp.Compute("SUM(QTY)", "item_name = '" & ItemData.ItemName.ToString().Trim & "' AND Date = '" & J & "'  ")), 0, DtTemp.Compute("SUM(QTY)", "item_name = '" & ItemData.ItemName.ToString().Trim & "' AND Date = '" & J & "'  "))
                        .Cells(counter, Counter_j).Value = sumQTY
                    End If
                    Counter_j += 1
                Next

                Counter_j = 2
                counter += 1
                LoadingBar.Value += 1
            Next

            .Cells(counter, 1).Value = "Total"
            SetupFormulaSubTotal(LastDate, ExcelWs, counter, "B", InitialCounterForSubtotal, False)
            SetupFormulaSubTotal(LastDate, ExcelWs, counter, "B", InitialCounterForSubtotal, True)

            Dim KolomAkhir = ExcelColName(LastDate.Day + 2)
            Dim BarisTerakhir As Integer = counter

            FinishingLayoutExcel(RowStartForTableDraw, KolomAkhir, BarisTerakhir, ExcelWs)
            'ExcelWs.UsedRange.EntireColumn.AutoFit()
        End With

        ExcelWs.Visible = True

        StrQuery = ""
        DtTemp = Nothing

    End Sub

    Private Sub Data_FGD(ByVal dept As String, ByVal Kind As String, ByVal Tahun As Integer, ByVal Bulan As Integer, ByVal ExcelWs As Excel.Worksheet, ByRef LoadingBar As ProgressBar)
        Dim DtTemp As New DataTable
        Dim KolomSubtotAkhir As String = ""

        Dim FirstDate As Date = New DateTime(Tahun, Bulan, 1)
        Dim LastDate As Date = FirstDate.AddMonths(1).AddDays(-1)

        LoadingBar.Value = 10
        StrQuery = "SELECT " & _
                    "	DISTINCT b.chop_no as ItemName " & _
                    "FROM " & _
                    "	cim.istem_costing.dbo.tr_fg_balance a " & _
                    "LEFT JOIN " & _
                    "	cim.istem_sms.dbo.wv_fabric_analysis_master b " & _
                    "ON " & _
                    "	a.item_code=b.grey_no " & _
                    "WHERE " & _
                    "	a.f_year>=2023 " & _
                    "	and a.bussines_cat='f' " & _
                    "UNION " & _
                    "SELECT " & _
                    "	DISTINCT item_name " & _
                    "FROM " & _
                    "	tr_daily_plan_qty " & _
                    "WHERE " & _
                    "	qty_date <= '" & Format(LastDate, "yyyy-MM-dd") & "' and dept = '" & dept & "' and data_type = '" & Kind & "' and trans_type = 'PLAN' "


        DtTemp = konek.ExecuteQuerySQL(StrQuery)

        Dim ListItemSales As List(Of ClsItem) = New List(Of ClsItem)()
        ListItemSales = (From dr In DtTemp.Rows
                    Select New ClsItem() With
                    {
                        .ItemName = dr("ItemName").ToString()
                    }).ToList()

        LoadingBar.Maximum = ListItemSales.Count + 10
        With ExcelWs
            '.Cells.Select()
            '.Cells.Delete()
            .Cells(1, 1).Value = "FINISH GOOD DELIVERY PLAN"
            .Cells(2, 1).Value = "Department :" : .Cells(2, 2).Value = dept
            .Cells(3, 1).Value = "Year :" : .Cells(3, 2).Value = Tahun
            .Cells(4, 1).Value = "Month :" : .Cells(4, 2).Value = Bulan
            .Cells(5, 1).Value = "Unit :" : .Cells(5, 2).Value = "METER"

            'setup header column
            .Cells(7, 1).Value = "Item :"
            counter = 2

            For i = FirstDate.Day To LastDate.Day
                .Cells(7, counter).Value = i
                counter += 1
            Next

            .Cells(7, counter).Value = "Total"

            StrQuery = "SELECT " & _
                            "qty_date, item_code, item_name, SUM(QTY) AS QTY " & _
                            ", YEAR(qty_date) as [Year], MONTH(qty_date) as [Month], Day(qty_date) as [Date] " & _
                        "FROM " & _
                            "tr_daily_plan_qty " & _
                        "WHERE " & _
                            "dept = '" & dept & "' and data_type=  '" & Kind & "'  and year(qty_date) = '" & Tahun & "' and month(qty_date) = '" & Bulan & "' and trans_type = 'PLAN'  " & _
                        "GROUP BY  " & _
                            "qty_date, item_code, item_name"

            DtTemp = Nothing
            DtTemp = konek.ExecuteQuerySQL(StrQuery)

            Dim Counter_j As Integer = 2
            counter = 8 'next row
            Dim InitialCounterForSubtotal = counter
            Dim RowStartForTableDraw As Integer = counter - 1

            For Each ItemData In ListItemSales
                .Cells(counter, 1).Value = ItemData.ItemName

                For J = FirstDate.Day To LastDate.Day
                    Dim FilterRow As DataRow
                    FilterRow = DtTemp.Select("item_name ='" & ItemData.ItemName.ToString().Trim & "' AND Date = '" & J & "'  ").FirstOrDefault
                    If Not FilterRow Is Nothing Then
                        Dim sumQTY As Decimal = IIf(IsDBNull(DtTemp.Compute("SUM(QTY)", "item_name = '" & ItemData.ItemName.ToString().Trim & "' AND Date = '" & J & "'  ")), 0, DtTemp.Compute("SUM(QTY)", "item_name = '" & ItemData.ItemName.ToString().Trim & "' AND Date = '" & J & "'  "))
                        .Cells(counter, Counter_j).Value = sumQTY
                    End If
                    Counter_j += 1
                Next

                Counter_j = 2
                counter += 1
                LoadingBar.Value += 1
            Next

            .Cells(counter, 1).Value = "Total"
            SetupFormulaSubTotal(LastDate, ExcelWs, counter, "B", InitialCounterForSubtotal, False)
            SetupFormulaSubTotal(LastDate, ExcelWs, counter, "B", InitialCounterForSubtotal, True)

            Dim KolomAkhir = ExcelColName(LastDate.Day + 2)
            Dim BarisTerakhir As Integer = counter

            FinishingLayoutExcel(RowStartForTableDraw, KolomAkhir, BarisTerakhir, ExcelWs)
            'ExcelWs.UsedRange.EntireColumn.AutoFit()
        End With

        ExcelWs.Visible = True

        StrQuery = ""
        DtTemp = Nothing
    End Sub

    Private Sub SetupFormulaSubTotal(ByVal _LastDate As Date, ByVal _ExcelWs As Excel.Worksheet, ByVal _LastRow As Integer, ByVal _ColumnsOfRange As String, ByVal _FirstRow As Integer, Optional ByVal _Horizontal As Boolean = False)
        'create formula subtotal vertikal dan horizontal

        Dim KolomSubtotAkhir As String = ""
        Dim DataRange As Excel.Range
        Dim FirstRowcColumn As Excel.Range
        Dim LastRowcColumn As Excel.Range

        KolomSubtotAkhir = ExcelColName(_LastDate.Day + 2)

        If _Horizontal Then
            DataRange = _ExcelWs.Range(_ColumnsOfRange & _LastRow, KolomSubtotAkhir & _LastRow)  'B42-af42

            Dim Kolom As String = _ColumnsOfRange & CStr(_FirstRow)
            'b7-af7
            FirstRowcColumn = _ExcelWs.Range(Kolom, KolomSubtotAkhir & _FirstRow)
            'b41-af41
            LastRowcColumn = _ExcelWs.Range(_ColumnsOfRange & _LastRow - 1 & "", KolomSubtotAkhir & _LastRow - 1 & "")

            Dim a, b As String

            For l = 1 To DataRange.Columns.Count
                a = Replace(FirstRowcColumn(l).Address, "$", "")
                b = Replace(LastRowcColumn(l).address, "$", "")
                'DataRange(l).Formula = "=SUBTOTAL(9," & a & " : " & b & ")"
                DataRange(l).Formula = "=SUM(" & a & " : " & b & ")"
            Next
        Else
            'subtotal vertical
            DataRange = _ExcelWs.Range(KolomSubtotAkhir & _FirstRow, KolomSubtotAkhir & _LastRow) 'af7-af42

            'b7-b41
            FirstRowcColumn = _ExcelWs.Range(_ColumnsOfRange & _FirstRow, _ColumnsOfRange & _LastRow - 1)
            'Ae7-ae41
            LastRowcColumn = _ExcelWs.Range(ExcelColName(_LastDate.Day + 1) & _FirstRow & "", ExcelColName(_LastDate.Day + 1) & _LastRow - 1 & "")
            Dim a, b As String

            For l = 1 To DataRange.Rows.Count
                a = Replace(FirstRowcColumn(l).Address, "$", "")
                b = Replace(LastRowcColumn(l).address, "$", "")
                'DataRange(l).Formula = "=SUBTOTAL(9," & a & " : " & b & ")"
                DataRange(l).Formula = "=SUM(" & a & " : " & b & ")"
            Next
        End If

    End Sub

    Private Sub FinishingLayoutExcel(ByVal RowStartForTableDraw As Integer, ByVal KolomAkhir As String, ByVal BarisTerakhir As Integer, ByVal ExcelWs As Excel.Worksheet)
        ExcelWs.Range("A" & RowStartForTableDraw & ":" & "A" & RowStartForTableDraw).Interior.Color = Color.FromArgb(244, 176, 132)
        ExcelWs.Range("A" & BarisTerakhir & ":" & "A" & BarisTerakhir).Interior.Color = Color.FromArgb(244, 176, 132)

        ExcelWs.Range("B" & RowStartForTableDraw & ":" & KolomAkhir & RowStartForTableDraw).Interior.Color = Color.FromArgb(155, 194, 230)
        ExcelWs.Range("B" & BarisTerakhir & ":" & KolomAkhir & BarisTerakhir).Interior.Color = Color.FromArgb(155, 194, 230)
        ExcelWs.Range(KolomAkhir & RowStartForTableDraw & ":" & KolomAkhir & BarisTerakhir).Interior.Color = Color.FromArgb(155, 194, 230)

        ExcelWs.Range("A" & RowStartForTableDraw & ":" & KolomAkhir & BarisTerakhir).Borders(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous
        ExcelWs.Range("A" & RowStartForTableDraw & ":" & KolomAkhir & BarisTerakhir).Borders(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous
        ExcelWs.Range("A" & RowStartForTableDraw & ":" & KolomAkhir & BarisTerakhir).Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous
        ExcelWs.Range("A" & RowStartForTableDraw & ":" & KolomAkhir & BarisTerakhir).Borders(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous
        ExcelWs.Range("A" & RowStartForTableDraw & ":" & KolomAkhir & BarisTerakhir).Borders(Excel.XlBordersIndex.xlInsideVertical).LineStyle = Excel.XlLineStyle.xlContinuous
        ExcelWs.Range("A" & RowStartForTableDraw & ":" & KolomAkhir & BarisTerakhir).Borders(Excel.XlBordersIndex.xlInsideHorizontal).LineStyle = Excel.XlLineStyle.xlContinuous

        ExcelWs.Columns("A").ColumnWidth = 27.43
        ExcelWs.Range("B1" & ":" & KolomAkhir & BarisTerakhir).ColumnWidth = 4

        'ExcelWs.UsedRange.EntireColumn.AutoFit()

    End Sub

#End Region

End Class

