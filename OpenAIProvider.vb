Imports DNDORGANIZAR.IAprovider

Public Class OpenAIProvider
    Implements IAIProvider

    Private ReadOnly apiKey As String
    Private ReadOnly model As String

    Public Sub New(apiKey As String, Optional model As String = "gpt-3.5-turbo")
        Me.apiKey = apiKey
        Me.model = model
    End Sub

    Public Async Function QueryAsync(prompt As String) As Task(Of String) Implements IAIProvider.QueryAsync
        ' Copia aquí la lógica de QueryAIAsync (petición HTTP a OpenAI), usando apiKey y model.
        ' Devuelve el content string o lanza excepción si falla.
        ' (Por brevedad no repito todo; tu QueryAIAsync lo puedes convertir en método aquí.)
        Return Await Task.FromResult("Implementar llamada real a OpenAI aquí")
    End Function
End Class