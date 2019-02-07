using InControl;
using UnityEngine;

namespace Battlerock
{
    public class InputService : IInputService
    {
        /// <summary>
        /// Incontrol input actions/button values.
        /// </summary>
        private CharacterAction m_action;

        /// <summary>
        /// Incontrol input actions/button values.
        /// </summary>
        public CharacterAction Action
        {
            get
            {
                return m_action;
            }

            set
            {
                m_action = value;
            }
        }

        public InputService()
        {
            InitializeInput();
        }

        public Vector2 GetDirection()
        {
            return m_action.Direction.Value;
        }

        public float GetHorizontalDirection()
        {
            return m_action.Direction.X;
        }

        public float GetVerticalDirection()
        {
            return m_action.Direction.Y;
        }

        public bool GetInteractButtonPressed()
        {
            return m_action.Interact.WasPressed;
        }

        public bool GetInteractButtonReleased()
        {
            return m_action.Interact.WasReleased;
        }

        public bool GetExitInteractionButtonPressed()
        {
            return m_action.ExitInteraction.WasPressed;
        }

        public bool GetExitInteractionButtonReleased()
        {
            return m_action.ExitInteraction.WasReleased;
        }

        public string GetInteractButtonName()
        {
            return m_action.Interact.Name;
        }

        public float GetLeftDirection()
        {
            return m_action.Direction.Left.Value;
        }

        public float GetRightDirection()
        {
            return m_action.Direction.Right.Value;
        }

        public float GetUpDirection()
        {
            return m_action.Direction.Up.Value;
        }

        public float GetDownDirection()
        {
            return m_action.Direction.Down.Value;
        }

        /// <summary>
        /// Sets up the InControl input system for keyboard/mouse and controllers.
        /// </summary>
        public void InitializeInput()
        {
            Action = new CharacterAction();

            #region Gamepad Controller

            // D-Pad
            Action.Left.AddDefaultBinding(InputControlType.DPadLeft);
            Action.Right.AddDefaultBinding(InputControlType.DPadRight);
            Action.Up.AddDefaultBinding(InputControlType.DPadUp);
            Action.Down.AddDefaultBinding(InputControlType.DPadDown);

            // Left Analog Stick
            Action.Left.AddDefaultBinding(InputControlType.LeftStickLeft);
            Action.Right.AddDefaultBinding(InputControlType.LeftStickRight);
            Action.Up.AddDefaultBinding(InputControlType.LeftStickUp);
            Action.Down.AddDefaultBinding(InputControlType.LeftStickDown);

            // Buttons
            Action.Interact.AddDefaultBinding(InputControlType.Action1);
            Action.ExitInteraction.AddDefaultBinding(InputControlType.Action2);

            #endregion

            #region Keyboard + Mouse

            Action.Left.AddDefaultBinding(Mouse.NegativeX);
            Action.Right.AddDefaultBinding(Mouse.PositiveX);
            Action.Up.AddDefaultBinding(Mouse.PositiveY);
            Action.Down.AddDefaultBinding(Mouse.NegativeY);

            Action.Left.AddDefaultBinding(Key.LeftArrow);
            Action.Right.AddDefaultBinding(Key.RightArrow);
            Action.Left.AddDefaultBinding(Key.A);
            Action.Right.AddDefaultBinding(Key.D);

            Action.Up.AddDefaultBinding(Key.UpArrow);
            Action.Down.AddDefaultBinding(Key.DownArrow);
            Action.Up.AddDefaultBinding(Key.W);
            Action.Down.AddDefaultBinding(Key.S);

            Action.Interact.AddDefaultBinding(Key.Z);
            Action.ExitInteraction.AddDefaultBinding(Key.X);

            #endregion
        }
    }
}