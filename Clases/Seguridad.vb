Imports System.Security.Cryptography

Public Class Seguridad

    Private Const TamanoSalt As Integer = 16
    Private Const TamanoHash As Integer = 32
    Private Const Iteraciones As Integer = 100000

    Public Shared Function GenerarHash(password As String) As String
        Dim salt(TamanoSalt - 1) As Byte
        Using rng As New RNGCryptoServiceProvider()
            rng.GetBytes(salt)
        End Using

        Using pbkdf2 As New Rfc2898DeriveBytes(password, salt, Iteraciones, HashAlgorithmName.SHA256)
            Dim hash As Byte() = pbkdf2.GetBytes(TamanoHash)
            Return Convert.ToBase64String(salt) & ":" & Convert.ToBase64String(hash)
        End Using
    End Function

    Public Shared Function VerificarPassword(password As String, hashGuardado As String) As Boolean
        Dim partes As String() = hashGuardado.Split(":"c)
        If partes.Length <> 2 Then Return False

        Dim salt As Byte() = Convert.FromBase64String(partes(0))
        Dim hashEsperado As Byte() = Convert.FromBase64String(partes(1))

        Using pbkdf2 As New Rfc2898DeriveBytes(password, salt, Iteraciones, HashAlgorithmName.SHA256)
            Dim hashCalculado As Byte() = pbkdf2.GetBytes(TamanoHash)

            'Comparación sin cortar en la primera diferencia
            Dim diferencia As Integer = 0
            For i As Integer = 0 To hashEsperado.Length - 1
                diferencia = diferencia Or (hashEsperado(i) Xor hashCalculado(i))
            Next
            Return diferencia = 0
        End Using
    End Function

End Class