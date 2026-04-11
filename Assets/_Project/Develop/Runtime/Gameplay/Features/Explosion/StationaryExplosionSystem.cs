using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Gameplay.Features.TeamsFeature;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;


namespace Assets._Project.Develop.Runtime.Gameplay.Features.Explosion
{
	public class StationaryExplosionSystem : IInitializableSystem, IUpdatableSystem
	{
		private ReactiveVariable<float> _explosionDamage;
		private ReactiveVariable<float> _blastRadius;
		private Transform               _transform;
		private ReactiveVariable<bool>  _markedForDeath;
		private Entity                  _entity;

		private readonly CollidersRegistryService _collidersRegistryService;

		private Collider[] _collidersWithinRadius;

		public StationaryExplosionSystem (CollidersRegistryService collidersRegistryService)
		{
			_collidersRegistryService = collidersRegistryService;
		}


		public void OnInit (Entity entity)
		{
			_explosionDamage = entity.ExplosionDamage;
			_blastRadius     = entity.BlastRadius;
			_transform       = entity.Transform;
			_markedForDeath  = entity.MarkedForDeath;
			_entity          = entity;
		}

		public void OnUpdate (float deltaTime)
		{
			Physics.OverlapSphereNonAlloc(
				_transform.position,
				_blastRadius.Value,
				_collidersWithinRadius,
				LayerMask.GetMask("Characters"));

			if (_collidersWithinRadius.Length > 0)
			{
				foreach (Collider hitCollider in _collidersWithinRadius)
				{
					Entity entity = _collidersRegistryService.GetBy(hitCollider);

					if (EntitiesHelper.TryTakeDamageFrom(_entity, entity, _explosionDamage.Value))
					{
						_markedForDeath.Value = true;
					}
				}
			}
			}
		}
}
