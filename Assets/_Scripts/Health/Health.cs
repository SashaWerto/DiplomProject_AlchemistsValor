using UnityEngine;
using UnityEngine.UI;
public class Health : MonoBehaviour, IDamageable
{
    [Header("References")]
    [SerializeField] private GameObject _gfx;
    [SerializeField] private Image _healthBar;
    [SerializeField] private Image _stunBar;
    [Header("Preferences")]
    [SerializeField] private float _health;
    [SerializeField] private float _maxStun;
    private bool _dead;
    private float _currentStun;
    private bool _onStun;
    private float _maxHealth;
    private float _stunSubstractMultiplier = 0.1f;
    public float CurrentHealth { get => _health; set => _health = value; }
    public float MaxHealth { get => _maxHealth; set => _maxHealth = value; }
    public float CurrentStun { get => _currentStun; set => _currentStun = value; }
    public float MaxStun { get => _maxStun; set => _maxStun = value; }
    public bool OnStun { get => _onStun; set => _onStun = value; }
    public bool Dead { get => _dead; set => _dead = value; }
    void Start()
    {
        _maxHealth = _health;
        RefreshHealthUI();
    }
    private void Update()
    {
        if (_onStun)
        {
            _stunSubstractMultiplier = 1f;
            if(_currentStun <= 0f) _onStun = false;
        }
        else _stunSubstractMultiplier = 0.1f;
        _currentStun = Mathf.Clamp(_currentStun - _stunSubstractMultiplier * Time.deltaTime, 0, _maxStun);
        RefreshStunUI();
    }
    public void AddStun(float stun)
    {
        if(!_onStun) _currentStun += stun;
        if (_currentStun >= _maxStun)
        {
            _onStun = true;
        }        
    }
    public void RefreshHealthUI()
    {
        if(_healthBar) _healthBar.fillAmount = _health / _maxHealth;
        if(_gfx)
        {
            if (_health <= 0f) _gfx.SetActive(false);
            else _gfx.SetActive(true);
        }       
    }
    public void RefreshStunUI()
    {
        if(_stunBar) _stunBar.fillAmount = _currentStun / _maxStun;
    }

    public void GetDamage(float damage, float stun = 0)
    {
        CurrentHealth -= damage;
        AddStun(stun);
        RefreshHealthUI();
        if (CurrentHealth <= 0f)
        {
            _dead = true;
        }
        if(TryGetComponent<BaseAI>(out var enemyHealth))
        {
            enemyHealth.GetDamage();
        }
    }
    public bool CheckState()
    {
        return _dead;
    }
}
