Imports MySql.Data.MySqlClient
Imports System.Data
Imports System.IO
Public Class Form2
    Dim constr As String = "server=localhost;user id=root;database=employee_management"
    Dim conn As MySqlConnection
    Dim cmd As MySqlCommand


    Private Sub ButtonCreate_Click(sender As Object, e As EventArgs) Handles ButtonCreate.Click
        Dim connection As New MySqlConnection(constr)

        Dim ms As New MemoryStream

        If Not PictureBox1.Image Is Nothing And TextBoxFname.Text IsNot "" And TextBoxLname.Text IsNot "" And TextBoxEmail.Text IsNot "" And TextBoxMobile.Text IsNot "" And TextBoxAddress.Text IsNot "" And TextBoxPosition.Text IsNot "" Then

            connection.Open()
            Dim cmd As New MySqlCommand("SELECT COUNT(*) FROM employee WHERE email=@email OR  mobile=@mobi", connection)
            cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = TextBoxEmail.Text
            cmd.Parameters.Add("@mobi", MySqlDbType.Int64).Value = TextBoxMobile.Text
            Dim i As Integer = cmd.ExecuteScalar()
            cmd = Nothing
            connection.Close()
            If i > 0 Then
                MessageBox.Show("Your data already exists")
            Else
                PictureBox1.Image.Save(ms, PictureBox1.Image.RawFormat)
                Dim command As New MySqlCommand("INSERT INTO `employee`(`fName`, `lName`, `gender`, `email`, `mobile`, `address`, `position`,`image`) VALUES (@fn,@ln,@gn,@email,@mobi,@adr,@posit,@img)", connection)

                Dim gender As String

                If RadioButtonMale.Checked = True Then
                    gender = "male"
                ElseIf RadioButtonFemale.Checked = True Then
                    gender = "female"
                Else
                    gender = "unknown"
                End If

                command.Parameters.Add("@fn", MySqlDbType.VarChar).Value = TextBoxFname.Text
                command.Parameters.Add("@ln", MySqlDbType.VarChar).Value = TextBoxLname.Text
                command.Parameters.Add("@gn", MySqlDbType.VarChar).Value = gender
                command.Parameters.Add("@email", MySqlDbType.VarChar).Value = TextBoxEmail.Text
                command.Parameters.Add("@mobi", MySqlDbType.Int64).Value = TextBoxMobile.Text
                command.Parameters.Add("@adr", MySqlDbType.VarChar).Value = TextBoxAddress.Text
                command.Parameters.Add("@posit", MySqlDbType.VarChar).Value = TextBoxPosition.Text
                command.Parameters.Add("@img", MySqlDbType.Blob).Value = ms.ToArray()
                connection.Open()
                If command.ExecuteNonQuery() = 1 Then
                    MessageBox.Show("Inserted")
                    showData()
                Else
                    MessageBox.Show("Not Inserted")
                End If

                connection.Close()
            End If

        Else
            MessageBox.Show("Please input all field")
        End If

    End Sub

    Private Sub TextBoxMobile_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBoxMobile.KeyPress
        Dim Ch As Char = e.KeyChar
        If Not Char.IsDigit(Ch) AndAlso Asc(Ch) <> 8 Then
            e.Handled = True
        End If
    End Sub

    Public Sub showData()
        Dim myAdapter As New MySqlDataAdapter
        Dim myData As New DataTable
        Dim sql As String
        Dim cmd As New MySqlCommand
        conn = New MySqlConnection(constr)
        Try
            conn.Open()
            sql = "Select `id`, `fName`, `lName`, `gender`, `email`, `mobile`, `address`, `position` From employee"

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
    Private Sub ButtonShow_Click(sender As Object, e As EventArgs) Handles ButtonShow.Click
        showData()
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick

        TextBoxFname.Text = DataGridView1.SelectedRows(0).Cells(1).Value.ToString
        TextBoxLname.Text = DataGridView1.SelectedRows(0).Cells(2).Value.ToString
        TextBoxEmail.Text = DataGridView1.SelectedRows(0).Cells(4).Value.ToString
        TextBoxMobile.Text = DataGridView1.SelectedRows(0).Cells(5).Value.ToString
        TextBoxAddress.Text = DataGridView1.SelectedRows(0).Cells(6).Value.ToString
        TextBoxPosition.Text = DataGridView1.SelectedRows(0).Cells(7).Value.ToString

        If DataGridView1.SelectedRows(0).Cells(3).Value.ToString = "male" Then
            RadioButtonMale.Select()
        ElseIf DataGridView1.SelectedRows(0).Cells(3).Value.ToString = "female" Then
            RadioButtonFemale.Select()
        End If
        imageR()

    End Sub

    Private Sub ButtonUpdate_Click(sender As Object, e As EventArgs) Handles ButtonUpdate.Click
        conn = New MySqlConnection(constr)
        Dim ms As New MemoryStream
        If Not PictureBox1.Image Is Nothing And TextBoxFname.Text IsNot "" And TextBoxLname.Text IsNot "" And TextBoxEmail.Text IsNot "" And TextBoxMobile.Text IsNot "" And TextBoxAddress.Text IsNot "" And TextBoxPosition.Text IsNot "" Then
            PictureBox1.Image.Save(ms, PictureBox1.Image.RawFormat)
            Dim command As New MySqlCommand("UPDATE `employee` SET `fName`=@fn,`lName`=@ln,`gender`=@gn,`email`=@email,`mobile`=@mobi,`address`=@adr,`position`=@posit,`image`=@img WHERE `id` = @id", conn)

            Dim gender As String

            If RadioButtonMale.Checked = True Then
                gender = "male"
            ElseIf RadioButtonFemale.Checked = True Then
                gender = "female"
            Else
                gender = "unknown"
            End If

            command.Parameters.Add("@id", MySqlDbType.Int64).Value = DataGridView1.SelectedRows(0).Cells(0).Value.ToString
            command.Parameters.Add("@fn", MySqlDbType.VarChar).Value = TextBoxFname.Text
            command.Parameters.Add("@ln", MySqlDbType.VarChar).Value = TextBoxLname.Text
            command.Parameters.Add("@gn", MySqlDbType.VarChar).Value = gender
            command.Parameters.Add("@email", MySqlDbType.VarChar).Value = TextBoxEmail.Text
            command.Parameters.Add("@mobi", MySqlDbType.Int64).Value = TextBoxMobile.Text
            command.Parameters.Add("@adr", MySqlDbType.VarChar).Value = TextBoxAddress.Text
            command.Parameters.Add("@posit", MySqlDbType.VarChar).Value = TextBoxPosition.Text
            command.Parameters.Add("@img", MySqlDbType.Blob).Value = ms.ToArray()

            conn.Open()

            If command.ExecuteNonQuery() = 1 Then
                MessageBox.Show("Updated")
                showData()
            Else
                MessageBox.Show("Error")
            End If
        Else
            MessageBox.Show("Please input all field")
        End If



    End Sub

    Private Sub ButtonDelete_Click(sender As Object, e As EventArgs) Handles ButtonDelete.Click
        conn = New MySqlConnection(constr)
        Dim str As String
        Dim id As Integer
        Try
            If (MsgBox(“Are you sure you want to delete the item?”, MsgBoxStyle.YesNo) = MsgBoxResult.Yes) Then
                id = DataGridView1.SelectedRows(0).Cells(0).Value.ToString
                str = “DELETE FROM employee WHERE id=@id”
                conn.Open()
                Dim command As New MySqlCommand(str, conn)
                command.Parameters.Add("@id", MySqlDbType.Int64).Value = DataGridView1.SelectedRows(0).Cells(0).Value.ToString
                command.ExecuteNonQuery()
                MsgBox(“Data Deleted!”, MsgBoxStyle.Information)
                conn.Close()
                showData()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        conn.Close()
        End Try

    End Sub

    Private Sub ButtonImage_Click(sender As Object, e As EventArgs) Handles ButtonImage.Click
        Dim opf As New OpenFileDialog
        opf.Filter = "Choose Image(*.JPG;*.PNG;*.GIF)|*.jpg;*.png;*.gif"
        If opf.ShowDialog = Windows.Forms.DialogResult.OK Then
            PictureBox1.Image = Image.FromFile(opf.FileName)
        End If
    End Sub

    Public Sub imageR()

        Dim cnn As MySqlConnection
        Dim connectionString As String
        connectionString = "server=localhost;user id=root;database=employee_management"
        cnn = New MySqlConnection(connectionString)

        Dim stream As New MemoryStream()
        cnn.Open()
        Dim command As New MySqlCommand("select image from employee where id=@id", cnn)
        command.Parameters.Add("@id", MySqlDbType.Int64).Value = DataGridView1.SelectedRows(0).Cells(0).Value.ToString
        Dim image As Byte() = DirectCast(command.ExecuteScalar(), Byte())
        stream.Write(image, 0, image.Length)
        cnn.Close()
        Dim bitmap As New Bitmap(stream)
        PictureBox1.Image = bitmap
    End Sub

    Private Sub Form2_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        Application.Exit()
    End Sub

    Private Sub ButtonFilter_Click(sender As Object, e As EventArgs) Handles ButtonFilter.Click
        Me.Hide()
        Form3.Show()
    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class