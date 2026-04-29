Imports MySql.Data.MySqlClient

Public Class CurrentStayfrm
    ' Sub para i-load ang mga guests na may status na 'Staying'
    Public Sub LoadCurrentStay()
        Try
            If conn.State = ConnectionState.Closed Then conn.Open()
            ' Query: Kasama ang Email at Cottage Name gamit ang INNER JOIN
            Dim sql As String = "SELECT b.booking_id AS 'ID', b.guest_name AS 'Guest Name', b.guest_email AS 'Email', " &
              "r.room_name AS 'Cottage/Room', b.check_in_date AS 'Check-in Date', b.total_price AS 'Total' " &
              "FROM bookings b INNER JOIN rooms r ON b.room_id = r.room_id " &
              "WHERE b.status = 'Staying' ORDER BY b.booking_id DESC"

            Dim adp As New MySqlDataAdapter(sql, conn)
            Dim dt As New DataTable
            dt.Clear()
            adp.Fill(dt)
            dgvCurrentStay.DataSource = dt

            ' Formatting ng DataGridView
            If dgvCurrentStay.Columns.Count > 0 Then
                dgvCurrentStay.Columns("ID").Visible = False ' Hidden ID
                dgvCurrentStay.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
                dgvCurrentStay.Columns("Guest Name").Width = 180
                dgvCurrentStay.Columns("Email").Width = 180
                dgvCurrentStay.Columns("Cottage/Room").Width = 150
                dgvCurrentStay.Columns("Total").DefaultCellStyle.Format = "N2"
            End If
            dgvCurrentStay.ScrollBars = ScrollBars.Both
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            conn.Close()
        End Try
    End Sub

    ' Event kapag nag-load ang Form
    Private Sub CurrentStayfrm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadCurrentStay() ' Automatic na magpapakita ang data
        btnCheckout.Enabled = False ' Disabled ang checkout sa simula
    End Sub

    ' Double click para ma-enable ang checkout button
    Private Sub dgvCurrentStay_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvCurrentStay.CellDoubleClick
        If e.RowIndex >= 0 Then
            btnCheckout.Enabled = True
            dgvCurrentStay.Rows(e.RowIndex).Selected = True
        End If
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click
        ' Existing click event
    End Sub

    ' Search functionality
    Private Sub txtSearchCurrent_TextChanged(sender As Object, e As EventArgs) Handles TxtSearchCurrent.TextChanged
        Try
            If conn.State = ConnectionState.Closed Then conn.Open()
            Dim sql As String = "SELECT b.booking_id AS 'ID', b.guest_name AS 'Guest Name', r.room_name AS 'Cottage/Room', b.status " &
                    "FROM bookings b INNER JOIN rooms r ON b.room_id = r.room_id " &
                    "WHERE b.status = 'Staying' AND b.guest_name LIKE @s"
            Dim cmd As New MySqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@s", "%" & TxtSearchCurrent.Text & "%")

            Dim adp As New MySqlDataAdapter(cmd)
            Dim dt As New DataTable
            dt.Clear()
            adp.Fill(dt)
            dgvCurrentStay.DataSource = dt
            If dgvCurrentStay.Columns.Count > 0 Then dgvCurrentStay.Columns("ID").Visible = False
        Catch ex As Exception
            ' Silent error para sa search
        Finally
            conn.Close()
        End Try
    End Sub

    ' Logic para sa Check-out
    Private Sub btnCheckout_Click(sender As Object, e As EventArgs) Handles btnCheckout.Click
        If dgvCurrentStay.SelectedRows.Count > 0 Then
            Dim bookingID As String = dgvCurrentStay.CurrentRow.Cells("ID").Value.ToString()
            Dim guestName As String = dgvCurrentStay.CurrentRow.Cells("Guest Name").Value.ToString()

            ' Message Box bago mag-confirm
            Dim result As DialogResult = MessageBox.Show("Are you sure you want to check out " & guestName & "?", "Confirm Check-out", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

            If result = DialogResult.Yes Then
                Try
                    If conn.State = ConnectionState.Closed Then conn.Open()
                    ' I-update ang status sa Checked Out
                    Dim sql As String = "UPDATE bookings SET status = 'Checked Out' WHERE booking_id = @id"
                    Dim cmd As New MySqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@id", bookingID)

                    If cmd.ExecuteNonQuery() > 0 Then
                        MessageBox.Show(guestName & " has been successfully checked out.", "Resort Management", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        LoadCurrentStay() ' Refresh para mawala na siya sa listahan
                        btnCheckout.Enabled = False ' Disable ulit pagkatapos
                    End If
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                Finally
                    conn.Close()
                End Try
            End If
        Else
            MessageBox.Show("Please select a guest to check out.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    ' Navigation Buttons
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim f1 As New FrmConfirm
        f1.Show()
        Me.Hide()
    End Sub

    Private Sub btnConfirm_Click(sender As Object, e As EventArgs) Handles btnConfirm.Click
        Dim f1 As New Dashboardfrm
        f1.Show()
        Me.Hide()
    End Sub

    ' Logout Logic
    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        Dim response = MsgBox("Are you sure you want to log out?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Logout")

        If response = MsgBoxResult.Yes Then
            Dim login As New Loginform()
            login.Show()
            Me.Dispose()
        End If
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        ' 1. Reload the latest data from the database
        LoadCurrentStay()

        ' 2. Clear the search text box so the full list is shown
        TxtSearchCurrent.Clear()

        ' 3. Disable the checkout button until a guest is selected again
        btnCheckout.Enabled = False

        ' Optional: Small feedback to show it worked
        ' MessageBox.Show("List updated successfully.", "Refresh", MessageBoxButtons.OK, MessageBoxIcon.Information)
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
End Class

