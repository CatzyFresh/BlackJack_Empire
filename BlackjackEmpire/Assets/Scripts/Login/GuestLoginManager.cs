using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class GuestLoginManager : MonoBehaviour
{
    private const string FileName = "PlayerData.json";
    private string FilePath => Path.Combine(Application.persistentDataPath, FileName);
    private const string EncryptionKey = "YourEncryptionKey123"; // Replace with a secure key

    public PlayerData CurrentPlayerData { get; private set; }
    public static GuestLoginManager Instance { get; private set; }

    public static event Action OnPlayerDataUpdated; // Event for player data update

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Make this GameObject persistent
    }

    public bool HasExistingPlayerData()
    {
        return File.Exists(FilePath);
    }

    public void GuestLogin()
    {
        if (HasExistingPlayerData())
        {
            Debug.Log("Existing profile found. Loading data...");
            LoadPlayerData();
        }
        else
        {
            Debug.Log("No profile found. Creating new guest profile...");
            CreateNewGuestProfile();
        }

        // Navigate to the main menu or display player information
        NotifyPlayerDataUpdated();
        Debug.Log($"Welcome, {CurrentPlayerData.PlayerName}! Chips: {CurrentPlayerData.Chips}");
    }

    public void AutoLogin()
    {
        if (HasExistingPlayerData())
        {
            Debug.Log("Auto-logging in with existing profile...");
            LoadPlayerData();
        }
        else
        {
            Debug.LogError("No existing player data found for auto-login.");
        }

        NotifyPlayerDataUpdated();
    }

    private void CreateNewGuestProfile()
    {
        string uuid = Guid.NewGuid().ToString();
        string playerName = $"Guest_{UnityEngine.Random.Range(1000, 9999)}";
        CurrentPlayerData = new PlayerData(uuid, playerName);
        SavePlayerData();
    }

    public void SavePlayerData()
    {
        try
        {
            string json = JsonUtility.ToJson(CurrentPlayerData);
            string encryptedData = Encrypt(json, EncryptionKey);
            File.WriteAllText(FilePath, encryptedData);
            Debug.Log("Player data saved successfully.");
            NotifyPlayerDataUpdated();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error saving player data: {ex.Message}");
        }
    }

    private void LoadPlayerData()
    {
        try
        {
            string encryptedData = File.ReadAllText(FilePath);
            string decryptedJson = Decrypt(encryptedData, EncryptionKey);
            CurrentPlayerData = JsonUtility.FromJson<PlayerData>(decryptedJson);
            Debug.Log("Player data loaded successfully.");
            NotifyPlayerDataUpdated();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error loading player data: {ex.Message}");
        }
    }

    private string Encrypt(string plainText, string key)
    {
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] keyBytes = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.GenerateIV();
            byte[] iv = aes.IV;
            using (var encryptor = aes.CreateEncryptor(aes.Key, iv))
            using (var ms = new MemoryStream())
            {
                ms.Write(iv, 0, iv.Length);
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(plainBytes, 0, plainBytes.Length);
                }
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    private string Decrypt(string cipherText, string key)
    {
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        byte[] keyBytes = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;
            using (var ms = new MemoryStream(cipherBytes))
            {
                byte[] iv = new byte[16];
                ms.Read(iv, 0, 16);
                aes.IV = iv;
                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }

    private void NotifyPlayerDataUpdated()
    {
        OnPlayerDataUpdated?.Invoke();
    }
}
