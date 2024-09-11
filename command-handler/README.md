# Command Handler Function for CQRS Pattern

This folder contains the C# code for the **Command Handler Function**. This function handles the insertion of data by publishing events to **Azure Event Hubs**.

## 📑 Files

- **`CommandHandler.csproj`**: C# project file for the Command Handler function.
- **`CommandHandler.cs`**: Main code for the Command Handler function.
- **`deploy-command-handler.yml`**: GitHub Action workflow to automate the deployment of the Command Handler function.

## 🚀 How to Deploy the Command Handler Function

### Prerequisites

1. **Azure Subscription**: You need an active Azure account.
2. **Azure Function App**: Ensure the Azure Function App is created (using the Bicep template in the `/infrastructure` folder).
3. **GitHub Secrets Configuration**:
   - **`AZURE_FUNCTIONAPP_PUBLISH_PROFILE_COMMAND_HANDLER`**: Publish profile for the Azure Function App.

### Steps to Deploy

1. **Add Required Secrets to GitHub**:
   - Go to your repository's **Settings > Secrets and variables > Actions > New repository secret**.
   - Add the following secret:
     - **`AZURE_FUNCTIONAPP_PUBLISH_PROFILE_COMMAND_HANDLER`**: The publish profile for your Azure Function App.

2. **Run the GitHub Action**:
   - Push your changes to the `main` branch or manually trigger the **Deploy Command Handler Function** workflow from the **Actions** tab.

3. **Monitor the Deployment**:
   - Go to the **Actions** tab in your GitHub repository.
   - Select the **Deploy Command Handler Function** workflow to monitor the deployment progress.

### 📝 How It Works

- The **Command Handler Function** listens for HTTP POST requests.
- Upon receiving a request, it validates the data, then publishes the event to **Azure Event Hubs**.
- The **Update Processor Function** will process the event and update the **Command DB**.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](../LICENSE) file for details.
