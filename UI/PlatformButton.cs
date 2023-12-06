using Godot;
using System;
using BeegMode2023.Scripts;

public class PlatformButton : Button
{
    [Export] public EditorController.Platforms platformType;
    private EditorController _editorController;

    public override void _Ready()
    {
        _editorController =  Utilities.GetChildByType<EditorController>(GetTree().Root, true);
        SetupButton();
    }

    private void SetupButton()
    {
        /*var rng = new RandomNumberGenerator();
        rng.Randomize();
        int max = Enum.GetNames(typeof(EditorController.Platforms)).Length - 1;
        var i = rng.RandiRange(0,max);
        platformType= (EditorController.Platforms )i;*/
        Text = platformType.ToString();
       // GD.Print("PlatformType: " + platformType.ToString());
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("1"))
        {
            SetupButton();
        }
        
    }
    private void On_Button_button_down()
    {
        _editorController.ProcessPlatformToSpawn(platformType);
    }
    
 
}





