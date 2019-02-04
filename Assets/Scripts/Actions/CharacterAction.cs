using InControl;

namespace Battlerock
{
    public class CharacterAction : PlayerActionSet
    {
        public PlayerAction Left;
        public PlayerAction Right;
        public PlayerAction Up;
        public PlayerAction Down;
        public PlayerTwoAxisAction Direction;

        public PlayerAction Interact;
        public PlayerAction ExitInteraction;
        public PlayerAction Jump;

        public CharacterAction()
        {
            Left = CreatePlayerAction("Move Left");
            Right = CreatePlayerAction("Move Right");
            Up = CreatePlayerAction("Move Up");
            Down = CreatePlayerAction("Move Down");
            Direction = CreateTwoAxisPlayerAction(Left, Right, Down, Up);

            Interact = CreatePlayerAction("Interact");
            ExitInteraction = CreatePlayerAction("ExitInteraction");
            Jump = CreatePlayerAction("Jump");
        }
    }
}