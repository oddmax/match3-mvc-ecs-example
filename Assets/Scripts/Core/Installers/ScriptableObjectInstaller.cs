using Core.Config;
using Features.Config;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    /// <summary>
    /// Installer with SO dependencies (mostly Game Design configs)
    /// </summary>
    [CreateAssetMenu(fileName = "SOInstaller", menuName = "Installers/SOInstaller")]
    public class ScriptableObjectInstaller : ScriptableObjectInstaller<ScriptableObjectInstaller>
    {
        [Header("Art Assets configuration")] 
        public GameSceneCatalogue GameSceneCatalogue;
        public AssetsCatalogue AssetsCatalogue;

        [Header("Game data configuration")] 
        public GameConfig GameConfig;
        public PlayerConfig PlayerConfig;
    
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameSceneCatalogue>().FromInstance(GameSceneCatalogue).AsSingle();
            Container.BindInterfacesAndSelfTo<AssetsCatalogue>().FromInstance(AssetsCatalogue).AsSingle();
            Container.BindInterfacesAndSelfTo<GameConfig>().FromInstance(GameConfig).AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerConfig>().FromInstance(PlayerConfig).AsSingle();
        }
    }
}