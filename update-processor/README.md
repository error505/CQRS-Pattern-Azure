# Update Processor Function for CQRS Pattern

This folder contains the C# code for the **Update Processor Function**. This function listens to events from **Azure Event Hubs** and updates the **Command DB**.

## 📑 Files

- **`UpdateProcessor.csproj`**: C# project file for the Update Processor function.
- **`UpdateProcessor.cs`**: Main code for the Update Processor function.
- **`deploy-update-processor.yml`**: GitHub Action workflow to automate the deployment of the Update Processor function.

## 🚀 How to Deploy the Update Processor Function

### Prerequisites

1. **Azure Subscription**: You need an active Azure account.
2. **Azure Function App**: Ensure the Azure Function App is created (using the Bicep template in the `/infrastructure` folder).
3. **GitHub Secrets Configuration**:
   - **`AZURE_FUNCTIONAPP_PUBLISH_PROFILE_UPDATE_PROCESSOR`**: Publish profile for the Azure Function App.

### Steps to Deploy

1. **Add Required Secrets to GitHub**:
   - Go to your repository's **Settings > Secrets and variables > Actions > New repository secret**.
   - Add the following secret:
     - **`AZURE_FUNCTIONAPP_PUBLISH_PROFILE_UPDATE_PROCESSOR`**: The publish profile for your Azure Function App.

2. **Run the GitHub Action**:
   - Push your changes to the `main` branch or manually trigger the **Deploy Update Processor Function** workflow from the **Actions** tab.

3. **Monitor the Deployment**:
   - Go to the **Actions** tab in your GitHub repository.
   - Select the **Deploy Update Processor Function** workflow to monitor the deployment progress.

### 📝 How It Works

- The **Update Processor Function** listens to **Azure Event Hubs** for events.
- It processes each event and updates the **Command DB** accordingly.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](../LICENSE) file for details.
