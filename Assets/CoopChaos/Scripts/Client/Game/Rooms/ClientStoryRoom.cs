using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace CoopChaos
{
    public class ClientStoryRoom : ClientInteractableObjectBase
    {
        [SerializeField] private GameObject highlight;
        
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private Button button;

        [SerializeField] private GameObject menu;

        private StoryRoomState state;
        private bool highlighted = false;

        public override void Highlight()
        {
            highlighted = true;
            highlight.SetActive(true);
        }

        public override void Unhighlight()
        {
            highlighted = false;
            highlight.SetActive(false);
            menu.SetActive(false);
        }

        public override void OnNetworkSpawn()
        {
            if (!IsClient)
            {
                enabled = false;
                return;
            }
        }

        private void Start()
        {
            state = GetComponent<StoryRoomState>();
            
            state.Description.OnValueChanged += (_, v) =>
            {
                description.text = v;
            };
            
            state.Title.OnValueChanged += (_, v) =>
            {
                title.text = v;
            };
            
            state.CanTriggerNext.OnValueChanged += (_, v) =>
            {
                if (v)
                {
                    title.text = "Hyperantrieb";
                    description.text =
                        "Hier gibt es nichts mehr zu sehen. Unser Hyperantrieb ist aufgeladen und wir können weiter";
                }
                
                button.interactable = v;
            };

            state.InteractEvent += clientId =>
            {
                if (clientId == NetworkManager.Singleton.LocalClientId && highlighted)
                    menu.SetActive(!menu.activeSelf);
            };

            description.text = state.Description.Value;
            title.text = state.Title.Value;
            
            button.interactable = state.CanTriggerNext.Value;
            button.onClick.AddListener(() => state.TriggerNextFlightSequenceServerRpc());
        }
    }
}