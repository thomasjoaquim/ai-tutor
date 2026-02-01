#!/bin/bash

# Life's QR Docker Management Script

case "$1" in
    "build")
        echo "🔨 Building Docker images..."
        docker-compose build
        ;;
    "up")
        echo "🚀 Starting Life's QR application..."
        docker-compose --env-file .env.docker up -d
        echo "✅ Application running at http://localhost:5000"
        ;;
    "down")
        echo "🛑 Stopping Life's QR application..."
        docker-compose down
        ;;
    "logs")
        echo "📋 Showing application logs..."
        docker-compose logs -f web
        ;;
    "mysql")
        echo "🗄️ Connecting to MySQL..."
        docker exec -it lifesqr_mysql mysql -u admin -pP@zzword1 my_database
        ;;
    "users")
        echo "👥 Showing all users..."
        docker exec lifesqr_mysql mysql -u admin -pP@zzword1 my_database -e "SELECT Id, Email, FirstName, LastName, CreatedAt, IsActive FROM Users;"
        ;;
    "memorials")
        echo "📝 Showing all memorials..."
        docker exec lifesqr_mysql mysql -u admin -pP@zzword1 my_database -e "SELECT m.Id, m.Name, m.BirthDate, m.DeathDate, u.Email as Owner, m.CreatedAt FROM Memorials m JOIN Users u ON m.UserId = u.Id;"
        ;;
    "tables")
        echo "📊 Showing database structure..."
        docker exec lifesqr_mysql mysql -u admin -pP@zzword1 my_database -e "SHOW TABLES;"
        ;;
    "clean")
        echo "🧹 Cleaning up Docker resources..."
        docker-compose down -v
        docker system prune -f
        ;;
    *)
        echo "Life's QR Docker Commands:"
        echo "  ./docker.sh build     - Build Docker images"
        echo "  ./docker.sh up        - Start application"
        echo "  ./docker.sh down      - Stop application"
        echo "  ./docker.sh logs      - View logs"
        echo "  ./docker.sh mysql     - Connect to MySQL"
        echo "  ./docker.sh users     - Show all users"
        echo "  ./docker.sh memorials - Show all memorials"
        echo "  ./docker.sh tables    - Show database tables"
        echo "  ./docker.sh clean     - Clean up resources"
        ;;
esac