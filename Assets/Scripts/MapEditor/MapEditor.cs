using System;
using System.Collections.Generic;
using Crawler;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Crawler.MapEditor
{
    public class MapEditor : MonoBehaviour
    {
        public GameObject pointer;
        public MapEditingMode mode;
        public int brushSize = 1;
        public TileGroup group;
        public TileType type;
        public LayerMask layer;
        public TileInfoWindow tileInfoWindow;

        private MapEditorContext context;
        private Vector3Int currentCoordinate;
        [SerializeField] private float currentRotation;
        private TileController selectedTile;
        private MapEditorCommand currentCommand;
        private MapEditorCommand[] commands;
        private bool overUi;

        public MapEditorContext Context { get => context; set => context = value; }
        public MapEditorCommand CurrentCommand { get => currentCommand; set => currentCommand = value; }
        public Vector3Int CurrentCoordinate { get => currentCoordinate; set => currentCoordinate = value; }
        public MapEditorCommand[] Commands { get => commands; set => commands = value; }
        public float CurrentRotation { get => currentRotation; set => currentRotation = value; }
        public TileController SelectedTile { get => selectedTile; set => selectedTile = value; }

        public void Init(MapEditorContext context)
        {
            this.context = context;
            currentCoordinate = Vector3Int.zero;

            commands = new MapEditorCommand[3]
            {
                new DrawCommand("Draw"),
                new EraseCommand("Erase"),
                new SelectCommand("Select")
            };

            currentCommand = commands[1];
        }

        void Update()
        {
            Raycast();
            Rotate();
            Execute();
            BrushSizeHandler();
        }

        public void Rotate()
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                pointer.transform.Rotate(Vector3.up, 90);
                currentRotation = pointer.transform.rotation.eulerAngles.y;
            }
            else if(Input.GetKeyDown(KeyCode.E))
            {
                pointer.transform.Rotate(Vector3.up, -90);
                currentRotation = pointer.transform.rotation.eulerAngles.y;
            }
        }

        public void Raycast()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            overUi = EventSystem.current.IsPointerOverGameObject();
            
            if(Physics.Raycast(ray, out hit, 100f, layer) && !overUi)
            {
                Vector3 intersectionPoint = hit.point;
                Vector3Int nearestPoint = new Vector3Int(
                    Mathf.RoundToInt(intersectionPoint.x),
                    0,
                    Mathf.RoundToInt(intersectionPoint.z));

                currentCoordinate = nearestPoint / 3;
                pointer.GetComponent<MapEditorPointer>().MovePointer(nearestPoint);
            }
        }

        public void Execute()
        {
            if(Input.GetMouseButton(0) && !overUi)
            {
                currentCommand.Execute(this);
            }
        }

        public void DrawFeature(int featureId)
        {
            if(selectedTile != null)
            {
                selectedTile.Tile.Feature = (ETileFeature) featureId;
                selectedTile.Draw();
            }
        }

        public void SetCommand(int index)
        {
            currentCommand = commands[index];
        }

        public void BrushSizeHandler()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            if (scroll > 0f)
            {
                brushSize = Mathf.Clamp(brushSize + 1, 1, 3);
            }
            else if (scroll < 0f)
            {
                brushSize = Mathf.Clamp(brushSize - 1, 1, 3);
            }
        }

        public void ChangeType(int id)
        {
            type = (TileType) id;
        }

        public void ChangeGroup(int id)
        {
            group = (TileGroup) id;
        }
    }

    public enum MapEditingMode
    {
        Draw, Erase, Select
    }
}