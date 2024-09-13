Public Class frmLandingPage
    Private Sub BtnDownload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnDownload.Click
        FrmDownloadDailyPlan.Show()
        Me.Close()
    End Sub

    Private Sub BtnUpload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnUpload.Click
        FrmUploadDailyPlan.Show()
        Me.Close()
    End Sub

    Private Sub BtnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnClose.Click
        Application.Exit()
    End Sub

    Private Sub BtnPosting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnPosting.Click
        FrmPostingPlanActual.Show()
        Me.Close()
    End Sub

    Private Sub frmLandingPage_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If UserId = "ADMIN" Then
            BtnPosting.Enabled = True
        Else
            BtnPosting.Enabled = False
        End If
    End Sub
End Class