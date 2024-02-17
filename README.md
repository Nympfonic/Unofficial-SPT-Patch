# Current List of Fixes

- Incompatible ammo in magazine notifications will no longer appear randomly
   - This was caused by a bot spawning with incompatible ammo loaded in its gun and the notification would appear if it tried to shoot its gun
   - This fix only prevents the notification from showing for the player, bots can still spawn with incompatible ammo
