Imports System.Data
Imports System.Data.SqlClient

Public Class Form1
    Private connection As SqlConnection
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RadioButton1.Checked = True
        connection = New SqlConnection
        connection.ConnectionString = "Data Source=.\SQLEXPRESS;Initial Catalog=MAGATZEM;Trusted_Connection=True;"
        Try
            connection.Open()
        Catch
            MsgBox("problema en el servidor de base de datos")
        End Try

        If connection.State = ConnectionState.Closed Then
            MsgBox("tanquem")
            Me.Close()
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        cargarInformacion()

    End Sub

    Private Sub cargarInformacion()
        Dim command As SqlCommand
        Dim reader As SqlDataReader
        Dim apellidos, nombre, idEmpleado As String

        command = New SqlCommand
        command.Connection = connection

        If RadioButton1.Checked = True Then
            command.CommandText = "select idEmpleado, Apellidos, Nombre,Cargo from Empleados order by idEmpleado"
        Else
            command.CommandText = "select idEmpleado, Apellidos, Nombre,Cargo from Empleados order by Apellidos"

        End If

        reader = command.ExecuteReader
        ListBox1.Items.Clear()
        ListBox2.Items.Clear()

        While reader.Read()
            apellidos = reader("Apellidos").ToString
            nombre = reader("Nombre").ToString
            idEmpleado = reader("idEmpleado").ToString
            ListBox1.Items.Add(nombre + "-" + apellidos)
            ComboBox1.Items.Add(nombre + "-" + apellidos)
            ListBox2.Items.Add(idEmpleado)
        End While
        reader.Close()

    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        Dim command, command1 As SqlCommand
        Dim reader, reader1 As SqlDataReader
        Dim posicion As Integer
        Dim id As String
        Dim jefe As String


        posicion = ListBox1.SelectedIndex
        id = ListBox2.Items.Item(posicion)

        command = New SqlCommand
        command.Connection = connection

        command1 = New SqlCommand
        command1.Connection = connection
        command.CommandText = "select idEmpleado, Apellidos, Nombre,Cargo,FechaNacimiento,Jefe from Empleados where idEmpleado = " + id
        reader = command.ExecuteReader


        reader.Read()
        TextBox1.Text = reader("idEmpleado").ToString
        TextBox2.Text = reader("Apellidos").ToString
        TextBox3.Text = reader("Nombre").ToString
        TextBox4.Text = reader("Cargo").ToString
        TextBox5.Text = reader("FechaNacimiento").ToString
        jefe = reader("Jefe").ToString
        MsgBox(jefe)
        reader.Close()

        If jefe IsNot "" Then
            command1.CommandText = "select Apellidos, Nombre from Empleados where idEmpleado = " & Integer.Parse(jefe)
            reader1 = command1.ExecuteReader
            If reader1.Read() Then
                For c As Integer = 1 To ComboBox1.Items.Count - 1
                    If (ComboBox1.Items.Item(c).Equals(reader1("Nombre").ToString & "-" & reader1("Apellidos").ToString)) Then
                        ComboBox1.SelectedIndex = c

                    Else
                        ComboBox1.SelectedIndex = 0
                    End If
                Next

            End If
            reader1.Close()
        Else
            ComboBox1.SelectedIndex = 0
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            ListBox1.SelectedIndex = ListBox1.SelectedIndex + 1
        Catch
            MsgBox("No hay más empleados.")
        End Try

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim command As SqlCommand

        command = New SqlCommand()
        command.Connection = connection
        command.CommandText = String.Format("DELETE FROM Empleados where idempleado =" + TextBox1.Text)

        If command.ExecuteNonQuery() = 0 Then
            MsgBox("No se ha borrado ningun empleado")
        Else
            MsgBox("Empleado Borrado")
            cargarInformacion()
            ListBox1.SelectedIndex = 0
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim command As SqlCommand

        command = New SqlCommand()
        command.Connection = connection
        command.CommandText = String.Format("update empleados set apellidos = '{0}',nombre = '{1}',cargo = '{2}',FechaNacimiento = '{3}',jefe = '{4}' where idempleado = {5}", TextBox2.Text, TextBox3.Text, TextBox4.Text, TextBox5.Text, ComboBox1.SelectedIndex, TextBox1.Text)

        If command.ExecuteNonQuery() = 0 Then
            MsgBox("No se ha modificado la información")
        Else
            MsgBox("Midificacion realizada")
            cargarInformacion()

        End If
    End Sub
End Class
