using UnityEngine.SceneManagement;
/// <summary>
/// 场景信息
/// </summary>
public class LoadSceneInfo
{
    /// <summary>
    /// 是否保留原场景的东西
    /// </summary>
    public LoadSceneMode mode;
    /// <summary>
    /// 场景名
    /// </summary>
    public ESceneID sceneID;

    /// <summary>
    /// 异步or同步
    /// </summary>
    public LoadSceneType type;

    public LoadSceneInfo(ESceneID sceneID, LoadSceneType type, LoadSceneMode mode)
    {
        this.sceneID = sceneID;
        this.type = type;
        this.mode = mode;
    }
}
