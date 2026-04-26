Imports MySql.Data.MySqlClient
Public Class Loginform
    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Dim result As DialogResult = MessageBox.Show("Are you sure you want to Enter?", "Confirm", MessageBoxButtons.OKCancel)
        If result = DialogResult.OK Then
            Application.Exit()
        End If
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        If txtUserName.Text = "aura" And txtPassword.Text = "auraG5" Then
            MsgBox("Access Granted!", MsgBoxStyle.Information, "Success")

            Dim mainMENU As New Dashboardfrm
            mainMENU.Show()

            Me.Hide()
        Else
            MsgBox("Invalid login details!", MsgBoxStyle.Critical, "Error")
            txtUserName.Focus()
            txtPassword.Clear()
        End If
    End Sub


End Class

