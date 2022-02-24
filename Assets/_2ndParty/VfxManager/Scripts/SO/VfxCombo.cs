using System.Collections.Generic;
using UnityEngine;

namespace VfxManager
{

    [CreateAssetMenu(fileName = "New Vfx Combo", menuName = "VfxManager/Vfx Combo")]
    public class VfxCombo : ScriptableObject
    {
        public string _name;
        public Sequence _sequence = Sequence.Sequential;
        public bool _isRepeated = false;
        public List<VfxElement> list;
    }

}