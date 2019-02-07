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

        public Vector2 GetMovementDirection()
        {
            return m_action.Movement.Value;
        }

        public float GetHorizontalMovementDirection()
        {
            return m_action.Movement.X;
        }

        public float GetVerticalMovementDirection()
        {
            return m_action.Movement.Y;
        }

        public float GetHorizontalRotationDirection()
        {
            return m_action.Rotation.X;
        }

        public float GetVerticalRotationDirection()
        {
            return m_action.Rotation.Y;
        }

        public bool GetInteractButtonPressed()
        {
            return m_action.Interact.WasPressed;
        }     

        public bool GetExitInteractionButtonPressed()
        {
            return m_action.ExitInteraction.WasPressed;
        } 

        /// <summary>
        /// Sets up the InControl input system for keyboard/mouse and controllers.
        /// </summary>
        public void InitializeInput()
        {
            Action = new CharacterAction();

            #region Gamepad Controller

            // D-Pad
            Action.MoveLeft.AddDefaultBinding(InputControlType.DPadLeft);
            Action.MoveRight.AddDefaultBinding(InputControlType.DPadRight);
            Action.MoveUp.AddDefaultBinding(InputControlType.DPadUp);
            Action.MoveDown.AddDefaultBinding(InputControlType.DPadDown);

            // Left Analog Stick
            Action.MoveLeft.AddDefaultBinding(InputControlType.LeftStickLeft);
            Action.MoveRight.AddDefaultBinding(InputControlType.LeftStickRight);
            Action.MoveUp.AddDefaultBinding(InputControlType.LeftStickUp);
            Action.MoveDown.AddDefaultBinding(InputControlType.LeftStickDown);

            // Right Analog Stick
            Action.RotateLeft.AddDefaultBinding(InputControlType.RightStickLeft);
            Action.RotateRight.AddDefaultBinding(InputControlType.RightStickRight);
            Action.RotateUp.AddDefaultBinding(InputControlType.RightStickUp);
            Action.RotateDown.AddDefaultBinding(InputControlType.RightStickDown);

            // Buttons
            Action.Interact.AddDefaultBinding(InputControlType.Action1);
            Action.ExitInteraction.AddDefaultBinding(InputControlType.Action2);

            #endregion

            #region Keyboard + Mouse

            #region Rotation Input
           
            Action.RotateLeft.AddDefaultBinding(Mouse.NegativeX);
            Action.RotateRight.AddDefaultBinding(Mouse.PositiveX);
            Action.RotateUp.AddDefaultBinding(Mouse.PositiveY);
            Action.RotateDown.AddDefaultBinding(Mouse.NegativeY);

            #endregion

            #region Movement Input
           
            Action.MoveLeft.AddDefaultBinding(Key.LeftArrow);
            Action.MoveRight.AddDefaultBinding(Key.RightArrow);
            Action.MoveUp.AddDefaultBinding(Key.UpArrow);
            Action.MoveDown.AddDefaultBinding(Key.DownArrow);

            Action.MoveLeft.AddDefaultBinding(Key.A);
            Action.MoveRight.AddDefaultBinding(Key.D);
            Action.MoveUp.AddDefaultBinding(Key.W);
            Action.MoveDown.AddDefaultBinding(Key.S);

            #endregion

            Action.Interact.AddDefaultBinding(Key.Z);
            Action.ExitInteraction.AddDefaultBinding(Key.X);
            Action.Interact.AddDefaultBinding(Mouse.LeftButton);
            Action.ExitInteraction.AddDefaultBinding(Mouse.RightButton);

            #endregion
        }
    }
}