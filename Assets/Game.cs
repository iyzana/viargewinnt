[System.Serializable]
class Game
{
    public string state;
    public long id;
    public string[] players;
    public string currentPlayer;
    public int width;
    public int height;
    public Column[] grid;
}