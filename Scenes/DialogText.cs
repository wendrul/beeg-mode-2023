using Godot;
using System;
using System.Collections.Generic;
using BeegMode2023.Scripts;

public class DialogText : RichTextLabel
{
    private CanvasLayer _wholeThing;
    private Tween _tween;
    private bool isDialoging;
    private IEnumerator<int> currentDialog;

    public override void _Ready()
    {
        _wholeThing = GetNode<CanvasLayer>("/root/rootNode/Dialog");
        _wholeThing.Hide();
        _tween = GetNode<Tween>("./Tween");
        Utilities.DialogEntity = this;
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("jump"))
        {
            if (isDialoging)
            {
                this.currentDialog.MoveNext();
            }
        }
    }

    private IEnumerator<int> NextDialog(string currentMessage)
    {
        foreach (string rawMsg in currentMessage.Split('\n'))
        {
            string msg = rawMsg.Trim();
            if (msg.Empty())
            {
                continue;
            }
            BbcodeText = msg;
            PercentVisible = 0;
            
            float secsPerChar = 0.01f;
            
            _tween.Reset(this);
            _tween.InterpolateProperty(this, "percent_visible", 0.0f, 1.0f, secsPerChar * msg.Length , Tween.TransitionType.Cubic, Tween.EaseType.InOut);
            _tween.Start();
            yield return 1; 
        } 
        EndDialog();
        yield break;
    }

    private void EndDialog()
    {
        _wholeThing.Hide();
        isDialoging = false;
    }

    public void PopUpDialog(string message)
    {
        _wholeThing.Show();
        currentDialog = NextDialog(message);
        currentDialog.MoveNext();
        isDialoging = true;
    }

    public void SkipDialog()
    {
        if (isDialoging)
        {
            _tween.Stop(this);
            this.PercentVisible = 1f;
            isDialoging = false;
        }
    }
}
