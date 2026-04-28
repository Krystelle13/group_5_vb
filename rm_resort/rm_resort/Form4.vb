Imports MySql.Data.MySqlClient

Public Class FrmConfirm
    ' 1. Connection string (Dapat pareho sa ginamit mo sa Dashboard)
    Dim conn As New MySqlConnection("server=localhost;user=root;password=;database=db_resort")

    ' 2. Pagka-load ng Form, kusa niyang tatawagin ang listahan
    Private Sub FrmConfirm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadConfirmedBookings()
        btnCheckin.Enabled = False ' Disable sa simula

    End Sub

    ' 3. Function para hulaan ang mga 'Paid' customers
    Public Sub LoadConfirmedBookings()
        Try
            If conn.State = ConnectionState.Closed Then conn.Open()

            ' Query: Kumpleto ang details, status ay 'Confirmed'
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
                            "WHERE b.status = 'Confirmed' " &
                            "ORDER BY b.booking_id DESC"

            Dim adp As New MySqlDataAdapter(sql, conn)
            Dim dt As New DataTable
            dt.Clear()
            adp.Fill(dt)

            dgvConfirmed.DataSource = dt

            ' --- FORMATTING: SAME SA PENDING PARA CONSISTENT ---
            If dgvConfirmed.Columns.Count > 0 Then
                ' Itago ang ID
                dgvConfirmed.Columns("ID").Visible = False

                ' Lapad ng Columns para scrollable
                dgvConfirmed.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
                dgvConfirmed.Columns("Guest Name").Width = 180
                dgvConfirmed.Columns("Email Address").Width = 200
                dgvConfirmed.Columns("Cottage/Room").Width = 180
                dgvConfirmed.Columns("Date").Width = 120
                dgvConfirmed.Columns("Payment").Width = 120
                dgvConfirmed.Columns("Total").Width = 100
                dgvConfirmed.Columns("Status").Width = 100

                dgvConfirmed.Columns("Total").DefaultCellStyle.Format = "N2"
            End If

            dgvConfirmed.ScrollBars = ScrollBars.Both

        Catch ex As Exception
            MessageBox.Show("Error loading confirmed list: " & ex.Message)
        Finally
            conn.Close()
        End Try
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

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles TxtSearch.TextChanged
        Try
            If conn.State = ConnectionState.Closed Then conn.Open()

            Dim sql As String = "SELECT b.booking_id AS 'ID', b.guest_name AS 'Guest Name', b.guest_email AS 'Email Address', " &
                                "r.room_name AS 'Cottage/Room', b.check_in_date AS 'Date', b.payment_option AS 'Payment', " &
                                "b.total_price AS 'Total', b.status AS 'Status' FROM bookings b " &
                                "INNER JOIN rooms r ON b.room_id = r.room_id " &
                                "WHERE b.status = 'Confirmed' AND (b.guest_name LIKE @s OR b.guest_email LIKE @s) " &
                                "ORDER BY b.booking_id DESC"

            Dim cmd As New MySqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@s", "%" & TxtSearch.Text & "%")

            Dim adp As New MySqlDataAdapter(cmd)
            Dim dt As New DataTable
            adp.Fill(dt)

            dgvConfirmed.DataSource = dt
            ' Siguraduhin na tago pa rin ang ID kahit nag-search
            If dgvConfirmed.Columns.Count > 0 Then dgvConfirmed.Columns("ID").Visible = False

        Catch ex As Exception
        Finally
            conn.Close()
        End Try
    End Sub

    Private Sub btnConfirm_Click(sender As Object, e As EventArgs) Handles btnConfirm.Click
        Dim f1 As New Dashboardfrm
        f1.Show()
        Me.Hide()
    End Sub


    Private Sub dgvConfirmed_CellContentDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvConfirmed.CellContentDoubleClick
        If e.RowIndex >= 0 Then
            btnCheckin.Enabled = True
            dgvConfirmed.Rows(e.RowIndex).Selected = True
        End If
    End Sub

    Private Sub btnCheckin_Click(sender As Object, e As EventArgs) Handles btnCheckin.Click
        If dgvConfirmed.SelectedRows.Count > 0 Then
            Dim bookingID As String = dgvConfirmed.CurrentRow.Cells("ID").Value.ToString()
            Dim guestName As String = dgvConfirmed.CurrentRow.Cells("Guest Name").Value.ToString()

            If MessageBox.Show("Check-in " & guestName & " now?", "Confirm Check-in", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                Try
                    If conn.State = ConnectionState.Closed Then conn.Open()
                    ' I-update ang status sa 'Staying'
                    Dim sql As String = "UPDATE bookings SET status = 'Staying' WHERE booking_id = @id"
                    Dim cmd As New MySqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@id", bookingID)

                    If cmd.ExecuteNonQuery() > 0 Then
                        MessageBox.Show(guestName & " is now Checked-in!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        LoadConfirmedBookings() ' Refresh list
                        btnCheckin.Enabled = False ' Disable ulit
                    End If
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                Finally
                    conn.Close()
                End Try
            End If
        End If
    End Sub

    Private Sub btnCurrent_Click(sender As Object, e As EventArgs) Handles btnCurrent.Click
        Dim f1 As New CurrentStayfrm
        f1.Show()
        Me.Hide()
    End Sub

    Private Sub dgvConfirmed_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvConfirmed.CellContentClick

    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        ' 1. Reload the data from the database
        LoadConfirmedBookings()

        ' 2. Clear the search box to show all confirmed guests
        TxtSearch.Clear()

        ' 3. Reset the Check-in button to disabled
        btnCheckin.Enabled = False

        ' Optional: Show a small toast/message in the status bar if you have one
        ' MessageBox.Show("List updated.", "Refresh", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub


End Class