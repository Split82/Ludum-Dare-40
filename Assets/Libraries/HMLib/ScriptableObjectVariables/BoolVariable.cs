using UnityEngine;


[CreateAssetMenu(fileName = "BoolVariable", menuName = "HMLib/BoolVariable")]
public class BoolVariable : ScriptableObject {

    [SerializeField] bool _defaultValue;
    public bool value;

    private void OnEnable() {

        value = _defaultValue;
    }
}
