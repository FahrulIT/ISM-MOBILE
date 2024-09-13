Imports System.IO

Public Class FrmUploadDailyPlan
    Dim FileExcel As String = ""
    Dim Error_Message As String = ""
    Dim tgl_server As Date
    Dim Helper As New ClsConnection
    Dim dt_dept As New DataTable

    Private Sub GetYear()
        Dim Tahun As Integer = tgl_server.Year

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


    Private Sub FrmUploadDailyPlan_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim konek As New ClsConnection
        tgl_server = CDate(konek.ExecuteSkalar("select getdate() as Tgl_sekarang"))

        GetYear()
        GetMonth()
        GetDept()

        If Dept <> "ADMIN" Then
            GetKind(Dept)
        End If
        'Dim message_error() As String = {"Error1", "Error 2", "Error 3"}
        'TxtMessage.Lines = message_error


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

        If TxtFile.Text = "" Then
            status = False
            Error_Message = Error_Message & "File Excel not Valid" & Environment.NewLine
        End If


        'If tgl_server.Day > 25 And DateTime.Now.Year Then
        '    status = False
        '    Error_Message = Error_Message & "File Excel not Valid" & Environment.NewLine
        'End If

        Return status
    End Function

    Private Sub BtnUpload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnUpload.Click
        Try
            TxtMessage.Clear() : TxtMessage.Text = "Message"

            If Not Valid() Then
                Throw New System.Exception(Error_Message)
                Exit Sub
            Else
                Dim UploadExcel As New ClsUpload(FileExcel, CbDept.Text, CbKind.Text, CbYear.Text, MonthToInteger(CbMonth.Text), PbUpload)
                If UploadExcel.ListItemNotFound.Count > 0 Then
                    TxtMessage.Lines = UploadExcel.ListItemNotFound.ToArray()
                    TxtMessage.Text = TxtMessage.Text & Environment.NewLine & "Item Quantity Failed to Upload"
                    UploadExcel.ListItemNotFound.Clear()
                    MessageBox.Show("Upload Success with several data failed to upload")
                Else
                    MessageBox.Show("Upload Success")
                End If
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
        End Try
        
    End Sub

    Private Sub CbDept_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles CbDept.SelectionChangeCommitted
        CbKind.Items.Clear()
        Dim str_query As String = "SELECT KindData FROM v_ms_kind_data where Dept = '" & CbDept.Text & "' "
        dt_dept = Helper.ExecuteQuerySQL(str_query)

        For Each dr As DataRow In dt_dept.Rows
            CbKind.Items.Add(dr.Item(0))
        Next
    End Sub

    Private Sub BtnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnClose.Click
        If MessageBox.Show("Are you sure to close this application?", "Close", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            Application.Exit()
        Else
        End If
    End Sub

    Private Sub BtnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnBrowse.Click
        BrowseFile.FileName = ""
        BrowseFile.Filter = "(*.xls)|*.xlsx"

        If BrowseFile.ShowDialog() = DialogResult.OK Then
            Dim strExt As String = Path.GetExtension(BrowseFile.FileName)
            FileExcel = BrowseFile.FileName
            TxtFile.Text = FileExcel
        Else
            Exit Sub
        End If
    End Sub

    Private Sub BtnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnBack.Click
        frmLandingPage.Show()
        Me.Close()
    End Sub

    Private Sub CbMonth_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles CbMonth.SelectionChangeCommitted
        Try
            If UserId <> "ADMIN" Then
                'tgl_server = New Date(2024, 12, 25)
                If CbYear.Text <> "" Then
                    If tgl_server.Month < 11 Then
                        If tgl_server.Day > 25 Then
                            If MonthToInteger(CbMonth.Text) <= tgl_server.Month + 1 And CbYear.Text = tgl_server.Year Then
                                BtnUpload.Enabled = False
                                MessageBox.Show("Current Date already above 25th, Choose another month")
                                Exit Sub
                            Else
                                BtnUpload.Enabled = True
                            End If
                        Else
                            If MonthToInteger(CbMonth.Text) <= tgl_server.Month And CbYear.Text = tgl_server.Year Then
                                BtnUpload.Enabled = False
                                MessageBox.Show("Choose another month")
                                Exit Sub
                            Else
                                BtnUpload.Enabled = True
                            End If
                        End If
                    Else
                        If tgl_server.Day > 25 Then
                            If tgl_server.Month = 11 Then
                                If CbYear.Text = tgl_server.Year Then
                                    BtnUpload.Enabled = False
                                    MessageBox.Show("Current Date already above 25th, Choose another year and another month")
                                Else
                                    BtnUpload.Enabled = True
                                End If
                            End If

                            If tgl_server.Month = 12 Then
                                If CbYear.Text > tgl_server.Year Then
                                    If MonthToInteger(CbMonth.Text) < 2 Then
                                        BtnUpload.Enabled = False
                                        MessageBox.Show("Current Date already above 25th, Choose another year and another month")
                                    Else
                                        BtnUpload.Enabled = True
                                    End If
                                Else
                                    BtnUpload.Enabled = False
                                    MessageBox.Show("Choose another month")
                                End If
                            End If

                        Else
                            If MonthToInteger(CbMonth.Text) <= tgl_server.Month And CbYear.Text = tgl_server.Year Then
                                BtnUpload.Enabled = False
                                MessageBox.Show("Choose another month")
                                Exit Sub
                            Else
                                BtnUpload.Enabled = True
                            End If
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            MessageBox.Show(ex.InnerException.ToString())
        End Try
    End Sub

    Private Sub CbYear_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles CbYear.SelectionChangeCommitted
        Try
            If UserId <> "ADMIN" Then
                'tgl_server = New Date(2024, 12, 25)
                If CbMonth.Text <> "" Then
                    If tgl_server.Month < 11 Then
                        If tgl_server.Day > 25 Then
                            If MonthToInteger(CbMonth.Text) <= tgl_server.Month + 1 And CbYear.Text = tgl_server.Year Then
                                BtnUpload.Enabled = False
                                MessageBox.Show("Current Date already above 25th, Choose another month")
                                Exit Sub
                            Else
                                BtnUpload.Enabled = True
                            End If
                        Else
                            If MonthToInteger(CbMonth.Text) <= tgl_server.Month And CbYear.Text = tgl_server.Year Then
                                BtnUpload.Enabled = False
                                MessageBox.Show("Choose another month")
                                Exit Sub
                            Else
                                BtnUpload.Enabled = True
                            End If
                        End If
                    Else
                        If tgl_server.Day > 25 Then
                            If tgl_server.Month = 11 Then
                                If CbYear.Text = tgl_server.Year Then
                                    BtnUpload.Enabled = False
                                    MessageBox.Show("Current Date already above 25th, Choose another year and another month")
                                Else
                                    BtnUpload.Enabled = True
                                End If
                            End If

                            If tgl_server.Month = 12 Then
                                If CbYear.Text > tgl_server.Year Then
                                    If MonthToInteger(CbMonth.Text) < 2 Then
                                        BtnUpload.Enabled = False
                                        MessageBox.Show("Current Date already above 25th, Choose another year and another month")
                                    Else
                                        BtnUpload.Enabled = True
                                    End If
                                Else
                                    BtnUpload.Enabled = False
                                    MessageBox.Show("Current Date already above 25th, Choose another year and another month")
                                End If
                            End If

                        Else
                            If MonthToInteger(CbMonth.Text) <= tgl_server.Month And CbYear.Text = tgl_server.Year Then
                                BtnUpload.Enabled = False
                                MessageBox.Show("Choose another month")
                                Exit Sub
                            Else
                                BtnUpload.Enabled = True
                            End If
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            MessageBox.Show(ex.InnerException.ToString())
        End Try
    End Sub
End Class