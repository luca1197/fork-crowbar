# Crowbar
 Crowbar - GoldSource and Source Engine Modding Tool
 
 [Website](https://steamcommunity.com/groups/CrowbarTool)
 
 IMPORTANT: When providing link to Crowbar, please only use the above website link, because that page provides info and links to everything related to Crowbar.

# Building

## GitHub Actions (Automated)
This project includes automated builds via GitHub Actions. On every push, the workflow builds the solution in Release x86 configuration and produces downloadable artifacts.

[![Build Crowbar](../../actions/workflows/build.yml/badge.svg)](../../actions/workflows/build.yml)

To download a build:
1. Go to the **Actions** tab in GitHub
2. Click on the latest successful workflow run
3. Download the **Crowbar-Release-x86** artifact

## Manual Build (Visual Studio)
I currently build via Visual Basic in Visual Studio Community 2017.
I use Debug x86 when debugging and Release x86 when releasing to public.

I tested building in Visual Studio Community 2019 on 15-Apr-2021. All I had to change were the settings from Debug Any CPU and Release Any CPU to Debug x86 and Release x86 at the top in the default toolbars.
