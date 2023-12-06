using Godot;
using System;
using BeegMode2023.Scripts;

public class Spikes : Node2D
{
    private PlaceablePlatform _placeablePlatform;
    private CollisionShape2D collision;
    [Export] private NodePath platformPath;
    
    public override void _Ready()
    {
        _placeablePlatform = (PlaceablePlatform)GetNode(platformPath);
        collision = Utilities.GetChildByType<CollisionShape2D>(this, true);
        collision.Disabled = true;
        _placeablePlatform.OnPlacement += SetUpSpikes;
    }

    //Signal
    public void On_Area2D_body_entered(Node body)
    {
        if (body.IsInGroup("Player"))
        {
            GetTree().ReloadCurrentScene();
        }

    }

    public override void _ExitTree()
    {
        _placeablePlatform.OnPlacement -= SetUpSpikes;
    }


    private void SetUpSpikes()
    {
        collision.Disabled = false;
        var tween = CreateTween().SetLoops();
        tween.TweenProperty(this, "position:y", 10f, .05f).SetDelay(2f);
        tween.Chain().TweenProperty(this, "position:y", 0f, .05f).SetDelay(3f);
    }
 
    
}
