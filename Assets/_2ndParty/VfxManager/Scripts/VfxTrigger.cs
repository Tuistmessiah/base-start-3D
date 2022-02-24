using System.Collections;
using System.Collections.Generic;
using System;
// using System.Threading.Tasks;
using UnityEngine;

namespace VfxManager {
    public enum Sequence { Sequential, Parallel }

    /** Main script that stores all instances of Vfxs that it creates and plays them.
     * This script can either play the vfxs it has referenced in its inspector or run a foreign SO or prefab with vfx
     * @obs Types of SO used are "VfxCombo" and "VfxElement".
     * @state _activeVfxs Dictionary with all active vfxs, saved with a reference "int".
     * @state _internalId Integer value to change key used each time an instance of a vfx is created.
     * @state _vfxs Gameobject references to prefabs with Particle Sytems (vfxs) that can be played by name.
     * @state _vfxCombos SO collection of available vfxs.
     * @method TriggerCombo Calls "TriggerVfxs(IEnumerator)" has a normal function
     * @method TriggerVfxs(IEnumerator) Trigger a combo from the list of local SObjects (uses SO, by name or by reference, see overloads)
     * @method TriggerVfx
     * @method SpawnVfx(IEnumerator)
     */
    public class VfxTrigger : MonoBehaviour {
        public static VfxTrigger instance;
        static public Dictionary<int, GameObject> _activeVfxs = new Dictionary<int, GameObject>();
        static int _internalId = 0;
        [SerializeField] private GameObject[] _vfxsEditorSetup;
        [SerializeField] private VfxCombo[] _vfxCombosEditorSetup;
        [SerializeField] static protected GameObject[] _vfxs;
        [SerializeField] static protected VfxCombo[] _vfxCombos;

        // * Exposed
        // TODO: Add vfxs optionally with a composed key that can be provided through a third party, in case the vfx needs to be changed some how while it plays.
        // TODO: Add a method in Manager to preview combo in a default deactivated floor at 0,0,0 or special prefab
        // TODO: Add same amount of examples in "Test.cs" for "Parallel" cases (maybe middle click)
        // TODO: Add to SO case the ability to: "rotate over time", "increase size over time", "use particle system's time" for VfxElement
        // TODO: (Maybe) Transform into async await (using System.Threading.Tasksm async void, await Task.Yield())
        // TODO: Use object pooling to handle instances

        void Awake() { //called when an instance awakes in the game
            if(!instance) instance = this; //set our static reference to our newly initialized instance
            _vfxs = _vfxsEditorSetup;
            _vfxCombos = _vfxCombosEditorSetup;
        }

        static public void TriggerCombo(string _comboName, Transform _target, Vector3 _position) {
            instance.StartCoroutine(TriggerVfxs(_comboName, _target, _position));
        }

        static public void TriggerCombo(string _comboName, Transform _target, Vector3 _position, Vector3 _normal) {
            instance.StartCoroutine(TriggerVfxs(_comboName, _target, _position, _normal));
        }

        static public void TriggerCombo(string _comboName, Transform _target) {
            instance.StartCoroutine(TriggerVfxs(_comboName, _target));
        }

        static public void TriggerCombo(string[] _names, Sequence _sequence, Transform _target, Vector3[] _dirs, float[] _sizes, float[] _durations) {
            instance.StartCoroutine(instance.TriggerVfxs(_names, _sequence, _target, _dirs, _sizes, _durations));
        }

        static public void TriggerCombo(GameObject[] _prefabs, Sequence _sequence, Transform _target, Vector3[] _dirs, float[] _sizes, float[] _durations) {
            instance.StartCoroutine(instance.TriggerVfxs(_prefabs, _sequence, _target, _dirs, _sizes, _durations));
        }

        /** Trigger a combo from the list of local SObjects */
        // TODO: Add an overload that receives a VfxCombo
        static public IEnumerator TriggerVfxs(string _comboName, Transform _target, Vector3? _positionOpt = null, Vector3? _normalOpt = null) {
            VfxCombo vfxCombo = Array.Find(_vfxCombos, (x) => x.name == _comboName);
            if (!vfxCombo) yield break;

            // Sequential
            if (vfxCombo._sequence == Sequence.Sequential) {
                foreach (VfxElement vfx in vfxCombo.list) {
                    if (vfx._prefab) {
                        if(_positionOpt != null || _normalOpt != null)  {
                            Vector3 position = _positionOpt ?? _target.position;
                            Vector3 normal = _normalOpt ?? Vector3.up;
                            yield return instance.StartCoroutine(instance.SpawnVfx(vfx, position, _target, normal));
                        }
                        else {
                            yield return instance.StartCoroutine(instance.SpawnVfx(vfx, _target));
                        }
                    }
                    else if (vfx._prefabName != null) {
                        GameObject _vfxPrefab = Array.Find(_vfxs, (x) => x.name == vfx._prefabName);
                        if (!_vfxPrefab) yield return null;
                        yield return instance.StartCoroutine(instance.SpawnVfx(_vfxPrefab, vfx, _target));
                    }
                    else yield return null;
                }
            }
            // Parallel
            else if (vfxCombo._sequence == Sequence.Parallel) {
                foreach (VfxElement vfx in vfxCombo.list) {
                    if (vfx._prefab) yield return instance.SpawnVfx(vfx._prefab, _target, vfx._yDirection, vfx._sizeRadius, vfx._time);
                    else if (vfx._prefabName != null) yield return instance.SpawnVfx(vfx._prefab, _target, vfx._yDirection, vfx._sizeRadius, vfx._time);
                    else yield return null;
                }
            }
        }

        #region Prefab Vfxs

        /** Trigger a combo from the list of local prefabs (by name) */
        public IEnumerator TriggerVfxs(string[] _names, Sequence _sequence, Transform _target, Vector3[] _dirs, float[] _sizes, float[] _durations) {
            for (int i = 0; i < _names.Length && i < _dirs.Length && i < _sizes.Length && i < _durations.Length; i++) {
                GameObject foundPrefab = Array.Find(_vfxs, (x) => x.transform.name == _names[i]);
                if (!foundPrefab) {
                    if (_sequence == Sequence.Sequential) yield return new WaitForSeconds(_durations[i]);
                    else if (_sequence == Sequence.Parallel) yield return null;
                } else {
                    if (_sequence == Sequence.Sequential) yield return StartCoroutine(SpawnVfx(foundPrefab.gameObject, _target, _dirs[i], _sizes[i], _durations[i]));
                    else if (_sequence == Sequence.Parallel) StartCoroutine(SpawnVfx(foundPrefab.gameObject, _target, _dirs[i], _sizes[i], _durations[i]));
                }
            }
        }

        /** Trigger a combo from given prefabs */
        public IEnumerator TriggerVfxs(GameObject[] _prefabs, Sequence _sequence, Transform _target, Vector3[] _dirs, float[] _sizes, float[] _durations) {
            for (int i = 0; i < _prefabs.Length && i < _dirs.Length && i < _sizes.Length && i < _durations.Length; i++) {
                GameObject prefab = _prefabs[i];
                if (_sequence == Sequence.Sequential) yield return StartCoroutine(SpawnVfx(prefab.gameObject, _target, _dirs[i], _sizes[i], _durations[i]));
                else if (_sequence == Sequence.Parallel) StartCoroutine(SpawnVfx(prefab.gameObject, _target, _dirs[i], _sizes[i], _durations[i]));
            }
        }

        #endregion

        #region One Vfx

        /** Trigger a VFX 
        * @param "_target" Will follow targets transform
        * @param "_point" Of impact/position
        * @param "_dir" z-axis of orientation normal
        */
        // > Using local Prefabs

        /** Trigger one vfx (find prefab in list, by name) */
        static public void TriggerVfx(string _name, Transform _target, Vector3 _dir, float _size, float _time) {
            GameObject _vfxPrefab = Array.Find(_vfxs, (x) => x.name == _name);
            if (!_vfxPrefab) return;
            instance.StartCoroutine(instance.SpawnVfx(_vfxPrefab, _target, _dir, _size, _time));
        }

        /** Trigger one vfx (by prefab) */
        static public void TriggerVfx(GameObject _vfxPrefab, Transform _target, Vector3 _dir, float _size, float _time) {
            instance.StartCoroutine(instance.SpawnVfx(_vfxPrefab, _target, _dir, _size, _time));
        }

        // > Using VfxElement SO (Overloads)

        /** Overload (local space) */
        static public void TriggerVfx(VfxElement _vfx, Transform _target) {
            instance.StartCoroutine(instance.SpawnVfx(_vfx, _target));
        }

        /** Overload (local space) */
        static public void TriggerVfx(GameObject _vfxPrefab, VfxElement _vfx, Transform _target) {
            instance.StartCoroutine(instance.SpawnVfx(_vfxPrefab, _vfx, _target));
        }

        /** Overload (local space) */
        static public void TriggerVfx(VfxElement _vfx, Transform _target, Quaternion _offsetRotation, Vector3 _offsetPosition) {
            instance.StartCoroutine(instance.SpawnVfx(_vfx, _target, _offsetRotation, _offsetPosition));
        }

        /** Overload (local space) */
        static public void TriggerVfx(GameObject _vfxPrefab, VfxElement _element, Transform _target, Quaternion _offsetRotation, Vector3 _offsetPosition)
        {
            instance.StartCoroutine(instance.SpawnVfx(_vfxPrefab, _element, _target, _offsetRotation, _offsetPosition));
        }

        /** Local Space */
        static public void TriggerVfx(GameObject _vfxPrefab, VfxElement _element, Transform _target, Vector3 _dir, Vector3 _up) {
            instance.StartCoroutine(instance.SpawnVfx(_vfxPrefab, _element, _target, _dir, _up));
        }

        /** Overload (local space) */
        static public void TriggerVfx(GameObject _vfxPrefab, VfxElement _element, Transform _target, Vector3 _dir) {
            TriggerVfx(_vfxPrefab, _element, _target, _dir, Vector3.up);
        }

        /** Overload (local space) */
        static public void TriggerVfx(GameObject _vfxPrefab, VfxElement _element, Vector3 _up, Transform _target) {
            TriggerVfx(_vfxPrefab, _element, _target, _target.forward, _up);
        }

        #endregion

        // * Internal

        // > Using Local Prefabs

        /** Apply simple/manual */
        IEnumerator 
        
        SpawnVfx(GameObject _vfxPrefab, Transform _target, Vector3 _dir, float _size, float _time) {
            GameObject vfx = Instantiate(_vfxPrefab);

            Vector3 groundCross = Vector3.Cross(_dir, Vector3.up).normalized;
            Vector3 slopeVector = Vector3.Cross(groundCross, _dir).normalized;

            vfx.transform.position = _target.transform.position;
            vfx.transform.rotation = Quaternion.LookRotation(_dir, slopeVector);
            vfx.transform.localScale = new Vector3(_size, _size, _size);
            vfx.transform.SetParent(_target);

            vfx.SetActive(true);
            vfx.GetComponent<ParticleSystem>().Play();
            yield return instance.StartCoroutine(ActivateAndDestroy(vfx, _time));
        }

        // > Using VfxElement SO
        /** Apply local offset (overload) */
        IEnumerator SpawnVfx(VfxElement _element, Vector3 _position, Transform _target, Vector3 _normal){
            yield return instance.StartCoroutine(SpawnVfx(null, _element, _position, _target, _normal));
        }

        /** Apply local offset (overload) */
        IEnumerator SpawnVfx(VfxElement _element, Transform _target) {
            yield return instance.StartCoroutine(SpawnVfx(_element._prefab, _element, _target, Quaternion.identity, Vector3.zero));
        }

        /** Apply local offset (overload) */
        IEnumerator SpawnVfx(GameObject _vfxPrefab, VfxElement _element, Transform _target) {
            yield return instance.StartCoroutine(SpawnVfx(_vfxPrefab, _element, _target, Quaternion.identity, Vector3.zero));
        }

        /** Apply local offset (overload) */
        IEnumerator SpawnVfx(VfxElement _element, Transform _target, Quaternion _offsetRotation, Vector3 _offsetPosition) {
            yield return instance.StartCoroutine(SpawnVfx(_element._prefab, _element, _target, _offsetRotation, _offsetPosition));
        }

        /** Apply local offset ("_zDirection" and "_yDirection" will be ignored) */
        IEnumerator SpawnVfx(GameObject _vfxPrefab, VfxElement _element, Transform _target, Quaternion _offsetRotation, Vector3 _offsetPosition) {
            GameObject vfx = CreateInstance(_vfxPrefab, _element);
            if(!vfx) yield break;

            if(_element._isTargetBound) vfx.transform.SetParent(_target);
            else vfx.transform.SetParent(this.gameObject.transform);
            vfx.transform.position = _target.rotation * _offsetPosition + _target.position;
            vfx.transform.rotation = _target.rotation * _offsetRotation;
            vfx.transform.localScale = new Vector3(_element._sizeRadius, _element._sizeRadius, _element._sizeRadius);

            yield return instance.StartCoroutine(ActivateAndDestroy(vfx, _element._time));
        }

        /** Apply local offset (position can be updated with "_isTargetBound" in VfxElement) */
        IEnumerator SpawnVfx(GameObject _vfxPrefab, VfxElement _element, Transform _target, Vector3 _dir, Vector3 _up) {
            GameObject vfx = CreateInstance(_vfxPrefab, _element);
            if(!vfx) yield break;

            Vector3 dir = (_element._zDirection != Vector3.zero) ? _element._zDirection : _dir;
            Vector3 up = (_element._yDirection != Vector3.zero) ? _element._yDirection : _up;
            Quaternion rot = new Quaternion();
            rot.SetLookRotation(dir, up);

            if(_element._isTargetBound) vfx.transform.SetParent(_target);
            else vfx.transform.SetParent(this.gameObject.transform);
            vfx.transform.position = _target.position;
            vfx.transform.rotation = rot;
            vfx.transform.localScale = new Vector3(_element._sizeRadius, _element._sizeRadius, _element._sizeRadius);

            yield return "message";
            yield return instance.StartCoroutine(ActivateAndDestroy(vfx, _element._time));
        }

        /** Apply position independent of target and using normal */
        IEnumerator SpawnVfx(GameObject _vfxPrefab, VfxElement _element, Vector3 _position, Transform _target, Vector3 _normal) {
            GameObject vfx = CreateInstance(_vfxPrefab, _element);
            if(!vfx) yield break;

            Vector3 dir = (_element._zDirection != Vector3.zero) ? _element._zDirection : Vector3.forward;
            Vector3 up = (_element._yDirection != Vector3.zero) ? _element._yDirection : Vector3.up;
            Vector3 tangentSide = Vector3.Cross(_normal, Vector3.up).normalized;
            Vector3 tangentUp = Vector3.Cross(tangentSide, _normal).normalized;
            Quaternion rot = Quaternion.LookRotation(_normal, tangentUp);

            if(_element._isTargetBound) vfx.transform.SetParent(_target);
            else vfx.transform.SetParent(this.gameObject.transform);
            vfx.transform.position = _position;
            vfx.transform.rotation = rot;
            vfx.transform.localScale = new Vector3(_element._sizeRadius, _element._sizeRadius, _element._sizeRadius);

            yield return "message";
            yield return instance.StartCoroutine(ActivateAndDestroy(vfx, _element._time));
        }

        GameObject CreateInstance(GameObject _vfxPrefab, VfxElement _element) {
            GameObject vfx;
            if(!_vfxPrefab) {
                if(!_element._prefab) {
                    Debug.Log("WARNING: No prefab available at 'SpawnVfx' ");
                    return null;
                }
                vfx = Instantiate(_element._prefab);
            } else vfx = Instantiate(_vfxPrefab);
            _activeVfxs.Add(_internalId, vfx);


            _internalId++;
            return vfx;
        }

        IEnumerator ActivateAndDestroy(GameObject _vfx, float _time) {
            _vfx.SetActive(true);
            _vfx.GetComponent<ParticleSystem>().Play();
            yield return new WaitForSeconds(_time);
            _vfx.SetActive(false);
            Destroy(_vfx);
        }
    }

}
