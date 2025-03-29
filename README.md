# Setting Up and Running the RedditMemeScraper

This guide will help you set up and run the Telegram bot that fetches the top Reddit memes and generates reports using the provided credentials and connections. Follow the steps below to get your bot running.

---

## Prerequisites

Before you begin, ensure you have:

- A **Telegram** account.
- Access to **Reddit Developer** portal for API credentials.
- A **MongoDB** instance (local or cloud).
- **.NET** SDK installed (for running the bot).

---

## Step 1: Get the Client Key for the Telegram Bot

1. Open **Telegram** on your phone or desktop.
2. Search for the **BotFather** in Telegram. This is an official bot used to create and manage other Telegram bots.
3. Start a conversation with **BotFather** and type `/newbot`.
4. Follow the instructions to set up the bot:
   - Choose a name for your bot.
   - Choose a unique username for your bot (it must end with "bot", e.g., `RedditMemeReport_bot`).
5. After creating the bot, **BotFather** will give you a **Telegram Bot Token** (API token). Keep this token safe.

---

## Step 2: Get the Reddit Secret and Reddit Client ID

1. Visit the **Reddit Developer Portal**: [https://www.reddit.com/prefs/apps](https://www.reddit.com/prefs/apps).
2. If you don’t have one, create an account or sign in.
3. Scroll down to the "Developed Applications" section and click **Create App**.
4. Fill out the form:
   - **App Name**: Choose a name (e.g., `RedditMemeReport`).
   - **App Description**: A brief description of what your app will do (e.g., `Fetch and report top memes`).
   - **App Website**: Optional, but can be a placeholder (e.g., `http://localhost`).
   - **Redirect URI**: Use `http://localhost` for this step.
   - **Permissions**: Select the required permissions for reading Reddit data.
5. Once the app is created, you will be provided with:
   - **Client ID** (under the app name).
   - **Client Secret** (below the client ID).

---

## Step 3: Get the MongoDB Client Connection

1. If you already have a **MongoDB** instance set up (either locally or on a cloud service), gather the connection string.
   - For local MongoDB, the default connection string is usually: `mongodb://localhost:27017`.
   - For MongoDB Atlas (cloud version), you will get a connection string from your MongoDB Atlas dashboard.
2. This string will be used to connect your bot to the database to store the crawled Reddit posts.

---

## Step 4: Add the Credentials to the `.env` File

1. In the project folder, locate the `.env.sample` file.
2. Rename the file to `.env`.
3. Open the `.env` file and add the following values:

   - **REDDIT_CLIENT_ID**: Your Reddit Client ID (from Step 2).
   - **REDDIT_CLIENT_SECRET**: Your Reddit Client Secret (from Step 2).
   - **MONGO_CONNECTION**: Your MongoDB connection string (from Step 3).
   - **TELEGRAM_BOT_TOKEN**: Your Telegram Bot Token (from Step 1).

   Example `.env` file:

   ```env
   REDDIT_CLIENT_ID=your_reddit_client_id
   REDDIT_CLIENT_SECRET=your_reddit_client_secret
   MONGO_CONNECTION=mongodb://localhost:27017
   TELEGRAM_BOT_TOKEN=your_telegram_bot_token
   ```

---

## Step 5: Navigate to the `TelegramBot` Folder

1. Open a terminal or command prompt.
2. Navigate to the `TelegramBot` folder in your project directory.

   ```bash
   cd /path/to/TelegramBot
   ```

---

## Step 6: Run the Application

1. Once you're in the `TelegramBot` folder, run the following command to start the bot:

   ```bash
   dotnet run
   ```

2. Your Telegram bot should now be running, and it will start fetching top Reddit memes, generating reports, and interacting with users on Telegram.

---

## Final Notes

- If you encounter any issues during setup, make sure all values in the `.env` file are correct, and that MongoDB, Reddit, and Telegram Bot credentials are valid.
- Once the bot is up and running, you can interact with it on Telegram to receive reports of the top memes!

---

Feel free to reach out if you run into any issues or have questions! Enjoy your Meme Report Bot! 😄
