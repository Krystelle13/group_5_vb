Imports MySql.Data.MySqlClient

Public Class FrmTotalIncome
    ' Connection string
    Dim conn As New MySqlConnection("server=localhost;user=root;password=;database=db_resort")

    ' =========================================================================
    ' 📊 DATA LOADING LOGIC (Revenue & Counts)
    ' =========================================================================
    Public Sub LoadAllDashboardData()
        Try
            If conn.State = ConnectionState.Closed Then conn.Open()

            ' 1. TOTAL REVENUE (Confirmed, Staying, Checked Out only)
            ' Hindi kasama ang 'Pending' at 'Partial' dito para hindi mag-zero ang dashboard mo sa revenue
            Dim incomeQuery As String = "SELECT COALESCE(SUM(total_price), 0) AS 'GRAND TOTAL REVENUE' " &
                                      "FROM bookings " &
                                      "WHERE status IN ('Confirmed', 'Staying', 'Checked Out')"
            LoadStatusCount(incomeQuery, dgvTotal)

            ' 2. TOTAL PENDING (Kasama ang 'Pending' at 'Partial' status)
            ' Dito natin sinisiguro na kahit 'Partial' na ang status, counted pa rin sya sa Pending List count
            Dim pendingQuery As String = "SELECT COUNT(*) AS 'TOTAL PENDING' FROM bookings WHERE status IN ('Pending', 'Partial')"
            LoadStatusCount(pendingQuery, dgvPend)

            ' 3. TOTAL CONFIRMED
            LoadStatusCount("SELECT COUNT(*) AS 'TOTAL CONFIRMED' FROM bookings WHERE status = 'Confirmed'", dgvconfirm)

            ' 4. TOTAL STAYING
            LoadStatusCount("SELECT COUNT(*) AS 'TOTAL STAYING' FROM bookings WHERE status = 'Staying'", dgvcurrent)

            ' Apply visual styles for grid
            StyleGrid(dgvTotal, True)  ' True = money (₱)
            StyleGrid(dgvPend, False)
            StyleGrid(dgvconfirm, False)
            StyleGrid(dgvcurrent, False)

        Catch ex As Exception
            MessageBox.Show("Error loading dashboard: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            conn.Close()
        End Try
    End Sub

    ' Helper Function para mag-load ng data sa specific DataGridView
    Private Sub LoadStatusCount(query As String, dgv As DataGridView)
        Try
            Dim adp As New MySqlDataAdapter(query, conn)
            Dim dt As New DataTable
            dt.Clear()
            adp.Fill(dt)
            dgv.DataSource = dt
            dgv.ClearSelection()
        Catch ex As Exception
            ' Silent error
        End Try
    End Sub

    ' =========================================================================
    ' 🔒 FIXED STYLING (CENTRALIZED & NON-EDITABLE)
    ' =========================================================================
    Private Sub StyleGrid(dgv As DataGridView, isCurrency As Boolean)
        If dgv.Columns.Count > 0 Then
            ' Protection settings
            dgv.ReadOnly = True
            dgv.Enabled = False
            dgv.AllowUserToAddRows = False
            dgv.AllowUserToDeleteRows = False
            dgv.AllowUserToOrderColumns = False
            dgv.AllowUserToResizeColumns = False
            dgv.AllowUserToResizeRows = False

            dgv.ColumnHeadersVisible = False
            dgv.RowHeadersVisible = False
            dgv.ScrollBars = ScrollBars.None
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            dgv.BackgroundColor = Color.White
            dgv.BorderStyle = BorderStyle.None

            ' Tinatanggal ang internal border para mas malinis ang alignment
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.None

            With dgv.DefaultCellStyle
                If isCurrency Then
                    .Format = "₱ #,##0.00"
                    .Font = New Font("Segoe UI", 18, FontStyle.Bold)
                Else
                    .Format = "N0"
                    .Font = New Font("Segoe UI", 24, FontStyle.Bold)
                End If

                ' ITO ANG PINAKAMAHALAGA: Centralization
                .Alignment = DataGridViewContentAlignment.MiddleCenter
                .Padding = New Padding(0) ' Siguraduhin na walang extra space

                .WrapMode = DataGridViewTriState.False
                .ForeColor = Color.FromArgb(0, 51, 102) ' Dark Blue
                .BackColor = Color.White
                .SelectionBackColor = Color.White
                .SelectionForeColor = Color.FromArgb(0, 51, 102)
            End With

            ' Pinupuwersa ang Row Height na maging kasing laki ng DataGridView control
            ' para ang "MiddleCenter" alignment ay gumana vertical-wise.
            If dgv.Rows.Count > 0 Then
                dgv.Rows(0).Height = dgv.Height
            End If
        End If
    End Sub

    ' =========================================================================
    ' 🖱️ NAVIGATION & BUTTON EVENTS
    ' =========================================================================

    Private Sub FrmTotalIncome_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadAllDashboardData()
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadAllDashboardData()
    End Sub

    Private Sub btnCurrent_Click(sender As Object, e As EventArgs) Handles btnCurrent.Click
        Dim f1 As New CurrentStayfrm
        f1.Show()
        Me.Hide()
    End Sub

    Private Sub btnConfirm_Click(sender As Object, e As EventArgs) Handles btnConfirm.Click
        Dim f1 As New Dashboardfrm
        f1.Show()
        Me.Hide()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim f1 As New FrmConfirm
        f1.Show()
        Me.Hide()
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        Dim response = MsgBox("Are you sure you want to log out?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Logout")
        If response = MsgBoxResult.Yes Then
            Dim login As New Loginform()
            login.Show()
            Me.Dispose()
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Application.Exit()
    End Sub
End Class