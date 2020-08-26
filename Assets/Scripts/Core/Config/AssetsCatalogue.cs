using Features.Config;
using Features.Data;
using UnityEngine;

namespace Core.Config
{
    /// <summary>
    /// Catalogue of game art assets
    /// </summary>
    [CreateAssetMenu(fileName = "New AssetsCatalogue", menuName = "AssetCatalogue/AssetsCatalogue")]
    public class AssetsCatalogue : ScriptableObject
    {
        public GemSpriteConfig[] gemSpriteConfigs;

        public Sprite GetSpriteConfig(GemColor gemColor)
        {
            foreach (var spriteConfig in gemSpriteConfigs)
            {
                if (spriteConfig.Color == gemColor)
                    return spriteConfig.Sprite;
            }

            return null;
        }
    }
}