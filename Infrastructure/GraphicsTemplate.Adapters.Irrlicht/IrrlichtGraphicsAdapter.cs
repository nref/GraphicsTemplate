﻿using Irrlicht;
using Irrlicht.Core;
using Irrlicht.Video;
using Irrlicht.Scene;
using Irrlicht.GUI;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using GraphicsTemplate.Models;

namespace GraphicsTemplate.Adapters.Irrlicht
{
    public class IrrlichtGraphicsAdapter : IGraphicsAdapter
    {
        private IrrlichtDevice _device;

        private VideoDriver _driver => _device?.VideoDriver;
        private SceneManager _smgr => _device?.SceneManager;
        private GUIEnvironment _gui => _device?.GUIEnvironment;
        private CameraSceneNode _cam;
        private List<LightSceneNode> _lights;

        private string _path = ".";

        private Size _size;
        public Size Size 
        { 
            get => _size; 
            set
            {
                if (_size == value)
                    return;

                _size = value;
                HandleSizeChanged();
            }
        }

        public IrrlichtGraphicsAdapter()
        {
        }

        public IrrlichtGraphicsAdapter(string path)
        {
            _path = path;
        }

        public IntPtr WindowID => _device.VideoDriver.ExposedVideoData.WindowID;

        public void Start(IntPtr hwnd)
        {
            var p = new IrrlichtCreationParameters
            {
                WindowID = hwnd,
                DriverType = DriverType.Direct3D9,
                AntiAliasing = 8
            };

            _device = IrrlichtDevice.CreateDevice(p);
            _device.OnEvent += new IrrlichtDevice.EventHandler(HandleDeviceEvent);

            _cam = AddCamera();
            ResetCamera();
            _lights = AddLights();

            var texture = _driver.GetTexture($"{_path}/blue.png");
            _smgr.AddSkyBoxSceneNode(texture, texture, texture, texture, texture, texture);

            //_gui.AddImage(_driver.GetTexture($"{_path}/logo.png"), new Vector2Di(0, 10));

            Task.Run(() => AddMeshes(new[] {
                $"{_path}/box.obj",
            }));
        }

        private CameraSceneNode AddCamera()
        {
            return _smgr.AddCameraSceneNodeMaya(null, -150, 150, 15);
        }

        private List<LightSceneNode> AddLights()
        {
            var x = 15.0f;
            var y = 0.5f;
            var z = 15.0f;
            var radius = 30.0f;
            var warm = new Colorf(255 / 255.0f, 244 / 255.0f, 229 / 255.0f, 1.0f);

            return new List<LightSceneNode>
            {
                _smgr.AddLightSceneNode(null, new Vector3Df(x, y, z), warm, radius),
                _smgr.AddLightSceneNode(null, new Vector3Df(-x, y, z), warm, radius),
                _smgr.AddLightSceneNode(null, new Vector3Df(-x, y, -z), warm, radius),
                _smgr.AddLightSceneNode(null, new Vector3Df(x, y, -z), warm, radius),
            };
        }

        public void AddMeshes(string[] files)
        {
            foreach (var mesh in files)
            {
                AddMesh(mesh);
            }
        }

        private readonly Dictionary<Guid, MeshSceneNode> _nodes 
            = new Dictionary<Guid, MeshSceneNode>();

        public Guid AddMesh(string file)
        {
            var mesh = _smgr.GetMesh(file);

            if (mesh == default)
            {
                return default;
            }

            var node = _smgr.AddMeshSceneNode(mesh);
            AddWireframe(mesh, node);

            //var guid = Guid.NewGuid();
            var guid = Guid.Parse("655bbef0-511d-4d0c-8d54-90a172349f41");
            _nodes[guid] = node;
            return guid;
        }

        private void AddWireframe(Mesh mesh, SceneNode node)
        {
            var wireframe = _smgr.AddMeshSceneNode(mesh, node);
            wireframe.Scale *= 1.01f;

            wireframe.SetMaterial(0, new Material
            {
                DiffuseColor = Color.SolidBlack,
                ColorMaterial = ColorMaterial.None,
                Lighting = true,
                Wireframe = true,
                Thickness = 0.5f,
                BackfaceCulling = false,
            });
        }

        public void Run()
        {
            while (_device.Run())
            {
                _device.VideoDriver.BeginScene();
                _device.SceneManager.DrawAll();
                _device.GUIEnvironment.DrawAll();

                DrawFrame();
                _device.VideoDriver.EndScene();
            }

            _device.Drop();
        }

        private void DrawFrame()
        {
            DrawLine(new Line3Df(0, 0, 0, 1, 0, 0), Color.SolidRed);
            DrawLine(new Line3Df(0, 0, 0, 0, 1, 0), Color.SolidGreen);
            DrawLine(new Line3Df(0, 0, 0, 0, 0, 1), Color.SolidBlue);
        }

        private void DrawLine(Line3Df line, Color color)
        {
            _driver.SetMaterial(new Material { Lighting = false });
            _driver.SetTransform(TransformationState.World, Matrix.Identity);
            _driver.Draw3DLine(line, color);
        }

        private void ResetCamera()
        {
            _cam.Target = new Vector3Df(0);
            _cam.Position = new Vector3Df(5, 5, 5);
            _cam.FOV = (float)Math.PI / 32;
            _cam.NearValue = 0.5f;
        }

        public void Close()
        {
            if (_device == null)
                return;

            _device.Close();
        }

        public void HandleKey(Key key, bool pressed)
        {
            var c = char.TryParse(key.ToString(), out var cc) ? cc : '\0';
            _device.PostEvent(new Event(c, KeyMapper.Map(key), pressed));
        }

        private void HandleSizeChanged()
        {
            if (_cam != null)
            {
                _cam.AspectRatio = (float)Size.Width / (float)Size.Height;
            }

            if (_driver != null)
            {
                _driver.ResizeNotify(new Dimension2Di((int)Size.Width, (int)Size.Height));
            }
        }

        private bool HandleDeviceEvent(Event e)
        {
            if (e.Type == EventType.Key && e.Key.PressedDown)
            {
                switch (e.Key.Key)
                {
                    case KeyCode.Space:
                        ResetCamera();
                        break;
                }
            }

            if (e.Type == EventType.Mouse)
            {
                if (e.Mouse.Type == MouseEventType.Wheel)
                {
                    int sign = Math.Sign(e.Mouse.Wheel);
                    float ratio = 1.05f;
                    ratio = sign > 0 ? ratio : 1 / ratio;
                    _cam.FOV *= ratio;
                }
            }

            return false;
        }

        public void SetTransform(Guid id, Transform t)
        {
            _nodes[id].Position = new Vector3Df((float)t.X, (float)t.Y, (float)t.Z);
            _nodes[id].Rotation = new Vector3Df((float)t.Rx, (float)t.Ry, (float)t.Rz);
        }
    }
}
