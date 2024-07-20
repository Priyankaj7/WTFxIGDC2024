using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using UnityEngine;
// using UnityEditor;

namespace Minimalist.MapBuilder
{
    // [ExecuteInEditMode]
    public class MapBuilderRuntime : MonoBehaviour
    {
        // Public properties
        public Transform[] GridTransforms
        {
            get
            {
                return _gridTransform.GetComponents<Transform>();
            }
        }
        public bool DoneInstantiating { get; set; }
        // public bool EditState { get; set; }
        public Vector3 HoveredPosition { get; set; }
        public List<Vector3> SelectedPositions { get; set; }
        public bool Dragging { get; set; }

        // Public fields
        [Header("Base:")]
        public EditModeInstanceBhv basePrefab;
        public Color baseColor = Color.black;
        public Vector3Int baseScale = new Vector3Int(30, 1, 30);

        [Header("Tiles:")]
        public EditModeInstanceBhv tilePrefab;
        public Color tileColor = Color.white;
        public Vector3Int tileScale = new Vector3Int(1, 1, 1);

        // Private fields
        private Transform _transform;

        private Transform _baseTransform;
        private EditModeInstanceBhv _baseEditModeInstance;
        private EditModeInstanceBhv _dragOnsetTileInstance;
        private Renderer _baseRenderer;
        private Collider _baseCollider;
        private Material _baseMaterial;

        private Transform _gridTransform;
        private MeshFilter _gridMeshFilter;
        private List<Vector3> _gridVertices;

        private Transform _tileParentTransform;
        private Material _tileMaterial;
        private Vector3 _tileHalfHeight;

        private Vector3 _offset;
        private bool _busy;

        private void Awake()
        {
// #if UNITY_EDITOR
//             if (PrefabUtility.IsPartOfAnyPrefab(this.gameObject))
//             {
//                 PrefabUtility.UnpackPrefabInstance(this.gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
//             }
// #endif
        }

        // private void OnValidate()
        // {
        //     if (_gridTransform != null || _baseTransform != null && _baseTransform.localScale.y > baseScale.y)
        //     {
        //         this.Update();
        //     }
        // }

        private void Update()
        {
            if (_busy)
            {
                return;
            }

            _busy = true;
             OnSceneGUI();
            this.PseudoAwake();

            this.ValidateBaseScale();

            this.UpdateBase();

            this.UpdateGrid();

            this.UpdateTileParent();

            this.ValidateTileScale();

            this.UpdateGridMesh();

            this.DoneInstantiating = true;

            _busy = false;
        }
        // public GameObject g;

        private void PseudoAwake()
        {
            _transform = this.GetComponent<Transform>();

            _gridTransform = _transform.Find("Grid");

            if (_gridTransform == null)
            {
                this.InstantiateGrid();
            }

            _gridTransform.hideFlags = HideFlags.NotEditable;

            _gridMeshFilter = _gridTransform.GetComponent<MeshFilter>();

            // for(int i =0; i< _gridMeshFilter.sharedMesh.vertices.Length;i++){
            //     Instantiate(g,_gridMeshFilter.sharedMesh.vertices[i],Quaternion.identity);
            // }

            if (_gridMeshFilter.sharedMesh == null)
            {
                _gridMeshFilter.sharedMesh = new Mesh() { name = "Grid" };
            }

            _baseTransform = _transform.Find("Base");

            if (_baseTransform == null)
            {
                this.InstantiateBase();
            }

            _baseEditModeInstance = _baseTransform.GetComponent<EditModeInstanceBhv>();

            _baseRenderer = _baseTransform.GetComponent<Renderer>();

            _baseCollider = _baseTransform.GetComponent<Collider>();

            _tileParentTransform = _transform.Find("Tiles");

            if (_tileParentTransform == null)
            {
                this.InstantiateTileParent();
            }

            _baseMaterial = basePrefab.GetComponent<Renderer>().sharedMaterial;

            _tileMaterial = tilePrefab.GetComponent<Renderer>().sharedMaterial;

            if (this.SelectedPositions == null)
            {
                this.SelectedPositions = new List<Vector3>();
            }

            _offset = _baseTransform.position + new Vector3(0f,-0.01f,0f);
        }

        private void InstantiateGrid()
        {
            GameObject gridGameObject = new GameObject("Grid");

            gridGameObject.AddComponent<MeshFilter>();

            gridGameObject.AddComponent<MeshRenderer>();

            _gridTransform = gridGameObject.transform;

            _gridTransform.parent = _transform;
        }

        private void InstantiateBase()
        {
            basePrefab.scale = baseScale;

            basePrefab.material = _baseMaterial;

            basePrefab.color = baseColor;

            _baseEditModeInstance = Instantiate(basePrefab, Vector3.zero, Quaternion.identity, _transform);

            _baseTransform = _baseEditModeInstance.GetComponent<Transform>();

            _baseTransform.name = "Base";
        }

        private void InstantiateTileParent()
        {
            GameObject tileParentGameObject = new GameObject("Tiles");

            _tileParentTransform = tileParentGameObject.transform;

            _tileParentTransform.parent = _transform;
        }

        private void ValidateBaseScale()
        {
            int x = (int)Mathf.Clamp(baseScale.x, 1, 50);
            int y = (int)Mathf.Clamp(baseScale.y, 1, 50);
            int z = (int)Mathf.Clamp(baseScale.z, 1, 50);

            baseScale = new Vector3Int(x, y, z);
        }

        private void UpdateBase()
        {
            _baseEditModeInstance.scale = baseScale;

            _baseEditModeInstance.color = baseColor;

            _baseTransform.localScale = _baseEditModeInstance.scale;

            _baseRenderer.material = new Material(_baseMaterial) { color = baseColor };
        }

        private void UpdateGrid()
        {
            _gridTransform.position = _baseTransform.position;
        }

        private void UpdateTileParent()
        {
            _tileParentTransform.position = _offset + Vector3.up * baseScale.y / 2f;
        }

        private void ValidateTileScale()
        {
            int x = (int)Mathf.Clamp(tileScale.x, 1, baseScale.x);
            int y = (int)Mathf.Clamp(tileScale.y, 1, Mathf.Infinity);
            int z = (int)Mathf.Clamp(tileScale.z, 1, baseScale.z);

            tileScale = new Vector3Int(x, y, z);

            _tileHalfHeight = Vector3.up * tileScale.y / 2f;
        }

        // public void UpdateGridMesh()
        // {
        //     int xSize = baseScale.x - (int)(tileScale.x - 1);
        //     int zSize = baseScale.z - (int)(tileScale.z - 1);

        //     List<Vector3> allVertices = new List<Vector3>();

        //     for (int i = 0; i < zSize; i++)
        //     {
        //         for (int j = 0; j < xSize; j++)
        //         {
        //             float x = j - baseScale.x / 2f + tileScale.x / 2f;
        //             float z = i - baseScale.z / 2f + tileScale.z / 2f;

        //             Vector3 rayOrigin = new Vector3(x, int.MaxValue, z) + _offset;

        //             RaycastHit[] hits = Physics.RaycastAll(rayOrigin, Vector3.down, Mathf.Infinity);

        //             foreach (RaycastHit hit in hits)
        //             {
        //                 if (hit.point.y < _offset.y)
        //                 {
        //                     continue;
        //                 }

        //                 float y = hit.point.y - _offset.y;

        //                 Vector3 vertex = new Vector3(x, y, z);

        //                 allVertices.Add(vertex);
        //             }
        //         }
        //     }

        //     _gridVertices = new List<Vector3>(allVertices);

        //     foreach (Vector3 vertex in allVertices)
        //     {
        //         bool isValid = true;

        //         Vector3 globalVertex = vertex + _offset + _tileHalfHeight;

        //         Bounds vertexBounds = new Bounds(globalVertex, (Vector3)tileScale * .95f);

        //         Collider[] placedCollidersInReach = Physics.OverlapBox(globalVertex, (Vector3)tileScale / 2f);

        //         foreach (Collider placedTileCollider in placedCollidersInReach)
        //         {
        //             if (placedTileCollider == _baseCollider)
        //             {
        //                 continue;
        //             }

        //             if (placedTileCollider.bounds.Intersects(vertexBounds))
        //             {
        //                 isValid = false;

        //                 break;
        //             }
        //         }

        //         if (!isValid)
        //         {
        //             _gridVertices.Remove(vertex);

        //             continue;
        //         }

        //         foreach (Vector3 selectedPosition in this.SelectedPositions)
        //         {
        //             Bounds selectedBounds = new Bounds(selectedPosition + _tileHalfHeight, (Vector3)tileScale * 0.95f);

        //             if (selectedBounds.Intersects(vertexBounds))
        //             {
        //                 isValid = false;

        //                 break;
        //             }
        //         }

        //         if (!isValid)
        //         {
        //             _gridVertices.Remove(vertex);
        //         }
        //     }

        //     _gridMeshFilter.sharedMesh.Clear();

        //     _gridMeshFilter.sharedMesh.vertices = _gridVertices.ToArray();
        // }

        public void UpdateGridMesh()
        {
            int xSize = baseScale.x - (int)(tileScale.x - 1);
            int zSize = baseScale.z - (int)(tileScale.z - 1);

            List<Vector3> allVertices = new List<Vector3>();

            for (int i = 0; i < zSize; i+=2)
            {
                for (int j = 0; j < xSize; j+=2)
                {
                    float x = j - baseScale.x / 2f + tileScale.x / 2f;
                    float z = i - baseScale.z / 2f + tileScale.z / 2f;

                    Vector3 rayOrigin = new Vector3(x, int.MaxValue, z) + _offset;

                    RaycastHit[] hits = Physics.RaycastAll(rayOrigin, Vector3.down, Mathf.Infinity);

                    foreach (RaycastHit hit in hits)
                    {
                        if (hit.point.y < _offset.y)
                        {
                            continue;
                        }

                        float y = hit.point.y - _offset.y;

                        Vector3 vertex = new Vector3(x, y, z);

                        allVertices.Add(vertex);
                    }
                }
            }

            _gridVertices = new List<Vector3>(allVertices);
            

            foreach (Vector3 vertex in allVertices)
            {
                bool isValid = true;

                Vector3 globalVertex = vertex + _offset + _tileHalfHeight;

                Bounds vertexBounds = new Bounds(globalVertex, (Vector3)tileScale * .95f);

                Collider[] placedCollidersInReach = Physics.OverlapBox(globalVertex, (Vector3)tileScale / 2f);

                foreach (Collider placedTileCollider in placedCollidersInReach)
                {
                    if (placedTileCollider == _baseCollider)
                    {
                        continue;
                    }

                    if (placedTileCollider.bounds.Intersects(vertexBounds))
                    {
                        isValid = false;

                        break;
                    }
                }

                if (!isValid)
                {
                    _gridVertices.Remove(vertex);

                    continue;
                }

                foreach (Vector3 selectedPosition in this.SelectedPositions)
                {
                    Bounds selectedBounds = new Bounds(selectedPosition + _tileHalfHeight, (Vector3)tileScale * 0.95f);

                    if (selectedBounds.Intersects(vertexBounds))
                    {
                        isValid = false;

                        break;
                    }
                }

                if (!isValid)
                {
                    _gridVertices.Remove(vertex);
                }
            }

            _gridMeshFilter.sharedMesh.Clear();
            // Debug.Log("grid vertices count "+ _gridVertices.Count);  
            _gridMeshFilter.sharedMesh.vertices = _gridVertices.ToArray();
        }

        // private void OnDrawGizmosSelectedABC()
        // {
        //     if ( _baseTransform == null || _tileParentTransform == null)
        //     {
        //         return;
        //     }

        //     Gizmos.color = new Color(tileColor.r, tileColor.g, tileColor.b, .01f);

        //     for (int i = 0; i < _gridVertices.Count; i++)
        //     {
        //         Gizmos.DrawCube(_gridVertices[i] + _offset + _tileHalfHeight, tileScale);

        //         Gizmos.DrawWireCube(_gridVertices[i] + _offset + _tileHalfHeight, tileScale);
        //     }

        //     if (!this.Dragging)
        //     {
        //         Gizmos.color = new Color(tileColor.r, tileColor.g, tileColor.b, .25f);

        //         Gizmos.DrawCube(this.HoveredPosition + _tileHalfHeight, tileScale);

        //         Gizmos.color = new Color(tileColor.r, tileColor.g, tileColor.b, .5f);

        //         Gizmos.DrawWireCube(this.HoveredPosition + _tileHalfHeight, tileScale);
        //     }
        //     else
        //     {
        //         foreach (Vector3 selectedPosition in this.SelectedPositions)
        //         {
        //             Gizmos.color = new Color(tileColor.r, tileColor.g, tileColor.b, .25f);

        //             Gizmos.DrawCube(selectedPosition + _tileHalfHeight, tileScale);

        //             Gizmos.color = new Color(tileColor.r, tileColor.g, tileColor.b, .5f);

        //             Gizmos.DrawWireCube(selectedPosition + _tileHalfHeight, tileScale);
        //         }
        //     }

        //     //Gizmos.color = new Color(1 - baseColor.r, 1 - baseColor.g, 1 - baseColor.b, .5f);

        //     //for (int i = 0; i < _gridVertices.Count; i++)
        //     //{
        //     //    Gizmos.DrawSphere(_gridVertices[i] + _offset + Vector3.up * .05f, .1f);
        //     //}
        // }

        public void InstantiateTile(Vector3 position)
        {
            tilePrefab.scale = tileScale;

            tilePrefab.material = _tileMaterial;

            tilePrefab.color = tileColor;

            Instantiate(tilePrefab, position + _tileHalfHeight, Quaternion.identity, _tileParentTransform);
        }

        public void InstantiateTiles()
        {
            foreach (Vector3 selectedPosition in this.SelectedPositions)
            {
                tilePrefab.scale = tileScale;

                // tilePrefab.material = _tileMaterial;

                // tilePrefab.color = tileColor;

                Instantiate(tilePrefab, selectedPosition, Quaternion.identity, _tileParentTransform);
            }

            this.SelectedPositions.Clear();
        }
        GameObject tempObject;
        public void InstantiateTemproryTiles(Vector3 position)
        {
            // foreach (Vector3 selectedPosition in this.SelectedPositions)
            // {
                tilePrefab.scale = tileScale;

                // tilePrefab.material = _tileMaterial;

                // tilePrefab.color = tileColor;

                tempObject =  Instantiate(tilePrefab, position , Quaternion.identity).gameObject;
                Destroy(tempObject.GetComponent<EditModeInstanceBhv>());
                Destroy(tempObject.GetComponent<BoxCollider>());
            // }

        }
        public void SetTempTilePos(Vector3 position){
            if(tempObject != null){
                tempObject.transform.position = position;
            }

        }
        void DestroyTemproryTile(){
            if(tempObject != null){
                DestroyImmediate(tempObject);
            }
            
        }

        public void DestroyTile(Collider collider)
        {
            if (collider != _baseCollider)
            {
                DestroyImmediate(collider.gameObject);
            }
        }

        public void DestroyAllTiles()
        {
            DestroyImmediate(_tileParentTransform.gameObject);
        }

        private void OnSceneGUI()
        {
            MapBuilderRuntime mapBuilder = this;

            if (mapBuilder == null || !mapBuilder.DoneInstantiating)
            {
                return;
            }

            // int controlId = GUIUtility.GetControlID(FocusType.Passive);

            // HandleUtility.AddDefaultControl(controlId);

            // Event e = Event.current;
            // I
            // if(Input.mo)

            // if (!e.alt && !e.shift && !e.control)
            {
                Vector3 mousePosition = Input.mousePosition;

                // Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(ray, out RaycastHit prehit, Mathf.Infinity);

                // var nearestVertex = prehit.point;
                Vector3 nearestVertex = FindNearestVertex(mapBuilder._gridTransform,prehit.point);
                Debug.Log(nearestVertex.ToString("F4"));

                // HandleUtility.FindNearestVertex(mousePosition, mapBuilder.GridTransforms, out Vector3 nearestVertex);

                mapBuilder.HoveredPosition = nearestVertex;

                Bounds bounds = new Bounds(nearestVertex + Vector3.up * mapBuilder.tileScale.y / 2f, mapBuilder.tileScale);

                Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity);
                // if(hit.transform == _gridTransform){
                //     hit.point;
                // }

                // left mouse button
                MouseEventsNew mEvent;// =  MouseEventsNew.MouseDown;
                if(Input.GetMouseButtonDown(0)){
                    mEvent = MouseEventsNew.MouseDown;
                }
                else if(Input.GetMouseButton(0)){
                    mEvent = MouseEventsNew.MouseDrag;
                }
                else if(Input.GetMouseButtonUp(0)){
                    mEvent = MouseEventsNew.MouseUp;
                }
                else{
                    mEvent = MouseEventsNew.MouseLeaveWindow;
                }

                //if (e.button == 0)
                {
                    switch (mEvent)
                    {
                        case MouseEventsNew.MouseDown:

                            mapBuilder.Dragging = bounds.IntersectRay(ray);
                            if(Dragging){
                                InstantiateTemproryTiles(nearestVertex);
                            }
                            // if (bounds.IntersectRay(ray) && !mapBuilder.SelectedPositions.Contains(nearestVertex))
                            // {
                            //     mapBuilder.SelectedPositions.Add(nearestVertex);

                            //     mapBuilder.UpdateGridMesh();
                            // }

                            goto case MouseEventsNew.MouseDrag;

                        case MouseEventsNew.MouseDrag:

                            if (bounds.IntersectRay(ray) && !mapBuilder.SelectedPositions.Contains(nearestVertex) && nearestVertex.y <0.52f)
                            {
                                SetTempTilePos(nearestVertex);
                                // mapBuilder.SelectedPositions.Add(nearestVertex);

                                // mapBuilder.UpdateGridMesh();
                            }

                            break;

                        case MouseEventsNew.MouseLeaveWindow:

                        case MouseEventsNew.MouseUp:
                            if(Dragging){
                                
                                if (bounds.IntersectRay(ray) && !mapBuilder.SelectedPositions.Contains(nearestVertex) && nearestVertex.y <0.52f)
                                {
                                    mapBuilder.SelectedPositions.Add(nearestVertex);

                                    mapBuilder.UpdateGridMesh();
                                }
                                DestroyTemproryTile();

                            }
                            

                            mapBuilder.InstantiateTiles();

                            mapBuilder.Dragging = false;

                            break;
                    }

                    // InternalEditorUtility.RepaintAllViews();
                }

                // right mouse button
                // else if (e.button == 1 && hit.collider != null)
                // {
                //     switch (e.type)
                //     {
                //         case EventType.MouseDown:

                //             GUIUtility.hotControl = controlId;

                //             _dragOnsetTileInstance = hit.transform.GetComponent<EditModeInstanceBhv>();

                //             break;

                //         case EventType.MouseDrag:

                //         case EventType.MouseUp:

                //             EditModeInstanceBhv tileInstance = hit.transform.GetComponent<EditModeInstanceBhv>();

                //             if (tileInstance == _dragOnsetTileInstance)
                //             {
                //                 mapBuilder.DestroyTile(hit.collider);
                //             }

                //             break;
                //     }

                //     // InternalEditorUtility.RepaintAllViews();
                // }
            }
        }

        Vector3 FindNearestVertex(Transform transform, Vector3 hitPoint){
            Vector3[] vertices = transform.GetComponent<MeshFilter>().sharedMesh.vertices;
            float nearestVertexDistance  = Vector3.Distance( vertices[0],hitPoint);
            int nearestVertexIndex =0;
            for(int i =1; i< vertices.Length; i++){
                float distance = Vector3.Distance(hitPoint,vertices[i]);
                if(distance< nearestVertexDistance){
                    nearestVertexDistance = distance;
                    nearestVertexIndex = i;
                }
            }
            return vertices[nearestVertexIndex];

        }
    }
}
public enum MouseEventsNew{
    MouseDown,
    MouseDrag,
    MouseUp,
    MouseLeaveWindow
}