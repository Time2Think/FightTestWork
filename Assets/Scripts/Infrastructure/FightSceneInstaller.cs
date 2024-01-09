using UI;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class FightSceneInstaller : MonoInstaller
    {
        [SerializeField]
        private Ability _ability;
        public override void InstallBindings()
        {
            Container.Bind<Ability>().FromInstance(_ability).AsSingle().NonLazy();
            Container.Bind<PlayerInputAction>().AsSingle().NonLazy();
            Container.Bind<Cooldown>().AsSingle().NonLazy();
        }
    }
}
