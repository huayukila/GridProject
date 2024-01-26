using UnityEngine;

namespace Framework.BuildProject
{
    public class CenterCore : BuildingBase
    {
        private void Start()
        {
            this.RegisterEvent<GameClearEvent>(e =>
            {
                winParticle.Play();
            }).UnregisterWhenGameObjectDestroyed(gameObject);
        }

        public ParticleSystem explosionParticle;
        public ParticleSystem winParticle;
        protected override void OnDead()
        {
            explosionParticle.Play();
            this.SendEvent<GameOverEvent>();
        }
    }
}