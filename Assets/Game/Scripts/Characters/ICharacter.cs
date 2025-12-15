public interface ICharacter

{
    float InjuredMoveSpeed { get; }

    void SetMoveSpeed(float speed);
    void SetDeathState(bool isDead);
    void StopMove();
    void ResumeMove();
}