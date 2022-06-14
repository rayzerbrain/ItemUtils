# ItemUtils
Exiled plugin allowing for extensive configuration of most base-game items.

EXILED 5.0 ONLY. BEFORE REPORTING BUGS/ISSUES, MAKE SURE EVERYTHING IS UP TO DATE.

### Details
99% of this plugin is operating from the configuration you must set up. Default configuration has been provided as an example, so when in doubt refer to it as an example.
The list of available configuration is listed below

# Configuration
|Config name|Data type|Description|
|-----------|---------|-----------|
|is_enabled|Boolean|Determines whether the plugin is enabled or not|
|excluded_custom_items|List\<string>|A list of custom item names. These custom items will not be affected by modifications you create|
|item_modifiers|Dictionary<string, ItemModifier>|List of declared item modifiers. See below for more information|
|debug_mode|Boolean|Determines if debug logs will be shown|

### Example Config
```yml
item_utils:
  is_enabled: true
  excluded_custom_items: []
  item_modifiers:
    health_increaser:
      affected_items:
      - Medkit
      - Painkillers
      excluded_roles: []
      scale: 
        x: 1.1
        y: 1
        z: 1
      use_time_multi: 1.2
      hp_added: 10
    flat_item: 
      affected_items:
      - None
      scale:
        x: 1
        y: 0.25
        z: 1
    modified logicer:
      affected_items:
      - GunLogicer
      modified_attachments:
        None:
          DamageMultiplier: 1.01
        Foregrip:
          HipInaccuracyMultiplier: 1.05
        Flashlight: 
          BulletInaccuracyMultiplier: 0.9
  debug_mode: false
```
Note how the flat_item modifier affects the item "None". This inversely represents ALL items, so use this to affect attributes of all items simultaneously. This is also true for attachments: using None as the modifier name will affect the default gun's stats regardless of the attachments on it.

Also note that since the `flat_item` modifier is declared after `health_increaser`, `flat_item` has "higher priority".
This is important because the modifier with the higher priority will take effect LAST. 
In the above case, `flat_item`'s scale will take effect for all items after any other scale changes before it, rendering the given scale for `health_increaser` useless.
### Modifier information
Most attributes have examples within the default config that comes with the plugin

NOTE: Many properties require specific pre-defined values, like the names of types of items(ItemType) and roles(RoleType). To find the exact definition of these values, go to #resources in the exiled discord or ask around


|Modifiable Attribute|Valid Items|Data type|Default value|Description|
|--------------------|-----------|---------|-------------|-----------|
|affected_items|All|List\<ItemType>|[ ]|The list of items that will receive modifications|
|ignored_roles|All|List\<RoleType>|[ ]|Prevents modifications from taking effect for each role in this list|
|scale|All|Vector3|x: 1, y: 1, z: 1|Permanently changes the scale of the item|
|scp_damage_multi|Grenades and Guns|float|1|Affects the amount of damage dealt to Scps|
|human_damage_multi|Grenades and Guns|float|1|Affects the amount of damage dealt to Humans|
|use_time_multi|Consumables (and hat)|float|1|Affects how long it takes to completely use an item|
|hp_added|Consumables (and hat)|float|0|Adds a certain amount of hp after using the item|
|ahp_added|Consumables (and hat)|float|0|Adds an amount of ahp after using the item|
|effects|Consumables (and hat)|List\<EffectType>|[ ]|List of effects that the item can give after being used (these can modified in duration and chance)|
|can_be_used_remotely|Keycards|Boolean|false|Determines if the card can be used from the inventory|
|added_permissions|Keycards|List\<KeycardPermission>|[ ]|List of permissions the card will gain|
|removed_permissions|Keycards|List\<KeycardPermission>|[ ]|List of permissions the card will lose|
|needs_ammo|Guns|Boolean|true|Determines whether the gun needs ammo to fire|
|can_disarm|Guns|Boolean|true|Determines whether the gun can be used to disarm someone|
|weight_multi|Guns|float|1|Affects the default weight of a firearm|
|length_multi|Guns|float|1|Affects the default length of a firearm|
|modified_attachments|Guns|Dictionary|{ }|A list of modified attachments the gun will have. See the config above for an example|
|effect_duration_multi|Grenades|float|1|Affects how long the effects of a grenade will last on a player|
|fuse_time_multi|Grenades|float|1|Affects the fuse time of a grenade|
|ammo_limit_multis|Armors|Dictionary<AmmoType, float>|{ }|Affects the ammo limits of a type of armor (currently unavailable due to client side checks)|
|helmet_protection_multi|Armors|float|1|Affects the amount of headshot protection the armor gives|
|body_protection_multi|Armors|float|1|Affects the amount of body protection the armor gives|
|stamina_use_multi|Armors|float|Varies|Sets the stamina usage multiplier (note: can ONLY be between 1 and 2)|
|starting_energy_multi|Micro and Radio|float|1|Affects the amount of energy the item starts with. (note: must be less than or equal to one)|
|has_infinite_use|Micro and Radio|Boolean|false|Determines whether the item can be used indefinitely or not|
  
Feel free to suggest additional ones.

# Attachments
Version 1.2 brings modifiable attachments. MANY modifiers/parameters are NOT able to be affected, like the zoom/speed/loudness, but things like recoil/inaccuracy/damage are.

See below for a list of modifiable attachment attributes. 

NOTE: if an attachment already affects that specific modifier, it's value will be changed, not completely replaced.
Not all of the values are directly multiplied; Running the plugin with debug mode enabled lets you see how the values are changed.

### Attachment Modifiers (AttachmentParam)

```yaml
AdsZoomMultiplier 
AdsMouseSensitivityMultiplier 
DamageMultiplier 
PenetrationMultiplier 
FireRateMultiplier 
OverallRecoilMultiplier 
AdsRecoilMultiplier 
BulletInaccuracyMultiplier 
HipInaccuracyMultiplier 
AdsInaccuracyMultiplier 
DrawSpeedMultiplier 
GunshotLoudnessMultiplier 
MagazineCapacityModifier 
DrawTimeModifier 
ReloadTimeModifier 
ShotClipIdOverride 
AdsSpeedMultiplier 
SpreadMultiplier 
SpreadPredictability
```
