using System;
using System.Collections;
using System.Collections.Generic;
using Code.ECS.Events;
using Code.ECS.Systems;
using Code.UI.Widgets;
using Morpeh;
using Morpeh.Globals;
using UniMob;
using UniMob.UI;
using UniMob.UI.Widgets;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code
{
    public class Bootloader : MonoBehaviour
    {
        [SerializeField] private ViewPanel rootViewPanel = default;
        [SerializeField] private GameObject persistentObjectHolder = default;

        [Header("Preloader")]
        [SerializeField] private GameObject preloaderObject = default;
        [SerializeField] private CanvasGroup preloaderCanvasGroup = default;
        [SerializeField] private float preloaderFadeTime = 0.4f;

        [Header("Views")]
        [SerializeField] private GameObject mainMenuView = default;
        [SerializeField] private GameObject gameMenuView = default;

        [Header("Systems")]
        [SerializeField] private GameSystem gameSystem = default;

        [Header("Variables")]
        [SerializeField] private GlobalVariableBool gameRunning = default;

        [SerializeField] private GlobalVariableFloat timeRemaining = default;
        [SerializeField] private GlobalVariableString primaryStyle = default;

        [Header("Events")]
        [SerializeField] private StartGameRequestedEvent startGameRequestedEvent = default;

        private IEnumerator Start()
        {
            DontDestroyOnLoad(persistentObjectHolder);
            DontDestroyOnLoad(gameObject);

            var world = World.Default;
            world.InitializeGlobals();

            yield return Initialize();
            yield return RegisterSystems(world);
            yield return RegisterWidgetStates();
            yield return LoadMenuScene();
            yield return CreateUI();
            yield return FadePreloader();

            Destroy(gameObject);
            Destroy(preloaderObject);
        }

        private IEnumerator Initialize()
        {
            yield return new WaitForSeconds(1f); // some long initialization
        }

        private IEnumerator LoadMenuScene()
        {
            yield return SceneManager.LoadSceneAsync("Menu Scene");
        }

        private IEnumerator RegisterSystems(World world)
        {
            var group = world.CreateSystemsGroup();

            group.AddSystem(gameSystem);

            world.AddSystemsGroup(0, group);
            yield break;
        }

        private IEnumerator RegisterWidgetStates()
        {
            StateProvider.Register<MainMenuWidget>(() => new MainMenuState(
                view: WidgetViewReference.FromPrefab(mainMenuView),
                primaryStyle: primaryStyle
            ));

            StateProvider.Register<GameMenuWidget>(() => new GameMenuState(
                view: WidgetViewReference.FromPrefab(gameMenuView),
                primaryStyle: primaryStyle,
                gameRunning: gameRunning,
                timeRemaining: timeRemaining,
                startGameRequestedEvent: startGameRequestedEvent
            ));

            yield break;
        }

        private IEnumerator CreateUI()
        {
            UniMobUI.RunApp(Lifetime.Eternal, rootViewPanel, BuildApp);
            yield break;
        }

        private IEnumerator FadePreloader()
        {
            var fadeTimer = preloaderFadeTime;
            while (fadeTimer > 0)
            {
                fadeTimer -= Time.deltaTime;
                preloaderCanvasGroup.alpha = fadeTimer / preloaderFadeTime;
                yield return null;
            }
        }

        private Widget BuildApp(BuildContext context)
        {
            return new Navigator("main", new Dictionary<string, Func<Route>>
            {
                ["main"] = BuildMainRoute,
            });
        }

        private Route BuildMainRoute()
        {
            return new PageRouteBuilder(
                settings: new RouteSettings("main", RouteModalType.Fullscreen),
                pageBuilder: (buildContext, controller, secondaryAnimation) => new MainMenuWidget()
            );
        }
    }
}