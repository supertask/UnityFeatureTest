
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Camera))]
public class StencilObjCommandBufferEffect : MonoBehaviour
{

    //�C���[�W�G�t�F�N�g��������shader
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

        //�R�}���h�o�b�t�@�̃C���X�^���X��
        var commandBuffer = new CommandBuffer();
        commandBuffer.name = "Tasuku.StencilObjCommandBufferEffect";

        //���̒n�_�ł̃����_�����O���ʂ��ꎞ�I��Render Texture�փR�s�[
        int tempTextureIdentifier = Shader.PropertyToID("_PostEffect");
        commandBuffer.GetTemporaryRT(tempTextureIdentifier, -1, -1);
        commandBuffer.Blit(BuiltinRenderTextureType.CameraTarget, tempTextureIdentifier);

        //���̎��_�ł̃����_�����O���ʂ��C���[�W�G�t�F�N�g���������}�e���A���ɔ��f������
        commandBuffer.Blit(tempTextureIdentifier, BuiltinRenderTextureType.CameraTarget, material);

        // �ꎞ�I�ȃ����_�[�e�N�X�`�������
        commandBuffer.ReleaseTemporaryRT(tempTextureIdentifier);

        //�R�}���h�o�b�t�@��ǉ��������ꏊ��o�^
        camera.AddCommandBuffer(CameraEvent.BeforeImageEffects, commandBuffer);
    }
}


