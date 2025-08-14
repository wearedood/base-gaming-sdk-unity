using System;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;

namespace BaseSDK
{
    /// <summary>
    /// NFT Manager for handling NFT operations on Base blockchain
    /// Supports minting, transferring, and querying NFT collections
    /// </summary>
    public class NFTManager : MonoBehaviour
    {
        [Header("NFT Collection Settings")]
        [SerializeField] private string defaultCollectionAddress = "0x...";
        [SerializeField] private string nftStorageApiKey = "";
        
        [Header("Display Settings")]
        [SerializeField] private Transform nftDisplayParent;
        [SerializeField] private GameObject nftPrefab;
        
        // Events
        public static event Action<NFTData> OnNFTMinted;
        public static event Action<NFTData> OnNFTTransferred;
        public static event Action<List<NFTData>> OnNFTsLoaded;
        public static event Action<string> OnNFTError;
        
        // NFT Data Structure
        [System.Serializable]
        public class NFTData
        {
            public string tokenId;
            public string name;
            public string description;
            public string imageUrl;
            public string metadataUrl;
            public string owner;
            public Dictionary<string, object> attributes;
            
            public NFTData()
            {
                attributes = new Dictionary<string, object>();
            }
        }
        
        private List<NFTData> ownedNFTs = new List<NFTData>();
        
        private void Start()
        {
            // Subscribe to BaseSDK events
            BaseSDK.OnWalletConnected += OnWalletConnected;
            BaseSDK.OnWalletDisconnected += OnWalletDisconnected;
        }
        
        private void OnDestroy()
        {
            BaseSDK.OnWalletConnected -= OnWalletConnected;
            BaseSDK.OnWalletDisconnected -= OnWalletDisconnected;
        }
        
        private async void OnWalletConnected(string walletAddress)
        {
            Debug.Log("[NFTManager] Wallet connected, loading NFTs...");
            await LoadUserNFTs(walletAddress);
        }
        
        private void OnWalletDisconnected()
        {
            Debug.Log("[NFTManager] Wallet disconnected, clearing NFTs...");
            ClearNFTs();
        }
        
        /// <summary>
        /// Load all NFTs owned by the user
        /// </summary>
        public async Task LoadUserNFTs(string walletAddress)
        {
            try
            {
                Debug.Log($"[NFTManager] Loading NFTs for wallet: {walletAddress}");
                
                // Simulate API call to fetch NFTs
                await Task.Delay(1000);
                
                // Mock NFT data
                ownedNFTs.Clear();
                for (int i = 0; i < 3; i++)
                {
                    var nft = new NFTData
                    {
                        tokenId = i.ToString(),
                        name = $"Base Game Item #{i + 1}",
                        description = $"A rare gaming item from the Base ecosystem",
                        imageUrl = $"https://example.com/nft/{i}.png",
                        metadataUrl = $"https://example.com/metadata/{i}.json",
                        owner = walletAddress
                    };
                    
                    nft.attributes["rarity"] = i == 0 ? "Legendary" : i == 1 ? "Epic" : "Rare";
                    nft.attributes["power"] = (i + 1) * 100;
                    
                    ownedNFTs.Add(nft);
                }
                
                OnNFTsLoaded?.Invoke(ownedNFTs);
                DisplayNFTs();
                
                Debug.Log($"[NFTManager] Loaded {ownedNFTs.Count} NFTs");
            }
            catch (Exception ex)
            {
                OnNFTError?.Invoke($"Failed to load NFTs: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Mint a new NFT
        /// </summary>
        public async Task<bool> MintNFT(string name, string description, string imageUrl, Dictionary<string, object> attributes = null)
        {
            if (!BaseSDK.Instance.IsWalletConnected)
            {
                OnNFTError?.Invoke("Wallet not connected");
                return false;
            }
            
            try
            {
                Debug.Log($"[NFTManager] Minting NFT: {name}");
                
                // Simulate minting process
                await Task.Delay(2000);
                
                var newNFT = new NFTData
                {
                    tokenId = UnityEngine.Random.Range(1000, 9999).ToString(),
                    name = name,
                    description = description,
                    imageUrl = imageUrl,
                    owner = BaseSDK.Instance.ConnectedWallet,
                    attributes = attributes ?? new Dictionary<string, object>()
                };
                
                ownedNFTs.Add(newNFT);
                OnNFTMinted?.Invoke(newNFT);
                
                Debug.Log($"[NFTManager] NFT minted successfully: {newNFT.tokenId}");
                return true;
            }
            catch (Exception ex)
            {
                OnNFTError?.Invoke($"Failed to mint NFT: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Transfer NFT to another address
        /// </summary>
        public async Task<bool> TransferNFT(string tokenId, string toAddress)
        {
            if (!BaseSDK.Instance.IsWalletConnected)
            {
                OnNFTError?.Invoke("Wallet not connected");
                return false;
            }
            
            try
            {
                Debug.Log($"[NFTManager] Transferring NFT {tokenId} to {toAddress}");
                
                var nft = ownedNFTs.Find(n => n.tokenId == tokenId);
                if (nft == null)
                {
                    OnNFTError?.Invoke("NFT not found");
                    return false;
                }
                
                // Simulate transfer
                await Task.Delay(1500);
                
                nft.owner = toAddress;
                ownedNFTs.Remove(nft);
                
                OnNFTTransferred?.Invoke(nft);
                
                Debug.Log($"[NFTManager] NFT transferred successfully");
                return true;
            }
            catch (Exception ex)
            {
                OnNFTError?.Invoke($"Failed to transfer NFT: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Display NFTs in the UI
        /// </summary>
        private void DisplayNFTs()
        {
            if (nftDisplayParent == null || nftPrefab == null) return;
            
            // Clear existing displays
            foreach (Transform child in nftDisplayParent)
            {
                Destroy(child.gameObject);
            }
            
            // Create new displays
            foreach (var nft in ownedNFTs)
            {
                var nftObject = Instantiate(nftPrefab, nftDisplayParent);
                var nftDisplay = nftObject.GetComponent<NFTDisplay>();
                if (nftDisplay != null)
                {
                    nftDisplay.SetNFTData(nft);
                }
            }
        }
        
        /// <summary>
        /// Clear all NFT displays
        /// </summary>
        private void ClearNFTs()
        {
            ownedNFTs.Clear();
            DisplayNFTs();
        }
        
        /// <summary>
        /// Get NFT by token ID
        /// </summary>
        public NFTData GetNFT(string tokenId)
        {
            return ownedNFTs.Find(n => n.tokenId == tokenId);
        }
        
        /// <summary>
        /// Get all owned NFTs
        /// </summary>
        public List<NFTData> GetAllNFTs()
        {
            return new List<NFTData>(ownedNFTs);
        }
    }
}
