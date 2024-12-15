module QuizLogic

open Models
open Utilities
open System.Windows.Forms
open System.Diagnostics

let fetchAllUserAnswers (panel: Panel) (questions: Map<string, Question>) =
    questions
    |> Map.fold (fun acc key question ->
        let questionId = 
            match question with
            | WrittenQuestion(_, _, id) -> id.ToString()
            | MultipleChoiceQuestion(_, _, _, id) -> id.ToString()
        match fetchControlValue panel questionId with
        | Some answer -> (key, answer) :: acc
        | None -> acc
    ) []


let calculateScore (userAnswers: (string * string) list) (questions: Map<string, Question>) =
    userAnswers
    |> List.fold (fun score (questionId, userAnswer) ->
        match Map.tryFind questionId questions with
        | Some (WrittenQuestion(_, correctAnswer, _)) when userAnswer = correctAnswer -> score + 1
        | Some (MultipleChoiceQuestion(_, _, correctAnswer, _)) when userAnswer = correctAnswer -> score + 1
        | _ -> score
    ) 0
