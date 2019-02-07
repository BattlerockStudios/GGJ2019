using InControl;

namespace Battlerock
{
    public class CharacterAction : PlayerActionSet
    {
        public PlayerAction MoveLeft;
        public PlayerAction MoveRight;
        public PlayerAction MoveUp;
        public PlayerAction MoveDown;

        public PlayerAction RotateLeft;
        public PlayerAction RotateRight;
        public PlayerAction RotateUp;
        public PlayerAction RotateDown;

        public PlayerTwoAxisAction Movement;
        public PlayerTwoAxisAction Rotation;

        public PlayerAction Interact;
        public PlayerAction ExitInteraction;

        public CharacterAction()
        {
            MoveLeft = CreatePlayerAction("Move Left");
            MoveRight = CreatePlayerAction("Move Right");
            MoveUp = CreatePlayerAction("Move Up");
            MoveDown = CreatePlayerAction("Move Down");
            Movement = CreateTwoAxisPlayerAction(MoveLeft, MoveRight, MoveDown, MoveUp);

            RotateLeft = CreatePlayerAction("Rotate Left");
            RotateRight = CreatePlayerAction("Rotate Right");
            RotateUp = CreatePlayerAction("Rotate Up");
            RotateDown = CreatePlayerAction("Rotate Down");
            Rotation = CreateTwoAxisPlayerAction(RotateLeft, RotateRight, RotateDown, RotateUp);

            Interact = CreatePlayerAction("Interact");
            ExitInteraction = CreatePlayerAction("ExitInteraction");
        }
    }
}