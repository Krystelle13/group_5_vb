Imports MySql.Data.MySqlClient

Public Class CurrentStayfrm
    ' Connection string
    Dim conn As New MySqlConnection("server=localhost;user=root;password=;database=db_resort")

    ' =========================================================================
    ' 🛠️ HELPER PARA SA GRID STYLING (Full Width & No Extra Rows)
    ' =========================================================================
    Private Sub ApplyGridFormat()
        ' 1. Tanggalin ang extra empty row sa ibaba at arrow sa gilid
        dgvCurrentStay.AllowUserToAddRows = False
        dgvCurrentStay.AllowUserToDeleteRows = False
        dgvCurrentStay.RowHeadersVisible = False

        If dgvCurrentStay.Columns.Count > 0 Then
            ' 2. Itago ang ID column
            If dgvCurrentStay.Columns.Contains("ID") Then dgvCurrentStay.Columns("ID").Visible = False

            ' 3. E-OCCUPY ANG BUONG SPACE (Fill Mode)
            ' Gagamitin natin ang 'Fill' para mag-stretch ang columns hanggang dulo
            dgvCurrentStay.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            ' 4. (Optional) Kung gusto mo ng specific width sa iba pero 'Fill' sa huli:
            ' dgvCurrentStay.Columns("Guest Name").FillWeight = 150
            ' dgvCurrentStay.Columns("Email").FillWeight = 150
            ' dgvCurrentStay.Columns("Cottage/Room").FillWeight = 120

            ' 5. Format para sa currency at alignment
            If dgvCurrentStay.Columns.Contains("Total") Then
                dgvCurrentStay.Columns("Total").DefaultCellStyle.Format = "N2"
                dgvCurrentStay.Columns("Total").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End If

            ' General Styles
            dgvCurrentStay.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            dgvCurrentStay.MultiSelect = False
            dgvCurrentStay.ScrollBars = ScrollBars.Both

            ' TINANGGAL ANG BACKGROUNDCOLOR = WHITE PARA HINDI MAG-WHITE ANG DGV
            ' dgvCurrentStay.BackgroundColor = Color.White 
        End If
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

            If MessageBox.Show("Are you sure you want to check out " & guestName & "?", "Confirm", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                Try
                    If conn.State = ConnectionState.Closed Then conn.Open()
                    Dim sql As String = "UPDATE bookings SET status = 'Checked Out' WHERE booking_id = @id"
                    Dim cmd As New MySqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@id", bookingID)

                    If cmd.ExecuteNonQuery() > 0 Then
                        MessageBox.Show("Checked out successfully.")
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
        If MsgBox("Logout?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            Loginform.Show() : Me.Dispose()
        End If
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadCurrentStay()
        TxtSearchCurrent.Clear()
        btnCheckout.Enabled = False
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Application.Exit()
    End Sub

    Private Sub btnSettings_Click(sender As Object, e As EventArgs) Handles btnSettings.Click
        FrmTotalIncome.Show() : Me.Hide()
    End Sub
End Class