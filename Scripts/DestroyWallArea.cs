using Godot;
using System;
using BeegMode2023.Scripts;

public class DestroyWallArea : Area2D
{
    [Export(PropertyHint.MultilineText)] public string msg;
    private bool triggered;

    public override void _Ready()
    {
        triggered = false;
        Connect("body_entered", this, "OnDestroyWallAreaBodyEnter");        
    }
    
    public void OnDestroyWallAreaBodyEnter(Node body)
    {
        if (body.IsInGroup("Player"))
        {
            if (!triggered)
            {
                triggered = true;
                DestroyFourthWall();
            }

        }
    }

    private void DestroyFourthWall()
    {
        Utilities.PopUpNPCDialog(msg);
        Utilities.MusicPlayer.Play();
        EditorController.hasPower = true;
    }
}
