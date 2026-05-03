<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmTotalIncome
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmTotalIncome))
        Me.tmrRefresh = New System.Windows.Forms.Timer(Me.components)
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ColorDialog1 = New System.Windows.Forms.ColorDialog()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ContextMenuStrip2 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.btnLogout = New System.Windows.Forms.Button()
        Me.btnSettings = New System.Windows.Forms.Button()
        Me.btnCurrent = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.btnConfirm = New System.Windows.Forms.Button()
        Me.ColorDialog2 = New System.Windows.Forms.ColorDialog()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.dgvTotal = New System.Windows.Forms.DataGridView()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.dgvPend = New System.Windows.Forms.DataGridView()
        Me.dgvconfirm = New System.Windows.Forms.DataGridView()
        Me.dgvcurrent = New System.Windows.Forms.DataGridView()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.Panel1.SuspendLayout()
        CType(Me.dgvTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvPend, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvconfirm, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvcurrent, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tmrRefresh
        '
        Me.tmrRefresh.Enabled = True
        Me.tmrRefresh.Interval = 5000
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(61, 4)
        '
        'btnRefresh
        '
        Me.btnRefresh.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnRefresh.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRefresh.Location = New System.Drawing.Point(1198, -2)
        Me.btnRefresh.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(84, 38)
        Me.btnRefresh.TabIndex = 23
        Me.btnRefresh.Text = "↻"
        Me.btnRefresh.UseVisualStyleBackColor = False
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 5000
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(76, Byte), Integer), CType(CType(60, Byte), Integer))
        Me.Button2.Location = New System.Drawing.Point(1289, -3)
        Me.Button2.Margin = New System.Windows.Forms.Padding(4)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(84, 38)
        Me.Button2.TabIndex = 27
        Me.Button2.Text = "X"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft YaHei", 21.75!, System.Drawing.FontStyle.Bold)
        Me.Label1.Location = New System.Drawing.Point(532, 111)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(209, 39)
        Me.Label1.TabIndex = 22
        Me.Label1.Text = "Total Income"
        '
        'ContextMenuStrip2
        '
        Me.ContextMenuStrip2.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip2.Size = New System.Drawing.Size(61, 4)
        '
        'btnLogout
        '
        Me.btnLogout.BackColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(76, Byte), Integer), CType(CType(60, Byte), Integer))
        Me.btnLogout.FlatAppearance.BorderSize = 0
        Me.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLogout.Font = New System.Drawing.Font("Segoe UI", 21.75!, System.Drawing.FontStyle.Bold)
        Me.btnLogout.ForeColor = System.Drawing.Color.Snow
        Me.btnLogout.Location = New System.Drawing.Point(-21, 606)
        Me.btnLogout.Margin = New System.Windows.Forms.Padding(4)
        Me.btnLogout.Name = "btnLogout"
        Me.btnLogout.Size = New System.Drawing.Size(436, 104)
        Me.btnLogout.TabIndex = 11
        Me.btnLogout.Text = "Logout"
        Me.btnLogout.UseVisualStyleBackColor = False
        '
        'btnSettings
        '
        Me.btnSettings.FlatAppearance.BorderSize = 0
        Me.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSettings.Font = New System.Drawing.Font("Segoe UI", 21.75!, System.Drawing.FontStyle.Bold)
        Me.btnSettings.ForeColor = System.Drawing.Color.Orange
        Me.btnSettings.Location = New System.Drawing.Point(-30, 117)
        Me.btnSettings.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSettings.Name = "btnSettings"
        Me.btnSettings.Size = New System.Drawing.Size(436, 104)
        Me.btnSettings.TabIndex = 10
        Me.btnSettings.Text = "Dashboard"
        Me.btnSettings.UseVisualStyleBackColor = True
        '
        'btnCurrent
        '
        Me.btnCurrent.FlatAppearance.BorderSize = 0
        Me.btnCurrent.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCurrent.Font = New System.Drawing.Font("Segoe UI", 21.75!, System.Drawing.FontStyle.Bold)
        Me.btnCurrent.ForeColor = System.Drawing.Color.Orange
        Me.btnCurrent.Location = New System.Drawing.Point(-21, 446)
        Me.btnCurrent.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCurrent.Name = "btnCurrent"
        Me.btnCurrent.Size = New System.Drawing.Size(436, 104)
        Me.btnCurrent.TabIndex = 9
        Me.btnCurrent.Text = "Currently Staying"
        Me.btnCurrent.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.FlatAppearance.BorderSize = 0
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.Font = New System.Drawing.Font("Segoe UI", 21.75!, System.Drawing.FontStyle.Bold)
        Me.Button1.ForeColor = System.Drawing.Color.Orange
        Me.Button1.Location = New System.Drawing.Point(-21, 323)
        Me.Button1.Margin = New System.Windows.Forms.Padding(4)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(436, 104)
        Me.Button1.TabIndex = 8
        Me.Button1.Text = "Confirmed"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'btnConfirm
        '
        Me.btnConfirm.FlatAppearance.BorderSize = 0
        Me.btnConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnConfirm.Font = New System.Drawing.Font("Segoe UI", 21.75!, System.Drawing.FontStyle.Bold)
        Me.btnConfirm.ForeColor = System.Drawing.Color.Orange
        Me.btnConfirm.Location = New System.Drawing.Point(-30, 220)
        Me.btnConfirm.Margin = New System.Windows.Forms.Padding(4)
        Me.btnConfirm.Name = "btnConfirm"
        Me.btnConfirm.Size = New System.Drawing.Size(436, 104)
        Me.btnConfirm.TabIndex = 6
        Me.btnConfirm.Text = "Pending"
        Me.btnConfirm.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(62, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.Panel1.Controls.Add(Me.PictureBox2)
        Me.Panel1.Controls.Add(Me.btnLogout)
        Me.Panel1.Controls.Add(Me.btnSettings)
        Me.Panel1.Controls.Add(Me.btnCurrent)
        Me.Panel1.Controls.Add(Me.Button1)
        Me.Panel1.Controls.Add(Me.btnConfirm)
        Me.Panel1.ForeColor = System.Drawing.Color.Chocolate
        Me.Panel1.Location = New System.Drawing.Point(-2, -6)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(406, 776)
        Me.Panel1.TabIndex = 20
        '
        'dgvTotal
        '
        Me.dgvTotal.AccessibleRole = System.Windows.Forms.AccessibleRole.PageTab
        Me.dgvTotal.AllowUserToResizeColumns = False
        Me.dgvTotal.AllowUserToResizeRows = False
        Me.dgvTotal.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvTotal.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.dgvTotal.BackgroundColor = System.Drawing.Color.RoyalBlue
        Me.dgvTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvTotal.ColumnHeadersVisible = False
        Me.dgvTotal.Location = New System.Drawing.Point(490, 190)
        Me.dgvTotal.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvTotal.Name = "dgvTotal"
        Me.dgvTotal.RowHeadersVisible = False
        Me.dgvTotal.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.dgvTotal.Size = New System.Drawing.Size(303, 66)
        Me.dgvTotal.TabIndex = 21
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft YaHei", 21.75!, System.Drawing.FontStyle.Bold)
        Me.Label2.Location = New System.Drawing.Point(1008, 111)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(222, 39)
        Me.Label2.TabIndex = 31
        Me.Label2.Text = "Total Pending"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft YaHei", 21.75!, System.Drawing.FontStyle.Bold)
        Me.Label3.Location = New System.Drawing.Point(532, 413)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(220, 39)
        Me.Label3.TabIndex = 32
        Me.Label3.Text = "Total Confirm"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft YaHei", 21.75!, System.Drawing.FontStyle.Bold)
        Me.Label4.Location = New System.Drawing.Point(951, 413)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(331, 39)
        Me.Label4.TabIndex = 33
        Me.Label4.Text = "Total Current Staying"
        '
        'dgvPend
        '
        Me.dgvPend.AccessibleRole = System.Windows.Forms.AccessibleRole.PageTab
        Me.dgvPend.AllowUserToResizeColumns = False
        Me.dgvPend.AllowUserToResizeRows = False
        Me.dgvPend.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvPend.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.dgvPend.BackgroundColor = System.Drawing.Color.RoyalBlue
        Me.dgvPend.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPend.ColumnHeadersVisible = False
        Me.dgvPend.Location = New System.Drawing.Point(967, 190)
        Me.dgvPend.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvPend.Name = "dgvPend"
        Me.dgvPend.RowHeadersVisible = False
        Me.dgvPend.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.dgvPend.Size = New System.Drawing.Size(303, 66)
        Me.dgvPend.TabIndex = 34
        '
        'dgvconfirm
        '
        Me.dgvconfirm.AccessibleRole = System.Windows.Forms.AccessibleRole.PageTab
        Me.dgvconfirm.AllowUserToResizeColumns = False
        Me.dgvconfirm.AllowUserToResizeRows = False
        Me.dgvconfirm.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvconfirm.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.dgvconfirm.BackgroundColor = System.Drawing.Color.RoyalBlue
        Me.dgvconfirm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvconfirm.ColumnHeadersVisible = False
        Me.dgvconfirm.Location = New System.Drawing.Point(490, 494)
        Me.dgvconfirm.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvconfirm.Name = "dgvconfirm"
        Me.dgvconfirm.RowHeadersVisible = False
        Me.dgvconfirm.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.dgvconfirm.Size = New System.Drawing.Size(303, 66)
        Me.dgvconfirm.TabIndex = 35
        '
        'dgvcurrent
        '
        Me.dgvcurrent.AccessibleRole = System.Windows.Forms.AccessibleRole.PageTab
        Me.dgvcurrent.AllowUserToResizeColumns = False
        Me.dgvcurrent.AllowUserToResizeRows = False
        Me.dgvcurrent.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvcurrent.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.dgvcurrent.BackgroundColor = System.Drawing.Color.RoyalBlue
        Me.dgvcurrent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvcurrent.ColumnHeadersVisible = False
        Me.dgvcurrent.Location = New System.Drawing.Point(967, 494)
        Me.dgvcurrent.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvcurrent.Name = "dgvcurrent"
        Me.dgvcurrent.RowHeadersVisible = False
        Me.dgvcurrent.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.dgvcurrent.Size = New System.Drawing.Size(303, 66)
        Me.dgvcurrent.TabIndex = 36
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(97, 3)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(187, 107)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox2.TabIndex = 12
        Me.PictureBox2.TabStop = False
        '
        'FrmTotalIncome
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1386, 738)
        Me.ControlBox = False
        Me.Controls.Add(Me.dgvcurrent)
        Me.Controls.Add(Me.dgvconfirm)
        Me.Controls.Add(Me.dgvPend)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnRefresh)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.dgvTotal)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "FrmTotalIncome"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "DashboardForm"
        Me.Panel1.ResumeLayout(False)
        CType(Me.dgvTotal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvPend, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvconfirm, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvcurrent, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tmrRefresh As Timer
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents ColorDialog1 As ColorDialog
    Friend WithEvents btnRefresh As Button
    Friend WithEvents Timer1 As Timer
    Friend WithEvents Button2 As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents ContextMenuStrip2 As ContextMenuStrip
    Friend WithEvents btnLogout As Button
    Friend WithEvents btnSettings As Button
    Friend WithEvents btnCurrent As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents btnConfirm As Button
    Friend WithEvents ColorDialog2 As ColorDialog
    Friend WithEvents Panel1 As Panel
    Friend WithEvents dgvTotal As DataGridView
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents dgvPend As DataGridView
    Friend WithEvents dgvconfirm As DataGridView
    Friend WithEvents dgvcurrent As DataGridView
    Friend WithEvents PictureBox2 As PictureBox
End Class
