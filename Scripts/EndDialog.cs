using Godot;
using System;
using BeegMode2023.Scripts;

public class EndDialog : Area2D
{
    private bool alreadyPoped;
    private AudioStreamPlayer _music;

    public override void _Ready()
    {
        _music = GetNode<AudioStreamPlayer>("/root/rootNode/music");
        Connect("body_entered", this, "OnDialogTriggerBodyEntered");
    }

    public void OnDialogTriggerBodyEntered(Node body)
    {
        if (body.IsInGroup("Player"))
        {
            int deaths = Utilities.Deaths;
            int platforms = 0, walls = 0,  spikes = 0;

            if (Utilities.PlatformTally.TryGetValue("Platform", out int val))
            {
                    platforms = val;
            }
            if (Utilities.PlatformTally.TryGetValue("Wall", out int val2))
            {
                    walls = val2;
            }
            if (Utilities.PlatformTally.TryGetValue("Spikes", out int val3))
            {
                    spikes = val3;
            }


            if (!alreadyPoped)
            {
                string tallyMsg = $"It only took you {deaths} tries, using {platforms} platforms {walls} walls and {spikes} spikes";
                _music.Stop();

                if (spikes > 0)
                {
                    tallyMsg += "(what did you even use those for?)";

                }
                string message = string.Format(@"
Oh you're here! Finally...

{0}

But enough of that, isn't it nice to be up here? 

Taking in the view, letting the world run at it's own pace while keeping this distance?

No? I should've guessed that...

You didn't seem to be like anyone living here, I'm sure this all looks very different to you. Maybe you've seen the most beautiful pixels and landscapes...

But you know, this is all I know.

For me, this is the most beautiful thing I've ever seen, for as ugly and monotone as it may seem.

[monologue continues]

[continues further]

[...]

[...]

Enjoy your last moments here.

Farewell traveler,

                    ", tallyMsg);

                alreadyPoped = true;
                Utilities.PopUpNPCDialog(message);
            } else
            {
                
                string tallyMsg = $"You've died {deaths} times, used {platforms} platforms, {walls} walls and {spikes} spikes";

                if (spikes > 0)
                {
                    tallyMsg += "(are you ok?)";

                }
                Utilities.PopUpNPCDialog(tallyMsg);
            }
        }

    }
}
