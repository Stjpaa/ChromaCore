using Godot;
using System;

/// <summary>
/// Handles Navigation in UI Menus by switching between Mouse and Keyboard Navigation.
/// </summary>
public partial class UINavigationManager : Control
{
    [Export] private Control focusNodeOnStart; // the first Node that will be Highlighted when navigating a menu
    [Export] private Panel mouseBlockPanel;  // stops mouse UI Interaction while active

    private double mouseMoveDistance = 0f;
    private const double distanceToGetVisable = 20;

    private bool readyHappened = false;

    private enum UIControlTypeEnum
    {
        MouseControl,
        KeyboardControl
    }
    private UIControlTypeEnum currentControlType;

    public void SignalOnMenuVisabilityChange()  // happens whenever this Node becomes visable in scene. -> the menu this is part of grabs ui focus
    {
        if (readyHappened == false)     // this signal gets called before Ready happens which causes an error, because the sceneTree doesnst seem to be created before this is called.
        {                               
            return; 
        }

        if (this.IsVisibleInTree())
        {

            if (Input.MouseMode == Input.MouseModeEnum.Visible)
            {
                SwitchToMouseControl();
            }
            else
            {
                SwitchToKeyboardControl();
            }
        }
        else     // when no longer visable
        {
            mouseBlockPanel.Visible = false;
        }
    }



    public override void _Ready()
    {
        readyHappened = true;
        if (this.IsVisibleInTree() == false) // dont grab focus if this isnst the active menu
        {
            return;
        }

        SwitchToKeyboardControl();  // on the first menu start with keyboard control

    }


    public override void _Process(double delta)
    {

        switch (currentControlType)
        {
            case UIControlTypeEnum.KeyboardControl:
                {
                    mouseMoveDistance += Input.GetLastMouseVelocity().Length() * delta;

                    if (mouseMoveDistance >= distanceToGetVisable)
                    {
                        SwitchToMouseControl();
                        mouseMoveDistance = 0;
                    }
                }
                break;

            case UIControlTypeEnum.MouseControl:
                {
                    if (KeyboardInteractionHappened())
                    {
                        SwitchToKeyboardControl();
                    }
                }
                break;
        }
    }

    private void MoveMouseScreenPosition(Vector2 position)
    {
        Input.WarpMouse(position);
    }

    private void SwitchToMouseControl()
    {
        mouseBlockPanel.Visible = false;
        Input.MouseMode = Input.MouseModeEnum.Visible;
        currentControlType = UIControlTypeEnum.MouseControl;

        focusNodeOnStart.GrabFocus();   // to insure the Focus gets removed, Focus wouldnt get removed out of Scene if a different Control currently has Focus
        focusNodeOnStart.ReleaseFocus();
    }

    private void SwitchToKeyboardControl()
    {

        mouseBlockPanel.Visible = true;
        //MoveMouseScreenPosition(GetViewport().GetMousePosition() + new Vector2(0.1f, 0));   // ugly temporary solution. Fixes the Problem, that an UI element still thinks its being hovered by the mouse after the block panel was added. this gets updated once the mouse is moved


        focusNodeOnStart.GrabFocus();

        Input.MouseMode = Input.MouseModeEnum.Hidden;
        currentControlType = UIControlTypeEnum.KeyboardControl;
        mouseMoveDistance = 0;  // Reset Distance because 
    }

    private bool KeyboardInteractionHappened()
    {
        // maybe switch to on any Key pressed not just some ui events.

        if (Input.IsActionJustPressed("ui_up"))
        {
            return true;
        }
        else if (Input.IsActionJustPressed("ui_down"))
        {
            return true;
        }
        else if (Input.IsActionJustPressed("ui_accept"))
        {
            return true;
        }
        else if (Input.IsActionJustPressed("ui_left"))
        {
            return true;
        }
        else if (Input.IsActionJustPressed("ui_right"))
        {
            return true;
        }
        else
        {
            return false;
        }


    }


}
