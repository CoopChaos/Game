using UnityEngine;

namespace CoopChaos.CoopChaos.Scripts.Shared.Game
{
    [CreateAssetMenu(fileName = "Spaceship", menuName = "CoopChaos/Game/Spaceship", order = 0)]
    public class SpaceshipScriptableObject : ScriptableObject
    {
        public string Name;
        public GameObject Prefab;
    }
}
