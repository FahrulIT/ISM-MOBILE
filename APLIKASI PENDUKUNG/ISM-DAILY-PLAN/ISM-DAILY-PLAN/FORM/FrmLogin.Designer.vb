<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmLogin
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmLogin))
        Me.BatalBtn = New System.Windows.Forms.Button()
        Me.PswTxt = New System.Windows.Forms.TextBox()
        Me.KdPenggunaTxt = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.OkBtn = New System.Windows.Forms.Button()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'BatalBtn
        '
        Me.BatalBtn.BackColor = System.Drawing.SystemColors.Control
        Me.BatalBtn.FlatAppearance.BorderSize = 0
        Me.BatalBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.BatalBtn.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BatalBtn.ForeColor = System.Drawing.Color.Black
        Me.BatalBtn.Image = CType(resources.GetObject("BatalBtn.Image"), System.Drawing.Image)
        Me.BatalBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BatalBtn.Location = New System.Drawing.Point(222, 225)
        Me.BatalBtn.Name = "BatalBtn"
        Me.BatalBtn.Size = New System.Drawing.Size(209, 43)
        Me.BatalBtn.TabIndex = 4
        Me.BatalBtn.Text = "    Close"
        Me.BatalBtn.UseVisualStyleBackColor = False
        '
        'PswTxt
        '
        Me.PswTxt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.PswTxt.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PswTxt.Location = New System.Drawing.Point(263, 164)
        Me.PswTxt.Name = "PswTxt"
        Me.PswTxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.PswTxt.Size = New System.Drawing.Size(168, 32)
        Me.PswTxt.TabIndex = 2
        '
        'KdPenggunaTxt
        '
        Me.KdPenggunaTxt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.KdPenggunaTxt.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KdPenggunaTxt.Location = New System.Drawing.Point(263, 119)
        Me.KdPenggunaTxt.Name = "KdPenggunaTxt"
        Me.KdPenggunaTxt.Size = New System.Drawing.Size(168, 32)
        Me.KdPenggunaTxt.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(169, 124)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(71, 25)
        Me.Label1.TabIndex = 28
        Me.Label1.Text = "User Id"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Black
        Me.Label2.Location = New System.Drawing.Point(161, 169)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(91, 25)
        Me.Label2.TabIndex = 29
        Me.Label2.Text = "Password"
        '
        'OkBtn
        '
        Me.OkBtn.BackColor = System.Drawing.SystemColors.Control
        Me.OkBtn.FlatAppearance.BorderSize = 0
        Me.OkBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.OkBtn.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OkBtn.ForeColor = System.Drawing.Color.Black
        Me.OkBtn.Image = CType(resources.GetObject("OkBtn.Image"), System.Drawing.Image)
        Me.OkBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.OkBtn.Location = New System.Drawing.Point(11, 225)
        Me.OkBtn.Name = "OkBtn"
        Me.OkBtn.Size = New System.Drawing.Size(205, 43)
        Me.OkBtn.TabIndex = 3
        Me.OkBtn.Text = "  Login"
        Me.OkBtn.UseVisualStyleBackColor = False
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(20, 84)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(135, 126)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox2.TabIndex = 33
        Me.PictureBox2.TabStop = False
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Black
        Me.Label5.Location = New System.Drawing.Point(166, 68)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(263, 30)
        Me.Label5.TabIndex = 34
        Me.Label5.Text = "Please Input Your ID and Password"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel1
        '
        Me.Panel1.BackgroundImage = CType(resources.GetObject("Panel1.BackgroundImage"), System.Drawing.Image)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(443, 55)
        Me.Panel1.TabIndex = 38
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.Label3.Location = New System.Drawing.Point(3, 14)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(437, 25)
        Me.Label3.TabIndex = 39
        Me.Label3.Text = "Daily Plan "
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.White
        Me.Panel2.Controls.Add(Me.Panel1)
        Me.Panel2.Controls.Add(Me.Label5)
        Me.Panel2.Controls.Add(Me.PictureBox2)
        Me.Panel2.Controls.Add(Me.OkBtn)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Controls.Add(Me.Label1)
        Me.Panel2.Controls.Add(Me.KdPenggunaTxt)
        Me.Panel2.Controls.Add(Me.PswTxt)
        Me.Panel2.Controls.Add(Me.BatalBtn)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(443, 276)
        Me.Panel2.TabIndex = 3
        '
        'FrmLogin
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(443, 276)
        Me.Controls.Add(Me.Panel2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "FrmLogin"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Down/Upl Daily Plan"
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents BatalBtn As System.Windows.Forms.Button
    Friend WithEvents PswTxt As System.Windows.Forms.TextBox
    Friend WithEvents KdPenggunaTxt As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents OkBtn As System.Windows.Forms.Button
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
End Class
