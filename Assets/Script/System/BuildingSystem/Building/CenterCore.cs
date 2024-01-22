using UnityEngine;

namespace Framework.BuildProject
{
    public class CenterCore : BuildingBase
    {
        public ParticleSystem particle;

        protected override void OnDead()
        {
            particle.Play();
            this.SendEvent<GameOverEvent>();
        }
    }
}