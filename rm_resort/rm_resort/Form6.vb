Imports MySql.Data.MySqlClient

Public Class CurrentStayfrm
    ' Connection string
    Dim conn As New MySqlConnection("server=localhost;user=root;password=;database=db_resort")

    ' =========================================================================
    ' 🛠️ HELPER PARA SA GRID STYLING (Full Width & Protected)
    ' =========================================================================
    Private Sub ApplyGridFormat()
        With dgvCurrentStay
            ' 1. Protection Settings (Hindi na ma-e-edit o mabubura ang data/columns)
            .ReadOnly = True
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToOrderColumns = False
            .RowHeadersVisible = False

            If .Columns.Count > 0 Then
                ' 2. Itago ang ID column
                If .Columns.Contains("ID") Then .Columns("ID").Visible = False

                ' 3. E-OCCUPY ANG BUONG SPACE (Fill Mode)
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

                ' 4. Format para sa currency at alignment
                If .Columns.Contains("Total") Then
                    .Columns("Total").DefaultCellStyle.Format = "N2"
                    .Columns("Total").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End If

                ' General Styles
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect
                .MultiSelect = False
                .ScrollBars = ScrollBars.Both
            End If
        End With
    End Sub

    ' Sub para i-load ang data
    Public Sub LoadCurrentStay()
        Try
            If conn.State = ConnectionState.Closed Then conn.Open()
            Dim sql As String = "SELECT b.booking_id AS 'ID', b.guest_name AS 'Guest Name', b.guest_email AS 'Email', " &
                               "r.room_name AS 'Cottage/Room', b.check_in_date AS 'Check-in Date', b.total_price AS 'Total' " &
                               "FROM bookings b INNER JOIN rooms r ON b.room_id = r.room_id " &
                               "WHERE b.status = 'Staying' ORDER BY b.booking_id DESC"

            Dim adp As New MySqlDataAdapter(sql, conn)
            Dim dt As New DataTable
            dt.Clear()
            adp.Fill(dt)
            dgvCurrentStay.DataSource = dt

            ApplyGridFormat()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            conn.Close()
        End Try
    End Sub

    ' Event kapag nag-load ang Form
    Private Sub CurrentStayfrm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadCurrentStay()
        btnCheckout.Enabled = False
    End Sub

    ' Double click para ma-enable ang checkout button
    Private Sub dgvCurrentStay_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvCurrentStay.CellDoubleClick
        If e.RowIndex >= 0 Then
            btnCheckout.Enabled = True
            dgvCurrentStay.Rows(e.RowIndex).Selected = True
        End If
    End Sub

    ' Search functionality (Occupies full width even when searching)
    Private Sub txtSearchCurrent_TextChanged(sender As Object, e As EventArgs) Handles TxtSearchCurrent.TextChanged
        Try
            If conn.State = ConnectionState.Closed Then conn.Open()
            Dim sql As String = "SELECT b.booking_id AS 'ID', b.guest_name AS 'Guest Name', b.guest_email AS 'Email', " &
                                "r.room_name AS 'Cottage/Room', b.check_in_date AS 'Check-in Date', b.total_price AS 'Total' " &
                                "FROM bookings b INNER JOIN rooms r ON b.room_id = r.room_id " &
                                "WHERE b.status = 'Staying' AND b.guest_name LIKE @s"

            Dim cmd As New MySqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@s", "%" & TxtSearchCurrent.Text & "%")

            Dim adp As New MySqlDataAdapter(cmd)
            Dim dt As New DataTable
            dt.Clear()
            adp.Fill(dt)
            dgvCurrentStay.DataSource = dt

            ' Importante: Tawagin ulit ang formatting para hindi bumalik sa default ang columns habang nagse-search
            ApplyGridFormat()

        Catch ex As Exception
        Finally
            conn.Close()
        End Try
    End Sub

    ' Logic para sa Check-out
    Private Sub btnCheckout_Click(sender As Object, e As EventArgs) Handles btnCheckout.Click
        If dgvCurrentStay.SelectedRows.Count > 0 Then
            Dim bookingID As String = dgvCurrentStay.CurrentRow.Cells("ID").Value.ToString()
            Dim guestName As String = dgvCurrentStay.CurrentRow.Cells("Guest Name").Value.ToString()

            If MessageBox.Show("Are you sure you want to check out " & guestName & "?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                Try
                    If conn.State = ConnectionState.Closed Then conn.Open()
                    Dim sql As String = "UPDATE bookings SET status = 'Checked Out' WHERE booking_id = @id"
                    Dim cmd As New MySqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@id", bookingID)

                    If cmd.ExecuteNonQuery() > 0 Then
                        MessageBox.Show(guestName & " has checked out successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        LoadCurrentStay()
                        btnCheckout.Enabled = False
                    End If
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                Finally
                    conn.Close()
                End Try
            End If
        End If
    End Sub

    ' Navigation Buttons
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim f1 As New FrmConfirm : f1.Show() : Me.Hide()
    End Sub

    Private Sub btnConfirm_Click(sender As Object, e As EventArgs) Handles btnConfirm.Click
        Dim f1 As New Dashboardfrm : f1.Show() : Me.Hide()
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        If MsgBox("Are you sure you want to logout?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Logout") = MsgBoxResult.Yes Then
            Dim login As New Loginform()
            login.Show()
            Me.Dispose()
        End If
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadCurrentStay()
        TxtSearchCurrent.Clear()
        btnCheckout.Enabled = False
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim result As DialogResult = MessageBox.Show("Are you sure you want to Exit?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)
        If result = DialogResult.OK Then
            Application.Exit()
        End If
    End Sub

    Private Sub btnSettings_Click(sender As Object, e As EventArgs) Handles btnSettings.Click
        Dim f1 As New FrmTotalIncome
        f1.Show() : Me.Hide()
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click
    End Sub
End Class