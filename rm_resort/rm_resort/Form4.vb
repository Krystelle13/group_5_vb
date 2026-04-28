Imports MySql.Data.MySqlClient

Public Class FrmConfirm
    ' 1. Connection string
    Dim conn As New MySqlConnection("server=localhost;user=root;password=;database=db_resort")

    ' 2. Pagka-load ng Form
    Private Sub FrmConfirm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set selection mode to FullRowSelect so double-clicking anywhere on the row works
        dgvConfirmed.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvConfirmed.MultiSelect = False

        LoadConfirmedBookings()
        btnCheckin.Enabled = False
    End Sub

    ' 3. Function para hulaan ang mga 'Paid' customers
    Public Sub LoadConfirmedBookings()
        Try
            If conn.State = ConnectionState.Closed Then conn.Open()
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

            If dgvConfirmed.Columns.Count > 0 Then
                dgvConfirmed.Columns("ID").Visible = False
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

    ' SEARCH FUNCTION
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
            If dgvConfirmed.Columns.Count > 0 Then dgvConfirmed.Columns("ID").Visible = False
        Catch ex As Exception
        Finally
            conn.Close()
        End Try
    End Sub

    ' DOUBLE CLICK TO POP UP CHECK-IN
    Private Sub dgvConfirmed_CellMouseDoubleClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgvConfirmed.CellMouseDoubleClick
        If e.RowIndex >= 0 Then
            ' Force selection of the row that was double clicked
            dgvConfirmed.Rows(e.RowIndex).Selected = True
            btnCheckin.Enabled = True

            ' Run the logic
            PerformCheckInAction()
        End If
    End Sub

    ' BUTTON CLICK TO POP UP CHECK-IN
    Private Sub btnCheckin_Click(sender As Object, e As EventArgs) Handles btnCheckin.Click
        PerformCheckInAction()
    End Sub

    ' ACTUAL CHECK-IN LOGIC
    Private Sub PerformCheckInAction()
        ' Check if there's a selected row
        If dgvConfirmed.SelectedRows.Count > 0 Then
            Dim bookingID As String = dgvConfirmed.CurrentRow.Cells("ID").Value.ToString()
            Dim guestName As String = dgvConfirmed.CurrentRow.Cells("Guest Name").Value.ToString()

            ' THE POP UP MESSAGE
            If MessageBox.Show("Check-in " & guestName & " now?", "Confirm Check-in", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                Try
                    If conn.State = ConnectionState.Closed Then conn.Open()
                    Dim sql As String = "UPDATE bookings SET status = 'Staying' WHERE booking_id = @id"
                    Dim cmd As New MySqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@id", bookingID)

                    If cmd.ExecuteNonQuery() > 0 Then
                        MessageBox.Show(guestName & " is now Checked-in!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        LoadConfirmedBookings() ' Refresh
                        btnCheckin.Enabled = False
                    End If
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                Finally
                    conn.Close()
                End Try
            End If
        Else
            MessageBox.Show("Please select a guest first.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    ' NAVIGATION
    Private Sub btnConfirm_Click(sender As Object, e As EventArgs) Handles btnConfirm.Click
        Dim f1 As New Dashboardfrm
        f1.Show()
        Me.Hide()
    End Sub

    Private Sub btnCurrent_Click(sender As Object, e As EventArgs) Handles btnCurrent.Click
        Dim f1 As New CurrentStayfrm
        f1.Show()
        Me.Hide()
    End Sub

<<<<<<< HEAD
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
=======
    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        Dim response = MsgBox("Are you sure you want to log out?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Logout")
        If response = MsgBoxResult.Yes Then
            Dim login As New Loginform()
            login.Show()
            Me.Dispose()
        End If
    End Sub
End Class
>>>>>>> 630f838884799a16ab2bde1c60d156e452035f61
