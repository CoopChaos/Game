using UnityEngine;

namespace CoopChaos
{
    // behaviour of user over the whole connection
    public class ServerUserPersistentBehaviour : MonoBehaviour
    {
        public ServerUserModel UserModel { get; set; }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
