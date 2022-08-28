using UnityEngine;

namespace Rendering
{
    public class BloomCamera : MonoBehaviour
    {
        public RenderTexture ObjectsRT;
        public RenderTexture BloomRT;
        public RenderTexture BloomHalfRT;
        public RenderTexture BloomQuarterRT;
        public RenderTexture BloomEighthRT;
        public Material BloomMat;
        public Material BloomPingPongMat;
        public Material UnlitMat;

        public void FixedUpdate()
        {
            ClearOutRenderTexture(BloomRT);
            ClearOutRenderTexture(BloomHalfRT);
            ClearOutRenderTexture(BloomQuarterRT);
            ClearOutRenderTexture(BloomEighthRT);

            Graphics.Blit(ObjectsRT, BloomRT, BloomMat);

            Graphics.Blit(BloomRT, BloomHalfRT, BloomPingPongMat);
            ClearOutRenderTexture(BloomRT);
            Graphics.Blit(BloomHalfRT, BloomQuarterRT, BloomPingPongMat);
            ClearOutRenderTexture(BloomHalfRT);
            Graphics.Blit(BloomQuarterRT, BloomEighthRT, BloomPingPongMat);
            ClearOutRenderTexture(BloomQuarterRT);

            Graphics.Blit(BloomEighthRT, BloomQuarterRT, BloomPingPongMat);
            Graphics.Blit(BloomQuarterRT, BloomHalfRT, BloomPingPongMat);
            Graphics.Blit(BloomHalfRT, BloomRT, BloomPingPongMat);
        }

        public void ClearOutRenderTexture(RenderTexture renderTexture)
        {
            RenderTexture rt = RenderTexture.active;
            RenderTexture.active = renderTexture;
            GL.Clear(true, true, Color.clear);
            RenderTexture.active = rt;
        }
    }
}
