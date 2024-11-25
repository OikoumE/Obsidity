using System;

[Serializable] //
public class ObsidityData
{
    public string textContent;
    public string textDate;
    public string textTags;
    public string textTitle;

    public ObsidityData(
        string textContent,
        string textDate,
        string textTags,
        string textTitle
    )
    {
        this.textContent = textContent;
        this.textDate = textDate;
        this.textTags = textTags;
        this.textTitle = textTitle;
    }

    private void SerializeDataToFile()
    {
    }
}