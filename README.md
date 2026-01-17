# Farming RPG | Unity | C#

A relationship-driven farming simulator featuring crop growth systems, NPC interactions, and persistent world state. Built with MVC architecture and event-driven communication to manage complex interdependent systems across multiple scenes.

**Tech Stack:** Unity 3D | C# | ScriptableObjects | State Pattern | Strategy Pattern | Observer Pattern

---

## Development Approach

I structured the game around MVC with a central manager pattern - LandManager coordinates between individual land plots, CropSystem handles growth states, and TimeManager broadcasts updates to all registered listeners. When a player waters farmland, the call flows LandView → LandController → LandModel → LandManager, which batches all changes before saving. This separation meant I could test crop growth independently from rendering.

ScriptableObjects store all static data (crop growth rates, NPC dialogue trees, item stats). The TimeManager implements `ITimeTracker` interface - systems like WeatherManager, LandController, and GameStateManager register themselves and receive `ClockUpdate()` callbacks every in-game minute. This eliminated tight coupling between time-dependent systems.

```mermaid
flowchart TD
    subgraph CoreSystems["Core Systems"]
        TimeManager["TimeManager<br/>ITimeTracker Registry"]
        GameStateManager["GameStateManager<br/>Sleep/Save/DayReset"]
        SceneTransitionManager["SceneTransitionManager<br/>Location Switching"]
        
        TimeManager -->|ClockUpdate| GameStateManager
        TimeManager -->|ClockUpdate| SceneTransitionManager
    end
    
    subgraph Relationships["Relationships"]
        RelationshipStats["RelationshipStats<br/>NPCs"]
        AnimalStats["AnimalStats<br/>Animals"]
        DialogueContext["DialogueContext<br/>Strategy Pattern"]
    end
    
    subgraph Economy["Economy"]
        InventoryManager["InventoryManager<br/>MVC"]
        Shop["Shop<br/>Purchase"]
        ShippingBin["ShippingBin<br/>Timed Sales"]
    end
    
    subgraph FarmSystems["Farm Systems"]
        WeatherManager["WeatherManager<br/>Rain Logic"]
        LandManager["LandManager<br/>108 Land Plots"]
        LandController["LandController<br/>MVC Instance"]
        CropSystem["CropSystem<br/>State Machine"]
        
        TimeManager -->|ClockUpdate| WeatherManager
        WeatherManager -->|Rain Event| LandController
        LandController --> CropSystem
    end
    
    GameStateManager -->|OnDayReset| RelationshipStats
    GameStateManager -->|OnDayReset| AnimalStats
    GameStateManager -->|UpdateShipping| ShippingBin
    SceneTransitionManager -->|Scene Load| LandManager
    
    classDef coreStyle fill:#E6E6FA,stroke:#333,stroke-width:2px
    classDef relationshipStyle fill:#FFF8DC,stroke:#333,stroke-width:2px
    classDef economyStyle fill:#FFFACD,stroke:#333,stroke-width:2px
    classDef farmStyle fill:#F0FFF0,stroke:#333,stroke-width:2px
    
    class TimeManager,GameStateManager,SceneTransitionManager coreStyle
    class RelationshipStats,AnimalStats,DialogueContext relationshipStyle
    class InventoryManager,Shop,ShippingBin economyStyle
    class WeatherManager,LandManager,LandController,CropSystem farmStyle
```

---

## Key Technical Systems

* ### State Pattern for Crop Lifecycle
    - Crops transition through four states (Seed → Seedling → Harvestable → Wilted) using a state machine. Each state implements `ICropState` with `EnterState()`, `Grow()`, and `Wither()` methods. The challenge was handling regrowable crops - when harvested, they need to reset growth timers without destroying the GameObject.
    - I solved this by having HarvestableState detach the harvestable GameObject from the crop's transform when it's not regrowable, then calling `context.RemoveCrop()` which destroys the entire crop. For regrowables, the crop stays parented and calls `context.Regrow()`, which sets `growth` to `maxGrowth - regrowTimeInMinutes` and transitions back to Seedling state.

    ```mermaid
    stateDiagram-v2
        [*] --> Seed: Plant()
        
        Seed --> Seedling: growth >= maxGrowth/2
        
        Seedling --> Harvestable: growth >= maxGrowth
        Seedling --> Wilted: health <= 0
        
        Harvestable --> Seedling: Regrow()<br/>(regrowable crops)
        Harvestable --> [*]: RemoveCrop()<br/>(normal crops)
        
        Wilted --> [*]: Remove with Shovel
        
        note right of Harvestable
            Regrowable crops:
            growth = maxGrowth - regrowTime
            State persists
        end note
    ```

    - The CropContext holds all shared data (`growth`, `health`, `maxGrowth`) and mediates state transitions. This kept state classes stateless - they operate on context data rather than storing their own. When loading saves, I instantiate the correct state using `CropStateFactory.CreateState(stateType)` and directly set growth/health values before calling `EnterState()`.

### Strategy Pattern for NPC Dialogue

NPCs have two dialogue flows: default conversation and gift-giving. Instead of branching logic in InteractableCharacter, I used the Strategy pattern with `IDialogueStrategy`. The DialogueContext switches between `DefaultDialogueStrategy` and `GiftDialogueStrategy` based on whether the player is holding an item.

GiftDialogueStrategy is complex - it checks `FirstMeeting()` to reject gifts, evaluates `GetReactionToGift()` against the character's likes/dislikes lists, and multiplies friendship points by 8x if `IsBirthday()` returns true. I chained actions using `System.Action` delegates to stack the gift consumption and character rotation reset callbacks.

### Animal Husbandry with Mood System

Animals have both friendship and mood values. Mood (0-255) decays by 100 daily if not fed, while friendship changes based on interaction. The challenge was tracking which animals were fed when multiple animals of the same type exist.

I created AnimalFeedManager with a `Dictionary<AnimalData, bool[]>` where each feedbox has an ID. When the player feeds a box, it finds the first eligible animal of that type where `giftGivenToday == false` and sets it true. On day reset, all feedboxes clear and mood/friendship update based on whether flags were set. ChickenBehaviour checks these conditions in `LayEgg()` - eggs only spawn if `age >= daysToMature` AND `Mood > 30` AND `!givenProduceToday`.

```mermaid
sequenceDiagram
    participant Player
    participant Feedbox
    participant AnimalFeedManager
    participant AnimalStats
    participant Animal
    
    Player->>Feedbox: Interact with Food
    Feedbox->>AnimalFeedManager: FeedAnimal(id)
    AnimalFeedManager->>AnimalStats: GetAnimalsByType()
    AnimalStats-->>AnimalFeedManager: List<Animals>
    AnimalFeedManager->>Animal: Set giftGivenToday=true
    AnimalFeedManager->>AnimalFeedManager: feedboxStatus[type][id]=true
    
    rect rgb(255, 255, 200)
        Note over Player,Animal: On Day Reset
        AnimalFeedManager->>Feedbox: Clear all feedboxes
        AnimalStats->>Animal: Mood += 15 if fed<br/>Mood -= 100 if not fed
    end
```

### Weather System with Time Integration

The WeatherManager subscribes to TimeManager and runs two systems: scheduled rain (14:00-16:00 with seasonal probability) and random rain events. The problem was making rain affect crops even when the player wasn't on the farm scene.

I solved this by having WeatherManager set a boolean `isRaining` flag that GameStateManager checks during `UpdateFarmState()`. If raining, it directly modifies the saved `LandSaveState` structs to switch from Farmland to Watered status:

```csharp
if (WeatherManager.Instance.IsCurrentlyRaining()) {
    if (land.landStatus == LandModel.LandStatus.Farmland)
        land.landStatus = LandModel.LandStatus.Watered;
}
```

The seasonal probabilities (Spring: 100%, Summer: 40%, Fall: 80%, Winter: 30%) affect both scheduled and random rain. For random events, I check every 60 in-game minutes and roll against `seasonalChance * 0.1f` to avoid constant rain.

### Economy: Shop and Time-Delayed Shipping

The Shop uses a simple purchase flow, but ShippingBin needed delayed execution. Players place items throughout the day, but sales only process at 18:00. I used a static `List<ItemSlotData>` that accumulates items across scenes.

`GameStateManager.ClockUpdate()` checks if `timestamp.hour == 18 && timestamp.minute == 0`, then calls `ShippingBin.ShipItems()` which tallies item values, adds money to PlayerModel, and clears the list. The challenge was preventing duplicate sales if the time check ran multiple times - I solved this by only checking on the exact minute (`minute == 0`), ensuring it fires once per hour transition.

### Calendar System with Birthday Tracking

The calendar renders 30 days per season with CalendarEntry components. Each entry calculates its color based on `DayOfTheWeek` and whether it matches today's date. The complex part was birthday tracking - I needed to search all CharacterScriptableObjects and display their portrait on matching dates.

`CalendarUIListing.GetCharacterWithBirthday()` iterates through `allCharacters` and compares `timestamp.day` and `timestamp.season` to each character's birthday timestamp. If found, it passes the portrait sprite to `CalendarEntry.Display()`. The calendar supports navigation between seasons by constructing new `GameTimestamp` objects with incremented season values and year rollover logic.

### Save System with Scene Persistence

The save system needed to handle data that persists even when scenes unload. I used static variables in manager classes (`LandManager.farmData`, `AnimalStats.animalRelationships`) that survive scene transitions via `DontDestroyOnLoad`.

The tricky part was crops growing while the player is in other locations. When `ClockUpdate()` fires and the player isn't on the farm, GameStateManager directly modifies the saved `LandSaveState` and `CropSaveState` structs. Structs are value types, so I had to reassign them back to the lists after modification. When the player returns to the farm, `LandManager.ImportCropData()` spawns CropBehaviour instances and calls `LoadCrop()` with the saved growth/health values.

---

## Technical Challenges

- **Event-Driven UI Updates:** Initially, changing money triggered a full UI re-render. I added `MoneyChanged` event to PlayerModel that only updates the money text. Similarly, LandModel fires `OnLandStatusChanged` which LandView subscribes to for material swaps.

- **Cross-Scene Animal Spawning:** Incubators hatch eggs after 3 in-game days. If hatching occurs while the player is in another scene, IncubationManager stores the incubator ID and spawns the chicken on the next scene load. I used `IncubationManager.eggsIncubating` (a static list) that persists across scenes, checking `timeToIncubate` each clock tick and removing entries when <= 0.

- **Time-Based Withering:** Crops needed to wither if not watered within 24 hours. I stored `GameTimestamp lastWatered` in LandModel and used `GameTimestamp.CompareTimestamps()` to calculate elapsed hours. If > 24 hours and not raining, land switches from Watered to Farmland and crops call `Wither()`.

---

## What I Learned

Using interfaces (`ITimeTracker`, `IDialogueStrategy`, `ICropState`) made systems testable in isolation - I could mock TimeManager callbacks when testing crop growth. The Strategy pattern eliminated branching in NPC interactions, making it trivial to add new dialogue types like trading or quests. Managing scene persistence with static variables was fragile - for production I'd use a singleton SaveManager with serialized data instead of relying on class statics. The animal mood/friendship dual system taught me to separate short-term state (mood) from long-term progression (friendship) for better game balance.

---

## Play Link
- https://sayannandi.itch.io/blossom-valley

---

[![Watch the video](https://img.youtube.com/vi/AS5MBmQLmX4/maxresdefault.jpg)](https://youtu.be/AS5MBmQLmX4)
### [Watch the Gameplay Video here](https://youtu.be/AS5MBmQLmX4)

![Image](https://github.com/user-attachments/assets/e4484a17-2a1a-4560-be55-1a2057832b94)

![Image](https://github.com/user-attachments/assets/1a6ee98a-a7b2-4037-82b8-20b15ed68c51)

![Image](https://github.com/user-attachments/assets/b587344f-bde5-4fc4-b72c-7c78f6701e75)

![Image](https://github.com/user-attachments/assets/eb8aa959-dbc4-49a0-987c-758566195626)

![Image](https://github.com/user-attachments/assets/7a687d17-30a1-4bd4-a84f-7b14526ddbd4)

![Image](https://github.com/user-attachments/assets/cd194954-6639-4637-b700-418cb930cfbe)

![Image](https://github.com/user-attachments/assets/32ccac5f-c0d0-4c82-b04e-e5ec7daeff30)

![Image](https://github.com/user-attachments/assets/ee6c42e9-6cb2-44a3-b263-95dc99e19aa2)

![Image](https://github.com/user-attachments/assets/5b481226-149c-445c-9b74-a82af9854f35)

![Image](https://github.com/user-attachments/assets/c846f79a-9f93-4cbf-8c58-675515173213)

![Image](https://github.com/user-attachments/assets/bdebce67-fd4e-478a-a732-f71ca5f8e3dd)

![Image](https://github.com/user-attachments/assets/e2f6bf86-c2fb-4143-9530-d58d3ba427c0)

![Image](https://github.com/user-attachments/assets/bd3d1e26-4515-4266-9c1c-de3a240f4a1c)

![Image](https://github.com/user-attachments/assets/fb27bd14-b5ee-4071-b0ce-803de7f1dbe4)

![Image](https://github.com/user-attachments/assets/89c31dff-05f6-4e55-a471-4285d56dc71f)

![Image](https://github.com/user-attachments/assets/b080183b-c615-4e39-9e95-06379100ecdf)

![Image](https://github.com/user-attachments/assets/08e3199f-228f-4a2d-b57c-4450d41977ec)

![Image](https://github.com/user-attachments/assets/6cbaa854-a889-4b52-a486-095880e06e53)

![Image](https://github.com/user-attachments/assets/74126ae7-04a8-477f-901e-bb36a55c13b5)

![Image](https://github.com/user-attachments/assets/c6c99191-d521-4b07-826c-7b816f6ba374)

![Image](https://github.com/user-attachments/assets/54caee43-e0f9-4296-85fd-22aa93b5377f)

![Image](https://github.com/user-attachments/assets/e3fcf277-8754-4c12-b8bf-c8dd2967086b)

![Image](https://github.com/user-attachments/assets/aaf4ceef-d9bb-4ea1-82d3-6577fc5e76cb)

![Image](https://github.com/user-attachments/assets/bfb511c6-e3db-4195-86a3-c297b804cd0f)
