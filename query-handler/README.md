# Query Handler Function for CQRS Pattern

This folder contains the C# code for the **Query Handler Function**. This function handles data retrieval (query) operations by accessing the **Query DB**.

## 📑 Files

- **`QueryHandler.csproj`**: C# project file for the Query Handler function.
- **`QueryHandler.cs`**: Main code for the Query Handler function.
- **`deploy-query-handler.yml`**: GitHub Action workflow to automate the deployment of the Query Handler function.

## 🚀 How to Deploy the Query Handler Function

### Prerequisites

1. **Azure Subscription**: You need an active Azure account.
2. **Azure Function App**: Ensure the Azure Function App is created (using the Bicep template in the `/infrastructure` folder).
3. **GitHub Secrets Configuration**:
   - **`AZURE_FUNCTIONAPP_PUBLISH_PROFILE_QUERY_HANDLER`**: Publish profile for the Azure Function App.

### Steps to Deploy

1. **Add Required Secrets to GitHub**:
   - Go to your repository's **Settings > Secrets and variables > Actions > New repository secret**.
   - Add the following secret:
     - **`AZURE_FUNCTIONAPP_PUBLISH_PROFILE_QUERY_HANDLER`**: The publish profile for your Azure Function App.

2. **Run the GitHub Action**:
   - Push your changes to the `main` branch or manually trigger the **Deploy Query Handler Function** workflow from the **Actions** tab.

3. **Monitor the Deployment**:
   - Go to the **Actions** tab in your GitHub repository.
   - Select the **Deploy Query Handler Function** workflow to monitor the deployment progress.

### 📝 How It Works

- The **Query Handler Function** listens for HTTP GET requests.
- It retrieves the requested data from the **Query DB** and returns the result.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](../LICENSE) file for details.
