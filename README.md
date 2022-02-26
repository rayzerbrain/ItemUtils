# ItemUtils
Exiled plugin allowing for extensive configuration of most base-game items.

BEFORE REPORTING BUGS/ISSUES, MAKE SURE EVERYTHING IS UP TO DATE.

### Details
99% of this plugin is operating from the configuration you must set up. Default configuration has been provided as an example, so when in doubt refer to it as an example.
The list of available configuration is listed below

# Configuration
|Config name|Data type|Description|
|-----------|---------|-----------|
|is_enabled|Boolean|Determines whether the plugin is enabled or not|
|modified_items|Dictionary<ItemType, string>|Maps items to a specified modifier declared below|
|item_modifiers|Dictionary<string, ItemModifier>|List of declared item modifiers. See below for more information|
|debug_mode|Boolean|Determines if debug logs will be shown|

### Example Config
```yml
item_utils:
  is_enabled: true
  modified_items: 
    Medkit: health_increaser
    Painkillers: health_increaser
    None: flat_item
  item_modifiers:
    health_increaser:
      excluded_roles: []
      scale: 
        x: 1
        y: 1
        z: 1
      pickup_time_multi: 1
      use_time_multi: 1.2
      cooldown_multi: 1
      hp_added: 10
      ahp_added: 0
    flat_item: 
      scale:
        x: 1
        y: 0.25
        z: 1
```
Note how a modifier definition is mapped to the item "None". This inversely represents ALL items, so use this to affect attributes of all items simultaneously.
### Modifier information
Most attributes have examples within the default config that comes with the plugin

NOTE: Many properties require specific pre-defined values, like the names of types of ammo(AmmoType) and roles(RoleType). To find the exact definition of these values, go to #resources in the exiled discord


|Modifiable Attribute|Valid Items|Description|Data type|Default value|Currently working?|
|--------------------|-----------|-----------|---------|-------------|--------------|
|excluded_roles|All|Prevents modifications from taking effect for each role in this list|List<RoleType>|[ ]|Yes|
|scale|All|Permanently changes the scale of the item|Vector3|x: 1, y: 1, z: 1|Yes|
|scp_damage_multi|Grenades and Guns|Affects the amount of damage dealt to Scps|float|1|Unknown|
|human_damage_multi|Grenades and Guns|Affects the amount of damage dealt to Humans|float|1|Unknown|
|use_time_multi|Consumables (and hat)|Affects how long it takes to completely use an item|float|1|Yes|
|cooldown_multi|Consumables (and hat)|Affects how long it takes before you can use that item after cancelling it (does NOT affect hat cooldown)|float|1|Unknown|
|hp_added|Consumables (and hat)|Adds a certain amount of hp after using the item|float|0|Yes|
|ahp_added|Consumables (and hat)|Adds an amount of ahp after using the item|float|0|Yes|
|effects|Consumables (and hat)|List of effects that the item can give after being used (these can modified in duration and chance)|List<EffectType>|[ ]|Yes|
|can_be_used_remotely|Keycards|Determines if the card can be used from the inventory|Boolean|false|Yes|
|added_permissions|Keycards|List of permissions the card will gain|List<KeycardPermission>|[ ]|Yes|
|removed_permissions|Keycards|List of permissions the card will lose|List<KeycardPermission>|[ ]|Yes|
|needs_ammo|Guns|Determines whether the gun needs ammo to fire|Boolean|true|Yes|
|needs_reloading|Guns|Determines whether the gun needs to be reloaded|Boolean|true|Yes (Shooting event only called after stopped firing)|
|can_disarm|Guns|Determines whether the gun can be used to disarm someone|Boolean|true|Unknown|
|effect_duration_multi|Grenades|Affects how long the effects of a grenade will last on a player|float|1|Unknown|
|fuse_time_multi|Grenades|Affects the fuse time of a grenade|float|1|Yes|
|aoe_multi|Grenades|Affects the AOE (area of effect) of a grenade|float|1|Unknown|
|ammo_limit_multis|Armors|Affects the ammo limits of a type of armor|Dictionary<AmmoType, float>|{ }|Yes|
|helmet_protection_multi|Armors|Affects the amount of headshot protection the armor gives|float|1|Unknown|
|body_protection_multi|Armors|Affects the amount of body protection the armor gives|float|1|Unknown|
|stamina_use_multi|Armors|Sets the stamina usage multiplier (note: can ONLY be between 1 and 2)|float|Varies|Yes|
|starting_energy_multi|Micro and Radio|Affects the amount of energy the item starts with. (note: must be less than one)|float|1|Yes|
|has_infinite_use|Micro and Radio|Determines whether the item can be used indefinitely or not|Boolean|false|Yes|
  
Last updated 2/25 19:11, Feel free to suggest additional ones.

Possibly more coming soon<sup>TM</sup>...
