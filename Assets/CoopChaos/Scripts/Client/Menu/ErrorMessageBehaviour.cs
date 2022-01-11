using TMPro;
using UnityEngine;

namespace Yame
{
    public class ErrorMessageBehaviour : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI errorText;

        private int counter;
        
        public void SetErrorMessage(string message)
        {
            errorText.text = $"{++counter} {message}";
        }

        private void Start()
        {
            counter = 0;
        }
    }
}
