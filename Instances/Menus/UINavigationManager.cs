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

    private enum UIControlTypeEnum
    {
        MouseControl,
        KeyboardControl
    }
    private UIControlTypeEnum currentControlType;

    public void SignalOnMenuVisabilityChange()  // happens whenever this Node becomes visable in scene. -> the menu this is part of grabs ui focus
    {
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
        if (this.IsVisibleInTree() == false) // dont grab focus if this isnst the active menu
        {
            return;
        }

        GD.Print("visible");

        SwitchToKeyboardControl();  // on the first menu start with keyboard control

    }


    public override void _Process(double delta)
    {

        //GD.Print("hello");
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
        GD.Print("SwitchToMouseControl");
        mouseBlockPanel.Visible = false;
        Input.MouseMode = Input.MouseModeEnum.Visible;
        currentControlType = UIControlTypeEnum.MouseControl;

        focusNodeOnStart.GrabFocus();   // to insure the Focus gets removed, Focus wouldnt get removed out of Scene if a different Control currently has Focus
        focusNodeOnStart.ReleaseFocus();
    }

    private void SwitchToKeyboardControl()
    {
        GD.Print("SwitchToKeyboardControl");

        GD.PrintErr("Change the way UINavigationManager works, reset when this gets disabled");
        mouseBlockPanel.Visible = true;
        MoveMouseScreenPosition(GetViewport().GetMousePosition() + new Vector2(0.1f, 0));   // ugly temporary solution. Fixes the Problem, that an UI element still thinks its being hovered by the mouse after the block panel was added. this gets updated once the mouse is moved


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
