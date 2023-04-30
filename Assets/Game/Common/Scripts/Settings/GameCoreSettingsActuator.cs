using Ballance2.Services;

namespace Ballance2.Game
{
  class GameCoreSettingsActuator : GameSettingsActuator
  {
    public GameCoreSettingsActuator() : base(GamePackageManager.SYSTEM_PACKAGE_NAME)
    {
    }

    private T GetDefaultValue<T>(string key) {
      if (SettingConstants.SettingsDefaultValues.TryGetValue(key, out var c))
        return (T)c;
      return default(T);
    }

    public override string GetString(string key)
    {
      return GetString(key, GetDefaultValue<string>(key));
    }
    public override float GetFloat(string key)
    {
      return GetFloat(key, GetDefaultValue<float>(key));
    }    
    public override bool GetBool(string key)
    {
      return GetBool(key, GetDefaultValue<bool>(key));
    }
    public override int GetInt(string key)
    {
      return GetInt(key, GetDefaultValue<int>(key));
    }
  }
}