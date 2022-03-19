using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

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
                float x = Random.Range(-0.5f, 0.5f) * 3.0f;
                float y = Random.Range(-0.5f, 0.5f) * 3.0f;

                transform.position = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

                elapsed += Time.deltaTime;

                yield return null;
            }

            transform.position = originalPos;
        }

        public IEnumerator WarpDrive()
        {
            var p = FindObjectOfType<PostProcessVolume>();

            var lensDistortion = (LensDistortion) p.profile.settings.First(s => s is LensDistortion);
            var chromaticAberration = (ChromaticAberration) p.profile.settings.First(s => s is ChromaticAberration);

            const float duration = 2.0f;
            float targetHighTime = duration * 0.8f;
            
            float time = 0.0f;
            
            const float targetLensDistortion = -50.0f;
            const float targetChromaticAberration = 1.0f;
            
            while (time < duration)
            {
                time += Time.deltaTime;
                
                if (time < targetHighTime)
                {
                    lensDistortion.intensity.value = Mathf.Lerp(0.0f, targetLensDistortion, time / targetHighTime);
                    chromaticAberration.intensity.value = Mathf.Lerp(0.0f, targetChromaticAberration, time / targetHighTime);
                }
                else
                {
                    lensDistortion.intensity.value = Mathf.Lerp(targetLensDistortion, 0.0f, (time - targetHighTime) / (duration - targetHighTime));
                    chromaticAberration.intensity.value = Mathf.Lerp(targetChromaticAberration, 0.0f, (time - targetHighTime) / (duration - targetHighTime));
                }

                yield return null;
            }
            
            lensDistortion.intensity.value = 0.0f;
            chromaticAberration.intensity.value = 0.0f;
        }
        
        public IEnumerator Death()
        {
            var p = FindObjectOfType<PostProcessVolume>();

            var bloom = (Bloom) p.profile.settings.First(s => s is Bloom);
            
            const float duration = 2.0f;
            const float targetBloomIntensity = 100.0f;
            
            float time = 0.0f;
            
            while (time < duration)
            {
                time += Time.deltaTime;
                
                bloom.intensity.value = Mathf.Lerp(0.0f, targetBloomIntensity, time / duration);

                yield return null;
            }
            
            
        }
        
        public IEnumerator Damage()
        {
            var p = FindObjectOfType<PostProcessVolume>();

//            p.profile.settings.First(s => s is);
            
            yield return null;
        }
    }
}
