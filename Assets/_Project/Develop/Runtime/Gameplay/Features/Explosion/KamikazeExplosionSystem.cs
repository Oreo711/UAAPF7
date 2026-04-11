using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Gameplay.Features.ApplyDamage;
using Assets._Project.Develop.Runtime.Gameplay.Features.MainHero;
using Assets._Project.Develop.Runtime.Gameplay.Features.TeamsFeature;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;


namespace Assets._Project.Develop.Runtime.Gameplay.Features.Explosion
{
	public class KamikazeExplosionSystem : IInitializableSystem, IUpdatableSystem
	{
		private ReactiveVariable<float> _maxHealth;
		private ReactiveVariable<float> _explosionDamage;
		private ReactiveVariable<float> _blastRadius;
		private Transform               _transform;
		private ReactiveEvent<float>    _takeDamageRequest;
		private Entity                  _entity;

		private readonly MainHeroHolderService _mainHeroHolderService;
		private readonly CollidersRegistryService _collidersRegistryService;

		public KamikazeExplosionSystem(MainHeroHolderService mainHeroHolderService, CollidersRegistryService collidersRegistryService)
		{
			_mainHeroHolderService    = mainHeroHolderService;
			_collidersRegistryService = collidersRegistryService;
		}

		public void OnInit (Entity entity)
		{
			_maxHealth         = entity.MaxHealth;
			_explosionDamage   = entity.ExplosionDamage;
			_blastRadius       = entity.BlastRadius;
			_transform         = entity.Transform;
			_takeDamageRequest = entity.TakeDamageRequest;
			_entity            = entity;
		}
		public void OnUpdate (float deltaTime)
		{
			if ((_mainHeroHolderService.MainHero.Transform.position - _transform.position).magnitude < _blastRadius.Value)
			{
				Collider[] hitColliders = Physics.OverlapSphere(_transform.position, _blastRadius.Value, LayerMask.GetMask("Characters"));

				foreach (Collider hitCollider in hitColliders)
				{
					Entity entity = _collidersRegistryService.GetBy(hitCollider);

					EntitiesHelper.TryTakeDamageFrom(_entity, entity, _explosionDamage.Value);
				}

				_takeDamageRequest.Invoke(_maxHealth.Value);
			}
		}
	}
}
