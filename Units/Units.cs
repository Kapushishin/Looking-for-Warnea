using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;


public abstract class Units : MonoBehaviour
{
    #region Unit Stats
    // unit stats
    [SerializeField] string _name = "Unit";
    [SerializeField] float _damage = 2.0f;
    [SerializeField] float _maxHealth = 10f;
    [SerializeField] float _armor = 1.0f;
    [SerializeField] float _attackCD = 2f;
    #endregion

    #region Action Interfaces
    // actions interfaces
    public IMovement _movement;
    public ITargeted _targeted;
    public ITakeDamage _takedamage;
    public IDoDamage _dodamage;
    public IIngameUI _ingameUI;
    #endregion

    #region Misc Variables
    // unit mesh
    [SerializeField] Outline _meshOutline;
    // radius attack point
    [SerializeField] float _weaponRadius = 1.5f;
    // enemy layer
    [SerializeField] LayerMask _enemyLayer;

    // misc
    private NavMeshAgent _navMeshAgent;
    private TMP_Text _namefield;
    private int _unitLayer;
    private bool _isAiEnabled = false;
    private bool _selectedMesh = false;
    private int _damagableFound;
    private readonly Collider[] _weapon_colliders = new Collider[1];
    private bool canAttack = true;
    private bool _isAttacking = false;
    private int _playerLayer = 7;
    private float _currentHealth;
    #endregion

    private void OnEnable()
    {
        InitUnitActions();
        _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        _namefield = GameObject.FindGameObjectWithTag("NameField").GetComponent<TMP_Text>();
        _unitLayer = gameObject.layer;
        _currentHealth = _maxHealth;
    }

    public virtual void InitUnitActions()
    {
        _movement = new DefaultMovement();
        _targeted = new DefaultTargeted();
        _takedamage = new DefaultTakeDamage();
        _dodamage = new DefaultDoDamage();
        _ingameUI = GameObject.FindGameObjectWithTag("UIPanel").GetComponent<DefaultIngameUI>();
    }

    private void FixedUpdate()
    {
        if (_isAiEnabled)
        {
            _isAttacking = _movement.Patroling();
        }

        // в переменные записывается количество пересечений созданного коллайдера точки и объектов, с которыми возможно взаимодействие
        _damagableFound = Physics.OverlapSphereNonAlloc(gameObject.transform.position, _weaponRadius, _weapon_colliders, _enemyLayer);

        // если враг обнаружил и атакует игрока и нет кулдауна атаки, то вызывать метод атаки
        if (_isAttacking && canAttack)
        {
            StartCoroutine(DoDamage());
        }
    }

    #region Movement Part
    // ==============================
    //        Movement Part
    // ==============================
    public void MoveUnit(Vector3 position)
    {
        if (_navMeshAgent)
        {
            _isAiEnabled = false;
            _movement.SetDestination(_navMeshAgent, position);
        }
    }

    public void StartAI(Vector3 position)
    {
        if (_navMeshAgent)
        {
            _movement.StartAI(_navMeshAgent, _enemyLayer, gameObject, position);
            _isAiEnabled = true;
        }
    }
    #endregion

    #region Damage Part
    // ==============================
    //        Damage Part
    // ==============================
    public void TakeDamage(float damage)
    {
        _currentHealth = _takedamage.OnTakeDamage(_currentHealth, _armor, damage);
        if (_selectedMesh) _ingameUI.SetStatsPanel(_name, _currentHealth, _maxHealth, _damage, _armor);
    }

    private IEnumerator DoDamage()
    {
        if (_damagableFound > 0)
        {
            canAttack = false;
            Units _interactable = _weapon_colliders[0].GetComponent<Units>();
            _dodamage.OnDoDamage(_interactable, _damage);
            yield return new WaitForSeconds(_attackCD);
            canAttack = true;
        }
    }
    #endregion

    #region Selection Outline Part
    // ==============================
    //     Selection Outline Part
    // ==============================
    public void SelectUnit(bool isFriendly)
    {
        if (isFriendly)
        {
            _targeted.SetOutline(_meshOutline, true, Color.yellow, 5f);
            _ingameUI.SetStatsPanel(_name, _currentHealth, _maxHealth, _damage, _armor);
            gameObject.layer = _playerLayer;
        }
        else
        {
            _targeted.SetOutline(_meshOutline, true, Color.magenta, 5f);
        }
        _selectedMesh = true;
        _namefield.text = "";
    }

    public void DeselectUnit()
    {
        _targeted.SetOutline(_meshOutline, false, Color.white, 2f);
        _selectedMesh = false;
        gameObject.layer = _unitLayer;
    }
    #endregion

    #region Mouseover Outline Part
    // ==============================
    //     Mouseover Outline Part
    // ==============================
    public void OnMouseOver()
    {
        if (!_selectedMesh)
        {
            _targeted.SetOutline(_meshOutline, true, Color.white, 2f);
            _namefield.transform.position = new Vector3(Input.mousePosition.x + 100, Input.mousePosition.y, Input.mousePosition.z);
            _namefield.text = _name;
        }
    }

    private void OnMouseExit()
    {
        if (!_selectedMesh)
        {
            _targeted.SetOutline(_meshOutline, false, Color.white, 2f);
            _namefield.text = "";
        }
    }
    #endregion

    #region Misc Part
    // ==============================
    //          Misc Part
    // ==============================

    // отрисовывает радиус коллайдеров точек взаимодействия, просто для визуального удобства.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(gameObject.transform.position, _weaponRadius);
    }
    #endregion
}
