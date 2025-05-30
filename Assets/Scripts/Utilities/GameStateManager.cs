using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlossomValley.AnimalSystem;
using BlossomValley.IncubationSystem;

public class GameStateManager : MonoBehaviour, ITimeTracker
{
    public static GameStateManager Instance { get; private set; }

    private bool screenFadedOut;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        FadeEventManager.OnFadeOutComplete += OnFadeOutComplete;
    }

    private void OnDestroy() => FadeEventManager.OnFadeOutComplete -= OnFadeOutComplete;

    private void Start()
    {
        SoundManager.Instance.PlayMusic(SoundType.BackgroundMusic);
        TimeManager.Instance.RegisterTracker(this);
    }

    public void ClockUpdate(GameTimestamp timestamp)
    {
        UpdateShippingState(timestamp);
        UpdateFarmState(timestamp);
        IncubationManager.UpdateEggs();

        if (timestamp.hour == 0 && timestamp.minute == 0)
            OnDayReset();
    }

    private void OnDayReset()
    {
        foreach (NPCRelationshipState npc in RelationshipStats.relationships)
        {
            npc.hasTalkedToday = false;
            npc.giftGivenToday = false;
        }

        AnimalFeedManager.ResetFeedboxes();
        AnimalStats.OnDayReset();
    }

    private void UpdateShippingState(GameTimestamp timestamp)
    {
        if (timestamp.hour == ShippingBin.hourToShip && timestamp.minute == 0)
            ShippingBin.ShipItems();
    }

    private void UpdateFarmState(GameTimestamp timestamp)
    {
        if (SceneTransitionManager.Instance.currentLocation != SceneTransitionManager.Location.Farm)
        {
            if (LandManager.farmData == null) return;

            List<LandSaveState> landData = LandManager.farmData.Item1;
            List<CropSaveState> cropData = LandManager.farmData.Item2;

            if (cropData.Count == 0) return;

            for (int i = 0; i < cropData.Count; i++)
            {
                CropSaveState crop = cropData[i];
                LandSaveState land = landData[crop.landID];

                if (crop.cropState == CropBehaviour.CropState.Wilted) continue;

                land.ClockUpdate(timestamp);

                if (land.landStatus == LandModel.LandStatus.Watered)
                    crop.Grow();
                else if (crop.cropState != CropBehaviour.CropState.Seed)
                    crop.Wither();

                cropData[i] = crop;
                landData[crop.landID] = land;
            }
        }
    }

    public void Sleep()
    {
        UIManager.Instance.FadeOutScreen();
        screenFadedOut = false;
        StartCoroutine(TransitionTime());
    }

    private IEnumerator TransitionTime()
    {
        GameTimestamp timestampOfNextDay = TimeManager.Instance.GetGameTimestamp();
        timestampOfNextDay.day += 1;
        timestampOfNextDay.hour = 6;
        timestampOfNextDay.minute = 0;

        while (!screenFadedOut)
            yield return new WaitForSeconds(1f);

        TimeManager.Instance.SkipTime(timestampOfNextDay);
        SaveManager.Save(ExportSaveState());
        screenFadedOut = false;
        UIManager.Instance.ResetFadeDefaults();
    }

    public void OnFadeOutComplete() => screenFadedOut = true;

    public GameSaveState ExportSaveState()
    {
        FarmSaveState farmSaveState = FarmSaveState.Export();
        InventorySaveState inventorySaveState = InventorySaveState.Export();
        PlayerSaveState playerSaveState = PlayerSaveState.Export();
        GameTimestamp timestamp = TimeManager.Instance.GetGameTimestamp();
        RelationshipSaveState relationshipSaveState = RelationshipSaveState.Export();

        return new GameSaveState(farmSaveState, inventorySaveState, timestamp, playerSaveState, relationshipSaveState);
    }

    public void LoadSave()
    {
        GameSaveState save = SaveManager.Load();
        TimeManager.Instance.LoadTime(save.timestamp);
        save.inventorySaveState.LoadData();
        save.farmSaveState.LoadData();
        PlayerModel playerModel = UnityEngine.Object.FindAnyObjectByType<PlayerView>().PlayerModel;
        save.playerSaveState.LoadData(playerModel);
        save.relationshipSaveState.LoadData();
    }
}