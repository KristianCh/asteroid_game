using UnityEngine;

namespace Rendering
{
    public class RenderCamera : MonoBehaviour
    {
        public Material ScreenEffectsMat;

        void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            // Read pixels from the source RenderTexture, apply the material, copy the updated results to the destination RenderTexture
            Graphics.Blit(src, dest, ScreenEffectsMat);
        }
    }
}
