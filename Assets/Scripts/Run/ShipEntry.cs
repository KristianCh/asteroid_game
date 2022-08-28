// Author: Kristián Chovančák
// Created: 28.08.2022
// Copyright: (c) Noxgames
// http://www.noxgames.com/

namespace Run
{
    [System.Serializable]
    public class ShipEntry
    {
        public string Type = "";
        public string Name = "";
        public float HealthPercentage = 100;
        public int Level = 1;
        public int SubLevel = 0;
        public bool IsFlagShip = false;

        public ShipEntry(string type, float healthPercentage, int level, int subLevel, bool isFlagship)
        {
            Type = type;
            HealthPercentage = healthPercentage;
            Level = level;
            SubLevel = subLevel;
            IsFlagShip = isFlagship;
        }
    }
}