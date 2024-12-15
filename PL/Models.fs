module Models

type Question =
    | WrittenQuestion of questionText: string * correctAnswer: string
    | MultipleChoiceQuestion of questionText: string * choices: string list * correctAnswer: string

// Define a function to get the correct answer for each question type
let getCorrectAnswer = function
    | WrittenQuestion(_, correctAnswer) -> correctAnswer
    | MultipleChoiceQuestion(_, _, correctAnswer) -> correctAnswer

// Define the structure for each question entry
type QuestionEntry =
    { Type: string
      QuestionText: string
      Choices: string list option
      CorrectAnswer: string }

// Define a data type for quiz state that is immutable
type QuizData = 
    { Questions: Map<string, Question> 
      UserAnswers: Map<string, string> }


