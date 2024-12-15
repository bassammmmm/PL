module QuizData

open Models
open System.IO
open Newtonsoft.Json
open Newtonsoft.Json.Linq

let loadQuizFromFile (filePath: string) =
    let json = File.ReadAllText(filePath) // Reads the JSON content as a string
    let data = JsonConvert.DeserializeObject<JObject>(json) // Deserialize JSON into JObject
    data.Properties()
    |> Seq.map (fun prop ->
        let key = prop.Name
        let value = prop.Value :?> JObject
        let questionType = value.["type"].ToString()
        let question =
            match questionType with
            | "WrittenQuestion" ->
                let questionText = value.["questionText"].ToString()
                let correctAnswer = value.["correctAnswer"].ToString()
                WrittenQuestion(questionText, correctAnswer)
            | "MultipleChoiceQuestion" ->
                let questionText = value.["questionText"].ToString()
                let choices = value.["choices"].ToObject<string[]>()
                let correctAnswer = value.["correctAnswer"].ToString()
                MultipleChoiceQuestion(questionText, List.ofArray choices, correctAnswer)
            | _ -> failwith "Unknown question type"
        (key, question)
    )
    |> Map.ofSeq
