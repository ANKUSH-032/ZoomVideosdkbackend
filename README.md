
📹 Zoom Video SDK Integration (Angular + .NET Core 8)

This project demonstrates how to implement **Zoom Video SDK** using **Zoom UI Toolkit** on the frontend (Angular) and **.NET Core 8 Web API** on the backend.

📦 Prerequisites

* Node.js ≥ 18.x
* Angular CLI ≥ 17.x
* .NET SDK ≥ 8.0
* Zoom Video SDK credentials (SDK Key & Secret)
* Zoom Video SDK UI Toolkit (npm module)
* A Zoom developer account
  
### 🧑‍💻 Backend Setup (.NET Core 8)

1. **Clone the project**

   ```bash
   git clone https://github.com/yourusername/zoom-video-sdk-integration.git
   cd zoom-video-sdk-integration/backend
   ```

2. **Configure appsettings.json**
   Set your Zoom SDK credentials in `appsettings.json`:

   ```json
   "ZoomConfig": {
     "SdkKey": "your_sdk_key_here",
     "SdkSecret": "your_sdk_secret_here"
   }
   ```

3. **Run the API**

   ```bash
   dotnet build
   dotnet run
   ```

   The API should now run at `https://localhost:5001` or your configured port.

---

### 🌐 Frontend Setup (Angular)

1. **Navigate to frontend folder**

   ```bash
   cd ../frontend
   ```

2. **Install dependencies**

   ```bash
   npm install
   ```

3. **Install Zoom Video SDK UI Toolkit**

   ```bash
   npm install @zoom/videosdk-ui-toolkit --save
   ```

4. **Update `environment.ts` with API base URL**

   ```ts
   export const environment = {
     apiBaseUrl: 'https://localhost:5001/api'
   };
   ```

5. **Run Angular project**

   ```bash
   ng serve
   ```

   Open `http://localhost:4200` in your browser.

---

### ✅ Usage Steps

1. Click the **Start Zoom Call** button.
2. The system calls your backend to generate a JWT token using SDK Key/Secret.
3. The Angular app joins the Zoom session using UI Toolkit.
4. Video and audio capabilities are managed by the Zoom UI.

---

### 📁 Folder Structure

```
zoom-video-sdk-integration/
│
├── backend/             # ASP.NET Core 8 Web API
│   └── Controllers/
│   └── Services/
│   └── appsettings.json
│
├── frontend/            # Angular 17+ UI
│   └── src/app/
│   └── angular.json
```

---

### 📌 Notes

* Make sure your backend uses HTTPS (required by Zoom SDK).
* Use only **Zoom Video SDK credentials**, not OAuth or JWT credentials for Zoom Meeting SDK.
* For production, ensure proper CORS setup on the backend.

---

### 🔗 Resources

* [Zoom Video SDK Docs](https://marketplace.zoom.us/docs/sdk/video/web/)
* [Zoom UI Toolkit](https://www.npmjs.com/package/@zoom/videosdk-ui-toolkit)
* [Angular Docs](https://angular.io/)
* [ASP.NET Core Docs](https://learn.microsoft.com/en-us/aspnet/core/)


