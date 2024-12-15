module Models

type Question =
    | WrittenQuestion of questionText: string * correctAnswer: string * Id: int
    | MultipleChoiceQuestion of questionText: string * choices: string list * correctAnswer: string * Id: int

let getCorrectAnswer = function
    | WrittenQuestion(_, correctAnswer, id) -> correctAnswer
    | MultipleChoiceQuestion(_, _, correctAnswer, id) -> correctAnswer

type QuestionEntry =
    { Id: int
      Type: string
      QuestionText: string
      Choices: string list option
      CorrectAnswer: string }

type QuizData = 
    { Questions: Map<string, Question> 
      UserAnswers: Map<string, string> }


