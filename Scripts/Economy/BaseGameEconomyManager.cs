using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Nethereum.Web3;
using Nethereum.Contracts;
using System.Numerics;

namespace BaseGamingSDK.Economy
{
    /// <summary>
    /// BaseGameEconomyManager - Comprehensive in-game economy management for Base blockchain games
    /// Handles token economics, NFT trading, rewards distribution, and DeFi integrations
    /// </summary>
    public class BaseGameEconomyManager : MonoBehaviour
    {
        [Header("Base Network Configuration")]
        [SerializeField] private string baseRpcUrl = "https://mainnet.base.org";
        [SerializeField] private string gameTokenAddress;
        [SerializeField] private string nftContractAddress;
        [SerializeField] private string stakingContractAddress;
        
        [Header("Economy Settings")]
        [SerializeField] private float dailyRewardMultiplier = 1.0f;
        [SerializeField] private int maxDailyRewards = 1000;
        [SerializeField] private float stakingAPY = 12.0f;
        [SerializeField] private bool enableDynamicPricing = true;
        
        [Header("Game Balance")]
        [SerializeField] private List<GameItem> gameItems = new List<GameItem>();
        [SerializeField] private List<RewardTier> rewardTiers = new List<RewardTier>();
        
        // Events
        public event Action<string, BigInteger> OnTokensEarned;
        public event Action<string, BigInteger> OnTokensSpent;
        public event Action<string, int> OnNFTMinted;
        public event Action<string, string, BigInteger> OnNFTTraded;
        public event Action<string, BigInteger> OnStakingReward;
        public event Action<string> OnEconomyError;
        
        // Private fields
        private Web3 web3;
        private Contract gameTokenContract;
        private Contract nftContract;
        private Contract stakingContract;
        private Dictionary<string, PlayerEconomyData> playerData = new Dictionary<string, PlayerEconomyData>();
        private MarketDataProvider marketData;
        private RewardCalculator rewardCalculator;
        
        // Structs and Classes
        [Serializable]
        public class GameItem
        {
            public string itemId;
            public string name;
            public ItemType type;
            public BigInteger basePrice;
            public float demandMultiplier;
            public bool isNFT;
            public ItemRarity rarity;
            public Dictionary<string, object> attributes;
        }
        
        [Serializable]
        public class RewardTier
        {
            public string tierId;
            public int minLevel;
            public float multiplier;
            public BigInteger dailyBonus;
            public List<string> unlockedItems;
        }
        
        public class PlayerEconomyData
        {
            public string playerId;
            public BigInteger tokenBalance;
            public BigInteger totalEarned;
            public BigInteger totalSpent;
            public List<OwnedNFT> nftInventory;
            public StakingPosition stakingPosition;
            public DateTime lastRewardClaim;
            public int playerLevel;
            public float reputationScore;
        }
        
        public class OwnedNFT
        {
            public BigInteger tokenId;
            public string contractAddress;
            public Dictionary<string, object> metadata;
            public DateTime acquiredDate;
            public BigInteger purchasePrice;
        }
        
        public class StakingPosition
        {
            public BigInteger stakedAmount;
            public DateTime stakeStartTime;
            public BigInteger accumulatedRewards;
            public float currentAPY;
        }
        
        public enum ItemType { Weapon, Armor, Consumable, Cosmetic, Utility, Land, Character }
        public enum ItemRarity { Common, Uncommon, Rare, Epic, Legendary, Mythic }
        
        private void Start()
        {
            InitializeEconomy();
        }
        
        /// <summary>
        /// Initializes the game economy system
        /// </summary>
        private async void InitializeEconomy()
        {
            try
            {
                web3 = new Web3(baseRpcUrl);
                
                // Initialize contracts
                if (!string.IsNullOrEmpty(gameTokenAddress))
                {
                    gameTokenContract = web3.Eth.GetContract(GetERC20ABI(), gameTokenAddress);
                }
                
                if (!string.IsNullOrEmpty(nftContractAddress))
                {
                    nftContract = web3.Eth.GetContract(GetERC721ABI(), nftContractAddress);
                }
                
                if (!string.IsNullOrEmpty(stakingContractAddress))
                {
                    stakingContract = web3.Eth.GetContract(GetStakingABI(), stakingContractAddress);
                }
                
                // Initialize market data provider
                marketData = new MarketDataProvider(web3);
                rewardCalculator = new RewardCalculator(rewardTiers, dailyRewardMultiplier);
                
                Debug.Log("Base Game Economy Manager initialized successfully");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to initialize economy: {ex.Message}");
                OnEconomyError?.Invoke($"Initialization failed: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Registers a new player in the economy system
        /// </summary>
        public void RegisterPlayer(string playerId, string walletAddress)
        {
            if (!playerData.ContainsKey(playerId))
            {
                playerData[playerId] = new PlayerEconomyData
                {
                    playerId = playerId,
                    tokenBalance = BigInteger.Zero,
                    totalEarned = BigInteger.Zero,
                    totalSpent = BigInteger.Zero,
                    nftInventory = new List<OwnedNFT>(),
                    stakingPosition = new StakingPosition(),
                    lastRewardClaim = DateTime.UtcNow,
                    playerLevel = 1,
                    reputationScore = 100.0f
                };
                
                Debug.Log($"Player {playerId} registered in economy system");
            }
        }
        
        /// <summary>
        /// Awards tokens to a player for gameplay achievements
        /// </summary>
        public async Task<bool> AwardTokens(string playerId, BigInteger amount, string reason)
        {
            try
            {
                if (!playerData.ContainsKey(playerId))
                {
                    RegisterPlayer(playerId, "");
                }
                
                var player = playerData[playerId];
                
                // Apply reward multipliers
                var finalAmount = rewardCalculator.CalculateReward(amount, player.playerLevel, reason);
                
                // Update player data
                player.tokenBalance += finalAmount;
                player.totalEarned += finalAmount;
                
                OnTokensEarned?.Invoke(playerId, finalAmount);
                
                Debug.Log($"Awarded {finalAmount} tokens to {playerId} for {reason}");
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to award tokens: {ex.Message}");
                OnEconomyError?.Invoke($"Token award failed: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Processes item purchase with dynamic pricing
        /// </summary>
        public async Task<bool> PurchaseItem(string playerId, string itemId, int quantity = 1)
        {
            try
            {
                var item = gameItems.Find(i => i.itemId == itemId);
                if (item == null)
                {
                    Debug.LogError($"Item {itemId} not found");
                    return false;
                }
                
                var player = playerData[playerId];
                var totalPrice = CalculateItemPrice(item, quantity);
                
                if (player.tokenBalance < totalPrice)
                {
                    Debug.LogWarning($"Insufficient balance for {playerId}");
                    return false;
                }
                
                // Process purchase
                player.tokenBalance -= totalPrice;
                player.totalSpent += totalPrice;
                
                // Update item demand (affects future pricing)
                if (enableDynamicPricing)
                {
                    item.demandMultiplier += 0.01f * quantity;
                }
                
                OnTokensSpent?.Invoke(playerId, totalPrice);
                
                Debug.Log($"Player {playerId} purchased {quantity}x {item.name} for {totalPrice} tokens");
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Purchase failed: {ex.Message}");
                OnEconomyError?.Invoke($"Purchase failed: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Mints an NFT for a player
        /// </summary>
        public async Task<bool> MintNFT(string playerId, string metadataUri, ItemRarity rarity)
        {
            try
            {
                if (nftContract == null)
                {
                    Debug.LogError("NFT contract not initialized");
                    return false;
                }
                
                // This would interact with the actual NFT contract
                // For now, we'll simulate the minting process
                var tokenId = GenerateTokenId();
                
                var nft = new OwnedNFT
                {
                    tokenId = tokenId,
                    contractAddress = nftContractAddress,
                    metadata = new Dictionary<string, object>
                    {
                        ["uri"] = metadataUri,
                        ["rarity"] = rarity.ToString(),
                        ["mintDate"] = DateTime.UtcNow.ToString()
                    },
                    acquiredDate = DateTime.UtcNow,
                    purchasePrice = BigInteger.Zero
                };
                
                playerData[playerId].nftInventory.Add(nft);
                OnNFTMinted?.Invoke(playerId, (int)tokenId);
                
                Debug.Log($"Minted NFT {tokenId} for player {playerId}");
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"NFT minting failed: {ex.Message}");
                OnEconomyError?.Invoke($"NFT minting failed: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Processes NFT trading between players
        /// </summary>
        public async Task<bool> TradeNFT(string sellerId, string buyerId, BigInteger tokenId, BigInteger price)
        {
            try
            {
                var seller = playerData[sellerId];
                var buyer = playerData[buyerId];
                
                var nft = seller.nftInventory.Find(n => n.tokenId == tokenId);
                if (nft == null)
                {
                    Debug.LogError("NFT not found in seller's inventory");
                    return false;
                }
                
                if (buyer.tokenBalance < price)
                {
                    Debug.LogError("Buyer has insufficient balance");
                    return false;
                }
                
                // Process trade
                seller.nftInventory.Remove(nft);
                buyer.nftInventory.Add(nft);
                
                buyer.tokenBalance -= price;
                seller.tokenBalance += price;
                
                // Take marketplace fee (2.5%)
                var fee = price * 25 / 1000;
                seller.tokenBalance -= fee;
                
                OnNFTTraded?.Invoke(sellerId, buyerId, price);
                
                Debug.Log($"NFT {tokenId} traded from {sellerId} to {buyerId} for {price} tokens");
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"NFT trade failed: {ex.Message}");
                OnEconomyError?.Invoke($"NFT trade failed: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Stakes tokens for rewards
        /// </summary>
        public async Task<bool> StakeTokens(string playerId, BigInteger amount)
        {
            try
            {
                var player = playerData[playerId];
                
                if (player.tokenBalance < amount)
                {
                    Debug.LogError("Insufficient balance for staking");
                    return false;
                }
                
                player.tokenBalance -= amount;
                player.stakingPosition.stakedAmount += amount;
                player.stakingPosition.stakeStartTime = DateTime.UtcNow;
                player.stakingPosition.currentAPY = stakingAPY;
                
                Debug.Log($"Player {playerId} staked {amount} tokens");
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Staking failed: {ex.Message}");
                OnEconomyError?.Invoke($"Staking failed: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Claims staking rewards
        /// </summary>
        public async Task<BigInteger> ClaimStakingRewards(string playerId)
        {
            try
            {
                var player = playerData[playerId];
                var stakingPos = player.stakingPosition;
                
                if (stakingPos.stakedAmount == 0)
                {
                    return BigInteger.Zero;
                }
                
                var stakingDays = (DateTime.UtcNow - stakingPos.stakeStartTime).TotalDays;
                var rewards = stakingPos.stakedAmount * (BigInteger)(stakingPos.currentAPY / 365 * stakingDays) / 100;
                
                player.tokenBalance += rewards;
                stakingPos.accumulatedRewards += rewards;
                stakingPos.stakeStartTime = DateTime.UtcNow; // Reset for next calculation
                
                OnStakingReward?.Invoke(playerId, rewards);
                
                Debug.Log($"Player {playerId} claimed {rewards} staking rewards");
                return rewards;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Reward claim failed: {ex.Message}");
                OnEconomyError?.Invoke($"Reward claim failed: {ex.Message}");
                return BigInteger.Zero;
            }
        }
        
        /// <summary>
        /// Calculates dynamic item pricing based on demand
        /// </summary>
        private BigInteger CalculateItemPrice(GameItem item, int quantity)
        {
            var basePrice = item.basePrice * quantity;
            
            if (enableDynamicPricing)
            {
                var demandAdjustment = (BigInteger)(basePrice * (decimal)item.demandMultiplier);
                return basePrice + demandAdjustment;
            }
            
            return basePrice;
        }
        
        /// <summary>
        /// Generates a unique token ID for NFTs
        /// </summary>
        private BigInteger GenerateTokenId()
        {
            return new BigInteger(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        }
        
        // Contract ABI methods (simplified)
        private string GetERC20ABI()
        {
            return "[{\"constant\":true,\"inputs\":[{\"name\":\"_owner\",\"type\":\"address\"}],\"name\":\"balanceOf\",\"outputs\":[{\"name\":\"balance\",\"type\":\"uint256\"}],\"type\":\"function\"}]";
        }
        
        private string GetERC721ABI()
        {
            return "[{\"constant\":false,\"inputs\":[{\"name\":\"to\",\"type\":\"address\"},{\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"mint\",\"outputs\":[],\"type\":\"function\"}]";
        }
        
        private string GetStakingABI()
        {
            return "[{\"constant\":false,\"inputs\":[{\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"stake\",\"outputs\":[],\"type\":\"function\"}]";
        }
        
        // Public getters
        public PlayerEconomyData GetPlayerData(string playerId)
        {
            return playerData.ContainsKey(playerId) ? playerData[playerId] : null;
        }
        
        public List<GameItem> GetGameItems()
        {
            return gameItems;
        }
        
        public BigInteger GetPlayerBalance(string playerId)
        {
            return playerData.ContainsKey(playerId) ? playerData[playerId].tokenBalance : BigInteger.Zero;
        }
    }
    
    // Helper classes
    public class MarketDataProvider
    {
        private Web3 web3;
        
        public MarketDataProvider(Web3 web3Instance)
        {
            web3 = web3Instance;
        }
        
        public async Task<decimal> GetTokenPrice(string tokenAddress)
        {
            // Implementation for fetching real-time token prices
            return 1.0m; // Placeholder
        }
    }
    
    public class RewardCalculator
    {
        private List<BaseGameEconomyManager.RewardTier> tiers;
        private float baseMultiplier;
        
        public RewardCalculator(List<BaseGameEconomyManager.RewardTier> rewardTiers, float multiplier)
        {
            tiers = rewardTiers;
            baseMultiplier = multiplier;
        }
        
        public BigInteger CalculateReward(BigInteger baseAmount, int playerLevel, string reason)
        {
            var tier = tiers.FindLast(t => t.minLevel <= playerLevel);
            var multiplier = tier?.multiplier ?? 1.0f;
            
            return baseAmount * (BigInteger)(multiplier * baseMultiplier);
        }
    }
}
