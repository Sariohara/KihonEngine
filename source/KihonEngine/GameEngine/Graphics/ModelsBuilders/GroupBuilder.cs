using KihonEngine.GameEngine.Graphics.ModelDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace KihonEngine.GameEngine.Graphics.ModelsBuilders
{
    public class GroupBuilder
    {
        public LayeredModel3D Create(IEnumerable<LayeredModel3D> models)
        {
            var layeredModel = LayeredModel3D.Create(ModelType.Group);
            layeredModel.Metadata.Add(ModelType.Group.ToString(), new GroupMetadata { Volumes = null });

            Model3DGroup group = new Model3DGroup();
            foreach (var model in models)
            {
                group.Children.Add(model.WrapModel());
            }
            layeredModel.Children.Add(group);

            return layeredModel;
        }
    }
}
