using System.Collections.Generic;
using UnityEngine;
public class PlayerDataHandler
{
    //public PlayerAttributes[] CheckSaveDataPlayers(PlayerAttributes[] Players)
    //{
    //    List<PlayerStats> playerStats = SaveData.Instance.Players;
    //    // if there is no entry
    //    if (playerStats.Count == 0)
    //    {
    //        List<PlayerStats> tmp = new List<PlayerStats>(Players.Length);
    //        for (int index = 0; index < Players.Length; index++)
    //        {
    //            PlayerStats stat = new PlayerStats();
    //            stat.Name = Players[index].Name;
    //            stat.Lock = Players[index].Locked;
    //            tmp.Add(stat);
    //        }
    //        playerStats = tmp;
    //    }
    //    // if there is some entry
    //    else if ((Players.Length - playerStats.Count) != 0)
    //    {
    //        int AdditionalAmount = (Players.Length - playerStats.Count);
    //        if (AdditionalAmount > 0)
    //        {
    //            List<PlayerStats> tmp = new List<PlayerStats>();
    //            List<PlayerStats> Player = new List<PlayerStats>();
    //            for (int i = 0; i < Players.Length; i++)
    //            {
    //                PlayerStats stat = new PlayerStats();

    //                stat.Name = Players[i].Name;
    //                stat.Lock = Players[i].Locked;
    //                Player.Add(stat);
    //            }
    //            tmp = giveUnique(playerStats, Player);
    //            // this loop iterate the difference
    //            for (int playerindex = 0; playerindex < tmp.Count; playerindex++)
    //            {
    //                playerStats.Add(tmp[playerindex]);
    //            }
    //            SaveData.Instance.Players = playerStats;
    //        }
    //        else if (AdditionalAmount < 0)
    //        {
    //            List<PlayerStats> tmp = new List<PlayerStats>();
    //            List<PlayerStats> Player = new List<PlayerStats>();
    //            for (int i = 0; i < Players.Length; i++)
    //            {
    //                PlayerStats stat = new PlayerStats();

    //                stat.Name = Players[i].Name;
    //                stat.Lock = Players[i].Locked;
    //                Player.Add(stat);
    //            }
    //            tmp = giveUnique(Player, playerStats);

    //            for (int playerindex = 0; playerindex < tmp.Count; playerindex++)
    //            {
    //                for (int x = 0; x < playerStats.Count; x++)
    //                {
    //                    if (tmp[playerindex].Name == playerStats[x].Name)
    //                    {
    //                        playerStats.RemoveAt(x);
    //                        playerStats.TrimExcess();
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    SaveData.Instance.Players = playerStats;
    //    GSF_SaveLoad.SaveProgress();

    //    // reading values
    //    for (int saveDataIndex = 0; saveDataIndex < SaveData.Instance.Players.Count; saveDataIndex++)
    //    {
    //        // comparing Name
    //        if (Players[saveDataIndex].Name == SaveData.Instance.Players[saveDataIndex].Name)
    //        {
    //            Players[saveDataIndex].Locked = SaveData.Instance.Players[saveDataIndex].Lock;
    //        }
    //    }

    //    return Players;
    //}

    //List<PlayerStats> giveUnique(List<PlayerStats> Small, List<PlayerStats> Big)
    //{
    //    List<PlayerStats> st = new List<PlayerStats>();
    //    bool isfound = false;
    //    int k = 0;
    //    for (int j = 0; j < Big.Count; j++)
    //    {
    //        for (int i = 0; i < Small.Count; i++)
    //        {
    //            if (Big[j].Name == Small[i].Name)
    //            {
    //                isfound = true;
    //                break;
    //            }
    //            else
    //            {
    //                isfound = false;
    //                k = j;
    //            }
    //        }
    //        if (!isfound)
    //        {
    //            PlayerStats stat = new PlayerStats();

    //            stat.Name = Big[k].Name;
    //            stat.Lock = Big[k].Lock;
    //            st.Add(stat);
    //        }
    //    }
        //return st;
    //}
}
