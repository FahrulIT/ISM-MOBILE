Public Class FrmPostingPlanActual
    Private Sub BtnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnBack.Click
        frmLandingPage.Show()
        Me.Close()
    End Sub

    Private Sub BtnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnClose.Click
        If MessageBox.Show("Are you sure to close this application?", "Close", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            Application.Exit()
        Else
        End If
    End Sub

    Private Sub BtnPosting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnPosting.Click
        Try
            Dim PostingToWeb As New ClsPostingPlanActual(PbPosting)
            MessageBox.Show("Posting plan data to web success")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
End Class