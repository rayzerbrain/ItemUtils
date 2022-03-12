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
        HipInaccuracyMultiplier: 1.05
  debug_mode: false
```
Note how the flat_item modifier affects the item "None". This inversely represents ALL items, so use this to affect attributes of all items simultaneously. This is also true for attachments: using None as the modifier name will affect the default gun's stats regardless of the attachments on it.

Also note that since the `flat_item` modifier is declared after `health_increaser`, `flat_item` has "higher priority".
This is important because the modifier with the higher priority will take effect LAST. 
In the above case, `flat_item`'s scale will take effect for all items after any other scale changes before it, rendering the given scale for `health_increaser` useless.
### Modifier information
Most attributes have examples within the default config that comes with the plugin

NOTE: Many properties require specific pre-defined values, like the names of types of items(ItemType) and roles(RoleType). To find the exact definition of these values, go to #resources in the exiled discord or ask around


|Modifiable Attribute|Valid Items|Description|Data type|Default value|
|--------------|------|----------------------------------|------|-------------|
|affected_items|All|The list of items that will receive modifications|List\<ItemType>|[ ]|
|ignored_roles|All|Prevents modifications from taking effect for each role in this list|List\<RoleType>|[ ]|
|scale|All|Permanently changes the scale of the item|Vector3|x: 1, y: 1, z: 1|
|scp_damage_multi|Grenades and Guns|Affects the amount of damage dealt to Scps|float|1|
|human_damage_multi|Grenades and Guns|Affects the amount of damage dealt to Humans|float|1|
|use_time_multi|Consumables (and hat)|Affects how long it takes to completely use an item|float|1|
|hp_added|Consumables (and hat)|Adds a certain amount of hp after using the item|float|0|
|ahp_added|Consumables (and hat)|Adds an amount of ahp after using the item|float|0|
|effects|Consumables (and hat)|List of effects that the item can give after being used (these can modified in duration and chance)|List\<EffectType>|[ ]|
|can_be_used_remotely|Keycards|Determines if the card can be used from the inventory|Boolean|false|
|added_permissions|Keycards|List of permissions the card will gain|List\<KeycardPermission>|[ ]|
|removed_permissions|Keycards|List of permissions the card will lose|List\<KeycardPermission>|[ ]|
|needs_ammo|Guns|Determines whether the gun needs ammo to fire|Boolean|true|
|can_disarm|Guns|Determines whether the gun can be used to disarm someone|Boolean|true|
|modified_attachments|Guns|A list of modified attachments the gun will have. See the config above for an example|Dictionary<AttachmentNameTranslation, Dictionary<AttachmentParam, float>>|{ }
|effect_duration_multi|Grenades|Affects how long the effects of a grenade will last on a player|float|1|
|fuse_time_multi|Grenades|Affects the fuse time of a grenade|float|1|
|ammo_limit_multis|Armors|Affects the ammo limits of a type of armor (currently unavailable due to client side checks)|Dictionary<AmmoType, float>|{ }|
|helmet_protection_multi|Armors|Affects the amount of headshot protection the armor gives|float|1|
|body_protection_multi|Armors|Affects the amount of body protection the armor gives|float|1|
|stamina_use_multi|Armors|Sets the stamina usage multiplier (note: can ONLY be between 1 and 2)|float|Varies|
|starting_energy_multi|Micro and Radio|Affects the amount of energy the item starts with. (note: must be less than or equal to one)|float|1|
|has_infinite_use|Micro and Radio|Determines whether the item can be used indefinitely or not|Boolean|false|
  
Last updated 2/25 22:02, Feel free to suggest additional ones.

Possibly more coming soon<sup>TM</sup>...

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
