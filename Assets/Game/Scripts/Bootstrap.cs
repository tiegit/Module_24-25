using UnityEngine;
using UnityEngine.AI;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] Pointer _pointerPrefab;
    [SerializeField] private GameObject _patrolPointPrefab;

    [SerializeField, Space(20)] private Character _character;
    [SerializeField] private Animator _characterAnimator;
    [SerializeField, Space(5)] private HealthBarView _characterHealthBarView;

    [SerializeField, Space(20)] private AgentCharacter _agentCharacter;
    [SerializeField] private Animator _agentCharacterAnimator;
    [SerializeField, Space(5)] private HealthBarView _agentCharacterhealthBarView;

    [SerializeField, Space(20)] private Character _enemyCharacter;
    [SerializeField] private Animator _enemyCharacterAnimator;

    [SerializeField, Space(20)] private AgentCharacter _agentEnemyCharacter;
    [SerializeField] private Animator _agentEnemyCharacterAnimator;
    [SerializeField, Space(5)] private HealthBarView _agentEnemyHealthBarView;
    [SerializeField, Space(5)] private float _idleBehaviourSwitchTime = 4f;

    private Controller _characterController;
    private Controller _agentCharacterController;
    private Controller _enemyCharacterController;
    private Controller _agentEnemyCharacterController;

    private CharacterView _characterView;

    private CharacterView _agentCharacterView;
    private AgentCharacterJumpView _agentCharacterJumpView;
    private CharacterView _enemyCharacterView;

    private CharacterView _agentEnemyCharacterView;
    private AgentCharacterJumpView _agentEnemyCharacterJumpView;


    private NavMeshPath _path;
    private MovementControllerHandler _movementHandler;
    private PlayerClickInputHandler _clickHandler;

    private void Awake()
    {
        PlayerInput playerInput = new PlayerInput();
        _clickHandler = new PlayerClickInputHandler(playerInput);

        var patrolPointPrefabInstance = Instantiate(_patrolPointPrefab);

        NavMeshQueryFilter queryFilter = new NavMeshQueryFilter();
        queryFilter.agentTypeID = 0;
        queryFilter.areaMask = NavMesh.AllAreas;

        #region Character

        _characterView = new CharacterView(_characterAnimator, _character, _character, this);
        HealthMediator characterHealthMediator = new HealthMediator(_characterView);

        _character.Initialize(characterHealthMediator);

        ClickToMoveController playerMoveController = new ClickToMoveController(_clickHandler, _character, _character, _character, queryFilter);

        Pointer pointer = Instantiate(_pointerPrefab);
        pointer.Initialize(playerMoveController);

        DirectionalMovableAutoPatrolController playerAutoPatrolController =
            new DirectionalMovableAutoPatrolController(_character, queryFilter, 15f, 0.5f, 0.2f, patrolPointPrefabInstance);

        _characterController = new CompositeController(playerMoveController,
                                                       playerAutoPatrolController,
                                                       new AlongMovableVelocityRotatableController(_character, _character));
        _characterController.Enable();

        _characterHealthBarView?.Initialize(_character);

        _movementHandler = new MovementControllerHandler(playerInput, playerMoveController, playerAutoPatrolController, _idleBehaviourSwitchTime);

        #endregion

        #region AgentCharacter

        _agentCharacterView = new CharacterView(_agentCharacterAnimator, _agentCharacter, _agentCharacter, this);
        HealthMediator agentCharacterHealthMediator = new HealthMediator(_agentCharacterView);

        _agentCharacter.Initialize(agentCharacterHealthMediator);

        ClickToMoveController agentPlayerMoveController = new ClickToMoveController(_clickHandler, _agentCharacter, _agentCharacter, _agentCharacter, queryFilter);

        Pointer agentPointer = Instantiate(_pointerPrefab);
        agentPointer.Initialize(agentPlayerMoveController);

        _agentCharacterJumpView = new AgentCharacterJumpView(_agentCharacterAnimator, _agentCharacter);

        _agentCharacterController = new CompositeController(agentPlayerMoveController,
                                                       new AlongMovableVelocityRotatableController(_agentCharacter, _agentCharacter));
        _agentCharacterController.Enable();

        _agentCharacterhealthBarView?.Initialize(_agentCharacter);

        #endregion

        #region EnemyCharacter

        _enemyCharacterView = new CharacterView(_enemyCharacterAnimator, _enemyCharacter, _enemyCharacter, this);
        HealthMediator enemyCharacterHealthMediator = new HealthMediator(_enemyCharacterView);

        _enemyCharacter.Initialize(enemyCharacterHealthMediator);

        _enemyCharacterController = new CompositeController(new DirectionalMovableAgroController(_enemyCharacter, _character.transform, 10f, 2f, queryFilter, 1f),
                                                            new AlongMovableVelocityRotatableController(_enemyCharacter, _enemyCharacter));
        _enemyCharacterController.Enable();

        #endregion

        #region AgentEnemyCharacter

        _agentEnemyCharacterView = new CharacterView(_agentEnemyCharacterAnimator, _agentEnemyCharacter, _agentEnemyCharacter, this);
        HealthMediator agentEnemyCharacterHealthMediator = new HealthMediator(_agentEnemyCharacterView);

        _agentEnemyCharacter.Initialize(agentEnemyCharacterHealthMediator);

        _agentEnemyCharacterJumpView = new AgentCharacterJumpView(_agentEnemyCharacterAnimator, _agentEnemyCharacter);

        //_agentEnemyCharacterController = new AgentCharacterAgroController(_agentEnemyCharacter, _character.transform, 100, 2, 1);
        _agentEnemyCharacterController = new AgentCharacterAgroController(_agentEnemyCharacter, _agentCharacter.transform, 100, 2, 1);

        _agentEnemyCharacterController.Enable();

        _agentEnemyHealthBarView?.Initialize(_agentEnemyCharacter);

        #endregion

        _path = new NavMeshPath();
    }

    private void Start()
    {
        //_character.gameObject.SetActive(false);
        _enemyCharacter.gameObject.SetActive(false);
    }

    private void Update()
    {
        _clickHandler.UpdateInput();

        //_characterController.Update(Time.deltaTime);

        _agentCharacterController.Update(Time.deltaTime);

        _enemyCharacterController.Update(Time.deltaTime);

        _agentEnemyCharacterController.Update(Time.deltaTime);

        _movementHandler.Update(Time.deltaTime);

        //_characterView.Update(Time.deltaTime);

        _agentCharacterView.Update(Time.deltaTime);
        _agentCharacterJumpView.Update(Time.deltaTime);

        _enemyCharacterView.Update(Time.deltaTime);

        _agentEnemyCharacterView.Update(Time.deltaTime);
        _agentEnemyCharacterJumpView.Update(Time.deltaTime);
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