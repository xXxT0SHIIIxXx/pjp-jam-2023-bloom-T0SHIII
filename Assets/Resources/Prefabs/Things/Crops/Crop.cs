using UnityEngine;

public class Crop : MonoBehaviour
{
    public CropId Id;
    public float GrowthRateMultiplier = 1;

    [SerializeField]
    private GameObject _seedlingModel;

    [SerializeField]
    private GameObject _fullyGrownModel;

    [SerializeField]
    private float _baseGrowthRateSeconds = 5;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float _requiredWaterLevel = 0.25f;
    private float _maxWaterLevel = 1.0f;

    [SerializeField]
    // how fast the crop takes damage when water level is too low
    private float _droughtDamageRateSeconds = 7;

    [SerializeField]
    // how much damage not having enough water does
    private float _droughtDamage = 2;

    private float _health;

    // range of 0.0f - 1.0f
    private float _growth;
    private float _maxGrowth = 1.0f;
    public float GrowthPercentage => _growth;
    private float _sinceGrowTime;
    private float _growthAmount = 0.035f;

    // range of 0.0f - 1.0f
    private float _waterLevel;
    public float WaterPercentage => _waterLevel;
    private float _droughtDamageTime;

    void Awake()
    {
        // So the crop doesn't start taking damage instantly.
        _waterLevel = _requiredWaterLevel + 0.05f;
    }

    void FixedUpdate()
    {
        if (_growth >= _maxGrowth)
        {
            _seedlingModel.SetActive(false);
            _fullyGrownModel.SetActive(true);
            return;
        }

        if (_growth >= 0.25f)
        {
            _seedlingModel.SetActive(true);
        }
        else
            _seedlingModel.SetActive(false);

        if (_waterLevel < _requiredWaterLevel && Time.time - _droughtDamageTime >= _droughtDamageRateSeconds)
        {
            TakeDamage(_droughtDamage);
            _droughtDamageTime = Time.time;
        }

        var growthRate = _baseGrowthRateSeconds * GrowthRateMultiplier;
        if (Time.time - _sinceGrowTime >= growthRate)
        {
            _growth += _growthAmount;
            _sinceGrowTime = Time.time;

            Debug.Log($"{Id.ToString()} Growth value: {_growth}");
        }
    }

    private void Die()
    {

    }

    public void TakeDamage(float amount)
    {
        _health -= amount;
        Debug.Log($"Took damage :{amount}, health is {_health}");
        if (_health <= 0)
        {
            Die();
        }
    }

    public void Water(float amount)
    {
        _waterLevel += amount;
        if (_waterLevel > _maxWaterLevel)
            _waterLevel = _maxWaterLevel;
    }
}