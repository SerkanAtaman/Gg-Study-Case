using UnityEngine;

namespace GG.TileMapSystem
{
    [System.Serializable]
    public struct TileRect
    {
        public Vector3 Center { get; private set; }
        public Vector3 Extend { get; private set; }

        public TileRect(Vector3 center, Vector3 extend)
        {
            Center = center;
            Extend = extend;
        }

        public Vector3 GetTopCenterPosition()
        {
            return Center + new Vector3(0, Extend.y, 0);
        }
    }
}