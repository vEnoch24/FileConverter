# FileConverter

Converts files from one format to another using CloudConvert (server-side) and a Blazor-based web UI. The project includes a Blazor Server UI (Razor pages & components), a ConverterService that uses the CloudConvert API to upload/convert/export files, and helper components for file selection and download.

## What this project is

FileConverter is a Blazor-based web app that lets users upload a file, select an output format, and convert the file using the CloudConvert service. The UI uses MudBlazor components and Syncfusion controls for the dropdowns and other UI elements. The server-side conversion flow is implemented in `Services/ConverterService.cs`.

This repository appears to be a working prototype: it has a client UI (Razor page `Pages/Index.razor`), a converter service that orchestrates CloudConvert jobs, and app configuration scaffolding.

---

## Quick links

- Main UI / Upload page: `Pages/Index.razor`
- Conversion logic: `Services/ConverterService.cs`
- App startup: `Program.cs`
- App settings: `appsettings.json` and `appsettings.Development.json`
- Project file: `FileConverter.csproj`
- Static files: `wwwroot/` (if you add any)

---

## Features

- Drag-and-drop file uploading and a file picker (via Blazor InputFile).
- Select conversion target format from a categorized dropdown (Syncfusion DropDownList).
- Convert files using CloudConvert REST API (Create job → upload → wait → export URL).
- Download converted file via generated URL returned from CloudConvert.
- UI feedback (snackbars) via MudBlazor to report success/error.
- 50 MB upload size limit enforced in the UI.

---

## Supported formats (from UI)

The Index page contains a built-in list of target formats grouped by category. Current options include (non-exhaustive):

- Documents: DOC, DOCX, PDF, HTML, MD, ODT, RST, RFT, TEX, TXT
- Presentation: ODP, PPT, PPTX
- Spreadsheet: CSV, ODS, XLS, XLSX
- Archive: 7Z, RAR, TAR, TAR.BZ2, TAR.GZ, ZIP
- Image: PNG, JPG, AVIF, BMP, EPS, GIF, ICNS, ICO, ODD, PS, PSD, TIFF, WEBP, XPS

(These are configured in `Pages/Index.razor` as the `formats` list.)

---

## Tech stack and dependencies

- .NET / C# (Blazor Server / Razor pages)
- MudBlazor (UI components)
- Syncfusion.Blazor (DropDownList and other controls)
- CloudConvert API (file conversion)
- Some JavaScript + CSS (front-end assets inside `wwwroot` if present)
- The project references Syncfusion licensing APIs and contains a call to register a Syncfusion license in `Program.cs` (see Security notes).

Files & examples:
- Server startup: `Program.cs` uses `builder.Services.AddSyncfusionBlazor()` and registers Syncfusion license with `Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(...)`.
- Converter service: `Services/ConverterService.cs` uses `CloudConvert.API` library and a `CloudConvertAPI` wrapper, assembles jobs and uploads files.

---

## Prerequisites

- .NET SDK 6.0 or 7.0 (verify with `dotnet --version`)
- Visual Studio 2022 / VS Code (with C# / Blazor support) recommended
- CloudConvert API key (required)
- Syncfusion license (if you rely on licensed Syncfusion components in production — the repo registers a license string at runtime)
- Network access to CloudConvert API endpoints

---

## Configuration

1. CloudConvert API key
   - Add CloudConvert API key to your configuration. Example `appsettings.Development.json` entry:
     ```json
     {
       "CloudConvert": {
         "ApiKey": "your-cloudconvert-api-key"
       }
     }
     ```
   - The `ConverterService` reads `_configuration["CloudConvert:ApiKey"]`.

2. Syncfusion license & ConvertApi key
   - The repo currently contains:
     - A Syncfusion license registration string in `Program.cs`.
     - A hard-coded ConvertApi key string inside `ConverterService.convert()` (ConvertApi ("hyfzEoT6NJbmgHgM")).
   - Best practice: remove hard-coded keys and use configuration or secrets (environment variables or user secrets) instead. See Security section below.

3. File size limit
   - The UI enforces a 50 MB limit (50 * 1024 * 1024 bytes). Adjust as needed in `Pages/Index.razor`.

4. CORS and endpoint URLs
   - If you separate front-end and server, ensure CORS and SignalR (if used) are properly configured.

---

## How to run locally

1. Clone the repository
   ```bash
   git clone https://github.com/vEnoch24/FileConverter.git
   cd FileConverter
