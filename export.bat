@echo off
chcp 65001 > nul
gh issue list --state=all --json id,title,labels,assignees,updatedAt,state --limit 1000