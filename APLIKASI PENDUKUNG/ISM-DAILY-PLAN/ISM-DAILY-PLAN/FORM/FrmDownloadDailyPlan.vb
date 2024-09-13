Imports Excel = Microsoft.Office.Interop.Excel

Public Class FrmDownloadDailyPlan
    Dim Error_Message As String = ""
    Dim Helper As New ClsConnection
    Dim dt_dept As New DataTable

    Private Sub GetYear()
        Dim Tahun As Integer = Date.Now.Year

        For i = Tahun To Tahun + 1
            CbYear.Items.Add(i)
        Next
    End Sub
    Private Sub GetMonth()
        CbMonth.Items.Add("January")
        CbMonth.Items.Add("February")
        CbMonth.Items.Add("March")
        CbMonth.Items.Add("April")
        CbMonth.Items.Add("May")
        CbMonth.Items.Add("June")
        CbMonth.Items.Add("July")
        CbMonth.Items.Add("August")
        CbMonth.Items.Add("September")
        CbMonth.Items.Add("October")
        CbMonth.Items.Add("November")
        CbMonth.Items.Add("December")
    End Sub

    Private Function MonthToInteger(ByVal month As String)
        Dim bulan As Integer = 1

        Select Case month
            Case Is = "January"
                bulan = 1
            Case Is = "February"
                bulan = 2
            Case Is = "March"
                bulan = 3
            Case Is = "April"
                bulan = 4
            Case Is = "May"
                bulan = 5
            Case Is = "June"
                bulan = 6
            Case Is = "July"
                bulan = 7
            Case Is = "August"
                bulan = 8
            Case Is = "September"
                bulan = 9
            Case Is = "October"
                bulan = 10
            Case Is = "November"
                bulan = 11
            Case Is = "December"
                bulan = 12

            Case Else
                Throw New System.Exception("BULAN DALAM TAHUN TIDAK KETEMU")
        End Select

        Return bulan
    End Function

    Private Sub GetDept()
        If Dept = "ADMIN" Then
            Dim str_query As String = "SELECT " & _
                                        "A.DEPT AS Dept " & _
                                                    "FROM " & _
                                        "( " & _
                                        "	Select DISTINCT dept AS DEPT from v_ms_kind_data " & _
                                        ")A " & _
                                        "ORDER BY " & _
                                        "                Case A.DEPT " & _
                                        "		WHEN 'SPINNING' THEN 1 " & _
                                        "		WHEN 'WEAVING' THEN 2 " & _
                                        "		WHEN 'DYEING' THEN 3 " & _
                                        "	ELSE 4 " & _
                                        "	END"
            dt_dept = Helper.ExecuteQuerySQL(str_query)

            For Each Dr As DataRow In dt_dept.Rows
                CbDept.Items.Add(Dr.Item(0))
            Next
        Else
            CbDept.Items.Add(Helper.ExecuteSkalar("SELECT top 1 Dept from v_ms_kind_data where dept ='" & Dept & "' ").ToString)
            CbDept.SelectedIndex = 0
        End If
        dt_dept = Nothing
    End Sub

    Private Sub GetKind(ByVal _dept As String) 'for user dept only
        Dim str_query As String = "SELECT KindData FROM v_ms_kind_data WHERE Dept  = '" & _dept & "'"
        dt_dept = Helper.ExecuteQuerySQL(str_query)

        For Each Dr As DataRow In dt_dept.Rows
            CbKind.Items.Add(Dr.Item(0))
        Next
        dt_dept = Nothing

        If CbDept.Text = "SALES" Then
            CbKind.SelectedIndex = 0
        End If
    End Sub

    Private Sub releaseObject(ByVal Obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(Obj)
            Obj = Nothing
        Catch ex As Exception
            Obj = Nothing
        Finally
            GC.Collect()
        End Try
    End Sub

    Private Function Valid() As Boolean
        Dim status As Boolean = True
        Error_Message = ""

        If CbDept.Text = "" Then
            status = False
            Error_Message = Error_Message & "Dept Input not Valid" & Environment.NewLine
        End If

        If CbKind.Text = "" Then
            status = False
            Error_Message = Error_Message & "Kind of Data Input not Valid" & Environment.NewLine
        End If

        If CbYear.Text = "" Then
            status = False
            Error_Message = Error_Message & "Year Input not Valid" & Environment.NewLine
        End If

        If CbMonth.Text = "" Then
            status = False
            Error_Message = Error_Message & "Month Input not Valid" & Environment.NewLine
        End If

        Return status
    End Function

    Private Sub FrmDownloadDailyPlan_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetYear()
        GetMonth()
        GetDept()

        If Dept <> "ADMIN" Then
            GetKind(Dept)
        End If
    End Sub

    'for user admin
    Private Sub CbDept_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles CbDept.SelectionChangeCommitted
        CbKind.Items.Clear()
        Dim str_query As String = "SELECT KindData FROM v_ms_kind_data where Dept = '" & CbDept.Text & "' "
        dt_dept = Helper.ExecuteQuerySQL(str_query)

        For Each dr As DataRow In dt_dept.Rows
            CbKind.Items.Add(dr.Item(0))
        Next

        'If CbDept.Text = "SPINNING" Then
        '    CbKind.Items.Clear()
        '    CbKind.Items.Add("Raw Material Consumption")
        '    CbKind.Items.Add("Yarn Warehouse")
        'End If

        'If CbDept.Text = "WEAVING" Then
        '    CbKind.Items.Clear()
        '    CbKind.Items.Add("Yarn Consumption")
        '    CbKind.Items.Add("Grey Warehouse")
        'End If

        'If CbDept.Text = "DYEING" Then
        '    CbKind.Items.Clear()
        '    CbKind.Items.Add("Grey Consumption")
        '    CbKind.Items.Add("Finish Good Warehouse")
        'End If

        'If CbDept.Text = "SALES" Then
        '    CbKind.Items.Clear()
        '    CbKind.Items.Add("Finish Good Delivery")
        'End If
        dt_dept = Nothing
    End Sub

    Private Sub Btn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn.Click
        Try
            If Not Valid() Then
                Throw New System.Exception(Error_Message)
                Exit Sub
            Else
                'do nothing
            End If

            Me.Cursor = Cursors.WaitCursor
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US")
            Dim xlAppUpload As New Excel.Application
            Dim excelBook As Excel.Workbook
            Dim excelWorksheet As Excel.Worksheet

            xlAppUpload.UserControl = True
            excelBook = xlAppUpload.Workbooks.Add
            excelWorksheet = CType(excelBook.Worksheets(1), Excel.Worksheet)

            '--------------------------------------------------------
            'Proses Download
            '--------------------------------------------------------
            Dim ProsesDownload As New ClsDownload(CbDept.Text, CbKind.Text, CbYear.Text, MonthToInteger(CbMonth.Text), excelWorksheet, PbDownload)
            '--------------------------------------------------------

            xlAppUpload.Visible = True

            releaseObject(xlAppUpload)
            releaseObject(excelBook)
            releaseObject(excelWorksheet)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub BtnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnClose.Click
        If MessageBox.Show("Are you sure to close this application?", "Close", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            Application.Exit()
        Else
        End If
    End Sub

    Private Sub BtnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnBack.Click
        frmLandingPage.Show()
        Me.Close()
    End Sub
End Class