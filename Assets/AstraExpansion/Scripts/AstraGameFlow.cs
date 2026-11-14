using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Small, editable prototype UI flow for ASTRA.
// All economy values here are DEMO values, not NASA mission equations.
public class AstraGameFlow : MonoBehaviour
{
    public enum MenuPage { Main, MoonSelection, Shop, MissionDemo, Failure, Success }
    public MenuPage page;

    [Header("Optional UI references populated by the editor builder")]
    public Text heading;
    public Text subtitle;
    public Text balance;
    public Text notice;
    public GameObject instructionsOverlay;
    public Image missionBackground;
    public Sprite[] themedBackgrounds;
    public Button[] moonButtons;
    public Text[] moonButtonTexts;
    public Text[] shopButtonTexts;
    public Button[] shopButtons;
    public Text nextMoonButtonLabel;

    private const string Prefix = "ASTRA_3_MOONS_V2_";
    private static readonly string[] MoonNames = { "PHOBOS", "EUROPA", "TITAN" };
    private static readonly string[] MoonHints =
    {
        "Rocky craters and low gravity",
        "Icy surface and hidden oceans",
        "Golden haze and methane lakes"
    };
    private static readonly int[] UpgradeBasePrices = { 120, 140, 110 };
    private static readonly int[] ResourcePrices = { 75, 90, 100 };
    private static readonly string[] UpgradeNames = { "ENGINE", "SCANNER", "OXYGEN TANK" };
    private static readonly string[] ResourceNames = { "FUEL +3", "OXYGEN +3", "CRYSTALS +2" };

    private static int Get(string key, int defaultValue) { return PlayerPrefs.GetInt(Prefix + key, defaultValue); }
    private static void Put(string key, int value) { PlayerPrefs.SetInt(Prefix + key, value); PlayerPrefs.Save(); }
    private static int Coins { get { return Get("Credits", 450); } set { Put("Credits", value); } }
    private static int Fuel { get { return Get("Fuel", 7); } set { Put("Fuel", value); } }
    private static int Oxygen { get { return Get("Oxygen", 5); } set { Put("Oxygen", value); } }
    private static int Crystals { get { return Get("Crystals", 2); } set { Put("Crystals", value); } }
    private static int Unlocked { get { return Mathf.Clamp(Get("Unlocked", 0), 0, MoonNames.Length - 1); } set { Put("Unlocked", Mathf.Clamp(value, 0, MoonNames.Length - 1)); } }
    private static int Selected { get { return Mathf.Clamp(Get("Selected", 0), 0, MoonNames.Length - 1); } set { Put("Selected", Mathf.Clamp(value, 0, MoonNames.Length - 1)); } }

    private static void OpenScene(string sceneName) { SceneManager.LoadScene(sceneName); }

    private void Start() { Refresh(); }

    private void Refresh()
    {
        if (balance != null)
            balance.text = "CREDITS  " + Coins + "     FUEL  " + Fuel + "     OXYGEN  " + Oxygen + "     CRYSTALS  " + Crystals;

        if (page == MenuPage.MoonSelection) RefreshMoonSelection();
        if (page == MenuPage.Shop) RefreshShop();
        if (page == MenuPage.MissionDemo) RefreshMission();
        if (page == MenuPage.Success) RefreshSuccess();
        if (page == MenuPage.Failure) RefreshFailure();
    }

    public void OpenMainMenu() { OpenScene("AstraMainMenu"); }
    public void OpenMoonSelection() { OpenScene("AstraMoonSelection"); }
    public void OpenShop() { OpenScene("AstraShop"); }

    public void ShowHowToPlay()
    {
        if (instructionsOverlay != null) instructionsOverlay.SetActive(true);
    }
    public void HideHowToPlay()
    {
        if (instructionsOverlay != null) instructionsOverlay.SetActive(false);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void RefreshMoonSelection()
    {
        if (notice != null) notice.text = "SELECT A DESTINATION  •  CLEAR MISSIONS TO UNLOCK MORE MOONS";
        for (int i = 0; i < MoonNames.Length; ++i)
        {
            bool available = i <= Unlocked;
            if (moonButtons != null && i < moonButtons.Length && moonButtons[i] != null)
                moonButtons[i].interactable = available;
            if (moonButtonTexts != null && i < moonButtonTexts.Length && moonButtonTexts[i] != null)
                moonButtonTexts[i].text = available ? (MoonNames[i] + "\n" + MoonHints[i]) : (MoonNames[i] + "\nLOCKED");
        }
    }

    public void SelectMoon0() { LaunchMoon(0); }
    public void SelectMoon1() { LaunchMoon(1); }
    public void SelectMoon2() { LaunchMoon(2); }

    private void LaunchMoon(int index)
    {
        if (index > Unlocked) { SetNotice("MOON LOCKED — COMPLETE THE PREVIOUS MISSION FIRST"); return; }
        int fuelCost = UpgradeLevel(0) >= 3 ? 0 : 1;
        if (Fuel < fuelCost) { SetNotice("NOT ENOUGH FUEL — OPEN THE SHOP TO REFILL"); return; }
        if (Oxygen < 1) { SetNotice("NOT ENOUGH OXYGEN — OPEN THE SHOP TO REFILL"); return; }
        Selected = index;
        Fuel -= fuelCost; // Engine level 3 allows a free-fuel demo attempt.
        Oxygen -= 1;
        OpenScene("AstraMissionDemo");
    }

    private void RefreshMission()
    {
        int moon = Selected;
        if (heading != null) heading.text = "MISSION  /  " + MoonNames[moon];
        if (subtitle != null) subtitle.text = MoonHints[moon].ToUpperInvariant() + "  •  RESCUE THE LOST CREW";
        if (missionBackground != null && themedBackgrounds != null && moon < themedBackgrounds.Length)
            missionBackground.sprite = themedBackgrounds[moon];
        if (notice != null)
            notice.text = "DEMO ONLY: CHOOSE A TEST OUTCOME BELOW. REPLACE THIS SCENE WITH YOUR REAL GAMEPLAY.";
    }

    public void SimulateSuccess()
    {
        int reward = 180 + 40 * UpgradeLevel(1) + 15 * UpgradeLevel(0);
        Put("LastReward", reward);
        Coins += reward;
        Crystals += 1;
        if (Selected == Unlocked && Unlocked < MoonNames.Length - 1) Unlocked += 1;
        OpenScene("AstraSuccess");
    }
    public void SimulateFailure() { OpenScene("AstraFailure"); }
    public void RetryMission() { LaunchMoon(Selected); }

    private void RefreshSuccess()
    {
        if (nextMoonButtonLabel != null)
            nextMoonButtonLabel.text = Selected == MoonNames.Length - 1 ? "BACK TO MOONS" : "NEXT MOON";
        if (heading != null) heading.text = "MISSION SUCCESS";
        if (subtitle != null)
        {
            subtitle.text = "CREW RESCUED ON " + MoonNames[Selected] + "!\n" +
                "+" + Get("LastReward", 180) + " CREDITS    +1 CRYSTAL\n" +
                (Selected < MoonNames.Length - 1 ? "NEXT MOON AVAILABLE: " + MoonNames[Selected + 1] : "ALL THREE MOONS COMPLETED!");
        }
    }
    private void RefreshFailure()
    {
        if (heading != null) heading.text = "MISSION FAILED";
        if (subtitle != null)
            subtitle.text = "YOUR CREW WAS NOT RESCUED ON " + MoonNames[Selected] + ".\nRETRY THE MISSION OR VISIT THE SHOP.";
    }
    public void NextMoon()
    {
        if (Selected >= MoonNames.Length - 1) { OpenMoonSelection(); return; }
        LaunchMoon(Selected + 1);
    }

    private static int UpgradeLevel(int i) { return Get("Upgrade" + i, 0); }
    private static int UpgradePrice(int i) { return UpgradeBasePrices[i] + UpgradeLevel(i) * 90; }

    private void RefreshShop()
    {
        for (int i = 0; i < 6; ++i)
        {
            bool upgrade = i < 3;
            int price = upgrade ? UpgradePrice(i) : ResourcePrices[i - 3];
            string desc = upgrade ? UpgradeNames[i] + "  LV " + UpgradeLevel(i) + "/3" : ResourceNames[i - 3];
            if (shopButtonTexts != null && i < shopButtonTexts.Length && shopButtonTexts[i] != null)
                shopButtonTexts[i].text = desc + "\n" + (upgrade && UpgradeLevel(i) >= 3 ? "MAX LEVEL" : price + " CREDITS");
            if (shopButtons != null && i < shopButtons.Length && shopButtons[i] != null)
                shopButtons[i].interactable = Coins >= price && (!upgrade || UpgradeLevel(i) < 3);
        }
        if (notice != null && string.IsNullOrEmpty(notice.text))
            notice.text = "BUY UPGRADES OR STOCK UP ON MISSION RESOURCES";
        if (balance != null) balance.text = "CREDITS  " + Coins + "   •   FUEL  " + Fuel + "   •   OXYGEN  " + Oxygen + "   •   CRYSTALS  " + Crystals;
    }

    private void BuyUpgrade(int index)
    {
        if (UpgradeLevel(index) >= 3) { SetNotice("UPGRADE IS ALREADY AT MAX LEVEL"); return; }
        int price = UpgradePrice(index);
        if (Coins < price) { SetNotice("NOT ENOUGH CREDITS"); return; }
        Coins -= price;
        Put("Upgrade" + index, UpgradeLevel(index) + 1);
        if (index == 2) Oxygen += 2; // Oxygen tank upgrade gives 2 bonus units in this demo.
        SetNotice(UpgradeNames[index] + " UPGRADED!");
        RefreshShop();
    }
    private void BuyResource(int index)
    {
        int price = ResourcePrices[index];
        if (Coins < price) { SetNotice("NOT ENOUGH CREDITS"); return; }
        Coins -= price;
        if (index == 0) Fuel += 3;
        if (index == 1) Oxygen += 3;
        if (index == 2) Crystals += 2;
        SetNotice(ResourceNames[index] + " PURCHASED!");
        RefreshShop();
    }
    public void BuyEngine() { BuyUpgrade(0); }
    public void BuyScanner() { BuyUpgrade(1); }
    public void BuyTank() { BuyUpgrade(2); }
    public void BuyFuel() { BuyResource(0); }
    public void BuyOxygen() { BuyResource(1); }
    public void BuyCrystals() { BuyResource(2); }

    private void SetNotice(string message) { if (notice != null) notice.text = message; }

    public void ResetDemoProgress()
    {
        string[] keys = { "Credits", "Fuel", "Oxygen", "Crystals", "Unlocked", "Selected", "LastReward", "Upgrade0", "Upgrade1", "Upgrade2" };
        foreach (string key in keys) PlayerPrefs.DeleteKey(Prefix + key);
        PlayerPrefs.Save();
        Refresh();
        SetNotice("DEMO PROGRESS RESET. PHOBOS IS UNLOCKED.");
    }
}
