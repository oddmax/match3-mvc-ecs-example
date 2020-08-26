using Data;
using Features.Config;
using UnityEngine;

namespace Features.Pieces.View
{
    public class ParticleEffectView : MonoBehaviour
    {
        public ParticleSystem ParticleSystem;
        public float Time;

        public void Show(Sprite gemSprite)
        {
            var textureSheetAnimation = ParticleSystem.textureSheetAnimation;
            if(gemSprite != null)
                textureSheetAnimation.SetSprite(0, gemSprite);
            ParticleSystem.Play();
            Destroy(gameObject, Time);
        }
    }
}