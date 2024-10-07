#!/bin/bash

# Define the source file and destination directory
SOURCE="/home/ubuntu/setting/appsettings.json"
DESTINATION="/home/ubuntu/ECom"

# Copy the file to the current directory
if cp "$SOURCE" "$DESTINATION"; then
    echo "File copied successfully."
else
    echo "Failed to copy file."
    exit 1
fi

# Restart the ECom service
if sudo systemctl restart ECom; then
    echo "ECom service restarted successfully."
else
    echo "Failed to restart ECom service."
    exit 1
fi
