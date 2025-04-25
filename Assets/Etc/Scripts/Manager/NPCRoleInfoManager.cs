using System.Diagnostics;

public class NPCRoleInfoManager
{    
    public string GetPrompt(string npcName) => GetNPCRoleByName(npcName)?.prompt;

    /// <summary>
    /// string name �޴� ���� ���� �ʿ��� - jsonManager���� �̸��� �߶� ����
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
