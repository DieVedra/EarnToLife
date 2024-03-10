using System.Text;
public static class BuildString
{
    public static string GetString(string[] text)
    {
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < text.Length; i++)
        {
            stringBuilder.Append(text[i]);
        }
        return stringBuilder.ToString();
    }
}
