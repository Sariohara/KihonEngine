using KihonEngine.GameEngine.Graphics.ModelDefinitions;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace KihonEngine.GameEngine.Graphics.ModelsBuilders
{
    public class LightBuilder
    {
        public Color Color { get; set; }

        private List<LayeredModel3D> models;

        public LightBuilder(Color color, List<LayeredModel3D> models)
        {
            Color = color;
            this.models = models;
        }

        public LayeredModel3D Create(Vector3D direction)
        {
            var layeredModel = LayeredModel3D.Create(ModelType.Light);
            layeredModel.Metadata.Add(ModelType.Light.ToString(), new LightMetadata { Direction = direction });

            var light = new DirectionalLight();
            light.Color = Color;
            light.Direction = direction;

            Model3DGroup group = new Model3DGroup();
            group.Children.Add(light);

            layeredModel.Children.Add(group);

            models.Add(layeredModel);
            return layeredModel;
        }
    }
}
