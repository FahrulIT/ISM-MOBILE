<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmPostingPlanActual
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmPostingPlanActual))
        Me.BtnPosting = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.BtnBack = New System.Windows.Forms.Button()
        Me.BtnClose = New System.Windows.Forms.Button()
        Me.PbPosting = New System.Windows.Forms.ProgressBar()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'BtnPosting
        '
        Me.BtnPosting.BackColor = System.Drawing.SystemColors.Control
        Me.BtnPosting.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.BtnPosting.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnPosting.Location = New System.Drawing.Point(12, 30)
        Me.BtnPosting.Name = "BtnPosting"
        Me.BtnPosting.Size = New System.Drawing.Size(120, 43)
        Me.BtnPosting.TabIndex = 0
        Me.BtnPosting.Text = "Posting"
        Me.BtnPosting.UseVisualStyleBackColor = False
        '
        'Panel1
        '
        Me.Panel1.BackgroundImage = CType(resources.GetObject("Panel1.BackgroundImage"), System.Drawing.Image)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(397, 55)
        Me.Panel1.TabIndex = 40
        '
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.Label9.Location = New System.Drawing.Point(3, 15)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(364, 29)
        Me.Label9.TabIndex = 46
        Me.Label9.Text = "Posting Plan - Actual Data"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel2.Controls.Add(Me.BtnBack)
        Me.Panel2.Controls.Add(Me.BtnClose)
        Me.Panel2.Controls.Add(Me.PbPosting)
        Me.Panel2.Controls.Add(Me.BtnPosting)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 55)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(397, 142)
        Me.Panel2.TabIndex = 41
        '
        'BtnBack
        '
        Me.BtnBack.BackColor = System.Drawing.SystemColors.Control
        Me.BtnBack.FlatAppearance.BorderSize = 0
        Me.BtnBack.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.BtnBack.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnBack.ForeColor = System.Drawing.Color.Black
        Me.BtnBack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnBack.Location = New System.Drawing.Point(264, 30)
        Me.BtnBack.Name = "BtnBack"
        Me.BtnBack.Size = New System.Drawing.Size(120, 43)
        Me.BtnBack.TabIndex = 55
        Me.BtnBack.Text = "Back"
        Me.BtnBack.UseVisualStyleBackColor = False
        '
        'BtnClose
        '
        Me.BtnClose.BackColor = System.Drawing.SystemColors.Control
        Me.BtnClose.FlatAppearance.BorderSize = 0
        Me.BtnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.BtnClose.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnClose.ForeColor = System.Drawing.Color.Black
        Me.BtnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnClose.Location = New System.Drawing.Point(138, 30)
        Me.BtnClose.Name = "BtnClose"
        Me.BtnClose.Size = New System.Drawing.Size(120, 43)
        Me.BtnClose.TabIndex = 54
        Me.BtnClose.Text = "Close"
        Me.BtnClose.UseVisualStyleBackColor = False
        '
        'PbPosting
        '
        Me.PbPosting.Location = New System.Drawing.Point(12, 102)
        Me.PbPosting.Name = "PbPosting"
        Me.PbPosting.Size = New System.Drawing.Size(372, 27)
        Me.PbPosting.TabIndex = 53
        '
        'Panel3
        '
        Me.Panel3.BackgroundImage = CType(resources.GetObject("Panel3.BackgroundImage"), System.Drawing.Image)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel3.Location = New System.Drawing.Point(0, 194)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Panel3.Size = New System.Drawing.Size(397, 3)
        Me.Panel3.TabIndex = 48
        '
        'FrmPostingPlanActual
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(397, 197)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FrmPostingPlanActual"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FrmPostingPlanActual"
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents BtnPosting As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents PbPosting As System.Windows.Forms.ProgressBar
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents BtnBack As System.Windows.Forms.Button
    Friend WithEvents BtnClose As System.Windows.Forms.Button
End Class
