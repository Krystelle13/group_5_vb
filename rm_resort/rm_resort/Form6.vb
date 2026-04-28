Imports MySql.Data.MySqlClient

Public Class CurrentStayfrm

    ' =========================================================================
    ' 🎨 HARMONIZED MODERN PALETTE (Deep Slate & Vibrant Teal)
    ' =========================================================================
    Private ReadOnly SidebarBgColor As Color = Color.FromArgb(31, 41, 55)   ' Left Panel Background
    Private ReadOnly MainCanvasBg As Color = Color.FromArgb(243, 244, 246) ' Form Canvas Background
    Private ReadOnly AccentTeal As Color = Color.FromArgb(20, 184, 166)    ' High-contrast Teal Accent
    Private ReadOnly DangerRed As Color = Color.FromArgb(239, 68, 68)      ' Harmonized Red for Logout
    Private ReadOnly DarkText As Color = Color.FromArgb(17, 24, 39)        ' Rich Charcoal Text
    Private ReadOnly HoverGray As Color = Color.FromArgb(55, 65, 81)       ' Dark Slate Hover

    ' =========================================================================
    ' 🚀 1. INITIALIZATION & LIFECYCLE
    ' =========================================================================
    Private Sub CurrentStayfrm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Apply modern visuals first to prevent UI flickering
        ApplyAestheticTheme()
        LoadCurrentStay()
        btnCheckout.Enabled = False
    End Sub

    ' =========================================================================
    ' 🎨 2. AESTHETIC STYLING ENGINE (The requested changes)
    ' =========================================================================
    Private Sub ApplyAestheticTheme()
        ' Global canvas color
        Me.BackColor = MainCanvasBg

        ' Styling the specific left side panel navigation buttons
        ' This sequence strictly forces VB to kill borders and apply flat fills
        StyleSidebarButton(Button1)
        StyleSidebarButton(btnConfirm)
        StyleSidebarButton(btnLogout, DangerRed)

        ' Styling the operational workspace buttons
        StyleCanvasButton(btnCheckout, AccentTeal)
        StyleCanvasButton(btnRefresh, Color.FromArgb(59, 130, 246))

        ' Modernize the search box border
        TxtSearchCurrent.BorderStyle = BorderStyle.FixedSingle
        TxtSearchCurrent.Font = New Font("Segoe UI", 11)

        ' Harmonizing the DataGridView grid UI
        With dgvCurrentStay
            .BorderStyle = BorderStyle.None
            .BackgroundColor = Color.White
            .EnableHeadersVisualStyles = False
            .RowHeadersVisible = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect

            ' Grid Header (Matches the sidebar for color harmony)
            .ColumnHeadersDefaultCellStyle.BackColor = SidebarBgColor
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
            .ColumnHeadersHeight = 40

            ' Grid Rows
            .DefaultCellStyle.Font = New Font("Segoe UI", 9)
            .DefaultCellStyle.ForeColor = DarkText
            .DefaultCellStyle.SelectionBackColor = Color.FromArgb(204, 251, 241) ' Translucent Teal
            .DefaultCellStyle.SelectionForeColor = DarkText
            .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 251)
        End With
    End Sub

    ' Hard-removes borders and aligns Left Sidebar Buttons
    Private Sub StyleSidebarButton(btn As Button, Optional customBg As Color = Nothing)
        btn.FlatStyle = FlatStyle.Flat
        btn.FlatAppearance.BorderSize = 0
        btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(17, 24, 39)
        btn.FlatAppearance.MouseOverBackColor = HoverGray
        btn.BackColor = If(customBg = Nothing, SidebarBgColor, customBg)
        btn.ForeColor = Color.White
        btn.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        btn.TextAlign = ContentAlignment.MiddleLeft ' True Sidebar Left-alignment
        btn.Padding = New Padding(15, 0, 0, 0) ' Pushes text away from the left edge
        btn.Cursor = Cursors.Hand
    End Sub

    ' Hard-removes borders for normal operational canvas buttons
    Private Sub StyleCanvasButton(btn As Button, bgCol As Color)
        btn.FlatStyle = FlatStyle.Flat
        btn.FlatAppearance.BorderSize = 0
        btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(30, 41, 59)
        btn.BackColor = bgCol
        btn.ForeColor = Color.White
        btn.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        btn.Cursor = Cursors.Hand
    End Sub

    ' =========================================================================
    ' 📊 3. DATABASE QUERIES & DATA MANIPULATION
    ' =========================================================================
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

            ' Grid sizing mechanics
            If dgvCurrentStay.Columns.Count > 0 Then
                dgvCurrentStay.Columns("ID").Visible = False
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
            ' Silent catch specifically for uninterrupted typing on search bars
        Finally
            conn.Close()
        End Try
    End Sub

    ' =========================================================================
    ' 🖱️ 4. INTERACTION & ACTION EVENTS
    ' =========================================================================
    Private Sub dgvCurrentStay_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvCurrentStay.CellDoubleClick
        If e.RowIndex >= 0 Then
            btnCheckout.Enabled = True
            dgvCurrentStay.Rows(e.RowIndex).Selected = True
        End If
    End Sub

    Private Sub btnCheckout_Click(sender As Object, e As EventArgs) Handles btnCheckout.Click
        If dgvCurrentStay.SelectedRows.Count > 0 Then
            Dim bookingID As String = dgvCurrentStay.CurrentRow.Cells("ID").Value.ToString()
            Dim guestName As String = dgvCurrentStay.CurrentRow.Cells("Guest Name").Value.ToString()

            Dim result As DialogResult = MessageBox.Show("Are you sure you want to check out " & guestName & "?", "Confirm Check-out", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

            If result = DialogResult.Yes Then
                Try
                    If conn.State = ConnectionState.Closed Then conn.Open()
                    Dim sql As String = "UPDATE bookings SET status = 'Checked Out' WHERE booking_id = @id"
                    Dim cmd As New MySqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@id", bookingID)

                    If cmd.ExecuteNonQuery() > 0 Then
                        MessageBox.Show(guestName & " has been successfully checked out.", "Resort Management", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        LoadCurrentStay()
                        btnCheckout.Enabled = False
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

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadCurrentStay()
        TxtSearchCurrent.Clear()
        btnCheckout.Enabled = False
    End Sub

    ' =========================================================================
    ' 🧭 5. NAVIGATION & MISCELLANEOUS
    ' =========================================================================
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

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        Dim response = MsgBox("Are you sure you want to log out?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Logout")
        If response = MsgBoxResult.Yes Then
            Dim login As New Loginform()
            login.Show()
            Me.Dispose()
        End If
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
    End Sub
End Class
