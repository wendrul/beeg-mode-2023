using Godot;
using System;
using BeegMode2023.Scripts;

public class DialogTrigger : Area2D
{
    [Export(PropertyHint.MultilineText)] public string message = "hey, i'm lonely";
    [Export] public bool oneShot = true;
    private bool alreadyPoped = false;

    public override void _Ready()
    {
        Connect("body_entered", this, "OnDialogTriggerBodyEntered");
    }

    public void OnDialogTriggerBodyEntered(Node body)
    {
        if (body.IsInGroup("Player"))
        {
            if (!oneShot || !alreadyPoped)
            {
                alreadyPoped = true;
                Utilities.PopUpNPCDialog(message);
            }
        }

    }
}
