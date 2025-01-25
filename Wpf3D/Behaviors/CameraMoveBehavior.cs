using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using Wpf3D.Controls;

namespace Wpf3D.Behaviors
{
    internal class CameraMoveBehavior : Behavior<Map>
    {
        private Point _lastMousePosition;

        public CameraMoveBehavior() : base()
        {
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            SubscribeMouseEvents();
        }

        protected override void OnDetaching()
        {
            UnsubscribeMouseEvents();
            base.OnDetaching();
        }

        private void SubscribeMouseEvents()
        {
            AssociatedObject.MouseLeftButtonDown += OnMouseLeftButtonDown;
            AssociatedObject.MouseRightButtonDown += OnMouseRightButtonDown;
            AssociatedObject.MouseMove += OnMouseMove;
            AssociatedObject.MouseWheel += OnMouseWheel;
        }

        private void UnsubscribeMouseEvents()
        {
            AssociatedObject.MouseLeftButtonDown -= OnMouseLeftButtonDown;
            AssociatedObject.MouseRightButtonDown -= OnMouseRightButtonDown;
            AssociatedObject.MouseMove -= OnMouseMove;
            AssociatedObject.MouseWheel -= OnMouseWheel;
        }

        private void OnMouseLeftButtonDown(object? sender, MouseEventArgs args)
        {
        }

        private void OnMouseRightButtonDown(object? sender, MouseEventArgs args)
        {
            _lastMousePosition = args.GetPosition(AssociatedObject);
        }

        private void OnMouseMove(object? sender, MouseEventArgs args)
        {
            if (AssociatedObject.MainCamera is not PerspectiveCamera camera) { return; }
            var currentMousePosition = args.GetPosition(AssociatedObject);
            var mouseMove = currentMousePosition - _lastMousePosition;

            var newCamera = camera.Clone();
            if (args.RightButton == MouseButtonState.Pressed)
            {
                var updatedLookDirection = CalculateCameraLookDirection(camera.LookDirection, camera.UpDirection, mouseMove);
                newCamera.LookDirection = updatedLookDirection;
            }
            else if (args.MiddleButton == MouseButtonState.Pressed)
            {
                var updatedPosition = CalculateCameraPosition(camera.Position, camera.LookDirection, mouseMove);
                newCamera.Position = updatedPosition;
            }
            AssociatedObject.MainCamera = newCamera;

            _lastMousePosition = currentMousePosition;
        }

        private void OnMouseWheel(object? sender, MouseWheelEventArgs args)
        {
        }

        private static Vector3D CalculateCameraLookDirection(Vector3D lookDirection, Vector3D upDirection, Vector mouseMove)
        {
            var angleDelta = mouseMove / 100;

            // ヨー
            var rotate = new RotateTransform3D(new AxisAngleRotation3D(upDirection, angleDelta.X));
            lookDirection = rotate.Transform(lookDirection);
            lookDirection.Normalize();

            // ピッチ
            var rotateY = new RotateTransform3D(new AxisAngleRotation3D(Vector3D.CrossProduct(lookDirection, upDirection), -angleDelta.Y));
            lookDirection = rotateY.Transform(lookDirection);
            lookDirection.Normalize();

            return lookDirection;
        }

        private static Point3D CalculateCameraPosition(Point3D position, Vector3D lookDirection, Vector mouseMove)
        {
            var positionDelta = mouseMove / 100;

            var theta = Math.Atan2(-lookDirection.Z, lookDirection.X);
            var (sin, cos) = Math.SinCos(theta);
            var delta = new Vector3D(-positionDelta.X * sin, positionDelta.Y, -positionDelta.X * cos);

            return position + delta;
        }
    }
}
