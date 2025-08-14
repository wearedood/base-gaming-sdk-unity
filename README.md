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
    "nftContract": "0x...",
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
