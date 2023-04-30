using Godot;
using System;

/// <summary>
/// Handles Navigation in UI Menus by switching between Mouse and Keyboard Navigation.
/// </summary>
public partial class UINavigationManager : Control
{
    [Export] private Control focusNodeOnStart; // the first Node that will be Highlighted when navigating a menu

    private Control currentlyFocusedNode;

    private double mouseMoveDistance = 0f;
    private const double distanceToGetVisable = 10;

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
        Input.MouseMode = Input.MouseModeEnum.Visible;
        currentControlType = UIControlTypeEnum.MouseControl;

        currentlyFocusedNode.GrabFocus();   // to insure the Focus gets removed, Focus wouldnt get removed out of Scene if a different Control currently has Focus
        currentlyFocusedNode.ReleaseFocus();
    }

    private void SwitchToKeyboardControl()
    {
        GD.PrintErr("switch on any Key pressed not justui_up/down");

        currentlyFocusedNode.GrabFocus();

        Input.MouseMode = Input.MouseModeEnum.Hidden;
        currentControlType = UIControlTypeEnum.KeyboardControl;

        mouseMoveDistance = 0;  // Reset Distance because 
    }

    private bool KeyboardInteractionHappened()
    {
        if (Input.IsActionJustPressed("ui_up"))
        {
            return true;
        }
        else if (Input.IsActionJustPressed("ui_down"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
