using System;
using Godot;

namespace BeegMode2023.Scripts
{
    public class EditorController : Node2D
    {
        public enum Platforms
        {
            Platform,
            Wall
        }
        public bool HasObject;
        public bool IsHoveringAnObject;
        private Platforms currentPlatform = Platforms.Platform;
        private PackedScene Platform = GD.Load<PackedScene>("res://Prefabs/Floor.tscn");
        private PackedScene Wall = GD.Load<PackedScene>("res://Prefabs/wall.tscn");

        private bool _isEditorMode = false;
        private CanvasLayer _ui;

        public override void _Ready()
        {
            _ui = Utilities.GetChildByType<CanvasLayer>(this, false);
            ExitEditorMode();
        }
        
        public override void _PhysicsProcess(float delta)
        {
        }

        private void EnterEditorMode()
        {
            Engine.TimeScale = 0.2f; 
            _ui.Show();
            _isEditorMode = true;
        }
        
        private void ExitEditorMode()
        {
            Engine.TimeScale = 1; 
            _ui.Hide();
            _isEditorMode = false;
        }

        private void ToggleEditorMode()
        {
            if (!_isEditorMode) EnterEditorMode();
            else ExitEditorMode();
        }

        public override void _Process(float delta)
        {
            if (Input.IsActionJustPressed("toggle_editor_mode"))
            {
                ToggleEditorMode();
            }
            //Select type of platform
            /*
            if (Input.IsActionPressed("1"))
                currentPlatform = Platforms.Platform;
            if (Input.IsActionPressed("2"))
                currentPlatform = Platforms.Wall;
                */
            
            //Spawn or movePlatform
        }


        public void ProcessPlatformToSpawn(Platforms platformType)
        {
            PlaceablePlatform platform;
            switch (platformType)
            {
                case Platforms.Platform:
                    platform = (PlaceablePlatform)Platform.Instance();
                    break;
                case Platforms.Wall:
                    platform = (PlaceablePlatform)Wall.Instance();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            AddChild(platform);
            platform.followMouse = true;
            HasObject = true;
            platform.onPlacement = () => this.ExitEditorMode();
        }
        
    }
}
