using System;
using Godot;

namespace BeegMode2023.Scripts
{
    public class EditorController : Node2D
    {
        enum Platforms
        {
            Platform,
            Wall
        }
        public bool HasObject;
        public bool IsHoveringAnObject;
        private Platforms currentPlatform = Platforms.Platform;
        private PackedScene Platform = GD.Load<PackedScene>("res://Prefabs/Floor.tscn");
        private PackedScene Wall = GD.Load<PackedScene>("res://Prefabs/wall.tscn");
        public override void _PhysicsProcess(float delta)
        {
            base._PhysicsProcess(delta);
        }

        public override void _Process(float delta)
        {
            //Select type of platform
            if (Input.IsActionPressed("1"))
                currentPlatform = Platforms.Platform;
            if (Input.IsActionPressed("2"))
                currentPlatform = Platforms.Wall;
            
            //Spawn or movePlatform
            if (Input.IsActionPressed("mouse_click") && !HasObject && !IsHoveringAnObject)
                ProcessPlatformToSpawn();
            else if (!Input.IsActionPressed("mouse_click") && HasObject)
                HasObject = false;
        }

        private void ProcessPlatformToSpawn()
        {
            switch (currentPlatform)
            {
                case Platforms.Platform:
                    Testing testPlatform = (Testing)Platform.Instance();
                    AddChild(testPlatform);
                    testPlatform.followMouse = true;
                    HasObject = true;
                    break;
                case Platforms.Wall:
                    Testing wallTest = (Testing)Wall.Instance();
                    AddChild(wallTest);
                    wallTest.followMouse = true;
                    HasObject = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public override void _Ready()
        {
            base._Ready();
        }
    }
}
