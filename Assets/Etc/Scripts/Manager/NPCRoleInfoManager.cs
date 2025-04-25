using System.Diagnostics;

public class NPCRoleInfoManager
{    
    public string GetPrompt(string npcName) => GetNPCRoleByName(npcName)?.prompt;

    /// <summary>
    /// string name 받는 로직 수정 필요함 - jsonManager에서 이름을 잘라서 받음
    /// </summary>
    /// <param name="npcName"></param>
    /// <returns></returns>
    private NPCRoleInfo GetNPCRoleByName(string npcName)
    {
        if (JsonManager.npcRoleInfoList != null)
        {
            foreach (NPCRoleInfo info in JsonManager.npcRoleInfoList.npcRoleInfoList)
            {
                string name = info.name;
                if (name == npcName)
                {                    
                    return info;
                }
            }
        }
        return null;
    }
}
