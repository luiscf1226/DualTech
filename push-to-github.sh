#!/bin/bash

# Colors for output
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Check if git is installed
if ! command -v git &> /dev/null; then
    echo -e "${YELLOW}Git is not installed. Please install git first.${NC}"
    exit 1
fi

# Check if the directory is a git repository
if [ ! -d .git ]; then
    echo -e "${YELLOW}Initializing git repository...${NC}"
    git init
    
    # Copy README-GITHUB.md to README.md if it exists
    if [ -f README-GITHUB.md ]; then
        echo -e "${YELLOW}Copying README-GITHUB.md to README.md...${NC}"
        cp README-GITHUB.md README.md
    fi
    
    echo -e "${GREEN}Git repository initialized.${NC}"
fi

# Ask for GitHub repository URL if not already set
REMOTE_URL=$(git config --get remote.origin.url)
if [ -z "$REMOTE_URL" ]; then
    echo -e "${YELLOW}Please enter your GitHub repository URL (e.g., https://github.com/username/repo.git):${NC}"
    read REPO_URL
    
    if [ -z "$REPO_URL" ]; then
        echo -e "${YELLOW}No repository URL provided. Exiting.${NC}"
        exit 1
    fi
    
    git remote add origin $REPO_URL
    echo -e "${GREEN}Remote repository set to $REPO_URL${NC}"
fi

# Add all files
echo -e "${YELLOW}Adding files to git...${NC}"
git add .

# Commit changes
echo -e "${YELLOW}Please enter a commit message:${NC}"
read COMMIT_MESSAGE

if [ -z "$COMMIT_MESSAGE" ]; then
    COMMIT_MESSAGE="Initial commit"
fi

git commit -m "$COMMIT_MESSAGE"
echo -e "${GREEN}Changes committed.${NC}"

# Push to GitHub
echo -e "${YELLOW}Pushing to GitHub...${NC}"
git push -u origin master || git push -u origin main

echo -e "${GREEN}Successfully pushed to GitHub!${NC}" 