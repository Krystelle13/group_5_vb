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

            ' Query para hilaan ang customers na 'Pending' pa lang
            Dim query As String = "SELECT booking_id, guest_name, check_in_date, status FROM bookings WHERE status = 'Pending'"
            Dim adp As New MySqlDataAdapter(query, conn)
            Dim dt As New DataTable

            adp.Fill(dt)

            ' Siguraduhin na dgvBookings ang pangalan ng DataGridView mo
            dgvBookings.DataSource = dt

        Catch ex As Exception
            ' Tanggalin ang comment sa baba para makita ang exact error message
            ' MessageBox.Show(ex.Message)
        Finally
            conn.Close()
        End Try
    End Sub




    Dim selectedBookingID As Integer



    Private Sub dgvBookings_CellContentDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvBookings.CellContentDoubleClick
        If e.RowIndex >= 0 Then
            ' Kunin ang booking_id sa unang column (index 0)
            selectedBookingID = dgvBookings.Rows(e.RowIndex).Cells(0).Value

            ' Enable ang button
            btnConfirmPaid.Enabled = True

            MessageBox.Show("Customer Selected: " & dgvBookings.Rows(e.RowIndex).Cells(1).Value.ToString())
        End If
    End Sub

    Private Sub btnConfirmPaid_Click(sender As Object, e As EventArgs) Handles btnConfirmPaid.Click
        Try
            If conn.State = ConnectionState.Closed Then conn.Open()

            ' Query para i-update ang status
            Dim cmd As New MySqlCommand("UPDATE bookings SET status = 'Paid' WHERE booking_id = @id", conn)
            cmd.Parameters.AddWithValue("@id", selectedBookingID)

            If cmd.ExecuteNonQuery() > 0 Then
                MessageBox.Show("Payment Confirmed! Customer moved to Confirmed List.")

                ' I-refresh ang grid para mawala na ang customer
                LoadBookings()

                ' I-disable ulit ang button
                btnConfirmPaid.Enabled = False
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            conn.Close()
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim f1 As New FrmConfirm
        f1.Show()
        Me.Hide()
    End Sub

    Private Sub TxtSearch_TextChanged(sender As Object, e As EventArgs) Handles TxtSearch.TextChanged
        Try
            Dim str As String
            str = "select * from student where fullname like '%" & TxtSearch.Text & "%'"
            conn.Open()
            Dim mysda As New MySqlDataAdapter(str, conn)
            Dim ds As New DataSet
            mysda.Fill(ds, "student")
            dgvBookings.DataSource = ds.Tables("student")
            conn.Close()

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation)
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
End Class