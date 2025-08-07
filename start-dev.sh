#!/bin/bash

# Function to kill processes on specific ports
kill_port() {
    local port=$1
    lsof -ti :$port | xargs kill -9 2>/dev/null
}

# Function to check if a port is in use
check_port() {
    local port=$1
    if lsof -i :$port > /dev/null; then
        return 0
    else
        return 1
    fi
}

# Function to wait for a port to be available
wait_for_port() {
    local port=$1
    local timeout=$2
    local counter=0
    while ! check_port $port; do
        sleep 1
        counter=$((counter + 1))
        if [ $counter -ge $timeout ]; then
            echo "Error: Port $port is not available after $timeout seconds"
            return 1
        fi
    done
    return 0
}

# Kill any existing processes on our ports
echo "Checking for existing processes..."
kill_port 5005  # API port
kill_port 4200  # Frontend port

# Start the API in the background
echo "Starting LifeyLife API..."
cd LifeyLife.Api
ASPNETCORE_ENVIRONMENT=Development dotnet run &
API_PID=$!

# Wait for API to start
echo "Waiting for API to start..."
if ! wait_for_port 5005 30; then
    echo "Failed to start API"
    kill $API_PID 2>/dev/null
    exit 1
fi

# Start the frontend
echo "Starting LifeyLife Frontend..."
cd ../LifeyLife.App/ClientApp
npm run start:dev &
FRONTEND_PID=$!

# Wait for frontend to start
echo "Waiting for frontend to start..."
if ! wait_for_port 4200 30; then
    echo "Failed to start frontend"
    kill $API_PID $FRONTEND_PID 2>/dev/null
    exit 1
fi

# Handle script termination
trap "kill $API_PID $FRONTEND_PID" EXIT

# Wait for both processes
wait