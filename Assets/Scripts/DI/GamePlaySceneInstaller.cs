using Animals.Pets.Bee;
using Animals.Pets.CrawlingPets;
using Animals.Pets.Ghost;
using Animals.Pets.globalAnimControllers;
using Enviroment.Plants;
using Enviroment.Time;
using Player.ToolsLogic;
using UI.SampleScene;
using UI.SampleScene.Shop;
using UI.UIManager;
using UnityEngine;
using Zenject;

namespace DI
{
    public class GamePlaySceneInstaller : MonoInstaller
    {
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject statsPanel;
        [SerializeField] private GameObject pauseButton;
        [SerializeField] private GameObject inventoryPanel;
        [SerializeField] private GameObject upgradePanel;
        [SerializeField] private GameObject shopPanel;
        
        public override void InstallBindings()
        {
            //Pets bindings
            Container.Bind<PetAnimController>().FromComponentInHierarchy().AsSingle();
            Container.Bind<CrawlingPetAnimController>().FromComponentInHierarchy().AsSingle();
            Container.Bind<GhostPetAnimController>().FromComponentInHierarchy().AsSingle();
            Container.Bind<BeePetAnimController>().FromComponentInHierarchy().AsSingle();
            //Plants bindings
            Container.Bind<ToolSwitcher>().FromComponentInHierarchy().AsSingle();
            Container.Bind<Planting>().FromComponentInHierarchy().AsSingle();
            Container.Bind<WateringCanLogic>().FromComponentInHierarchy().AsSingle();
            Container.Bind<DayNightCycle>().FromComponentInHierarchy().AsSingle();
            //UI bindings
            Container.Bind<UIManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<SellingItemsLogic>().FromComponentInHierarchy().AsSingle();
            
            Container.Bind<UIElements>().FromMethod(() =>
                new UIElements(
                    pausePanel, 
                    settingsPanel, 
                    statsPanel, 
                    pauseButton, 
                    inventoryPanel, 
                    upgradePanel, 
                    shopPanel
                )).AsSingle();
        }
    }
}