using KihonEngine.GameEngine.Graphics.ModelsBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace KihonEngine.GameEngine.Graphics.ModelDefinitions
{
    public class ModelMetadata
    {
        public ModelType Type;
        public Color Color { get; set; }
        public bool UseBackMaterial { get; set; }
    }
}
