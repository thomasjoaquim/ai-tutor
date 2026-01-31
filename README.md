# AI Tutor - Life's QR Memorial Creator

A web application built with ASP.NET Core that helps users create digital memorials with AI-powered biography assistance.

## Features

- **Memorial Creation**: Create digital memorials with basic information (name, birth/death dates)
- **AI Biography Assistant**: Interactive chat powered by OpenAI to help write meaningful biographies
- **MySQL Integration**: Store memorial data in MySQL database
- **Responsive Design**: Modern UI inspired by Life's QR website

## Technologies Used

- ASP.NET Core (.NET 10)
- Razor Pages
- MySQL Database
- OpenAI API Integration
- Bootstrap 5
- JavaScript (ES6+)

## Setup Instructions

### Prerequisites
- .NET 10 SDK
- MySQL Server
- OpenAI API Key

### Installation

1. Clone the repository:
```bash
git clone https://github.com/thomasjoaquim/ai-tutor.git
cd ai-tutor
```

2. Configure the database connection in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=my_database;Uid=admin;Pwd=your_password;Port=3306;"
  }
}
```

3. Add your OpenAI API key in `appsettings.json`:
```json
{
  "OpenAI": {
    "ApiKey": "your-openai-api-key-here"
  }
}
```

4. Restore packages and run:
```bash
dotnet restore
dotnet run --urls="http://localhost:5000"
```

5. Open your browser and navigate to `http://localhost:5000`

## Usage

1. **Home Page**: View the main landing page with memorial information
2. **Create Memorial**: Click "Create Memorial" to start creating a new memorial
3. **Fill Basic Info**: Enter name, birth date, and death date
4. **Biography Section**: 
   - Write biography manually, or
   - Click "🤖 AI HELP" to get assistance from the AI chatbot
   - The AI will ask thoughtful questions to help build a meaningful biography
   - Click "Apply to Biography" to transfer the conversation to the biography field
5. **Submit**: Create the memorial

## API Endpoints

- `POST /api/chat/biography-assistance` - Get AI assistance for biography writing

## Project Structure

```
MyProject/
├── Controllers/          # API Controllers
├── Pages/               # Razor Pages
├── Services/            # Business logic services
├── wwwroot/            # Static files
├── appsettings.json    # Configuration
└── Program.cs          # Application entry point
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Submit a pull request

## License

This project is licensed under the MIT License.