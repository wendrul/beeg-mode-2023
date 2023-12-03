using Godot;
using System;

public class UI : CanvasLayer
{
    [Export] public NodePath labelPath;

    private int _score;
    private int Score
    {
        get => _score;
        set
        {
            _score = value;
            UpdateScoreLabel();
        }
    }

    private Label scoreLabel;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        scoreLabel = (Label)GetNode(labelPath);
        scoreLabel.Text = "Ready for test";
        Score = 0;
    }

    private void UpdateScoreLabel()
    {
        GD.Print(Score);
        scoreLabel.Text = Score.ToString();
    }
    
}



