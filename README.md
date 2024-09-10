# unity-inventory-system
This is a code snippet/modular system that I use for my Unity projects that require an inventory system.

It features:

Item Manager - Meant to handle the loading and unloading of items in the scene from Save Data

Inventory Scripts
Inventory Slot - One unit or tile of inventory space; contains methods for single items and stackable items.
Inventory Holder - Classes derive from this class if they are able to hold items (Player Inventory, Chest Inventory, etc.) Made up of inventory slots.
Chest Inventory - Takes in an ID for a chest inventory and loads a gameobject's inventory with that item data.

Inventory System - Determines whether or not an Inventory slot is empty, 
contains an item that is different from the Mouse Data Item, 
contains an item that is the same as the Mouse Data Item but is not stackable or stack is full,
or contains an item that is the same as the Mouse Data Item that is stackable and there is room available in the stack.


Mouse Item Data -
Allows the player to click on an Inventory Slot that contains an item to attach it to the cursor.
Allows the player to release an item to an available inventory slot and updates accordingly.
Handles spillover in the event that the Mouse Data Item completes a stack with some leftover.

Player Inventory Holder - 
Player inventory. Using 'I' key to open and close the window.

ReadOnlyAttribute - Used for UniqueID
UniqueID - Attach as a component to a game object. Running the game will populate this field with a persistent and Unique ID; useful for saving, loading, and preserving uniqueness.


Inventory UI
Inventory UI Controller - Handles requests for loading and unloading UI elements specific to the inventory system.

Inventory Display - Base class for all UI Inventory interactions. Allows for stack splitting and updating Inventory Slots

Inventory Slot - Single unit tile for containing item data. Inventory displays are made up of these.

Dynamic Inventory Display - Allows developer to set dynamic number of slots for an inventory.
Static Inventory Display - An invetory display that does not change. (Player Hotbar)

Player Hotbar Display - Static Inventory Display; set number of slots in editor, using offset to make sure that the first x amount of items go to this display and spillover into a player's backpack. Contains controls for mousewheel scrolling and highlighting active item.


Item Scripts
Resources - Pre-created item data examples.
Inventory Item Data - Asset menu template for creating new item data using scriptable objects.
Item Database - Asset menu template for creating a database for items using scriptable objects.
Item Pick-up - Allows the player to perform actions to pick-up items either using an interaction system or option to toggle auto-pick-up for the item.
