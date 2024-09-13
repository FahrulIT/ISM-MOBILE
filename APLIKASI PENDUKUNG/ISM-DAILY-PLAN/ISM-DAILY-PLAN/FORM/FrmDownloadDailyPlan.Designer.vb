<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmDownloadDailyPlan
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmDownloadDailyPlan))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.BtnBack = New System.Windows.Forms.Button()
        Me.PbDownload = New System.Windows.Forms.ProgressBar()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.CbMonth = New System.Windows.Forms.ComboBox()
        Me.CbYear = New System.Windows.Forms.ComboBox()
        Me.CbKind = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.BtnClose = New System.Windows.Forms.Button()
        Me.CbDept = New System.Windows.Forms.ComboBox()
        Me.Btn = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackgroundImage = CType(resources.GetObject("Panel1.BackgroundImage"), System.Drawing.Image)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(505, 55)
        Me.Panel1.TabIndex = 39
        '
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.Label9.Location = New System.Drawing.Point(3, 15)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(499, 29)
        Me.Label9.TabIndex = 46
        Me.Label9.Text = "Download To Excel"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.White
        Me.Panel2.Controls.Add(Me.BtnBack)
        Me.Panel2.Controls.Add(Me.PbDownload)
        Me.Panel2.Controls.Add(Me.Label8)
        Me.Panel2.Controls.Add(Me.Label7)
        Me.Panel2.Controls.Add(Me.Label6)
        Me.Panel2.Controls.Add(Me.Label1)
        Me.Panel2.Controls.Add(Me.CbMonth)
        Me.Panel2.Controls.Add(Me.CbYear)
        Me.Panel2.Controls.Add(Me.CbKind)
        Me.Panel2.Controls.Add(Me.Label5)
        Me.Panel2.Controls.Add(Me.Label4)
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.Controls.Add(Me.BtnClose)
        Me.Panel2.Controls.Add(Me.CbDept)
        Me.Panel2.Controls.Add(Me.Btn)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 55)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(505, 279)
        Me.Panel2.TabIndex = 40
        '
        'BtnBack
        '
        Me.BtnBack.BackColor = System.Drawing.SystemColors.Control
        Me.BtnBack.FlatAppearance.BorderSize = 0
        Me.BtnBack.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.BtnBack.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnBack.ForeColor = System.Drawing.Color.Black
        Me.BtnBack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnBack.Location = New System.Drawing.Point(322, 199)
        Me.BtnBack.Name = "BtnBack"
        Me.BtnBack.Size = New System.Drawing.Size(120, 43)
        Me.BtnBack.TabIndex = 53
        Me.BtnBack.Text = "Back"
        Me.BtnBack.UseVisualStyleBackColor = False
        '
        'PbDownload
        '
        Me.PbDownload.Location = New System.Drawing.Point(5, 253)
        Me.PbDownload.Name = "PbDownload"
        Me.PbDownload.Size = New System.Drawing.Size(495, 21)
        Me.PbDownload.TabIndex = 52
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.Black
        Me.Label8.Location = New System.Drawing.Point(150, 148)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(17, 29)
        Me.Label8.TabIndex = 45
        Me.Label8.Text = ":"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Black
        Me.Label7.Location = New System.Drawing.Point(150, 106)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(17, 29)
        Me.Label7.TabIndex = 44
        Me.Label7.Text = ":"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Black
        Me.Label6.Location = New System.Drawing.Point(150, 62)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(17, 29)
        Me.Label6.TabIndex = 43
        Me.Label6.Text = ":"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(150, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(17, 29)
        Me.Label1.TabIndex = 42
        Me.Label1.Text = ":"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'CbMonth
        '
        Me.CbMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CbMonth.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CbMonth.FormattingEnabled = True
        Me.CbMonth.Location = New System.Drawing.Point(169, 148)
        Me.CbMonth.Name = "CbMonth"
        Me.CbMonth.Size = New System.Drawing.Size(293, 29)
        Me.CbMonth.TabIndex = 41
        '
        'CbYear
        '
        Me.CbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CbYear.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CbYear.FormattingEnabled = True
        Me.CbYear.Location = New System.Drawing.Point(169, 106)
        Me.CbYear.Name = "CbYear"
        Me.CbYear.Size = New System.Drawing.Size(293, 29)
        Me.CbYear.TabIndex = 40
        '
        'CbKind
        '
        Me.CbKind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CbKind.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CbKind.FormattingEnabled = True
        Me.CbKind.Location = New System.Drawing.Point(169, 62)
        Me.CbKind.Name = "CbKind"
        Me.CbKind.Size = New System.Drawing.Size(293, 29)
        Me.CbKind.TabIndex = 39
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Black
        Me.Label5.Location = New System.Drawing.Point(26, 148)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(121, 29)
        Me.Label5.TabIndex = 38
        Me.Label5.Text = "Month"
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Black
        Me.Label4.Location = New System.Drawing.Point(27, 106)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(120, 28)
        Me.Label4.TabIndex = 37
        Me.Label4.Text = "Year"
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Black
        Me.Label3.Location = New System.Drawing.Point(26, 18)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(121, 29)
        Me.Label3.TabIndex = 36
        Me.Label3.Text = "Department"
        '
        'BtnClose
        '
        Me.BtnClose.BackColor = System.Drawing.SystemColors.Control
        Me.BtnClose.FlatAppearance.BorderSize = 0
        Me.BtnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.BtnClose.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnClose.ForeColor = System.Drawing.Color.Black
        Me.BtnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnClose.Location = New System.Drawing.Point(196, 199)
        Me.BtnClose.Name = "BtnClose"
        Me.BtnClose.Size = New System.Drawing.Size(120, 43)
        Me.BtnClose.TabIndex = 35
        Me.BtnClose.Text = "Close"
        Me.BtnClose.UseVisualStyleBackColor = False
        '
        'CbDept
        '
        Me.CbDept.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CbDept.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CbDept.FormattingEnabled = True
        Me.CbDept.Location = New System.Drawing.Point(169, 18)
        Me.CbDept.Name = "CbDept"
        Me.CbDept.Size = New System.Drawing.Size(293, 29)
        Me.CbDept.TabIndex = 33
        '
        'Btn
        '
        Me.Btn.BackColor = System.Drawing.SystemColors.Control
        Me.Btn.FlatAppearance.BorderSize = 0
        Me.Btn.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.Btn.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Btn.ForeColor = System.Drawing.Color.Black
        Me.Btn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Btn.Location = New System.Drawing.Point(70, 199)
        Me.Btn.Name = "Btn"
        Me.Btn.Size = New System.Drawing.Size(120, 43)
        Me.Btn.TabIndex = 31
        Me.Btn.Text = "Download"
        Me.Btn.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Black
        Me.Label2.Location = New System.Drawing.Point(26, 62)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(121, 29)
        Me.Label2.TabIndex = 32
        Me.Label2.Text = "Kind of Data"
        '
        'Panel3
        '
        Me.Panel3.BackgroundImage = CType(resources.GetObject("Panel3.BackgroundImage"), System.Drawing.Image)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel3.Location = New System.Drawing.Point(0, 334)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Panel3.Size = New System.Drawing.Size(505, 3)
        Me.Panel3.TabIndex = 47
        '
        'FrmDownloadDailyPlan
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(505, 337)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FrmDownloadDailyPlan"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Daily plan to excel"
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents CbMonth As System.Windows.Forms.ComboBox
    Friend WithEvents CbYear As System.Windows.Forms.ComboBox
    Friend WithEvents CbKind As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents BtnClose As System.Windows.Forms.Button
    Friend WithEvents CbDept As System.Windows.Forms.ComboBox
    Friend WithEvents Btn As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents PbDownload As System.Windows.Forms.ProgressBar
    Friend WithEvents BtnBack As System.Windows.Forms.Button
End Class
