using UnityEngine;

public class EnemyController : BaseTank
{
    [Header("Bot Options")]
    [SerializeField, Min(0)]
    private float _movementTime = 2f;
}
