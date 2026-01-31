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

## Docker Setup

### Prerequisites
- Docker
- Docker Compose

### Quick Start with Docker

1. Clone the repository:
```bash
git clone https://github.com/thomasjoaquim/ai-tutor.git
cd ai-tutor
```

2. Configure your OpenAI API key in `.env.docker`:
```bash
cp .env.docker.example .env.docker
# Edit .env.docker with your real API key
OPENAI_API_KEY=your-openai-api-key-here
```

3. Start the application:
```bash
./docker.sh up
```

4. Access the application at `http://localhost:5000`

### Docker Commands
```bash
./docker.sh build  # Build Docker images
./docker.sh up     # Start application
./docker.sh down   # Stop application
./docker.sh logs   # View logs
./docker.sh mysql  # Connect to MySQL
./docker.sh clean  # Clean up resources
```

## Manual Setup

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

3. Create a `.env` file based on `.env.example`:
```bash
cp .env.example .env
```

4. Configure your environment variables in `.env`:
```env
# Database Configuration
DB_SERVER=localhost
DB_DATABASE=my_database
DB_USER=admin
DB_PASSWORD=your_password_here
DB_PORT=3306

# OpenAI Configuration
OPENAI_API_KEY=your-openai-api-key-here
```

5. Restore packages and run:
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