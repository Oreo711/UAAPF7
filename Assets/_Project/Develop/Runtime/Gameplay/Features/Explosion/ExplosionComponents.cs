using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;


namespace Assets._Project.Develop.Runtime.Gameplay.Features.Explosion
{
	public class ExplosionDamage : IEntityComponent
  	{
		  public ReactiveVariable<float> Value;
	  }

	public class BlastRadius : IEntityComponent
	{
		public ReactiveVariable<float> Value;
	}
}
