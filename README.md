# 🏥 Hospital Management System - San Vicente Hospital

## 📋 What’s This About?
This is a web system built with **ASP.NET Core** to manage all patient, doctor, and appointment records for San Vicente Hospital.  
The goal is to get rid of manual processes and messy Excel sheets — everything is now digital, clean, and organized.

## 🎯 What You Can Do With It

### 👥 Patients
- Register and edit patients with full details.  
- Prevent duplicate ID numbers.  
- View a complete list of patients.  
- Simple and modern interface built with Bootstrap.

### 🩺 Doctors
- Add and edit doctors with their specialties.  
- Unique document ID per doctor.  
- Avoid duplicate doctors with the same name and specialty.  
- Filter list by specialty.  
- Fully responsive and professional layout.

### 📅 Medical Appointments
- Schedule appointments by selecting patient, doctor, date, and time.  
- Detect schedule conflicts automatically.  
- Appointment statuses: Pending, Confirmed, Attended, Canceled, No-Show.  
- List appointments by patient or doctor.  
- Simulate confirmation email sending.  
- View history of sent emails.  
- Change status or cancel appointments easily.

## 🛠️ Tech Stack

**Backend**
- ASP.NET Core 8.0  
- Entity Framework Core for database access  
- FluentValidation for clean validations  
- LINQ for data queries  
- MySQL as the main database

**Frontend**
- Bootstrap 5.3  
- Bootstrap Icons  
- JavaScript ES6  
- jQuery Validation for client-side checks

**Architecture**
- MVC (Model-View-Controller)  
- Repository Pattern to avoid direct SQL everywhere  
- Service Layer for business logic  
- Dependency Injection for clean object management

## 🧰 Requirements
- .NET SDK 8.0 or higher  
- MySQL 8.0 or higher  
- A modern browser (Chrome, Firefox, Edge)  
- Database with creation permissions

## 🚀 How to Run It

1. Clone the repository:
```bash
git clone https://github.com/yourusername/pruebahospital.git
cd pruebahospital
```

2. Configure the database in `appsettings.json` with your credentials:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=HospitalDB;Uid=your_user;Pwd=your_password;"
  }
}
```

3. Apply migrations:
```bash
dotnet tool install --global dotnet-ef
dotnet ef database update
```

4. Run the app:
```bash
# Development mode with hot reload
dotnet watch

# Production mode
dotnet run
```

5. Open in your browser:
```
https://localhost:7000
```
*(Port may vary — check the console for details.)*

## 📸 What It Looks Like
<!-- TITLE HERE -->
#Landing Page Preview #1
![Screenshot 1](ScreenShots/Imagen%20pegada.png)

<!-- TITLE HERE -->
#Landing Page Preview #2
![Screenshot 2](ScreenShots/Imagen%20pegada%20(2).png)

<!-- TITLE HERE -->
#Management of Pacients
![Screenshot 3](ScreenShots/Imagen%20pegada%20(3).png)

<!-- TITLE HERE -->
#Management of Doctors
![Screenshot 4](ScreenShots/Imagen%20pegada%20(4).png)

<!-- TITLE HERE -->
#Management of Appointments
![Screenshot 5](ScreenShots/Imagen%20pegada%20(5).png)

<!-- TITLE HERE -->
#Email Capture
![Screenshot 6](ScreenShots/Imagen%20pegada%20(6).png)

## 🔒 Security & Validations
- Unique document ID for patients and doctors.  
- Unique name + specialty combination for doctors.  
- Prevents overlapping appointments.  
- Validates emails, phone numbers, and age.  
- Anti-forgery tokens in all forms.  
- Clear error messages for users.

## 🐛 Common Issues
- **Database connection fails** → Check MySQL service and credentials.  
- **Pending migrations** → Run `dotnet ef migrations add InitialFix` and `dotnet ef database update`.  
- **Port already in use** → Change port or kill the process.

## 👨‍💻 Developer Info
- **Name:** Jeims Velez
- **Document Number:** 1015188992
- **Clan:** Csharp
- **Email:** jeims1221jeims@gmail.com  
- **Development Date:** October 14, 2025  

## 📄 License
This project was developed as a technical test for Module 5.3 - C# ASP.NET.
