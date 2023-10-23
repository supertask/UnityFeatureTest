using PrefsGUI;
using RosettaUI;
using PrefsGUI.RosettaUI;
using UnityEngine;


namespace InteractiveVJ {
    [RequireComponent(typeof(RosettaUIRoot))]
    public class MainUI : MonoBehaviour
    {
        public Vector2 windowPosition;
        private Element mainWindow;
        
        private void Start()
        {
            var root = GetComponent<RosettaUIRoot>();
            CreateElement();
            root.Build(mainWindow);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                mainWindow.SetEnable(!mainWindow.Enable);
            }
        }

        void CreateElement()
        {
            mainWindow =  UI.Window(
                "Real Mapping Sim",
                UI.WindowLauncher<CameraSetting>("Camera"),
                UI.WindowLauncher<SyncTimeSetting>("SyncTime"),
                //UI.WindowLauncher<SceneSetting>("Scene setting"),
                UI.WindowLauncher(UI.Window(nameof(PrefsSearch), PrefsSearch.CreateElement())),
                UI.Space().SetHeight(15f),
                UI.Label(() => $"file path: {PrefsGUI.Kvs.PrefsKvsPathSelector.path}"),
                UI.Button(nameof(Prefs.Save), Prefs.Save),
                UI.Button(nameof(Prefs.DeleteAll), Prefs.DeleteAll)
            ).SetPosition(windowPosition).SetWidth(350f);

        }
    }
}