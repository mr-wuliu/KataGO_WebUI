
public class MStringUtils
{
    const string boardLetters = "ABCDEFGHJKLMNOPQRSTUVWXYZ";
    public static string ConvertPositionToKataGoFormat(int x, int y)
    {
        return boardLetters[x] + (y + 1).ToString();
    }
}

