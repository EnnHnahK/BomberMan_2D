using UnityEngine;
public interface IMapProcessing
{
    void InitializeGridHandler();
    void FillGrid(Vector2Int _pos, GameObject cell);
    void EmptyGrid(Vector2Int _pos);
    GameObject GetCellAtPosition(Vector2Int _pos);
}

public class MapProcessing : IMapProcessing
{
    private IMapGenerator mapGenerator;
    public void InitializeGridHandler()
    {
        mapGenerator = ServiceLocator.GetService<IMapGenerator>();
    }
    public void EmptyGrid(Vector2Int _pos)
    {
        mapGenerator.mapCells[_pos.x, _pos.y] = null;
    }
    public void FillGrid(Vector2Int _pos, GameObject cell)
    {
        if (mapGenerator.mapCells[_pos.x, _pos.y] == null)
            mapGenerator.mapCells[_pos.x, _pos.y] = cell;
    }

    public GameObject GetCellAtPosition(Vector2Int _pos)
    {
        GameObject gameObject = null;
        if (mapGenerator.mapCells[_pos.x, _pos.y]) { 
            gameObject = mapGenerator.mapCells[_pos.x, _pos.y];
            GameObject tempObject = GameObject.FindWithTag("Floor");
            if (gameObject.tag == tempObject.tag) {
                return null;
            }
        }


        return gameObject;
    }

}
