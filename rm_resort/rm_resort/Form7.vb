Imports MySql.Data.MySqlClient

Public Class FrmTotalIncome

    ' =========================================================================
    ' 📊 DATA LOADING LOGIC (Revenue & Counts)
    ' =========================================================================
    Public Sub LoadAllDashboardData()
        Try
            ' Siguraduhin na bukas ang koneksyon
            If conn.State = ConnectionState.Closed Then conn.Open()

            ' 1. TOTAL REVENUE (Sum ng pera)
            ' Gagamit tayo ng LIKE '%Confirmed%' para siguradong mahanap kahit may konting typo sa database
            LoadStatusCount("SELECT COALESCE(SUM(total_price), 0) AS 'GRAND TOTAL REVENUE' FROM bookings WHERE status LIKE '%Confirmed%'", dgvTotal)

            ' 2. TOTAL PENDING (Bilang ng naghihintay)
            LoadStatusCount("SELECT COUNT(*) AS 'TOTAL PENDING' FROM bookings WHERE status LIKE '%Pending%'", dgvPend)

            ' 3. TOTAL CONFIRMED (Bilang ng mga bayad na)
            LoadStatusCount("SELECT COUNT(*) AS 'TOTAL CONFIRMED' FROM bookings WHERE status LIKE '%Confirmed%'", dgvconfirm)

            ' 4. TOTAL STAYING (Bilang ng mga naka-check in)
            LoadStatusCount("SELECT COUNT(*) AS 'TOTAL STAYING' FROM bookings WHERE status LIKE '%Staying%'", dgvcurrent)

            ' Pag-apply ng visual styles sa bawat grid
            StyleGrid(dgvTotal, True)  ' True dahil Currency (₱) ito
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
        Catch ex As Exception
            ' Silent error para sa individual grids
        End Try
    End Sub

    ' Helper Function para sa Styling (Para magmukhang Total Boxes)
    Private Sub StyleGrid(dgv As DataGridView, isCurrency As Boolean)
        If dgv.Columns.Count > 0 Then
            With dgv.DefaultCellStyle
                If isCurrency Then .Format = "₱ #,##0.00"
                .Alignment = DataGridViewContentAlignment.MiddleCenter
                .Font = New Font("Segoe UI", 16, FontStyle.Bold)
                .ForeColor = Color.DarkBlue
            End With

            ' Linisin ang itsura ng Grid
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            dgv.RowHeadersVisible = False
            dgv.AllowUserToAddRows = False
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            dgv.EnableHeadersVisualStyles = False
            dgv.ScrollBars = ScrollBars.None
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
        MessageBox.Show("All totals updated!", "Island Aura System", MessageBoxButtons.OK, MessageBoxIcon.Information)
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

    ' Iba pang button handlers (I-paste lang ang navigation logic mo dito gaya ng dati)
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim f1 As New FrmConfirm
        f1.Show()
        Me.Hide()
    End Sub

    Private Sub btnSettings_Click(sender As Object, e As EventArgs) Handles btnSettings.Click
        ' Maintain current form
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvPend.CellContentClick

    End Sub
End Class