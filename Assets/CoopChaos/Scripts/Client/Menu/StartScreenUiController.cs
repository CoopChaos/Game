using UnityEngine;
using UnityEngine.UI;

namespace CoopChaos.Menu
{
    public class StartScreenUiController : MonoBehaviour
    {
        [SerializeField] private Text roleText;

        public void Start()
        {
            Debug.Log("StartScreenStage");

            roleText.text = GetComponent<GameStageUserState>().Role.Value.ToString();
        }
    }
}


