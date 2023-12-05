using Godot;
using System;
using BeegMode2023.Scripts;

public class Spikes : Node2D
{

    [Export] public Node Node;
    private Area2D _area2D;
    
    public override void _Ready()
    {
        Test2();
    }

    public override void _PhysicsProcess(float delta)
    {
    }


    public void On_Area2D_body_entered(Node body)
    {
        GD.Print("body entered");
        if (body.IsInGroup("Player"))
        {
            GetTree().ReloadCurrentScene();
        }

    }

    public override void _Process(float delta)
    {
    }


    private void Test2()
    {
        var tween = CreateTween().SetLoops();
        tween.TweenProperty(this, "position:y", 10f, .2f).SetDelay(2f);
        tween.Chain().TweenProperty(this, "position:y", 0f, .5f).SetDelay(3f);
    }
    private async void Test()
    {
      
    }
    
}
