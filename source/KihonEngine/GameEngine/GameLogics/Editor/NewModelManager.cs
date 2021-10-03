using KihonEngine.GameEngine.Graphics;
using KihonEngine.GameEngine.Graphics.ModelDefinitions;
using KihonEngine.GameEngine.Graphics.ModelsBuilders;
using KihonEngine.GameEngine.State;
using KihonEngine.Services;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace KihonEngine.GameEngine.GameLogics.Editor
{
    public class NewModelManager : INewModelManager
    {
        private const int selectionBoxDistance = 1;

        protected ILogService LogService 
            => Container.Get<ILogService>();

        protected IGameEngineState State
            => Container.Get<IGameEngineState>();

        protected IWorldEngine WorldEngine 
            => Container.Get<IWorldEngine>();

        public void StartAddNewModel(ModelBaseDefinition definition)
        {
            if (State.Editor.ActionNew.Mode == GameEngine.State.Editor.NewModelMode.Active)
            {
                return;
            }

            var modelBuilder = new ModelBuilderFromDefinition();
            var model = modelBuilder.Build(definition);
            var points = TransformHelper.GetPoints(model.Children);
            
            var selectionBoxDefinition = new VolumeDefinition
            {
                Color = Colors.White,
                Position = new Point3D(
                            model.Translation.Value.OffsetX - selectionBoxDistance,
                            model.Translation.Value.OffsetY - selectionBoxDistance,
                            model.Translation.Value.OffsetZ - selectionBoxDistance),

                Metadata = new VolumeMetadata
                {
                    XSize = points.Max(x => x.X) + selectionBoxDistance * 2,
                    YSize = points.Max(x => x.Y) + selectionBoxDistance * 2,
                    ZSize = points.Max(x => x.Z) + selectionBoxDistance * 2,
                    Opacity = .5,
                }
            };


            State.Editor.ActionSelect.SelectedModel = model;
            State.Editor.ActionNew.NewModel = model;
            State.Editor.ActionNew.NewModelSelectionBox = modelBuilder.Build(selectionBoxDefinition);
            State.Editor.ActionNew.Mode = GameEngine.State.Editor.NewModelMode.Active;

            var position = State.Graphics.PlayerCamera.GetTransformedPosition();
            var meterWallColor = State.Editor.ActionMove.ShowMeterWallWhenMove ? Colors.Yellow : Colors.Transparent;
            var midSize = 200;

            var meterBoxDefinition = new VolumeDefinition
            {
                Color = meterWallColor,
                Position = new Point3D(position.X - midSize, position.Y - midSize, position.Z - midSize),
                Metadata = new VolumeMetadata
                {
                    XSize = midSize * 2,
                    YSize = midSize * 2,
                    ZSize = midSize * 2,
                    Opacity = .5, 
                    UseBackMaterial = true,
                }
            };

            State.Editor.ActionNew.InvisibleMeterModel = modelBuilder.Build(meterBoxDefinition);
            WorldEngine.AddModel(State.Editor.ActionNew.InvisibleMeterModel);
        }

        public bool UpdateNewModelPosition(System.Windows.Point mousePosition)
        {
            var updated = false;

            if (State.Editor.ActionNew.Mode == GameEngine.State.Editor.NewModelMode.Active)
            {
                WorldEngine.RemoveModel(State.Editor.ActionNew.NewModel);
                WorldEngine.RemoveModel(State.Editor.ActionNew.NewModelSelectionBox);
                WorldEngine.GetModel(mousePosition, out var hitPoint);
                WorldEngine.AddModel(State.Editor.ActionNew.NewModel);
                WorldEngine.AddModel(State.Editor.ActionNew.NewModelSelectionBox);

                var matrix = State.Editor.ActionNew.NewModel.Translation.Value;

                var y = hitPoint.Value.Y;
                
                if (State.Editor.ActionNew.NewModel.Type == ModelType.Floor)
                {
                    y += 1;
                }
                else if (State.Editor.ActionNew.NewModel.Type == ModelType.Ceiling)
                {
                    y += 20;
                }

                var position = new Point3D(matrix.OffsetX, matrix.OffsetY, matrix.OffsetZ);

                if (hitPoint.HasValue && position != hitPoint)
                {
                    updated = true;
                    State.Editor.ActionNew.NewModel
                        .Translate(new Vector3D(hitPoint.Value.X, y, hitPoint.Value.Z));

                    State.Editor.ActionNew.NewModelSelectionBox
                        .Translate(new Vector3D(hitPoint.Value.X - selectionBoxDistance, y - selectionBoxDistance, hitPoint.Value.Z - selectionBoxDistance));
                }
            }

            return updated;
        }

        public void CancelAddNewModel()
        {
            if (State.Editor.ActionNew.Mode == GameEngine.State.Editor.NewModelMode.Active)
            {
                State.Editor.ActionNew.Mode = GameEngine.State.Editor.NewModelMode.NotActive;

                WorldEngine.RemoveModel(State.Editor.ActionNew.InvisibleMeterModel);
                WorldEngine.RemoveModel(State.Editor.ActionNew.NewModelSelectionBox);
                WorldEngine.RemoveModel(State.Editor.ActionNew.NewModel);

                State.Editor.ActionNew.NewModel = null;
                State.Editor.ActionNew.NewModelSelectionBox = null;
                State.Editor.ActionNew.InvisibleMeterModel = null;
                State.Editor.ActionSelect.SelectedModel = null;
            }
        }

        public void ApplyNewModel()
        {
            if (State.Editor.ActionNew.Mode == GameEngine.State.Editor.NewModelMode.Active)
            {
                State.Editor.ActionNew.Mode = GameEngine.State.Editor.NewModelMode.NotActive;

                WorldEngine.RemoveModel(State.Editor.ActionNew.InvisibleMeterModel);
                WorldEngine.RemoveModel(State.Editor.ActionNew.NewModelSelectionBox);

                State.Editor.ActionNew.NewModel = null;
                State.Editor.ActionNew.NewModelSelectionBox = null;
                State.Editor.ActionNew.InvisibleMeterModel = null;
            }
        }
    }
}
