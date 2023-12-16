using System;
using Godot;
using System.Collections.Generic;

namespace BeegMode2023.Scripts
{
    public static class Utilities
    {
        public static DialogText DialogEntity {get; set;}
        public static DeathScreen DeatScreenEntity {get; set;}
        public static music MusicPlayer { get; internal set; }
        public static Dictionary<string, int> PlatformTally {get; set;} = new Dictionary<string, int>();
        public static Vector2 LastCheckpoint { get; internal set; } = new Vector2(-379, -32);
        public static bool ShouldPlayMusciOnStart { get; internal set; }

        public static int Deaths = 0;

        public static T GetChildByType<T>(this Node node, bool recursive = true) 
            where T : Node 
        {

            int childCount = node.GetChildCount();

            for (int i = 0; i < childCount; i++)

            {

                Node child = node.GetChild(i);

                if (child is T childT)

                    return childT;

                if (recursive && child.GetChildCount() > 0)

                {

                    T recursiveResult = child.GetChildByType<T>(true);

                    if (recursiveResult != null)

                        return recursiveResult;

                }

            }

            return null;

        }
        public static void PopUpNPCDialog(string dialog)
        {
            DialogEntity.PopUpDialog(dialog);
        }

        internal static void DeathByWiiGlitch()
        {
            DeatScreenEntity.DeathByWiiGlitch();
            Deaths += 1;
        }
    }

}
