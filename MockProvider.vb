Imports System.Threading.Tasks
Imports DNDORGANIZAR.IAprovider

Public Class MockProvider
    Implements IAIProvider

    ' Simula respuestas útiles dirigidas a DnD.
    Public Function QueryAsync(prompt As String) As Task(Of String) Implements IAIProvider.QueryAsync
        ' Respuesta simple: aplicar regla básica para detectar presets
        Dim lower = If(prompt, "").ToLowerInvariant()
        If lower.Contains("encuentro") Then
            Return Task.FromResult("Encuentro sugerido: 3 lobos (CR 1/4) + 1 lobo alfa (CR 1) en un claro del Bosque Encantado. Recompensa: 30 monedas de oro y un mapa medio quemado. Gancho: los animales evitan la antigua cruz de piedra cerca de la cascada.")
        ElseIf lower.Contains("npc") Then
            Return Task.FromResult("NPC sugerido: Garen, un cazador retirado, humano, hombre, 42 años. Rasgos: reservado, con cicatriz en la mejilla; Motivación: recuperar un amuleto familiar que perdió en el bosque. Ganchos de roleo y tres líneas para introducirlo en la escena.")
        ElseIf lower.Contains("botín") Or lower.Contains("loot") Then
            Return Task.FromResult("Botín sugerido: Cofre pequeño con 2 pociones menores de curación, 45 monedas de plata, y una daga con runas que brilla levemente a la medianoche.")
        Else
            ' Devolver una respuesta genérica y útil
            Return Task.FromResult("Respuesta MOCK: " & prompt & vbCrLf & vbCrLf & "Sugerencia: prueba con plantillas 'Generar Encuentro', 'Generar NPC' o 'Generar Loot' para pruebas rápidas.")
        End If
    End Function
End Class