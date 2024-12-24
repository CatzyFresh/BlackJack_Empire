// DealerController Class
public class DealerController : PlayerController
{
    
    public void PlayTurn()
    {
        while (GetHandValue() < 17)
        {
            AddCardToHand(DeckManager.Instance.DrawCard());
        }
    }
}