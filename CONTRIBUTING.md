# Contributing to Base Gaming SDK for Unity

Thank you for your interest in contributing to the Base Gaming SDK for Unity! This document provides guidelines and information for contributors.

## 🚀 Getting Started

### Prerequisites

- Unity 2021.3 LTS or later
- Git and GitHub account
- Basic knowledge of C# and Unity development
- Understanding of blockchain and Base network concepts
- Node.js and npm (for testing tools)

### Development Environment Setup

1. **Fork the Repository**
   ```bash
   git clone https://github.com/YOUR_USERNAME/base-gaming-sdk-unity.git
   cd base-gaming-sdk-unity
   ```

2. **Install Dependencies**
   ```bash
   npm install
   ```

3. **Configure Unity Project**
   - Open Unity Hub
   - Add the project folder
   - Ensure Unity 2021.3 LTS is installed

4. **Set Up Base Network**
   - Create a Base testnet account
   - Obtain test ETH from Base faucet
   - Configure API keys in `Assets/BaseSDK/Config`

## 📋 Contribution Guidelines

### Code Style

- Follow C# coding conventions
- Use meaningful variable and method names
- Add XML documentation for public APIs
- Maintain consistent indentation (4 spaces)
- Keep line length under 120 characters

### Commit Message Format

Use conventional commit format:
```
type(scope): description

[optional body]

[optional footer]
```

**Types:**
- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation changes
- `style`: Code style changes
- `refactor`: Code refactoring
- `test`: Adding or updating tests
- `chore`: Maintenance tasks

**Examples:**
```
feat(nft): add batch minting functionality
fix(wallet): resolve connection timeout issue
docs(readme): update installation instructions
```

### Pull Request Process

1. **Create Feature Branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```

2. **Make Changes**
   - Write clean, well-documented code
   - Add unit tests for new functionality
   - Update documentation as needed

3. **Test Your Changes**
   ```bash
   # Run unit tests
   npm run test
   
   # Run integration tests
   npm run test:integration
   
   # Test in Unity
   # Open Unity Test Runner and run all tests
   ```

4. **Submit Pull Request**
   - Push your branch to your fork
   - Create a pull request with clear description
   - Link any related issues
   - Request review from maintainers

## 🧪 Testing

### Unit Tests

Write unit tests for all new functionality:

```csharp
[Test]
public async Task TestNFTMinting()
{
    var nftManager = new NFTManager();
    var result = await nftManager.MintAsync(testMetadata);
    Assert.IsNotNull(result.TokenId);
}
```

### Integration Tests

Test with Base testnet:

```csharp
[Test]
public async Task TestWalletConnection()
{
    BaseSDK.Initialize(testConfig);
    var connected = await BaseSDK.Wallet.ConnectAsync();
    Assert.IsTrue(connected);
}
```

### Manual Testing

- Test all features in Unity editor
- Verify mobile compatibility
- Test with different wallet providers
- Validate gas optimization

## 📚 Documentation

### Code Documentation

- Add XML documentation for all public APIs
- Include usage examples in documentation
- Document complex algorithms and business logic

```csharp
/// <summary>
/// Mints a new NFT with the specified metadata
/// </summary>
/// <param name="metadata">The NFT metadata</param>
/// <returns>The minting result with token ID</returns>
public async Task<MintResult> MintAsync(NFTMetadata metadata)
```

### README Updates

- Update README.md for new features
- Add code examples for new functionality
- Update installation instructions if needed

## 🐛 Bug Reports

### Before Reporting

- Check existing issues for duplicates
- Test with latest version
- Verify the issue on Base testnet

### Bug Report Template

```markdown
**Describe the bug**
A clear description of what the bug is.

**To Reproduce**
Steps to reproduce the behavior:
1. Go to '...'
2. Click on '....'
3. See error

**Expected behavior**
What you expected to happen.

**Environment:**
- Unity Version: [e.g. 2021.3.15f1]
- Base SDK Version: [e.g. 1.0.0]
- Platform: [e.g. Windows, macOS, iOS, Android]
- Base Network: [Mainnet/Testnet]

**Additional context**
Any other context about the problem.
```

## 💡 Feature Requests

### Feature Request Template

```markdown
**Is your feature request related to a problem?**
A clear description of what the problem is.

**Describe the solution you'd like**
A clear description of what you want to happen.

**Describe alternatives you've considered**
Alternative solutions or features you've considered.

**Additional context**
Any other context or screenshots about the feature request.
```

## 🏗️ Architecture Guidelines

### Code Organization

```
Assets/
├── BaseSDK/
│   ├── Core/           # Core SDK functionality
│   ├── NFT/            # NFT-related features
│   ├── Wallet/         # Wallet integration
│   ├── Contracts/      # Smart contract interfaces
│   ├── Utils/          # Utility functions
│   └── Examples/       # Usage examples
```

### Design Principles

- **Modularity**: Keep components loosely coupled
- **Extensibility**: Design for easy feature additions
- **Performance**: Optimize for mobile devices
- **Security**: Follow blockchain security best practices
- **Usability**: Provide intuitive APIs for developers

## 🔒 Security

### Security Guidelines

- Never commit private keys or sensitive data
- Use secure random number generation
- Validate all user inputs
- Implement proper error handling
- Follow OWASP guidelines for web3 applications

### Reporting Security Issues

For security vulnerabilities, please email security@basegamingsdk.com instead of creating public issues.

## 📞 Community

### Communication Channels

- **Discord**: [Base Gaming SDK Community](https://discord.gg/basegaming)
- **GitHub Discussions**: For feature discussions
- **GitHub Issues**: For bug reports and feature requests
- **Email**: support@basegamingsdk.com

### Code of Conduct

We are committed to providing a welcoming and inclusive environment. Please read our [Code of Conduct](CODE_OF_CONDUCT.md).

## 🎯 Contribution Areas

### High Priority

- Mobile platform optimization
- Gas optimization improvements
- Additional wallet integrations
- Performance benchmarking
- Documentation improvements

### Medium Priority

- Multi-chain support
- Advanced analytics
- Developer tools
- Example projects

### Low Priority

- UI/UX improvements
- Additional language bindings
- Third-party integrations

## 🏆 Recognition

Contributors will be recognized in:

- README.md contributors section
- Release notes
- Discord community highlights
- Annual contributor awards

## 📄 License

By contributing, you agree that your contributions will be licensed under the MIT License.

---

**Thank you for contributing to the Base Gaming SDK for Unity! Together, we're building the future of blockchain gaming.** 🎮⛓️
