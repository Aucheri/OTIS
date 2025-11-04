***Welcome to OTIS***

The Online Therapist Interactive System

<img src="https://github.com/Aucheri/AI-Wellness-Bot/blob/main/frontend/src/lib/assets/AITherapistLogo.png" />

***What is it?***

As the name suggests, it is an online therapist. It uses ChatGPT to respond and support users, recommending a myriad of online resources to ensure that everyone's mental health is being cared for in a personal way.

It is served through an API, but there is also a demo frontend in the **[Frontend](https://github.com/Aucheri/AI-Wellness-Bot/tree/main/frontend)** folder. It runs on Svelte and is mainly within a component called Chatbox.svelte in the lib folder. It assumes the API is running on 'localhost:5156/message', but is easily changeable

**What do you need to get started?**

Dotnet 9 is the main requirement, you can obtain it [here](https://dotnet.microsoft.com/en-us/download/dotnet/9.0), the project runs on it.

You also need an [API key from OpenAI](https://platform.openai.com/docs/quickstart), you will need to pay for tokens. It assumes that you're storing the API key as an Environment Variable, you can set it with the PowerShell command "setx OPENAI_API_KEY <your_api_key_here>" on Windows and the command "export OPENAI_API_KEY=<your_api_key_here>" on MacOS and Linux. 

For the frontend you need [NodeJS](https://nodejs.org/en/download/current) and [NPM](https://docs.npmjs.com/downloading-and-installing-node-js-and-npm). You can open the "api-and-frontend-startup.bat" if you are on Windows.

 If you are on MacOS or Linux, you can "dotnet run" the root and then open the frontend folder and run "npm run dev". I will work on making an sh script. 