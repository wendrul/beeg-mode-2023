using Godot;

namespace BeegMode2023.Scripts
{
    public class MovingPlatform : PlaceablePlatform
    {
        public enum MovementType
        {
            Horizontal,
            Vertical
        }

        [Export] public MovementType _movementType;
        private float originalPos;
        public override void _Ready()
        {
            base._Ready();
            base.OnPlacement += StartMovement;
        }

        private void StartMovement()
        {
            originalPos = Position.y;
            var tween = CreateTween().SetLoops();
            if (_movementType == MovementType.Vertical)
            {
                
                tween.TweenProperty(this, "position:y", Position.y + 200f, 3f).SetDelay(.5f);
                tween.Chain().TweenProperty(this, "position:y", originalPos, 3f).SetDelay(.5f);
            }
            else
            {
                
            }
        }
    }
}