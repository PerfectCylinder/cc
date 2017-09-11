using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawLine : MonoBehaviour
{
    //线段预制
    [Tooltip("Line renderer used for the line drawing.")]
    public LineRenderer linePrefab;

    //线段相关保存和下标
    private List<GameObject> linesDrawn = new List<GameObject>();
    private LineRenderer currentLine;
    private int lineVertexIndex = 2;

    void Update()
    {
        //删除最近一笔2333
        if (Input.GetKeyDown(KeyCode.U))
        {
            // U-key means Undo
            DeleteLastLine();
        }

        if (currentLine == null &&
           Input.GetMouseButton(0))
        {
            // 鼠标按下，开始画线
            currentLine = Instantiate(linePrefab).GetComponent<LineRenderer>();
            currentLine.name = "Line" + linesDrawn.Count;
            currentLine.transform.parent = transform;

            Vector3 cursorPos = Input.mousePosition;
            cursorPos.z = 0f;

            //将鼠标按下的屏幕坐标转换成世界坐标
            Vector3 cursorSpacePos = Camera.main.ScreenToWorldPoint(cursorPos);
            cursorSpacePos.z = 0f;
            currentLine.SetPosition(0, cursorSpacePos);
            currentLine.SetPosition(1, cursorSpacePos);

            lineVertexIndex = 2;
            linesDrawn.Add(currentLine.gameObject);

            StartCoroutine(DrawLines());
        }

        if (currentLine != null &&
            Input.GetMouseButtonUp(0))
        {
            // 鼠标左键抬起结束当前笔画
            currentLine = null;
        }
    }

    //撤销最后一笔
    public void DeleteLastLine()
    {
        if (linesDrawn.Count > 0)
        {
            GameObject goLastLine = linesDrawn[linesDrawn.Count - 1];
            linesDrawn.RemoveAt(linesDrawn.Count - 1);
            Destroy(goLastLine);
        }
    }

    //持续画线
    IEnumerator DrawLines()
    {
        while (Input.GetMouseButton(0))
        {
            yield return new WaitForEndOfFrame();

            if (currentLine != null)
            {
                lineVertexIndex++;
                currentLine.SetVertexCount(lineVertexIndex);

                Vector3 cursorPos = Input.mousePosition;
                cursorPos.z = 0f;

                Vector3 cursorSpacePos = Camera.main.ScreenToWorldPoint(cursorPos);
                cursorSpacePos.z = 0f;
                currentLine.SetPosition(lineVertexIndex - 1, cursorSpacePos);
            }
        }
    }
}
