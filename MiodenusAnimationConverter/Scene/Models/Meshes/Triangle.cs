using System;
using OpenTK.Mathematics;

namespace MiodenusAnimationConverter.Scene.Models.Meshes
{
    public struct Triangle : IMiodenusObject
    {
        public const byte VertexesAmount = 3;
        public readonly Vector4 Normal;
        public readonly Vertex[] Vertexes;

        public Triangle(in Vertex[] vertexes, Vector4 normal)
        {
            CheckVertexesArgument(vertexes);

            Normal = normal;
            Vertexes = new Vertex[3];
            Array.Copy(vertexes, Vertexes, vertexes.Length);
        }

        public static Vector4 CalculateNormal(in Vertex[] vertexes)
        {
            CheckVertexesArgument(vertexes);

            var AB = new Vector3(vertexes[1].Position - vertexes[0].Position);
            var AC = new Vector3(vertexes[2].Position - vertexes[0].Position);

            return new Vector4(Vector3.Cross(AB, AC).Normalized(), 1.0f);
        }

        private static void CheckVertexesArgument(in Vertex[] vertexes)
        {
            if (vertexes == null)
            {
                throw new ArgumentException("Argument \"vertexes\" can not be null.");
            }
            
            if (vertexes.Length != VertexesAmount)
            {
                throw new ArgumentException($"Triangle vertexes amount must be {VertexesAmount}.");
            }
        }
    }
}