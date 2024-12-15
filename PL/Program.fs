open System.Windows.Forms
open QuizData
open QuizUI
open Models
[<EntryPoint>]

let main _ =
    // Path to your JSON file
    let filePath = @"D:\PL3\PL\PL\PL\questions.json" 

    try
        let questions = loadQuizFromFile filePath
        let quizData = { Questions = questions; UserAnswers = Map.empty }

        Application.EnableVisualStyles()
        Application.Run(createQuizForm quizData)

        0
    with
    | :? System.IO.FileNotFoundException -> 
        MessageBox.Show("The questions.json file could not be found.", "Error") |> ignore
        1
    | ex ->
        MessageBox.Show($"An error occurred: {ex.Message}", "Error") |> ignore
        1
