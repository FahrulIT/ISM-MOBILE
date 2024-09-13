<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLandingPage
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLandingPage))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.BtnDownload = New System.Windows.Forms.Button()
        Me.BtnUpload = New System.Windows.Forms.Button()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.BtnClose = New System.Windows.Forms.Button()
        Me.BtnPosting = New System.Windows.Forms.Button()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackgroundImage = CType(resources.GetObject("Panel1.BackgroundImage"), System.Drawing.Image)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(357, 55)
        Me.Panel1.TabIndex = 40
        '
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.Label9.Location = New System.Drawing.Point(3, 15)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(354, 29)
        Me.Label9.TabIndex = 46
        Me.Label9.Text = "Menu"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BtnDownload
        '
        Me.BtnDownload.BackColor = System.Drawing.SystemColors.Control
        Me.BtnDownload.FlatAppearance.BorderSize = 0
        Me.BtnDownload.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.BtnDownload.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnDownload.ForeColor = System.Drawing.Color.Black
        Me.BtnDownload.Image = CType(resources.GetObject("BtnDownload.Image"), System.Drawing.Image)
        Me.BtnDownload.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnDownload.Location = New System.Drawing.Point(113, 65)
        Me.BtnDownload.Name = "BtnDownload"
        Me.BtnDownload.Size = New System.Drawing.Size(174, 43)
        Me.BtnDownload.TabIndex = 41
        Me.BtnDownload.Text = "   Download"
        Me.BtnDownload.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.BtnDownload.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.BtnDownload.UseVisualStyleBackColor = False
        '
        'BtnUpload
        '
        Me.BtnUpload.BackColor = System.Drawing.SystemColors.Control
        Me.BtnUpload.FlatAppearance.BorderSize = 0
        Me.BtnUpload.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.BtnUpload.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnUpload.ForeColor = System.Drawing.Color.Black
        Me.BtnUpload.Image = CType(resources.GetObject("BtnUpload.Image"), System.Drawing.Image)
        Me.BtnUpload.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.BtnUpload.Location = New System.Drawing.Point(40, 114)
        Me.BtnUpload.Name = "BtnUpload"
        Me.BtnUpload.Size = New System.Drawing.Size(174, 43)
        Me.BtnUpload.TabIndex = 42
        Me.BtnUpload.Text = "   Upload"
        Me.BtnUpload.UseVisualStyleBackColor = False
        '
        'Panel3
        '
        Me.Panel3.BackgroundImage = CType(resources.GetObject("Panel3.BackgroundImage"), System.Drawing.Image)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel3.Location = New System.Drawing.Point(0, 270)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Panel3.Size = New System.Drawing.Size(357, 3)
        Me.Panel3.TabIndex = 51
        '
        'BtnClose
        '
        Me.BtnClose.BackColor = System.Drawing.SystemColors.Control
        Me.BtnClose.FlatAppearance.BorderSize = 0
        Me.BtnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.BtnClose.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnClose.ForeColor = System.Drawing.Color.Black
        Me.BtnClose.Image = CType(resources.GetObject("BtnClose.Image"), System.Drawing.Image)
        Me.BtnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.BtnClose.Location = New System.Drawing.Point(40, 213)
        Me.BtnClose.Name = "BtnClose"
        Me.BtnClose.Size = New System.Drawing.Size(174, 43)
        Me.BtnClose.TabIndex = 52
        Me.BtnClose.Text = "Close       "
        Me.BtnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.BtnClose.UseVisualStyleBackColor = False
        '
        'BtnPosting
        '
        Me.BtnPosting.BackColor = System.Drawing.SystemColors.Control
        Me.BtnPosting.Enabled = False
        Me.BtnPosting.FlatAppearance.BorderSize = 0
        Me.BtnPosting.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.BtnPosting.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnPosting.ForeColor = System.Drawing.Color.Black
        Me.BtnPosting.Image = CType(resources.GetObject("BtnPosting.Image"), System.Drawing.Image)
        Me.BtnPosting.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnPosting.Location = New System.Drawing.Point(113, 164)
        Me.BtnPosting.Name = "BtnPosting"
        Me.BtnPosting.Size = New System.Drawing.Size(174, 43)
        Me.BtnPosting.TabIndex = 53
        Me.BtnPosting.Text = "Posting Plan"
        Me.BtnPosting.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.BtnPosting.UseVisualStyleBackColor = False
        '
        'frmLandingPage
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(357, 273)
        Me.Controls.Add(Me.BtnPosting)
        Me.Controls.Add(Me.BtnClose)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.BtnUpload)
        Me.Controls.Add(Me.BtnDownload)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmLandingPage"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Landing Page"
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents BtnDownload As System.Windows.Forms.Button
    Friend WithEvents BtnUpload As System.Windows.Forms.Button
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents BtnClose As System.Windows.Forms.Button
    Friend WithEvents BtnPosting As System.Windows.Forms.Button
End Class
