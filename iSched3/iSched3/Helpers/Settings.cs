// Helpers/Settings.cs

using System;
using iSched3.Models;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace iSched3.Helpers
{
  /// <summary>
  /// This is the Settings static class that can be used in your Core solution or in any
  /// of your client applications. All settings are laid out the same exact way with getters
  /// and setters. 
  /// </summary>
  public static class Settings
  {
    private static ISettings AppSettings
    {
      get
      {
        return CrossSettings.Current;
      }
    }

    #region Setting Constants

    private const string SettingsKey = "settings_key";
    private static readonly string SettingsDefault = string.Empty;

    private const string TokenKey = "token_key";
    private static readonly string TokenDefault = string.Empty;

    private const string TokenExpirationKey = "tokenExpiration_key";
    private static readonly DateTime TokenExpirationDefault = new DateTime(2099, 6, 6);

    private const string UserNameKey = "username_key";
    private static readonly string UserNameDefault = string.Empty;

    private const string PassSecretKey = "passSecret_key";
    private static readonly string PassSecretDefault = string.Empty;

    #endregion


    public static string GeneralSettings
    {
      get
      {
        return AppSettings.GetValueOrDefault<string>(SettingsKey, SettingsDefault);
      }
      set
      {
        AppSettings.AddOrUpdateValue<string>(SettingsKey, value);
      }
    }

      public static string Token
      {
          get { return AppSettings.GetValueOrDefault<string>(TokenKey, TokenDefault); }
          set { AppSettings.AddOrUpdateValue<string>(TokenKey, value); }
      }

      public static DateTime TokenExpiration
      {
            get { return AppSettings.GetValueOrDefault<DateTime>(TokenExpirationKey, TokenExpirationDefault); }
            set { AppSettings.AddOrUpdateValue<DateTime>(TokenExpirationKey, value); }
      }

      public static string Username
      {
            get { return AppSettings.GetValueOrDefault<string>(UserNameKey, UserNameDefault); }
            set { AppSettings.AddOrUpdateValue<string>(UserNameKey, value); }
      }

      public static string PassSecret
      {
            get { return AppSettings.GetValueOrDefault<string>(PassSecretKey, PassSecretDefault); }
            set { AppSettings.AddOrUpdateValue<string>(PassSecretKey, value); }
      }

  }
}