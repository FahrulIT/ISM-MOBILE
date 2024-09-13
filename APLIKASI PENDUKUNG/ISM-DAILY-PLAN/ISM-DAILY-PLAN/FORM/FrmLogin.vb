Imports System.Data.SqlClient

Public Class FrmLogin
    Dim Konek As New ClsConnection
    Dim UserList As List(Of ClsUserUploadDownload) = New List(Of ClsUserUploadDownload)()

    Public Sub SetCulture(ByVal culture As String)
        Dim myCulture As New Globalization.CultureInfo(culture)
        'With myCulture.NumberFormat
        '    .NumberDecimalSeparator = "."
        'End With

        With myCulture.DateTimeFormat
            .ShortDatePattern = "yyyy-MM-dd"
        End With

        'apply this customized culture to your application
        Application.CurrentCulture = myCulture
    End Sub

    Private Sub GetUserData()
        Dim StrQuery As String = "SELECT RTRIM([Username]) AS Username, RTRIM([Pass]) AS Pass, rtrim(Dept) as Dept FROM [ISM_MOBILE].[dbo].[UserUploadDownload]"

        Dim DtUser As New DataTable
        DtUser = Konek.ExecuteQuerySQL(StrQuery)

        UserList = (From dr In DtUser.Rows
                    Select New ClsUserUploadDownload() With
                    {
                        .Userid = dr("Username").ToString(),
                        .Sandi = dr("Pass").ToString(),
                        .Dept = dr("Dept").ToString()
                    }).ToList()
    End Sub

    Protected Sub loginApp()
        Try
            'StrIp = Konek.SetIpAdress()
            Dim FindUser As List(Of ClsUserUploadDownload) = New List(Of ClsUserUploadDownload)
            FindUser = (From Data As ClsUserUploadDownload In UserList Where Data.Userid.Equals(Me.KdPenggunaTxt.Text.ToString) AndAlso Data.Sandi.Equals(Me.PswTxt.Text.ToString) Select Data).ToList

            If FindUser.Count > 0 Then
                For Each StrData In FindUser
                    UserId = StrData.Userid
                    Dept = StrData.Dept
                Next
              frmLandingPage.Show()
                Me.Hide()
            Else
                MessageBox.Show("Please Check ID and Password" & vbCrLf & "Login Failed")
            End If

        Catch ex As Exception
            Throw New System.Exception("Login Failed, System Error" & vbCr & ex.Message)
        End Try
    End Sub

    Private Sub FrmLogin_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'KdPenggunaTxt.Text = "NPC001"
            'PswTxt.Text = "IT1628"
            SetCulture("EN-US")
            GetUserData()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub OkBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OkBtn.Click

        'If KdPenggunaTxt.Text = "ADMIN" And PswTxt.Text = "ADMIN" Then
        '    UserId = KdPenggunaTxt.Text
        '    IpAddress = Konek.SetIpAdress
        '    frmLandingPage.Show()
        'End If

        loginApp()
    End Sub

    Private Sub BatalBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BatalBtn.Click
        Application.Exit()
    End Sub

    Private Sub KdPenggunaTxt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles KdPenggunaTxt.KeyDown
        If e.KeyCode = Keys.Enter Then
            PswTxt.Focus()
        End If
    End Sub

    Private Sub PswTxt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles PswTxt.KeyDown
        If e.KeyCode = Keys.Enter Then
            OkBtn.PerformClick()
        End If
    End Sub
End Class