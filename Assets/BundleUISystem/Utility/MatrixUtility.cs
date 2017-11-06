using UnityEngine;
using System.Collections;

namespace BundleUISystem
{
    public static class MatrixUtility
    {
        public static void LoadmatrixInfo(Matrix4x4 materix, Transform transform)
        {
            transform.position = materix.GetColumn(0);
            transform.eulerAngles = materix.GetColumn(1);
            transform.localScale = materix.GetColumn(2);
        }
    }
}