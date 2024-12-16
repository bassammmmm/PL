module Utilities
open System.Windows.Forms
open System.Diagnostics
//utilites
let fetchControlValue (panel: Panel) (name: string) =
    Debug.WriteLine(name);
    let controls = panel.Controls.Find(name, true)
    if controls.Length > 0 then
        match controls.[0] with
        | :? TextBox as textbox -> Some(textbox.Text)
        | :? ComboBox as comboBox -> 
            if comboBox.SelectedItem <> null then 
                Some(comboBox.SelectedItem.ToString())
            else 
                None
        | _ -> None
    else
        None
