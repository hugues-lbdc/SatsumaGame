/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BotManager : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Player player = MinDistance(Player bot, Player[] dico);
        MomeTo(Player bot, player);
        
    }
    
    public static double Distance(Player bot, Player player )
    {
        
        int y1 = bot.transform.position.Y;
        int x1 = bot.tranform.position.X;
        int y2 = player.transform.position.Y;
        int x2 = player.transform.position.X;
        int distance = Math.Sqrt(Sqr(y2 - y1) + Sqr(x2 - x1));
        return distance;

                
    }
    public static Player MinDistance (Player bot, player[] dico)
    {
        Player min = dico[0];
        int player = 1;

        for (player in dico.Lenght)
        {
            if Distance(bot, dico[player]) < Distance(bot, min)
                min = player;
        }
        return min;
    }
    public static void MomeTo (Player bot,Player player)
    {
        
    }



}
*/