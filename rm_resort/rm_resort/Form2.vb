Imports MySql.Data.MySqlClient

Public Class Dashboardfrm
    ' 1. Database Connection
    Dim conn As New MySqlConnection("server=localhost;user=root;password=;database=db_resort")
    Dim selectedBookingID As Integer

    Private Sub Dashboardfrm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btnConfirmPaid.Enabled = False
        btnCancel.Enabled = False

        ' SETUP SCROLLING
        dgvBookings.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        dgvBookings.ScrollBars = ScrollBars.Both
        dgvBookings.DefaultCellStyle.WrapMode = DataGridViewTriState.False

        LoadBookings()
    End Sub

    ' =========================================================================
    ' 📊 DATA LOADING LOGIC
    ' =========================================================================
    Public Sub LoadBookings()
        Try
            If conn.State = ConnectionState.Closed Then conn.Open()

            ' Tinanggal lang ang Contact Number display
            Dim sql As String = "SELECT b.booking_id AS 'ID', " &
                                "b.guest_name AS 'Guest Name', " &
                                "b.guest_email AS 'Email', " &
                                "r.room_name AS 'Cottage/Room', " &
                                "b.payment_option AS 'Method', " &
                                "b.total_price AS 'Total', " &
                                "b.amount_paid AS 'Paid', " &
                                "(b.total_price - b.amount_paid) AS 'Balance', " &
                                "b.status AS 'Status', " &
                                "b.check_in_date AS 'Date' " &
                                "FROM bookings b " &
                                "INNER JOIN rooms r ON b.room_id = r.room_id " &
                                "WHERE b.status IN ('Pending', 'Partial') " &
                                "ORDER BY b.booking_id DESC"

            Dim adp As New MySqlDataAdapter(sql, conn)
            Dim dt As New DataTable
            adp.Fill(dt)
            dgvBookings.DataSource = dt

            ApplyGridFormatting()

        Catch ex As Exception
            MessageBox.Show("Error loading bookings: " & ex.Message)
        Finally
            conn.Close()
        End Try
    End Sub

    Private Sub ApplyGridFormatting()
        If dgvBookings.Columns.Count > 0 Then
            dgvBookings.Columns("ID").Visible = False
            dgvBookings.ReadOnly = True
            dgvBookings.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            dgvBookings.RowHeadersVisible = False

            dgvBookings.Columns("Email").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            dgvBookings.Columns("Guest Name").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

            dgvBookings.Columns("Cottage/Room").Width = 150
            dgvBookings.Columns("Method").Width = 100
            dgvBookings.Columns("Total").Width = 100
            dgvBookings.Columns("Paid").Width = 100
            dgvBookings.Columns("Balance").Width = 100
            dgvBookings.Columns("Status").Width = 100
            dgvBookings.Columns("Date").Width = 120

            ' Formatting Money
            Dim moneyCols() As String = {"Total", "Paid", "Balance"}
            For Each col In moneyCols
                dgvBookings.Columns(col).DefaultCellStyle.Format = "N2"
                dgvBookings.Columns(col).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            Next

            dgvBookings.Columns("Balance").DefaultCellStyle.ForeColor = Color.Red
            dgvBookings.Columns("Paid").DefaultCellStyle.ForeColor = Color.Blue
        End If
    End Sub

    ' =========================================================================
    ' 💰 PAYMENT LOGIC
    ' =========================================================================
    Private Sub btnConfirmPaid_Click(sender As Object, e As EventArgs) Handles btnConfirmPaid.Click
        If dgvBookings.SelectedRows.Count > 0 Then
            Dim bookingID As String = dgvBookings.CurrentRow.Cells("ID").Value.ToString()
            Dim guestName As String = dgvBookings.CurrentRow.Cells("Guest Name").Value.ToString()
            Dim totalDue As Decimal = Convert.ToDecimal(dgvBookings.CurrentRow.Cells("Total").Value)
            Dim currentPaid As Decimal = Convert.ToDecimal(dgvBookings.CurrentRow.Cells("Paid").Value)
            Dim currentBalance As Decimal = Convert.ToDecimal(dgvBookings.CurrentRow.Cells("Balance").Value)

            Dim inputAmount As String = InputBox("Guest: " & guestName & vbCrLf &
                                               "Total Price: ₱" & totalDue.ToString("N2") & vbCrLf &
                                               "Current Balance: ₱" & currentBalance.ToString("N2") & vbCrLf & vbCrLf &
                                               "Enter Amount to Pay:", "Payment", "0.00")

            Dim paymentInput As Decimal
            If Decimal.TryParse(inputAmount, paymentInput) AndAlso paymentInput > 0 Then
                Dim newTotalPaid As Decimal = currentPaid + paymentInput
                Dim newStatus As String = ""

                If newTotalPaid >= totalDue Then
                    newStatus = "Confirmed"
                Else
                    newStatus = "Partial"
                End If

                Try
                    If conn.State = ConnectionState.Closed Then conn.Open()
                    Dim sql As String = "UPDATE bookings SET amount_paid = @paid, status = @status WHERE booking_id = @id"
                    Dim cmd As New MySqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@paid", newTotalPaid)
                    cmd.Parameters.AddWithValue("@status", newStatus)
                    cmd.Parameters.AddWithValue("@id", bookingID)

                    If cmd.ExecuteNonQuery() > 0 Then
                        If newStatus = "Confirmed" Then
                            MessageBox.Show("Fully Paid! Booking confirmed.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Else
                            Dim remaining As Decimal = totalDue - newTotalPaid
                            MessageBox.Show("Partial payment recorded. Remaining balance: ₱" & remaining.ToString("N2"), "Partial Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        End If
                        LoadBookings()
                    End If
                Catch ex As Exception
                    MessageBox.Show("Error: " & ex.Message)
                Finally
                    conn.Close()
                End Try
            End If
        End If
    End Sub

    ' =========================================================================
    ' 🔍 SEARCH LOGIC
    ' =========================================================================
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles TxtSearch.TextChanged
        Try
            If conn.State = ConnectionState.Closed Then conn.Open()

            ' Tinanggal lang ang Contact Number display
            Dim sql As String = "SELECT b.booking_id AS 'ID', b.guest_name AS 'Guest Name', " &
                                "b.guest_email AS 'Email', " &
                                "r.room_name AS 'Cottage/Room', b.payment_option AS 'Method', " &
                                "b.total_price AS 'Total', b.amount_paid AS 'Paid', " &
                                "(b.total_price - b.amount_paid) AS 'Balance', " &
                                "b.status AS 'Status', b.check_in_date AS 'Date' " &
                                "FROM bookings b INNER JOIN rooms r ON r.room_id = b.room_id " &
                                "WHERE b.status IN ('Pending', 'Partial') AND b.guest_name LIKE @search " &
                                "ORDER BY b.booking_id DESC"

            Dim cmd As New MySqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@search", "%" & TxtSearch.Text & "%")

            Dim adp As New MySqlDataAdapter(cmd)
            Dim dt As New DataTable
            adp.Fill(dt)
            dgvBookings.DataSource = dt

            ApplyGridFormatting()

        Catch ex As Exception
        Finally
            conn.Close()
        End Try
    End Sub

    ' --- NAVIGATION & BUTTONS ---
    Private Sub dgvBookings_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvBookings.CellClick
        If e.RowIndex >= 0 Then
            selectedBookingID = Convert.ToInt32(dgvBookings.Rows(e.RowIndex).Cells("ID").Value)
            btnConfirmPaid.Enabled = True
            btnCancel.Enabled = True
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        If MessageBox.Show("Cancel this reservation?", "Confirm", MessageBoxButtons.YesNo) = DialogResult.Yes Then
            Try
                If conn.State = ConnectionState.Closed Then conn.Open()
                Dim cmd As New MySqlCommand("DELETE FROM bookings WHERE booking_id = @id", conn)
                cmd.Parameters.AddWithValue("@id", selectedBookingID)
                cmd.ExecuteNonQuery()
                LoadBookings()
            Catch ex As Exception
            Finally
                conn.Close()
            End Try
        End If
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadBookings()
        TxtSearch.Clear()
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        If MsgBox("Log out?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            Loginform.Show()
            Me.Dispose()
        End If
    End Sub

    Private Sub btnCurrent_Click(sender As Object, e As EventArgs) Handles btnCurrent.Click
        CurrentStayfrm.Show()
        Me.Hide()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        FrmConfirm.Show()
        Me.Hide()
    End Sub

    Private Sub btnSettings_Click(sender As Object, e As EventArgs) Handles btnSettings.Click
        FrmTotalIncome.Show()
        Me.Hide()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Application.Exit()
    End Sub

    Private Sub btnConfirmPaid_MouseEnter(sender As Object, e As EventArgs) Handles btnConfirmPaid.MouseEnter
        btnConfirmPaid.BackColor = ColorTranslator.FromHtml("#2ECC71")
    End Sub

    Private Sub btnConfirmPaid_MouseLeave(sender As Object, e As EventArgs) Handles btnConfirmPaid.MouseLeave
        btnConfirmPaid.BackColor = ColorTranslator.FromHtml("#27AE60")
    End Sub
End Class