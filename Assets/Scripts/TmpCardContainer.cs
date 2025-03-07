using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

public class TmpCardContainer
{
    [ShowInInspector] [ListDrawerSettings(IsReadOnly = true)]
    public readonly List<Card> TemporallyCards;

    public TmpCardContainer()
    {
        TemporallyCards = new List<Card>();
    }

    public bool CheckWinLose()
    {
        bool flipResult =  TemporallyCards.All(x => x.Id == TemporallyCards[0].Id);;
        
        if (flipResult)
        {
            foreach (var tmpCard in TemporallyCards)
            {
                tmpCard.RemoveCard();
            }
        }
        else
        {
            foreach (var tmpCard in TemporallyCards)
            {
                tmpCard.HideCard();
            }
        }
        TemporallyCards.Clear();
        
        return flipResult;
    }
    
}