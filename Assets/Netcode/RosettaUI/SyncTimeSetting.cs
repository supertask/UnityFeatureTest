using UnityEngine;

using Klak.Spout;
using PrefsGUI;
using PrefsGUI.RosettaUI;
using RosettaUI;

using Unity.Netcode;


namespace InteractiveVJ
{
    public class SyncTimeSetting : MonoBehaviour, IElementCreator
    {

        public PrefsFloat timeScale = new("Time scale", 1);


        void Start()
        {
        }

        void Update()
        {
        }

        public Element CreateElement(LabelElement label)
        {
            return UI.Page(
                //cameraSpoutName.CreateElement().RegisterValueChangeCallback(() =>
                //{
                //    spoutSender.spoutName = cameraSpoutName.Get();
                //}),
                timeScale.CreateSlider(0, 20f).RegisterValueChangeCallback(() => {
                    Time.timeScale = timeScale.Get();
                })
            );
        }


        private void OnDestroy()
        {
        }


    }
}