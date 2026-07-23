using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Resource", menuName = "Scriptable Objects/Resource")]
public class Resource : ScriptableObject
{
    public event Action onValueChanged;

    private float minValue;
    private float maxValue;
    private float value;

    public float Min { get { return minValue; } }
    public float Max { get { return maxValue; } }
    public float Value { get { return value; } }

    public void Initialise(float start, float min, float max)
    {
        minValue = min;
        maxValue = max;
        value = Mathf.Clamp(start, minValue, maxValue);
        onValueChanged?.Invoke();
    }

    public float ChangeValue(float change)
    {
        float newValue = Mathf.Clamp(value + change, minValue, maxValue);
        if (newValue != value)
        {
            value = newValue;
            onValueChanged?.Invoke();
        }
        return value; //might want to return bool instead
    }

    public float SetValue(float set)
    {
        float newValue = Mathf.Clamp(set, minValue, maxValue);
        if (newValue != value)
        {
            value = newValue;
            onValueChanged?.Invoke();
        }
        return value; //might want to return bool instead
    }
}
