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
}

public static class ObsidityStrings
{
    public static readonly string DefaultPath = "/Obsidity/Vault";
    public static readonly string FindObsidityWindow =
        "You can always find the editor in the menu: 'Window/Obsidity/Obsidity Editor' Or click the button below";

    public static readonly string CheckForVault = "If you believe this is wrong you can 'Check for Obsidity vault'";
    public static readonly string AlreadyInitialized = "Obsidity is already initialized at";
    public static readonly string EmptyError = "Please fill out all the fields.";

    public static readonly string NotInitializedError =
        "Please initialize the vault via 'Window/Obsidity/Obsidity Welcome' first";

    public static readonly string SaveError = "Error when saving, check console output.";
    public static readonly string SaveSuccess = "Successfully saved file: ";
    public static readonly string SelectVaultName = "Please select a vault name";
}