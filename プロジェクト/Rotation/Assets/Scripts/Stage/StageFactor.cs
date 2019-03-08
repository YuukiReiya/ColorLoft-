using UnityEngine;

/// <summary>
/// ステージ生成スクリプト
/// </summary>
public class StageFactor : MonoBehaviour {

    [SerializeField] GameObject copyObject;
    [SerializeField] Vector3 leftBottomPos;
    [SerializeField] Vector2 offset;//scale
    [SerializeField] Vector2 limit;
    [SerializeField] GameObject resetZone;

    [ContextMenu("CreateStage")]
    private void CreateStage()
    {
        var parentObject = new GameObject();
        parentObject.name = "new Stage";
        parentObject.AddComponent<StageManager>();

        for (int i = 0; i < limit.x; ++i)
        {
            for (int j = 0; j < limit.y; ++j)
            {

                var inst = Instantiate(copyObject);
                inst.transform.parent = parentObject.transform;
                inst.transform.localPosition = Vector3.zero;

                Vector3 pos = new Vector3(
                    leftBottomPos.x + offset.x * i,
                    leftBottomPos.y,
                    leftBottomPos.z + offset.y * j

                );
                inst.transform.localPosition = pos;
            }
        }

        var reset = Instantiate(resetZone);
        reset.transform.parent = parentObject.transform;
        
    }
}
