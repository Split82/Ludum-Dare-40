using UnityEngine;


[CreateAssetMenu(fileName = "FloatVariable", menuName = "HMLib/FloatVariable")]
public class FloatVariable : ScriptableObject {

    [SerializeField] float _defaultValue;
    public float value;

    private void OnEnable() {

        value = _defaultValue;
    }
}
