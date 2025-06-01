# 🚜 Blossom Valley – A 3D Farming Simulator
Experience the joy of cultivating your own patch of paradise: clear obstacles, sow seeds, raise animals, build relationships, and watch your world flourish through seasons, weather, and time.

---

## 🌾 Land & Cropping

### Obstacle Clearing
- Three obstacle types spawn randomly on farmland plots:  
  - **Woody Stumps:** Remove with an Axe  
  - **Rocks:** Break with a Pickaxe  
  - **Weeds:** Dig up with a Shovel  
- Clearing returns usable space and may yield resources (wood, stone, fibers).

### Soil Preparation
- Use a Hoe to till cleared land into Farmable Soil.  
- Tilled tiles register in the Land System, track watering status, and accept seeds.

### Planting & Growth Lifecycle
- **Seed Placement:** Plant seeds into hoed soil.  
- **Watering Mechanics:**  
  - **Watering Can:** Apply water to freshly planted seeds → triggers seedling growth timer.  
  - **Ongoing Watering:** Each watered interval advances the crop’s growth stage.  
  - **Neglecting Water:** If watering lapses, a Seedling can turn into a Wilted Crop.  
- **Growth States** (managed by the Crop System’s state machine):  
  1. _Seed_ → 2. _Seedling_ → 3. _Harvestable_ → (if regrowable) 4. _Regrowth Cycles_  
  - Regular crops yield a one-time Harvestable vegetable.  
  - Regrowable varieties (e.g., **Tomato**): Once the first Harvestable phase completes, new vegetables spawn after subsequent watering/time cycles.  
  - Wilted crops must be removed and replanted.  
  - Growth timing ties into the Time System for synchronization with days, hours, and seasons.

### Land System Integration
- Manages tile states (`Untilled`, `Tilled–Dry`, `Tilled–Wet`, `Planted`, `Wilted`).  
- Reacts to weather events: Rain automatically waters Farmable Soil.  
- Triggers visual feedback (soil material changes) and events for other systems (e.g., Inventory, UI).  
- Supports Save/Load so every tile’s status persists between sessions.

---

## 🐄 Animal Husbandry

### Egg Incubation & Hatching
- Collect **Eggs** from shipping, foraging, or previous chicken produce.  
- Place Eggs into an Incubator (one of multiple slots).  
- Incubator System tracks a countdown (in-game hours/days):  
  - **Empty:** No egg present.  
  - **Incubating:** Egg is warming; UI shows progress.  
  - **Ready:** After the designated time, a **Chick** spawns at the incubator location.

### Chick → Chicken Growth
- **Feeding Mechanics:**  
  - Feed Chicks daily (e.g., with starter feed) to progress their age.  
  - Skimping on feed → slower growth; risk of illness (optional future expansion).  
- Once grown, Chicks become **Chickens** with distinct 3D models and animations.  
- Chickens automatically lay Eggs at regular intervals (tracked by the Time System).

### Relationship & Mood
- Each animal has a **Mood** (`Happy`, `Neutral`, `Grumpy`) and a **Fondness** score toward the Player.  
- **Daily Interaction:**  
  - Talking to animals (trigger dialogue bubbles/animations).  
  - Feeding influences Mood:  
    - **Frequent Care** (daily talk + feed) → Mood stays _Happy_ or _Content_, Fondness ↑.  
    - **Neglect** → Mood tends toward _Grumpy_, Fondness ↓.  
  - Mood impacts animal produce (e.g., happier chickens lay higher-quality eggs).  
  - Friendship thresholds unlock small bonuses (e.g., more frequent egg-laying or cosmetic interactions).

---

## 🏘️ Town & NPC Relationships

### Social Hub
- Central Town area where the Player can enter buildings, stroll streets, and encounter NPCs.  
- NPCs follow daily schedules (work, home, leisure) and display portrait icons when nearby.

### Dialogue & Gift System
- **Dialogue System:**  
  - Typewriter-style conversations; multiple dialogue contexts (first meeting, daily greeting, birthday, gifting).  
  - Dialogue queue ensures one message at a time; UI prompts advance with player input.  
  - Callbacks trigger quests or unlock content when conversations complete.  
- **Friendship Points:**  
  - NPCs have Preference Profiles (ScriptableObject):  
    - _Like_ Tags, _Dislike_ Tags, _Neutral_ Items.  
  - **Gift‐Giving Mechanics:**  
    - **Liked Gifts** → Friendship ↑.  
    - **Disliked Gifts** → Friendship ↓.  
    - **Birthday Bonuses:** On their birthday (shown in Calendar), liked gifts yield extra Friendship.  
  - **Daily Limits:** One gift per NPC per day; over‐gifting has no extra benefit.  
  - **First‐Meeting Detection:** Triggers a welcome quest or unlocks initial dialogue.

### Character Unlocks & Events
- Hitting Friendship milestones unlocks new cutscenes or town events (festivals, side quests).  
- NPC schedules can change if Friendship is high (e.g., visiting the Player’s farm, sharing special recipes).

---

## 🛒 Commerce & Economy

### Shop System
- In‐town shops stock seeds, tools, animal feed, furnishings, and seasonal decor.  
- **Immediate Transactions:**  
  - Player opens Shop UI via dialogue with Shopkeeper.  
  - Browse categorized inventory (Seeds, Tools, Animal Supplies).  
  - Select quantity, confirm purchase → currency deducted, item added to Inventory.  
  - Stock refreshes daily or by season (e.g., Winter seeds unavailable).  
  - Discounts may apply based on NPC Friendship levels.

### Shipping Bin (Selling)
- **Drop‐Off:** Place sellable items (crops, animal produce, crafted goods) into Shipping Bin anytime during the day.  
  - UI displays deposited items and expected revenue.  
- **Scheduled Processing:**  
  - At a set hour (e.g., 6:00 PM), Shipping Bin processes all items.  
  - Funds appear in Wallet the next morning.  
  - Receipts viewable in UI, showing quality and quantity sold.

---

## 📅 Calendar & Time System

### Custom Time Scale
- In‐game clock flows: Minutes → Hours → Days → Weeks → Months → Seasons → Years.  
- **Seasons:**  
  - Spring, Summer, Autumn, Winter (each lasting a fixed number of in‐game days).  
  - Season affects crop growth rates, wild forage availability, Shop stock, and Weather probabilities.  
- **Day/Night Cycle:**  
  - Sun/Moon visuals rotate according to the current time; affects NPC schedules (day vs. night activities).  
  - Certain buildings close at night (e.g., shops).  
- **Time Controls:**  
  - Speed up or slow down (1×, 2×, 3×) for faster progression.  
  - “Skip to Next Morning” or jump to specific times via UI.

### Calendar UI
- Month‐View grid with color coding for Seasons.  
- **Highlights:**  
  - **NPC Birthdays** (portrait icon on that day).  
  - **Festivals/Events** (distinct icons).  
  - **Season Transitions** indicated by colored headers.  
- **Navigation:** Move forward/back by month or season; current date shown prominently.  
- **Interactive Access:** Open via Calendar Stand in Town or hotkey; click a day to view birthdays and events.

---

## 🌦️ Weather System

### Rain Events
- Each day has a **Chance of Rain** based on the current Season (e.g., higher in spring).  
- Random timing within a configured window (e.g., 8 AM–4 PM), driven by Coroutines.  
- **Scheduled Rain:** Certain story or festival days force Rain for quests or ambiance.  
- **Automatic Effects:**  
  - Rain waters all Tilled & Planted tiles (no manual watering needed that day).  
  - Rain prevents crop withering due to lack of watering.  
  - Rain influences NPC schedules (seek shelter).  
  - Visuals: Particle systems for raindrops, puddles, and ambient sound.

---

## 💾 Save & Load System

### Complete Game Snapshot
- Captures:  
  - **Farm State:** Tile statuses, obstacles, crop data (state, timers, quality).  
  - **Inventory & Tools:** Quantities, equipped items.  
  - **Player Stats:** Currency, position, current time and season.  
  - **Animal Data:** Incubator slots, all animals (age, mood, fondness).  
  - **NPC Relationships:** Friendship points, unlocked events.  
  - **Calendar & Weather:** Exact timestamp, pending rain.  
  - **Shop & Shipping:** Stock levels, items queued for shipment.

### Serialization Methods
- **JSON** for readability; **binary** for space efficiency.  
- Data Transfer Objects (DTOs) handle nested structures (lists, dictionaries, enums).  
- Save triggered manually (Save button) or automatically at night’s end.  
- Load restores the exact game state: camera position, UI elements, and running Coroutines (e.g., crop growth).

---

## 🎮 Player & Interaction Systems

### Movement & Exploration
- Standard 3D CharacterController for walking and running.  
- Directional input (WASD/Analog) moves the Player; smooth camera follow and rotation.  
- Raycast-based highlighting for interactive objects (tools, crops, animals, NPCs).  
- Interaction Prompts appear above objects (“Press [E] to…”).

### Tool Usage & Inventory
- **Inventory:**  
  - Tools (`Axe`, `Pickaxe`, `Shovel`, `Hoe`, `Watering Can`, `Incubator Key`) occupy slots.  
  - Seeds/Produce/Animal Feed/Items in stackable consumable slots.  
  - Quick-Select Wheel allows switching tools/seeds on the fly.  
- **Tool Logic:**  
  - **Axe:** Swing animation → checks for Woody Obstacle in range → removes and adds “Wood” to Inventory.  
  - **Pickaxe** and **Shovel** operate similarly for Rocks and Weeds.  
  - **Hoe:** Toggles “Tilling Mode”: Player targets a tile → press [E] to till.  
  - **Watering Can:** Toggles “Watering Mode”: Tap tiles to water until can is empty or all targeted.  
  - **Seed Placement Mode:** With seeds selected, point at Tilled soil → press [E] to plant.  
  - **Incubator Interact:** Stand near incubator → press [E] to “Insert Egg” or “Collect Chick.”  
  - **Feed Animal:** With feed selected, approach Chicken → press [E] to feed.  
  - **Talk to NPC:** Approach until a prompt appears → press [E] to open Dialogue UI (option to “Give Gift” if holding an item).

### Energy/Stamina (Optional Future Expansion)
- No stamina in the current version; tasks can be performed indefinitely.  
- (Potential future DLC may add fatigue mechanics.)

---

## 🖥️ UI & Dialogue

### Core UI Panels
- **Inventory Panel:** Grid layout showing tools, stacks, and equipped item.  
- **Toolbelt/Quick Bar:** HUD display of up to 8 quick-select items.  
- **Stats Overlay:** Currency, current date & time, season icon, daily Shipping preview.  
- **Calendar Window:** Interactive month grid (see Calendar & Time).  
- **Shop Window:** Item grid with icons, names, prices, quantity selector, and purchase/close buttons.  
- **Shipping Bin Window:** Lists deposited items (icon, quantity, expected price) and total.  
- **Animal Status Window:** For selected animal—Mood icon, Fondness hearts, hunger bar.  
- **Dialogue Box:**  
  - Shows **Speaker Name** and **Portrait** at top.  
  - **Typewriter Text** area for dialogue lines.  
  - **Press [E]** prompts to continue; callbacks trigger after completion.  
  - **Choice Dialogues** appear when multiple response options exist.

### HUD Elements
- On-screen Tool Icon & Durability (if applicable).  
- Watering Can Gauge (if limited water; otherwise infinite).  
- Day/Night Indicator (sun/moon icon) next to clock.  
- Weather Icon (rain cloud when raining).  
- On-screen Prompts above interactables (“Press [E] to Talk,” “Press [E] to Plant,” etc.).

### UI Architecture Concepts
- **MVC (Model-View-Controller):**  
  - **Models** hold data (`InventoryModel`, `CropModel`, `AnimalModel`, `NPCModel`).  
  - **Views** (MonoBehaviour scripts) render UI and listen for input.  
  - **Controllers** handle logic (`InventoryController`, `ShopController`, `DialogueController`).  
  - **Observer/Event System** propagates changes: when a model updates (e.g., inventory change), views automatically refresh.

---

## 🔊 Audio & Sound

### Sound Management Concepts
- **Singleton SoundManager** (example): Provides a single access point for all SFX and BGM.  
- **SoundType Enum** categorizes sounds (`UI_CLICK`, `FOOTSTEP`, `CHOP`, `DIG`, `WATER`, `CROP_GROWTH`, `ANIMAL_CHIRP`, `RAIN_AMBIENCE`, `SHOP_INTERACT`, etc.).  
- **One-Shot SFX:** Played via `PlayOneShot` for tool swings, footsteps, and short events.  
- **BGM Loops:**  
  - **Overworld Theme** loops while on the farm.  
  - **Town Theme** plays when entering the town area.  
  - **Night Ambience** (e.g., crickets) after sundown.  
  - Transitions use fade-in/out between tracks.  
- **Volume Controls:** Exposed in Options UI (Master, Music, SFX sliders).

---

## 🔧 Architecture & Code Concepts

### Design Patterns
- **MVC (Model-View-Controller)**  
  - *Example:* `InventoryModel` holds items → `InventoryView` displays them → `InventoryController` manages adding/removing items.  
- **Observer/Event Pattern**  
  - *Example:* `CropSystem` subscribes to `TimeManager` ticks; each hour triggers growth stage checks.  
- **Singleton Pattern**  
  - *Example:* A central `SoundManager` ensures only one audio manager exists in the scene.  
- **State Pattern**  
  - *Example:* Crop objects transition through states (_Seed → Seedling → Harvestable → Wilted_) using dedicated state classes.  
- **Factory & Object Pooling**  
  - *Example:* Crop Prefabs are spawned and recycled through a pooling system to minimize garbage collection spikes.  
- **ScriptableObjects & Enums**  
  - *Example:* Crop Definitions (growth times, sprite references) live in ScriptableObjects; `ToolType` and `AnimalMood` are defined via Enums.  
- **Dependency Injection & Interfaces**  
  - *Example:* Any component wanting time updates implements an `ITimeListener` interface and registers with the TimeManager.  
- **Encapsulation & Namespacing**  
  - *Example:* Animal classes keep mood and fondness private, exposing only safe methods to modify them; code organized under namespaces like `BlossomValley.Farm` or `BlossomValley.Animal`.

### Coroutines (Unity)
- Used for asynchronous operations:  
  - **Crop Growth Timers:** Wait for in-game hours/days before advancing a crop’s stage.  
  - **Incubator Hatching Countdown:** Automatically spawns chicks after a set period.  
  - **Rain Duration:** Starts and stops rain particle systems at scheduled times.

### Generic Collections & Data Structures
- Lists track active Crops, Animals, NPCs, and Shop Items.  
- Dictionaries map tile coordinates to `TileStatus` (e.g., `Untilled`, `Tilled–Wet`, `Planted`).  
- Structs like `GameTimestamp` hold composite time data (Day, Hour, Season, Year).  
- Data Transfer Objects (DTOs) package nested structures for Save/Load.

---

## 📝 A Note on the Journey
Building Blossom Valley has been a labor of love—combining relaxing farming loops with robust, data-driven architecture that balances performance and extensibility. Every system, from tiling soil to nurturing chickens, from forging town relationships to weathering seasonal rains, is designed for:

- **Maintainability** through clear separation of concerns (MVC, Observer/Event).  
- **Extensibility** via ScriptableObjects so designers can tweak crop stats, animal moods, and NPC gift preferences without touching code.  
- **Performance** using object pooling, coroutines, and lightweight save structures to ensure smooth gameplay on modest hardware.  
- **Immersion** through synchronized audio-visual feedback: rain that waters your crops, camera shakes when an Axe fells a stump, and NPCs who greet you by name.

There’s always room to evolve—perhaps add fishing, cooking, advanced livestock genetics, or multiplayer co-op. Feedback and collaboration are welcome—let’s cultivate Blossom Valley together and watch our communities flourish! 🌸

https://youtu.be/AS5MBmQLmX4

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
