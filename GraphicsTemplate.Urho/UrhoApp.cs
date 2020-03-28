using System;
using System.Diagnostics;
using Urho;
using Urho.Gui;
using Urho.Shapes;
using Application = Urho.Application;
using UnhandledExceptionEventArgs = Urho.UnhandledExceptionEventArgs;
using HorizontalAlignment = Urho.Gui.HorizontalAlignment;
using VerticalAlignment = Urho.Gui.VerticalAlignment;

namespace GraphicsTemplate.Urho
{
    public class UrhoApp : Application
    {
        private Vector3 _cameraTarget = new Vector3();
        private Vector3 _cameraPosition = new Vector3();
        private Node _cameraNode;
        private Camera _camera;
        private ArcBall _arcBall;
        private Scene _scene;
        private bool MouseLeft => Input.GetMouseButtonDown(MouseButton.Left);
        private bool MouseRight => Input.GetMouseButtonDown(MouseButton.Right);

        public UrhoApp(ApplicationOptions options) : base(options)
        {
            Log.LogMessage += e => System.Console.WriteLine($"[{e.Level}] {e.Message}");
            UnhandledException += HandleException;
            Started += HandleStarted;
        }

        protected override void Start()
        {
            base.Start();
            _arcBall = new ArcBall(Input, Graphics);
            Graphics.LineAntiAlias = true;

            ShowText("Graphics Template");
            CreateScene();

            Input.Enabled = true;
            Input.KeyDown += HandleKeyDown;
        }

        protected override void OnUpdate(float timeStep)
        {
            base.OnUpdate(timeStep);

            if (MouseLeft)
            {
                _cameraNode.Position = _arcBall.Rotate(_cameraNode.Position);
                _cameraNode.LookAt(_cameraTarget, new Vector3(0, 1, 0));
            }

            if (MouseRight)
            {
                var mouseMove = new Vector3
                (
                    -(float)Input.MouseMove.X / 100,
                    (float)Input.MouseMove.Y / 100, 
                    0
                );

                _cameraNode.Position += _cameraNode.Rotation * mouseMove;
                _cameraTarget += _cameraNode.Rotation * mouseMove;
            }

            if (Input.MouseMoveWheel != 0)
            {
                int sign = Math.Sign(Input.MouseMoveWheel);

                float ratio = 1.05f;
                ratio = sign > 0 ? ratio : 1 / ratio;

                _camera.Fov = ratio * _camera.Fov;
            }
        }

        protected void ShowText(string text = "")
        {
            var textElement = new Text
            {
                Value = text,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            textElement.SetFont(ResourceCache.GetFont("Fonts/Anonymous Pro.ttf"), 15);
            UI.Root.AddChild(textElement);
        }

        private void CreateScene()
        {
            _scene = new Scene(Context);
            _scene.CreateComponent<Octree>();

            // Box
            Node boxNode = _scene.CreateChild();
            boxNode.SetScale(1f);
            var box = boxNode.CreateComponent<Box>();
            box.Color = Color.White;
            Material mat = box.GetMaterial(0);
            mat.SetTechnique(0, CoreAssets.Techniques.NoTextureVCol, 1, 1);
            box.SetMaterial(mat);

            // Wireframe for box
            Node boxNode2 = _scene.CreateChild();
            boxNode2.SetScale(0.95f);
            var box2 = boxNode.CreateComponent<Box>();
            box2.Color = Color.Black;
            Material mat2 = box2.GetMaterial(0);
            mat2.FillMode = FillMode.Wireframe;
            mat2.LineAntiAlias = true;

            // Light
            Node zoneNode = _scene.CreateChild();
            var zone = zoneNode.CreateComponent<Zone>();
            zone.SetBoundingBox(new BoundingBox(-10000.0f, 10000.0f));
            zone.AmbientColor = new Color(1, 1, 1);

            DrawFrame();

            // Camera
            _cameraNode = _scene.CreateChild();
            _camera = _cameraNode.CreateComponent<Camera>();
            ResetCamera();

            // Viewport
            var viewport = new Viewport(Context, _scene, _camera, null);
            var sky = Color.FromByteFormat(65, 156, 211, 255);
            viewport.SetClearColor(sky);
            Renderer.SetViewport(0, viewport);
        }

        private void DrawFrame()
        {
            float size = 1;

            DrawLine(Vector3.Zero, Vector3.UnitX * size, Color.Red);
            DrawLine(Vector3.Zero, Vector3.UnitY * size, Color.Green);
            DrawLine(Vector3.Zero, Vector3.UnitZ * size, Color.Blue);
        }

        private void DrawLine(Vector3 start, Vector3 end, Color color)
        {
            Node node = _scene.CreateChild();
            CustomGeometry geom = node.CreateComponent<CustomGeometry>();
            geom.BeginGeometry(0, PrimitiveType.LineList);
            var mat = new Material();
            mat.SetTechnique(0, CoreAssets.Techniques.NoTextureUnlitVCol, 1, 1);
            mat.LineAntiAlias = true;

            geom.SetMaterial(mat);

            geom.DefineVertex(start);
            geom.DefineColor(color);
            geom.DefineVertex(end);
            geom.DefineColor(color);

            geom.Commit();
        }

        private void ResetCamera()
        {
            _cameraPosition = new Vector3(5, 5, 5);
            _cameraTarget = new Vector3();
            _cameraNode.Position = _cameraPosition;
            _cameraNode.LookAt(_cameraTarget, new Vector3(0, 1, 0));
        }

        private void HandleException(object sender, UnhandledExceptionEventArgs e)
        {
            Debug.WriteLine($"{e.Exception}");
        }

        private void HandleStarted()
        {
        }

        private void HandleKeyDown(KeyDownEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Space:
                    ResetCamera();
                    break;
            }
        }
    }
}
