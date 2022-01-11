using UnityEngine;

namespace Yame
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
