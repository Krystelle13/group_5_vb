Public Class Form3
    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles txtReservationDetail.TextChanged

    End Sub

    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Dim result As DialogResult = MessageBox.Show("Are you sure you want to Exit?", "Confirm", MessageBoxButtons.OKCancel)
        If result = DialogResult.OK Then
            Application.Exit()
        End If
    End Sub
End Class