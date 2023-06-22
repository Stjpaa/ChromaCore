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
        none,up, down, left, right, dash, jump, interact
    }

    [Export] private Panel inputPanel;
    private RemapInputsButton currentRemapInputsButton;
    private bool replaceInput = false;

    private InputEvent[] currenControls;
    private RemapInputsSavedata remapInputsSavedata = new RemapInputsSavedata();


    private InputEvent keyboardOne;
    private InputEvent keyboardTwo;
    private InputEvent controllerOne;

    private string actionName = "ui_up";

    public override void _Ready()
    {
        inputPanel = (Panel)GetNode("SetInputPanel");

        if(currentRemapInputsButton == null)
        {
            LoadSaveData();
        }

        //GD.PrintErr("could cause Problems ith the order, because new actions would cause controllerOne to move up to [1] in the InputList should probalby safe new Button");

        //UpdateCurrentInputevents(actionName);

        //PrintAllInputs(actionName);

        //InputEvent replacementEvent = controllerOne;
        //ReplaceInputEvent(actionName, keyboardOne, replacementEvent);
        //PrintAllInputs(actionName);
    }

    //private void UpdateCurrentInputevents(string action)
    //{
    //    var currentInputEvents = InputMap.ActionGetEvents(action);

    //    keyboardOne = currentInputEvents[0];
    //    keyboardTwo = currentInputEvents[1];
    //    controllerOne = currentInputEvents[2];
    //}

    private void PrintAllInputs(string action)
    {
        foreach (var input in InputMap.ActionGetEvents(action))
        {
            GD.Print(input.AsText());
        }
    }

    /// <summary>
    /// Removes a Event that is currently in the InputMap and Replaces it with a new one.
    /// 
    /// </summary>
    public void ReplaceInputEvent(string action, InputEvent eventToRemove, InputEvent eventToAdd)
    {
        //causes Problems if the same Input is assigned twice to the same action,
        //or if it is part of a different action (for example up and rigth are both assigned d -> causes Problem)

        InputMap.ActionEraseEvent(action, eventToRemove);
        InputMap.ActionAddEvent(action, eventToAdd);
    }

    public override void _Input(InputEvent inputEvent)
    {
        if (replaceInput == false)
        {
            return;
        }

        if (inputEvent is InputEventKey inputKey)
        {
            GD.Print(inputEvent.AsText());

            //ReplaceInputEvent(currentRemapInputsButton.actionName, controllerOne, inputEvent);

            HandleInputType(currentRemapInputsButton, inputKey);
            
            currentRemapInputsButton.Text = inputEvent.AsText();
            setBlockPanelVisibility(false);
            replaceInput = false;
        }
        //GD.Print(inputEvent.GetType());

        //GD.Print(inputEvent.AsText());
    }

    private void HandleInputType(RemapInputsButton pressedButton, InputEventKey newInput)
    {
        switch (pressedButton.inputToRemapType)
        {
            case RemapedInputs.none:
                {
                    GD.Print("no type was assigned on remap button");
                }
                break;

            case RemapedInputs.up:
                {
                    //jump
                    //ui_up
                    ReplaceInputEvent("ui_up", currenControls[pressedButton.positionInRemapArray], newInput);
                    ReplaceInputEvent("ui_up", currenControls[pressedButton.positionInRemapArray], newInput);

                }
                break;

            case RemapedInputs.down:
                {


                }
                break;

            case RemapedInputs.left:
                {


                }
                break;

            case RemapedInputs.right:
                {


                }
                break;

            case RemapedInputs.dash:
                {


                }
                break;

            case RemapedInputs.jump:
                {


                }
                break;

            case RemapedInputs.interact:
                {


                }
                break;

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
        if(currenControls == null)
        {
            GD.Print("controls where not yet assigned");
            LoadSaveData();
        }

        return "Test";
    }

    private void LoadSaveData()
    {
        string pathToSavedata = ProjectSettings.GlobalizePath(SaveSystem.pathOfRemapInputsData);
        if (SaveSystem.DoesFileExistAtPath(pathToSavedata))
        {
            //remapInputsSavedata = Loaded;
            //remapInputsSavedata = SaveSystem.LoadRemapInputsSavedata();

            //GD.Print("Load + " +remapInputsSavedata.upKeyboard1.AsText());//Load existing Savedata
            
        }
        else
        {
            remapInputsSavedata.LoadBaseInputMap();//Use base values
            SaveSystem.SaveRemapInputsSavedata(remapInputsSavedata);
        }
    }
}
