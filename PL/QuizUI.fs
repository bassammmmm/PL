module QuizUI

open Models
open System.Windows.Forms
open System.Drawing

let createQuizForm quizData =
    // Create the form
    let form = new Form(Text = "Quiz Application", Size = Size(600, 500))
    let panel = new Panel(AutoScroll = true, Dock = DockStyle.Fill)
    let submitButton = new Button(Text = "Submit", Dock = DockStyle.Bottom, Height = 50)
    submitButton.BackColor <- Color.FromArgb(60, 120, 240)
    submitButton.ForeColor <- Color.White
    submitButton.Font <- new Font("Arial", 12.0f, FontStyle.Bold)
    submitButton.FlatStyle <- FlatStyle.Flat
    submitButton.FlatAppearance.BorderSize <- 0
    submitButton.Padding <- Padding(10)
    // Immutable user answers that won't mutate the original quiz data
   
    let userAnswers = ref quizData.UserAnswers
    // Function to render the questions using pattern matching
    let rec renderQuestions (quizData: QuizData) (panel: Panel) =
        let y = ref 10
        quizData.Questions
        |> Map.iter (fun key question ->
            // Question label
            let questionLabel = new Label(
                Location = Point(10, !y),
                AutoSize = true,
                Font = new Font("Arial", 12.0f, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60),
                Text = 
                    match question with
                    | WrittenQuestion(questionText, _) -> questionText
                    | MultipleChoiceQuestion(questionText, _, _) -> questionText
            )
            panel.Controls.Add(questionLabel)
            y := !y + 40

            // Pattern matching to create controls based on question type
            match question with
            | WrittenQuestion(_, _) ->
                // TextBox for written questions
                let textbox = new TextBox(Location = Point(10, !y), Width = 500, Height = 30)
                textbox.Font <- new Font(textbox.Font.FontFamily, 12.0f) // Larger font for easier readability
                textbox.BorderStyle <- BorderStyle.FixedSingle
                textbox.BackColor <- Color.FromArgb(240, 240, 240)
                textbox.ForeColor <- Color.FromArgb(50, 50, 50)
                textbox.Padding <- Padding(10)
                panel.Controls.Add(textbox)
                textbox.TextChanged.Add(fun _ ->
                    // Create a new map with the updated answer
                    userAnswers := Map.add key textbox.Text !userAnswers
                    // Re-render the questions with the updated answers
                    renderQuestions { quizData with UserAnswers = !userAnswers } panel
                )
            | MultipleChoiceQuestion(_, choices, _) ->
                // ComboBox for multiple choice questions
                let comboBox = new ComboBox(
                    Location = Point(10, !y),
                    Width = 500,
                    DropDownStyle = ComboBoxStyle.DropDownList
                )
                comboBox.Font <- new Font("Arial", 12.0f)
                comboBox.ForeColor <- Color.FromArgb(50, 50, 50)
                comboBox.BackColor <- Color.FromArgb(240, 240, 240)
                comboBox.Items.AddRange(choices |> List.toArray |> Array.map box)
                panel.Controls.Add(comboBox)
                comboBox.SelectedIndexChanged.Add(fun _ ->
                    if comboBox.SelectedItem <> null then
                        // Create a new map with the updated answer
                        userAnswers := Map.add key (comboBox.SelectedItem.ToString()) !userAnswers
                        // Re-render the questions with the updated answers
                        renderQuestions { quizData with UserAnswers = !userAnswers } panel
                )

            y := !y + 60 // Increase spacing between questions
        )

    renderQuestions quizData panel
    form.Controls.Add(panel)
    form.Controls.Add(submitButton)
// Function to update the user answers in an immutable way
   

    // Submit button logic
    submitButton.Click.Add(fun _ ->
        printfn "User Answers: %A" quizData.UserAnswers
        let score = QuizLogic.calculateScore !userAnswers quizData.Questions
        // Show the score and then close the form when OK is clicked
        let resultMessage = $"Your score is: {score}/{quizData.Questions.Count}"
        let resultDialog = MessageBox.Show(resultMessage, "Quiz Result", MessageBoxButtons.OK, MessageBoxIcon.Information)
        if resultDialog = DialogResult.OK then
            form.Close() // Close the form after the user clicks OK
    )

    form
