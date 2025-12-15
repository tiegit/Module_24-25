using UnityEngine;
using UnityEngine.AI;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] private CharacterView _characterView;
    [SerializeField] private Health _characterHealth;
    [SerializeField] private HealthBarView _healthBarView;

    [SerializeField, Space(15)] Pointer _pointerPrefab;
    [SerializeField] private GameObject _patrolPointPrefab;

    [SerializeField, Space(15)] private Character _enemyCharacter;
    [SerializeField] private CharacterView _enemyCharacterView;
    [SerializeField] private Health _enemyCharacterHealth;

    [SerializeField, Space(15)] private AgentCharacter _agentEnemyCharacter;
    [SerializeField] private AgentCharacterView _agentEnemyCharacterView;
    [SerializeField] private Health _agentEnemyCharacterHealth;
    [SerializeField] private HealthBarView _agentEnemyHealthBarView;
    [SerializeField] private float _idleBehaviourSwitchTime = 4f;

    [SerializeField, Space(15)] private MineManager _mineManager;

    private Controller _characterController;
    private Controller _enemyCharacterController;
    private Controller _agentEnemyCharacterController;

    private NavMeshPath _path;
    private MovementControllerHandler _movementHandler;

    private void Awake()
    {
        PlayerInput playerInput = new PlayerInput();

        _path = new NavMeshPath();

        NavMeshQueryFilter queryFilter = new NavMeshQueryFilter();
        queryFilter.agentTypeID = 0;
        queryFilter.areaMask = NavMesh.AllAreas;

        ClickToMoveController playerMoveController = new(playerInput, _character, queryFilter);

        Pointer pointer = Instantiate(_pointerPrefab);
        pointer.Initialize(playerMoveController);

        var patrolPointPrefabInstance = Instantiate(_patrolPointPrefab);

        DirectionalMovableAutoPatrolController playerAutoPatrolController = 
            new DirectionalMovableAutoPatrolController(_character, queryFilter, 15f, 0.5f, 0.2f, patrolPointPrefabInstance);

        _characterController = new CompositeController(playerMoveController,
                                                       /*new PlayerDirectionalMovableController(playerInput, _character),*/
                                                       playerAutoPatrolController,
                                                       new AlongMovableVelocityRotatableController(_character, _character));
        _characterController.Enable();

        _movementHandler = new MovementControllerHandler(playerInput, playerMoveController, playerAutoPatrolController, _idleBehaviourSwitchTime);

        _characterView.Initialize(_character);

        _enemyCharacterController = new CompositeController(new DirectionalMovableAgroController(_enemyCharacter, _character.transform, 10f, 2f, queryFilter, 1f),
                                                            new AlongMovableVelocityRotatableController(_enemyCharacter, _enemyCharacter));
        _enemyCharacterController.Enable();
        _enemyCharacterView.Initialize(_enemyCharacter);

        _agentEnemyCharacterController = new AgentCharacterAgroController(_agentEnemyCharacter, _character.transform, 20, 2, 1);

        _agentEnemyCharacterController.Enable();

        _agentEnemyCharacterView.Initialize(_agentEnemyCharacter);

        DamagableManager damagableManager = new DamagableManager();

        _characterHealth.Initialize(_character, damagableManager, _characterView);
        _healthBarView.Initialize(_characterHealth);

        _agentEnemyCharacterHealth.Initialize(_agentEnemyCharacter, damagableManager, _agentEnemyCharacterView);
        _agentEnemyHealthBarView?.Initialize(_agentEnemyCharacterHealth);

        _enemyCharacterHealth.Initialize(_enemyCharacter, damagableManager, _enemyCharacterView);

        _mineManager.Initialize(damagableManager);
    }

    //private void Start() => _enemyCharacter.gameObject.SetActive(false);

    private void Update()
    {
        _characterController.Update(Time.deltaTime);
        _enemyCharacterController.Update(Time.deltaTime);
        _agentEnemyCharacterController.Update(Time.deltaTime);

        _movementHandler.Update(Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.F))
            _characterHealth.TakeDamage(10);
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
            return;

        NavMeshQueryFilter queryFilter = new NavMeshQueryFilter();
        queryFilter.agentTypeID = 0;
        queryFilter.areaMask = NavMesh.AllAreas;

        NavMesh.CalculatePath(_enemyCharacter.transform.position, _character.transform.position, queryFilter, _path);

        Gizmos.color = Color.red;

        if (_path.status != NavMeshPathStatus.PathInvalid)
            foreach (Vector3 corner in _path.corners)
                Gizmos.DrawSphere(corner, 0.3f);
    }
}