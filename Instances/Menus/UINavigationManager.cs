using Godot;
using System;

/// <summary>
/// Handles Navigation in UI Menus by switching between Mouse and Keyboard Navigation.
/// </summary>
public partial class UINavigationManager : Control
{
    [Export] private Control focusNodeOnStart; // the first Node that will be Highlighted when navigating a menu
    [Export] private Panel mouseBlockPanel;  // stops mouse UI Interaction while active

    private Control currentlyFocusedNode;

    private double mouseMoveDistance = 0f;
    private const double distanceToGetVisable = 20;

    private enum UIControlTypeEnum
    {
        MouseControl,
        KeyboardControl
    }
    private UIControlTypeEnum currentControlType;

    public override void _Ready()
    {
        if (focusNodeOnStart != null)
        {
            focusNodeOnStart.GrabFocus();
            currentlyFocusedNode = focusNodeOnStart;
        }
        else
        {
            GD.PrintErr("assign first highlighted control node in UINavigationManager");
        }
        SwitchToKeyboardControl();

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

        currentlyFocusedNode.GrabFocus();   // to insure the Focus gets removed, Focus wouldnt get removed out of Scene if a different Control currently has Focus
        currentlyFocusedNode.ReleaseFocus();
    }

    private void SwitchToKeyboardControl()
    {
        mouseBlockPanel.Visible = true;
        GD.PrintErr("Change the way UINavigationManager works, reset when this gets disabled");
        MoveMouseScreenPosition(GetViewport().GetMousePosition() + new Vector2(0.1f,0));   // ugly temporary solution. Fixes the Problem, that an UI element still thinks its being hovered by the mouse after the block panel was added. this gets updated once the mouse is moved


        currentlyFocusedNode.GrabFocus();

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
