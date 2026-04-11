using _Project.Develop.Runtime.Meta.Features.Stats;
using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Gameplay.Infrastructure;
using Assets._Project.Develop.Runtime.Meta.Features.LevelsProgression;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilities.DataManagment.DataProviders;
using Assets._Project.Develop.Runtime.Utilities.SceneManagment;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.States
{
    public class WinState : EndGameState, IUpdatableState
    {
        private readonly LevelsProgressionService _levelsProgressionService;
        private readonly GameplayInputArgs        _gameplayInputArgs;
        private readonly PlayerDataProvider       _playerDataProvider;
        private readonly SceneSwitcherService     _sceneSwitcherService;
        private readonly ICoroutinesPerformer     _coroutinesPerformer;
        private readonly StatsService             _statsService;

        public WinState(
            IInputService inputService,
            StatsService statsService,
            LevelsProgressionService levelsProgressionService,
            GameplayInputArgs gameplayInputArgs,
            PlayerDataProvider playerDataProvider,
            SceneSwitcherService sceneSwitcherService,
            ICoroutinesPerformer coroutinesPerformer
        ) : base(inputService)
        {
            _levelsProgressionService = levelsProgressionService;
            _statsService             = statsService;
            _gameplayInputArgs        = gameplayInputArgs;
            _playerDataProvider       = playerDataProvider;
            _sceneSwitcherService     = sceneSwitcherService;
            _coroutinesPerformer      = coroutinesPerformer;
        }

        public override void Enter()
        {
            base.Enter();

            Debug.Log("ПОБЕДА!");

            _levelsProgressionService.AddLevelToCompleted(_gameplayInputArgs.LevelNumber);
            _statsService.IncrementWins();

            _coroutinesPerformer.StartCoroutine(_playerDataProvider.SaveAsync());
        }

        public void Update(float deltaTime)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                _coroutinesPerformer.StartCoroutine(_sceneSwitcherService.ProcessSwitchTo(Scenes.MainMenu));
            }
        }
    }
}
