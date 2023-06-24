using Godot;
using System;
using System.ComponentModel.Design;
using System.Threading.Tasks;

/// <summary>
/// Used to change which Keyboard/ Controller Inputs move the player. Fore Example remap from wasd to esdf as movement.
/// </summary>
public partial class RemapInputs : Control
{
    public enum RemapedInputs   // the different inputs that can be remapped, used in _Input to to see what should be replaced
    {
        none, up, down, left, right, dash
    }

    [Export] private Panel inputPanel;
    private RemapInputsButton currentRemapInputsButton;
    private bool replaceInput = false;

    private InputsResource inputsResource;

    public override void _Ready()
    {
        inputPanel = (Panel)GetNode("SetInputPanel");

        if (inputsResource == null)
        {
            inputsResource = InputsResource.LoadInputsResource();
        }
    }


    public override void _Input(InputEvent inputEvent)
    {
        if (replaceInput == false)
        {
            return;
        }

        if (inputEvent is InputEventKey inputKey)
        {
            if(inputKey.AsTextKeycode() == "Escape")
            {
                GD.Print("Escape was Pressed to Cancel assingning");
            }
            else if (inputsResource.IsKeyUsedAnywhere(inputKey.AsText()))
            {
                GD.Print("Button is already used somewhere");
            }
            else
            {
                inputsResource.ReplaceInputMapAction(currentRemapInputsButton.inputToRemapType, currentRemapInputsButton.positionInRemapArray, inputEvent);
            }

           
            currentRemapInputsButton.UpdateButtonText();
            setBlockPanelVisibility(false);
            replaceInput = false;
        }
    }



    public void OnButtonPressed(RemapInputsButton clickedButton)
    {
        setBlockPanelVisibility(true);
        replaceInput = true;
        currentRemapInputsButton = clickedButton;

    }

    private void setBlockPanelVisibility(bool visibleBool)
    {
        inputPanel.Visible = visibleBool;
    }

    public string GetTextForButton(RemapInputsButton button)
    {
        if (inputsResource == null)
        {
            inputsResource = InputsResource.LoadInputsResource();
        }

        return inputsResource.GetTextForEvent(button.inputToRemapType, button.positionInRemapArray);
    }

}
