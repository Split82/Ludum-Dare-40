using System;
using UnityEngine;

[Serializable]
public class FloatReference {
    
    [SerializeField] bool _useConstant = true;
    [SerializeField] float _constantValue;
    [SerializeField] FloatVariable _variable;

    public FloatReference() {}

    public FloatReference(float value) {

        _useConstant = true;
        _constantValue = value;
    }

    public float Value {

        get { return _useConstant ? _constantValue : _variable.value; }
    }

    public static implicit operator float(FloatReference reference) {

        return reference.Value;
    }
}
