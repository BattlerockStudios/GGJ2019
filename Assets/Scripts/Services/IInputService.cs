using Battlerock;
using UnityEngine;

public interface IInputService
{
    CharacterAction Action { get; set; }

    Vector2 GetDirection();
    bool GetInteractButtonPressed();
    bool GetInteractButtonReleased();
    float GetHorizontalDirection();
    bool GetExitInteractionButtonPressed();
    bool GetExitInteractionButtonReleased();
    float GetVerticalDirection();
    void InitializeInput();
    string GetInteractButtonName();
    float GetLeftDirection();
    float GetRightDirection();
    float GetUpDirection();
    float GetDownDirection();
}
