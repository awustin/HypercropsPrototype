using UnityEngine;
using TMPro;

namespace Assets.Hypercrops.Gameplay
{
    public class HexaGridGenerator : MonoBehaviour
    {
        // TODO: Create Hexagonal grid.
        // * it should support multiple layers on top: environment, map entities, colliders and interaction
        // * it should be "paginated" (only load cells within the screen, unload the rest)
        // * as the camera moves vertically, load more in the screen and unload what's hidden
        public GameObject GridObject;
        public GameObject CellObject;
        public float LongDiameter = 12f;
        public float Gap = 0.05f;
        public int MaxColCount = 7;
        public int MaxRowCount = 22;

        public void InitialiseGrid()
        {
            int colCount;
            int rowCount = 0;

            for (int row = 0; rowCount < MaxRowCount; row++)
            {
                GameObject rowObject = GetRowObject(row);
                float y = GetYAtRow(row);

                colCount = 0;

                for (int col = 0; colCount < MaxColCount; col++)
                {
                    GameObject cell = Instantiate(CellObject, new(GetXAtCol(row, col), 0, 0), Quaternion.identity, rowObject.transform);

                    cell.name = $"Cell {row}:{col}";
                    cell.transform.Find("Visuals/Canvas/Label").gameObject.GetComponent<TMP_Text>().text = $"{row}:{col}";

                    colCount++;
                }

                rowObject.transform.position = new(0, 0, y);
                rowCount++;
            }

            GridObject.transform.position = new(-Mathf.Sqrt(3)/2 * 6 * LongDiameter/2, 0, -40);
        }

        private GameObject GetRowObject(int row)
        {
            GameObject rowObject;
            Transform rowTransform = GridObject.transform.Find($"Row{row}");

            if (rowTransform == null)
            {
                rowObject = new GameObject($"Row{row}");
                GridObject.AddChild(rowObject);
            }
            else
            {
                rowObject = rowTransform.gameObject;
            }

            return rowObject;
        }

        private float GetXAtCol(int row, int col)
        {
            float apothem = Mathf.Sqrt(3) * LongDiameter / 4;
            float offset = apothem;

            if (row % 2 == 0)
                offset = 0;

            return (2 * apothem + Gap) * col + offset;
        }

        private float GetYAtRow(int row)
        {
            return (1.5f * LongDiameter / 2 + Gap) * row;
        }
    }
}
