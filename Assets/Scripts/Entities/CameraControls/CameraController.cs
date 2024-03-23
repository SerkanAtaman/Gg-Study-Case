using GG.TileMapSystem;
using UnityEngine;

namespace GG.Entities.CameraControls
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private OnTileMapCreated _onTileMapCreated;

        public Camera Camera
        {
            get
            {
                if(_cam == null)
                {
                    _cam = transform.GetComponent<Camera>();
                }

                return _cam;
            }
        }
        private Camera _cam;

        private const float MIN_VIEWPORT_WITDH_REQUIRED = 56.25f;

        private void OnEnable()
        {
            _onTileMapCreated.RegisterListener<TileMap>(SetCameraPosition);
        }

        private void Start()
        {
            SetCamOrtoSize();
        }

        private void OnDisable()
        {
            _onTileMapCreated.RemoveListener<TileMap>(SetCameraPosition);
        }

        private void SetCamOrtoSize()
        {
            var cameraTopRightPos = Camera.ViewportToWorldPoint(new Vector3(1, 1, 10));
            var cameraBottomLeftPos = Camera.ViewportToWorldPoint(new Vector3(0, 0, 10));

            var currentWidth = cameraTopRightPos.x - cameraBottomLeftPos.x;

            if (currentWidth >= MIN_VIEWPORT_WITDH_REQUIRED) return;

            Camera.orthographicSize += 1;

            cameraTopRightPos = Camera.ViewportToWorldPoint(new Vector3(1, 1, 10));
            cameraBottomLeftPos = Camera.ViewportToWorldPoint(new Vector3(0, 0, 10));

            var withChangePerSize = (cameraTopRightPos.x - cameraBottomLeftPos.x) - currentWidth;
            var requiredSizeChange = (MIN_VIEWPORT_WITDH_REQUIRED - currentWidth) / withChangePerSize;

            Camera.orthographicSize += requiredSizeChange - 1;
        }

        private void SetCameraPosition(TileMap tileMap)
        {
            transform.position = tileMap.MapRect.Center - new Vector3(0, 0, 20);
        }
    }
}