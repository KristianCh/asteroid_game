using System.Collections;
using System.Collections.Generic;
using Data;
using Modules;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Run
{
    [CreateAssetMenu(fileName="RunDataDefinition", menuName="Run Data Definition")]
    public class RunData : ScriptableObject
    {
        [SerializeField]
        private List<ShipEntry> _Ships = new List<ShipEntry>();
        public  List<ShipEntry> Ships => _Ships;
        
        [SerializeField]
        private List<Module> _Modules = new List<Module>();
        public  List<Module> Modules => _Modules;

        [SerializeField]
        private int _Stage = 1;
        public int Stage
        {
            get => _Stage;
            set => _Stage = value;
        }

        [SerializeField]
        private int _Credits = 0;
        public  int Credits
        {
            get => _Credits;
            set => _Credits = value;
        }

        [SerializeField]
        private int _FleetLimit => 3 + _Stage / 3;
        public  int FleetLimit => _FleetLimit;

        [SerializeField]
        private DifficultySettingsData _Difficulty;
        public  DifficultySettingsData Difficulty
        {
            get => _Difficulty;
            set => _Difficulty = value;
        }
    }
}