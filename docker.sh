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
        docker exec -it lifesqr_mysql mysql -u admin -p my_database
        ;;
    "clean")
        echo "🧹 Cleaning up Docker resources..."
        docker-compose down -v
        docker system prune -f
        ;;
    *)
        echo "Life's QR Docker Commands:"
        echo "  ./docker.sh build  - Build Docker images"
        echo "  ./docker.sh up     - Start application"
        echo "  ./docker.sh down   - Stop application"
        echo "  ./docker.sh logs   - View logs"
        echo "  ./docker.sh mysql  - Connect to MySQL"
        echo "  ./docker.sh clean  - Clean up resources"
        ;;
esac