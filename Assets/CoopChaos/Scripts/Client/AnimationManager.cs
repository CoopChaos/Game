using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoopChaos
{
    public class AnimationManager : MonoBehaviour
    {
        public IEnumerator Shake()
        {
            float elapsed = 0.0f;
            float duration = 0.5f;
            
            Vector3 originalPos = transform.position;

            while (elapsed < duration)
            {
                float x = Random.Range(-0.5f, 0.5f) * 0.5f;
                float y = Random.Range(-0.5f, 0.5f) * 0.5f;

                transform.position = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z);

                elapsed += Time.deltaTime;

                yield return null;
            }
        }
    }
}
