using Fusee.Base.Common;
using Fusee.Base.Core;
using Fusee.Engine.Common;
using Fusee.Engine.Core;
using Fusee.Engine.Core.Scene;
using Fusee.Math.Core;
using Fusee.Serialization;
using Fusee.Xene;
using static Fusee.Engine.Core.Input;
using static Fusee.Engine.Core.Time;
using Fusee.Engine.GUI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FuseeApp
{
    [FuseeApplication(Name = "Tut08_FirstSteps", Description = "Yet another FUSEE App.")]
    public class Tut08_FirstSteps : RenderCanvas
    { 
        private float _camAngle = 0;
        private Cube[] cubes;

        private int arrsize = 5;
        private Random rnd = new Random();

        // Init is called on startup. 
        public override void Init()
        {
            
             RC.ClearColor = (float4) ColorUint.DarkGreen;
            _scene = new SceneContainer();

            cubes = new Cube[arrsize];
            var maxsize = 5;
            var prevy = -maxsize * arrsize + maxsize*1.5f;
            for (var i = 0; i < arrsize; i++){
                float edgelength = (i+1) * ((float) maxsize/arrsize);
                float3 size = new float3(edgelength);
                var newcube = new Cube(new float3(1,1,1), new float3(0,prevy + 2 * size.y,0), (float4)ColorUint.Blue, size, edgelength);
                cubes[i] = newcube;
                _scene.Children.Add(cubes[i].node);
                prevy = (int) newcube.trans.Translation.y;
            }
            _sceneRenderer = new SceneRendererForward(_scene);
        }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
            SetProjectionAndViewport();
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            for (var i = 0; i < cubes.Length; i++){
                float4 rgb = HSLtoRGB(Time.TimeSinceStart * 180 + i * 360/arrsize, 1f, 0.5f);
                cubes[i].changeColor(rgb);

                cubes[i].rotate((45f * M.Pi/180f) * Time.DeltaTime);
                cubes[i].setTranslate((float) Math.Cos(2 * Time.TimeSinceStart) * cubes[i].size * 3);
                cubes[i].setScale(Math.Abs(cubes[i].trans.Translation.x) / (cubes[i].size * 3));
            }
            
            RC.View = float4x4.CreateTranslation(0, 0, 50) * float4x4.CreateRotationY(_camAngle);

            _sceneRenderer.Render(RC);
            Present();
        }

        public void SetProjectionAndViewport()
        {
            // Set the rendering area to the entire window size
            RC.Viewport(0, 0, Width, Height);

            // Create a new projection matrix generating undistorted images on the new aspect ratio.
            var aspectRatio = Width / (float)Height;

            // 0.25*PI Rad -> 45° Opening angle along the vertical direction. Horizontal opening angle is calculated based on the aspect ratio
            // Front clipping happens at 1 (Objects nearer than 1 world unit get clipped)
            // Back clipping happens at 2000 (Anything further away from the camera than 2000 world units gets clipped, polygons will be cut)
            var projection = float4x4.CreatePerspectiveFieldOfView(M.PiOver4, aspectRatio, 1, 20000);
            RC.Projection = projection;
        }        

    }
}