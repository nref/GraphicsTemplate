using System.Diagnostics;
using Urho;
using Urho.Gui;
using Urho.Shapes;
using Application = Urho.Application;
using HorizontalAlignment = Urho.Gui.HorizontalAlignment;
using VerticalAlignment = Urho.Gui.VerticalAlignment;

namespace GraphicsTemplate.Urho
{
    public class UrhoApp : Application
    {
        private Node _cameraNode;
        private Camera _camera;
        private float _cameraYaw;
        private float _cameraPitch;
        private const float _mouseSensitivity = .1f;

        public UrhoApp(ApplicationOptions options) : base(options)
        {
            Log.LogMessage += e => System.Console.WriteLine($"[{e.Level}] {e.Message}");
            UnhandledException += HandleException;
            Started += HandleStarted;
        }

        protected override void Start()
        {
            base.Start();

            Show("UrhoGraphicsService");
            CreateScene();

            Engine.PostUpdate += args =>
            {

            };

            Input.Enabled = true;
            Input.KeyDown += HandleKeyDown;
        }

        protected override void OnUpdate(float timeStep)
        {
            base.OnUpdate(timeStep);

            if (!Input.GetMouseButtonDown(MouseButton.Left))
            {
                return;
            }    

            var mouseMove = Input.MouseMove;
            _cameraYaw += _mouseSensitivity * mouseMove.X;
            _cameraPitch += _mouseSensitivity * mouseMove.Y;
            _cameraPitch = MathHelper.Clamp(_cameraPitch, -90, 90);

            _cameraNode.Rotation = new Quaternion(_cameraPitch, _cameraYaw, 0);
        }

        protected void Show(string text = "")
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
            // 3D scene with Octree
            var scene = new Scene(Context);
            scene.CreateComponent<Octree>();

            Node boxNode = scene.CreateChild();
            boxNode.SetScale(1f);
            var box = boxNode.CreateComponent<Box>();
            box.Color = Color.White;

            Node boxNode2 = scene.CreateChild();
            boxNode2.SetScale(0.95f);
            var box2 = boxNode.CreateComponent<Box>();
            box2.Color = Color.Black;
            Material mat = box2.GetMaterial(0);
            mat.FillMode = FillMode.Wireframe;
            mat.LineAntiAlias = true;

            // Light
            Node lightNode = scene.CreateChild(name: "light");
            var light = lightNode.CreateComponent<Light>();
            light.LightType = LightType.Point;
            light.Range = 50;

            // Camera
            _cameraNode = scene.CreateChild(name: "camera");
            _cameraNode.Position = new Vector3(0, 1, -5);
            _cameraNode.LookAt(boxNode.Position, new Vector3());

            _camera = _cameraNode.CreateComponent<Camera>();

            // Viewport
            Renderer.SetViewport(0, new Viewport(Context, scene, _camera, null));
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
        }
    }
}
