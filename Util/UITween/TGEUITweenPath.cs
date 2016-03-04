namespace TGE.Widget
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;
    
    public class TGEUITweenPath : TGEUITween
    {
        private RectTransform selfTransform;
        protected override void Awake()
        {
            base.Awake();
            selfTransform = transform as RectTransform;
        }

        protected override void UpdateTween(float _radio)
        {
            selfTransform.transform.position = nodes[0] + (nodes[1] - nodes[0]) * _radio;
        }

        public Color pathColor = Color.cyan;
        public List<Vector3> nodes = new List<Vector3>() { Vector3.zero, Vector3.zero };
        public int nodeCount;
        public bool initialized = false;

        public void OnDrawGizmosSelected()
        {
            if (enabled)
            {
                if (nodes.Count > 0)
                {
                    DrawPath(nodes.ToArray(), pathColor);
                }
            }
        }

        public void OnDrawGizmos()
        {

        }
        public static void DrawPath(Vector3[] path, Color color)
        {
            if (path.Length > 0)
            {
                Vector3[] vector3s = PathControlPointGenerator(path);

                Vector3 prevPt = Interp(vector3s, 0);
                Gizmos.color = color;
                int SmoothAmount = path.Length * 20;
                for (int i = 1; i <= SmoothAmount; i++)
                {
                    float pm = (float)i / SmoothAmount;
                    Vector3 currPt = Interp(vector3s, pm);
                    Gizmos.DrawLine(currPt, prevPt);

                    prevPt = currPt;
                }
            }
        }
        private static Vector3 Interp(Vector3[] pts, float t)
        {
            int numSections = pts.Length - 3;
            int currPt = Mathf.Min(Mathf.FloorToInt(t * (float)numSections), numSections - 1);
            float u = t * (float)numSections - (float)currPt;

            Vector3 a = pts[currPt];
            Vector3 b = pts[currPt + 1];
            Vector3 c = pts[currPt + 2];
            Vector3 d = pts[currPt + 3];

            return .5f * (
                (-a + 3f * b - 3f * c + d) * (u * u * u)
                + (2f * a - 5f * b + 4f * c - d) * (u * u)
                + (-a + c) * u
                + 2f * b
            );
        }
        private static Vector3[] PathControlPointGenerator(Vector3[] path)
        {
            Vector3[] suppliedPath;
            Vector3[] vector3s;

            //create and store path points:
            suppliedPath = path;

            //populate calculate path;
            int offset = 2;
            vector3s = new Vector3[suppliedPath.Length + offset];
            Array.Copy(suppliedPath, 0, vector3s, 1, suppliedPath.Length);

            //populate start and end control points:
            //vector3s[0] = vector3s[1] - vector3s[2];
            vector3s[0] = vector3s[1] + (vector3s[1] - vector3s[2]);
            vector3s[vector3s.Length - 1] = vector3s[vector3s.Length - 2] + (vector3s[vector3s.Length - 2] - vector3s[vector3s.Length - 3]);

            //is this a closed, continuous loop? yes? well then so let's make a continuous Catmull-Rom spline!
            if (vector3s[1] == vector3s[vector3s.Length - 2])
            {
                Vector3[] tmpLoopSpline = new Vector3[vector3s.Length];
                Array.Copy(vector3s, tmpLoopSpline, vector3s.Length);
                tmpLoopSpline[0] = tmpLoopSpline[tmpLoopSpline.Length - 3];
                tmpLoopSpline[tmpLoopSpline.Length - 1] = tmpLoopSpline[2];
                vector3s = new Vector3[tmpLoopSpline.Length];
                Array.Copy(tmpLoopSpline, vector3s, tmpLoopSpline.Length);
            }

            return (vector3s);
        }
    }
}