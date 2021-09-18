using KihonEngine.GameEngine.Graphics.ModelDefinitions;
using KihonEngine.GameEngine.Graphics.ModelsBuilders;
using KihonEngine.GameEngine.InputControls;
using KihonEngine.Studio.GameEngine.State.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace KihonEngine.GameEngine.GameLogics.Editor
{
    public class EditorLogic : GameLogicBase
    {
        public override string LogicName => "EditorLogic";

        private const int wallDistance = 2;
        private const int selectionBoxDistance = 1;

        public EditorLogic() : base()
        {

        }

        private bool ContainsModel(LayeredModel3D layeredModel, string face, Model3D modelHit)
        {
            if (layeredModel.Metadata.ContainsKey(face))
            {
                var children = layeredModel.Metadata[face];
                return children is Model3DGroup && ((Model3DGroup)children).Children.Contains(modelHit);
            }

            return false;
        }

        private void SelectModel(KeyboardSettings keyboardSettings, Key[] keys, MouseEvent mouseEvent, List<Action> graphicUpdates)
        {
            var needRefresh = false;

            if (mouseEvent.Type == MouseEventType.Click
                && mouseEvent.LeftButton == MouseButtonState.Pressed)
            {
                WorldEngine.RemoveModel(State.Editor.ActionSelect.SelectionBoxModel);
                State.Editor.ActionSelect.SelectionBoxModel = null;

                var selectedModel = WorldEngine.GetModel(mouseEvent.Position, out var hitPoint);
                State.Editor.ActionSelect.HitPoint = hitPoint;

                if (selectedModel != null)
                {
                    Rect3D bb = selectedModel.GetModel().Content.Bounds;
                    Rect3D rect = new Rect3D();
                    bool isIntersecting = rect.IntersectsWith(bb);

                    LogService.Log($"bounds:{bb.X},{bb.Y},{bb.Z}, {bb.SizeX}, {bb.SizeY}, {bb.SizeZ}");

                    State.Editor.ActionSelect.SelectedModel = selectedModel;
                    //_moveModelState.SelectedModel = selectedModel;
                    var points = TransformHelper.GetPoints(selectedModel.Children);

                    var initialPosition = new Point3D(
                         selectedModel.Translation.Value.OffsetX,
                        selectedModel.Translation.Value.OffsetY,
                        selectedModel.Translation.Value.OffsetZ);
                        //bb.X,
                        //bb.Y,
                        //bb.Z);

                    State.Editor.ActionSelect.InitialModelPosition = initialPosition;
                    State.Editor.ActionSelect.MinX = points.Min(x => x.X);
                    State.Editor.ActionSelect.MinY = points.Min(x => x.Y);
                    State.Editor.ActionSelect.MinZ = points.Min(x => x.Z);
                    State.Editor.ActionSelect.MaxX = points.Max(x => x.X);
                    State.Editor.ActionSelect.MaxY = points.Max(x => x.Y);
                    State.Editor.ActionSelect.MaxZ = points.Max(x => x.Z);
                    //State.Editor.ActionSelect.MinX = bb.X;
                    //State.Editor.ActionSelect.MinY = bb.Y;
                    //State.Editor.ActionSelect.MinZ = bb.Z;
                    //State.Editor.ActionSelect.MaxX = bb.SizeX;
                    //State.Editor.ActionSelect.MaxY = bb.SizeY;
                    //State.Editor.ActionSelect.MaxZ = bb.SizeZ;

                    var modelBuilder = new ModelBuilderFromDefinition();

                    // Build Selection box
                    var selectionBoxDefinition = new VolumeDefinition
                    {
                        Color = Colors.White,
                        Position = new Point3D(
                            initialPosition.X - selectionBoxDistance,
                            initialPosition.Y - selectionBoxDistance,
                            initialPosition.Z - selectionBoxDistance),

                        Metadata = new VolumeMetadata
                        {
                            Width = State.Editor.ActionSelect.MaxX + selectionBoxDistance * 2,
                            Height = State.Editor.ActionSelect.MaxY + selectionBoxDistance * 2,
                            Length = State.Editor.ActionSelect.MaxZ + selectionBoxDistance * 2,
                            Opacity = .5,
                        }
                    };

                    var selectBoxModel = modelBuilder.Build(selectionBoxDefinition);
                    //selectBoxModel.RotateByAxisX(selectedModel.AxisXRotationAngle);
                    //selectBoxModel.RotateByAxisY(selectedModel.AxisYRotationAngle);
                    //selectBoxModel.RotateByAxisZ(selectedModel.AxisZRotationAngle);
                    State.Editor.ActionSelect.SelectionBoxModel = selectBoxModel;
                    WorldEngine.AddModel(selectBoxModel);
                    needRefresh = true;
                }
            }
            else if (mouseEvent.Type == MouseEventType.Click
                && mouseEvent.LeftButton == MouseButtonState.Released
                && State.Editor.ActionSelect.SelectionBoxModel != null)
            {
                WorldEngine.RemoveModel(State.Editor.ActionSelect.SelectionBoxModel);
                State.Editor.ActionSelect.SelectionBoxModel = null;
                needRefresh = true;
            }

            if ( needRefresh)
            {
                // for the moment, only for launch NotifyIO
                graphicUpdates.Add(() => { });
            }
        }

        private void MoveModel(KeyboardSettings keyboardSettings, Key[] keys, MouseEvent mouseEvent, List<Action> graphicUpdates)
        {
            var needRefresh = false;

            // Leave : no Left Click
            if (
                State.Editor.ActionMove.Mode != MoveModelMode.NotActive
                && (
                    !(keys.Contains(keyboardSettings.EditorMoveModelFromAxisX) || keys.Contains(keyboardSettings.EditorMoveModelFromAxisY) || keys.Contains(keyboardSettings.EditorMoveModelFromAxisZ))
                    || mouseEvent.LeftButton == MouseButtonState.Released))
            {
                WorldEngine.RemoveModel(State.Editor.ActionMove.InvisibleMeterModel);
                State.Editor.ActionMove.InvisibleMeterModel = null;

                State.Editor.ActionMove.Mode = MoveModelMode.NotActive;
                needRefresh = true;
            }

            // Enter : SPACE + (X|Y|Z) + Left Click
            else if (
                State.Editor.ActionMove.Mode == MoveModelMode.NotActive
                && (keys.Contains(keyboardSettings.EditorMoveModelFromAxisX) || keys.Contains(keyboardSettings.EditorMoveModelFromAxisY) || keys.Contains(keyboardSettings.EditorMoveModelFromAxisZ))
                && mouseEvent.LeftButton == MouseButtonState.Pressed)
            {
                    var selectBoxModel = State.Editor.ActionSelect.SelectionBoxModel;
                    var initialPosition = State.Editor.ActionSelect.InitialModelPosition;

                    // Build Meter Box 
                    VolumeDefinition definition = null;
                    var midSize = 100000;

                    var hittedModel = WorldEngine.GetModel(mouseEvent.Position, out var selectedBoxHitPoint, out var modelHit);

                if (hittedModel != selectBoxModel)
                {
                    WorldEngine.RemoveModel(selectBoxModel);
                }
                else
                {
                    var xFaceSelected = ContainsModel(selectBoxModel, "Left1", modelHit)
                        || ContainsModel(selectBoxModel, "Left2", modelHit)
                        || ContainsModel(selectBoxModel, "Right1", modelHit)
                        || ContainsModel(selectBoxModel, "Right2", modelHit);

                    var yFaceSelected = ContainsModel(selectBoxModel, "Top1", modelHit)
                        || ContainsModel(selectBoxModel, "Top2", modelHit)
                        || ContainsModel(selectBoxModel, "Bottom1", modelHit)
                        || ContainsModel(selectBoxModel, "Bottom2", modelHit);

                    var zFaceSelected = ContainsModel(selectBoxModel, "Front1", modelHit)
                        || ContainsModel(selectBoxModel, "Front2", modelHit)
                        || ContainsModel(selectBoxModel, "Back1", modelHit)
                        || ContainsModel(selectBoxModel, "Back2", modelHit);

                    var moveOnAxisX = keys.Contains(keyboardSettings.EditorMoveModelFromAxisX);
                    var moveOnAxisY = keys.Contains(keyboardSettings.EditorMoveModelFromAxisY);
                    var moveOnAxisZ = keys.Contains(keyboardSettings.EditorMoveModelFromAxisZ);

                    if (moveOnAxisX && xFaceSelected)
                    {
                        xFaceSelected = false;
                        zFaceSelected = true;
                    }
                    else if (moveOnAxisY && yFaceSelected)
                    {
                        yFaceSelected = false;
                        xFaceSelected = true;
                    }
                    else if (moveOnAxisZ && zFaceSelected)
                    {
                        zFaceSelected = false;
                        xFaceSelected = true;
                    }

                    var meterWallColor = State.Editor.ActionMove.ShowMeterWallWhenMove ? Colors.Yellow : Colors.Transparent;

                    if (zFaceSelected)
                    {
                        // Wall X, Y
                        definition = new VolumeDefinition
                        {
                            Color = meterWallColor,
                            Position = new Point3D(-midSize, -midSize, initialPosition.Z - wallDistance),
                            Metadata = new VolumeMetadata
                            {
                                Width = midSize * 2,
                                Height = midSize * 2,
                                Length = State.Editor.ActionSelect.MaxZ + wallDistance * 2,
                                Opacity = .5,
                            }
                        };
                    }
                    else if (yFaceSelected)
                    {
                        // Wall X, Z
                        definition = new VolumeDefinition
                        {
                            Color = meterWallColor,
                            Position = new Point3D(-midSize, initialPosition.Y - wallDistance, -midSize),
                            Metadata = new VolumeMetadata
                            {
                                Width = midSize * 2,
                                Height = State.Editor.ActionSelect.MaxY + wallDistance * 2,
                                Length = midSize * 2,
                                Opacity = .5,
                            }
                        };
                    }
                    else if (xFaceSelected)
                    {
                        // Wall Z, Y
                        definition = new VolumeDefinition
                        {
                            Color = meterWallColor,
                            Position = new Point3D(initialPosition.X - wallDistance, -midSize, -midSize),
                            Metadata = new VolumeMetadata
                            {
                                Width = State.Editor.ActionSelect.MaxX + wallDistance * 2,
                                Height = midSize * 2,
                                Length = midSize * 2,
                                Opacity = .5,
                            }
                        };
                    }

                    // Move on axis X
                    if (moveOnAxisX)
                    {
                        State.Editor.ActionMove.Mode = MoveModelMode.MoveOnAxisX;
                        State.Editor.ActionMove.Delta = initialPosition.X - State.Editor.ActionSelect.HitPoint.Value.X;

                    }
                    // Move on axis Y
                    else if (moveOnAxisY)
                    {
                        State.Editor.ActionMove.Mode = MoveModelMode.MoveOnAxisY;
                        State.Editor.ActionMove.Delta = initialPosition.Y - State.Editor.ActionSelect.HitPoint.Value.Y;
                    }
                    // Move on axis Z
                    else if (moveOnAxisZ)
                    {
                        State.Editor.ActionMove.Mode = MoveModelMode.MoveOnAxisZ;
                        State.Editor.ActionMove.Delta = initialPosition.Z - State.Editor.ActionSelect.HitPoint.Value.Z;
                    }

                    var modelBuilder = new ModelBuilderFromDefinition();
                    State.Editor.ActionMove.InvisibleMeterModel = modelBuilder.Build(definition);
                    WorldEngine.AddModel(State.Editor.ActionMove.InvisibleMeterModel);
                }

                needRefresh = true;
            }
            // Move : still Left Click
            else if (
                State.Editor.ActionMove.Mode != MoveModelMode.NotActive
                && (keys.Contains(keyboardSettings.EditorMoveModelFromAxisX) || keys.Contains(keyboardSettings.EditorMoveModelFromAxisY) || keys.Contains(keyboardSettings.EditorMoveModelFromAxisZ))
                && mouseEvent.Type == MouseEventType.Move
                && mouseEvent.LeftButton == MouseButtonState.Pressed)
            {
                var selectedModel = WorldEngine.GetModel(mouseEvent.Position, out var hitPoint);
                if (selectedModel == State.Editor.ActionMove.InvisibleMeterModel)
                {
                    if (State.Editor.ActionMove.Mode == MoveModelMode.MoveOnAxisX)
                    {
                        var newValue = Math.Round(hitPoint.Value.X + State.Editor.ActionMove.Delta);

                        if (newValue != State.Editor.ActionSelect.InitialModelPosition.X)
                        {
                            State.Editor.ActionSelect.SelectionBoxModel.Translate(new Vector3D(
                                newValue - selectionBoxDistance ,
                                State.Editor.ActionSelect.InitialModelPosition.Y - selectionBoxDistance,
                                State.Editor.ActionSelect.InitialModelPosition.Z - selectionBoxDistance));

                            State.Editor.ActionSelect.SelectedModel.Translate(new Vector3D(
                                newValue,
                                State.Editor.ActionSelect.InitialModelPosition.Y,
                                State.Editor.ActionSelect.InitialModelPosition.Z));
                            needRefresh = true;
                        }
                    }
                    else if (State.Editor.ActionMove.Mode == MoveModelMode.MoveOnAxisY)
                    {
                        var newValue = Math.Round(hitPoint.Value.Y + State.Editor.ActionMove.Delta);

                        if (newValue != State.Editor.ActionSelect.InitialModelPosition.Y)
                        {
                            State.Editor.ActionSelect.SelectionBoxModel.Translate(new Vector3D(
                                State.Editor.ActionSelect.InitialModelPosition.X - selectionBoxDistance,
                                newValue - selectionBoxDistance,
                                State.Editor.ActionSelect.InitialModelPosition.Z - selectionBoxDistance));

                            State.Editor.ActionSelect.SelectedModel.Translate(new Vector3D(
                                State.Editor.ActionSelect.InitialModelPosition.X,
                                newValue,
                                State.Editor.ActionSelect.InitialModelPosition.Z));
                            needRefresh = true;
                        }
                    }
                    else if (State.Editor.ActionMove.Mode == MoveModelMode.MoveOnAxisZ)
                    {
                        var newValue = Math.Round(hitPoint.Value.Z + State.Editor.ActionMove.Delta);

                        if (newValue != State.Editor.ActionSelect.InitialModelPosition.Z)
                        {
                            State.Editor.ActionSelect.SelectionBoxModel.Translate(new Vector3D(
                                State.Editor.ActionSelect.InitialModelPosition.X - selectionBoxDistance,
                                State.Editor.ActionSelect.InitialModelPosition.Y - selectionBoxDistance,
                                newValue - selectionBoxDistance));

                            State.Editor.ActionSelect.SelectedModel.Translate(new Vector3D(
                                State.Editor.ActionSelect.InitialModelPosition.X,
                                State.Editor.ActionSelect.InitialModelPosition.Y,
                                newValue));
                            needRefresh = true;
                        }
                    }
                }
            }

            if (needRefresh)
            {
                // for the moment, only for launch NotifyIO
                graphicUpdates.Add(() => { });
            }
        }

        public void SwitchCameraPosition(KeyboardSettings keyboardSettings, Key[] keys, MouseEvent mouseEvent, List<Action> graphicUpdates)
        {
            if (State.Editor.ActionMove.Mode != MoveModelMode.NotActive)
            {
                return;
            }

            if (mouseEvent.Type == MouseEventType.Move)
            {
                if (mouseEvent.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                {
                    if (keys.Contains(Key.Space))
                    {
                        graphicUpdates.Add(() =>
                        {
                            CameraController.RotateHorizontal(-mouseEvent.DX * 10);
                            CameraController.RotateVertical(mouseEvent.DY * 10);
                        });
                    }
                    else
                    {
                        graphicUpdates.Add(() =>
                        {
                            CameraController.DeltaRotateFromOriginOnAxisY(-mouseEvent.DX);
                            CameraController.DeltaRotateFromOriginOnAxisX(-mouseEvent.DY);
                        });
                    }
                }
                else if (mouseEvent.RightButton == System.Windows.Input.MouseButtonState.Pressed)
                {
                    graphicUpdates.Add(() => CameraController.MoveLateral(mouseEvent.DX * 10));
                    graphicUpdates.Add(() => CameraController.MoveVertical(mouseEvent.DY * 10));
                }
            }
            else if (mouseEvent.Type == MouseEventType.Wheel)
            {
                graphicUpdates.Add(() => CameraController.MoveLongitudinal(mouseEvent.Delta));
            }
        }

        public void DispatchEvents(List<Action> graphicUpdates)
        {
            var selectedLayeredModel = State.Editor.ActionSelect.SelectedModel;
            var mouseSettings = Configuration.GetMouseSettings();
            var keyboardSettings = Configuration.GetKeyboardSettings();
            var keys = _keyboardPressedKeys.ToArray();

            while (_mouseEvents.TryDequeue(out var mouseEvent))
            {
                State.Graphics.Viewport.Dispatcher.Invoke(() =>
                {
                    SelectModel(keyboardSettings, keys, mouseEvent, graphicUpdates);
                    MoveModel(keyboardSettings, keys, mouseEvent, graphicUpdates);
                });

                SwitchCameraPosition(keyboardSettings, keys, mouseEvent, graphicUpdates);
            }
        }

        private void UpdateGraphics(List<Action> graphicUpdates)
        {
            if (graphicUpdates.Any())
            {
                foreach (var update in graphicUpdates)
                {
                    State.Graphics.Viewport.Dispatcher.Invoke(update);
                }

                OnStateChanged();
            }
        }


        protected override void MainLoop(CancellationToken stoppingToken)
        {
            LogService.Log($"Editor Started");

            while (!stoppingToken.IsCancellationRequested)
            {
                Wait(10, stoppingToken);

                if (!stoppingToken.IsCancellationRequested)
                {
                    var graphicUpdates = new List<Action>();

                    // Calculate changes
                    DispatchEvents(graphicUpdates);

                    // Update graphics
                    UpdateGraphics(graphicUpdates);
                }
            }

            LogService.Log($"Editor stopped");
        }
    }
}
