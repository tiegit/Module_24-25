using UnityEngine;

public class CharacterControllerDirectionalMover : DirectionalMover
{
    private CharacterController _characterController;


    public CharacterControllerDirectionalMover(CharacterController characterController,
                                               float moveSpeed,
                                               ObstacleChecker groundChecker,
                                               float gravityForce) : base(moveSpeed, groundChecker, gravityForce)
    {
        _characterController = characterController;

        SetMoveSpeed(moveSpeed);
    }

    protected override void ApplyVelocity(Vector3 velocity, float deltaTime) =>
        _characterController.Move(velocity * deltaTime);
}
