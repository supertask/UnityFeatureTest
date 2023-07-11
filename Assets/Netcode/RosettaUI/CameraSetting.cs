using UnityEngine;

using Klak.Spout;
using PrefsGUI;
using PrefsGUI.RosettaUI;
using RosettaUI;

using Unity.Netcode;


namespace InteractiveVJ
{
    public class CameraSetting : MonoBehaviour, IElementCreator
    {
        [SerializeField] public SpoutSender spoutSender;
        [SerializeField] public Camera captureCamera; 
        [SerializeField] public UnityEngine.UI.Image debugUIImage; 

        public PrefsString cameraSpoutName = new("Camera Spout Name", "F-PC-1");
        public PrefsVector2 cameraResolution = new("Camera Resolution", new Vector2(1200, 1200));

        private RenderTexture renderTexture;


        void Start()
        {
            //if (NetworkManager.Singleton.IsClient)
            //{
            //    spoutSender.spoutName = "Client"; //cameraSpoutName.Get();
            //}
            //else
            //{
            //    spoutSender.spoutName = "Server";
            //}
            SetCameraResolution((int)cameraResolution.Get().x, (int)cameraResolution.Get().y);
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
                cameraResolution.CreateElement().RegisterValueChangeCallback(() => { SetCameraResolution((int)cameraResolution.Get().x, (int)cameraResolution.Get().y); })
            );
        }

        public void SetCameraResolution(int newWidth, int newHeight)
        {
            ClearRenderTexture();

            renderTexture = new RenderTexture(newWidth, newHeight, 24);
            captureCamera.targetTexture = renderTexture;
            spoutSender.sourceTexture = renderTexture;
            debugUIImage.material.SetTexture("_MainTex", renderTexture);

        }

        private void ClearRenderTexture()
        {
            if (renderTexture != null)
            {
                renderTexture.Release();
                Destroy(renderTexture);
                renderTexture = null;
            }
        }


        private void OnDestroy()
        {
            ClearRenderTexture();
        }
        /*
        private Element WindowedImage(string label, Func<Texture> getImage, float maxSideSize)
        {
            return UI.Row(
                //UI.DynamicElementIf(() => label != null, () => UI.Label(label)),
                UI.WindowLauncher(
                    label,
                    UI.Window(label,
                        //UI.Image(getImage()).SetWidth(300f).SetHeight(300f)
                        AutoResizableImage(label, getImage, maxSideSize)
                    )
                )
            );
        }

        private Element AutoResizableImage(string label, Func<Texture> getImage, float maxSideSize)
        {
            return UI.Box(
                    UI.DynamicElementOnStatusChanged(getImage, image =>
                    {
                        if (image == null) return UI.Label("No image");
                        float aspect = (float)image.width / image.height;
                        float width = Mathf.Min(maxSideSize, image.width);
                        float height = Mathf.Min(maxSideSize, image.height);
                        width = Mathf.Min(maxSideSize, height * aspect);
                        height = Mathf.Min(maxSideSize, width / aspect);
                        return UI.Image(image).SetWidth(width).SetHeight(height);
                    })
            );
        }
        */




    }
}