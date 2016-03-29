/*
 Created by Juan Sebastian Munoz Arango
 all rights reserved
 naruse@gmail.com
 */
using System;
using UnityEngine;

public static class ProPivotModifier
{

    private static bool VertexOnWrongPos(Vector3 vert)
    {
        return (vert.x < 0 || vert.y < 0 || vert.z < 0);
    }

    public static void CenterPivot(Mesh meshToCenter, Transform referencePivot)
    {
        Vector3[] oldVertices = meshToCenter.vertices;
        Vector3[] newVertices = new Vector3[oldVertices.Length];

        Vector3 smallestPosTransformed = referencePivot.transform.TransformPoint(oldVertices[0]);

        Vector3 convertedTransformPoint = referencePivot.transform.TransformPoint(oldVertices[0]);

        Vector3 smallestPos = oldVertices[0];
        Vector3 biggestPos = oldVertices[0];
        for (int i = 0; i < oldVertices.Length; i++)
        {
            convertedTransformPoint = referencePivot.transform.TransformPoint(oldVertices[i]);

            smallestPosTransformed = new Vector3(convertedTransformPoint.x < smallestPosTransformed.x ? convertedTransformPoint.x : smallestPosTransformed.x,
                                                 convertedTransformPoint.y < smallestPosTransformed.y ? convertedTransformPoint.y : smallestPosTransformed.y,
                                                 convertedTransformPoint.z < smallestPosTransformed.z ? convertedTransformPoint.z : smallestPosTransformed.z);

            smallestPos = new Vector3(oldVertices[i].x < smallestPos.x ? oldVertices[i].x : smallestPos.x,
                            oldVertices[i].y < smallestPos.y ? oldVertices[i].y : smallestPos.y,
                            oldVertices[i].z < smallestPos.z ? oldVertices[i].z : smallestPos.z);
            biggestPos = new Vector3(oldVertices[i].x > biggestPos.x ? oldVertices[i].x : biggestPos.x,
                            oldVertices[i].y > biggestPos.y ? oldVertices[i].y : biggestPos.y,
                            oldVertices[i].z > biggestPos.z ? oldVertices[i].z : biggestPos.z);
        }
        Vector3 center = (biggestPos - smallestPos) / 2;
        bool meshCorrectlyOrganized = true;
        for (int i = 0; i < oldVertices.Length; i++)
        {
            newVertices[i] = oldVertices[i] - referencePivot.transform.InverseTransformPoint(smallestPosTransformed);
            if (VertexOnWrongPos(newVertices[i]))
            {
                meshCorrectlyOrganized = false;
            }
        }
        if (!meshCorrectlyOrganized)
        {//organize vertexes tu put them on the X > 0, Y > 0, Z > 0 space.
         //Debug.Log("Wrong sided mesh!, no worries reorganizing it for you ;)...");
            newVertices = ReOrganizeVertices(newVertices);
        }

        for (int i = 0; i < newVertices.Length; i++)
            newVertices[i] -= center;

        meshToCenter.vertices = newVertices;
    }

    //reorganize al vertices to be on the space X > 0, Y > 0, Z > 0
    private static Vector3[] ReOrganizeVertices(Vector3[] verts)
    {
        Vector3 organizedOffset = verts[0];
        for (int i = 0; i < verts.Length; i++)
        {
            organizedOffset = new Vector3(verts[i].x < organizedOffset.x ? verts[i].x : organizedOffset.x,
                                          verts[i].y < organizedOffset.y ? verts[i].y : organizedOffset.y,
                                          verts[i].z < organizedOffset.z ? verts[i].z : organizedOffset.z);
        }
        //Debug.Log("Reorganizing offset for displacement... almost there");
        organizedOffset = new Vector3(organizedOffset.x < 0 ? Mathf.Abs(organizedOffset.x) : 0,
                                      organizedOffset.y < 0 ? Mathf.Abs(organizedOffset.y) : 0,
                                      organizedOffset.z < 0 ? Mathf.Abs(organizedOffset.z) : 0);

        //Debug.Log("Organized Offset: " + organizedOffset);
        Vector3[] organizedVector = verts;
        for (int i = 0; i < organizedVector.Length; i++)
            organizedVector[i] += organizedOffset;
        return organizedVector;
    }

    public static void AverageCenterPivot(Mesh meshToCenter, Vector3 initialPivotPos)
    {
        Vector3[] oldVertices = meshToCenter.vertices;
        Vector3[] newVertices = new Vector3[oldVertices.Length];

        Vector3 avg = Vector3.zero;
        for (int i = 0; i < oldVertices.Length; i++)
        {
            avg += initialPivotPos + oldVertices[i];
        }
        avg = new Vector3(avg.x / oldVertices.Length, avg.y / oldVertices.Length, avg.z / oldVertices.Length);
        for (int i = 0; i < oldVertices.Length; i++)
        {
            newVertices[i] = oldVertices[i] + initialPivotPos - avg;
        }
        meshToCenter.vertices = newVertices;
    }

    public static void MoveSelectedMesh(Mesh meshToMove, Vector3 displacement)
    {

        Vector3[] oldVertices = meshToMove.vertices;
        Vector3[] newVertices = new Vector3[oldVertices.Length];

        for (int i = 0; i < oldVertices.Length; i++)
        {
            newVertices[i] = oldVertices[i] + displacement;
        }
        meshToMove.vertices = newVertices;
    }

    public static void RotateSelectedMesh(Mesh meshToRotate, Vector3 angle)
    {
        Vector3[] oldVertices = meshToRotate.vertices;
        Vector3[] newVertices = new Vector3[oldVertices.Length];

        Vector3[] oldNormals = meshToRotate.normals;
        Vector3[] newNormals = new Vector3[oldNormals.Length];

        for (int i = 0; i < oldVertices.Length; i++)
        {
            newVertices[i] = new Vector3((oldVertices[i].x * Mathf.Cos(Mathf.Deg2Rad * angle.y) + oldVertices[i].z * Mathf.Sin(Mathf.Deg2Rad * angle.y)) * (angle.y != 0 ? 1 : 0) + /*Y*/
                                         oldVertices[i].x * (angle.x != 0 ? 1 : 0) +                                                                                      /*X*/
                                         (oldVertices[i].x * Mathf.Cos(Mathf.Deg2Rad * angle.z) + oldVertices[i].y * Mathf.Sin(Mathf.Deg2Rad * angle.z)) * (angle.z != 0 ? 1 : 0)   /*Z*/,

                                         oldVertices[i].y * (angle.y != 0 ? 1 : 0) +                                                                                      /*Y*/
                                         (oldVertices[i].y * Mathf.Cos(Mathf.Deg2Rad * angle.x) + oldVertices[i].z * Mathf.Sin(Mathf.Deg2Rad * angle.x)) * (angle.x != 0 ? 1 : 0) + /*X*/
                                         (-oldVertices[i].x * Mathf.Sin(Mathf.Deg2Rad * angle.z) + oldVertices[i].y * Mathf.Cos(Mathf.Deg2Rad * angle.z)) * (angle.z != 0 ? 1 : 0)  /*Z*/,

                                         (-oldVertices[i].x * Mathf.Sin(Mathf.Deg2Rad * angle.y) + oldVertices[i].z * Mathf.Cos(Mathf.Deg2Rad * angle.y)) * (angle.y != 0 ? 1 : 0) + /*Y*/
                                         (-oldVertices[i].y * Mathf.Sin(Mathf.Deg2Rad * angle.x) + oldVertices[i].z * Mathf.Cos(Mathf.Deg2Rad * angle.x)) * (angle.x != 0 ? 1 : 0) + /*X*/
                                         oldVertices[i].z * (angle.z != 0 ? 1 : 0));                                                                                       /*Z*/

            newNormals[i] = new Vector3((oldNormals[i].x * Mathf.Cos(Mathf.Deg2Rad * angle.y) + oldNormals[i].z * Mathf.Sin(Mathf.Deg2Rad * angle.y)) * (angle.y != 0 ? 1 : 0) + /*Y*/
                                         oldNormals[i].x * (angle.x != 0 ? 1 : 0) +                                                                                    /*X*/
                                         (oldNormals[i].x * Mathf.Cos(Mathf.Deg2Rad * angle.z) + oldNormals[i].y * Mathf.Sin(Mathf.Deg2Rad * angle.z)) * (angle.z != 0 ? 1 : 0)  /*Z*/,

                                         oldNormals[i].y * (angle.y != 0 ? 1 : 0) +                                                                                     /*Y*/
                                         (oldNormals[i].y * Mathf.Cos(Mathf.Deg2Rad * angle.x) + oldNormals[i].z * Mathf.Sin(Mathf.Deg2Rad * angle.x)) * (angle.x != 0 ? 1 : 0) + /*X*/
                                         (-oldNormals[i].x * Mathf.Sin(Mathf.Deg2Rad * angle.z) + oldNormals[i].y * Mathf.Cos(Mathf.Deg2Rad * angle.z)) * (angle.z != 0 ? 1 : 0)  /*Z*/,

                                         (-oldNormals[i].x * Mathf.Sin(Mathf.Deg2Rad * angle.y) + oldNormals[i].z * Mathf.Cos(Mathf.Deg2Rad * angle.y)) * (angle.y != 0 ? 1 : 0) + /*Y*/
                                         (-oldNormals[i].y * Mathf.Sin(Mathf.Deg2Rad * angle.x) + oldNormals[i].z * Mathf.Cos(Mathf.Deg2Rad * angle.x)) * (angle.x != 0 ? 1 : 0) + /*X*/
                                         oldNormals[i].z * (angle.z != 0 ? 1 : 0));                                                                                      /*Z*/

        }
        meshToRotate.vertices = newVertices;
        meshToRotate.normals = newNormals;
    }
}

