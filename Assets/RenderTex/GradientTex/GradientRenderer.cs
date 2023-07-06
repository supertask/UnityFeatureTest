using UnityEngine;

public class GradientRenderer : MonoBehaviour {
    public Material gradientMaterial;
    public RenderTexture renderTexture;
    public int textureWidth = 1024;
    public int textureHeight = 1024;

    void Start() {
        if (gradientMaterial == null || gradientMaterial.shader.name != "Custom/") {
            Debug.LogError("Please assign a material with 'Custom/GradientX' shader");
            return;
        }

        // Create new render texture
        renderTexture = new RenderTexture(textureWidth, textureHeight, 24);
        renderTexture.enableRandomWrite = true;
        renderTexture.Create();

        // Render the gradient on the render texture
        Graphics.Blit(null, renderTexture, gradientMaterial);
    }
}
