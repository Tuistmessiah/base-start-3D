using UnityEngine;

namespace VfxManager
{

    [CreateAssetMenu(fileName = "New Vfx", menuName = "VfxManager/Vfx Single")]
    public class VfxElement : ScriptableObject
    {
        public string _name;
        public string _prefabName;
        public GameObject _prefab;
        public bool _isTargetBound = false;
        public Vector3 _zDirection = new Vector3(0, 0, 1);
        public Vector3 _yDirection = new Vector3(0, 1, 0);
        public float _sizeRadius = 1f;
        public float _time = 1f;
        public bool _isLoop = false;
    }

}