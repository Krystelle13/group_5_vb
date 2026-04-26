Imports MySql.Data.MySqlClient
Public Class Dashboardfrm
    ' 1. I-declare ang connection (Siguraduhin na tama ang database name)
    Dim conn As New MySqlConnection("server=localhost;user=root;password=;database=db_resort")

    ' 2. Ito ang tatakbo every 3 seconds dahil sa Timer
    Private Sub tmrRefresh_Tick(sender As Object, e As EventArgs) Handles tmrRefresh.Tick
        LoadNewCustomers()
    End Sub

    ' 3. Ang function na humihila ng data
    Public Sub LoadNewCustomers()
        Try
            ' Buksan ang connection
            If conn.State = ConnectionState.Closed Then conn.Open()

            ' SQL Query: Kunin lang ang mga 'Pending' (yung mga bagong booked lang)
            Dim sql As String = "SELECT guest_name, guest_email, check_in_date FROM bookings WHERE status = 'Pending' ORDER BY booking_id DESC"

            Dim adp As New MySqlDataAdapter(sql, conn)
            Dim dt As New DataTable()

            ' Punuin ang DataTable at ilagay sa DataGridView
            adp.Fill(dt)
            dgvBookings.DataSource = dt

        Catch ex As Exception
            ' Hayaan lang muna para hindi mag-error ang admin habang ginagamit
        Finally
            conn.Close()
        End Try
    End Sub
End Class