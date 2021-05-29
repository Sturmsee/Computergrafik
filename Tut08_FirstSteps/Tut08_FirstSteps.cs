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

        private SceneContainer _scene;
        private SceneRendererForward _sceneRenderer;

        // Init is called on startup. 
        public override void Init()
        {
            
            RC.ClearColor = (float4) ColorUint.DarkGreen;
                        
            _scene = new SceneContainer();
            


            _sceneRenderer = new SceneRendererForward(_scene);
        }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
            SetProjectionAndViewport();

            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            _camAngle = _camAngle + 90.0f * M.Pi/180.0f * DeltaTime;
            



            RC.View = float4x4.CreateTranslation(0, 0, 50) * float4x4.CreateRotationY(_camAngle);
            Diagnistics.Log(_camAngle);

            _sceneRenderer.Render(RC);

            Present();
        }

        public void SetProjectionAndViewport()
        {
            RC.Viewport(0, 0, Width, Height);

            var aspectRatio = Width / (float)Height;

            var projection = float4x4.CreatePerspectiveFieldOfView(M.PiOver4, aspectRatio, 1, 20000);
            RC.Projection = projection;
        }        

        public class Cube {

            public Transform cubeTransform;
            public Mesh cubeMesh;
            public Fusee.Engine.Core.Effects.DefaultSurfaceEffect cubeShader;
            public SceneNode cubeNode;

            public float size;

            public Cube(float3 _scale, float3 _translate, float4 _color, float3 _dims, float _size) {
                this.cubeTransform = new Transform{Scale = _scale, Translation = _translate};
                this.cubeMesh = SimpleMeshes.CreateCuboid(_dims);
                this.cubeShader = MakeEffect.FromDiffuseSpecular(_color, float4.Zero);
                this.cubeNode = new SceneNode();
                this.cubeNode.Components.Add(this.cubeTransform);
                this.cubeNode.Components.Add(this.cubeShader);
                this.cubeNode.Components.Add(this.cubeMesh);
                this.size = _size;
            }

            public void changeColor(float4 _newcol) {
                this.cubeShader.SurfaceInput.Albedo = _newcol;
            }

            public void rotateCube(float _angle) {
                this.cubeTransform.Rotation.y += _angle;
            }

            public void setTranslate(float _x) {
                this.cubeTransform.Translation.x = _x;
            }

            public void setScale(float _scale) {
                this.cubeTransform.Scale.x = _scale;
            }
        
        }
    }
}