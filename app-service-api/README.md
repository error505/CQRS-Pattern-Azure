# App Service API for CQRS Pattern

This folder contains the C# Web API code for the **App Service API** used in the CQRS Pattern. The API exposes endpoints for both command (insert) and query (retrieve) operations.

## 📑 Files

- **`Program.cs`**: Main entry point for the API.
- **`appsettings.json`**: Configuration file for the API.
- **`CommandController.cs`**: Handles data insert operations.
- **`QueryController.cs`**: Handles data retrieval operations.
- **`deploy-api.yml`**: GitHub Action workflow to automate the deployment of the App Service API.

## 🚀 How to Deploy the API

### Prerequisites

1. **Azure Subscription**: You need an active Azure account.
2. **Azure Web App**: Ensure the Azure Web App is created (using the Bicep template in the `/infrastructure` folder).
3. **GitHub Secrets Configuration**:
   - **`AZURE_WEBAPP_PUBLISH_PROFILE`**: Publish profile for the Azure Web App.

### Steps to Deploy

1. **Add Required Secrets to GitHub**:
   - Go to your repository's **Settings > Secrets and variables > Actions > New repository secret**.
   - Add the following secret:
     - **`AZURE_WEBAPP_PUBLISH_PROFILE`**: The publish profile for your Azure Web App.

2. **Run the GitHub Action**:
   - Push your changes to the `main` branch or manually trigger the **Deploy App Service API** workflow from the **Actions** tab.

3. **Monitor the Deployment**:
   - Go to the **Actions** tab in your GitHub repository.
   - Select the **Deploy App Service API** workflow to monitor the deployment progress.

### 📝 How to Use the API

1. **Insert Data**:
   - Send a POST request to `/api/command/insert` with a JSON body containing the data to insert.
   - Example:
     ```json
     {
       "data": "Sample data to insert"
     }
     ```

2. **Query Data**:
   - Send a GET request to `/api/query/get?id={your_id}` to retrieve data by ID.

### 💡 How It Works

- The **CommandController** sends data to the **Command Handler Function** for insertion, which then publishes an event to **Azure Event Hubs**.
- The **QueryController** sends a query request to the **Query Handler Function**, which retrieves data from the **Query DB**.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](../LICENSE) file for details.
