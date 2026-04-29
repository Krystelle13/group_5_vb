Imports MySql.Data.MySqlClient

Public Class Dashboardfrm
    ' 1. Siguraduhin na nandito ang connection variable mo
    Dim conn As New MySqlConnection("server=localhost;user=root;password=;database=db_resort")

    Private Sub Dashboardfrm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btnConfirmPaid.Enabled = False
        btnCancel.Enabled = False ' Disabled ang cancel button sa simula
        LoadBookings()
        LoadBookings() ' Dito tinatawag ang function sa ibaba
    End Sub

    ' 2. Ito ang function na hinahanap ng Load event mo
    Public Sub LoadBookings()
        Try
            If conn.State = ConnectionState.Closed Then conn.Open()

            ' Query: Idinagdag ang guest_email at naka-INNER JOIN para sa cottage name
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
                            "WHERE b.status = 'Pending' " &
                            "ORDER BY b.booking_id DESC"

            Dim adp As New MySqlDataAdapter(sql, conn)
            Dim dt As New DataTable
            dt.Clear()
            adp.Fill(dt)

            ' I-set ang DataSource
            dgvBookings.DataSource = dt

            ' --- FORMATTING & HIDING ID ---
            If dgvBookings.Columns.Count > 0 Then
                ' 1. Itago ang ID column
                dgvBookings.Columns("ID").Visible = False

                ' 2. I-set ang lapad ng columns para maging scrollable
                dgvBookings.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
                dgvBookings.Columns("Guest Name").Width = 180
                dgvBookings.Columns("Email Address").Width = 200 ' Bagong column
                dgvBookings.Columns("Cottage/Room").Width = 180
                dgvBookings.Columns("Date").Width = 120
                dgvBookings.Columns("Payment").Width = 120
                dgvBookings.Columns("Total").Width = 100
                dgvBookings.Columns("Status").Width = 100

                ' 3. Format para sa presyo (₱ 0.00)
                dgvBookings.Columns("Total").DefaultCellStyle.Format = "N2"
            End If

            ' Siguraduhin na pwedeng mag-scroll pakanan
            dgvBookings.ScrollBars = ScrollBars.Both

        Catch ex As Exception
            MessageBox.Show("Error loading bookings: " & ex.Message)
        Finally
            conn.Close()
        End Try
    End Sub



    Dim selectedBookingID As Integer



    Private Sub dgvBookings_CellContentDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvBookings.CellContentDoubleClick
        ' Siguraduhin na ang row index ay valid (hindi header)
        If e.RowIndex >= 0 Then
            ' I-enable ang mga buttons dahil may napili na
            btnConfirmPaid.Enabled = True
            btnCancel.Enabled = True

            ' (Optional) Pwede mo ring i-select ang buong row para visual guide
            dgvBookings.Rows(e.RowIndex).Selected = True

            Dim guestName As String = dgvBookings.Rows(e.RowIndex).Cells("Guest Name").Value.ToString()
            ' MsgBox("Selected: " & guestName) ' Pwede itong gamitin para sa testing
        End If
    End Sub

    Private Sub btnConfirmPaid_Click(sender As Object, e As EventArgs) Handles btnConfirmPaid.Click
        ' 1. Siguraduhin na may napiling customer sa DataGridView
        If dgvBookings.SelectedRows.Count > 0 Then
            ' Kunin ang ID mula sa hidden column at ang pangalan para sa message
            Dim bookingID As String = dgvBookings.CurrentRow.Cells("ID").Value.ToString()
            Dim guestName As String = dgvBookings.CurrentRow.Cells("Guest Name").Value.ToString()

            ' 2. Magpakita ng confirmation prompt
            Dim ask As DialogResult = MessageBox.Show("Confirm payment and booking for " & guestName & "?", "Confirm Action", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If ask = DialogResult.Yes Then
                Try
                    If conn.State = ConnectionState.Closed Then conn.Open()

                    ' 3. SQL Update: Lilipat ang status sa 'Confirmed'
                    Dim sql As String = "UPDATE bookings SET status = 'Confirmed' WHERE booking_id = @id"
                    Dim cmd As New MySqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@id", bookingID)

                    Dim result As Integer = cmd.ExecuteNonQuery()

                    If result > 0 Then
                        MessageBox.Show("Booking successfully transferred to Confirmed List!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                        ' 4. Automatic Refresh: Mawawala na siya sa Dashboard/Pending list
                        LoadBookings()
                    End If

                Catch ex As Exception
                    MessageBox.Show("Error updating booking: " & ex.Message)
                Finally
                    conn.Close()
                End Try
            End If
        Else
            MessageBox.Show("Please select a customer from the list first.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim f1 As New FrmConfirm
        f1.Show()
        Me.Hide()
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles TxtSearch.TextChanged
        Try
            If conn.State = ConnectionState.Closed Then conn.Open()

            ' Query para sa Search (Same structure sa LoadBookings)
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
                            "WHERE b.status = 'Pending' AND (b.guest_name LIKE @search OR b.guest_email LIKE @search) " &
                            "ORDER BY b.booking_id DESC"

            Dim cmd As New MySqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@search", "%" & TxtSearch.Text & "%")

            Dim adp As New MySqlDataAdapter(cmd)
            Dim dt As New DataTable
            dt.Clear()
            adp.Fill(dt)

            dgvBookings.DataSource = dt

            ' --- APPLY SAME FORMATTING PARA HINDI MAGBAGO ANG ITSURA ---
            If dgvBookings.Columns.Count > 0 Then
                dgvBookings.Columns("ID").Visible = False
                dgvBookings.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
                dgvBookings.Columns("Guest Name").Width = 180
                dgvBookings.Columns("Email Address").Width = 200
                dgvBookings.Columns("Cottage/Room").Width = 180
                dgvBookings.Columns("Date").Width = 120
                dgvBookings.Columns("Payment").Width = 120
                dgvBookings.Columns("Total").Width = 100
                dgvBookings.Columns("Status").Width = 100

                dgvBookings.Columns("Total").DefaultCellStyle.Format = "N2"
            End If

        Catch ex As Exception
            ' Tahimik na error para hindi istorbo sa pag-type
        Finally
            conn.Close()
        End Try
    End Sub

    Private Sub dgvBookings_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvBookings.CellDoubleClick
        If e.RowIndex >= 0 Then
            ' Kunin ang ID ng piniling customer
            selectedBookingID = dgvBookings.Rows(e.RowIndex).Cells(0).Value

            ' Enable pareho ang buttons
            btnConfirmPaid.Enabled = True
            btnCancel.Enabled = True

            MessageBox.Show("Selected Guest: " & dgvBookings.Rows(e.RowIndex).Cells(1).Value.ToString())
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        ' Magtanong muna bago i-cancel
        Dim result As DialogResult = MessageBox.Show("Are you sure you want to cancel this reservation?", "Confirm Cancellation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

        If result = DialogResult.Yes Then
            Try
                If conn.State = ConnectionState.Closed Then conn.Open()

                ' Query para burahin ang record (o pwede ring i-update ang status to 'Cancelled')
                Dim cmd As New MySqlCommand("DELETE FROM bookings WHERE booking_id = @id", conn)
                cmd.Parameters.AddWithValue("@id", selectedBookingID)

                If cmd.ExecuteNonQuery() > 0 Then
                    MessageBox.Show("Reservation Cancelled Successfully.")
                    LoadBookings() ' I-refresh ang grid para mawala na ang pangalan

                    ' I-disable ulit ang mga buttons
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



    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        ' 1. Ask for confirmation so they don't log out by mistake
        Dim response = MsgBox("Are you sure you want to log out?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Logout")

        If response = MsgBoxResult.Yes Then
            ' 2. Create a new instance of your Login Form (Form2)
            Dim login As New Loginform()

            ' 3. Show the login form
            login.Show()

            ' 4. Close this Main Form completely
            Me.Dispose()
        End If

    End Sub

    Private Sub btnCurrent_Click(sender As Object, e As EventArgs) Handles btnCurrent.Click
        Dim f1 As New CurrentStayfrm
        f1.Show()
        Me.Hide()
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        ' 1. Reload the data from the database
        LoadBookings()

        ' 2. Clear the search text box
        TxtSearch.Clear()

        ' 3. Reset buttons to disabled state for safety
        btnConfirmPaid.Enabled = False
        btnCancel.Enabled = False

        ' 4. Optional: Inform the user
        ' MessageBox.Show("Data refreshed successfully.", "Refresh", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
    ' FOR THE CONFIRMED PAID BUTTON
    Private Sub btnConfirmPaid_MouseEnter(sender As Object, e As EventArgs) Handles btnConfirmPaid.MouseEnter
        ' Change to a slightly lighter green on hover
        btnConfirmPaid.BackColor = ColorTranslator.FromHtml("#2ECC71")
    End Sub

    Private Sub btnConfirmPaid_MouseLeave(sender As Object, e As EventArgs) Handles btnConfirmPaid.MouseLeave
        ' Return to original professional green
        btnConfirmPaid.BackColor = ColorTranslator.FromHtml("#27AE60")
    End Sub

    ' FOR THE CANCEL BUTTON
    Private Sub btnCancel_MouseEnter(sender As Object, e As EventArgs) Handles btnCancel.MouseEnter
        btnCancel.BackColor = ColorTranslator.FromHtml("#FF5C5C")
    End Sub

    Private Sub btnCancel_MouseLeave(sender As Object, e As EventArgs) Handles btnCancel.MouseLeave
        btnCancel.BackColor = ColorTranslator.FromHtml("#E74C3C")
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim result As DialogResult = MessageBox.Show("Are you sure you want to Exit?", "Confirm", MessageBoxButtons.OKCancel)
        If result = DialogResult.OK Then
            Application.Exit()
        End If
    End Sub

    Private Sub dgvBookings_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvBookings.CellContentClick

    End Sub

    Private Sub btnSettings_Click(sender As Object, e As EventArgs) Handles btnSettings.Click
        Dim f1 As New FrmTotalIncome
        f1.Show()
        Me.Hide()
    End Sub
End Class