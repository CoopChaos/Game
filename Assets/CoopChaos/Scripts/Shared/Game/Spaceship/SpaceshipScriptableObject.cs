using UnityEngine;

namespace CoopChaos.Yame.Scripts.Shared.Game
{
    [CreateAssetMenu(fileName = "Spaceship", menuName = "Yame/Game/Spaceship", order = 0)]
    public class SpaceshipScriptableObject : ScriptableObject
    {
        public string Name;
        public GameObject Prefab;
    }
}
