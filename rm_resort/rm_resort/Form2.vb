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
            ' 1. Siguraduhin na bukas ang koneksyon
            If conn.State = ConnectionState.Closed Then conn.Open()

            ' 2. SQL Query - Siguraduhin na ang column names ay match sa database mo
            ' DAPAT GANITO ANG SQL MO:
            Dim sql As String = "SELECT booking_id, guest_name, contact_no, cottage_type, check_in_date, payment_option, total_price, status FROM bookings WHERE status = 'Pending' ORDER BY booking_id DESC"

            Dim adp As New MySqlDataAdapter(sql, conn)
            Dim dt As New DataTable
            dt.Clear()
            adp.Fill(dt)

            ' 3. I-set ang DataSource sa DataGridView
            dgvBookings.DataSource = dt

            ' 4. FORMATTING PARA HINDI CROWDED
            If dgvBookings.Columns.Count > 0 Then
                ' I-set ang scrollable behavior
                dgvBookings.ScrollBars = ScrollBars.Both
                dgvBookings.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None

                ' Row height para may "hinga" ang bawat record
                dgvBookings.RowTemplate.Height = 30

                ' LOOP: Para maging pantay ang sukat ng lahat ng columns
                For Each col As DataGridViewColumn In dgvBookings.Columns
                    col.Width = 150 ' Gawin nating 150 para sapat ang luwang
                    col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                Next

                ' OPTIONAL: Gawin nating mas malapad ang Customer Name kung gusto mo
                If dgvBookings.Columns.Contains("guest_name") Then
                    dgvBookings.Columns("guest_name").Width = 200
                End If

                ' I-hide o ipakita ang ID
                If dgvBookings.Columns.Contains("booking_id") Then
                    dgvBookings.Columns("booking_id").Visible = True
                End If
            End If

        Catch ex As Exception
            ' I-print ang error sa console para sa debugging
            Console.WriteLine("Error: " & ex.Message)
        Finally
            ' Laging i-close ang koneksyon
            conn.Close()
        End Try
    End Sub




    Dim selectedBookingID As Integer



    Private Sub dgvBookings_CellContentDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvBookings.CellContentDoubleClick
        If e.RowIndex >= 0 Then
            ' Kunin ang ID ng row na na-click
            selectedBookingID = dgvBookings.Rows(e.RowIndex).Cells("booking_id").Value

            ' Enable ang control buttons
            btnConfirmPaid.Enabled = True
            btnCancel.Enabled = True

            ' Ipakita kung sinong customer ang napili
            Dim name = dgvBookings.Rows(e.RowIndex).Cells("guest_name").Value.ToString()
            MessageBox.Show("Now processing reservation for: " & name)
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
            ' 1. Siguraduhin na bukas ang koneksyon
            If conn.State = ConnectionState.Closed Then conn.Open()

            ' 2. SQL Query na may filter para sa Name at Email
            ' Gagamit tayo ng % sa paligid ng search term para kahit part lang ng pangalan ang i-type ay lalabas ito
            Dim sql As String = "SELECT booking_id, guest_name, guest_email, contact_no, cottage_type, check_in_date, status " &
                           "FROM bookings " &
                           "WHERE (guest_name LIKE @search OR guest_email LIKE @search) " &
                           "AND status = 'Pending' " &
                           "ORDER BY booking_id DESC"

            Dim cmd As New MySqlCommand(sql, conn)
            ' Ang @search ay kukuha ng value mula sa txtSearch box
            cmd.Parameters.AddWithValue("@search", "%" & TxtSearch.Text & "%")

            Dim adp As New MySqlDataAdapter(cmd)
            Dim dt As New DataTable
            dt.Clear()
            adp.Fill(dt)

            ' 3. I-update ang DataGridView
            dgvBookings.DataSource = dt

            ' 4. I-apply ang formatting para hindi mag-reset ang itsura ng columns
            If dgvBookings.Columns.Count > 0 Then
                For Each col As DataGridViewColumn In dgvBookings.Columns
                    col.Width = 150
                Next
            End If

        Catch ex As Exception
            Console.WriteLine("Search Error: " & ex.Message)
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

    Private Sub dgvBookings_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvBookings.CellContentClick

    End Sub
End Class