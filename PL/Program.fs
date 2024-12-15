// For more information see https://aka.ms/fsharp-console-apps
open System.Windows.Forms
open QuizData
open QuizUI
open Models
[<EntryPoint>]

let main _ =
    // Path to your JSON file
    let filePath = @"C:\Users\bassa\source\repos\PL\PL\questions.json" 

    // Load questions from the JSON file
    try
        let questions = loadQuizFromFile filePath
        let quizData = { Questions = questions; UserAnswers = Map.empty }

        // Set up and run the Windows Form with the quiz data
        Application.EnableVisualStyles()
        Application.Run(createQuizForm quizData)

        // Return 0 when everything runs successfully
        0
    with
    | :? System.IO.FileNotFoundException -> 
        MessageBox.Show("The questions.json file could not be found.", "Error") |> ignore
        1
    | ex ->
        MessageBox.Show($"An error occurred: {ex.Message}", "Error") |> ignore
        1
