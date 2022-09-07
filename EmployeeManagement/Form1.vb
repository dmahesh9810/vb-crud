Imports MySql.Data.MySqlClient
Public Class Form1
    Dim constr As String = "server=localhost;user id=root;database=employee_management"
    Dim conn As MySqlConnection
    Dim cmd As MySqlCommand
    Private Sub LoginBtn_Click(sender As Object, e As EventArgs) Handles LoginBtn.Click
        conn = New MySqlConnection(constr)
        Dim READER As MySqlDataReader
        Dim mail As String = TextBoxEmail.Text
        Dim pass As String = TextBoxPassword.Text
        If mail IsNot "" And pass IsNot "" Then
            Try
                conn.Open()
                Dim sql As String
                sql = "select * from employee_management.user where email='" & mail & "' and password='" & pass & "'"
                cmd = New MySqlCommand(sql, conn)
                READER = cmd.ExecuteReader
                Dim count As Integer
                count = 0
                While READER.Read
                    count = count + 1

                End While

                If count = 1 Then
                    Me.Hide()
                    Form2.Show()

                ElseIf count > 1 Then
                    MessageBox.Show("Username and password are Duplicate")
                Else
                    MessageBox.Show("Username and password are not correct")

                End If


                conn.Close()

            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        End If

    End Sub
End Class
