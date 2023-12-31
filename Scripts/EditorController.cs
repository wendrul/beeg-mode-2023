using System;
using Godot;
using System.Collections.Generic;

namespace BeegMode2023.Scripts
{
    public class EditorController : Node2D
    {
        public static bool hasPower;
        public enum Platforms
        {
            Platform,
            Wall,
            Spikes,
            Vertical
        }
      
        
        public bool HasObject;
        public bool IsHoveringAnObject;
        private PlaceablePlatform currentPlatform;
        private PackedScene Platform = GD.Load<PackedScene>("res://Prefabs/Floor.tscn");
        private PackedScene Wall = GD.Load<PackedScene>("res://Prefabs/wall.tscn");
        private PackedScene Spikes = GD.Load<PackedScene>("res://Prefabs/FloorSpikes.tscn");
        private PackedScene Vertical = GD.Load<PackedScene>("res://Prefabs/VerticalPlatform.tscn");

        private bool _isEditorMode = false;
        private CanvasLayer _ui;
        private TileMap _grid;
        private Node2D _nonUIelements;
        private CharacterController _player;


        private void TallyUplatforms(string platformType)
        {
            if (Utilities.PlatformTally.TryGetValue(platformType, out int val))
            {
                Utilities.PlatformTally[platformType] = val + 1;
            }
            else
            {
                Utilities.PlatformTally.Add(platformType, 1);
            } 
            foreach (KeyValuePair<string, int> kvp in Utilities.PlatformTally)
            {
                //textBox3.Text += ("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
                GD.Print(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
            }
        }

        public override void _Ready()
        {
            _ui = Utilities.GetChildByType<CanvasLayer>(this, false);
            _grid = GetNode<TileMap>("./Grid");
            _nonUIelements = GetNode<Line2D>("./DebugLine");
            _player = GetNode<CharacterController>("/root/rootNode/CharacterController");
            ExitEditorMode();
        }
        
        public override void _PhysicsProcess(float delta)
        {
        }

        private void EnterEditorMode()
        {
            Engine.TimeScale = 0.1f; 
            _ui.Show();
            _nonUIelements.Show();
            _grid.Show();
            _grid.Position = _player.Position;
            float x = _grid.Position.x - (_grid.Position.x % _grid.CellSize.x);
            float y = _grid.Position.y - (_grid.Position.y % _grid.CellSize.y);
            _grid.Position = new Vector2(x, y);
            _player.EditorModeLockInputs = true;
            _isEditorMode = true;
        }

        private void ExitEditorMode()
        {
            Engine.TimeScale = 1;
            if (currentPlatform != null)
            {
                var tween = CreateTween();
                tween.TweenProperty(
                    currentPlatform, "scale", new Vector2(1.2f, 1.2f), .1f);
                tween.Chain().TweenProperty(
                    currentPlatform, "scale", new Vector2(1f, 1f), .05f);
                currentPlatform.OnPlacement -= ExitEditorMode;
                currentPlatform = null;
            }
            _ui.Hide();
            _nonUIelements.Hide();
            _grid.Hide();
            _player.EditorModeLockInputs = false;
            _isEditorMode = false;
        }
        

        private void ToggleEditorMode()
        {
            if (!_isEditorMode) EnterEditorMode();
            else ExitEditorMode();
        }

        public override void _Process(float delta)
        {
            if (EditorController.hasPower)
            {
                if (Input.IsActionJustPressed("toggle_editor_mode"))
                {
                    ToggleEditorMode();
                }
            }
            
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
                case Platforms.Spikes:
                    platform = (PlaceablePlatform)Spikes.Instance();
                    break;
                case Platforms.Vertical:
                    platform = (PlaceablePlatform)Vertical.Instance();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            AddChild(platform);
            platform.followMouse = true;
            HasObject = true;
            platform.OnPlacement += ExitEditorMode;
            platform.OnPlacement += () => TallyUplatforms(platformType.ToString());
            currentPlatform = platform;
        }
        
        
    }
}
