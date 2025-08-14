using System;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BaseSDK
{
    /// <summary>
    /// Main SDK class for Base blockchain integration in Unity
    /// Provides core functionality for wallet connection, NFT operations, and smart contract interactions
    /// </summary>
    public class BaseSDK : MonoBehaviour
    {
        [Header("Base Network Configuration")]
        [SerializeField] private string rpcUrl = "https://mainnet.base.org";
        [SerializeField] private string chainId = "8453";
        
        [Header("SDK Settings")]
        [SerializeField] private bool enableLogging = true;
        [SerializeField] private bool autoConnectWallet = false;
        
        // Events
        public static event Action<string> OnWalletConnected;
        public static event Action OnWalletDisconnected;
        public static event Action<string> OnTransactionComplete;
        public static event Action<string> OnError;
        
        // Properties
        public static BaseSDK Instance { get; private set; }
        public bool IsWalletConnected { get; private set; }
        public string ConnectedWallet { get; private set; }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeSDK();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        /// <summary>
        /// Initialize the Base SDK
        /// </summary>
        private void InitializeSDK()
        {
            if (enableLogging)
                Debug.Log("[BaseSDK] Initializing Base Gaming SDK...");
                
            if (autoConnectWallet)
            {
                ConnectWallet();
            }
        }
        
        /// <summary>
        /// Connect to user's wallet
        /// </summary>
        public async Task<bool> ConnectWallet()
        {
            try
            {
                if (enableLogging)
                    Debug.Log("[BaseSDK] Attempting wallet connection...");
                
                // Simulate wallet connection (replace with actual Web3 implementation)
                await Task.Delay(1000);
                
                ConnectedWallet = "0x1234567890123456789012345678901234567890";
                IsWalletConnected = true;
                
                OnWalletConnected?.Invoke(ConnectedWallet);
                
                if (enableLogging)
                    Debug.Log($"[BaseSDK] Wallet connected: {ConnectedWallet}");
                
                return true;
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Wallet connection failed: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Disconnect wallet
        /// </summary>
        public void DisconnectWallet()
        {
            ConnectedWallet = null;
            IsWalletConnected = false;
            OnWalletDisconnected?.Invoke();
            
            if (enableLogging)
                Debug.Log("[BaseSDK] Wallet disconnected");
        }
        
        /// <summary>
        /// Get user's ETH balance
        /// </summary>
        public async Task<decimal> GetETHBalance()
        {
            if (!IsWalletConnected)
            {
                OnError?.Invoke("Wallet not connected");
                return 0;
            }
            
            try
            {
                // Simulate balance fetch
                await Task.Delay(500);
                return 1.5m; // Mock balance
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Failed to get balance: {ex.Message}");
                return 0;
            }
        }
    }
}
