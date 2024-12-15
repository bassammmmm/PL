module QuizUI

open Models
open System.Windows.Forms
open Utilities
open System.Drawing
open System.Diagnostics
open QuizLogic

let panel = new Panel(AutoScroll = true, Dock = DockStyle.Fill)


let createQuizForm quizData =
    let form = new Form(Text = "Quiz Application", Size = Size(600, 500))
    let submitButton = new Button(Text = "Submit", Dock = DockStyle.Bottom, Height = 50)
    submitButton.BackColor <- Color.FromArgb(60, 120, 240)
    submitButton.ForeColor <- Color.White
    submitButton.Font <- new Font("Arial", 12.0f, FontStyle.Bold)
    submitButton.FlatStyle <- FlatStyle.Flat
    submitButton.FlatAppearance.BorderSize <- 0
    submitButton.Padding <- Padding(10)

    let rec renderQuestions (quizData: QuizData) (panel: Panel) =
        let y = ref 10
        quizData.Questions
        |> Map.iter (fun key question ->
            let questionLabel = new Label(
                Location = Point(10, !y),
                AutoSize = true,
                Font = new Font("Arial", 12.0f, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60),
                Text = 
                    match question with
                    | WrittenQuestion(questionText, _, Id) -> questionText
                    | MultipleChoiceQuestion(questionText, _, _, Id) -> questionText
            )
            panel.Controls.Add(questionLabel)
            y := !y + 40

            match question with
            | WrittenQuestion(_, _, Id) ->
                let textbox = new TextBox(Location = Point(10, !y), Width = 500, Height = 30)
                textbox.Name <- Id.ToString();
                textbox.Font <- new Font(textbox.Font.FontFamily, 12.0f) // Larger font for easier readability
                textbox.BorderStyle <- BorderStyle.FixedSingle
                textbox.BackColor <- Color.FromArgb(240, 240, 240)
                textbox.ForeColor <- Color.FromArgb(50, 50, 50)
                textbox.Padding <- Padding(10)
                panel.Controls.Add(textbox)
                textbox.TextChanged.Add(fun _ ->
                    let newUserAnswers = Map.add key textbox.Text quizData.UserAnswers
                    let updatedQuizData = { quizData with UserAnswers = newUserAnswers }
                    renderQuestions updatedQuizData panel
                )
            | MultipleChoiceQuestion(_, choices, _, Id) ->
                let comboBox = new ComboBox(
                    Location = Point(10, !y),
                    Width = 500,
                    DropDownStyle = ComboBoxStyle.DropDownList
                )
                comboBox.Name <- Id.ToString();
                comboBox.Font <- new Font("Arial", 12.0f)
                comboBox.ForeColor <- Color.FromArgb(50, 50, 50)
                comboBox.BackColor <- Color.FromArgb(240, 240, 240)
                comboBox.Items.AddRange(choices |> List.toArray |> Array.map box)
                panel.Controls.Add(comboBox)
                comboBox.SelectedIndexChanged.Add(fun _ ->
                    if comboBox.SelectedItem <> null then
                        let newUserAnswers = Map.add key (comboBox.SelectedItem.ToString()) quizData.UserAnswers
                        let updatedQuizData = { quizData with UserAnswers = newUserAnswers }
                        renderQuestions updatedQuizData panel
                )

            y := !y + 60
        )

    renderQuestions quizData panel
    form.Controls.Add(panel)
    form.Controls.Add(submitButton)

  
    submitButton.Click.Add(fun _ ->

        let userAnswers = fetchAllUserAnswers panel quizData.Questions
        Debug.WriteLine($"Score: {userAnswers}")
        let score = calculateScore userAnswers quizData.Questions
        Debug.WriteLine($"Score: {score}")
        let resultMessage = $"Your score is: {score}/{quizData.Questions.Count}"
        let resultDialog = MessageBox.Show(resultMessage, "Quiz Result", MessageBoxButtons.OK, MessageBoxIcon.Information)
        if resultDialog = DialogResult.OK then
            form.Close()
    )

    form
