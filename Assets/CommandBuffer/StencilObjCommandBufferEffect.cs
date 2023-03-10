
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Camera))]
public class StencilObjCommandBufferEffect : MonoBehaviour
{

    //イメージエフェクトかけたいshader
    [SerializeField]
    private Shader _shader;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        var camera = this.GetComponent<Camera>();

        var material = new Material(_shader);

        //コマンドバッファのインスタンス化
        var commandBuffer = new CommandBuffer();
        commandBuffer.name = "Tasuku.StencilObjCommandBufferEffect";

        //今の地点でのレンダリング結果を一時的にRender Textureへコピー
        int tempTextureIdentifier = Shader.PropertyToID("_PostEffect");
        commandBuffer.GetTemporaryRT(tempTextureIdentifier, -1, -1);
        commandBuffer.Blit(BuiltinRenderTextureType.CameraTarget, tempTextureIdentifier);

        //今の時点でのレンダリング結果をイメージエフェクトかけたいマテリアルに反映させる
        commandBuffer.Blit(tempTextureIdentifier, BuiltinRenderTextureType.CameraTarget, material);

        // 一時的なレンダーテクスチャを解放
        commandBuffer.ReleaseTemporaryRT(tempTextureIdentifier);

        //コマンドバッファを追加したい場所を登録
        camera.AddCommandBuffer(CameraEvent.BeforeImageEffects, commandBuffer);
    }
}


