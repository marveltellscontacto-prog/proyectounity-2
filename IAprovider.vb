Public Class IAprovider
    Public Interface IAIProvider
        Function QueryAsync(prompt As String) As Task(Of String)
    End Interface
End Class
