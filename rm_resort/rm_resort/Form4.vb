Imports MySql.Data.MySqlClient

Public Class FrmConfirm
    ' 1. Connection string
    Dim conn As New MySqlConnection("server=localhost;user=root;password=;database=db_resort")

    ' 2. Pagka-load ng Form
    Private Sub FrmConfirm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' ENHANCEMENTS: Set properties para hindi ma-edit at maging professional tingnan
        With dgvConfirmed
            .ReadOnly = True ' HINDI MA-E-EDIT ANG CELLS
            .AllowUserToAddRows = False ' HINDI MAKAKAPAG-ADD NG MANUAL ROW
            .AllowUserToDeleteRows = False ' HINDI MAKAKAPAG-DELETE
            .AllowUserToOrderColumns = False ' HINDI MA-E-ERASE O MA-MU-MOVE ANG COLUMN ORDER
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .RowHeadersVisible = False ' Para mas malinis tingnan (optional)
            .BackgroundColor = Color.White
        End With

        LoadConfirmedBookings()
        btnCheckin.Enabled = False ' Disabled sa simula hangga't walang napipiling guest
    End Sub

    ' 3. Function para i-load ang mga 'Confirmed' bookings
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

            ' ENHANCEMENTS: Column Formatting
            If dgvConfirmed.Columns.Count > 0 Then
                dgvConfirmed.Columns("ID").Visible = False

                ' Pinapanatili ang headers at nilalagyan ng fixed width para hindi "ma-erase" sa paningin
                dgvConfirmed.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
                dgvConfirmed.Columns("Guest Name").Width = 180
                dgvConfirmed.Columns("Email Address").Width = 200
                dgvConfirmed.Columns("Cottage/Room").Width = 180
                dgvConfirmed.Columns("Date").Width = 120
                dgvConfirmed.Columns("Payment").Width = 120
                dgvConfirmed.Columns("Total").Width = 100
                dgvConfirmed.Columns("Status").Width = 100

                ' Format for currency
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

    ' SINGLE CLICK
    Private Sub dgvConfirmed_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvConfirmed.CellClick
        If e.RowIndex >= 0 Then
            btnCheckin.Enabled = True
        End If
    End Sub

    ' DOUBLE CLICK
    Private Sub dgvConfirmed_CellMouseDoubleClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgvConfirmed.CellMouseDoubleClick
        If e.RowIndex >= 0 Then
            dgvConfirmed.Rows(e.RowIndex).Selected = True
            btnCheckin.Enabled = True
            PerformCheckInAction()
        End If
    End Sub

    ' BUTTON CLICK TO POP UP CHECK-IN
    Private Sub btnCheckin_Click(sender As Object, e As EventArgs) Handles btnCheckin.Click
        PerformCheckInAction()
    End Sub

    ' ACTUAL CHECK-IN LOGIC
    Private Sub PerformCheckInAction()
        If dgvConfirmed.SelectedRows.Count > 0 Then
            Dim bookingID As String = dgvConfirmed.CurrentRow.Cells("ID").Value.ToString()
            Dim guestName As String = dgvConfirmed.CurrentRow.Cells("Guest Name").Value.ToString()

            If MessageBox.Show("Check-in " & guestName & " now?", "Confirm Check-in", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                Try
                    If conn.State = ConnectionState.Closed Then conn.Open()
                    Dim sql As String = "UPDATE bookings SET status = 'Staying' WHERE booking_id = @id"
                    Dim cmd As New MySqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@id", bookingID)

                    If cmd.ExecuteNonQuery() > 0 Then
                        MessageBox.Show(guestName & " is now Checked-in!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        LoadConfirmedBookings()
                        btnCheckin.Enabled = False
                        dgvConfirmed.ClearSelection()
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

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadConfirmedBookings()
        TxtSearch.Clear()
        btnCheckin.Enabled = False
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
        Dim result As DialogResult = MessageBox.Show("Are you sure you want to Exit?", "Confirm", MessageBoxButtons.OKCancel)
        If result = DialogResult.OK Then
            Application.Exit()
        End If
    End Sub

    Private Sub btnSettings_Click(sender As Object, e As EventArgs) Handles btnSettings.Click
        Dim f1 As New FrmTotalIncome
        f1.Show()
        Me.Hide()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click
    End Sub
End Class