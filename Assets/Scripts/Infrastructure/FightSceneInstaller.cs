using UI;
using UnityEngine;
using Zenject;
using Zenject.SpaceFighter;

namespace Infrastructure
{
    public class FightSceneInstaller : MonoInstaller
    {
        [SerializeField]
        private BattleController _battleController;
        [SerializeField]
        private Player _player;
        [SerializeField]
        private Ability _ability;
        
        public override void InstallBindings()
        {
            Container.Bind<BattleController>().FromInstance(_battleController).AsSingle().NonLazy();
            Container.Bind<Player>().FromInstance(_player).AsSingle().NonLazy();
            Container.Bind<Ability>().FromInstance(_ability).AsSingle().NonLazy();
            
            Container.Bind<PlayerInputAction>().AsSingle().NonLazy();
            Container.Bind<Cooldown>().AsSingle().NonLazy();
        }
    }
}
