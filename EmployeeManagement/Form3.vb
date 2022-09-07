Imports MySql.Data.MySqlClient
Imports System.Data
Imports System.IO
Public Class Form3
    Dim constr As String = "server=localhost;user id=root;database=employee_management"
    Dim conn As MySqlConnection
    Dim cmd As MySqlCommand
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Form2.Show()
        Me.Hide()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim myAdapter As New MySqlDataAdapter
        Dim myData As New DataTable
        Dim sql As String
        Dim cmd As New MySqlCommand
        conn = New MySqlConnection(constr)
        Try
            conn.Open()
            sql = "Select `id`, `fName`, `lName`, `gender`, `email`, `mobile`, `address`, `position` From employee where email='" + TextBox1.Text + "'"

            cmd.Connection = conn
            cmd.CommandText = sql

            myAdapter.SelectCommand = cmd
            myAdapter.Fill(myData)

            DataGridView1.DataSource = myData

            conn.Close()
        Catch myerror As MySqlException
            MessageBox.Show("Error: " & myerror.Message)
        Finally
            conn.Dispose()
        End Try
    End Sub

    Private Sub Form3_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        Application.Exit()
    End Sub
End Class