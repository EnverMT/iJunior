using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthBarUI : BaseHealthUI
{
    [SerializeField] private bool _isSmoothChange;
    [SerializeField, Range(0, 1f)] private float _speed;

    private Slider _slider;
    private Coroutine _coroutine;
    private float _currentValue;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    protected override void OnEnable()
    {
        if (_isSmoothChange)
            _currentValue = _healthAttribute.Value / _healthAttribute.MaxValue;

        base.OnEnable();
    }

    protected override void OnValueChanged(IAttribute<float> attribute)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        if (_isSmoothChange)
        {
            float target = attribute.Value / attribute.MaxValue;
            _coroutine = StartCoroutine(SmoothChange(target));
        }
        else
        {
            _slider.value = attribute.Value / attribute.MaxValue;
        }
    }

    private IEnumerator SmoothChange(float target)
    {
        while (Mathf.Approximately(_currentValue, target) == false)
        {
            _currentValue = Mathf.MoveTowards(_currentValue, target, _speed * Time.deltaTime);
            _slider.value = _currentValue;

            yield return null;
        }

        _currentValue = target;
    }
}