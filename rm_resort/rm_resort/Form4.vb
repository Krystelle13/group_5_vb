Imports MySql.Data.MySqlClient

Public Class FrmConfirm
    ' 1. Connection string (Dapat pareho sa ginamit mo sa Dashboard)
    Dim conn As New MySqlConnection("server=localhost;user=root;password=;database=db_resort")

    ' 2. Pagka-load ng Form, kusa niyang tatawagin ang listahan
    Private Sub FrmConfirm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadConfirmedBookings()
    End Sub

    ' 3. Function para hulaan ang mga 'Paid' customers
    Public Sub LoadConfirmedBookings()
        Try
            If conn.State = ConnectionState.Closed Then conn.Open()

            ' SQL Query: Kinukuha lang ang status na 'Paid'
            Dim query As String = "SELECT booking_id, guest_name, check_in_date, status FROM bookings WHERE status = 'Paid' ORDER BY booking_id DESC"

            Dim adp As New MySqlDataAdapter(query, conn)
            Dim dt As New DataTable
            dt.Clear()
            adp.Fill(dt)

            ' I-bind ang data sa iyong DataGridView
            dgvConfirmed.DataSource = dt

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            conn.Close()
        End Try
    End Sub

    Private Sub btnConfirm_Click(sender As Object, e As EventArgs) Handles btnConfirm.Click
        Dim f1 As New Dashboardfrm
        f1.Show()
        Me.Hide()
    End Sub

    Private Sub TxtSearch_TextChanged(sender As Object, e As EventArgs) Handles TxtSearch.TextChanged
        Try
            ' 1. Siguraduhin na bukas ang koneksyon sa database
            If conn.State = ConnectionState.Closed Then conn.Open()

            ' 2. SQL Query para sa Paid customers
            ' Naghahanap tayo sa guest_name at guest_email kung saan ang status ay 'Paid'
            Dim sql As String = "SELECT booking_id, guest_name, guest_email, contact_no, cottage_type, check_in_date, payment_option, total_price " &
                           "FROM bookings " &
                           "WHERE (guest_name LIKE @search OR guest_email LIKE @search) " &
                           "AND status = 'Paid' " &
                           "ORDER BY booking_id DESC"

            Dim cmd As New MySqlCommand(sql, conn)
            ' Gagamit tayo ng wildcard (%) para makuha kahit part lang ng pangalan
            cmd.Parameters.AddWithValue("@search", "%" & TxtSearch.Text & "%")

            Dim adp As New MySqlDataAdapter(cmd)
            Dim dt As New DataTable
            dt.Clear()
            adp.Fill(dt)

            ' 3. I-update ang DataGridView ng Confirmed Form
            dgvConfirmed.DataSource = dt

            ' 4. I-apply ang formatting para hindi siksikan (Uniform Width)
            If dgvConfirmed.Columns.Count > 0 Then
                dgvConfirmed.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
                For Each col As DataGridViewColumn In dgvConfirmed.Columns
                    col.Width = 150
                Next
            End If

        Catch ex As Exception
            Console.WriteLine("Search Error: " & ex.Message)
        Finally
            conn.Close()
        End Try
    End Sub

    ' 4. (Optional) Refresh Button kung gusto mong i-update ang listahan manual

End Class