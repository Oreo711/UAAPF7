using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Gameplay.Features.ApplyDamage;
using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Gameplay.Features.TeamsFeature;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;


namespace Assets._Project.Develop.Runtime.Gameplay.Features.Explosion
{
	public class PointAndClickExplosionSystem : IInitializableSystem, IUpdatableSystem
	{
		private ReactiveVariable<float> _explosionDamage;
		private ReactiveVariable<float> _blastRadius;
		private Entity                  _entity;

		private readonly IInputService            _inputService;
		private readonly CollidersRegistryService _collidersRegistryService;

		public PointAndClickExplosionSystem (IInputService inputService, CollidersRegistryService collidersRegistryService)
		{
			_inputService                  = inputService;
			_collidersRegistryService = collidersRegistryService;
		}


		public void OnInit (Entity entity)
		{
			_explosionDamage = entity.ExplosionDamage;
			_blastRadius     = entity.BlastRadius;
			_entity          = entity;
		}

		public void OnUpdate (float deltaTime)
		{
			if (_inputService.Clicked)
			{
				Collider[] hitColliders = Physics.OverlapSphere(Camera.main.ScreenToWorldPoint(Input.mousePosition), _blastRadius.Value, LayerMask.GetMask("Characters"));

				foreach (Collider hitCollider in hitColliders)
				{
					Entity entity = _collidersRegistryService.GetBy(hitCollider);

					EntitiesHelper.TryTakeDamageFrom(_entity, entity, _explosionDamage.Value);
				}
			}
		}
	}
}
