using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

namespace CoopChaos.CoopChaos.Scripts.Client.Menu
{
    public class SliderMarkers : MonoBehaviour
    {
        [SerializeField] private float normalFontSize;
        [SerializeField] private float selectedFontSize;

        [SerializeField] private Color normalColor;
        [SerializeField] private Color selectedColor;

        [SerializeField] private ElasticSlider slider;
        [SerializeField] private TextMeshProUGUI[] markers;

        private TextMeshProUGUI previousMarker;

        private void HandleSliderChanged(float value)
        {
            var index = Mathf.Clamp(Mathf.RoundToInt(value * (markers.Length - 1)), 0, markers.Length - 1);

            if (previousMarker != markers[index] && previousMarker != null)
            {
                previousMarker.fontSize = normalFontSize;
                previousMarker.fontStyle = FontStyles.Normal;
                previousMarker.color = normalColor;
            }

            markers[index].fontSize = selectedFontSize;
            markers[index].fontStyle = FontStyles.Bold;
            markers[index].color = selectedColor;

            previousMarker = markers[index];
        }

        private void Awake()
        {
            slider.onValueChanged.AddListener(HandleSliderChanged);
        }

        private void Start()
        {
            foreach (var marker in markers)
            {
                marker.fontSize = normalFontSize;
                marker.fontStyle = FontStyles.Normal;
                marker.color = normalColor;
            }
            HandleSliderChanged(slider.value);
        }
    }
}