[System.Serializable]
public class SerializiblePlayer
{
    public string PlayerName;
    public SerializibleColor Color;
    public int Money;
    public int id;

    public SerializiblePlayer(Player player) {
        PlayerName = player.PlayerName;
        Color = new SerializibleColor(player.PlayerColor);
        Money = player.Money;
        id = player.ID;
    }

    public Player ToPlayer() {
        Player player = new Player(PlayerName, Color.ToUnityColor());
        player.ID = id;
        player.Money = Money;
        return player;
    }
}
