# StockTracker Azure Function App

This **Azure Function App** is built using **C#** to provide stock market data via API endpoints. It integrates with **MarketStack** and **Alpha Vantage**, fetching stock details, top performers, and worst performers daily. The backend is deployed using **Azure DevOps**, **Azure Function Apps**, and **CI/CD pipelines** for automated builds and deployments.

<!--
## Live Project Display
	
To view this project live, please visit the link: https://purple-ocean-0e575860f.6.azurestaticapps.net/
-->


## 🚀 Technologies and API's Used

- **C# Azure Functions** – Backend logic implemented in C#.
- **Azure Function Apps** – Cloud-based execution environment for running serverless functions.
- **Azure DevOps** – Used for repository management and automated CI/CD deployments.
- **MarketStack API** – Retrieves stock details when users search for a stock symbol.
- **Alpha Vantage API** – Fetches top-performing and worst-performing stocks daily.

## 🔗 API Integration

This Azure Function App connects with two external stock market APIs:

1. **MarketStack API** – Provides stock details based on a searched symbol.
   - **API Endpoint:** `http://api.marketstack.com/v2/eod?access_key=YOUR_ACCESS_KEY&symbols=`
   - To obtain an API key, sign up at [MarketStack](https://marketstack.com/) and generate an access key.

2. **Alpha Vantage API** – Returns the top 10 best and worst-performing stocks daily.
   - **API Endpoint:** `https://www.alphavantage.co/query?function=TOP_GAINERS_LOSERS&apikey=YOUR_API_KEY`
   - To obtain an API key, sign up at [Alpha Vantage](https://www.alphavantage.co/) and generate an API key.

## 🌐 API Endpoints

The Azure Function App exposes three API endpoints:

1. **Stock Details Lookup**  
   - **URL:** `https://vpi-stocktracker.azurewebsites.net/api/StockDetails`
   - **Purpose:** Calls the MarketStack API to retrieve stock details.

2. **Top Performing Stocks**  
   - **URL:** `https://vpi-stocktracker.azurewebsites.net/api/BestPerformingStocks`
   - **Purpose:** Calls the Alpha Vantage API to fetch the top 10 best-performing stocks.

3. **Worst Performing Stocks**  
   - **URL:** `https://vpi-stocktracker.azurewebsites.net/api/WorstPerformingStocks`
   - **Purpose:** Calls the Alpha Vantage API to fetch the top 10 worst-performing stocks.

## 💻 Running the Azure Function Locally

### 1️⃣ Clone the Repository
```
git clone <repository-url>
cd stockTracker
```
### 2️⃣ Install Dependencies
```
Ensure you have the Azure Functions Core Tools installed.

npm install -g azure-functions-core-tools
```
### 3️⃣ Set Up Environment Variables
Create a local.settings.json file in the root directory and add:
```
json
{
  "IsEncrypted": false,
  "Values": {
	"AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "MarketStackStockDetailsApi": "http://api.marketstack.com/v2/eod?access_key=YOUR_ACCESS_KEY&symbols=",
    "AlphavantageStockPerformanceDetailsApi": "https://www.alphavantage.co/query?function=TOP_GAINERS_LOSERS&apikey=YOUR_API_KEY",
	"TopGainersRootElement": "top_gainers",
    "TopLosersRootElement": "top_losers"
  }
}
```
Replace YOUR_ACCESS_KEY and YOUR_API_KEY with valid API keys.
### 4️⃣ Start the Azure Function Locally
```
func start
Once running, test API routes via http://localhost:7071/api/{endpoint}.
```

## 📦 Deployment Process & CI/CD Integration

### Deployment Steps
1. **Code Development** – Azure Functions were written in C#.
2. **Push to Azure DevOps Repository** – Code was committed and pushed to an Azure DevOps repository (StockTracker).
3. **Create Azure Function App** – A function app was created in Azure Portal and linked to the repository.
4. **Setup CI/CD Pipelines** – Azure DevOps pipelines handle automated builds and deployments.

## Original App Preparation Documentation
```
STOCK TRACKER APP
- Endpoints
1. Endpoint to search and retrieve information for a given stock - getStockDetails
2. Endpoint to return top 10 best performing stocks of the day (by percentage increase) - getBestPerformingDailyStocks
3. Endpoint to return top 10 worst performing stocks of the day (by percentage decrease) - getWorstPerformingDailyStocks

- Endpoint planning 
1. getStockDetails
	- Accept stock ticker or company name
	- Get user to handle search error by displaying (stock information not found, check spelling error, etc...)
	- Stock information to return:
		1. Current stock price
		2. Daily high and low
		3. Open price and close price if available.
		4. Daily volume
		5. Stock Name

2. getBestPerformingDailyStocks
	- Displays the top 10 best performing stocks of the day
	- stock information displayed:
		1. The price of each stock
		2. The percentage increase of the stock from the open to current price or (close price if stock market is closed)

3. getWorstPerformingDailyStocks
	- Displays the top 10 worst performing stocks of the day
	- stock information displayed:
		1. The price of each stock
		2. The percentage decrease of the stock from the open to current price or (close price if stock market is closed)
```
