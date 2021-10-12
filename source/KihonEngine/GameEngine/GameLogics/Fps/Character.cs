using KihonEngine.GameEngine.Graphics.ModelsBuilders;
using KihonEngine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace KihonEngine.GameEngine.GameLogics.Fps
{
    public static class CharacterMetadata
    {
        public const string Character = "character";
        public const string CharacterHead = "character-head";
        public const string CharacterTorso = "character-torso";
        public const string CharacterLeftArm = "character-leftArm";
        public const string CharacterRightArm = "character-rightArm";
        public const string CharacterLeftLeg = "character-leftLeg";
        public const string CharacterRightLeg = "character-rightLeg";
        
        public static string CharacterId(string id) => $"character-id-{id}";
    }

    public class Character
    {
        protected ILogService LogService { get; set; }

        private const int PlayerCameraSizeY = 10;
        private const int PlayerSizeY = 12;
        private const int PlayerSizeX = 6;
        private const int PlayerSizeZ = 6;

        public Character(ILogService logService)
        {
            LogService = logService;
        }

        public LayeredModel3D Draw(Point3D position, Vector3D lookDirection)
        {
            var id = "666";
            var characterModels = new List<LayeredModel3D>();
            var builder = new VolumeBuilder(Colors.White, characterModels);

            var head = builder.Create(-2, 8, -2, 4, 4, 4);
            head.Tags.Add(CharacterMetadata.Character);
            head.Tags.Add(CharacterMetadata.CharacterHead);
            head.Tags.Add(CharacterMetadata.CharacterId(id));
            builder.ApplyTexture(head, VolumeFace.Front, "steve-front.png");
            builder.ApplyTexture(head, VolumeFace.Back, "steve-back.png");
            builder.ApplyTexture(head, VolumeFace.Left, "steve-left.png");
            builder.ApplyTexture(head, VolumeFace.Right, "steve-right.png");
            builder.ApplyTexture(head, VolumeFace.Top, "steve-top.png");

            builder.Color = Colors.Turquoise;
            var torso = builder.Create(-2, 4, -1, 4, 4, 2);
            torso.Tags.Add(CharacterMetadata.Character);
            torso.Tags.Add(CharacterMetadata.CharacterTorso);
            torso.Tags.Add(CharacterMetadata.CharacterId(id));

            builder.Color = Colors.White;
            var leftArmRotationOrigin = new Point3D(2.5, 3.5, 1.5);
            var leftArm = builder.Create(2, 4, -1, 1, 4, 2);
            leftArm.Tags.Add(CharacterMetadata.Character);
            leftArm.Tags.Add(CharacterMetadata.CharacterLeftArm);
            leftArm.Tags.Add(CharacterMetadata.CharacterId(id));

            var rightArmRotationOrigin = new Point3D(-2.5, 3.5, 1.5);
            var rightArm = builder.Create(-3, 4, -1, 1, 4, 2);
            rightArm.Tags.Add(CharacterMetadata.Character);
            rightArm.Tags.Add(CharacterMetadata.CharacterRightArm);
            rightArm.Tags.Add(CharacterMetadata.CharacterId(id));

            builder.Color = Colors.Violet;
            var leftLegRotationOrigin = new Point3D(1, 4, 1);
            var leftLeg = builder.Create(0, 0, -1, 2, 4, 2);
            leftLeg.Tags.Add(CharacterMetadata.Character);
            leftLeg.Tags.Add(CharacterMetadata.CharacterLeftLeg);
            leftLeg.Tags.Add(CharacterMetadata.CharacterId(id));

            var rightLegRotationOrigin = new Point3D(-1, 4, 1);
            var rightLeg = builder.Create(-2, 0, -1, 2, 4, 2);
            rightLeg.Tags.Add(CharacterMetadata.Character);
            rightLeg.Tags.Add(CharacterMetadata.CharacterRightLeg);
            rightLeg.Tags.Add(CharacterMetadata.CharacterId(id));

            // Position
            var vector = new Vector3D(position.X, position.Y - PlayerCameraSizeY, position.Z);

            // Look animations
            var lookAngle = Vector3D.AngleBetween(new Vector3D(0, lookDirection.Y, lookDirection.Z), new Vector3D(0, 0, lookDirection.Z));
            var lookRotationDirection = lookDirection.Y > 0 ? -1 : 1;
            LogService.Log($"Character : lookAngle:{lookAngle * lookRotationDirection}; lookDirection:{lookRotationDirection}");
            head.RotateByAxisX(lookAngle * lookRotationDirection);

            rightArm.RotateByAxisX(-90 + lookAngle * lookRotationDirection, rightArmRotationOrigin);

            // Walk animations
            leftArm.RotateByAxisX(-30, leftArmRotationOrigin);            

            leftLeg.RotateByAxisX(-45, leftLegRotationOrigin);
            rightLeg.RotateByAxisX(45, rightLegRotationOrigin);

            // Group body parts
            var groupBuilder = new GroupBuilder();
            var group = groupBuilder.Create(new[] { head, torso, leftArm, rightArm, leftLeg, rightLeg });
            group.Tags.Add(CharacterMetadata.Character);

            // Set group position and orientation
            var angle = Vector3D.AngleBetween(new Vector3D(lookDirection.X, 0, lookDirection.Z), new Vector3D(0, 0, 1));
            var rotationDirection = lookDirection.X > 0 ? 1 : -1;
            group.Translate(vector);
            group.RotateByAxisY(angle * rotationDirection, new Point3D(0,0,0));
            LogService.Log($"Character : angle:{angle * rotationDirection}; lookDirection:{lookDirection}");

            return group;
        }
    }
}
