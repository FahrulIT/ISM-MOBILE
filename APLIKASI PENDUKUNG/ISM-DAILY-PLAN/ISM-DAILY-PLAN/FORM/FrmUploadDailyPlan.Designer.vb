<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmUploadDailyPlan
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmUploadDailyPlan))
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.CbMonth = New System.Windows.Forms.ComboBox()
        Me.CbYear = New System.Windows.Forms.ComboBox()
        Me.CbKind = New System.Windows.Forms.ComboBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.TxtMessage = New System.Windows.Forms.TextBox()
        Me.BtnBack = New System.Windows.Forms.Button()
        Me.PbUpload = New System.Windows.Forms.ProgressBar()
        Me.TxtFile = New System.Windows.Forms.TextBox()
        Me.BtnBrowse = New System.Windows.Forms.Button()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.BtnClose = New System.Windows.Forms.Button()
        Me.CbDept = New System.Windows.Forms.ComboBox()
        Me.BtnUpload = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.BrowseFile = New System.Windows.Forms.OpenFileDialog()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.Black
        Me.Label8.Location = New System.Drawing.Point(193, 162)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(17, 29)
        Me.Label8.TabIndex = 45
        Me.Label8.Text = ":"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Black
        Me.Label6.Location = New System.Drawing.Point(193, 76)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(17, 29)
        Me.Label6.TabIndex = 43
        Me.Label6.Text = ":"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Black
        Me.Label7.Location = New System.Drawing.Point(193, 120)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(17, 29)
        Me.Label7.TabIndex = 44
        Me.Label7.Text = ":"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel1
        '
        Me.Panel1.BackgroundImage = CType(resources.GetObject("Panel1.BackgroundImage"), System.Drawing.Image)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(699, 55)
        Me.Panel1.TabIndex = 48
        '
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.Label9.Location = New System.Drawing.Point(3, 15)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(578, 29)
        Me.Label9.TabIndex = 46
        Me.Label9.Text = "Upload Data From Excel"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(193, 32)
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
        Me.CbMonth.Location = New System.Drawing.Point(212, 162)
        Me.CbMonth.Name = "CbMonth"
        Me.CbMonth.Size = New System.Drawing.Size(128, 29)
        Me.CbMonth.TabIndex = 41
        '
        'CbYear
        '
        Me.CbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CbYear.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CbYear.FormattingEnabled = True
        Me.CbYear.Location = New System.Drawing.Point(212, 120)
        Me.CbYear.Name = "CbYear"
        Me.CbYear.Size = New System.Drawing.Size(87, 29)
        Me.CbYear.TabIndex = 40
        '
        'CbKind
        '
        Me.CbKind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CbKind.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CbKind.FormattingEnabled = True
        Me.CbKind.Location = New System.Drawing.Point(212, 76)
        Me.CbKind.Name = "CbKind"
        Me.CbKind.Size = New System.Drawing.Size(213, 29)
        Me.CbKind.TabIndex = 39
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.White
        Me.Panel2.Controls.Add(Me.TxtMessage)
        Me.Panel2.Controls.Add(Me.BtnBack)
        Me.Panel2.Controls.Add(Me.PbUpload)
        Me.Panel2.Controls.Add(Me.TxtFile)
        Me.Panel2.Controls.Add(Me.BtnBrowse)
        Me.Panel2.Controls.Add(Me.Label10)
        Me.Panel2.Controls.Add(Me.Label11)
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
        Me.Panel2.Controls.Add(Me.BtnUpload)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 55)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(699, 510)
        Me.Panel2.TabIndex = 49
        '
        'TxtMessage
        '
        Me.TxtMessage.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtMessage.Location = New System.Drawing.Point(3, 365)
        Me.TxtMessage.Multiline = True
        Me.TxtMessage.Name = "TxtMessage"
        Me.TxtMessage.ReadOnly = True
        Me.TxtMessage.Size = New System.Drawing.Size(693, 138)
        Me.TxtMessage.TabIndex = 55
        Me.TxtMessage.Text = "Message"
        '
        'BtnBack
        '
        Me.BtnBack.BackColor = System.Drawing.SystemColors.Control
        Me.BtnBack.FlatAppearance.BorderSize = 0
        Me.BtnBack.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.BtnBack.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnBack.ForeColor = System.Drawing.Color.Black
        Me.BtnBack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnBack.Location = New System.Drawing.Point(431, 284)
        Me.BtnBack.Name = "BtnBack"
        Me.BtnBack.Size = New System.Drawing.Size(120, 43)
        Me.BtnBack.TabIndex = 54
        Me.BtnBack.Text = "Back"
        Me.BtnBack.UseVisualStyleBackColor = False
        '
        'PbUpload
        '
        Me.PbUpload.Location = New System.Drawing.Point(3, 338)
        Me.PbUpload.Name = "PbUpload"
        Me.PbUpload.Size = New System.Drawing.Size(693, 21)
        Me.PbUpload.TabIndex = 51
        '
        'TxtFile
        '
        Me.TxtFile.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtFile.Location = New System.Drawing.Point(212, 201)
        Me.TxtFile.Multiline = True
        Me.TxtFile.Name = "TxtFile"
        Me.TxtFile.Size = New System.Drawing.Size(411, 68)
        Me.TxtFile.TabIndex = 50
        '
        'BtnBrowse
        '
        Me.BtnBrowse.BackColor = System.Drawing.SystemColors.Control
        Me.BtnBrowse.BackgroundImage = CType(resources.GetObject("BtnBrowse.BackgroundImage"), System.Drawing.Image)
        Me.BtnBrowse.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.BtnBrowse.FlatAppearance.BorderSize = 0
        Me.BtnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.BtnBrowse.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnBrowse.ForeColor = System.Drawing.Color.Black
        Me.BtnBrowse.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnBrowse.Location = New System.Drawing.Point(629, 201)
        Me.BtnBrowse.Name = "BtnBrowse"
        Me.BtnBrowse.Size = New System.Drawing.Size(34, 29)
        Me.BtnBrowse.TabIndex = 49
        Me.BtnBrowse.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.BtnBrowse.UseVisualStyleBackColor = False
        '
        'Label10
        '
        Me.Label10.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.Black
        Me.Label10.Location = New System.Drawing.Point(193, 201)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(17, 29)
        Me.Label10.TabIndex = 48
        Me.Label10.Text = ":"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label11
        '
        Me.Label11.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.Black
        Me.Label11.Location = New System.Drawing.Point(65, 201)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(124, 29)
        Me.Label11.TabIndex = 46
        Me.Label11.Text = "File Template"
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Black
        Me.Label5.Location = New System.Drawing.Point(65, 162)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(124, 29)
        Me.Label5.TabIndex = 38
        Me.Label5.Text = "Month"
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Black
        Me.Label4.Location = New System.Drawing.Point(66, 120)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(123, 28)
        Me.Label4.TabIndex = 37
        Me.Label4.Text = "Year"
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Black
        Me.Label3.Location = New System.Drawing.Point(65, 32)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(124, 29)
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
        Me.BtnClose.Location = New System.Drawing.Point(305, 284)
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
        Me.CbDept.Location = New System.Drawing.Point(212, 32)
        Me.CbDept.Name = "CbDept"
        Me.CbDept.Size = New System.Drawing.Size(213, 29)
        Me.CbDept.TabIndex = 33
        '
        'BtnUpload
        '
        Me.BtnUpload.BackColor = System.Drawing.SystemColors.Control
        Me.BtnUpload.FlatAppearance.BorderSize = 0
        Me.BtnUpload.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.BtnUpload.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnUpload.ForeColor = System.Drawing.Color.Black
        Me.BtnUpload.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnUpload.Location = New System.Drawing.Point(179, 284)
        Me.BtnUpload.Name = "BtnUpload"
        Me.BtnUpload.Size = New System.Drawing.Size(120, 43)
        Me.BtnUpload.TabIndex = 31
        Me.BtnUpload.Text = "Upload"
        Me.BtnUpload.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Black
        Me.Label2.Location = New System.Drawing.Point(65, 76)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(124, 29)
        Me.Label2.TabIndex = 32
        Me.Label2.Text = "Kind of Data"
        '
        'Panel3
        '
        Me.Panel3.BackgroundImage = CType(resources.GetObject("Panel3.BackgroundImage"), System.Drawing.Image)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel3.Location = New System.Drawing.Point(0, 565)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Panel3.Size = New System.Drawing.Size(699, 3)
        Me.Panel3.TabIndex = 50
        '
        'BrowseFile
        '
        Me.BrowseFile.FileName = "OpenFileDialog1"
        '
        'FrmUploadDailyPlan
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(699, 568)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Panel3)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FrmUploadDailyPlan"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Daily plan to database"
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents CbMonth As System.Windows.Forms.ComboBox
    Friend WithEvents CbYear As System.Windows.Forms.ComboBox
    Friend WithEvents CbKind As System.Windows.Forms.ComboBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents BtnClose As System.Windows.Forms.Button
    Friend WithEvents CbDept As System.Windows.Forms.ComboBox
    Friend WithEvents BtnUpload As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents BtnBrowse As System.Windows.Forms.Button
    Friend WithEvents BrowseFile As System.Windows.Forms.OpenFileDialog
    Friend WithEvents TxtFile As System.Windows.Forms.TextBox
    Friend WithEvents PbUpload As System.Windows.Forms.ProgressBar
    Friend WithEvents BtnBack As System.Windows.Forms.Button
    Friend WithEvents TxtMessage As System.Windows.Forms.TextBox
End Class
