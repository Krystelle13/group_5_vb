Imports MySql.Data.MySqlClient

Public Class Dashboardfrm
    ' 1. Database Connection
    Dim conn As New MySqlConnection("server=localhost;user=root;password=;database=db_resort")
    Dim selectedBookingID As Integer

    Private Sub Dashboardfrm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btnConfirmPaid.Enabled = False
        btnCancel.Enabled = False
        LoadBookings()
    End Sub

    ' =========================================================================
    ' 📊 DATA LOADING LOGIC
    ' =========================================================================
    Public Sub LoadBookings()
        Try
            If conn.State = ConnectionState.Closed Then conn.Open()

            ' QUERY UPDATE: Isinama ang 'Partial' status para lumabas pa rin sila sa listahan
            Dim sql As String = "SELECT b.booking_id AS 'ID', " &
                                "b.guest_name AS 'Guest Name', " &
                                "b.guest_email AS 'Email Address', " &
                                "r.room_name AS 'Cottage/Room', " &
                                "b.check_in_date AS 'Date', " &
                                "b.payment_option AS 'Payment', " &
                                "b.total_price AS 'Total', " &
                                "b.status AS 'Status' " &
                                "FROM bookings b " &
                                "INNER JOIN rooms r ON b.room_id = r.room_id " &
                                "WHERE b.status IN ('Pending', 'Partial') " &
                                "ORDER BY b.booking_id DESC"

            Dim adp As New MySqlDataAdapter(sql, conn)
            Dim dt As New DataTable
            dt.Clear()
            adp.Fill(dt)

            dgvBookings.DataSource = dt

            ' --- MODERN FORMATTING ---
            If dgvBookings.Columns.Count > 0 Then
                dgvBookings.Columns("ID").Visible = False

                ' Proteksyon sa Grid
                dgvBookings.ReadOnly = True
                dgvBookings.AllowUserToAddRows = False
                dgvBookings.RowHeadersVisible = False
                dgvBookings.SelectionMode = DataGridViewSelectionMode.FullRowSelect

                ' Column Widths
                dgvBookings.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
                dgvBookings.Columns("Guest Name").Width = 180
                dgvBookings.Columns("Email Address").Width = 200
                dgvBookings.Columns("Cottage/Room").Width = 180
                dgvBookings.Columns("Date").Width = 120
                dgvBookings.Columns("Payment").Width = 120
                dgvBookings.Columns("Total").Width = 100
                dgvBookings.Columns("Status").Width = 100

                ' Currency Format
                dgvBookings.Columns("Total").DefaultCellStyle.Format = "N2"
                dgvBookings.Columns("Total").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End If

            dgvBookings.ScrollBars = ScrollBars.Both

        Catch ex As Exception
            MessageBox.Show("Error loading bookings: " & ex.Message)
        Finally
            conn.Close()
        End Try
    End Sub

    ' =========================================================================
    ' 💰 PAYMENT & CONFIRMATION LOGIC (WITH PARTIAL/FULL TRIGGER)
    ' =========================================================================
    Private Sub btnConfirmPaid_Click(sender As Object, e As EventArgs) Handles btnConfirmPaid.Click
        If dgvBookings.SelectedRows.Count > 0 Then
            ' Kunin ang data mula sa selected row
            Dim bookingID As String = dgvBookings.CurrentRow.Cells("ID").Value.ToString()
            Dim guestName As String = dgvBookings.CurrentRow.Cells("Guest Name").Value.ToString()
            Dim totalPrice As Decimal = Convert.ToDecimal(dgvBookings.CurrentRow.Cells("Total").Value)

            ' 1. Pop-up InputBox para sa Amount Paid
            Dim inputPrompt As String = "Guest: " & guestName & vbCrLf &
                                      "Total Amount Due: ₱" & totalPrice.ToString("N2") & vbCrLf & vbCrLf &
                                      "Enter Amount Paid:"

            Dim inputAmount As String = InputBox(inputPrompt, "Payment Processing", "0.00")

            ' Validation ng input
            Dim amountPaid As Decimal
            If Decimal.TryParse(inputAmount, amountPaid) Then

                Dim newStatus As String = ""
                Dim finalMsg As String = ""

                ' 2. Logic for Full vs Partial
                If amountPaid >= totalPrice Then
                    newStatus = "Confirmed"
                    finalMsg = "Payment Successful! Booking is now FULLY PAID and moved to Confirmed List."
                ElseIf amountPaid > 0 Then
                    newStatus = "Partial"
                    finalMsg = "Partial Payment Recorded. Booking will remain in the list as 'Partial'."
                Else
                    MessageBox.Show("Payment cannot be zero or negative.", "Invalid Amount", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Exit Sub
                End If

                ' 3. Database Update
                Try
                    If conn.State = ConnectionState.Closed Then conn.Open()

                    Dim sql As String = "UPDATE bookings SET status = @status WHERE booking_id = @id"
                    Dim cmd As New MySqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@status", newStatus)
                    cmd.Parameters.AddWithValue("@id", bookingID)

                    If cmd.ExecuteNonQuery() > 0 Then
                        MessageBox.Show(finalMsg, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        LoadBookings() ' Refresh the list

                        ' Reset buttons
                        btnConfirmPaid.Enabled = False
                        btnCancel.Enabled = False
                    End If
                Catch ex As Exception
                    MessageBox.Show("Database Error: " & ex.Message)
                Finally
                    conn.Close()
                End Try
            ElseIf inputAmount <> "" Then ' If not empty but not a number
                MessageBox.Show("Please enter a valid numeric amount.", "Format Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Else
            MessageBox.Show("Please select a guest from the list first.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    ' =========================================================================
    ' 🔍 SEARCH & NAVIGATION
    ' =========================================================================
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles TxtSearch.TextChanged
        Try
            If conn.State = ConnectionState.Closed Then conn.Open()

            Dim sql As String = "SELECT b.booking_id AS 'ID', b.guest_name AS 'Guest Name', b.guest_email AS 'Email Address', " &
                                "r.room_name AS 'Cottage/Room', b.check_in_date AS 'Date', b.payment_option AS 'Payment', " &
                                "b.total_price AS 'Total', b.status AS 'Status' " &
                                "FROM bookings b INNER JOIN rooms r ON b.room_id = r.room_id " &
                                "WHERE b.status IN ('Pending', 'Partial') AND (b.guest_name LIKE @search OR b.guest_email LIKE @search) " &
                                "ORDER BY b.booking_id DESC"

            Dim cmd As New MySqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@search", "%" & TxtSearch.Text & "%")

            Dim adp As New MySqlDataAdapter(cmd)
            Dim dt As New DataTable
            adp.Fill(dt)
            dgvBookings.DataSource = dt
        Catch ex As Exception
        Finally
            conn.Close()
        End Try
    End Sub

    Private Sub dgvBookings_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvBookings.CellClick
        If e.RowIndex >= 0 Then
            selectedBookingID = Convert.ToInt32(dgvBookings.Rows(e.RowIndex).Cells("ID").Value)
            btnConfirmPaid.Enabled = True
            btnCancel.Enabled = True
        End If
    End Sub

    ' =========================================================================
    ' 🔘 BUTTON ACTIONS (Cancel, Logout, etc.)
    ' =========================================================================
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Dim result As DialogResult = MessageBox.Show("Are you sure you want to cancel this reservation?", "Confirm Cancellation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        If result = DialogResult.Yes Then
            Try
                If conn.State = ConnectionState.Closed Then conn.Open()
                Dim cmd As New MySqlCommand("DELETE FROM bookings WHERE booking_id = @id", conn)
                cmd.Parameters.AddWithValue("@id", selectedBookingID)

                If cmd.ExecuteNonQuery() > 0 Then
                    MessageBox.Show("Reservation Cancelled Successfully.")
                    LoadBookings()
                    btnConfirmPaid.Enabled = False
                    btnCancel.Enabled = False
                End If
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            Finally
                conn.Close()
            End Try
        End If
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadBookings()
        TxtSearch.Clear()
        btnConfirmPaid.Enabled = False
        btnCancel.Enabled = False
    End Sub

    ' Sidebar Navigation
    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        If MsgBox("Are you sure you want to log out?", MsgBoxStyle.YesNo + MsgBoxStyle.Question) = MsgBoxResult.Yes Then
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
        If MessageBox.Show("Exit Application?", "Confirm", MessageBoxButtons.OKCancel) = DialogResult.OK Then
            Application.Exit()
        End If
    End Sub

    ' Hover Effects
    Private Sub btnConfirmPaid_MouseEnter(sender As Object, e As EventArgs) Handles btnConfirmPaid.MouseEnter
        btnConfirmPaid.BackColor = ColorTranslator.FromHtml("#2ECC71")
    End Sub
    Private Sub btnConfirmPaid_MouseLeave(sender As Object, e As EventArgs) Handles btnConfirmPaid.MouseLeave
        btnConfirmPaid.BackColor = ColorTranslator.FromHtml("#27AE60")
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub
End Class