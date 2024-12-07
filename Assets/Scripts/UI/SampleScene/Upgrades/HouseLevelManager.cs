using UnityEngine;

namespace UI.SampleScene.Upgrades
{
    public class HouseLevelManager : MonoBehaviour
    {
        private int _houseLevel = 1;

        public enum SceneNames
        {
            SampleScene,
            HomeSceneMini,
            HomeSceneMiddle,
            HomeSceneMax
        }
        
        public void UpdateHouseLevel()
        {
            if (_houseLevel < 3) _houseLevel++;
        }
        
        public SceneNames GetSceneForHouseLevel()
        {
            if (_houseLevel == 1)
                return SceneNames.HomeSceneMini;
            if(_houseLevel == 2)
                return SceneNames.HomeSceneMiddle;
            if (_houseLevel == 3)
                return SceneNames.HomeSceneMax;
            return SceneNames.HomeSceneMini;
        }
    }
}