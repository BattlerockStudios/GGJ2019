using Battlerock;
using UnityEngine;

public interface IInputService
{
    CharacterAction Action { get; set; }

    void InitializeInput();
    bool GetInteractButtonPressed();
    bool GetExitInteractionButtonPressed();
    float GetHorizontalMovementDirection();
    float GetVerticalMovementDirection();
    float GetHorizontalRotationDirection();
    float GetVerticalRotationDirection();
}
