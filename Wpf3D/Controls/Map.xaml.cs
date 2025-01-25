using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Wpf3D.Controls
{
    /// <summary>
    /// Map.xaml の相互作用ロジック
    /// </summary>
    public partial class Map : UserControl
    {
        private static readonly Material FloorMaterial = MaterialFactory.CreateDiffuseMaterial(Colors.Gray, 0.4);
        private static readonly Material ObjectMaterial = MaterialFactory.CreateDiffuseMaterial(Colors.Red, 0.8);
        private static readonly Material InterFloorObjectMaterial = MaterialFactory.CreateDiffuseMaterial(Colors.Blue, 0.4);
        private static readonly Model3D FloorBasePlane = Model3DFactory.CreateXZPlane(5, 5, FloorMaterial);

        public Camera MainCamera
        {
            get => Viewport.Camera;
            set => Viewport.Camera = value;
        }

        public Map()
        {
            InitializeComponent();
            InitializeViewport();
        }

        private void InitializeViewport()
        {
            // カメラ
            var camera = CreateCamera();
            Viewport.Camera = camera;

            var modelVisual = new ModelVisual3D();
            var group = new Model3DGroup();
            modelVisual.Content = group;

            // ライト
            foreach (var light in CreateLights())
            {
                group.Children.Add(light);
            }

            // モデル
            foreach (var model in CreateMap())
            {
                group.Children.Add(model);
            }

            Viewport.Children.Add(modelVisual);
        }

        private static PerspectiveCamera CreateCamera()
        {
            var camera = new PerspectiveCamera
            {
                Position = new Point3D(-8, 8, 10),
                LookDirection = new Vector3D(0.8, -0.5, -1),
                UpDirection = new Vector3D(0, 1, 0),
                FieldOfView = 45,
                NearPlaneDistance = 1,
                FarPlaneDistance = 100
            };
            return camera;
        }

        private static IEnumerable<Light> CreateLights()
        {
            yield return new DirectionalLight(Colors.White, new Vector3D(0, -0.2, -1));
            //yield return new AmbientLight(Colors.White);
        }

        private static IEnumerable<Model3DGroup> CreateMap()
        {
            for (var i = 0; i < 5; i++)
            {
                yield return CreateFloorMap(i, i < 4);
            }
        }

        private static Model3DGroup CreateFloorMap(int yOffset, bool hasNextFloor)
        {
            var floorMap = new Model3DGroup
            {
                Transform = new TranslateTransform3D(0, yOffset, 0)
            };

            floorMap.Children.Add(FloorBasePlane);

            var pathObject1 = Model3DFactory.CreateCube(
                position: new Vector3D(yOffset, 0, -yOffset),
                scale: new Vector3D(0.1, 0.1, 1.1),
                material: ObjectMaterial);
            floorMap.Children.Add(pathObject1);

            var pathObject2 = Model3DFactory.CreateCube(
                position: new Vector3D(yOffset + 0.1, 0, -yOffset - 1),
                scale: new Vector3D(1, 0.1, 0.1),
                material: ObjectMaterial);
            floorMap.Children.Add(pathObject2);

            if (hasNextFloor)
            {
                var interFloorObjectScale = new Vector3D(0.1, 0.9, 0.1);
                var interFloorObject = Model3DFactory.CreateCube(
                    position: new Vector3D(yOffset + 1, 0.1, -yOffset - 1),
                    scale: interFloorObjectScale,
                    material: InterFloorObjectMaterial);
                floorMap.Children.Add(interFloorObject);
            }

            return floorMap;
        }
    }
}
