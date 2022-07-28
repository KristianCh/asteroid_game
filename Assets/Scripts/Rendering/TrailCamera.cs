using UnityEngine;

namespace Rendering
{
    public class TrailCamera : MonoBehaviour
    {
        public RenderTexture TrailRT;
        public RenderTexture TrailHalfRT;
        public RenderTexture TrailQuarterRT;
        public Material TrailPingPongMat;
        
        void Update()
        {
            TrailPingPongMat.SetFloat("Time", Time.deltaTime);
            Graphics.Blit(TrailRT, TrailHalfRT, TrailPingPongMat);
            Graphics.Blit(TrailHalfRT, TrailQuarterRT, TrailPingPongMat);
            Graphics.Blit(TrailQuarterRT, TrailHalfRT, TrailPingPongMat);
            Graphics.Blit(TrailHalfRT, TrailRT, TrailPingPongMat);
        }
    }
}
