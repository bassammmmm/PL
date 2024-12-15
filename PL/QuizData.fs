module QuizData

open Models
open System.IO
open Newtonsoft.Json
open Newtonsoft.Json.Linq

let loadQuizFromFile (filePath: string) =
    let json = File.ReadAllText(filePath)
    let data = JsonConvert.DeserializeObject<JObject>(json)
    data.Properties()
    |> Seq.map (fun prop ->
        let key = prop.Name
        let value = prop.Value :?> JObject
        let questionType = value.["type"].ToString()
        let question =
            let questionText = value.["questionText"].ToString()
            let correctAnswer = value.["correctAnswer"].ToString()
            let questionId = value.["id"].ToObject<int>()
            match questionType with
            | "WrittenQuestion" ->
                WrittenQuestion(questionText, correctAnswer, questionId)
            | "MultipleChoiceQuestion" ->
                let choices = value.["choices"].ToObject<string[]>()
                MultipleChoiceQuestion(questionText, List.ofArray choices, correctAnswer, questionId)
            | _ -> failwith "Unknown question type"
        (key, question)
    )
    |> Map.ofSeq
