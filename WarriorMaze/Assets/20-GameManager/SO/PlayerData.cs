using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Maze Wizard/Player Data")]
public class PlayerData : ScriptableObject, APlayer
{
    public string charName;          
    public float movementSpeed;
    public float rotationSpeed;

    public void movement(Vector2 move, CharacterController controller, float dt)
    {
        controller.transform.Rotate(Vector3.up * (move.x * rotationSpeed * dt));

        controller.Move(controller.transform.forward * (move.y * movementSpeed * dt));
    }
}

public interface APlayer
{
    public void movement(Vector2 move, CharacterController controller, float dt);
}
