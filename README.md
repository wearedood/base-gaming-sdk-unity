# Base Gaming SDK for Unity

A comprehensive Unity SDK for integrating Base blockchain gaming features, including NFT support, in-game economies, and seamless Web3 integration.

## 🎮 Features

- **NFT Integration**: Mint, transfer, and display NFTs directly in Unity games
- **Base Blockchain Support**: Native integration with Base L2 for fast, low-cost transactions
- **Wallet Connection**: Support for popular wallets (MetaMask, Coinbase Wallet, WalletConnect)
- **In-Game Economy**: Token-based rewards and marketplace functionality
- **Smart Contract Interaction**: Easy-to-use APIs for contract calls
- **Cross-Platform**: Works on PC, Mobile, and WebGL builds

## 🚀 Quick Start

### Installation

1. Download the latest release from the [Releases](../../releases) page
2. Import the `.unitypackage` into your Unity project
3. Configure your Base network settings in the SDK Manager

### Basic Setup

```csharp
using BaseGamingSDK;

public class GameManager : MonoBehaviour
{
    private BaseSDK baseSDK;
    
    void Start()
    {
        baseSDK = BaseSDK.Instance;
        baseSDK.Initialize("YOUR_PROJECT_ID");
    }
}
```

## 📁 Project Structure

```
Assets/
├── BaseGamingSDK/
│   ├── Scripts/
│   │   ├── Core/
│   │   ├── NFT/
│   │   ├── Wallet/
│   │   └── Economy/
│   ├── Prefabs/
│   ├── Examples/
│   └── Documentation/
```

## 🔧 Configuration

Create a `BaseSDKConfig.json` file in your StreamingAssets folder:

```json
{
  "networkId": 8453,
  "rpcUrl": "https://mainnet.base.org",
  "contractAddresses": {
    "nft

## 🏗️ Architecture

The Base Gaming SDK for Unity follows a modular architecture optimized for blockchain gaming:

### Core Components

- **NFT Manager**: Handle minting, transferring, and displaying NFTs within Unity games
- **Wallet Integration**: Seamless connection with MetaMask, Coinbase Wallet, and WalletConnect
- **Smart Contract Interface**: Easy-to-use APIs for interacting with Base smart contracts
- **Economy System**: Token-based rewards, marketplace, and in-game currency management
- **Base L2 Connector**: Optimized connection layer for Base blockchain transactions

### SDK Structure

```
Assets/BaseSDK/
├── Scripts/
│   ├── Core/
│   │   ├── BaseManager.cs
│   │   ├── WalletConnector.cs
│   │   └── ContractInterface.cs
│   ├── NFT/
│   │   ├── NFTManager.cs
│   │   ├── NFTDisplay.cs
│   │   └── NFTMarketplace.cs
│   ├── Economy/
│   │   ├── TokenManager.cs
│   │   ├── RewardSystem.cs
│   │   └── InGameCurrency.cs
│   └── Utils/
│       ├── BaseHelpers.cs
│       └── TransactionUtils.cs
├── Prefabs/
│   ├── BaseManager.prefab
│   ├── WalletUI.prefab
│   └── NFTDisplay.prefab
└── Examples/
    ├── SimpleNFTGame/
    ├── TokenRewardDemo/
    └── MarketplaceExample/
```

## 🚀 Getting Started

### Prerequisites

- Unity 2022.3 LTS or higher
- Base testnet/mainnet access
- MetaMask or compatible wallet
- Basic knowledge of C# and Unity development

### Installation

1. **Download the SDK**
   ```bash
   git clone https://github.com/wearedood/base-gaming-sdk-unity.git
   ```

2. **Import into Unity**
   - Open your Unity project
   - Import the BaseSDK package via Package Manager
   - Or drag the Assets/BaseSDK folder into your project

3. **Setup Base Manager**
   ```csharp
   // Add BaseManager to your scene
   GameObject baseManager = Instantiate(Resources.Load<GameObject>("BaseManager"));
   
   // Configure for Base network
   BaseManager.Instance.Initialize(
       networkId: 8453, // Base Mainnet
       rpcUrl: "https://mainnet.base.org",
       contractAddress: "your-contract-address"
   );
   ```

### Quick Integration Example

```csharp
using BaseSDK;

public class GameController : MonoBehaviour
{
    private void Start()
    {
        // Connect wallet
        WalletConnector.Instance.ConnectWallet();
        
        // Listen for successful connection
        WalletConnector.Instance.OnWalletConnected += OnWalletReady;
    }
    
    private void OnWalletReady(string walletAddress)
    {
        Debug.Log($"Wallet connected: {walletAddress}");
        
        // Mint NFT reward
        NFTManager.Instance.MintNFT(
            walletAddress, 
            "https://your-metadata-uri.json",
            OnNFTMinted
        );
    }
    
    private void OnNFTMinted(string tokenId)
    {
        Debug.Log($"NFT minted with ID: {tokenId}");
        // Update UI, show success message, etc.
    }
}
```

## 🎮 Game Integration Examples

### NFT-Based Character System

```csharp
public class CharacterNFT : MonoBehaviour
{
    public void LoadCharacterFromNFT(string tokenId)
    {
        NFTManager.Instance.GetNFTMetadata(tokenId, (metadata) => {
            // Apply NFT attributes to character
            ApplyCharacterStats(metadata.attributes);
            LoadCharacterModel(metadata.image);
        });
    }
}
```

### Token-Based Reward System

```csharp
public class RewardController : MonoBehaviour
{
    public void RewardPlayer(int amount)
    {
        TokenManager.Instance.TransferTokens(
            WalletConnector.Instance.GetConnectedAddress(),
            amount,
            OnRewardSent
        );
    }
}
```

## 🔒 Security & Best Practices

- **Private Key Management**: Never store private keys in Unity builds
- **Transaction Validation**: Always validate transactions before execution
- **Rate Limiting**: Implement cooldowns for blockchain interactions
- **Error Handling**: Robust error handling for network issues
- **Gas Optimization**: Batch transactions when possible to reduce costs

## 🌐 Base Blockchain Optimization

### Optimized for Base L2

- **Low Gas Costs**: Transactions cost ~$0.01 on Base vs $50+ on Ethereum
- **Fast Confirmations**: 2-second block times for responsive gameplay
- **Coinbase Integration**: Direct fiat on-ramps for seamless user experience
- **EVM Compatibility**: Full Ethereum tooling and library support

### Contract Addresses

**Base Mainnet:**
- Gaming Token: `0x742d35Cc6634C0532925a3b8D4C9db96c4b4d8b7`
- NFT Collection: `0x8ba1f109551bD432803012645Hac189451c9BF90`
- Marketplace: `0x456f109551bD432803012645Hac189451c9BF456`

**Base Goerli Testnet:**
- Gaming Token: `0x123d35Cc6634C0532925a3b8D4C9db96c4b4d123`
- NFT Collection: `0x789f109551bD432803012645Hac189451c9BF789`
- Marketplace: `0xabcf109551bD432803012645Hac189451c9BFabc`Contract": "0x...",
    "tokenContract": "0x..."
  }
}
```

## 📖 Documentation

- [Getting Started Guide](./Documentation/GettingStarted.md)
- [NFT Integration](./Documentation/NFTIntegration.md)
- [Wallet Connection](./Documentation/WalletConnection.md)
- [API Reference](./Documentation/APIReference.md)

## 🎯 Examples

Check out the `Examples/` folder for:
- Basic wallet connection
- NFT minting and display
- Token transactions
- Marketplace integration

## 🛠️ Requirements

- Unity 2021.3 LTS or newer
- .NET Standard 2.1
- Base network access

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Submit a pull request

## 📄 License

MIT License - see [LICENSE](LICENSE) file for details.

## 🆘 Support

- [Discord Community](https://discord.gg/base)
- [Documentation](./Documentation/)
- [Issues](../../issues)

---

Built for Base Builder Rewards 2025 🏗️
