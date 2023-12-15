using Godot;
using System;

public class TriggerDialogTest : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Connect("body_entered", this, "OnDialogTriggerBodyEntered");
    }

    public void OnDialogTriggerBodyEntered(Node body)
    {
        GD.Print("we did it boys");
        if (body.IsInGroup("Player"))
        {
            GetTree().ReloadCurrentScene();
        }

    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
