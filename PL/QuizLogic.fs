module QuizLogic

open Models

let calculateScore (userAnswers: Map<string, string>) (questions: Map<string, Question>) =
    questions
    |> Map.fold (fun score key question ->
        match question with
        | WrittenQuestion(_, correctAnswer) ->
            if userAnswers |> Map.tryFind key = Some correctAnswer then
                score + 1
            else
                score
        | MultipleChoiceQuestion(_, choices, correctAnswer) ->
            if userAnswers |> Map.tryFind key = Some correctAnswer then
                score + 1
            else
                score
    ) 0
