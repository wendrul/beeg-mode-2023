using Godot;
using System;
using BeegMode2023.Scripts;

public class DestroyWallArea : Area2D
{
    [Export(PropertyHint.MultilineText)] public string msg;
    //[Export] public NodePath deathScreenFollow;
    private Path2D screenFollow;
    private bool triggered;

    public override void _Ready()
    {
        triggered = false;
       // screenFollow = GetNode<Path2D>(deathScreenFollow);
       // screenFollow.GetParent().RemoveChild(screenFollow);
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
        //AddChild(screenFollow);
        Utilities.PopUpNPCDialog(msg);
        Utilities.MusicPlayer.Play();
        EditorController.hasPower = true;
    }
}
