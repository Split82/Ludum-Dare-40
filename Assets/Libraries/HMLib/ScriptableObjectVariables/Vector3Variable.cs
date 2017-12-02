using UnityEngine;


[CreateAssetMenu(fileName = "Vector3Variable", menuName = "HMLib/Vector3Variable")]
public class Vector3Variable : ScriptableObject {

    [SerializeField] Vector3 _defaultValue;
    public Vector3 value;

    private void OnEnable() {

        value = _defaultValue;
    }
}
