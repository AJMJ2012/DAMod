using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace DAMod.Components;

// For some reason, this hook doesn't work anymore.
/*
class AllowLockOnSystem : ModSystem
{
    static readonly MethodInfo LockOnHelper_CanUseLockonSystem_Method = typeof(LockOnHelper).GetMethod("CanUseLockonSystem", BindingFlags.Public | BindingFlags.Static);
    delegate bool LockOnHelper_CanUseLockonSystem_Delegate();

    public override void Load()
	{
		MonoModHooks.Add(LockOnHelper_CanUseLockonSystem_Method, Override_LockOnHelper_CanUseLockonSystem);
	}

    // Allow lock on system when using a keyboard and mouse and if enabled
    static bool Override_LockOnHelper_CanUseLockonSystem(LockOnHelper_CanUseLockonSystem_Delegate CanUseLockonSystem)
	{
		if (DAMod.instance.lockOnKey.JustPressed) PlayerInput.Triggers.JustPressed.LockOn = true;
		return Config.Server.EnableLockOn && Config.Client.EnableLockOn || CanUseLockonSystem();
	}
}
*/

class AllowLockOnSystem : ModSystem
{
	public override void PostUpdateInput()
	{
		if (!Main.gameMenu && DAMod.instance != null && DAMod.instance.lockOnKey != null && DAMod.instance.lockOnKey.JustPressed) PlayerInput.Triggers.JustPressed.LockOn = true;
		LockOnHelper.ForceUsability = Config.Server.EnableLockOn && Config.Client.EnableLockOn;
	}
}
