using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace KihonEngine.GameEngine.Graphics.ModelsBuilders
{
    public class LayeredModel3D
    {
        private ModelVisual3D _model;
        private Model3DGroup _model3DLayer;
        private Model3DGroup _axisXRotationLayer;
        private Model3DGroup _axisYRotationLayer;
        private Model3DGroup _axisZRotationLayer;
        private Model3DGroup _translationLayer;

        private double _axisXRotationAngle;
        private double _axisYRotationAngle;
        private double _axisZRotationAngle;

        private ModelType _type;
        private Dictionary<string, object> _metadata;

        private LayeredModel3D(ModelType type, ModelVisual3D model, Model3DGroup model3DLayer, Model3DGroup axisXRotationLayer, Model3DGroup axisYRotationLayer, Model3DGroup axisZRotationLayer, Model3DGroup translationLayer)
        {
            _type = type;
            _metadata = new Dictionary<string, object>();
            _model = model;
            _model3DLayer = model3DLayer;
            _axisXRotationLayer = axisXRotationLayer;
            _axisYRotationLayer = axisYRotationLayer;
            _axisZRotationLayer = axisZRotationLayer;
            _translationLayer = translationLayer;
        }

        //public static LayeredModel3D Load(ModelVisual3D model)
        //{
        //    TryLoad(model, out var layeredModel);
        //    return layeredModel;
        //}

        //public static bool TryLoad(ModelVisual3D model, out LayeredModel3D layeredModel)
        //{
        //    layeredModel = null;

        //    if (model.Content is Model3DGroup)
        //    {
        //        var translationLayer = model.Content as Model3DGroup;
        //        var axisZRotationLayer = GetSubLayer(translationLayer);
        //        var axisYRotationLayer = GetSubLayer(axisZRotationLayer);
        //        var axisXRotationLayer = GetSubLayer(axisYRotationLayer);
        //        var model3DLayer = GetSubLayer(axisXRotationLayer);

        //        if (model3DLayer != null)
        //        {
        //            layeredModel = new LayeredModel3D(ModelType.Undefined, model, model3DLayer, axisXRotationLayer, axisYRotationLayer, axisZRotationLayer, translationLayer);
        //        }
        //    }

        //    return layeredModel != null;
        //}

        //private static Model3DGroup GetSubLayer(Model3DGroup layer)
        //{
        //    if (layer == null)
        //    {
        //        return null;
        //    }

        //    Model3DGroup subLayer = null;
        //    if (layer.Children.Count == 1 && layer.Children.First() is Model3DGroup)
        //    {
        //        subLayer = layer.Children.First() as Model3DGroup;
        //    }

        //    return subLayer;
        //}

        public static LayeredModel3D Create(ModelType type)
        {
            var model3DLayer = new Model3DGroup();

            // Add X Rotation layer
            var axisXRotationLayer = new Model3DGroup();
            axisXRotationLayer.Children.Add(model3DLayer);

            // Add Y Rotation layer
            var axisYRotationLayer = new Model3DGroup();
            axisYRotationLayer.Children.Add(axisXRotationLayer);

            // Add Z Rotation layer
            var axisZRotationLayer = new Model3DGroup();
            axisZRotationLayer.Children.Add(axisYRotationLayer);

            // Add Translation layer
            var translationLayer = new Model3DGroup();
            translationLayer.Children.Add(axisZRotationLayer);

            var model = new ModelVisual3D();
            model.Content = translationLayer;

            return new LayeredModel3D(type, model, model3DLayer, axisXRotationLayer, axisYRotationLayer, axisZRotationLayer, translationLayer);
        }

        public Color GetColor()
        {
            Color selectedColor = Colors.Transparent;
            foreach (var children in Children)
            {
                var childrens = ((Model3DGroup)children).Children;
                foreach (var child in childrens)
                {
                    if (child is DirectionalLight)
                    {
                        var model = (DirectionalLight)child;
                        return model.Color;
                    }
                    else if (child is GeometryModel3D)
                    {
                        var model = (GeometryModel3D)child;
                        if (model.Material is MaterialGroup)
                        {
                            var materialGroup = (MaterialGroup)model.Material;
                            foreach (var material in materialGroup.Children)
                            {
                                if (material is DiffuseMaterial)
                                {
                                    var brush = ((DiffuseMaterial)material).Brush;
                                    if (brush is SolidColorBrush)
                                    {
                                        selectedColor = ((SolidColorBrush)brush).Color;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return selectedColor;
        }

        public void SetColor(Color color)
        {
            var materiaGroup = new MaterialGroup();
            materiaGroup.Children.Add(new DiffuseMaterial(new SolidColorBrush(color)));

            foreach (var children in Children)
            {
                var childrens = ((Model3DGroup)children).Children;
                foreach (var child in childrens)
                {
                    if (child is DirectionalLight)
                    {
                        ((DirectionalLight)child).Color = color;
                    }
                    else
                    {
                        ((GeometryModel3D)child).Material = materiaGroup;
                    }
                }
            }
        }

        public ModelType Type { get { return _type; } }

        public Dictionary<string, object> Metadata { get { return _metadata; } }

        public ModelVisual3D GetModel()
        {
            return _model;
        }

        public Model3DCollection Children
        {
            get { return _model3DLayer.Children; }
        }

        public double AxisXRotationAngle
        {
            get { return _axisXRotationAngle; }
        }

        public void RotateByAxisX(double angle)
        {
            _axisXRotationAngle = TransformHelper.NormalizeAngle(angle);
            _axisXRotationLayer.Transform = TransformHelper.TransformByXAxisRotation(_axisXRotationAngle, this);
        }

        public double AxisYRotationAngle
        {
            get { return _axisYRotationAngle; }
        }

        public void RotateByAxisY(double angle)
        {
            _axisYRotationAngle = TransformHelper.NormalizeAngle(angle);
            _axisYRotationLayer.Transform = TransformHelper.TransformByYAxisRotation(_axisYRotationAngle, this);
        }

        public double AxisZRotationAngle
        {
            get { return _axisZRotationAngle; }
        }

        public void RotateByAxisZ(double angle)
        {
            _axisZRotationAngle = TransformHelper.NormalizeAngle(angle);
            _axisZRotationLayer.Transform = TransformHelper.TransformByZAxisRotation(_axisZRotationAngle, this);
        }

        public Transform3D Translation
        {
            get { return _translationLayer.Transform; }
            set { _translationLayer.Transform = value; }
        }

        public void TranslateOnAxisX(double offset)
        {
            TranslateDelta(new Vector3D(offset, 0, 0));
        }

        public void TranslateOnAxisY(double offset)
        {
            TranslateDelta(new Vector3D(0, offset, 0));
        }

        public void TranslateOnAxisZ(double offset)
        {
            TranslateDelta(new Vector3D(0, 0, offset));
        }

        private void TranslateDelta(Vector3D newVector)
        {
            var matrix = Translation.Value;
            var vector = new Vector3D(matrix.OffsetX, matrix.OffsetY, matrix.OffsetZ);
            vector += newVector;

            matrix.OffsetX = Math.Round(vector.X);
            matrix.OffsetY = Math.Round(vector.Y);
            matrix.OffsetZ = Math.Round(vector.Z);

            Translation = new MatrixTransform3D(matrix);
        }

        public void Translate(Vector3D vector)
        {
            var matrix = Matrix3D.Identity;

            matrix.OffsetX = Math.Round(vector.X);
            matrix.OffsetY = Math.Round(vector.Y);
            matrix.OffsetZ = Math.Round(vector.Z);

            Translation = new MatrixTransform3D(matrix);
        }
    }
}
